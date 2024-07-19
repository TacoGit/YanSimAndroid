using UnityEngine;

public class NotificationManagerScript : MonoBehaviour
{
	public YandereScript Yandere;

	public Transform NotificationSpawnPoint;

	public Transform NotificationParent;

	public GameObject Notification;

	public int NotificationsSpawned;

	public int Phase = 1;

	public ClockScript Clock;

	public string PersonaName;

	public string CustomText;

	private NotificationTypeAndStringDictionary NotificationMessages;

	private void Awake()
	{
		NotificationMessages = new NotificationTypeAndStringDictionary
		{
			{
				NotificationType.Bloody,
				"Visibly Bloody"
			},
			{
				NotificationType.Body,
				"Near Body"
			},
			{
				NotificationType.Insane,
				"Visibly Insane"
			},
			{
				NotificationType.Armed,
				"Visibly Armed"
			},
			{
				NotificationType.Lewd,
				"Visibly Lewd"
			},
			{
				NotificationType.Intrude,
				"Intruding"
			},
			{
				NotificationType.Late,
				"Late For Class"
			},
			{
				NotificationType.Info,
				"Learned New Info"
			},
			{
				NotificationType.Topic,
				"Learned New Topic"
			},
			{
				NotificationType.Opinion,
				"Learned Opinion"
			},
			{
				NotificationType.Complete,
				"Mission Complete"
			},
			{
				NotificationType.Exfiltrate,
				"Leave School"
			},
			{
				NotificationType.Evidence,
				"Evidence Recorded"
			},
			{
				NotificationType.ClassSoon,
				"Class Begins Soon"
			},
			{
				NotificationType.ClassNow,
				"Class Begins Now"
			},
			{
				NotificationType.Eavesdropping,
				"Eavesdropping"
			},
			{
				NotificationType.Persona,
				"Persona"
			},
			{
				NotificationType.Custom,
				CustomText
			}
		};
	}

	private void Update()
	{
		if (NotificationParent.localPosition.y > 0.001f + -0.049f * (float)NotificationsSpawned)
		{
			NotificationParent.localPosition = new Vector3(NotificationParent.localPosition.x, Mathf.Lerp(NotificationParent.localPosition.y, -0.049f * (float)NotificationsSpawned, Time.deltaTime * 10f), NotificationParent.localPosition.z);
		}
		if (Phase == 1)
		{
			if (Clock.HourTime > 8.4f)
			{
				Yandere.StudentManager.TutorialWindow.ShowClassMessage = true;
				DisplayNotification(NotificationType.ClassSoon);
				Phase++;
			}
		}
		else if (Phase == 2)
		{
			if (Clock.HourTime > 8.5f)
			{
				DisplayNotification(NotificationType.ClassNow);
				Phase++;
			}
		}
		else if (Phase == 3)
		{
			if (Clock.HourTime > 13.4f)
			{
				DisplayNotification(NotificationType.ClassSoon);
				Phase++;
			}
		}
		else if (Phase == 4 && Clock.HourTime > 13.5f)
		{
			DisplayNotification(NotificationType.ClassNow);
			Phase++;
		}
	}

	public void DisplayNotification(NotificationType Type)
	{
		if (!Yandere.Egg)
		{
			GameObject gameObject = Object.Instantiate(Notification);
			NotificationScript component = gameObject.GetComponent<NotificationScript>();
			gameObject.transform.parent = NotificationParent;
			gameObject.transform.localPosition = new Vector3(0f, 0.60275f + 0.049f * (float)NotificationsSpawned, 0f);
			gameObject.transform.localEulerAngles = Vector3.zero;
			component.NotificationManager = this;
			string value;
			bool flag = NotificationMessages.TryGetValue(Type, out value);
			if (Type != NotificationType.Persona && Type != NotificationType.Custom)
			{
				component.Label.text = value;
			}
			else if (Type == NotificationType.Custom)
			{
				component.Label.text = CustomText;
			}
			else
			{
				component.Label.text = PersonaName + " " + value;
			}
			NotificationsSpawned++;
			component.ID = NotificationsSpawned;
		}
	}
}
