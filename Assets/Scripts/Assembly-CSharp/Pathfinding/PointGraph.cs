using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[JsonOptIn]
	public class PointGraph : NavGraph, IUpdatableGraph
	{
		[JsonMember]
		public Transform root;

		[JsonMember]
		public string searchTag;

		[JsonMember]
		public float maxDistance;

		[JsonMember]
		public Vector3 limits;

		[JsonMember]
		public bool raycast = true;

		[JsonMember]
		public bool use2DPhysics;

		[JsonMember]
		public bool thickRaycast;

		[JsonMember]
		public float thickRaycastRadius = 1f;

		[JsonMember]
		public bool recursive = true;

		[JsonMember]
		public LayerMask mask;

		[JsonMember]
		public bool optimizeForSparseGraph;

		private PointKDTree lookupTree = new PointKDTree();

		public PointNode[] nodes;

		public int nodeCount { get; protected set; }

		public override int CountNodes()
		{
			return nodeCount;
		}

		public override void GetNodes(Action<GraphNode> action)
		{
			if (nodes != null)
			{
				int num = nodeCount;
				for (int i = 0; i < num; i++)
				{
					action(nodes[i]);
				}
			}
		}

		public override NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
		{
			return GetNearestInternal(position, constraint, true);
		}

		public override NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
		{
			return GetNearestInternal(position, constraint, false);
		}

		private NNInfoInternal GetNearestInternal(Vector3 position, NNConstraint constraint, bool fastCheck)
		{
			if (nodes == null)
			{
				return default(NNInfoInternal);
			}
			if (optimizeForSparseGraph)
			{
				return new NNInfoInternal(lookupTree.GetNearest((Int3)position, (!fastCheck) ? constraint : null));
			}
			float num = ((constraint != null && !constraint.constrainDistance) ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistanceSqr);
			NNInfoInternal result = new NNInfoInternal(null);
			float num2 = float.PositiveInfinity;
			float num3 = float.PositiveInfinity;
			for (int i = 0; i < nodeCount; i++)
			{
				PointNode pointNode = nodes[i];
				float sqrMagnitude = (position - (Vector3)pointNode.position).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					num2 = sqrMagnitude;
					result.node = pointNode;
				}
				if (sqrMagnitude < num3 && sqrMagnitude < num && (constraint == null || constraint.Suitable(pointNode)))
				{
					num3 = sqrMagnitude;
					result.constrainedNode = pointNode;
				}
			}
			if (!fastCheck)
			{
				result.node = result.constrainedNode;
			}
			result.UpdateInfo();
			return result;
		}

		public PointNode AddNode(Int3 position)
		{
			return AddNode(new PointNode(active), position);
		}

		public T AddNode<T>(T node, Int3 position) where T : PointNode
		{
			if (nodes == null || nodeCount == nodes.Length)
			{
				PointNode[] array = new PointNode[(nodes == null) ? 4 : Math.Max(nodes.Length + 4, nodes.Length * 2)];
				if (nodes != null)
				{
					nodes.CopyTo(array, 0);
				}
				nodes = array;
			}
			node.SetPosition(position);
			node.GraphIndex = graphIndex;
			node.Walkable = true;
			nodes[nodeCount] = node;
			nodeCount++;
			if (optimizeForSparseGraph)
			{
				AddToLookup(node);
			}
			return node;
		}

		protected static int CountChildren(Transform tr)
		{
			int num = 0;
			foreach (Transform item in tr)
			{
				num++;
				num += CountChildren(item);
			}
			return num;
		}

		protected void AddChildren(ref int c, Transform tr)
		{
			foreach (Transform item in tr)
			{
				nodes[c].position = (Int3)item.position;
				nodes[c].Walkable = true;
				nodes[c].gameObject = item.gameObject;
				c++;
				AddChildren(ref c, item);
			}
		}

		public void RebuildNodeLookup()
		{
			if (!optimizeForSparseGraph || nodes == null)
			{
				lookupTree = new PointKDTree();
			}
			else
			{
				lookupTree.Rebuild(nodes, 0, nodeCount);
			}
		}

		private void AddToLookup(PointNode node)
		{
			lookupTree.Add(node);
		}

		protected virtual PointNode[] CreateNodes(int count)
		{
			PointNode[] array = new PointNode[count];
			for (int i = 0; i < nodeCount; i++)
			{
				array[i] = new PointNode(active);
			}
			return array;
		}

		protected override IEnumerable<Progress> ScanInternal()
		{
			yield return new Progress(0f, "Searching for GameObjects");
			if (root == null)
			{
				GameObject[] gos = ((searchTag == null) ? null : GameObject.FindGameObjectsWithTag(searchTag));
				if (gos == null)
				{
					nodes = new PointNode[0];
					nodeCount = 0;
					yield break;
				}
				yield return new Progress(0.1f, "Creating nodes");
				nodeCount = gos.Length;
				nodes = CreateNodes(nodeCount);
				for (int i = 0; i < gos.Length; i++)
				{
					nodes[i].position = (Int3)gos[i].transform.position;
					nodes[i].Walkable = true;
					nodes[i].gameObject = gos[i].gameObject;
				}
			}
			else if (!recursive)
			{
				nodeCount = root.childCount;
				nodes = CreateNodes(nodeCount);
				int num = 0;
				foreach (Transform item in root)
				{
					nodes[num].position = (Int3)item.position;
					nodes[num].Walkable = true;
					nodes[num].gameObject = item.gameObject;
					num++;
				}
			}
			else
			{
				nodeCount = CountChildren(root);
				nodes = CreateNodes(nodeCount);
				int c = 0;
				AddChildren(ref c, root);
			}
			if (optimizeForSparseGraph)
			{
				yield return new Progress(0.15f, "Building node lookup");
				RebuildNodeLookup();
			}
			foreach (Progress item2 in ConnectNodesAsync())
			{
				yield return item2.MapTo(0.16f, 1f);
			}
		}

		public void ConnectNodes()
		{
			foreach (Progress item in ConnectNodesAsync())
			{
			}
		}

		private IEnumerable<Progress> ConnectNodesAsync()
		{
			if (!(maxDistance >= 0f))
			{
				yield break;
			}
			List<Connection> connections = new List<Connection>();
			List<GraphNode> candidateConnections = new List<GraphNode>();
			long maxSquaredRange2;
			if (maxDistance == 0f && (limits.x == 0f || limits.y == 0f || limits.z == 0f))
			{
				maxSquaredRange2 = long.MaxValue;
			}
			else
			{
				maxSquaredRange2 = (long)(Mathf.Max(limits.x, Mathf.Max(limits.y, Mathf.Max(limits.z, maxDistance))) * 1000f) + 1;
				maxSquaredRange2 *= maxSquaredRange2;
			}
			for (int i = 0; i < nodeCount; i++)
			{
				if (i % 512 == 0)
				{
					yield return new Progress((float)i / (float)nodes.Length, "Connecting nodes");
				}
				connections.Clear();
				PointNode node = nodes[i];
				if (optimizeForSparseGraph)
				{
					candidateConnections.Clear();
					lookupTree.GetInRange(node.position, maxSquaredRange2, candidateConnections);
					for (int j = 0; j < candidateConnections.Count; j++)
					{
						PointNode pointNode = candidateConnections[j] as PointNode;
						float dist;
						if (pointNode != node && IsValidConnection(node, pointNode, out dist))
						{
							connections.Add(new Connection(pointNode, (uint)Mathf.RoundToInt(dist * 1000f)));
						}
					}
				}
				else
				{
					for (int k = 0; k < nodeCount; k++)
					{
						if (i != k)
						{
							PointNode pointNode2 = nodes[k];
							float dist2;
							if (IsValidConnection(node, pointNode2, out dist2))
							{
								connections.Add(new Connection(pointNode2, (uint)Mathf.RoundToInt(dist2 * 1000f)));
							}
						}
					}
				}
				node.connections = connections.ToArray();
			}
		}

		public virtual bool IsValidConnection(GraphNode a, GraphNode b, out float dist)
		{
			dist = 0f;
			if (!a.Walkable || !b.Walkable)
			{
				return false;
			}
			Vector3 vector = (Vector3)(b.position - a.position);
			if ((!Mathf.Approximately(limits.x, 0f) && Mathf.Abs(vector.x) > limits.x) || (!Mathf.Approximately(limits.y, 0f) && Mathf.Abs(vector.y) > limits.y) || (!Mathf.Approximately(limits.z, 0f) && Mathf.Abs(vector.z) > limits.z))
			{
				return false;
			}
			dist = vector.magnitude;
			if (maxDistance == 0f || dist < maxDistance)
			{
				if (raycast)
				{
					Ray ray = new Ray((Vector3)a.position, vector);
					Ray ray2 = new Ray((Vector3)b.position, -vector);
					if (use2DPhysics)
					{
						if (thickRaycast)
						{
							return !Physics2D.CircleCast(ray.origin, thickRaycastRadius, ray.direction, dist, mask) && !Physics2D.CircleCast(ray2.origin, thickRaycastRadius, ray2.direction, dist, mask);
						}
						return !Physics2D.Linecast((Vector3)a.position, (Vector3)b.position, mask) && !Physics2D.Linecast((Vector3)b.position, (Vector3)a.position, mask);
					}
					if (thickRaycast)
					{
						return !Physics.SphereCast(ray, thickRaycastRadius, dist, mask) && !Physics.SphereCast(ray2, thickRaycastRadius, dist, mask);
					}
					return !Physics.Linecast((Vector3)a.position, (Vector3)b.position, mask) && !Physics.Linecast((Vector3)b.position, (Vector3)a.position, mask);
				}
				return true;
			}
			return false;
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

		void IUpdatableGraph.UpdateArea(GraphUpdateObject guo)
		{
			if (nodes == null)
			{
				return;
			}
			for (int i = 0; i < nodeCount; i++)
			{
				PointNode pointNode = nodes[i];
				if (guo.bounds.Contains((Vector3)pointNode.position))
				{
					guo.WillUpdateNode(pointNode);
					guo.Apply(pointNode);
				}
			}
			if (!guo.updatePhysics)
			{
				return;
			}
			Bounds bounds = guo.bounds;
			if (thickRaycast)
			{
				bounds.Expand(thickRaycastRadius * 2f);
			}
			List<Connection> list = ListPool<Connection>.Claim();
			for (int j = 0; j < nodeCount; j++)
			{
				PointNode pointNode2 = nodes[j];
				Vector3 a = (Vector3)pointNode2.position;
				List<Connection> list2 = null;
				for (int k = 0; k < nodeCount; k++)
				{
					if (k == j)
					{
						continue;
					}
					Vector3 b = (Vector3)nodes[k].position;
					if (!VectorMath.SegmentIntersectsBounds(bounds, a, b))
					{
						continue;
					}
					PointNode pointNode3 = nodes[k];
					bool flag = pointNode2.ContainsConnection(pointNode3);
					float dist;
					bool flag2 = IsValidConnection(pointNode2, pointNode3, out dist);
					if (list2 == null && flag != flag2)
					{
						list.Clear();
						list2 = list;
						list2.AddRange(pointNode2.connections);
					}
					if (!flag && flag2)
					{
						uint cost = (uint)Mathf.RoundToInt(dist * 1000f);
						list2.Add(new Connection(pointNode3, cost));
					}
					else
					{
						if (!flag || flag2)
						{
							continue;
						}
						for (int l = 0; l < list2.Count; l++)
						{
							if (list2[l].node == pointNode3)
							{
								list2.RemoveAt(l);
								break;
							}
						}
					}
				}
				if (list2 != null)
				{
					pointNode2.connections = list2.ToArray();
				}
			}
			ListPool<Connection>.Release(ref list);
		}

		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			RebuildNodeLookup();
		}

		public override void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			base.RelocateNodes(deltaMatrix);
			RebuildNodeLookup();
		}

		protected override void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			base.DeserializeSettingsCompatibility(ctx);
			root = ctx.DeserializeUnityObject() as Transform;
			searchTag = ctx.reader.ReadString();
			maxDistance = ctx.reader.ReadSingle();
			limits = ctx.DeserializeVector3();
			raycast = ctx.reader.ReadBoolean();
			use2DPhysics = ctx.reader.ReadBoolean();
			thickRaycast = ctx.reader.ReadBoolean();
			thickRaycastRadius = ctx.reader.ReadSingle();
			recursive = ctx.reader.ReadBoolean();
			ctx.reader.ReadBoolean();
			mask = ctx.reader.ReadInt32();
			optimizeForSparseGraph = ctx.reader.ReadBoolean();
			ctx.reader.ReadBoolean();
		}

		protected override void SerializeExtraInfo(GraphSerializationContext ctx)
		{
			if (nodes == null)
			{
				ctx.writer.Write(-1);
			}
			ctx.writer.Write(nodeCount);
			for (int i = 0; i < nodeCount; i++)
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
			nodes = new PointNode[num];
			nodeCount = num;
			for (int i = 0; i < nodes.Length; i++)
			{
				if (ctx.reader.ReadInt32() != -1)
				{
					nodes[i] = new PointNode(active);
					nodes[i].DeserializeNode(ctx);
				}
			}
		}
	}
}
