using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class LayerGridGraph : GridGraph, IUpdatableGraph
	{
		[JsonMember]
		internal int layerCount;

		[JsonMember]
		public float mergeSpanRange = 0.5f;

		[JsonMember]
		public float characterHeight = 0.4f;

		internal int lastScannedWidth;

		internal int lastScannedDepth;

		public new LevelGridNode[] nodes;

		public override bool uniformWidthDepthGrid
		{
			get
			{
				return false;
			}
		}

		public override int LayerCount
		{
			get
			{
				return layerCount;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			RemoveGridGraphFromStatic();
		}

		private void RemoveGridGraphFromStatic()
		{
			LevelGridNode.SetGridGraph(active.data.GetGraphIndex(this), null);
		}

		public override int CountNodes()
		{
			if (nodes == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < nodes.Length; i++)
			{
				if (nodes[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		public override void GetNodes(Action<GraphNode> action)
		{
			if (nodes == null)
			{
				return;
			}
			for (int i = 0; i < nodes.Length; i++)
			{
				if (nodes[i] != null)
				{
					action(nodes[i]);
				}
			}
		}

		protected override List<GraphNode> GetNodesInRegion(Bounds b, GraphUpdateShape shape)
		{
			IntRect rectFromBounds = GetRectFromBounds(b);
			if (nodes == null || !rectFromBounds.IsValid() || nodes.Length != width * depth * layerCount)
			{
				return ListPool<GraphNode>.Claim();
			}
			List<GraphNode> list = ListPool<GraphNode>.Claim(rectFromBounds.Width * rectFromBounds.Height * layerCount);
			for (int i = 0; i < layerCount; i++)
			{
				int num = i * width * depth;
				for (int j = rectFromBounds.xmin; j <= rectFromBounds.xmax; j++)
				{
					for (int k = rectFromBounds.ymin; k <= rectFromBounds.ymax; k++)
					{
						int num2 = num + k * width + j;
						GraphNode graphNode = nodes[num2];
						if (graphNode != null && b.Contains((Vector3)graphNode.position) && (shape == null || shape.Contains((Vector3)graphNode.position)))
						{
							list.Add(graphNode);
						}
					}
				}
			}
			return list;
		}

		public override List<GraphNode> GetNodesInRegion(IntRect rect)
		{
			List<GraphNode> list = ListPool<GraphNode>.Claim();
			rect = IntRect.Intersection(b: new IntRect(0, 0, width - 1, depth - 1), a: rect);
			if (nodes == null || !rect.IsValid() || nodes.Length != width * depth * layerCount)
			{
				return list;
			}
			for (int i = 0; i < layerCount; i++)
			{
				int num = i * base.Width * base.Depth;
				for (int j = rect.ymin; j <= rect.ymax; j++)
				{
					int num2 = num + j * base.Width;
					for (int k = rect.xmin; k <= rect.xmax; k++)
					{
						LevelGridNode levelGridNode = nodes[num2 + k];
						if (levelGridNode != null)
						{
							list.Add(levelGridNode);
						}
					}
				}
			}
			return list;
		}

		public override int GetNodesInRegion(IntRect rect, GridNodeBase[] buffer)
		{
			rect = IntRect.Intersection(b: new IntRect(0, 0, width - 1, depth - 1), a: rect);
			if (nodes == null || !rect.IsValid() || nodes.Length != width * depth * layerCount)
			{
				return 0;
			}
			int num = 0;
			try
			{
				for (int i = 0; i < layerCount; i++)
				{
					int num2 = i * base.Width * base.Depth;
					for (int j = rect.ymin; j <= rect.ymax; j++)
					{
						int num3 = num2 + j * base.Width;
						for (int k = rect.xmin; k <= rect.xmax; k++)
						{
							LevelGridNode levelGridNode = nodes[num3 + k];
							if (levelGridNode != null)
							{
								buffer[num] = levelGridNode;
								num++;
							}
						}
					}
				}
				return num;
			}
			catch (IndexOutOfRangeException)
			{
				throw new ArgumentException("Buffer is too small");
			}
		}

		public override GridNodeBase GetNode(int x, int z)
		{
			if (x < 0 || z < 0 || x >= width || z >= depth)
			{
				return null;
			}
			return nodes[x + z * width];
		}

		public GridNodeBase GetNode(int x, int z, int layer)
		{
			if (x < 0 || z < 0 || x >= width || z >= depth || layer < 0 || layer >= layerCount)
			{
				return null;
			}
			return nodes[x + z * width + layer * width * depth];
		}

		void IUpdatableGraph.UpdateArea(GraphUpdateObject o)
		{
			if (nodes == null || nodes.Length != width * depth * layerCount)
			{
				Debug.LogWarning("The Grid Graph is not scanned, cannot update area ");
				return;
			}
			IntRect originalRect;
			IntRect affectRect;
			IntRect physicsRect;
			bool willChangeWalkability;
			int erosion;
			CalculateAffectedRegions(o, out originalRect, out affectRect, out physicsRect, out willChangeWalkability, out erosion);
			bool flag = o is LayerGridGraphUpdate && ((LayerGridGraphUpdate)o).recalculateNodes;
			bool flag2 = ((!(o is LayerGridGraphUpdate)) ? (!o.resetPenaltyOnPhysics) : ((LayerGridGraphUpdate)o).preserveExistingNodes);
			if (o.trackChangedNodes && flag)
			{
				Debug.LogError("Cannot track changed nodes when creating or deleting nodes.\nWill not update LayerGridGraph");
				return;
			}
			IntRect b = new IntRect(0, 0, width - 1, depth - 1);
			IntRect intRect = IntRect.Intersection(affectRect, b);
			if (!flag)
			{
				for (int i = intRect.xmin; i <= intRect.xmax; i++)
				{
					for (int j = intRect.ymin; j <= intRect.ymax; j++)
					{
						for (int k = 0; k < layerCount; k++)
						{
							o.WillUpdateNode(nodes[k * width * depth + j * width + i]);
						}
					}
				}
			}
			if (o.updatePhysics && !o.modifyWalkability)
			{
				collision.Initialize(base.transform, nodeSize);
				intRect = IntRect.Intersection(physicsRect, b);
				for (int l = intRect.xmin; l <= intRect.xmax; l++)
				{
					for (int m = intRect.ymin; m <= intRect.ymax; m++)
					{
						RecalculateCell(l, m, !flag2, false);
					}
				}
				for (int n = intRect.xmin; n <= intRect.xmax; n++)
				{
					for (int num = intRect.ymin; num <= intRect.ymax; num++)
					{
						CalculateConnections(n, num);
					}
				}
			}
			intRect = IntRect.Intersection(originalRect, b);
			for (int num2 = intRect.xmin; num2 <= intRect.xmax; num2++)
			{
				for (int num3 = intRect.ymin; num3 <= intRect.ymax; num3++)
				{
					for (int num4 = 0; num4 < layerCount; num4++)
					{
						int num5 = num4 * width * depth + num3 * width + num2;
						LevelGridNode levelGridNode = nodes[num5];
						if (levelGridNode == null)
						{
							continue;
						}
						if (willChangeWalkability)
						{
							levelGridNode.Walkable = levelGridNode.WalkableErosion;
							if (o.bounds.Contains((Vector3)levelGridNode.position))
							{
								o.Apply(levelGridNode);
							}
							levelGridNode.WalkableErosion = levelGridNode.Walkable;
						}
						else if (o.bounds.Contains((Vector3)levelGridNode.position))
						{
							o.Apply(levelGridNode);
						}
					}
				}
			}
			if (willChangeWalkability && erosion == 0)
			{
				intRect = IntRect.Intersection(affectRect, b);
				for (int num6 = intRect.xmin; num6 <= intRect.xmax; num6++)
				{
					for (int num7 = intRect.ymin; num7 <= intRect.ymax; num7++)
					{
						CalculateConnections(num6, num7);
					}
				}
			}
			else
			{
				if (!willChangeWalkability || erosion <= 0)
				{
					return;
				}
				IntRect a = IntRect.Union(originalRect, physicsRect).Expand(erosion);
				IntRect a2 = a.Expand(erosion);
				a = IntRect.Intersection(a, b);
				a2 = IntRect.Intersection(a2, b);
				for (int num8 = a2.xmin; num8 <= a2.xmax; num8++)
				{
					for (int num9 = a2.ymin; num9 <= a2.ymax; num9++)
					{
						for (int num10 = 0; num10 < layerCount; num10++)
						{
							int num11 = num10 * width * depth + num9 * width + num8;
							LevelGridNode levelGridNode2 = nodes[num11];
							if (levelGridNode2 != null)
							{
								bool walkable = levelGridNode2.Walkable;
								levelGridNode2.Walkable = levelGridNode2.WalkableErosion;
								if (!a.Contains(num8, num9))
								{
									levelGridNode2.TmpWalkable = walkable;
								}
							}
						}
					}
				}
				for (int num12 = a2.xmin; num12 <= a2.xmax; num12++)
				{
					for (int num13 = a2.ymin; num13 <= a2.ymax; num13++)
					{
						CalculateConnections(num12, num13);
					}
				}
				ErodeWalkableArea(a2.xmin, a2.ymin, a2.xmax + 1, a2.ymax + 1);
				for (int num14 = a2.xmin; num14 <= a2.xmax; num14++)
				{
					for (int num15 = a2.ymin; num15 <= a2.ymax; num15++)
					{
						if (a.Contains(num14, num15))
						{
							continue;
						}
						for (int num16 = 0; num16 < layerCount; num16++)
						{
							int num17 = num16 * width * depth + num15 * width + num14;
							LevelGridNode levelGridNode3 = nodes[num17];
							if (levelGridNode3 != null)
							{
								levelGridNode3.Walkable = levelGridNode3.TmpWalkable;
							}
						}
					}
				}
				for (int num18 = a2.xmin; num18 <= a2.xmax; num18++)
				{
					for (int num19 = a2.ymin; num19 <= a2.ymax; num19++)
					{
						CalculateConnections(num18, num19);
					}
				}
			}
		}

		protected override IEnumerable<Progress> ScanInternal()
		{
			if (nodeSize <= 0f)
			{
				yield break;
			}
			UpdateTransform();
			if (width > 1024 || depth > 1024)
			{
				Debug.LogError("One of the grid's sides is longer than 1024 nodes");
				yield break;
			}
			lastScannedWidth = width;
			lastScannedDepth = depth;
			SetUpOffsetsAndCosts();
			LevelGridNode.SetGridGraph((int)graphIndex, this);
			maxClimb = Mathf.Clamp(maxClimb, 0f, characterHeight);
			LinkedLevelNode[] linkedCells = new LinkedLevelNode[width * depth];
			collision = collision ?? new GraphCollision();
			collision.Initialize(base.transform, nodeSize);
			int progressCounter = 0;
			for (int z3 = 0; z3 < depth; z3++)
			{
				if (progressCounter >= 1000)
				{
					progressCounter = 0;
					yield return new Progress(Mathf.Lerp(0.1f, 0.5f, (float)z3 / (float)depth), "Calculating positions");
				}
				progressCounter += width;
				for (int i = 0; i < width; i++)
				{
					linkedCells[z3 * width + i] = SampleCell(i, z3);
				}
			}
			layerCount = 0;
			for (int j = 0; j < linkedCells.Length; j++)
			{
				int num = 0;
				for (LinkedLevelNode linkedLevelNode = linkedCells[j]; linkedLevelNode != null; linkedLevelNode = linkedLevelNode.next)
				{
					num++;
				}
				layerCount = Math.Max(layerCount, num);
			}
			if (layerCount > 255)
			{
				Debug.LogError("Too many layers, a maximum of " + 255 + " (LevelGridNode.MaxLayerCount) layers are allowed (found " + layerCount + ")");
				yield break;
			}
			nodes = new LevelGridNode[width * depth * layerCount];
			for (int z2 = 0; z2 < depth; z2++)
			{
				if (progressCounter >= 1000)
				{
					progressCounter = 0;
					yield return new Progress(Mathf.Lerp(0.5f, 0.8f, (float)z2 / (float)depth), "Creating nodes");
				}
				progressCounter += width;
				for (int k = 0; k < width; k++)
				{
					RecalculateCell(k, z2);
				}
			}
			for (int z = 0; z < depth; z++)
			{
				if (progressCounter >= 1000)
				{
					progressCounter = 0;
					yield return new Progress(Mathf.Lerp(0.8f, 0.9f, (float)z / (float)depth), "Calculating connections");
				}
				progressCounter += width;
				for (int l = 0; l < width; l++)
				{
					CalculateConnections(l, z);
				}
			}
			yield return new Progress(0.95f, "Calculating Erosion");
			for (int m = 0; m < nodes.Length; m++)
			{
				LevelGridNode levelGridNode = nodes[m];
				if (levelGridNode != null && !levelGridNode.HasAnyGridConnections())
				{
					levelGridNode.Walkable = false;
					levelGridNode.WalkableErosion = levelGridNode.Walkable;
				}
			}
			ErodeWalkableArea();
		}

		private LinkedLevelNode SampleCell(int x, int z)
		{
			LinkedLevelNode linkedLevelNode = null;
			Vector3 vector = base.transform.Transform(new Vector3((float)x + 0.5f, 0f, (float)z + 0.5f));
			RaycastHit[] array = collision.CheckHeightAll(vector);
			Vector3 rhs = base.transform.WorldUpAtGraphPosition(vector);
			for (int i = 0; i < array.Length / 2; i++)
			{
				RaycastHit raycastHit = array[i];
				array[i] = array[array.Length - 1 - i];
				array[array.Length - 1 - i] = raycastHit;
			}
			if (array.Length > 0)
			{
				LinkedLevelNode linkedLevelNode2 = null;
				for (int j = 0; j < array.Length; j++)
				{
					LinkedLevelNode linkedLevelNode3 = new LinkedLevelNode();
					linkedLevelNode3.position = array[j].point;
					if (linkedLevelNode2 != null && Vector3.Dot(linkedLevelNode3.position - linkedLevelNode2.position, rhs) <= mergeSpanRange)
					{
						linkedLevelNode2.position = linkedLevelNode3.position;
						linkedLevelNode2.hit = array[j];
						linkedLevelNode2.walkable = collision.Check(linkedLevelNode3.position);
						continue;
					}
					linkedLevelNode3.walkable = collision.Check(linkedLevelNode3.position);
					linkedLevelNode3.hit = array[j];
					linkedLevelNode3.height = float.PositiveInfinity;
					if (linkedLevelNode == null)
					{
						linkedLevelNode = linkedLevelNode3;
						linkedLevelNode2 = linkedLevelNode3;
					}
					else
					{
						linkedLevelNode2.next = linkedLevelNode3;
						linkedLevelNode2.height = Vector3.Dot(linkedLevelNode3.position - linkedLevelNode2.position, rhs);
						linkedLevelNode2 = linkedLevelNode2.next;
					}
				}
			}
			else
			{
				LinkedLevelNode linkedLevelNode4 = new LinkedLevelNode();
				linkedLevelNode4.position = vector;
				linkedLevelNode4.height = float.PositiveInfinity;
				linkedLevelNode4.walkable = !collision.unwalkableWhenNoGround && collision.Check(vector);
				linkedLevelNode = linkedLevelNode4;
			}
			return linkedLevelNode;
		}

		public override void RecalculateCell(int x, int z, bool resetPenalties = true, bool resetTags = true)
		{
			float num = Mathf.Cos(maxSlope * ((float)Math.PI / 180f));
			int i = 0;
			LinkedLevelNode linkedLevelNode = SampleCell(x, z);
			while (linkedLevelNode != null)
			{
				if (i >= layerCount)
				{
					if (i + 1 > 255)
					{
						Debug.LogError("Too many layers, a maximum of " + 255 + " are allowed (required " + (i + 1) + ")");
						return;
					}
					AddLayers(1);
				}
				int num2 = z * width + x + width * depth * i;
				LevelGridNode levelGridNode = nodes[num2];
				bool flag = levelGridNode == null;
				if (flag)
				{
					if (nodes[num2] != null)
					{
						nodes[num2].Destroy();
					}
					levelGridNode = (nodes[num2] = new LevelGridNode(active));
					levelGridNode.NodeInGridIndex = z * width + x;
					levelGridNode.LayerCoordinateInGrid = i;
					levelGridNode.GraphIndex = graphIndex;
				}
				levelGridNode.position = (Int3)linkedLevelNode.position;
				levelGridNode.Walkable = linkedLevelNode.walkable;
				levelGridNode.WalkableErosion = levelGridNode.Walkable;
				if (flag || resetPenalties)
				{
					levelGridNode.Penalty = initialPenalty;
					if (penaltyPosition)
					{
						levelGridNode.Penalty += (uint)Mathf.RoundToInt(((float)levelGridNode.position.y - penaltyPositionOffset) * penaltyPositionFactor);
					}
				}
				if (flag || resetTags)
				{
					levelGridNode.Tag = 0u;
				}
				if (linkedLevelNode.hit.normal != Vector3.zero && (penaltyAngle || num > 0.0001f))
				{
					float num3 = Vector3.Dot(linkedLevelNode.hit.normal.normalized, collision.up);
					if (resetTags && penaltyAngle)
					{
						levelGridNode.Penalty += (uint)Mathf.RoundToInt((1f - num3) * penaltyAngleFactor);
					}
					if (num3 < num)
					{
						levelGridNode.Walkable = false;
					}
				}
				if (linkedLevelNode.height < characterHeight)
				{
					levelGridNode.Walkable = false;
				}
				levelGridNode.WalkableErosion = levelGridNode.Walkable;
				linkedLevelNode = linkedLevelNode.next;
				i++;
			}
			for (; i < layerCount; i++)
			{
				int num4 = z * width + x + width * depth * i;
				if (nodes[num4] != null)
				{
					nodes[num4].Destroy();
				}
				nodes[num4] = null;
			}
		}

		private void AddLayers(int count)
		{
			int num = layerCount + count;
			if (num > 255)
			{
				Debug.LogError("Too many layers, a maximum of " + 255 + " are allowed (required " + num + ")");
			}
			else
			{
				LevelGridNode[] array = nodes;
				nodes = new LevelGridNode[width * depth * num];
				for (int i = 0; i < array.Length; i++)
				{
					nodes[i] = array[i];
				}
				layerCount = num;
			}
		}

		protected override bool ErosionAnyFalseConnections(GraphNode baseNode)
		{
			LevelGridNode levelGridNode = baseNode as LevelGridNode;
			if (neighbours == NumNeighbours.Six)
			{
				for (int i = 0; i < 6; i++)
				{
					if (!levelGridNode.GetConnection(GridGraph.hexagonNeighbourIndices[i]))
					{
						return true;
					}
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					if (!levelGridNode.GetConnection(j))
					{
						return true;
					}
				}
			}
			return false;
		}

		[Obsolete("CalculateConnections no longer takes a node array, it just uses the one on the graph")]
		public void CalculateConnections(LevelGridNode[] nodes, LevelGridNode node, int x, int z, int layerIndex)
		{
			CalculateConnections(x, z, layerIndex);
		}

		public override void CalculateConnections(GridNodeBase baseNode)
		{
			LevelGridNode levelGridNode = baseNode as LevelGridNode;
			CalculateConnections(levelGridNode.XCoordinateInGrid, levelGridNode.ZCoordinateInGrid, levelGridNode.LayerCoordinateInGrid);
		}

		[Obsolete("Use CalculateConnections(x,z,layerIndex) or CalculateConnections(node) instead")]
		public void CalculateConnections(int x, int z, int layerIndex, LevelGridNode node)
		{
			CalculateConnections(x, z, layerIndex);
		}

		public override void CalculateConnections(int x, int z)
		{
			for (int i = 0; i < layerCount; i++)
			{
				CalculateConnections(x, z, i);
			}
		}

		public void CalculateConnections(int x, int z, int layerIndex)
		{
			LevelGridNode levelGridNode = nodes[z * width + x + width * depth * layerIndex];
			if (levelGridNode == null)
			{
				return;
			}
			levelGridNode.ResetAllGridConnections();
			if (!levelGridNode.Walkable)
			{
				return;
			}
			Vector3 vector = (Vector3)levelGridNode.position;
			Vector3 rhs = base.transform.WorldUpAtGraphPosition(vector);
			float num = Vector3.Dot(vector, rhs);
			float num2 = ((layerIndex != layerCount - 1 && nodes[levelGridNode.NodeInGridIndex + width * depth * (layerIndex + 1)] != null) ? Math.Abs(num - Vector3.Dot((Vector3)nodes[levelGridNode.NodeInGridIndex + width * depth * (layerIndex + 1)].position, rhs)) : float.PositiveInfinity);
			for (int i = 0; i < 4; i++)
			{
				int num3 = x + neighbourXOffsets[i];
				int num4 = z + neighbourZOffsets[i];
				if (num3 < 0 || num4 < 0 || num3 >= width || num4 >= depth)
				{
					continue;
				}
				int num5 = num4 * width + num3;
				int value = 255;
				for (int j = 0; j < layerCount; j++)
				{
					GraphNode graphNode = nodes[num5 + width * depth * j];
					if (graphNode != null && graphNode.Walkable)
					{
						float num6 = Vector3.Dot((Vector3)graphNode.position, rhs);
						float num7 = ((j != layerCount - 1 && nodes[num5 + width * depth * (j + 1)] != null) ? Math.Abs(num6 - Vector3.Dot((Vector3)nodes[num5 + width * depth * (j + 1)].position, rhs)) : float.PositiveInfinity);
						float num8 = Mathf.Max(num6, num);
						float num9 = Mathf.Min(num6 + num7, num + num2);
						float num10 = num9 - num8;
						if (num10 >= characterHeight && Mathf.Abs(num6 - num) <= maxClimb)
						{
							value = j;
						}
					}
				}
				levelGridNode.SetConnectionValue(i, value);
			}
		}

		public override NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
		{
			if (nodes == null || depth * width * layerCount != nodes.Length)
			{
				return default(NNInfoInternal);
			}
			Vector3 vector = base.transform.InverseTransform(position);
			float x = vector.x;
			float z = vector.z;
			int num = Mathf.Clamp((int)x, 0, width - 1);
			int num2 = Mathf.Clamp((int)z, 0, depth - 1);
			LevelGridNode nearestNode = GetNearestNode(position, num, num2, null);
			NNInfoInternal result = new NNInfoInternal(nearestNode);
			float y = base.transform.InverseTransform((Vector3)nearestNode.position).y;
			result.clampedPosition = base.transform.Transform(new Vector3(Mathf.Clamp(x, num, (float)num + 1f), y, Mathf.Clamp(z, num2, (float)num2 + 1f)));
			return result;
		}

		private LevelGridNode GetNearestNode(Vector3 position, int x, int z, NNConstraint constraint)
		{
			int num = width * z + x;
			float num2 = float.PositiveInfinity;
			LevelGridNode result = null;
			for (int i = 0; i < layerCount; i++)
			{
				LevelGridNode levelGridNode = nodes[num + width * depth * i];
				if (levelGridNode != null)
				{
					float sqrMagnitude = ((Vector3)levelGridNode.position - position).sqrMagnitude;
					if (sqrMagnitude < num2 && (constraint == null || constraint.Suitable(levelGridNode)))
					{
						num2 = sqrMagnitude;
						result = levelGridNode;
					}
				}
			}
			return result;
		}

		public override NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
		{
			if (nodes == null || depth * width * layerCount != nodes.Length || layerCount == 0)
			{
				return default(NNInfoInternal);
			}
			Vector3 vector = position;
			position = base.transform.InverseTransform(position);
			float x = position.x;
			float z = position.z;
			int num = Mathf.Clamp((int)x, 0, width - 1);
			int num2 = Mathf.Clamp((int)z, 0, depth - 1);
			float num3 = float.PositiveInfinity;
			int num4 = 2;
			LevelGridNode levelGridNode = GetNearestNode(vector, num, num2, constraint);
			if (levelGridNode != null)
			{
				num3 = ((Vector3)levelGridNode.position - vector).sqrMagnitude;
			}
			if (levelGridNode != null && num4 > 0)
			{
				num4--;
			}
			float num5 = ((!constraint.constrainDistance) ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistance);
			float num6 = num5 * num5;
			int num7 = 1;
			while (true)
			{
				int num8 = num2 + num7;
				if (nodeSize * (float)num7 > num5)
				{
					break;
				}
				int i;
				for (i = num - num7; i <= num + num7; i++)
				{
					if (i < 0 || num8 < 0 || i >= width || num8 >= depth)
					{
						continue;
					}
					LevelGridNode nearestNode = GetNearestNode(vector, i, num8, constraint);
					if (nearestNode != null)
					{
						float sqrMagnitude = ((Vector3)nearestNode.position - vector).sqrMagnitude;
						if (sqrMagnitude < num3 && sqrMagnitude < num6)
						{
							num3 = sqrMagnitude;
							levelGridNode = nearestNode;
						}
					}
				}
				num8 = num2 - num7;
				for (i = num - num7; i <= num + num7; i++)
				{
					if (i < 0 || num8 < 0 || i >= width || num8 >= depth)
					{
						continue;
					}
					LevelGridNode nearestNode2 = GetNearestNode(vector, i, num8, constraint);
					if (nearestNode2 != null)
					{
						float sqrMagnitude2 = ((Vector3)nearestNode2.position - vector).sqrMagnitude;
						if (sqrMagnitude2 < num3 && sqrMagnitude2 < num6)
						{
							num3 = sqrMagnitude2;
							levelGridNode = nearestNode2;
						}
					}
				}
				i = num - num7;
				for (num8 = num2 - num7 + 1; num8 <= num2 + num7 - 1; num8++)
				{
					if (i < 0 || num8 < 0 || i >= width || num8 >= depth)
					{
						continue;
					}
					LevelGridNode nearestNode3 = GetNearestNode(vector, i, num8, constraint);
					if (nearestNode3 != null)
					{
						float sqrMagnitude3 = ((Vector3)nearestNode3.position - vector).sqrMagnitude;
						if (sqrMagnitude3 < num3 && sqrMagnitude3 < num6)
						{
							num3 = sqrMagnitude3;
							levelGridNode = nearestNode3;
						}
					}
				}
				i = num + num7;
				for (num8 = num2 - num7 + 1; num8 <= num2 + num7 - 1; num8++)
				{
					if (i < 0 || num8 < 0 || i >= width || num8 >= depth)
					{
						continue;
					}
					LevelGridNode nearestNode4 = GetNearestNode(vector, i, num8, constraint);
					if (nearestNode4 != null)
					{
						float sqrMagnitude4 = ((Vector3)nearestNode4.position - vector).sqrMagnitude;
						if (sqrMagnitude4 < num3 && sqrMagnitude4 < num6)
						{
							num3 = sqrMagnitude4;
							levelGridNode = nearestNode4;
						}
					}
				}
				if (levelGridNode != null)
				{
					if (num4 == 0)
					{
						break;
					}
					num4--;
				}
				num7++;
			}
			NNInfoInternal result = new NNInfoInternal(levelGridNode);
			if (levelGridNode != null)
			{
				int xCoordinateInGrid = levelGridNode.XCoordinateInGrid;
				int zCoordinateInGrid = levelGridNode.ZCoordinateInGrid;
				result.clampedPosition = base.transform.Transform(new Vector3(Mathf.Clamp(x, xCoordinateInGrid, (float)xCoordinateInGrid + 1f), base.transform.InverseTransform((Vector3)levelGridNode.position).y, Mathf.Clamp(z, zCoordinateInGrid, (float)zCoordinateInGrid + 1f)));
			}
			return result;
		}

		[Obsolete("Use node.GetConnection instead")]
		public static bool CheckConnection(LevelGridNode node, int dir)
		{
			return node.GetConnection(dir);
		}

		protected override void SerializeExtraInfo(GraphSerializationContext ctx)
		{
			if (nodes == null)
			{
				ctx.writer.Write(-1);
				return;
			}
			ctx.writer.Write(nodes.Length);
			for (int i = 0; i < nodes.Length; i++)
			{
				if (nodes[i] == null)
				{
					ctx.writer.Write(-1);
					continue;
				}
				ctx.writer.Write(0);
				nodes[i].SerializeNode(ctx);
			}
		}

		protected override void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				nodes = null;
				return;
			}
			nodes = new LevelGridNode[num];
			for (int i = 0; i < nodes.Length; i++)
			{
				if (ctx.reader.ReadInt32() != -1)
				{
					nodes[i] = new LevelGridNode(active);
					nodes[i].DeserializeNode(ctx);
				}
				else
				{
					nodes[i] = null;
				}
			}
		}

		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			UpdateTransform();
			lastScannedWidth = width;
			lastScannedDepth = depth;
			SetUpOffsetsAndCosts();
			LevelGridNode.SetGridGraph((int)graphIndex, this);
			if (nodes == null || nodes.Length == 0)
			{
				return;
			}
			if (width * depth * layerCount != nodes.Length)
			{
				Debug.LogError("Node data did not match with bounds data. Probably a change to the bounds/width/depth data was made after scanning the graph just prior to saving it. Nodes will be discarded");
				nodes = new LevelGridNode[0];
				return;
			}
			for (int i = 0; i < layerCount; i++)
			{
				for (int j = 0; j < depth; j++)
				{
					for (int k = 0; k < width; k++)
					{
						LevelGridNode levelGridNode = nodes[j * width + k + width * depth * i];
						if (levelGridNode != null)
						{
							levelGridNode.NodeInGridIndex = j * width + k;
							levelGridNode.LayerCoordinateInGrid = i;
						}
					}
				}
			}
		}
	}
}
