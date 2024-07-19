using Pathfinding.RVO;
using UnityEngine;

namespace Pathfinding.Legacy
{
	[AddComponentMenu("Pathfinding/Legacy/Local Avoidance/Legacy RVO Controller")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_legacy_1_1_legacy_r_v_o_controller.php")]
	public class LegacyRVOController : RVOController
	{
		[Tooltip("Layer mask for the ground. The RVOController will raycast down to check for the ground to figure out where to place the agent")]
		public new LayerMask mask = -1;

		public new bool enableRotation = true;

		public new float rotationSpeed = 30f;

		public void Update()
		{
			if (base.rvoAgent != null)
			{
				Vector3 vector = tr.position + CalculateMovementDelta(Time.deltaTime);
				RaycastHit hitInfo;
				if ((int)mask != 0 && Physics.Raycast(vector + Vector3.up * height * 0.5f, Vector3.down, out hitInfo, float.PositiveInfinity, mask))
				{
					vector.y = hitInfo.point.y;
				}
				else
				{
					vector.y = 0f;
				}
				tr.position = vector + Vector3.up * (height * 0.5f - center);
				if (enableRotation && base.velocity != Vector3.zero)
				{
					base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.LookRotation(base.velocity), Time.deltaTime * rotationSpeed * Mathf.Min(base.velocity.magnitude, 0.2f));
				}
			}
		}
	}
}
