using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Orbit")]
public class MouseOrbitAndZoom : MonoBehaviour
{
	public Transform target;

	public float distance = 10f;

	public float xSpeed = 250f;

	public float ySpeed = 120f;

	public float zSpeed = -100f;

	public float zMaxLimit = 10f;

	public float zMinLimit = 0.5f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	private float x;

	private float y;

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		x = eulerAngles.y;
		y = eulerAngles.x;
		Rigidbody component = GetComponent<Rigidbody>();
		if (component != null)
		{
			component.freezeRotation = true;
		}
	}

	private void LateUpdate()
	{
		if (target != null)
		{
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			distance += Input.GetAxis("Mouse ScrollWheel") * zSpeed * 0.02f;
			distance = ClampAngle(distance, zMinLimit, zMaxLimit);
			Quaternion quaternion = Quaternion.Euler(y, x, 0f);
			Vector3 position = quaternion * new Vector3(0f, 0f, 0f - distance) + target.position;
			base.transform.rotation = quaternion;
			base.transform.position = position;
		}
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
