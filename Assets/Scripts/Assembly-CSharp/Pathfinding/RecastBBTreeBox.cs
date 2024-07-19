using UnityEngine;

namespace Pathfinding
{
	public class RecastBBTreeBox
	{
		public Rect rect;

		public RecastMeshObj mesh;

		public RecastBBTreeBox c1;

		public RecastBBTreeBox c2;

		public RecastBBTreeBox(RecastMeshObj mesh)
		{
			this.mesh = mesh;
			Vector3 min = mesh.bounds.min;
			Vector3 max = mesh.bounds.max;
			rect = Rect.MinMaxRect(min.x, min.z, max.x, max.z);
		}

		public bool Contains(Vector3 p)
		{
			return rect.Contains(p);
		}
	}
}
