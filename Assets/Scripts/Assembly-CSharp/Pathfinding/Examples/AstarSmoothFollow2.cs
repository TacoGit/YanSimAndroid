using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_astar_smooth_follow2.php")]
	public class AstarSmoothFollow2 : MonoBehaviour
	{
		public Transform target;

		public float distance = 3f;

		public float height = 3f;

		public float damping = 5f;

		public bool smoothRotation = true;

		public bool followBehind = true;

		public float rotationDamping = 10f;

		public bool staticOffset;

		private void LateUpdate()
		{
			Vector3 b = (staticOffset ? (target.position + new Vector3(0f, height, distance)) : ((!followBehind) ? target.TransformPoint(0f, height, distance) : target.TransformPoint(0f, height, 0f - distance)));
			base.transform.position = Vector3.Lerp(base.transform.position, b, Time.deltaTime * damping);
			if (smoothRotation)
			{
				Quaternion b2 = Quaternion.LookRotation(target.position - base.transform.position, target.up);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b2, Time.deltaTime * rotationDamping);
			}
			else
			{
				base.transform.LookAt(target, target.up);
			}
		}
	}
}
