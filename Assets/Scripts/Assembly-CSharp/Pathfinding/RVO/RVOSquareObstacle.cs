using UnityEngine;

namespace Pathfinding.RVO
{
	[AddComponentMenu("Pathfinding/Local Avoidance/Square Obstacle")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_square_obstacle.php")]
	public class RVOSquareObstacle : RVOObstacle
	{
		public float height = 1f;

		public Vector2 size = Vector3.one;

		public Vector2 center = Vector3.zero;

		protected override bool StaticObstacle
		{
			get
			{
				return false;
			}
		}

		protected override bool ExecuteInEditor
		{
			get
			{
				return true;
			}
		}

		protected override bool LocalCoordinates
		{
			get
			{
				return true;
			}
		}

		protected override float Height
		{
			get
			{
				return height;
			}
		}

		protected override bool AreGizmosDirty()
		{
			return false;
		}

		protected override void CreateObstacles()
		{
			size.x = Mathf.Abs(size.x);
			size.y = Mathf.Abs(size.y);
			height = Mathf.Abs(height);
			Vector3[] array = new Vector3[4]
			{
				new Vector3(1f, 0f, -1f),
				new Vector3(1f, 0f, 1f),
				new Vector3(-1f, 0f, 1f),
				new Vector3(-1f, 0f, -1f)
			};
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Scale(new Vector3(size.x * 0.5f, 0f, size.y * 0.5f));
				array[i] += new Vector3(center.x, 0f, center.y);
			}
			AddObstacle(array, height);
		}
	}
}
