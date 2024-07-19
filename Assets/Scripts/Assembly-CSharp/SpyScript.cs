using UnityEngine;

public class SpyScript : MonoBehaviour
{
	public PromptBarScript PromptBar;

	public YandereScript Yandere;

	public PromptScript Prompt;

	public GameObject SpyCamera;

	public Transform SpyTarget;

	public Transform SpySpot;

	public float Timer;

	public bool CanRecord;

	public bool Recording;

	public int Phase;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Yandere.Character.GetComponent<Animation>().CrossFade("f02_spying_00");
			Yandere.CanMove = false;
			Phase++;
		}
		if (Phase == 1)
		{
			Quaternion b = Quaternion.LookRotation(SpyTarget.transform.position - Yandere.transform.position);
			Yandere.transform.rotation = Quaternion.Slerp(Yandere.transform.rotation, b, Time.deltaTime * 10f);
			Yandere.MoveTowardsTarget(SpySpot.position);
			Timer += Time.deltaTime;
			if (Timer > 1f)
			{
				if (Yandere.Inventory.DirectionalMic)
				{
					PromptBar.Label[0].text = "Record";
					CanRecord = true;
				}
				PromptBar.Label[1].text = "Stop";
				PromptBar.UpdateButtons();
				PromptBar.Show = true;
				Yandere.MainCamera.enabled = false;
				SpyCamera.SetActive(true);
				Phase++;
			}
		}
		else if (Phase == 2)
		{
			if (CanRecord && Input.GetButtonDown("A"))
			{
				Yandere.Character.GetComponent<Animation>().CrossFade("f02_spyRecord_00");
				Yandere.Microphone.SetActive(true);
				Recording = true;
			}
			if (Input.GetButtonDown("B"))
			{
				End();
			}
		}
	}

	public void End()
	{
		PromptBar.ClearButtons();
		PromptBar.Show = false;
		Yandere.Microphone.SetActive(false);
		Yandere.MainCamera.enabled = true;
		Yandere.CanMove = true;
		SpyCamera.SetActive(false);
		Timer = 0f;
		Phase = 0;
	}
}
