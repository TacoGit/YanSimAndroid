using UnityEngine;

public class BugScript : MonoBehaviour
{
	public PromptScript Prompt;

	public Renderer MyRenderer;

	public AudioSource MyAudio;

	public AudioClip[] Praise;

	private void Start()
	{
		MyRenderer.enabled = false;
	}

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			MyAudio.clip = Praise[Random.Range(0, Praise.Length)];
			MyAudio.Play();
			MyRenderer.enabled = true;
			PlayerGlobals.PantyShots += 5;
			base.enabled = false;
			Prompt.enabled = false;
			Prompt.Hide();
		}
	}
}
