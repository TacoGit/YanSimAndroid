using UnityEngine;

public class InterestManagerScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public YandereScript Yandere;

	public Transform[] Clubs;

	public Transform DelinquentZone;

	public Transform Library;

	public Transform Kitten;

	private void Start()
	{
		ConversationGlobals.SetTopicDiscovered(22, true);
		ConversationGlobals.SetTopicDiscovered(23, true);
		ConversationGlobals.SetTopicDiscovered(24, true);
	}

	private void Update()
	{
		if (!(Yandere.Follower != null))
		{
			return;
		}
		int studentID = Yandere.Follower.StudentID;
		for (int i = 1; i < 11; i++)
		{
			if (!ConversationGlobals.GetTopicLearnedByStudent(i, studentID) && Vector3.Distance(Yandere.Follower.transform.position, Clubs[i].position) < 5f)
			{
				if (!ConversationGlobals.GetTopicDiscovered(i))
				{
					Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
					ConversationGlobals.SetTopicDiscovered(i, true);
				}
				Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
				ConversationGlobals.SetTopicLearnedByStudent(i, studentID, true);
			}
		}
		if (!ConversationGlobals.GetTopicLearnedByStudent(11, studentID) && Vector3.Distance(Yandere.Follower.transform.position, Clubs[11].position) < 5f)
		{
			if (!ConversationGlobals.GetTopicDiscovered(11))
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				ConversationGlobals.SetTopicDiscovered(11, true);
				ConversationGlobals.SetTopicDiscovered(12, true);
				ConversationGlobals.SetTopicDiscovered(13, true);
				ConversationGlobals.SetTopicDiscovered(14, true);
			}
			Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
			ConversationGlobals.SetTopicLearnedByStudent(11, studentID, true);
			ConversationGlobals.SetTopicLearnedByStudent(12, studentID, true);
			ConversationGlobals.SetTopicLearnedByStudent(13, studentID, true);
			ConversationGlobals.SetTopicLearnedByStudent(14, studentID, true);
		}
		if (!ConversationGlobals.GetTopicLearnedByStudent(15, studentID) && Vector3.Distance(Yandere.Follower.transform.position, Kitten.position) < 2.5f)
		{
			if (!ConversationGlobals.GetTopicDiscovered(15))
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				ConversationGlobals.SetTopicDiscovered(15, true);
			}
			Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
			ConversationGlobals.SetTopicLearnedByStudent(15, studentID, true);
		}
		if (!ConversationGlobals.GetTopicLearnedByStudent(16, studentID) && Vector3.Distance(Yandere.Follower.transform.position, Clubs[6].position) < 5f)
		{
			if (!ConversationGlobals.GetTopicDiscovered(16))
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				ConversationGlobals.SetTopicDiscovered(16, true);
			}
			Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
			ConversationGlobals.SetTopicLearnedByStudent(16, studentID, true);
		}
		if (!ConversationGlobals.GetTopicLearnedByStudent(17, studentID) && Vector3.Distance(Yandere.Follower.transform.position, DelinquentZone.position) < 5f)
		{
			if (!ConversationGlobals.GetTopicDiscovered(17))
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				ConversationGlobals.SetTopicDiscovered(17, true);
			}
			Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
			ConversationGlobals.SetTopicLearnedByStudent(17, studentID, true);
		}
		if (!ConversationGlobals.GetTopicLearnedByStudent(18, studentID) && Vector3.Distance(Yandere.Follower.transform.position, Library.position) < 5f)
		{
			if (!ConversationGlobals.GetTopicDiscovered(18))
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				ConversationGlobals.SetTopicDiscovered(18, true);
			}
			Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
			ConversationGlobals.SetTopicLearnedByStudent(18, studentID, true);
		}
	}
}
