using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_CharacterInputAxis : FBasic_CharacterInputBase
	{
		protected override void Update()
		{
			SetInputAxis(new Vector2(CalculateClampedAxisValue("Horizontal"), CalculateClampedAxisValue()));
			SetInputDirection(Camera.main.transform.eulerAngles.y);
			if (Input.GetButtonDown("Jump"))
			{
				Jump();
			}
		}
	}
}
