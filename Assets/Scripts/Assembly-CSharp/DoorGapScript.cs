using UnityEngine;

public class DoorGapScript : MonoBehaviour
{
	public RummageSpotScript RummageSpot;

	public SchemesScript Schemes;

	public PromptScript Prompt;

	public Transform[] Papers;

	public float Timer;

	public int Phase = 1;

	private void Start()
	{
		Papers[1].gameObject.SetActive(false);
	}

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			if (Phase == 1)
			{
				Prompt.Hide();
				Prompt.enabled = false;
				Prompt.Yandere.Inventory.AnswerSheet = false;
				Papers[1].gameObject.SetActive(true);
				SchemeGlobals.SetSchemeStage(5, 3);
				Schemes.UpdateInstructions();
				GetComponent<AudioSource>().Play();
			}
			else
			{
				Prompt.Hide();
				Prompt.enabled = false;
				Prompt.Yandere.Inventory.AnswerSheet = true;
				Prompt.Yandere.Inventory.DuplicateSheet = true;
				Papers[2].gameObject.SetActive(false);
				RummageSpot.Prompt.Label[0].text = "     Return Answer Sheet";
				RummageSpot.Prompt.enabled = true;
				SchemeGlobals.SetSchemeStage(5, 4);
				Schemes.UpdateInstructions();
			}
			Phase++;
		}
		if (Phase == 2)
		{
			Timer += Time.deltaTime;
			if (Timer > 4f)
			{
				Prompt.Label[0].text = "     Pick Up Sheets";
				Prompt.enabled = true;
				Phase = 2;
			}
			else if (Timer > 3f)
			{
				Transform transform = Papers[2];
				transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(transform.localPosition.z, -0.166f, Time.deltaTime * 10f));
			}
			else if (Timer > 1f)
			{
				Transform transform2 = Papers[1];
				transform2.localPosition = new Vector3(transform2.localPosition.x, transform2.localPosition.y, Mathf.Lerp(transform2.localPosition.z, 0.166f, Time.deltaTime * 10f));
			}
		}
	}
}
