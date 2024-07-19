using UnityEngine;

public class PoisonBottleScript : MonoBehaviour
{
	public PromptScript Prompt;

	public int ID;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Yandere.TheftTimer = 0.1f;
			if (ID == 1)
			{
				Prompt.Yandere.Inventory.EmeticPoison = true;
			}
			else if (ID == 2)
			{
				Prompt.Yandere.Inventory.LethalPoison = true;
			}
			else if (ID == 3)
			{
				Prompt.Yandere.Inventory.RatPoison = true;
			}
			else if (ID == 4)
			{
				Prompt.Yandere.Inventory.HeadachePoison = true;
			}
			else if (ID == 5)
			{
				Prompt.Yandere.Inventory.Tranquilizer = true;
			}
			else if (ID == 6)
			{
				Prompt.Yandere.Inventory.Sedative = true;
			}
			Prompt.Yandere.StudentManager.UpdateAllBentos();
			Object.Destroy(base.gameObject);
		}
	}
}
