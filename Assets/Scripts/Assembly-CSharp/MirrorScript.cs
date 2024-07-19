using UnityEngine;

public class MirrorScript : MonoBehaviour
{
	public PromptScript Prompt;

	public string[] Personas;

	public string[] Idles;

	public string[] Walks;

	public int ID;

	public int Limit;

	private void Start()
	{
		Limit = Idles.Length - 1;
	}

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			if (Prompt.Yandere.Health > 0)
			{
				Prompt.Circle[0].fillAmount = 1f;
				ID++;
				if (ID == Limit)
				{
					ID = 0;
				}
				UpdatePersona();
			}
		}
		else if (Prompt.Circle[1].fillAmount == 0f && Prompt.Yandere.Health > 0)
		{
			Prompt.Circle[1].fillAmount = 1f;
			ID--;
			if (ID < 0)
			{
				ID = Limit - 1;
			}
			UpdatePersona();
		}
	}

	private void UpdatePersona()
	{
		if (!Prompt.Yandere.Carrying)
		{
			Prompt.Yandere.NotificationManager.PersonaName = Personas[ID];
			Prompt.Yandere.NotificationManager.DisplayNotification(NotificationType.Persona);
			Prompt.Yandere.IdleAnim = Idles[ID];
			Prompt.Yandere.WalkAnim = Walks[ID];
			Prompt.Yandere.UpdatePersona(ID);
		}
		Prompt.Yandere.OriginalIdleAnim = Idles[ID];
		Prompt.Yandere.OriginalWalkAnim = Walks[ID];
		Prompt.Yandere.StudentManager.UpdatePerception();
	}
}
