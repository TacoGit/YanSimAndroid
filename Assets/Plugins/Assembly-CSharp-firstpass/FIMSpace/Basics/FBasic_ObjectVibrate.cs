using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_ObjectVibrate : MonoBehaviour
	{
		[Tooltip("How fast object should change translation directions")]
		public float VibrationRate = 8f;

		[Tooltip("How far object can go from it's initial local position")]
		public float BaseRange = 0.5f;

		[Tooltip("Smoothing motion for object")]
		[Range(0f, 1f)]
		public float SmoothTranslation = 0.5f;

		private float[] randomOffsets = new float[6];

		private float time;

		private float speed;

		private float range;

		internal float intensity;

		public Vector3 AxesMultiplier = Vector3.one;

		public bool ChangeObjectPosition = true;

		public Vector3 initialPosition { get; private set; }

		public Vector3 localPosition { get; private set; }

		private void Start()
		{
			initialPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, base.transform.localPosition.z);
			localPosition = new Vector3(0f, 0f, 0f);
			ChooseNewSeed();
			speed = VibrationRate;
			range = BaseRange;
		}

		private void Update()
		{
			if (BaseRange == 0f)
			{
				if (ChangeObjectPosition)
				{
					base.transform.localPosition = initialPosition;
				}
				return;
			}
			intensity = Mathf.Max(intensity * Time.deltaTime, 0f);
			float num = Mathf.Max(0f, Mathf.Log(intensity) / 5f);
			intensity -= Mathf.Max(0f, num);
			speed = Mathf.Min(75f, VibrationRate + intensity * 4f - num * 5f);
			range = (BaseRange + intensity * 0.01f) * 0.01f;
			time += Time.deltaTime * speed;
			Vector3 b = new Vector3(0f, 0f, 0f);
			if (AxesMultiplier.x != 0f)
			{
				b.x = Mathf.Sin(time * randomOffsets[0] + randomOffsets[1]) * range * randomOffsets[3];
				b.x += Mathf.Pow(Mathf.Cos(time / 1.5f * randomOffsets[2]), 2f) * range * randomOffsets[5];
				b.x *= AxesMultiplier.x;
			}
			else
			{
				b.x = 0f;
			}
			if (AxesMultiplier.y != 0f)
			{
				b.y = Mathf.Cos(time * randomOffsets[1] + randomOffsets[2]) * range * randomOffsets[4];
				b.y += Mathf.Sin(time / 2.2f * randomOffsets[4] + randomOffsets[1]) * range * randomOffsets[3];
				b.y *= AxesMultiplier.y;
			}
			else
			{
				b.y = 0f;
			}
			if (AxesMultiplier.z != 0f)
			{
				b.z = Mathf.Sin(time * randomOffsets[2] + randomOffsets[0]) * range * randomOffsets[5];
				b.z += Mathf.Cos(time * 1.24f * randomOffsets[3] + randomOffsets[0]) * range * randomOffsets[4];
				b.z *= AxesMultiplier.z;
			}
			else
			{
				b.z = 0f;
			}
			float t = Mathf.Lerp(1f, Time.deltaTime * 0.5f, SmoothTranslation);
			localPosition = Vector3.Lerp(localPosition, b, t);
			if (ChangeObjectPosition)
			{
				b += initialPosition;
				base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, b, t);
			}
		}

		public void ChooseNewSeed()
		{
			Random.InitState(Random.Range(0, 999999));
			for (int i = 0; i < 3; i++)
			{
				randomOffsets[i] = Random.Range(0.8f, 1f);
			}
			for (int j = 3; j < 6; j++)
			{
				randomOffsets[j] = Random.Range(1f, 2.5f);
			}
			time = Random.Range(0f, 4f);
		}
	}
}
