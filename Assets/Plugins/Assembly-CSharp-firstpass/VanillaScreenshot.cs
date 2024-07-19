using UnityEngine;

public class VanillaScreenshot : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			string text = Application.dataPath + "/StreamingAssets";
			ScreenCapture.CaptureScreenshot(text + "/Vanilla_Test.png");
		}
	}
}
