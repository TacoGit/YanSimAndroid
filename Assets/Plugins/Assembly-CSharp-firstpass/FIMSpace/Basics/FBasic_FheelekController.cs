using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_FheelekController : FBasic_RigidbodyMovement
	{
		[Tooltip("Just lerping speed for rotating object")]
		public float RotationSpeed = 5f;

		protected bool movingBackward;

		protected FBasic_FheelekAnimator fheelekAnimator;

		private float turbo;

		private Transform wheel;

		private Transform fBody;

		protected override void Start()
		{
			base.Start();
			base.CharacterRigidbody = GetComponent<Rigidbody>();
			wheel = base.transform.Find("Wheel");
			fBody = base.transform.Find("Skeleton");
			fheelekAnimator = new FBasic_FheelekAnimator(this);
			onlyForward = true;
			diagonalMultiplier = 1f;
		}

		protected override void Update()
		{
			CheckGroundPlacement();
			wheel.localRotation *= Quaternion.Euler(accelerationForward * 480f * Time.deltaTime, 0f, 0f);
			fheelekAnimator.Animate(accelerationForward);
			if (Input.GetKey(KeyCode.LeftShift))
			{
				turbo = Mathf.Lerp(turbo, 1f, Time.deltaTime * 10f);
			}
			else
			{
				turbo = Mathf.Lerp(turbo, 0f, Time.deltaTime * 10f);
			}
			float num = targetDirection;
			bool flag = false;
			if (Grounded)
			{
				if (verticalValue != 0f)
				{
					if (!movingBackward)
					{
						targetDirection += 35f * horizontalValue;
					}
					else
					{
						targetDirection -= 35f * horizontalValue;
					}
				}
				else if (horizontalValue != 0f)
				{
					lastTargetVelocityForward = newVelocityForward;
					accelerationForward = Mathf.Min(1f + turbo, accelerationForward + AccelerationSpeed * Time.fixedDeltaTime);
					targetDirection = inputDirection + 90f * horizontalValue;
					flag = true;
				}
			}
			animatedDirection = Mathf.LerpAngle(base.transform.localRotation.eulerAngles.y, targetDirection, Time.deltaTime * RotationSpeed);
			RotationCalculations();
			if (!flag)
			{
				targetDirection = num;
			}
		}

		protected override void FixedUpdate()
		{
			lastTargetVelocityRight = Vector3.zero;
			base.FixedUpdate();
		}

		protected override void RotationCalculations()
		{
			base.transform.rotation = Quaternion.Euler(0f, animatedDirection, 0f);
			fBody.rotation = Quaternion.Euler(0f, Mathf.LerpAngle(fBody.rotation.eulerAngles.y, targetDirection, Time.deltaTime * RotationSpeed * 1.25f), 0f);
		}

		protected override void MoveForward(bool backward)
		{
			if (!backward)
			{
				lastTargetVelocityForward = newVelocityForward;
				accelerationForward = Mathf.Min(1f + turbo, accelerationForward + AccelerationSpeed * Time.fixedDeltaTime);
			}
			else
			{
				lastTargetVelocityForward = newVelocityForward;
				if (onlyForward)
				{
					accelerationForward = Mathf.Min(1f + turbo, accelerationForward + AccelerationSpeed * Time.fixedDeltaTime);
				}
				else
				{
					accelerationForward = Mathf.Max(-1f + turbo, accelerationForward - AccelerationSpeed * Time.fixedDeltaTime);
				}
			}
			movingBackward = backward;
			if (backward)
			{
				targetDirection = inputDirection + 180f;
			}
			else
			{
				targetDirection = inputDirection;
			}
		}

		protected override void Jump()
		{
			base.Jump();
			if (horizontalValue != 0f && verticalValue == 0f)
			{
				base.CharacterRigidbody.velocity += new Vector3(0f - newVelocityRight.z, 0f, newVelocityRight.x);
			}
			fheelekAnimator.Jump();
		}

		protected override void StoppingMovement()
		{
			Vector3 direction = new Vector3(0f, 0f, 1f);
			direction = base.transform.TransformDirection(direction);
			direction *= MaxSpeed;
			base.CharacterRigidbody.velocity = new Vector3(direction.x * accelerationForward, base.CharacterRigidbody.velocity.y, direction.z * accelerationForward);
			targetVelocity = base.CharacterRigidbody.velocity;
			if (inputAxes.x == 0f)
			{
				if (accelerationForward > 0f)
				{
					accelerationForward = Mathf.Max(0f, accelerationForward - DecelerateSpeed * Time.fixedDeltaTime);
				}
				else if (accelerationForward < 0f)
				{
					accelerationForward = Mathf.Min(0f, accelerationForward + DecelerateSpeed * Time.fixedDeltaTime);
				}
			}
		}
	}
}
