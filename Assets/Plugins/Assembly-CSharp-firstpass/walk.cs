using System;
using UnityEngine;

public class walk : MonoBehaviour
{
	private float timer;

	public float bobbingSpeed = 0.2f;

	public float bobbingAmount = 0.02f;

	public float midpoint = 0.8f;

	public Camera activeCamera;

	private void Awake()
	{
		Cursor.visible = false;
	}

	private void FixedUpdate()
	{
		if ((bool)activeCamera)
		{
			midpoint = activeCamera.transform.localPosition.y;
		}
		float num = 0f;
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		if (Mathf.Abs(axis) == 0f && Mathf.Abs(axis2) == 0f)
		{
			timer = 0f;
		}
		else
		{
			num = Mathf.Sin(timer);
			timer += bobbingSpeed;
			if (timer > (float)Math.PI * 2f)
			{
				timer -= (float)Math.PI * 2f;
			}
		}
		if (num != 0f)
		{
			float num2 = num * bobbingAmount;
			float num3 = Mathf.Clamp(Mathf.Abs(axis) + Mathf.Abs(axis2), 0f, 1f);
			num2 = num3 * num2;
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, midpoint + num2, base.transform.localPosition.z);
		}
		else
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, midpoint, base.transform.localPosition.z);
		}
	}
}
