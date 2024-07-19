using UnityEngine;

public class GenericBentoScript : MonoBehaviour
{
	public PromptScript Prompt;

	public bool Emetic;

	public bool Tranquil;

	public bool Headache;

	public bool Lethal;

	public bool Tampered;

	public int StudentID;

	private void Update()
	{
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
			Emetic = true;
			ShutOff();
		}
		else if (Prompt.Circle[1].fillAmount == 0f)
		{
			if (Prompt.Yandere.Inventory.Sedative)
			{
				Prompt.Yandere.Inventory.Sedative = false;
			}
			else
			{
				Prompt.Yandere.Inventory.Tranquilizer = false;
			}
			Prompt.Yandere.PoisonType = 4;
			Tranquil = true;
			ShutOff();
		}
		else if (Prompt.Circle[2].fillAmount == 0f)
		{
			if (Prompt.Yandere.Inventory.LethalPoison)
			{
				Prompt.Yandere.Inventory.LethalPoison = false;
				Prompt.Yandere.PoisonType = 2;
			}
			else
			{
				Prompt.Yandere.Inventory.ChemicalPoison = false;
				Prompt.Yandere.PoisonType = 2;
			}
			Lethal = true;
			ShutOff();
		}
		else if (Prompt.Circle[3].fillAmount == 0f)
		{
			Prompt.Yandere.Inventory.HeadachePoison = false;
			Prompt.Yandere.PoisonType = 5;
			Headache = true;
			ShutOff();
		}
	}

	private void ShutOff()
	{
		Prompt.Yandere.CharacterAnimation["f02_poisoning_00"].speed = 2f;
		Prompt.Yandere.CharacterAnimation.CrossFade("f02_poisoning_00");
		Prompt.Yandere.StudentManager.UpdateAllBentos();
		Prompt.Yandere.TargetBento = this;
		Prompt.Yandere.Poisoning = true;
		Prompt.Yandere.CanMove = false;
		Tampered = true;
		base.enabled = false;
		Prompt.enabled = false;
		Prompt.Hide();
	}

	public void UpdatePrompts()
	{
		Prompt.HideButton[0] = true;
		Prompt.HideButton[1] = true;
		Prompt.HideButton[2] = true;
		Prompt.HideButton[3] = true;
		if (Prompt.Yandere.Inventory.EmeticPoison || Prompt.Yandere.Inventory.RatPoison)
		{
			Prompt.HideButton[0] = false;
		}
		if (Prompt.Yandere.Inventory.Tranquilizer || Prompt.Yandere.Inventory.Sedative)
		{
			Prompt.HideButton[1] = false;
		}
		if (Prompt.Yandere.Inventory.LethalPoison || Prompt.Yandere.Inventory.ChemicalPoison)
		{
			Prompt.HideButton[2] = false;
		}
		if (Prompt.Yandere.Inventory.HeadachePoison)
		{
			Prompt.HideButton[3] = false;
		}
	}
}
