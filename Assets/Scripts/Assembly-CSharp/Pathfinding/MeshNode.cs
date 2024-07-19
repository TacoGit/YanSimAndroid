using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public abstract class MeshNode : GraphNode
	{
		public Connection[] connections;

		protected MeshNode(AstarPath astar)
			: base(astar)
		{
		}

		public abstract Int3 GetVertex(int i);

		public abstract int GetVertexCount();

		public abstract Vector3 ClosestPointOnNode(Vector3 p);

		public abstract Vector3 ClosestPointOnNodeXZ(Vector3 p);

		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse && connections != null)
			{
				for (int i = 0; i < connections.Length; i++)
				{
					if (connections[i].node != null)
					{
						connections[i].node.RemoveConnection(this);
					}
				}
			}
			ArrayPool<Connection>.Release(ref connections, true);
		}

		public override void GetConnections(Action<GraphNode> action)
		{
			if (connections != null)
			{
				for (int i = 0; i < connections.Length; i++)
				{
					action(connections[i].node);
				}
			}
		}

		public override void FloodFill(Stack<GraphNode> stack, uint region)
		{
			if (connections == null)
			{
				return;
			}
			for (int i = 0; i < connections.Length; i++)
			{
				GraphNode node = connections[i].node;
				if (node.Area != region)
				{
					node.Area = region;
					stack.Push(node);
				}
			}
		}

		public override bool ContainsConnection(GraphNode node)
		{
			for (int i = 0; i < connections.Length; i++)
			{
				if (connections[i].node == node)
				{
					return true;
				}
			}
			return false;
		}

		public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			pathNode.UpdateG(path);
			handler.heap.Add(pathNode);
			for (int i = 0; i < connections.Length; i++)
			{
				GraphNode node = connections[i].node;
				PathNode pathNode2 = handler.GetPathNode(node);
				if (pathNode2.parent == pathNode && pathNode2.pathID == handler.PathID)
				{
					node.UpdateRecursiveG(path, pathNode2, handler);
				}
			}
		}

		public override void AddConnection(GraphNode node, uint cost)
		{
			AddConnection(node, cost, -1);
		}

		public void AddConnection(GraphNode node, uint cost, int shapeEdge)
		{
			if (node == null)
			{
				throw new ArgumentNullException();
			}
			if (connections != null)
			{
				for (int i = 0; i < connections.Length; i++)
				{
					if (connections[i].node == node)
					{
						connections[i].cost = cost;
						connections[i].shapeEdge = ((shapeEdge < 0) ? connections[i].shapeEdge : ((byte)shapeEdge));
						return;
					}
				}
			}
			int num = ((connections != null) ? connections.Length : 0);
			Connection[] array = ArrayPool<Connection>.ClaimWithExactLength(num + 1);
			for (int j = 0; j < num; j++)
			{
				array[j] = connections[j];
			}
			array[num] = new Connection(node, cost, (byte)shapeEdge);
			if (connections != null)
			{
				ArrayPool<Connection>.Release(ref connections, true);
			}
			connections = array;
		}

		public override void RemoveConnection(GraphNode node)
		{
			if (connections == null)
			{
				return;
			}
			for (int i = 0; i < connections.Length; i++)
			{
				if (connections[i].node == node)
				{
					int num = connections.Length;
					Connection[] array = ArrayPool<Connection>.ClaimWithExactLength(num - 1);
					for (int j = 0; j < i; j++)
					{
						array[j] = connections[j];
					}
					for (int k = i + 1; k < num; k++)
					{
						array[k - 1] = connections[k];
					}
					if (connections != null)
					{
						ArrayPool<Connection>.Release(ref connections, true);
					}
					connections = array;
					break;
				}
			}
		}

		public virtual bool ContainsPoint(Int3 point)
		{
			return ContainsPoint((Vector3)point);
		}

		public abstract bool ContainsPoint(Vector3 point);

		public abstract bool ContainsPointInGraphSpace(Int3 point);

		public override int GetGizmoHashCode()
		{
			int num = base.GetGizmoHashCode();
			if (connections != null)
			{
				for (int i = 0; i < connections.Length; i++)
				{
					num ^= 17 * connections[i].GetHashCode();
				}
			}
			return num;
		}

		public override void SerializeReferences(GraphSerializationContext ctx)
		{
			if (connections == null)
			{
				ctx.writer.Write(-1);
				return;
			}
			ctx.writer.Write(connections.Length);
			for (int i = 0; i < connections.Length; i++)
			{
				ctx.SerializeNodeReference(connections[i].node);
				ctx.writer.Write(connections[i].cost);
				ctx.writer.Write(connections[i].shapeEdge);
			}
		}

		public override void DeserializeReferences(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				connections = null;
				return;
			}
			connections = ArrayPool<Connection>.ClaimWithExactLength(num);
			for (int i = 0; i < num; i++)
			{
				connections[i] = new Connection(ctx.DeserializeNodeReference(), ctx.reader.ReadUInt32(), (!(ctx.meta.version < AstarSerializer.V4_1_0)) ? ctx.reader.ReadByte() : byte.MaxValue);
			}
		}
	}
}
