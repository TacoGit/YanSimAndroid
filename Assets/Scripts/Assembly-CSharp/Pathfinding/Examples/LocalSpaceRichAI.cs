using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_local_space_rich_a_i.php")]
	public class LocalSpaceRichAI : RichAI
	{
		public LocalSpaceGraph graph;

		private void RefreshTransform()
		{
			graph.Refresh();
			richPath.transform = graph.transformation;
			movementPlane = graph.transformation;
		}

		protected override void Start()
		{
			RefreshTransform();
			base.Start();
		}

		protected override void CalculatePathRequestEndpoints(out Vector3 start, out Vector3 end)
		{
			RefreshTransform();
			base.CalculatePathRequestEndpoints(out start, out end);
			start = graph.transformation.InverseTransform(start);
			end = graph.transformation.InverseTransform(end);
		}

		protected override void Update()
		{
			RefreshTransform();
			base.Update();
		}
	}
}
