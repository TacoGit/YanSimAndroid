using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_Rotator : MonoBehaviour
	{
		public Vector3 RotationAxis = new Vector3(0f, 1f, 0f);

		public float RotationSpeed = 100f;

		public bool UnscaledDeltaTime;

		protected virtual void Update()
		{
			float num = ((!UnscaledDeltaTime) ? Time.deltaTime : Time.unscaledDeltaTime);
			base.transform.localRotation *= Quaternion.AngleAxis(num * RotationSpeed, RotationAxis);
		}
	}
}
