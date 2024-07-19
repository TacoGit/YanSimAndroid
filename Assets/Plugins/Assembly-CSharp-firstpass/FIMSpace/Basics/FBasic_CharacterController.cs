using UnityEngine;

namespace FIMSpace.Basics
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CharacterController))]
	public class FBasic_CharacterController : FBasic_CharacterHorizontalBase
	{
		protected CharacterController characterController;

		protected Vector3 translationVector;

		protected float yVelocity;

		[Tooltip("Switch to use SimpleMove() method in CharacterController instead of Move()")]
		public bool SimpleMove;

		[Header("When we go down the slope, preventing from bumping")]
		public float pushDownYVelocity = -0.085f;

		public Rigidbody CharacterRigidbody { get; protected set; }

		protected override void Start()
		{
			base.Start();
			characterController = GetComponent<CharacterController>();
			CharacterRigidbody = GetComponent<Rigidbody>();
			if ((bool)CharacterRigidbody)
			{
				CharacterRigidbody.isKinematic = true;
				CharacterRigidbody.freezeRotation = false;
				CharacterRigidbody.useGravity = false;
			}
		}

		protected override void MovementCalculations()
		{
			Grounded = characterController.isGrounded;
			if (Grounded)
			{
				Jumped = false;
			}
			base.MovementCalculations();
			translationVector = lastTargetVelocityForward * accelerationForward;
			translationVector += lastTargetVelocityRight * accelerationRight;
			translationVector *= Time.fixedDeltaTime * 0.72f;
			if (inputJump)
			{
				if (Grounded)
				{
					Jump();
				}
				inputJump = false;
			}
			if (Grounded)
			{
				yVelocity = pushDownYVelocity;
			}
			else
			{
				yVelocity -= GravityPower * Time.fixedDeltaTime / 55.5f;
			}
			translationVector.y = yVelocity;
			if (SimpleMove)
			{
				characterController.SimpleMove(translationVector * 50f);
			}
			else
			{
				characterController.Move(translationVector);
			}
		}

		protected override void RotationCalculations()
		{
			if (Grounded)
			{
				if (verticalValue != 0f)
				{
					targetDirection = inputDirection;
				}
				else if (horizontalValue != 0f)
				{
					targetDirection = inputDirection;
				}
				base.RotationCalculations();
			}
		}

		protected override void Jump()
		{
			Grounded = false;
			Jumped = true;
			yVelocity = CalculateJumpYVelocity() / 51.8f;
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (Vector3.Dot(hit.point - base.transform.position, base.transform.up) > 1f && yVelocity > -0.02f)
			{
				yVelocity = -0.02f;
			}
		}
	}
}
