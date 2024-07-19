using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_TPPCameraBehaviour : MonoBehaviour
	{
		[Header("Transform to be followed by camera")]
		public Transform ToFollow;

		[Header("Offset in position in reference to target transform (focus point)")]
		public Vector3 FollowingOffset = new Vector3(0f, 1.5f, 0f);

		[Header("Offset in position in reference to camera orientation")]
		public Vector3 FollowingOffsetDirection = new Vector3(0f, 0f, 0f);

		[Header("Clamp values for zoom of camera")]
		public Vector2 DistanceRanges = new Vector2(5f, 10f);

		private float targetDistance;

		private float animatedDistance;

		public Vector2 RotationRanges = new Vector2(-60f, 60f);

		private Vector2 targetSphericRotation = new Vector2(0f, 0f);

		private Vector2 animatedSphericRotation = new Vector2(0f, 0f);

		[Space(10f)]
		[Tooltip("Sensitivity value for rotating camera around following object")]
		public float RotationSensitivity = 10f;

		[Header("If you want camera rotation to be smooth")]
		[Range(0.1f, 1f)]
		public float RotationSpeed = 1f;

		[Header("If you want camera to follow target with some smoothness")]
		[Range(0f, 1f)]
		public float HardFollowValue = 1f;

		[Header("If you want to hold cursor (cursor switch on TAB)")]
		public bool LockCursor = true;

		private bool rotateCamera = true;

		private RaycastHit sightObstacleHit;

		[Header("Layer mask to check obstacles in sight ray")]
		public LayerMask SightLayerMask;

		private Vector3 targetPosition;

		[Header("How far forward raycast should check collision for camera")]
		public float CollisionOffset = 1f;

		public EFUpdateClock UpdateClock;

		private void Start()
		{
			targetDistance = (DistanceRanges.x + DistanceRanges.y) / 2f;
			animatedDistance = DistanceRanges.y;
			targetSphericRotation = new Vector2(0f, 23f);
			animatedSphericRotation = targetSphericRotation;
			if (LockCursor)
			{
				HelperSwitchCursor();
			}
		}

		private void UpdateMethods()
		{
			InputCalculations();
			ZoomCalculations();
			FollowCalculations();
			RaycastCalculations();
			SwitchCalculations();
		}

		private void LateUpdate()
		{
			if (UpdateClock == EFUpdateClock.LateUpdate)
			{
				UpdateMethods();
			}
		}

		private void Update()
		{
			if (UpdateClock == EFUpdateClock.Update)
			{
				UpdateMethods();
			}
		}

		private void FixedUpdate()
		{
			if (UpdateClock == EFUpdateClock.FixedUpdate)
			{
				UpdateMethods();
			}
		}

		private void InputCalculations()
		{
			targetDistance -= Input.GetAxis("Mouse ScrollWheel") * 5f;
			if (rotateCamera)
			{
				targetSphericRotation.x += Input.GetAxis("Mouse X") * RotationSensitivity;
				targetSphericRotation.y -= Input.GetAxis("Mouse Y") * RotationSensitivity;
			}
		}

		private void ZoomCalculations()
		{
			if (!sightObstacleHit.transform)
			{
				targetDistance = Mathf.Clamp(targetDistance, DistanceRanges.x, DistanceRanges.y);
			}
			animatedDistance = Mathf.Lerp(animatedDistance, targetDistance, Time.deltaTime * 8f);
		}

		private void FollowCalculations()
		{
			targetSphericRotation.y = HelperClampAngle(targetSphericRotation.y, RotationRanges.x, RotationRanges.y);
			if (RotationSpeed < 1f)
			{
				animatedSphericRotation = new Vector2(Mathf.LerpAngle(animatedSphericRotation.x, targetSphericRotation.x, Time.deltaTime * 30f * RotationSpeed), Mathf.LerpAngle(animatedSphericRotation.y, targetSphericRotation.y, Time.deltaTime * 30f * RotationSpeed));
			}
			else
			{
				animatedSphericRotation = targetSphericRotation;
			}
			Quaternion rotation = Quaternion.Euler(animatedSphericRotation.y, animatedSphericRotation.x, 0f);
			base.transform.rotation = rotation;
			Vector3 b = ToFollow.transform.position + FollowingOffset;
			if (HardFollowValue < 1f)
			{
				float num = Mathf.Lerp(0.5f, 40f, HardFollowValue);
				b = Vector3.Lerp(targetPosition, b, Time.deltaTime * num);
			}
			targetPosition = b;
		}

		private void RaycastCalculations()
		{
			Vector3 origin = ToFollow.transform.position + FollowingOffset + base.transform.TransformVector(FollowingOffsetDirection);
			Quaternion quaternion = Quaternion.Euler(targetSphericRotation.y, targetSphericRotation.x, 0f);
			Ray ray = new Ray(origin, quaternion * -Vector3.forward);
			if (Physics.Raycast(ray, out sightObstacleHit, targetDistance + CollisionOffset, SightLayerMask, QueryTriggerInteraction.Ignore))
			{
				base.transform.position = sightObstacleHit.point - ray.direction * CollisionOffset;
				return;
			}
			Vector3 vector = base.transform.rotation * -Vector3.forward * animatedDistance;
			base.transform.position = targetPosition + vector + base.transform.TransformVector(FollowingOffsetDirection);
		}

		private void SwitchCalculations()
		{
			if (LockCursor && Input.GetKeyDown(KeyCode.Tab))
			{
				HelperSwitchCursor();
				if (Cursor.visible)
				{
					rotateCamera = false;
				}
				else
				{
					rotateCamera = true;
				}
			}
		}

		private float HelperClampAngle(float angle, float min, float max)
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

		private void HelperSwitchCursor()
		{
			if (Cursor.visible)
			{
				if (Application.isFocused)
				{
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				}
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}
}
