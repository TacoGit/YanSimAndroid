using UnityEngine;

public class SecuritySystemScript : MonoBehaviour
{
	public PromptScript Prompt;

	public bool Evidence;

	public bool Masked;

	public SecurityCameraScript[] Cameras;

	public MetalDetectorScript[] Detectors;

	private void Start()
	{
		if (!SchoolGlobals.HighSecurity)
		{
			base.enabled = false;
			Prompt.Hide();
			Prompt.enabled = false;
		}
	}

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			for (int i = 0; i < Cameras.Length; i++)
			{
				Cameras[i].transform.parent.transform.parent.gameObject.GetComponent<AudioSource>().Stop();
				Cameras[i].gameObject.SetActive(false);
			}
			for (int i = 0; i < Detectors.Length; i++)
			{
				Detectors[i].MyCollider.enabled = false;
				Detectors[i].enabled = false;
			}
			GetComponent<AudioSource>().Play();
			Prompt.Hide();
			Prompt.enabled = false;
			Evidence = false;
			base.enabled = false;
		}
	}
}
