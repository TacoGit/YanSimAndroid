using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	public class TriangleMeshNode : MeshNode
	{
		public int v0;

		public int v1;

		public int v2;

		protected static INavmeshHolder[] _navmeshHolders = new INavmeshHolder[0];

		protected static readonly object lockObject = new object();

		public TriangleMeshNode(AstarPath astar)
			: base(astar)
		{
		}

		public static INavmeshHolder GetNavmeshHolder(uint graphIndex)
		{
			return _navmeshHolders[graphIndex];
		}

		public static void SetNavmeshHolder(int graphIndex, INavmeshHolder graph)
		{
			lock (lockObject)
			{
				if (graphIndex >= _navmeshHolders.Length)
				{
					INavmeshHolder[] array = new INavmeshHolder[graphIndex + 1];
					_navmeshHolders.CopyTo(array, 0);
					_navmeshHolders = array;
				}
				_navmeshHolders[graphIndex] = graph;
			}
		}

		public void UpdatePositionFromVertices()
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			GetVertices(out @int, out int2, out int3);
			position = (@int + int2 + int3) * 0.333333f;
		}

		public int GetVertexIndex(int i)
		{
			int result;
			switch (i)
			{
			case 0:
				result = v0;
				break;
			case 1:
				result = v1;
				break;
			default:
				result = v2;
				break;
			}
			return result;
		}

		public int GetVertexArrayIndex(int i)
		{
			INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
			int index;
			switch (i)
			{
			case 0:
				index = v0;
				break;
			case 1:
				index = v1;
				break;
			default:
				index = v2;
				break;
			}
			return navmeshHolder.GetVertexArrayIndex(index);
		}

		public void GetVertices(out Int3 v0, out Int3 v1, out Int3 v2)
		{
			INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
			v0 = navmeshHolder.GetVertex(this.v0);
			v1 = navmeshHolder.GetVertex(this.v1);
			v2 = navmeshHolder.GetVertex(this.v2);
		}

		public void GetVerticesInGraphSpace(out Int3 v0, out Int3 v1, out Int3 v2)
		{
			INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
			v0 = navmeshHolder.GetVertexInGraphSpace(this.v0);
			v1 = navmeshHolder.GetVertexInGraphSpace(this.v1);
			v2 = navmeshHolder.GetVertexInGraphSpace(this.v2);
		}

		public override Int3 GetVertex(int i)
		{
			return GetNavmeshHolder(base.GraphIndex).GetVertex(GetVertexIndex(i));
		}

		public Int3 GetVertexInGraphSpace(int i)
		{
			return GetNavmeshHolder(base.GraphIndex).GetVertexInGraphSpace(GetVertexIndex(i));
		}

		public override int GetVertexCount()
		{
			return 3;
		}

		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			GetVertices(out @int, out int2, out int3);
			return Polygon.ClosestPointOnTriangle((Vector3)@int, (Vector3)int2, (Vector3)int3, p);
		}

		internal Int3 ClosestPointOnNodeXZInGraphSpace(Vector3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			GetVerticesInGraphSpace(out @int, out int2, out int3);
			p = GetNavmeshHolder(base.GraphIndex).transform.InverseTransform(p);
			Vector3 vector = Polygon.ClosestPointOnTriangleXZ((Vector3)@int, (Vector3)int2, (Vector3)int3, p);
			Int3 int4 = (Int3)vector;
			if (ContainsPointInGraphSpace(int4))
			{
				return int4;
			}
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if (i != 0 || j != 0)
					{
						Int3 int5 = new Int3(int4.x + i, int4.y, int4.z + j);
						if (ContainsPointInGraphSpace(int5))
						{
							return int5;
						}
					}
				}
			}
			long sqrMagnitudeLong = (@int - int4).sqrMagnitudeLong;
			long sqrMagnitudeLong2 = (int2 - int4).sqrMagnitudeLong;
			long sqrMagnitudeLong3 = (int3 - int4).sqrMagnitudeLong;
			return (sqrMagnitudeLong < sqrMagnitudeLong2) ? ((sqrMagnitudeLong >= sqrMagnitudeLong3) ? int3 : @int) : ((sqrMagnitudeLong2 >= sqrMagnitudeLong3) ? int3 : int2);
		}

		public override Vector3 ClosestPointOnNodeXZ(Vector3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			GetVertices(out @int, out int2, out int3);
			return Polygon.ClosestPointOnTriangleXZ((Vector3)@int, (Vector3)int2, (Vector3)int3, p);
		}

		public override bool ContainsPoint(Vector3 p)
		{
			return ContainsPointInGraphSpace((Int3)GetNavmeshHolder(base.GraphIndex).transform.InverseTransform(p));
		}

		public override bool ContainsPointInGraphSpace(Int3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			GetVerticesInGraphSpace(out @int, out int2, out int3);
			if ((long)(int2.x - @int.x) * (long)(p.z - @int.z) - (long)(p.x - @int.x) * (long)(int2.z - @int.z) > 0)
			{
				return false;
			}
			if ((long)(int3.x - int2.x) * (long)(p.z - int2.z) - (long)(p.x - int2.x) * (long)(int3.z - int2.z) > 0)
			{
				return false;
			}
			if ((long)(@int.x - int3.x) * (long)(p.z - int3.z) - (long)(p.x - int3.x) * (long)(@int.z - int3.z) > 0)
			{
				return false;
			}
			return true;
		}

		public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			pathNode.UpdateG(path);
			handler.heap.Add(pathNode);
			if (connections == null)
			{
				return;
			}
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

		public override void Open(Path path, PathNode pathNode, PathHandler handler)
		{
			if (connections == null)
			{
				return;
			}
			bool flag = pathNode.flag2;
			for (int num = connections.Length - 1; num >= 0; num--)
			{
				Connection connection = connections[num];
				GraphNode node = connection.node;
				if (path.CanTraverse(connection.node))
				{
					PathNode pathNode2 = handler.GetPathNode(connection.node);
					if (pathNode2 != pathNode.parent)
					{
						uint num2 = connection.cost;
						if (flag || pathNode2.flag2)
						{
							num2 = path.GetConnectionSpecialCost(this, connection.node, num2);
						}
						if (pathNode2.pathID != handler.PathID)
						{
							pathNode2.node = connection.node;
							pathNode2.parent = pathNode;
							pathNode2.pathID = handler.PathID;
							pathNode2.cost = num2;
							pathNode2.H = path.CalculateHScore(node);
							pathNode2.UpdateG(path);
							handler.heap.Add(pathNode2);
						}
						else if (pathNode.G + num2 + path.GetTraversalCost(node) < pathNode2.G)
						{
							pathNode2.cost = num2;
							pathNode2.parent = pathNode;
							node.UpdateRecursiveG(path, pathNode2, handler);
						}
					}
				}
			}
		}

		public int SharedEdge(GraphNode other)
		{
			int result = -1;
			for (int i = 0; i < connections.Length; i++)
			{
				if (connections[i].node == other)
				{
					result = connections[i].shapeEdge;
				}
			}
			return result;
		}

		public override bool GetPortal(GraphNode toNode, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			int aIndex;
			int bIndex;
			return GetPortal(toNode, left, right, backwards, out aIndex, out bIndex);
		}

		public bool GetPortal(GraphNode toNode, List<Vector3> left, List<Vector3> right, bool backwards, out int aIndex, out int bIndex)
		{
			aIndex = -1;
			bIndex = -1;
			if (backwards || toNode.GraphIndex != base.GraphIndex)
			{
				return false;
			}
			TriangleMeshNode triangleMeshNode = toNode as TriangleMeshNode;
			int num = SharedEdge(triangleMeshNode);
			switch (num)
			{
			case 255:
				return false;
			case -1:
			{
				for (int j = 0; j < connections.Length; j++)
				{
					if (connections[j].node.GraphIndex != base.GraphIndex)
					{
						NodeLink3Node nodeLink3Node = connections[j].node as NodeLink3Node;
						if (nodeLink3Node != null && nodeLink3Node.GetOther(this) == triangleMeshNode)
						{
							nodeLink3Node.GetPortal(triangleMeshNode, left, right, false);
							return true;
						}
					}
				}
				return false;
			}
			default:
			{
				aIndex = num;
				bIndex = (num + 1) % GetVertexCount();
				Int3 vertex = GetVertex(num);
				Int3 vertex2 = GetVertex((num + 1) % GetVertexCount());
				int num2 = (GetVertexIndex(0) >> 12) & 0x7FFFF;
				int num3 = (triangleMeshNode.GetVertexIndex(0) >> 12) & 0x7FFFF;
				if (num2 != num3)
				{
					INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
					int x;
					int z;
					navmeshHolder.GetTileCoordinates(num2, out x, out z);
					int x2;
					int z2;
					navmeshHolder.GetTileCoordinates(num3, out x2, out z2);
					int i;
					if (Math.Abs(x - x2) == 1)
					{
						i = 2;
					}
					else
					{
						if (Math.Abs(z - z2) != 1)
						{
							return false;
						}
						i = 0;
					}
					int num4 = triangleMeshNode.SharedEdge(this);
					switch (num4)
					{
					case 255:
						throw new Exception("Connection used edge in one direction, but not in the other direction. Has the wrong overload of AddConnection been used?");
					default:
					{
						int val = Math.Min(vertex[i], vertex2[i]);
						int val2 = Math.Max(vertex[i], vertex2[i]);
						Int3 vertex3 = triangleMeshNode.GetVertex(num4);
						Int3 vertex4 = triangleMeshNode.GetVertex((num4 + 1) % triangleMeshNode.GetVertexCount());
						val = Math.Max(val, Math.Min(vertex3[i], vertex4[i]));
						val2 = Math.Min(val2, Math.Max(vertex3[i], vertex4[i]));
						if (vertex[i] < vertex2[i])
						{
							vertex[i] = val;
							vertex2[i] = val2;
						}
						else
						{
							vertex[i] = val2;
							vertex2[i] = val;
						}
						break;
					}
					case -1:
						break;
					}
				}
				if (left != null)
				{
					left.Add((Vector3)vertex);
					right.Add((Vector3)vertex2);
				}
				return true;
			}
			}
		}

		public override float SurfaceArea()
		{
			INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
			return (float)Math.Abs(VectorMath.SignedTriangleAreaTimes2XZ(navmeshHolder.GetVertex(v0), navmeshHolder.GetVertex(v1), navmeshHolder.GetVertex(v2))) * 0.5f;
		}

		public override Vector3 RandomPointOnSurface()
		{
			float value;
			float value2;
			do
			{
				value = UnityEngine.Random.value;
				value2 = UnityEngine.Random.value;
			}
			while (value + value2 > 1f);
			INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
			return (Vector3)(navmeshHolder.GetVertex(v1) - navmeshHolder.GetVertex(v0)) * value + (Vector3)(navmeshHolder.GetVertex(v2) - navmeshHolder.GetVertex(v0)) * value2 + (Vector3)navmeshHolder.GetVertex(v0);
		}

		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.writer.Write(v0);
			ctx.writer.Write(v1);
			ctx.writer.Write(v2);
		}

		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			v0 = ctx.reader.ReadInt32();
			v1 = ctx.reader.ReadInt32();
			v2 = ctx.reader.ReadInt32();
		}
	}
}
