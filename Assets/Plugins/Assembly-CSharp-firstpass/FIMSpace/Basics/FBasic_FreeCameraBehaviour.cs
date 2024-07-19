using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_FreeCameraBehaviour : MonoBehaviour
	{
		[Header("> Hold right mouse button to rotate camera <")]
		[Tooltip("How fast camera should fly")]
		public float SpeedMultiplier = 10f;

		[Tooltip("Value of acceleration smoothness")]
		public float AccelerationSmothnessValue = 10f;

		[Tooltip("Value of rotation smoothness")]
		public float RotationSmothnessValue = 10f;

		public float MouseSensitivity = 5f;

		private float turboModeMultiply = 5f;

		private Vector3 speeds;

		private float ySpeed;

		private Vector3 rotation;

		private float turbo = 1f;

		private void Start()
		{
			speeds = Vector3.zero;
			ySpeed = 0f;
			rotation = base.transform.rotation.eulerAngles;
		}

		private void Update()
		{
			float axis = Input.GetAxis("Vertical");
			float axis2 = Input.GetAxis("Horizontal");
			float num = axis * Time.smoothDeltaTime * SpeedMultiplier;
			float num2 = axis2 * Time.smoothDeltaTime * SpeedMultiplier;
			if (Input.GetKey(KeyCode.LeftShift))
			{
				turbo = Mathf.Lerp(turbo, turboModeMultiply, Time.smoothDeltaTime * 5f);
			}
			else
			{
				turbo = Mathf.Lerp(turbo, 1f, Time.smoothDeltaTime * 5f);
			}
			num *= turbo;
			num2 *= turbo;
			if (Input.GetMouseButton(1))
			{
				rotation.x -= Input.GetAxis("Mouse Y") * 1f * MouseSensitivity;
				rotation.y += Input.GetAxis("Mouse X") * 1f * MouseSensitivity;
			}
			speeds.z = Mathf.Lerp(speeds.z, num, Time.smoothDeltaTime * AccelerationSmothnessValue);
			speeds.x = Mathf.Lerp(speeds.x, num2, Time.smoothDeltaTime * AccelerationSmothnessValue);
			base.transform.position += base.transform.forward * speeds.z;
			base.transform.position += base.transform.right * speeds.x;
			base.transform.position += base.transform.up * speeds.y;
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(rotation), Time.smoothDeltaTime * RotationSmothnessValue);
			if (Input.GetKey(KeyCode.LeftControl))
			{
				ySpeed = Mathf.Lerp(ySpeed, 1f, Time.smoothDeltaTime * AccelerationSmothnessValue);
			}
			else if (Input.GetButton("Jump"))
			{
				ySpeed = Mathf.Lerp(ySpeed, -1f, Time.smoothDeltaTime * AccelerationSmothnessValue);
			}
			else
			{
				ySpeed = Mathf.Lerp(ySpeed, 0f, Time.smoothDeltaTime * AccelerationSmothnessValue);
			}
			base.transform.position += Vector3.down * ySpeed * turbo * Time.smoothDeltaTime * SpeedMultiplier;
		}

		public void FixedUpdate()
		{
			if (Input.GetMouseButton(1))
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}
}
