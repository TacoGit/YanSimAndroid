using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[Serializable]
	[AddComponentMenu("Pathfinding/Modifiers/Raycast Modifier")]
	[RequireComponent(typeof(Seeker))]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_raycast_modifier.php")]
	public class RaycastModifier : MonoModifier
	{
		public enum Quality
		{
			Low = 0,
			Medium = 1,
			High = 2,
			Highest = 3
		}

		public bool useRaycasting = true;

		public LayerMask mask = -1;

		[Tooltip("Checks around the line between two points, not just the exact line.\nMake sure the ground is either too far below or is not inside the mask since otherwise the raycast might always hit the ground.")]
		public bool thickRaycast;

		[Tooltip("Distance from the ray which will be checked for colliders")]
		public float thickRaycastRadius;

		[Tooltip("Check for intersections with 2D colliders instead of 3D colliders.")]
		public bool use2DPhysics;

		[Tooltip("Offset from the original positions to perform the raycast.\nCan be useful to avoid the raycast intersecting the ground or similar things you do not want to it intersect")]
		public Vector3 raycastOffset = Vector3.zero;

		[Tooltip("Use raycasting on the graphs. Only currently works with GridGraph and NavmeshGraph and RecastGraph. This is a pro version feature.")]
		public bool useGraphRaycasting;

		[Tooltip("When using the high quality mode the script will try harder to find a shorter path. This is significantly slower than the greedy low quality approach.")]
		public Quality quality = Quality.Medium;

		private static readonly int[] iterationsByQuality = new int[4] { 1, 2, 1, 3 };

		private static List<Vector3> buffer = new List<Vector3>();

		private static float[] DPCosts = new float[16];

		private static int[] DPParents = new int[16];

		public override int Order
		{
			get
			{
				return 40;
			}
		}

		public override void Apply(Path p)
		{
			if (!useRaycasting && !useGraphRaycasting)
			{
				return;
			}
			List<Vector3> b = p.vectorPath;
			if (ValidateLine(null, null, p.vectorPath[0], p.vectorPath[p.vectorPath.Count - 1]))
			{
				Vector3 item = p.vectorPath[0];
				Vector3 item2 = p.vectorPath[p.vectorPath.Count - 1];
				b.ClearFast();
				b.Add(item);
				b.Add(item2);
			}
			else
			{
				int num = iterationsByQuality[(int)quality];
				for (int i = 0; i < num; i++)
				{
					if (i != 0)
					{
						Polygon.Subdivide(b, buffer, 3);
						Memory.Swap(ref buffer, ref b);
						buffer.ClearFast();
						b.Reverse();
					}
					b = ((quality < Quality.High) ? ApplyGreedy(p, b) : ApplyDP(p, b));
				}
				if (num % 2 == 0)
				{
					b.Reverse();
				}
			}
			p.vectorPath = b;
		}

		private List<Vector3> ApplyGreedy(Path p, List<Vector3> points)
		{
			bool flag = points.Count == p.path.Count;
			int num;
			for (int i = 0; i < points.Count; i += num)
			{
				Vector3 vector = points[i];
				GraphNode n = ((!flag || !(points[i] == (Vector3)p.path[i].position)) ? null : p.path[i]);
				buffer.Add(vector);
				num = 1;
				int num2 = 2;
				while (true)
				{
					int num3 = i + num2;
					if (num3 >= points.Count)
					{
						num2 = points.Count - i;
						break;
					}
					Vector3 vector2 = points[num3];
					GraphNode n2 = ((!flag || !(vector2 == (Vector3)p.path[num3].position)) ? null : p.path[num3]);
					if (!ValidateLine(n, n2, vector, vector2))
					{
						break;
					}
					num = num2;
					num2 *= 2;
				}
				while (num + 1 < num2)
				{
					int num4 = (num + num2) / 2;
					int index = i + num4;
					Vector3 vector3 = points[index];
					GraphNode n3 = ((!flag || !(vector3 == (Vector3)p.path[index].position)) ? null : p.path[index]);
					if (ValidateLine(n, n3, vector, vector3))
					{
						num = num4;
					}
					else
					{
						num2 = num4;
					}
				}
			}
			Memory.Swap(ref buffer, ref points);
			buffer.ClearFast();
			return points;
		}

		private List<Vector3> ApplyDP(Path p, List<Vector3> points)
		{
			if (DPCosts.Length < points.Count)
			{
				DPCosts = new float[points.Count];
				DPParents = new int[points.Count];
			}
			for (int i = 0; i < DPParents.Length; i++)
			{
				DPCosts[i] = (DPParents[i] = -1);
			}
			bool flag = points.Count == p.path.Count;
			for (int j = 0; j < points.Count; j++)
			{
				float num = DPCosts[j];
				Vector3 vector = points[j];
				bool flag2 = flag && vector == (Vector3)p.path[j].position;
				for (int k = j + 1; k < points.Count; k++)
				{
					float num2 = num + (points[k] - vector).magnitude + 0.0001f;
					if (DPParents[k] == -1 || num2 < DPCosts[k])
					{
						bool flag3 = flag && points[k] == (Vector3)p.path[k].position;
						if (k != j + 1 && !ValidateLine((!flag2) ? null : p.path[j], (!flag3) ? null : p.path[k], vector, points[k]))
						{
							break;
						}
						DPCosts[k] = num2;
						DPParents[k] = j;
					}
				}
			}
			for (int num3 = points.Count - 1; num3 != -1; num3 = DPParents[num3])
			{
				buffer.Add(points[num3]);
			}
			buffer.Reverse();
			Memory.Swap(ref buffer, ref points);
			buffer.ClearFast();
			return points;
		}

		protected bool ValidateLine(GraphNode n1, GraphNode n2, Vector3 v1, Vector3 v2)
		{
			if (useRaycasting)
			{
				if (use2DPhysics)
				{
					if (thickRaycast && thickRaycastRadius > 0f && (bool)Physics2D.CircleCast(v1 + raycastOffset, thickRaycastRadius, v2 - v1, (v2 - v1).magnitude, mask))
					{
						return false;
					}
					if ((bool)Physics2D.Linecast(v1 + raycastOffset, v2 + raycastOffset, mask))
					{
						return false;
					}
				}
				else
				{
					if (thickRaycast && thickRaycastRadius > 0f && Physics.SphereCast(new Ray(v1 + raycastOffset, v2 - v1), thickRaycastRadius, (v2 - v1).magnitude, mask))
					{
						return false;
					}
					if (Physics.Linecast(v1 + raycastOffset, v2 + raycastOffset, mask))
					{
						return false;
					}
				}
			}
			if (useGraphRaycasting)
			{
				bool flag = n1 != null && n2 != null;
				if (n1 == null)
				{
					n1 = AstarPath.active.GetNearest(v1).node;
				}
				if (n2 == null)
				{
					n2 = AstarPath.active.GetNearest(v2).node;
				}
				if (n1 != null && n2 != null)
				{
					NavGraph graph = n1.Graph;
					NavGraph graph2 = n2.Graph;
					if (graph != graph2)
					{
						return false;
					}
					IRaycastableGraph raycastableGraph = graph as IRaycastableGraph;
					GridGraph gridGraph = graph as GridGraph;
					if (flag && gridGraph != null)
					{
						return !gridGraph.Linecast(n1 as GridNodeBase, n2 as GridNodeBase);
					}
					if (raycastableGraph != null)
					{
						return !raycastableGraph.Linecast(v1, v2, n1);
					}
				}
			}
			return true;
		}
	}
}
