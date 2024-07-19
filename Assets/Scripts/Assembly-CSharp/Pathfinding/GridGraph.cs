using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[JsonOptIn]
	public class GridGraph : NavGraph, IUpdatableGraph, ITransformedGraph, IRaycastableGraph
	{
		public class TextureData
		{
			public enum ChannelUse
			{
				None = 0,
				Penalty = 1,
				Position = 2,
				WalkablePenalty = 3
			}

			public bool enabled;

			public Texture2D source;

			public float[] factors = new float[3];

			public ChannelUse[] channels = new ChannelUse[3];

			private Color32[] data;

			public void Initialize()
			{
				if (!enabled || !(source != null))
				{
					return;
				}
				for (int i = 0; i < channels.Length; i++)
				{
					if (channels[i] != 0)
					{
						try
						{
							data = source.GetPixels32();
							break;
						}
						catch (UnityException ex)
						{
							Debug.LogWarning(ex.ToString());
							data = null;
							break;
						}
					}
				}
			}

			public void Apply(GridNode node, int x, int z)
			{
				if (enabled && data != null && x < source.width && z < source.height)
				{
					Color32 color = data[z * source.width + x];
					if (channels[0] != 0)
					{
						ApplyChannel(node, x, z, color.r, channels[0], factors[0]);
					}
					if (channels[1] != 0)
					{
						ApplyChannel(node, x, z, color.g, channels[1], factors[1]);
					}
					if (channels[2] != 0)
					{
						ApplyChannel(node, x, z, color.b, channels[2], factors[2]);
					}
					node.WalkableErosion = node.Walkable;
				}
			}

			private void ApplyChannel(GridNode node, int x, int z, int value, ChannelUse channelUse, float factor)
			{
				switch (channelUse)
				{
				case ChannelUse.Penalty:
					node.Penalty += (uint)Mathf.RoundToInt((float)value * factor);
					break;
				case ChannelUse.Position:
					node.position = GridNode.GetGridGraph(node.GraphIndex).GraphPointToWorld(x, z, value);
					break;
				case ChannelUse.WalkablePenalty:
					if (value == 0)
					{
						node.Walkable = false;
					}
					else
					{
						node.Penalty += (uint)Mathf.RoundToInt((float)(value - 1) * factor);
					}
					break;
				}
			}
		}

		[JsonMember]
		public InspectorGridMode inspectorGridMode;

		public int width;

		public int depth;

		[JsonMember]
		public float aspectRatio = 1f;

		[JsonMember]
		public float isometricAngle;

		[JsonMember]
		public bool uniformEdgeCosts;

		[JsonMember]
		public Vector3 rotation;

		[JsonMember]
		public Vector3 center;

		[JsonMember]
		public Vector2 unclampedSize;

		[JsonMember]
		public float nodeSize = 1f;

		[JsonMember]
		public GraphCollision collision;

		[JsonMember]
		public float maxClimb = 0.4f;

		[JsonMember]
		public float maxSlope = 90f;

		[JsonMember]
		public int erodeIterations;

		[JsonMember]
		public bool erosionUseTags;

		[JsonMember]
		public int erosionFirstTag = 1;

		[JsonMember]
		public NumNeighbours neighbours = NumNeighbours.Eight;

		[JsonMember]
		public bool cutCorners = true;

		[JsonMember]
		public float penaltyPositionOffset;

		[JsonMember]
		public bool penaltyPosition;

		[JsonMember]
		public float penaltyPositionFactor = 1f;

		[JsonMember]
		public bool penaltyAngle;

		[JsonMember]
		public float penaltyAngleFactor = 100f;

		[JsonMember]
		public float penaltyAnglePower = 1f;

		[JsonMember]
		public bool useJumpPointSearch;

		[JsonMember]
		public bool showMeshOutline = true;

		[JsonMember]
		public bool showNodeConnections;

		[JsonMember]
		public bool showMeshSurface = true;

		[JsonMember]
		public TextureData textureData = new TextureData();

		[NonSerialized]
		public readonly int[] neighbourOffsets = new int[8];

		[NonSerialized]
		public readonly uint[] neighbourCosts = new uint[8];

		[NonSerialized]
		public readonly int[] neighbourXOffsets = new int[8];

		[NonSerialized]
		public readonly int[] neighbourZOffsets = new int[8];

		internal static readonly int[] hexagonNeighbourIndices = new int[6] { 0, 1, 5, 2, 3, 7 };

		public const int getNearestForceOverlap = 2;

		public GridNode[] nodes;

		public virtual bool uniformWidthDepthGrid
		{
			get
			{
				return true;
			}
		}

		public virtual int LayerCount
		{
			get
			{
				return 1;
			}
		}

		protected bool useRaycastNormal
		{
			get
			{
				return Math.Abs(90f - maxSlope) > float.Epsilon;
			}
		}

		public Vector2 size { get; protected set; }

		public GraphTransform transform { get; private set; }

		public int Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}

		public int Depth
		{
			get
			{
				return depth;
			}
			set
			{
				depth = value;
			}
		}

		public GridGraph()
		{
			unclampedSize = new Vector2(10f, 10f);
			nodeSize = 1f;
			collision = new GraphCollision();
			transform = new GraphTransform(Matrix4x4.identity);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			RemoveGridGraphFromStatic();
		}

		protected override void DestroyAllNodes()
		{
			GetNodes(delegate(GraphNode node)
			{
				(node as GridNodeBase).ClearCustomConnections(true);
				node.ClearConnections(false);
				node.Destroy();
			});
		}

		private void RemoveGridGraphFromStatic()
		{
			GridNode.SetGridGraph(AstarPath.active.data.GetGraphIndex(this), null);
		}

		public override int CountNodes()
		{
			return nodes.Length;
		}

		public override void GetNodes(Action<GraphNode> action)
		{
			if (nodes != null)
			{
				for (int i = 0; i < nodes.Length; i++)
				{
					action(nodes[i]);
				}
			}
		}

		public override void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			throw new Exception("This method cannot be used for Grid Graphs. Please use the other overload of RelocateNodes instead");
		}

		public void RelocateNodes(Vector3 center, Quaternion rotation, float nodeSize, float aspectRatio = 1f, float isometricAngle = 0f)
		{
			GraphTransform previousTransform = transform;
			this.center = center;
			this.rotation = rotation.eulerAngles;
			this.aspectRatio = aspectRatio;
			this.isometricAngle = isometricAngle;
			SetDimensions(width, depth, nodeSize);
			GetNodes(delegate(GraphNode node)
			{
				GridNodeBase gridNodeBase = node as GridNodeBase;
				float y = previousTransform.InverseTransform((Vector3)node.position).y;
				node.position = GraphPointToWorld(gridNodeBase.XCoordinateInGrid, gridNodeBase.ZCoordinateInGrid, y);
			});
		}

		public Int3 GraphPointToWorld(int x, int z, float height)
		{
			return (Int3)transform.Transform(new Vector3((float)x + 0.5f, height, (float)z + 0.5f));
		}

		public uint GetConnectionCost(int dir)
		{
			return neighbourCosts[dir];
		}

		public GridNode GetNodeConnection(GridNode node, int dir)
		{
			if (!node.HasConnectionInDirection(dir))
			{
				return null;
			}
			if (!node.EdgeNode)
			{
				return nodes[node.NodeInGridIndex + neighbourOffsets[dir]];
			}
			int nodeInGridIndex = node.NodeInGridIndex;
			int num = nodeInGridIndex / Width;
			int x = nodeInGridIndex - num * Width;
			return GetNodeConnection(nodeInGridIndex, x, num, dir);
		}

		public bool HasNodeConnection(GridNode node, int dir)
		{
			if (!node.HasConnectionInDirection(dir))
			{
				return false;
			}
			if (!node.EdgeNode)
			{
				return true;
			}
			int nodeInGridIndex = node.NodeInGridIndex;
			int num = nodeInGridIndex / Width;
			int x = nodeInGridIndex - num * Width;
			return HasNodeConnection(nodeInGridIndex, x, num, dir);
		}

		public void SetNodeConnection(GridNode node, int dir, bool value)
		{
			int nodeInGridIndex = node.NodeInGridIndex;
			int num = nodeInGridIndex / Width;
			int x = nodeInGridIndex - num * Width;
			SetNodeConnection(nodeInGridIndex, x, num, dir, value);
		}

		private GridNode GetNodeConnection(int index, int x, int z, int dir)
		{
			if (!nodes[index].HasConnectionInDirection(dir))
			{
				return null;
			}
			int num = x + neighbourXOffsets[dir];
			if (num < 0 || num >= Width)
			{
				return null;
			}
			int num2 = z + neighbourZOffsets[dir];
			if (num2 < 0 || num2 >= Depth)
			{
				return null;
			}
			int num3 = index + neighbourOffsets[dir];
			return nodes[num3];
		}

		public void SetNodeConnection(int index, int x, int z, int dir, bool value)
		{
			nodes[index].SetConnectionInternal(dir, value);
		}

		public bool HasNodeConnection(int index, int x, int z, int dir)
		{
			if (!nodes[index].HasConnectionInDirection(dir))
			{
				return false;
			}
			int num = x + neighbourXOffsets[dir];
			if (num < 0 || num >= Width)
			{
				return false;
			}
			int num2 = z + neighbourZOffsets[dir];
			if (num2 < 0 || num2 >= Depth)
			{
				return false;
			}
			return true;
		}

		public void SetDimensions(int width, int depth, float nodeSize)
		{
			unclampedSize = new Vector2(width, depth) * nodeSize;
			this.nodeSize = nodeSize;
			UpdateTransform();
		}

		[Obsolete("Use SetDimensions instead")]
		public void UpdateSizeFromWidthDepth()
		{
			SetDimensions(width, depth, nodeSize);
		}

		[Obsolete("This method has been renamed to UpdateTransform")]
		public void GenerateMatrix()
		{
			UpdateTransform();
		}

		public void UpdateTransform()
		{
			CalculateDimensions(out width, out depth, out nodeSize);
			transform = CalculateTransform();
		}

		public GraphTransform CalculateTransform()
		{
			int num;
			int num2;
			float num3;
			CalculateDimensions(out num, out num2, out num3);
			Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 45f, 0f), Vector3.one);
			matrix4x = Matrix4x4.Scale(new Vector3(Mathf.Cos((float)Math.PI / 180f * isometricAngle), 1f, 1f)) * matrix4x;
			matrix4x = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, -45f, 0f), Vector3.one) * matrix4x;
			Matrix4x4 matrix4x2 = Matrix4x4.TRS((Matrix4x4.TRS(center, Quaternion.Euler(rotation), new Vector3(aspectRatio, 1f, 1f)) * matrix4x).MultiplyPoint3x4(-new Vector3((float)num * num3, 0f, (float)num2 * num3) * 0.5f), Quaternion.Euler(rotation), new Vector3(num3 * aspectRatio, 1f, num3)) * matrix4x;
			return new GraphTransform(matrix4x2);
		}

		private void CalculateDimensions(out int width, out int depth, out float nodeSize)
		{
			Vector2 vector = unclampedSize;
			vector.x *= Mathf.Sign(vector.x);
			vector.y *= Mathf.Sign(vector.y);
			nodeSize = Mathf.Max(this.nodeSize, vector.x / 1024f);
			nodeSize = Mathf.Max(this.nodeSize, vector.y / 1024f);
			vector.x = ((!(vector.x < nodeSize)) ? vector.x : nodeSize);
			vector.y = ((!(vector.y < nodeSize)) ? vector.y : nodeSize);
			size = vector;
			width = Mathf.FloorToInt(size.x / nodeSize);
			depth = Mathf.FloorToInt(size.y / nodeSize);
			if (Mathf.Approximately(size.x / nodeSize, Mathf.CeilToInt(size.x / nodeSize)))
			{
				width = Mathf.CeilToInt(size.x / nodeSize);
			}
			if (Mathf.Approximately(size.y / nodeSize, Mathf.CeilToInt(size.y / nodeSize)))
			{
				depth = Mathf.CeilToInt(size.y / nodeSize);
			}
		}

		public override NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
		{
			if (nodes == null || depth * width != nodes.Length)
			{
				return default(NNInfoInternal);
			}
			position = transform.InverseTransform(position);
			float x = position.x;
			float z = position.z;
			int num = Mathf.Clamp((int)x, 0, width - 1);
			int num2 = Mathf.Clamp((int)z, 0, depth - 1);
			NNInfoInternal result = new NNInfoInternal(nodes[num2 * width + num]);
			float y = transform.InverseTransform((Vector3)nodes[num2 * width + num].position).y;
			result.clampedPosition = transform.Transform(new Vector3(Mathf.Clamp(x, num, (float)num + 1f), y, Mathf.Clamp(z, num2, (float)num2 + 1f)));
			return result;
		}

		public override NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
		{
			if (nodes == null || depth * width != nodes.Length)
			{
				return default(NNInfoInternal);
			}
			Vector3 vector = position;
			position = transform.InverseTransform(position);
			float x = position.x;
			float z = position.z;
			int num = Mathf.Clamp((int)x, 0, width - 1);
			int num2 = Mathf.Clamp((int)z, 0, depth - 1);
			GridNode gridNode = nodes[num + num2 * width];
			GridNode gridNode2 = null;
			float num3 = float.PositiveInfinity;
			int num4 = 2;
			Vector3 clampedPosition = Vector3.zero;
			NNInfoInternal result = new NNInfoInternal(null);
			if (constraint == null || constraint.Suitable(gridNode))
			{
				gridNode2 = gridNode;
				num3 = ((Vector3)gridNode2.position - vector).sqrMagnitude;
				float y = transform.InverseTransform((Vector3)gridNode.position).y;
				clampedPosition = transform.Transform(new Vector3(Mathf.Clamp(x, num, (float)num + 1f), y, Mathf.Clamp(z, num2, (float)num2 + 1f)));
			}
			if (gridNode2 != null)
			{
				result.node = gridNode2;
				result.clampedPosition = clampedPosition;
				if (num4 == 0)
				{
					return result;
				}
				num4--;
			}
			float num5 = ((constraint != null && !constraint.constrainDistance) ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistance);
			float num6 = num5 * num5;
			for (int i = 1; !(nodeSize * (float)i > num5); i++)
			{
				bool flag = false;
				int num7 = num2 + i;
				int num8 = num7 * width;
				int j;
				for (j = num - i; j <= num + i; j++)
				{
					if (j < 0 || num7 < 0 || j >= width || num7 >= depth)
					{
						continue;
					}
					flag = true;
					if (constraint == null || constraint.Suitable(nodes[j + num8]))
					{
						float sqrMagnitude = ((Vector3)nodes[j + num8].position - vector).sqrMagnitude;
						if (sqrMagnitude < num3 && sqrMagnitude < num6)
						{
							num3 = sqrMagnitude;
							gridNode2 = nodes[j + num8];
							clampedPosition = transform.Transform(new Vector3(Mathf.Clamp(x, j, (float)j + 1f), transform.InverseTransform((Vector3)gridNode2.position).y, Mathf.Clamp(z, num7, (float)num7 + 1f)));
						}
					}
				}
				num7 = num2 - i;
				num8 = num7 * width;
				for (j = num - i; j <= num + i; j++)
				{
					if (j < 0 || num7 < 0 || j >= width || num7 >= depth)
					{
						continue;
					}
					flag = true;
					if (constraint == null || constraint.Suitable(nodes[j + num8]))
					{
						float sqrMagnitude2 = ((Vector3)nodes[j + num8].position - vector).sqrMagnitude;
						if (sqrMagnitude2 < num3 && sqrMagnitude2 < num6)
						{
							num3 = sqrMagnitude2;
							gridNode2 = nodes[j + num8];
							clampedPosition = transform.Transform(new Vector3(Mathf.Clamp(x, j, (float)j + 1f), transform.InverseTransform((Vector3)gridNode2.position).y, Mathf.Clamp(z, num7, (float)num7 + 1f)));
						}
					}
				}
				j = num - i;
				for (num7 = num2 - i + 1; num7 <= num2 + i - 1; num7++)
				{
					if (j < 0 || num7 < 0 || j >= width || num7 >= depth)
					{
						continue;
					}
					flag = true;
					if (constraint == null || constraint.Suitable(nodes[j + num7 * width]))
					{
						float sqrMagnitude3 = ((Vector3)nodes[j + num7 * width].position - vector).sqrMagnitude;
						if (sqrMagnitude3 < num3 && sqrMagnitude3 < num6)
						{
							num3 = sqrMagnitude3;
							gridNode2 = nodes[j + num7 * width];
							clampedPosition = transform.Transform(new Vector3(Mathf.Clamp(x, j, (float)j + 1f), transform.InverseTransform((Vector3)gridNode2.position).y, Mathf.Clamp(z, num7, (float)num7 + 1f)));
						}
					}
				}
				j = num + i;
				for (num7 = num2 - i + 1; num7 <= num2 + i - 1; num7++)
				{
					if (j < 0 || num7 < 0 || j >= width || num7 >= depth)
					{
						continue;
					}
					flag = true;
					if (constraint == null || constraint.Suitable(nodes[j + num7 * width]))
					{
						float sqrMagnitude4 = ((Vector3)nodes[j + num7 * width].position - vector).sqrMagnitude;
						if (sqrMagnitude4 < num3 && sqrMagnitude4 < num6)
						{
							num3 = sqrMagnitude4;
							gridNode2 = nodes[j + num7 * width];
							clampedPosition = transform.Transform(new Vector3(Mathf.Clamp(x, j, (float)j + 1f), transform.InverseTransform((Vector3)gridNode2.position).y, Mathf.Clamp(z, num7, (float)num7 + 1f)));
						}
					}
				}
				if (gridNode2 != null)
				{
					if (num4 == 0)
					{
						break;
					}
					num4--;
				}
				if (!flag)
				{
					break;
				}
			}
			result.node = gridNode2;
			result.clampedPosition = clampedPosition;
			return result;
		}

		public virtual void SetUpOffsetsAndCosts()
		{
			neighbourOffsets[0] = -width;
			neighbourOffsets[1] = 1;
			neighbourOffsets[2] = width;
			neighbourOffsets[3] = -1;
			neighbourOffsets[4] = -width + 1;
			neighbourOffsets[5] = width + 1;
			neighbourOffsets[6] = width - 1;
			neighbourOffsets[7] = -width - 1;
			uint num = (uint)Mathf.RoundToInt(nodeSize * 1000f);
			uint num2 = ((!uniformEdgeCosts) ? ((uint)Mathf.RoundToInt(nodeSize * Mathf.Sqrt(2f) * 1000f)) : num);
			neighbourCosts[0] = num;
			neighbourCosts[1] = num;
			neighbourCosts[2] = num;
			neighbourCosts[3] = num;
			neighbourCosts[4] = num2;
			neighbourCosts[5] = num2;
			neighbourCosts[6] = num2;
			neighbourCosts[7] = num2;
			neighbourXOffsets[0] = 0;
			neighbourXOffsets[1] = 1;
			neighbourXOffsets[2] = 0;
			neighbourXOffsets[3] = -1;
			neighbourXOffsets[4] = 1;
			neighbourXOffsets[5] = 1;
			neighbourXOffsets[6] = -1;
			neighbourXOffsets[7] = -1;
			neighbourZOffsets[0] = -1;
			neighbourZOffsets[1] = 0;
			neighbourZOffsets[2] = 1;
			neighbourZOffsets[3] = 0;
			neighbourZOffsets[4] = -1;
			neighbourZOffsets[5] = 1;
			neighbourZOffsets[6] = 1;
			neighbourZOffsets[7] = -1;
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
			if (useJumpPointSearch)
			{
				Debug.LogError("Trying to use Jump Point Search, but support for it is not enabled. Please enable it in the inspector (Grid Graph settings).");
			}
			SetUpOffsetsAndCosts();
			GridNode.SetGridGraph((int)graphIndex, this);
			yield return new Progress(0.05f, "Creating nodes");
			nodes = new GridNode[width * depth];
			for (int i = 0; i < depth; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int num = i * width + j;
					GridNode gridNode = (nodes[num] = new GridNode(active));
					gridNode.GraphIndex = graphIndex;
					gridNode.NodeInGridIndex = num;
				}
			}
			if (collision == null)
			{
				collision = new GraphCollision();
			}
			collision.Initialize(transform, nodeSize);
			textureData.Initialize();
			int progressCounter2 = 0;
			for (int z2 = 0; z2 < depth; z2++)
			{
				if (progressCounter2 >= 1000)
				{
					progressCounter2 = 0;
					yield return new Progress(Mathf.Lerp(0.1f, 0.7f, (float)z2 / (float)depth), "Calculating positions");
				}
				progressCounter2 += width;
				for (int k = 0; k < width; k++)
				{
					RecalculateCell(k, z2);
					textureData.Apply(nodes[z2 * width + k], k, z2);
				}
			}
			progressCounter2 = 0;
			for (int z = 0; z < depth; z++)
			{
				if (progressCounter2 >= 1000)
				{
					progressCounter2 = 0;
					yield return new Progress(Mathf.Lerp(0.7f, 0.9f, (float)z / (float)depth), "Calculating connections");
				}
				progressCounter2 += width;
				for (int l = 0; l < width; l++)
				{
					CalculateConnections(l, z);
				}
			}
			yield return new Progress(0.95f, "Calculating erosion");
			ErodeWalkableArea();
		}

		[Obsolete("Use RecalculateCell instead which works both for grid graphs and layered grid graphs")]
		public virtual void UpdateNodePositionCollision(GridNode node, int x, int z, bool resetPenalty = true)
		{
			RecalculateCell(x, z, resetPenalty, false);
		}

		public virtual void RecalculateCell(int x, int z, bool resetPenalties = true, bool resetTags = true)
		{
			GridNode gridNode = nodes[z * width + x];
			gridNode.position = GraphPointToWorld(x, z, 0f);
			RaycastHit hit;
			bool walkable;
			Vector3 vector = collision.CheckHeight((Vector3)gridNode.position, out hit, out walkable);
			gridNode.position = (Int3)vector;
			if (resetPenalties)
			{
				gridNode.Penalty = initialPenalty;
				if (penaltyPosition)
				{
					gridNode.Penalty += (uint)Mathf.RoundToInt(((float)gridNode.position.y - penaltyPositionOffset) * penaltyPositionFactor);
				}
			}
			if (resetTags)
			{
				gridNode.Tag = 0u;
			}
			if (walkable && useRaycastNormal && collision.heightCheck && hit.normal != Vector3.zero)
			{
				float num = Vector3.Dot(hit.normal.normalized, collision.up);
				if (penaltyAngle && resetPenalties)
				{
					gridNode.Penalty += (uint)Mathf.RoundToInt((1f - Mathf.Pow(num, penaltyAnglePower)) * penaltyAngleFactor);
				}
				float num2 = Mathf.Cos(maxSlope * ((float)Math.PI / 180f));
				if (num < num2)
				{
					walkable = false;
				}
			}
			gridNode.Walkable = walkable && collision.Check((Vector3)gridNode.position);
			gridNode.WalkableErosion = gridNode.Walkable;
		}

		protected virtual bool ErosionAnyFalseConnections(GraphNode baseNode)
		{
			GridNode node = baseNode as GridNode;
			if (neighbours == NumNeighbours.Six)
			{
				for (int i = 0; i < 6; i++)
				{
					if (!HasNodeConnection(node, hexagonNeighbourIndices[i]))
					{
						return true;
					}
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					if (!HasNodeConnection(node, j))
					{
						return true;
					}
				}
			}
			return false;
		}

		private void ErodeNode(GraphNode node)
		{
			if (node.Walkable && ErosionAnyFalseConnections(node))
			{
				node.Walkable = false;
			}
		}

		private void ErodeNodeWithTagsInit(GraphNode node)
		{
			if (node.Walkable && ErosionAnyFalseConnections(node))
			{
				node.Tag = (uint)erosionFirstTag;
			}
			else
			{
				node.Tag = 0u;
			}
		}

		private void ErodeNodeWithTags(GraphNode node, int iteration)
		{
			GridNodeBase gridNodeBase = node as GridNodeBase;
			if (!gridNodeBase.Walkable || gridNodeBase.Tag < erosionFirstTag || gridNodeBase.Tag >= erosionFirstTag + iteration)
			{
				return;
			}
			if (neighbours == NumNeighbours.Six)
			{
				for (int i = 0; i < 6; i++)
				{
					GridNodeBase neighbourAlongDirection = gridNodeBase.GetNeighbourAlongDirection(hexagonNeighbourIndices[i]);
					if (neighbourAlongDirection != null)
					{
						uint tag = neighbourAlongDirection.Tag;
						if (tag > erosionFirstTag + iteration || tag < erosionFirstTag)
						{
							neighbourAlongDirection.Tag = (uint)(erosionFirstTag + iteration);
						}
					}
				}
				return;
			}
			for (int j = 0; j < 4; j++)
			{
				GridNodeBase neighbourAlongDirection2 = gridNodeBase.GetNeighbourAlongDirection(j);
				if (neighbourAlongDirection2 != null)
				{
					uint tag2 = neighbourAlongDirection2.Tag;
					if (tag2 > erosionFirstTag + iteration || tag2 < erosionFirstTag)
					{
						neighbourAlongDirection2.Tag = (uint)(erosionFirstTag + iteration);
					}
				}
			}
		}

		public virtual void ErodeWalkableArea()
		{
			ErodeWalkableArea(0, 0, Width, Depth);
		}

		public void ErodeWalkableArea(int xmin, int zmin, int xmax, int zmax)
		{
			if (erosionUseTags)
			{
				if (erodeIterations + erosionFirstTag > 31)
				{
					Debug.LogError("Too few tags available for " + erodeIterations + " erode iterations and starting with tag " + erosionFirstTag + " (erodeIterations+erosionFirstTag > 31)", active);
					return;
				}
				if (erosionFirstTag <= 0)
				{
					Debug.LogError("First erosion tag must be greater or equal to 1", active);
					return;
				}
			}
			if (erodeIterations == 0)
			{
				return;
			}
			IntRect rect = new IntRect(xmin, zmin, xmax - 1, zmax - 1);
			List<GraphNode> list = GetNodesInRegion(rect);
			int count = list.Count;
			for (int i = 0; i < erodeIterations; i++)
			{
				if (erosionUseTags)
				{
					if (i == 0)
					{
						for (int j = 0; j < count; j++)
						{
							ErodeNodeWithTagsInit(list[j]);
						}
					}
					else
					{
						for (int k = 0; k < count; k++)
						{
							ErodeNodeWithTags(list[k], i);
						}
					}
				}
				else
				{
					for (int l = 0; l < count; l++)
					{
						ErodeNode(list[l]);
					}
					for (int m = 0; m < count; m++)
					{
						CalculateConnections(list[m] as GridNodeBase);
					}
				}
			}
			ListPool<GraphNode>.Release(ref list);
		}

		public virtual bool IsValidConnection(GridNodeBase node1, GridNodeBase node2)
		{
			if (!node1.Walkable || !node2.Walkable)
			{
				return false;
			}
			if (maxClimb <= 0f || collision.use2D)
			{
				return true;
			}
			if (transform.onlyTranslational)
			{
				return (float)Math.Abs(node1.position.y - node2.position.y) <= maxClimb * 1000f;
			}
			Vector3 vector = (Vector3)node1.position;
			Vector3 rhs = (Vector3)node2.position;
			Vector3 lhs = transform.WorldUpAtGraphPosition(vector);
			return Math.Abs(Vector3.Dot(lhs, vector) - Vector3.Dot(lhs, rhs)) <= maxClimb;
		}

		public void CalculateConnectionsForCellAndNeighbours(int x, int z)
		{
			CalculateConnections(x, z);
			for (int i = 0; i < 8; i++)
			{
				int x2 = x + neighbourXOffsets[i];
				int z2 = z + neighbourZOffsets[i];
				CalculateConnections(x2, z2);
			}
		}

		[Obsolete("Use the instance function instead")]
		public static void CalculateConnections(GridNode node)
		{
			(AstarData.GetGraph(node) as GridGraph).CalculateConnections((GridNodeBase)node);
		}

		public virtual void CalculateConnections(GridNodeBase node)
		{
			int nodeInGridIndex = node.NodeInGridIndex;
			int x = nodeInGridIndex % width;
			int z = nodeInGridIndex / width;
			CalculateConnections(x, z);
		}

		[Obsolete("CalculateConnections no longer takes a node array, it just uses the one on the graph")]
		public virtual void CalculateConnections(GridNode[] nodes, int x, int z, GridNode node)
		{
			CalculateConnections(x, z);
		}

		[Obsolete("Use CalculateConnections(x,z) or CalculateConnections(node) instead")]
		public virtual void CalculateConnections(int x, int z, GridNode node)
		{
			CalculateConnections(x, z);
		}

		public virtual void CalculateConnections(int x, int z)
		{
			GridNode gridNode = nodes[z * width + x];
			if (!gridNode.Walkable)
			{
				gridNode.ResetConnectionsInternal();
				return;
			}
			int nodeInGridIndex = gridNode.NodeInGridIndex;
			if (neighbours == NumNeighbours.Four || neighbours == NumNeighbours.Eight)
			{
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					int num2 = x + neighbourXOffsets[i];
					int num3 = z + neighbourZOffsets[i];
					if ((num2 >= 0 && num3 >= 0) & (num2 < width) & (num3 < depth))
					{
						GridNode node = nodes[nodeInGridIndex + neighbourOffsets[i]];
						if (IsValidConnection(gridNode, node))
						{
							num |= 1 << i;
						}
					}
				}
				int num4 = 0;
				if (neighbours == NumNeighbours.Eight)
				{
					if (cutCorners)
					{
						for (int j = 0; j < 4; j++)
						{
							if ((((num >> j) | (num >> j + 1) | (num >> j + 1 - 4)) & 1) == 0)
							{
								continue;
							}
							int num5 = j + 4;
							int num6 = x + neighbourXOffsets[num5];
							int num7 = z + neighbourZOffsets[num5];
							if ((num6 >= 0 && num7 >= 0) & (num6 < width) & (num7 < depth))
							{
								GridNode node2 = nodes[nodeInGridIndex + neighbourOffsets[num5]];
								if (IsValidConnection(gridNode, node2))
								{
									num4 |= 1 << num5;
								}
							}
						}
					}
					else
					{
						for (int k = 0; k < 4; k++)
						{
							if (((uint)(num >> k) & (true ? 1u : 0u)) != 0 && ((uint)((num >> k + 1) | (num >> k + 1 - 4)) & (true ? 1u : 0u)) != 0)
							{
								GridNode node3 = nodes[nodeInGridIndex + neighbourOffsets[k + 4]];
								if (IsValidConnection(gridNode, node3))
								{
									num4 |= 1 << k + 4;
								}
							}
						}
					}
				}
				gridNode.SetAllConnectionInternal(num | num4);
				return;
			}
			gridNode.ResetConnectionsInternal();
			for (int l = 0; l < hexagonNeighbourIndices.Length; l++)
			{
				int num8 = hexagonNeighbourIndices[l];
				int num9 = x + neighbourXOffsets[num8];
				int num10 = z + neighbourZOffsets[num8];
				if ((num9 >= 0 && num10 >= 0) & (num9 < width) & (num10 < depth))
				{
					GridNode node4 = nodes[nodeInGridIndex + neighbourOffsets[num8]];
					gridNode.SetConnectionInternal(num8, IsValidConnection(gridNode, node4));
				}
			}
		}

		public override void OnDrawGizmos(RetainedGizmos gizmos, bool drawNodes)
		{
			using (GraphGizmoHelper graphGizmoHelper = gizmos.GetSingleFrameGizmoHelper(active))
			{
				int num;
				int num2;
				float num3;
				CalculateDimensions(out num, out num2, out num3);
				Bounds bounds = default(Bounds);
				bounds.SetMinMax(Vector3.zero, new Vector3(num, 0f, num2));
				GraphTransform graphTransform = CalculateTransform();
				graphGizmoHelper.builder.DrawWireCube(graphTransform, bounds, Color.white);
				int num4 = ((nodes == null) ? (-1) : nodes.Length);
				if (this is LayerGridGraph)
				{
					num4 = (((this as LayerGridGraph).nodes == null) ? (-1) : (this as LayerGridGraph).nodes.Length);
				}
				if (drawNodes && width * depth * LayerCount != num4)
				{
					Color color = new Color(1f, 1f, 1f, 0.2f);
					for (int i = 0; i < num2; i++)
					{
						graphGizmoHelper.builder.DrawLine(graphTransform.Transform(new Vector3(0f, 0f, i)), graphTransform.Transform(new Vector3(num, 0f, i)), color);
					}
					for (int j = 0; j < num; j++)
					{
						graphGizmoHelper.builder.DrawLine(graphTransform.Transform(new Vector3(j, 0f, 0f)), graphTransform.Transform(new Vector3(j, 0f, num2)), color);
					}
				}
			}
			if (!drawNodes)
			{
				return;
			}
			GridNodeBase[] array = ArrayPool<GridNodeBase>.Claim(1024 * LayerCount);
			for (int num5 = width / 32; num5 >= 0; num5--)
			{
				for (int num6 = depth / 32; num6 >= 0; num6--)
				{
					int nodesInRegion = GetNodesInRegion(new IntRect(num5 * 32, num6 * 32, (num5 + 1) * 32 - 1, (num6 + 1) * 32 - 1), array);
					RetainedGizmos.Hasher hasher = new RetainedGizmos.Hasher(active);
					hasher.AddHash(showMeshOutline ? 1 : 0);
					hasher.AddHash(showMeshSurface ? 1 : 0);
					hasher.AddHash(showNodeConnections ? 1 : 0);
					for (int k = 0; k < nodesInRegion; k++)
					{
						hasher.HashNode(array[k]);
					}
					if (!gizmos.Draw(hasher))
					{
						using (GraphGizmoHelper graphGizmoHelper2 = gizmos.GetGizmoHelper(active, hasher))
						{
							if (showNodeConnections)
							{
								for (int l = 0; l < nodesInRegion; l++)
								{
									if (array[l].Walkable)
									{
										graphGizmoHelper2.DrawConnections(array[l]);
									}
								}
							}
							if (showMeshSurface || showMeshOutline)
							{
								CreateNavmeshSurfaceVisualization(array, nodesInRegion, graphGizmoHelper2);
							}
						}
					}
				}
			}
			ArrayPool<GridNodeBase>.Release(ref array);
			if (active.showUnwalkableNodes)
			{
				DrawUnwalkableNodes(nodeSize * 0.3f);
			}
		}

		private void CreateNavmeshSurfaceVisualization(GridNodeBase[] nodes, int nodeCount, GraphGizmoHelper helper)
		{
			int num = 0;
			for (int i = 0; i < nodeCount; i++)
			{
				if (nodes[i].Walkable)
				{
					num++;
				}
			}
			int[] array = ((neighbours == NumNeighbours.Six) ? hexagonNeighbourIndices : new int[4] { 0, 1, 2, 3 });
			float num2 = ((neighbours != NumNeighbours.Six) ? 0.5f : 0.333333f);
			int num3 = array.Length - 2;
			int num4 = 3 * num3;
			Vector3[] array2 = ArrayPool<Vector3>.Claim(num * num4);
			Color[] array3 = ArrayPool<Color>.Claim(num * num4);
			int num5 = 0;
			for (int j = 0; j < nodeCount; j++)
			{
				GridNodeBase gridNodeBase = nodes[j];
				if (!gridNodeBase.Walkable)
				{
					continue;
				}
				Color color = helper.NodeColor(gridNodeBase);
				if (color.a <= 0.001f)
				{
					continue;
				}
				for (int k = 0; k < array.Length; k++)
				{
					int num6 = array[k];
					int num7 = array[(k + 1) % array.Length];
					GridNodeBase gridNodeBase2 = null;
					GridNodeBase neighbourAlongDirection = gridNodeBase.GetNeighbourAlongDirection(num6);
					if (neighbourAlongDirection != null && neighbours != NumNeighbours.Six)
					{
						gridNodeBase2 = neighbourAlongDirection.GetNeighbourAlongDirection(num7);
					}
					GridNodeBase neighbourAlongDirection2 = gridNodeBase.GetNeighbourAlongDirection(num7);
					if (neighbourAlongDirection2 != null && gridNodeBase2 == null && neighbours != NumNeighbours.Six)
					{
						gridNodeBase2 = neighbourAlongDirection2.GetNeighbourAlongDirection(num6);
					}
					Vector3 point = new Vector3((float)gridNodeBase.XCoordinateInGrid + 0.5f, 0f, (float)gridNodeBase.ZCoordinateInGrid + 0.5f);
					point.x += (float)(neighbourXOffsets[num6] + neighbourXOffsets[num7]) * num2;
					point.z += (float)(neighbourZOffsets[num6] + neighbourZOffsets[num7]) * num2;
					point.y += transform.InverseTransform((Vector3)gridNodeBase.position).y;
					if (neighbourAlongDirection != null)
					{
						point.y += transform.InverseTransform((Vector3)neighbourAlongDirection.position).y;
					}
					if (neighbourAlongDirection2 != null)
					{
						point.y += transform.InverseTransform((Vector3)neighbourAlongDirection2.position).y;
					}
					if (gridNodeBase2 != null)
					{
						point.y += transform.InverseTransform((Vector3)gridNodeBase2.position).y;
					}
					point.y /= 1f + ((neighbourAlongDirection == null) ? 0f : 1f) + ((neighbourAlongDirection2 == null) ? 0f : 1f) + ((gridNodeBase2 == null) ? 0f : 1f);
					point = transform.Transform(point);
					array2[num5 + k] = point;
				}
				if (neighbours == NumNeighbours.Six)
				{
					array2[num5 + 6] = array2[num5];
					array2[num5 + 7] = array2[num5 + 2];
					array2[num5 + 8] = array2[num5 + 3];
					array2[num5 + 9] = array2[num5];
					array2[num5 + 10] = array2[num5 + 3];
					array2[num5 + 11] = array2[num5 + 5];
				}
				else
				{
					array2[num5 + 4] = array2[num5];
					array2[num5 + 5] = array2[num5 + 2];
				}
				for (int l = 0; l < num4; l++)
				{
					array3[num5 + l] = color;
				}
				for (int m = 0; m < array.Length; m++)
				{
					GridNodeBase neighbourAlongDirection3 = gridNodeBase.GetNeighbourAlongDirection(array[(m + 1) % array.Length]);
					if (neighbourAlongDirection3 == null || (showMeshOutline && gridNodeBase.NodeInGridIndex < neighbourAlongDirection3.NodeInGridIndex))
					{
						helper.builder.DrawLine(array2[num5 + m], array2[num5 + (m + 1) % array.Length], (neighbourAlongDirection3 != null) ? color : Color.black);
					}
				}
				num5 += num4;
			}
			if (showMeshSurface)
			{
				helper.DrawTriangles(array2, array3, num5 * num3 / num4);
			}
			ArrayPool<Vector3>.Release(ref array2);
			ArrayPool<Color>.Release(ref array3);
		}

		protected IntRect GetRectFromBounds(Bounds bounds)
		{
			bounds = transform.InverseTransform(bounds);
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			int xmin = Mathf.RoundToInt(min.x - 0.5f);
			int xmax = Mathf.RoundToInt(max.x - 0.5f);
			int ymin = Mathf.RoundToInt(min.z - 0.5f);
			int ymax = Mathf.RoundToInt(max.z - 0.5f);
			IntRect a = new IntRect(xmin, ymin, xmax, ymax);
			IntRect b = new IntRect(0, 0, width - 1, depth - 1);
			return IntRect.Intersection(a, b);
		}

		[Obsolete("This method has been renamed to GetNodesInRegion", true)]
		public List<GraphNode> GetNodesInArea(Bounds bounds)
		{
			return GetNodesInRegion(bounds);
		}

		[Obsolete("This method has been renamed to GetNodesInRegion", true)]
		public List<GraphNode> GetNodesInArea(GraphUpdateShape shape)
		{
			return GetNodesInRegion(shape);
		}

		[Obsolete("This method has been renamed to GetNodesInRegion", true)]
		public List<GraphNode> GetNodesInArea(Bounds bounds, GraphUpdateShape shape)
		{
			return GetNodesInRegion(bounds, shape);
		}

		public List<GraphNode> GetNodesInRegion(Bounds bounds)
		{
			return GetNodesInRegion(bounds, null);
		}

		public List<GraphNode> GetNodesInRegion(GraphUpdateShape shape)
		{
			return GetNodesInRegion(shape.GetBounds(), shape);
		}

		protected virtual List<GraphNode> GetNodesInRegion(Bounds bounds, GraphUpdateShape shape)
		{
			IntRect rectFromBounds = GetRectFromBounds(bounds);
			if (nodes == null || !rectFromBounds.IsValid() || nodes.Length != width * depth)
			{
				return ListPool<GraphNode>.Claim();
			}
			List<GraphNode> list = ListPool<GraphNode>.Claim(rectFromBounds.Width * rectFromBounds.Height);
			for (int i = rectFromBounds.xmin; i <= rectFromBounds.xmax; i++)
			{
				for (int j = rectFromBounds.ymin; j <= rectFromBounds.ymax; j++)
				{
					int num = j * width + i;
					GraphNode graphNode = nodes[num];
					if (bounds.Contains((Vector3)graphNode.position) && (shape == null || shape.Contains((Vector3)graphNode.position)))
					{
						list.Add(graphNode);
					}
				}
			}
			return list;
		}

		public virtual List<GraphNode> GetNodesInRegion(IntRect rect)
		{
			rect = IntRect.Intersection(b: new IntRect(0, 0, width - 1, depth - 1), a: rect);
			if (nodes == null || !rect.IsValid() || nodes.Length != width * depth)
			{
				return ListPool<GraphNode>.Claim(0);
			}
			List<GraphNode> list = ListPool<GraphNode>.Claim(rect.Width * rect.Height);
			for (int i = rect.ymin; i <= rect.ymax; i++)
			{
				int num = i * Width;
				for (int j = rect.xmin; j <= rect.xmax; j++)
				{
					list.Add(nodes[num + j]);
				}
			}
			return list;
		}

		public virtual int GetNodesInRegion(IntRect rect, GridNodeBase[] buffer)
		{
			rect = IntRect.Intersection(b: new IntRect(0, 0, width - 1, depth - 1), a: rect);
			if (nodes == null || !rect.IsValid() || nodes.Length != width * depth)
			{
				return 0;
			}
			if (buffer.Length < rect.Width * rect.Height)
			{
				throw new ArgumentException("Buffer is too small");
			}
			int num = 0;
			int num2 = rect.ymin;
			while (num2 <= rect.ymax)
			{
				Array.Copy(nodes, num2 * Width + rect.xmin, buffer, num, rect.Width);
				num2++;
				num += rect.Width;
			}
			return num;
		}

		public virtual GridNodeBase GetNode(int x, int z)
		{
			if (x < 0 || z < 0 || x >= width || z >= depth)
			{
				return null;
			}
			return nodes[x + z * width];
		}

		GraphUpdateThreading IUpdatableGraph.CanUpdateAsync(GraphUpdateObject o)
		{
			return GraphUpdateThreading.UnityThread;
		}

		void IUpdatableGraph.UpdateAreaInit(GraphUpdateObject o)
		{
		}

		void IUpdatableGraph.UpdateAreaPost(GraphUpdateObject o)
		{
		}

		protected void CalculateAffectedRegions(GraphUpdateObject o, out IntRect originalRect, out IntRect affectRect, out IntRect physicsRect, out bool willChangeWalkability, out int erosion)
		{
			Bounds bounds = transform.InverseTransform(o.bounds);
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			int xmin = Mathf.RoundToInt(min.x - 0.5f);
			int xmax = Mathf.RoundToInt(max.x - 0.5f);
			int ymin = Mathf.RoundToInt(min.z - 0.5f);
			int ymax = Mathf.RoundToInt(max.z - 0.5f);
			originalRect = new IntRect(xmin, ymin, xmax, ymax);
			affectRect = originalRect;
			physicsRect = originalRect;
			erosion = (o.updateErosion ? erodeIterations : 0);
			willChangeWalkability = o.updatePhysics || o.modifyWalkability;
			if (o.updatePhysics && !o.modifyWalkability && collision.collisionCheck)
			{
				Vector3 vector = new Vector3(collision.diameter, 0f, collision.diameter) * 0.5f;
				min -= vector * 1.02f;
				max += vector * 1.02f;
				physicsRect = new IntRect(Mathf.RoundToInt(min.x - 0.5f), Mathf.RoundToInt(min.z - 0.5f), Mathf.RoundToInt(max.x - 0.5f), Mathf.RoundToInt(max.z - 0.5f));
				affectRect = IntRect.Union(physicsRect, affectRect);
			}
			if (willChangeWalkability || erosion > 0)
			{
				affectRect = affectRect.Expand(erosion + 1);
			}
		}

		void IUpdatableGraph.UpdateArea(GraphUpdateObject o)
		{
			if (nodes == null || nodes.Length != width * depth)
			{
				Debug.LogWarning("The Grid Graph is not scanned, cannot update area");
				return;
			}
			IntRect originalRect;
			IntRect affectRect;
			IntRect physicsRect;
			bool willChangeWalkability;
			int erosion;
			CalculateAffectedRegions(o, out originalRect, out affectRect, out physicsRect, out willChangeWalkability, out erosion);
			IntRect b = new IntRect(0, 0, width - 1, depth - 1);
			IntRect intRect = IntRect.Intersection(affectRect, b);
			for (int i = intRect.xmin; i <= intRect.xmax; i++)
			{
				for (int j = intRect.ymin; j <= intRect.ymax; j++)
				{
					o.WillUpdateNode(nodes[j * width + i]);
				}
			}
			if (o.updatePhysics && !o.modifyWalkability)
			{
				collision.Initialize(transform, nodeSize);
				intRect = IntRect.Intersection(physicsRect, b);
				for (int k = intRect.xmin; k <= intRect.xmax; k++)
				{
					for (int l = intRect.ymin; l <= intRect.ymax; l++)
					{
						RecalculateCell(k, l, o.resetPenaltyOnPhysics, false);
					}
				}
			}
			intRect = IntRect.Intersection(originalRect, b);
			for (int m = intRect.xmin; m <= intRect.xmax; m++)
			{
				for (int n = intRect.ymin; n <= intRect.ymax; n++)
				{
					int num = n * width + m;
					GridNode gridNode = nodes[num];
					if (willChangeWalkability)
					{
						gridNode.Walkable = gridNode.WalkableErosion;
						if (o.bounds.Contains((Vector3)gridNode.position))
						{
							o.Apply(gridNode);
						}
						gridNode.WalkableErosion = gridNode.Walkable;
					}
					else if (o.bounds.Contains((Vector3)gridNode.position))
					{
						o.Apply(gridNode);
					}
				}
			}
			if (willChangeWalkability && erosion == 0)
			{
				intRect = IntRect.Intersection(affectRect, b);
				for (int num2 = intRect.xmin; num2 <= intRect.xmax; num2++)
				{
					for (int num3 = intRect.ymin; num3 <= intRect.ymax; num3++)
					{
						CalculateConnections(num2, num3);
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
				for (int num4 = a2.xmin; num4 <= a2.xmax; num4++)
				{
					for (int num5 = a2.ymin; num5 <= a2.ymax; num5++)
					{
						int num6 = num5 * width + num4;
						GridNode gridNode2 = nodes[num6];
						bool walkable = gridNode2.Walkable;
						gridNode2.Walkable = gridNode2.WalkableErosion;
						if (!a.Contains(num4, num5))
						{
							gridNode2.TmpWalkable = walkable;
						}
					}
				}
				for (int num7 = a2.xmin; num7 <= a2.xmax; num7++)
				{
					for (int num8 = a2.ymin; num8 <= a2.ymax; num8++)
					{
						CalculateConnections(num7, num8);
					}
				}
				ErodeWalkableArea(a2.xmin, a2.ymin, a2.xmax + 1, a2.ymax + 1);
				for (int num9 = a2.xmin; num9 <= a2.xmax; num9++)
				{
					for (int num10 = a2.ymin; num10 <= a2.ymax; num10++)
					{
						if (!a.Contains(num9, num10))
						{
							int num11 = num10 * width + num9;
							GridNode gridNode3 = nodes[num11];
							gridNode3.Walkable = gridNode3.TmpWalkable;
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
			}
		}

		public bool Linecast(Vector3 from, Vector3 to)
		{
			GraphHitInfo hit;
			return Linecast(from, to, null, out hit);
		}

		public bool Linecast(Vector3 from, Vector3 to, GraphNode hint)
		{
			GraphHitInfo hit;
			return Linecast(from, to, hint, out hit);
		}

		public bool Linecast(Vector3 from, Vector3 to, GraphNode hint, out GraphHitInfo hit)
		{
			return Linecast(from, to, hint, out hit, null);
		}

		protected static float CrossMagnitude(Vector2 a, Vector2 b)
		{
			return a.x * b.y - b.x * a.y;
		}

		protected static long CrossMagnitude(Int2 a, Int2 b)
		{
			return (long)a.x * (long)b.y - (long)b.x * (long)a.y;
		}

		protected bool ClipLineSegmentToBounds(Vector3 a, Vector3 b, out Vector3 outA, out Vector3 outB)
		{
			if (a.x < 0f || a.z < 0f || a.x > (float)width || a.z > (float)depth || b.x < 0f || b.z < 0f || b.x > (float)width || b.z > (float)depth)
			{
				Vector3 vector = new Vector3(0f, 0f, 0f);
				Vector3 vector2 = new Vector3(0f, 0f, depth);
				Vector3 vector3 = new Vector3(width, 0f, depth);
				Vector3 vector4 = new Vector3(width, 0f, 0f);
				int num = 0;
				bool intersects;
				Vector3 vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector, vector2, out intersects);
				if (intersects)
				{
					num++;
					if (!VectorMath.RightOrColinearXZ(vector, vector2, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector2, vector3, out intersects);
				if (intersects)
				{
					num++;
					if (!VectorMath.RightOrColinearXZ(vector2, vector3, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector3, vector4, out intersects);
				if (intersects)
				{
					num++;
					if (!VectorMath.RightOrColinearXZ(vector3, vector4, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector4, vector, out intersects);
				if (intersects)
				{
					num++;
					if (!VectorMath.RightOrColinearXZ(vector4, vector, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				if (num == 0)
				{
					outA = Vector3.zero;
					outB = Vector3.zero;
					return false;
				}
			}
			outA = a;
			outB = b;
			return true;
		}

		public bool Linecast(Vector3 from, Vector3 to, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
		{
			hit = default(GraphHitInfo);
			hit.origin = from;
			Vector3 outA = transform.InverseTransform(from);
			Vector3 outB = transform.InverseTransform(to);
			if (!ClipLineSegmentToBounds(outA, outB, out outA, out outB))
			{
				hit.point = to;
				return false;
			}
			GridNodeBase gridNodeBase = GetNearest(transform.Transform(outA), NNConstraint.None).node as GridNodeBase;
			GridNodeBase gridNodeBase2 = GetNearest(transform.Transform(outB), NNConstraint.None).node as GridNodeBase;
			if (!gridNodeBase.Walkable)
			{
				hit.node = gridNodeBase;
				hit.point = transform.Transform(outA);
				hit.tangentOrigin = hit.point;
				return true;
			}
			Vector2 vector = new Vector2(outA.x - 0.5f, outA.z - 0.5f);
			Vector2 vector2 = new Vector2(outB.x - 0.5f, outB.z - 0.5f);
			if (gridNodeBase == null || gridNodeBase2 == null)
			{
				hit.node = null;
				hit.point = from;
				return true;
			}
			Vector2 a = vector2 - vector;
			float num = CrossMagnitude(b: new Vector2(Mathf.Sign(a.x), Mathf.Sign(a.y)), a: a) * 0.5f;
			int num2 = ((!(a.y >= 0f)) ? 3 : 0) ^ ((!(a.x >= 0f)) ? 1 : 0);
			int num3 = (num2 + 1) & 3;
			int num4 = (num2 + 2) & 3;
			GridNodeBase gridNodeBase3 = gridNodeBase;
			while (gridNodeBase3.NodeInGridIndex != gridNodeBase2.NodeInGridIndex)
			{
				if (trace != null)
				{
					trace.Add(gridNodeBase3);
				}
				Vector2 vector3 = new Vector2(gridNodeBase3.XCoordinateInGrid, gridNodeBase3.ZCoordinateInGrid);
				float num5 = CrossMagnitude(a, vector3 - vector);
				float num6 = num5 + num;
				int num7 = ((!(num6 < 0f)) ? num3 : num4);
				GridNodeBase neighbourAlongDirection = gridNodeBase3.GetNeighbourAlongDirection(num7);
				if (neighbourAlongDirection != null)
				{
					gridNodeBase3 = neighbourAlongDirection;
					continue;
				}
				Vector2 vector4 = new Vector2(neighbourXOffsets[num7], neighbourZOffsets[num7]);
				Vector2 vector5 = new Vector2(neighbourXOffsets[(num7 - 1 + 4) & 3], neighbourZOffsets[(num7 - 1 + 4) & 3]);
				Vector2 vector6 = new Vector2(neighbourXOffsets[(num7 + 1) & 3], neighbourZOffsets[(num7 + 1) & 3]);
				Vector2 vector7 = vector3 + (vector4 + vector5) * 0.5f;
				Vector2 vector8 = VectorMath.LineIntersectionPoint(vector7, vector7 + vector6, vector, vector2);
				Vector3 vector9 = transform.InverseTransform((Vector3)gridNodeBase3.position);
				Vector3 point = new Vector3(vector8.x + 0.5f, vector9.y, vector8.y + 0.5f);
				Vector3 point2 = new Vector3(vector7.x + 0.5f, vector9.y, vector7.y + 0.5f);
				hit.point = transform.Transform(point);
				hit.tangentOrigin = transform.Transform(point2);
				hit.tangent = transform.TransformVector(new Vector3(vector6.x, 0f, vector6.y));
				hit.node = gridNodeBase3;
				return true;
			}
			if (trace != null)
			{
				trace.Add(gridNodeBase3);
			}
			if (gridNodeBase3 == gridNodeBase2)
			{
				hit.point = to;
				hit.node = gridNodeBase3;
				return false;
			}
			hit.point = (Vector3)gridNodeBase3.position;
			hit.tangentOrigin = hit.point;
			return true;
		}

		public bool SnappedLinecast(Vector3 from, Vector3 to, GraphNode hint, out GraphHitInfo hit)
		{
			return Linecast((Vector3)GetNearest(from, NNConstraint.None).node.position, (Vector3)GetNearest(to, NNConstraint.None).node.position, hint, out hit);
		}

		public bool Linecast(GridNodeBase fromNode, GridNodeBase toNode)
		{
			Int2 a = new Int2(toNode.XCoordinateInGrid - fromNode.XCoordinateInGrid, toNode.ZCoordinateInGrid - fromNode.ZCoordinateInGrid);
			long num = CrossMagnitude(a, new Int2(Math.Sign(a.x), Math.Sign(a.y)));
			int num2 = 0;
			if (a.x <= 0 && a.y > 0)
			{
				num2 = 1;
			}
			else if (a.x < 0 && a.y <= 0)
			{
				num2 = 2;
			}
			else if (a.x >= 0 && a.y < 0)
			{
				num2 = 3;
			}
			int num3 = (num2 + 1) & 3;
			int num4 = (num2 + 2) & 3;
			int num5 = ((a.x == 0 || a.y == 0) ? (-1) : (4 + ((num2 + 1) & 3)));
			Int2 b = new Int2(0, 0);
			while (fromNode != null && fromNode.NodeInGridIndex != toNode.NodeInGridIndex)
			{
				long num6 = CrossMagnitude(a, b) * 2;
				long num7 = num6 + num;
				int num8 = ((num7 >= 0) ? num3 : num4);
				if (num7 == 0 && num5 != -1)
				{
					num8 = num5;
				}
				fromNode = fromNode.GetNeighbourAlongDirection(num8);
				b += new Int2(neighbourXOffsets[num8], neighbourZOffsets[num8]);
			}
			return fromNode != toNode;
		}

		public bool CheckConnection(GridNode node, int dir)
		{
			if (neighbours == NumNeighbours.Eight || neighbours == NumNeighbours.Six || dir < 4)
			{
				return HasNodeConnection(node, dir);
			}
			int num = (dir - 4 - 1) & 3;
			int num2 = (dir - 4 + 1) & 3;
			if (!HasNodeConnection(node, num) || !HasNodeConnection(node, num2))
			{
				return false;
			}
			GridNode gridNode = nodes[node.NodeInGridIndex + neighbourOffsets[num]];
			GridNode gridNode2 = nodes[node.NodeInGridIndex + neighbourOffsets[num2]];
			if (!gridNode.Walkable || !gridNode2.Walkable)
			{
				return false;
			}
			if (!HasNodeConnection(gridNode2, num) || !HasNodeConnection(gridNode, num2))
			{
				return false;
			}
			return true;
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
			nodes = new GridNode[num];
			for (int i = 0; i < nodes.Length; i++)
			{
				nodes[i] = new GridNode(active);
				nodes[i].DeserializeNode(ctx);
			}
		}

		protected override void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			base.DeserializeSettingsCompatibility(ctx);
			aspectRatio = ctx.reader.ReadSingle();
			rotation = ctx.DeserializeVector3();
			center = ctx.DeserializeVector3();
			unclampedSize = ctx.DeserializeVector3();
			nodeSize = ctx.reader.ReadSingle();
			collision.DeserializeSettingsCompatibility(ctx);
			maxClimb = ctx.reader.ReadSingle();
			ctx.reader.ReadInt32();
			maxSlope = ctx.reader.ReadSingle();
			erodeIterations = ctx.reader.ReadInt32();
			erosionUseTags = ctx.reader.ReadBoolean();
			erosionFirstTag = ctx.reader.ReadInt32();
			ctx.reader.ReadBoolean();
			neighbours = (NumNeighbours)ctx.reader.ReadInt32();
			cutCorners = ctx.reader.ReadBoolean();
			penaltyPosition = ctx.reader.ReadBoolean();
			penaltyPositionFactor = ctx.reader.ReadSingle();
			penaltyAngle = ctx.reader.ReadBoolean();
			penaltyAngleFactor = ctx.reader.ReadSingle();
			penaltyAnglePower = ctx.reader.ReadSingle();
			isometricAngle = ctx.reader.ReadSingle();
			uniformEdgeCosts = ctx.reader.ReadBoolean();
			useJumpPointSearch = ctx.reader.ReadBoolean();
		}

		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			UpdateTransform();
			SetUpOffsetsAndCosts();
			GridNode.SetGridGraph((int)graphIndex, this);
			if (nodes == null || nodes.Length == 0)
			{
				return;
			}
			if (width * depth != nodes.Length)
			{
				Debug.LogError("Node data did not match with bounds data. Probably a change to the bounds/width/depth data was made after scanning the graph just prior to saving it. Nodes will be discarded");
				nodes = new GridNode[0];
				return;
			}
			for (int i = 0; i < depth; i++)
			{
				for (int j = 0; j < width; j++)
				{
					GridNode gridNode = nodes[i * width + j];
					if (gridNode == null)
					{
						Debug.LogError("Deserialization Error : Couldn't cast the node to the appropriate type - GridGenerator");
						return;
					}
					gridNode.NodeInGridIndex = i * width + j;
				}
			}
		}
	}
}
