using UnityEngine;

public class FadeInOut : MonoBehaviour
{
	public Texture2D fadeOutTexture;

	public float fadeSpeed = 0.3f;

	public int drawDepth = -1000;

	private float alpha = 1f;

	private float fadeDir = -1f;

	private void OnGUI()
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(-10f, -10f, (float)Screen.width + 10f, (float)Screen.height + 10f), fadeOutTexture);
	}

	private void fadeIn()
	{
		fadeDir = 1f;
	}

	private void fadeOut()
	{
		fadeDir = -1f;
	}

	private void Start()
	{
		alpha = 1f;
	}
}
