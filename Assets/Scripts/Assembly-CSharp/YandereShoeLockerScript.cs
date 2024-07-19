using UnityEngine;

public class YandereShoeLockerScript : MonoBehaviour
{
	public YandereScript Yandere;

	public PromptScript Prompt;

	public int Label = 1;

	private void Update()
	{
		if (Yandere.Schoolwear == 1 && !Yandere.ClubAttire && !Yandere.Egg)
		{
			if (Label == 2)
			{
				Prompt.Label[0].text = "     Change Shoes";
				Label = 1;
			}
			if (Prompt.Circle[0].fillAmount == 0f)
			{
				Prompt.Circle[0].fillAmount = 1f;
				Yandere.Casual = !Yandere.Casual;
				Yandere.ChangeSchoolwear();
				Yandere.CanMove = true;
			}
		}
		else
		{
			Prompt.Circle[0].fillAmount = 1f;
			if (Label == 1)
			{
				Prompt.Label[0].text = "     Not Available";
				Label = 2;
			}
		}
	}
}
