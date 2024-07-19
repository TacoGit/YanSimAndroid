using UnityEngine;

namespace FIMSpace.Basics
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Collider))]
	public abstract class FBasic_CharacterMovementBase : MonoBehaviour
	{
		[Header("Main speed factor")]
		public float MaxSpeed = 8f;

		public bool Grounded;

		public bool Jumped;

		protected float targetDirection;

		protected float animatedDirection;

		protected bool onlyForward;

		public float GravityPower = 25f;

		[Header("How quick acceleration should be lerped")]
		public float AccelerationSpeed = 10f;

		[Header("How quick deceleration should be lerped")]
		public float DecelerateSpeed = 6f;

		protected float accelerationForward;

		protected Vector3 newVelocityForward = Vector3.zero;

		protected Vector3 lastTargetVelocityForward = Vector3.zero;

		protected float verticalValue;

		protected Vector2 inputAxes = Vector2.zero;

		protected float inputDirection;

		protected bool inputJump;

		protected virtual void Start()
		{
			inputDirection = base.transform.eulerAngles.y;
			targetDirection = inputDirection;
		}

		protected virtual void Update()
		{
			RotationCalculations();
		}

		protected virtual void FixedUpdate()
		{
			MovementCalculations();
		}

		protected virtual void RotationCalculations()
		{
			targetDirection += inputAxes.x * Time.deltaTime * 150f;
			animatedDirection = Mathf.Lerp(animatedDirection, targetDirection, Time.deltaTime * 20f);
			base.transform.rotation = Quaternion.Euler(0f, animatedDirection, 0f);
		}

		protected virtual void MovementCalculations()
		{
			if (Grounded)
			{
				verticalValue = inputAxes.y;
				newVelocityForward = new Vector3(0f, 0f, verticalValue);
				if (verticalValue > 0f)
				{
					MoveForward(false);
				}
				else if (verticalValue < 0f)
				{
					MoveForward(true);
				}
				else
				{
					StoppingMovement();
				}
			}
		}

		protected virtual Vector3 CalculateTargetVelocity(Vector3 direction)
		{
			Vector3 direction2 = direction;
			direction2 = base.transform.TransformDirection(direction2);
			return direction2 * MaxSpeed;
		}

		protected virtual void MoveForward(bool backward)
		{
			if (!backward)
			{
				lastTargetVelocityForward = newVelocityForward;
				accelerationForward = Mathf.Min(1f, accelerationForward + AccelerationSpeed * Time.fixedDeltaTime);
				return;
			}
			lastTargetVelocityForward = newVelocityForward;
			if (onlyForward)
			{
				accelerationForward = Mathf.Min(1f, accelerationForward + AccelerationSpeed * Time.fixedDeltaTime);
			}
			else
			{
				accelerationForward = Mathf.Max(-1f, accelerationForward - AccelerationSpeed * Time.fixedDeltaTime);
			}
		}

		protected virtual void StoppingMovement()
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

		protected virtual void Jump()
		{
		}

		internal void SetInputAxis(Vector2 inputAxis)
		{
			inputAxes = inputAxis;
		}

		internal void SetInputDirection(float yDirection)
		{
			inputDirection = yDirection;
		}

		internal void SetJumpInput()
		{
			inputJump = true;
		}

		protected float CalculateJumpYVelocity()
		{
			return Mathf.Sqrt(4f * GravityPower);
		}
	}
}
