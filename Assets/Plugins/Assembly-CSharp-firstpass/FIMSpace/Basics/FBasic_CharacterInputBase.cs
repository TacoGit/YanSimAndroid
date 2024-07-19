using UnityEngine;

namespace FIMSpace.Basics
{
	public abstract class FBasic_CharacterInputBase : MonoBehaviour
	{
		protected FBasic_CharacterMovementBase characterController;

		protected virtual void Start()
		{
			characterController = GetComponent<FBasic_CharacterMovementBase>();
		}

		protected virtual void Update()
		{
			SetInputAxis(new Vector2(0f, 0f));
		}

		protected virtual void OnDisable()
		{
			SetInputAxis(new Vector2(0f, 0f));
		}

		public void SetInputAxis(Vector2 inputAxis)
		{
			characterController.SetInputAxis(inputAxis);
		}

		public void Jump()
		{
			characterController.SetJumpInput();
		}

		public void SetInputDirection(float yDirection)
		{
			characterController.SetInputDirection(yDirection);
		}

		protected float CalculateClampedAxisValue(string axis = "Vertical")
		{
			float axis2 = Input.GetAxis(axis);
			float result = 0f;
			if (axis2 < -0.2f || axis2 > 0.2f)
			{
				result = Mathf.Sign(axis2);
			}
			return result;
		}
	}
}
