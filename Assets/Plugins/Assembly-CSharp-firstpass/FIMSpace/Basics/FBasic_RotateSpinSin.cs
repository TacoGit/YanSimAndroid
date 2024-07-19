using System;
using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_RotateSpinSin : MonoBehaviour
	{
		[Tooltip("In which axis object should rotate")]
		public Vector3 RotationAxis = Vector3.up;

		[Tooltip("How far can go rotation")]
		public float RotationRange = 40f;

		[Tooltip("How fast object should rotate to it's ranges")]
		public float SinSpeed = 2f;

		private float time;

		private void Start()
		{
			time = UnityEngine.Random.Range(-(float)Math.PI, (float)Math.PI);
		}

		private void Update()
		{
			time += Time.deltaTime * SinSpeed;
			base.transform.Rotate(RotationAxis * Time.deltaTime * RotationRange * Mathf.Sin(time));
		}
	}
}
