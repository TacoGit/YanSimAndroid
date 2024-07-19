using UnityEngine;

public class esc : MonoBehaviour
{
	private void Awake()
	{
		Cursor.visible = false;
	}

	private void Update()
	{
		if (Input.GetKey("escape"))
		{
			Application.Quit();
		}
	}
}
