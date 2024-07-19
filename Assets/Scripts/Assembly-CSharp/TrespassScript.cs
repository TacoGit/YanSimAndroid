using UnityEngine;

public class TrespassScript : MonoBehaviour
{
	public GameObject YandereObject;

	public YandereScript Yandere;

	public bool HideNotification;

	public bool OffLimits;

	private void OnTriggerEnter(Collider other)
	{
		if (!base.enabled || other.gameObject.layer != 13)
		{
			return;
		}
		YandereObject = other.gameObject;
		Yandere = other.gameObject.GetComponent<YandereScript>();
		if (Yandere != null)
		{
			if (!Yandere.Trespassing)
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Intrude);
			}
			Yandere.Trespassing = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (Yandere != null && other.gameObject == YandereObject)
		{
			Yandere.Trespassing = false;
		}
	}
}
