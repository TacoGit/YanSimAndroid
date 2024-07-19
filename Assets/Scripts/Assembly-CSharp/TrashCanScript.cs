using UnityEngine;

public class TrashCanScript : MonoBehaviour
{
	public YandereScript Yandere;

	public PromptScript Prompt;

	public Transform TrashPosition;

	public GameObject Item;

	public bool Occupied;

	public bool Weapon;

	private void Update()
	{
		if (!Occupied)
		{
			if (Prompt.Circle[0].fillAmount == 0f)
			{
				Prompt.Circle[0].fillAmount = 1f;
				if (Yandere.PickUp != null)
				{
					Item = Yandere.PickUp.gameObject;
					Yandere.MyController.radius = 0.5f;
					Yandere.EmptyHands();
				}
				else
				{
					Item = Yandere.EquippedWeapon.gameObject;
					Yandere.DropTimer[Yandere.Equipped] = 0.5f;
					Yandere.DropWeapon(Yandere.Equipped);
					Weapon = true;
				}
				Item.transform.parent = TrashPosition;
				Item.GetComponent<Rigidbody>().useGravity = false;
				Item.GetComponent<Collider>().enabled = false;
				Item.GetComponent<PromptScript>().Hide();
				Item.GetComponent<PromptScript>().enabled = false;
				Occupied = true;
				UpdatePrompt();
			}
		}
		else if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			Item.GetComponent<PromptScript>().Circle[3].fillAmount = -1f;
			Item.GetComponent<PromptScript>().enabled = true;
			Item = null;
			Occupied = false;
			Weapon = false;
			UpdatePrompt();
		}
		if (Item != null)
		{
			if (Weapon)
			{
				Item.transform.localPosition = new Vector3(0f, 0.29f, 0f);
				Item.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
			}
			else
			{
				Item.transform.localPosition = new Vector3(0f, 0f, -0.021f);
				Item.transform.localEulerAngles = Vector3.zero;
			}
		}
	}

	public void UpdatePrompt()
	{
		if (!Occupied)
		{
			if (Yandere.Armed)
			{
				Prompt.Label[0].text = "     Insert";
				Prompt.HideButton[0] = false;
			}
			else if (Yandere.PickUp != null)
			{
				if (Yandere.PickUp.Evidence || Yandere.PickUp.Suspicious)
				{
					Prompt.Label[0].text = "     Insert";
					Prompt.HideButton[0] = false;
				}
				else
				{
					Prompt.HideButton[0] = true;
				}
			}
			else
			{
				Prompt.HideButton[0] = true;
			}
		}
		else
		{
			Prompt.Label[0].text = "     Remove";
			Prompt.HideButton[0] = false;
		}
	}
}
