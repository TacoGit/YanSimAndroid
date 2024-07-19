using UnityEngine;

namespace Pathfinding
{
	public struct GraphHitInfo
	{
		public Vector3 origin;

		public Vector3 point;

		public GraphNode node;

		public Vector3 tangentOrigin;

		public Vector3 tangent;

		public float distance
		{
			get
			{
				return (point - origin).magnitude;
			}
		}

		public GraphHitInfo(Vector3 point)
		{
			tangentOrigin = Vector3.zero;
			origin = Vector3.zero;
			this.point = point;
			node = null;
			tangent = Vector3.zero;
		}
	}
}
