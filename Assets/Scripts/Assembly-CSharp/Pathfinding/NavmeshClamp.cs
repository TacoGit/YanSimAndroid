using UnityEngine;

namespace Pathfinding
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_navmesh_clamp.php")]
	public class NavmeshClamp : MonoBehaviour
	{
		private GraphNode prevNode;

		private Vector3 prevPos;

		private void LateUpdate()
		{
			if (prevNode == null)
			{
				prevNode = AstarPath.active.GetNearest(base.transform.position).node;
				prevPos = base.transform.position;
			}
			if (prevNode == null)
			{
				return;
			}
			if (prevNode != null)
			{
				IRaycastableGraph raycastableGraph = AstarData.GetGraph(prevNode) as IRaycastableGraph;
				if (raycastableGraph != null)
				{
					GraphHitInfo hit;
					if (raycastableGraph.Linecast(prevPos, base.transform.position, prevNode, out hit))
					{
						hit.point.y = base.transform.position.y;
						Vector3 vector = VectorMath.ClosestPointOnLine(hit.tangentOrigin, hit.tangentOrigin + hit.tangent, base.transform.position);
						Vector3 point = hit.point;
						point += Vector3.ClampMagnitude((Vector3)hit.node.position - point, 0.008f);
						if (raycastableGraph.Linecast(point, vector, hit.node, out hit))
						{
							hit.point.y = base.transform.position.y;
							base.transform.position = hit.point;
						}
						else
						{
							vector.y = base.transform.position.y;
							base.transform.position = vector;
						}
					}
					prevNode = hit.node;
				}
			}
			prevPos = base.transform.position;
		}
	}
}
