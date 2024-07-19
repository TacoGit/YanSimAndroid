using UnityEngine;
using UnityEngine.UI;

public class FramerateScript : MonoBehaviour
{
	public float updateInterval = 0.5f;

	private Text fpsText;

	private float accum;

	private int frames;

	private float timeleft;

	public float FPS;

	private void Start()
	{
		fpsText = GetComponent<Text>();
		timeleft = updateInterval;
	}

	private void Update()
	{ // IM TOO LAZY FOR THIS
		//fpsText.text = "FPS:<>";
		//timeleft -= Time.deltaTime;
		//accum += Time.timeScale / Time.deltaTime;
		//frames++;
		//if (timeleft <= 0f)
		//{
		//	FPS = accum / (float)frames;
		//	int num = Mathf.Clamp((int)FPS, 0, Application.targetFrameRate);
		//	if (num > 0) fpsText.text = "FPS: " + num;
		//	else fpsText.text = "D_FPS: <NOT FOUND, REFER TO DEBUG_FPS.CS>";
		//	timeleft = updateInterval;
		//	accum = 0f;
		//	frames = 0;
		//}
	}
}
