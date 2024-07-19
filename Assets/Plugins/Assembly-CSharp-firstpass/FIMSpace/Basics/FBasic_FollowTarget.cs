using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_FollowTarget : MonoBehaviour
	{
		[Tooltip("Multiplies deltaTime")]
		[Range(1f, 60f)]
		public float MoveSpeed = 60f;

		public Transform TargetTransform;

		public Vector3 PositionOffsetInTargetSpace;

		public EFUpdateClock UpdateClock;

		private void Update()
		{
			if (UpdateClock == EFUpdateClock.Update)
			{
				Follow();
			}
		}

		private void LateUpdate()
		{
			if (UpdateClock == EFUpdateClock.LateUpdate)
			{
				Follow();
			}
		}

		private void FixedUpdate()
		{
			if (UpdateClock == EFUpdateClock.FixedUpdate)
			{
				Follow();
			}
		}

		private void Follow()
		{
			if (MoveSpeed < 60f)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, TargetTransform.position + TargetTransform.TransformVector(PositionOffsetInTargetSpace), Time.deltaTime * MoveSpeed);
			}
			else
			{
				base.transform.position = TargetTransform.position + TargetTransform.TransformVector(PositionOffsetInTargetSpace);
			}
		}
	}
}
