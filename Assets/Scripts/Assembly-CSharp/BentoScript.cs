using UnityEngine;

public class BentoScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public Transform PoisonSpot;

	public PromptScript Prompt;

	public int Poison;

	public int ID;

	private void Update()
	{
		if (!Prompt.Yandere.Inventory.EmeticPoison && !Prompt.Yandere.Inventory.RatPoison)
		{
			Prompt.HideButton[0] = true;
		}
		else
		{
			Prompt.HideButton[0] = false;
		}
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			if (Prompt.Yandere.Inventory.EmeticPoison)
			{
				Prompt.Yandere.Inventory.EmeticPoison = false;
				Prompt.Yandere.PoisonType = 1;
			}
			else
			{
				Prompt.Yandere.Inventory.RatPoison = false;
				Prompt.Yandere.PoisonType = 3;
			}
			Prompt.Yandere.CharacterAnimation.CrossFade("f02_poisoning_00");
			Prompt.Yandere.PoisonSpot = PoisonSpot;
			Prompt.Yandere.Poisoning = true;
			Prompt.Yandere.CanMove = false;
			base.enabled = false;
			Poison = 1;
			if (ID != 1)
			{
				StudentManager.Students[ID].Emetic = true;
			}
			Prompt.Hide();
			Prompt.enabled = false;
		}
		if (ID == 11 || ID == 6)
		{
			Prompt.HideButton[1] = !Prompt.Yandere.Inventory.LethalPoison;
			if (Prompt.Circle[1].fillAmount == 0f)
			{
				Prompt.Yandere.CharacterAnimation.CrossFade("f02_poisoning_00");
				Prompt.Yandere.Inventory.LethalPoison = false;
				StudentManager.Students[ID].Lethal = true;
				Prompt.Yandere.PoisonSpot = PoisonSpot;
				Prompt.Yandere.Poisoning = true;
				Prompt.Yandere.CanMove = false;
				Prompt.Yandere.PoisonType = 2;
				base.enabled = false;
				Poison = 2;
				Prompt.Hide();
				Prompt.enabled = false;
			}
		}
	}
}
