using System;
using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_FlyMovement : MonoBehaviour
	{
		[Tooltip("How quick model should fly on it's trajectory")]
		public float MainSpeed = 1f;

		[Tooltip("How far should fly our object")]
		public Vector3 RangeValue = Vector3.one;

		[Tooltip("Multiplier for range value but applied to all axes")]
		public float RangeMul = 5f;

		[Tooltip("Additional translation on y axis if you want movement to be little like butterfly flight")]
		public float AddYSin = 1f;

		public float AddYSinTimeSpeed = 1f;

		[Tooltip("How quick object should rotate to it's forward movement direction")]
		public float RotateForwardSpeed = 10f;

		private float time;

		private Vector3 offset;

		private Vector3 initPos;

		private Vector3 preOffsetNoYAdd;

		private Vector3 posOffsetNoYAdd;

		private Vector3 speeds;

		private Vector3 randomVector1;

		private Vector3 randomVector2;

		private void Start()
		{
			initPos = base.transform.position;
			time = UnityEngine.Random.Range((float)Math.PI * -3f, (float)Math.PI * 3f);
			randomVector1 = FVectorMethods.RandomVector(4.5f, 6.5f);
			randomVector2.x = UnityEngine.Random.Range(10f, 12f);
			randomVector2.y = UnityEngine.Random.Range(3.25f, 4.75f);
			randomVector2.z = UnityEngine.Random.Range(2.55f, 4.25f);
		}

		private void Update()
		{
			time += Time.deltaTime * MainSpeed;
			float num = (Mathf.Sin(time) * randomVector1.x + Mathf.Sin(time * 1.5f + 3.5f) * 4f + Mathf.Cos(time * 1.5f + 3.5f) * randomVector2.x) * MainSpeed;
			float num2 = (Mathf.Cos(time) * randomVector1.y + Mathf.Sin(time * 1.75f + 4.2f) * randomVector2.y) * MainSpeed;
			float num3 = (Mathf.Cos(time) * randomVector1.z + Mathf.Cos(time * 1.25f - 2.2f) * randomVector2.z) * MainSpeed;
			offset.x += num;
			offset.y += num2;
			offset.z += num3;
			Vector3 vector = offset;
			vector.x *= RangeValue.x;
			vector.y *= RangeValue.y;
			vector.z *= RangeValue.z;
			vector.y += (Mathf.Cos(time * AddYSinTimeSpeed * 2.2f + 4.2f) * 7f + Mathf.Sin(time * AddYSinTimeSpeed * 3f + 4.2f) * 5f) * AddYSin * MainSpeed;
			Vector3 vector2 = initPos + vector * RangeMul * 0.001f;
			if (RotateForwardSpeed > 0f)
			{
				Quaternion b = Quaternion.LookRotation(vector2 - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * RotateForwardSpeed);
			}
			base.transform.position = vector2;
		}
	}
}
