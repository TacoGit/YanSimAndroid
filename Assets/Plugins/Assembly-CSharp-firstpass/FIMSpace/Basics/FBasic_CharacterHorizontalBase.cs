using UnityEngine;

namespace FIMSpace.Basics
{
	public abstract class FBasic_CharacterHorizontalBase : FBasic_CharacterMovementBase
	{
		protected float accelerationRight;

		protected Vector3 newVelocityRight = Vector3.zero;

		protected Vector3 lastTargetVelocityRight = Vector3.zero;

		protected float horizontalValue;

		protected override void MovementCalculations()
		{
			if (Grounded)
			{
				verticalValue = inputAxes.y;
				newVelocityForward = CalculateTargetVelocity(new Vector3(0f, 0f, Mathf.Abs(verticalValue)));
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
				horizontalValue = inputAxes.x;
				newVelocityRight = CalculateTargetVelocity(new Vector3(Mathf.Abs(horizontalValue), 0f, 0f));
				if (horizontalValue > 0f)
				{
					MoveRight(false);
				}
				else if (horizontalValue < 0f)
				{
					MoveRight(true);
				}
				else
				{
					StoppingSidewaysMovement();
				}
			}
		}

		protected virtual void MoveRight(bool leftSide)
		{
			if (!leftSide)
			{
				lastTargetVelocityRight = newVelocityRight;
				accelerationRight = Mathf.Min(1f, accelerationRight + AccelerationSpeed * Time.fixedDeltaTime);
			}
			else
			{
				lastTargetVelocityRight = newVelocityRight;
				accelerationRight = Mathf.Max(-1f, accelerationRight - AccelerationSpeed * Time.fixedDeltaTime);
			}
		}

		protected virtual void StoppingSidewaysMovement()
		{
			if (accelerationRight > 0f)
			{
				accelerationRight = Mathf.Max(0f, accelerationRight - DecelerateSpeed * Time.fixedDeltaTime);
			}
			else if (accelerationRight < 0f)
			{
				accelerationRight = Mathf.Min(0f, accelerationRight + DecelerateSpeed * Time.fixedDeltaTime);
			}
		}

		protected override void RotationCalculations()
		{
			animatedDirection = Mathf.LerpAngle(animatedDirection, targetDirection, Time.deltaTime * 20f);
			base.transform.rotation = Quaternion.Euler(0f, animatedDirection, 0f);
		}
	}
}
