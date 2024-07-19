using UnityEngine;

public class InputDeviceScript : MonoBehaviour
{
	public InputDeviceType Type = InputDeviceType.Gamepad;

	public Vector3 MousePrevious;

	public Vector3 MouseDelta;

	public float Horizontal;

	public float Vertical;

	private void Update()
	{
		MouseDelta = Input.mousePosition - MousePrevious;
		MousePrevious = Input.mousePosition;
		InputDeviceType type = Type;
		if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || MouseDelta != Vector3.zero)
		{
			Type = InputDeviceType.MouseAndKeyboard;
		}
		if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.Joystick1Button3) || Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5) || Input.GetKey(KeyCode.Joystick1Button6) || Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Joystick1Button8) || Input.GetKey(KeyCode.Joystick1Button9) || Input.GetKey(KeyCode.Joystick1Button10) || Input.GetKey(KeyCode.Joystick1Button11) || Input.GetKey(KeyCode.Joystick1Button12) || Input.GetKey(KeyCode.Joystick1Button13) || Input.GetKey(KeyCode.Joystick1Button14) || Input.GetKey(KeyCode.Joystick1Button15) || Input.GetKey(KeyCode.Joystick1Button16) || Input.GetKey(KeyCode.Joystick1Button17) || Input.GetKey(KeyCode.Joystick1Button18) || Input.GetKey(KeyCode.Joystick1Button19) || Input.GetAxis("DpadX") > 0.5f || Input.GetAxis("DpadX") < -0.5f || Input.GetAxis("DpadY") > 0.5f || Input.GetAxis("DpadY") < -0.5f)
		{
			Type = InputDeviceType.Gamepad;
		}
		if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && (Input.GetAxis("Vertical") == 1f || Input.GetAxis("Vertical") == -1f || Input.GetAxis("Horizontal") == 1f || Input.GetAxis("Horizontal") == -1f))
		{
			Type = InputDeviceType.Gamepad;
		}
		if (Type != type)
		{
			PromptSwapScript[] array = Resources.FindObjectsOfTypeAll<PromptSwapScript>();
			PromptSwapScript[] array2 = array;
			foreach (PromptSwapScript promptSwapScript in array2)
			{
				promptSwapScript.UpdateSpriteType(Type);
			}
		}
		Horizontal = Input.GetAxis("Horizontal");
		Vertical = Input.GetAxis("Vertical");
	}
}
