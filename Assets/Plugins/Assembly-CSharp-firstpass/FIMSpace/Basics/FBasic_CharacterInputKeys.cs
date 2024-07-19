using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_CharacterInputKeys : FBasic_CharacterInputBase
	{
		protected override void Update()
		{
			Vector2 zero = Vector2.zero;
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				zero.x = -1f;
			}
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				zero.x = 1f;
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				zero.y = 1f;
			}
			else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				zero.y = -1f;
			}
			SetInputAxis(zero);
			SetInputDirection(Camera.main.transform.eulerAngles.y);
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Jump();
			}
		}
	}
}
