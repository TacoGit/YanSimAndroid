using UnityEngine;

public class HomeTriggerScript : MonoBehaviour
{
	public HomeCameraScript HomeCamera;

	public UILabel Label;

	public bool FadeIn;

	public int ID;

	private void Start()
	{
		Label.color = new Color(Label.color.r, Label.color.g, Label.color.b, 0f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			HomeCamera.ID = ID;
			FadeIn = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			HomeCamera.ID = 0;
			FadeIn = false;
		}
	}

	private void Update()
	{
		Label.color = new Color(Label.color.r, Label.color.g, Label.color.b, Mathf.MoveTowards(Label.color.a, (!FadeIn) ? 0f : 1f, Time.deltaTime * 10f));
	}

	public void Disable()
	{
		Label.color = new Color(Label.color.r, Label.color.g, Label.color.b, 0f);
		base.gameObject.SetActive(false);
	}
}
