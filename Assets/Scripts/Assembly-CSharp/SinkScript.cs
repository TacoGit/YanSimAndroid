using UnityEngine;

public class SinkScript : MonoBehaviour
{
	public YandereScript Yandere;

	public PromptScript Prompt;

	private void Start()
	{
		Yandere = GameObject.Find("YandereChan").GetComponent<YandereScript>();
	}

	private void Update()
	{
		if (Yandere.PickUp != null)
		{
			if (Yandere.PickUp.Bucket != null)
			{
				if (Yandere.PickUp.Bucket.Dumbbells == 0)
				{
					Prompt.enabled = true;
					if (!Yandere.PickUp.Bucket.Full)
					{
						Prompt.Label[0].text = "     Fill Bucket";
					}
					else
					{
						Prompt.Label[0].text = "     Empty Bucket";
					}
				}
				else if (Prompt.enabled)
				{
					Prompt.Hide();
					Prompt.enabled = false;
				}
			}
			else if (Yandere.PickUp.BloodCleaner != null)
			{
				if (Yandere.PickUp.BloodCleaner.Blood > 0f)
				{
					Prompt.Label[0].text = "     Empty Robot";
					Prompt.enabled = true;
				}
				else
				{
					Prompt.Hide();
					Prompt.enabled = false;
				}
			}
			else if (Prompt.enabled)
			{
				Prompt.Hide();
				Prompt.enabled = false;
			}
		}
		else if (Prompt.enabled)
		{
			Prompt.Hide();
			Prompt.enabled = false;
		}
		if (Prompt.Circle[0].fillAmount != 0f)
		{
			return;
		}
		if (Yandere.PickUp.Bucket != null)
		{
			if (!Yandere.PickUp.Bucket.Full)
			{
				Yandere.PickUp.Bucket.Fill();
			}
			else
			{
				Yandere.PickUp.Bucket.Empty();
			}
			if (!Yandere.PickUp.Bucket.Full)
			{
				Prompt.Label[0].text = "     Fill Bucket";
			}
			else
			{
				Prompt.Label[0].text = "     Empty Bucket";
			}
		}
		else if (Yandere.PickUp.BloodCleaner != null)
		{
			Yandere.PickUp.BloodCleaner.Blood = 0f;
			Yandere.PickUp.BloodCleaner.Lens.SetActive(false);
		}
		Prompt.Circle[0].fillAmount = 1f;
	}
}
