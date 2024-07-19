using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_local_space_graph.php")]
	public class LocalSpaceGraph : VersionedMonoBehaviour
	{
		private Matrix4x4 originalMatrix;

		public GraphTransform transformation { get; private set; }

		private void Start()
		{
			originalMatrix = base.transform.worldToLocalMatrix;
			base.transform.hasChanged = true;
			Refresh();
		}

		public void Refresh()
		{
			if (base.transform.hasChanged)
			{
				transformation = new GraphTransform(base.transform.localToWorldMatrix * originalMatrix);
				base.transform.hasChanged = false;
			}
		}
	}
}
