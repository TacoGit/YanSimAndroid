using UnityEngine;

public class DoorScript : MonoBehaviour
{
	[SerializeField]
	private Transform RelativeCharacter;

	[SerializeField]
	private HideColliderScript HideCollider;

	public StudentScript Student;

	[SerializeField]
	private YandereScript Yandere;

	[SerializeField]
	private BucketScript Bucket;

	public PromptScript Prompt;

	[SerializeField]
	private float[] ClosedPositions;

	[SerializeField]
	private float[] OpenPositions;

	[SerializeField]
	private Transform[] Doors;

	[SerializeField]
	private Texture[] Plates;

	[SerializeField]
	private UILabel[] Labels;

	[SerializeField]
	private float[] OriginX;

	[SerializeField]
	private bool CanSetBucket;

	[SerializeField]
	private bool HidingSpot;

	[SerializeField]
	private bool BucketSet;

	[SerializeField]
	private bool Swinging;

	public bool Locked;

	[SerializeField]
	private bool NoTrap;

	[SerializeField]
	private bool North;

	public bool Open;

	[SerializeField]
	private bool Near;

	[SerializeField]
	private float ShiftNorth = -0.1f;

	[SerializeField]
	private float ShiftSouth = 0.1f;

	[SerializeField]
	private float Rotation;

	public float Timer;

	[SerializeField]
	private float TrapSwing = 12.15f;

	[SerializeField]
	private float Swing = 150f;

	[SerializeField]
	private Renderer Sign;

	[SerializeField]
	private string RoomName = string.Empty;

	[SerializeField]
	private string Facing = string.Empty;

	[SerializeField]
	private int RoomID;

	[SerializeField]
	private ClubType Club;

	[SerializeField]
	private bool DisableSelf;

	private StudentManagerScript StudentManager;

	public int DoorID;

	private bool Double
	{
		get
		{
			return Doors.Length == 2;
		}
	}

	private void Start()
	{
		TrapSwing = 12.15f;
		Yandere = GameObject.Find("YandereChan").GetComponent<YandereScript>();
		StudentManager = Yandere.StudentManager;
		StudentManager.Doors[StudentManager.DoorID] = this;
		StudentManager.DoorID++;
		DoorID = StudentManager.DoorID;
		if (Swinging)
		{
			OriginX[0] = Doors[0].transform.localPosition.z;
			if (OriginX.Length > 1)
			{
				OriginX[1] = Doors[1].transform.localPosition.z;
			}
		}
		if (Labels.Length > 0)
		{
			Labels[0].text = RoomName;
			Labels[1].text = RoomName;
			UpdatePlate();
		}
		if (Club != 0 && ClubGlobals.GetClubClosed(Club))
		{
			Prompt.Hide();
			Prompt.enabled = false;
			base.enabled = false;
		}
		if (DisableSelf)
		{
			base.enabled = false;
		}
		Prompt.Student = false;
		Prompt.Door = true;
	}

	private void Update()
	{
		if (Prompt.DistanceSqr <= 1f)
		{
			if (Vector3.Distance(Yandere.transform.position, base.transform.position) < 2f)
			{
				if (!Near)
				{
					TopicCheck();
					Yandere.Location.Label.text = RoomName;
					Yandere.Location.Show = true;
					Near = true;
				}
				if (Prompt.Circle[0].fillAmount == 0f)
				{
					Prompt.Circle[0].fillAmount = 1f;
					if (!Open)
					{
						OpenDoor();
					}
					else
					{
						CloseDoor();
					}
				}
				if (Double && Swinging && Prompt.Circle[1].fillAmount == 0f)
				{
					Bucket = Yandere.PickUp.Bucket;
					Yandere.EmptyHands();
					Bucket.transform.parent = base.transform;
					Bucket.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					Bucket.Trap = true;
					Bucket.Prompt.Hide();
					Bucket.Prompt.enabled = false;
					CheckDirection();
					if (North)
					{
						Bucket.transform.localPosition = new Vector3(0f, 2.25f, 0.2975f);
					}
					else
					{
						Bucket.transform.localPosition = new Vector3(0f, 2.25f, -0.2975f);
					}
					Bucket.GetComponent<Rigidbody>().isKinematic = true;
					Bucket.GetComponent<Rigidbody>().useGravity = false;
					Prompt.HideButton[1] = true;
					CanSetBucket = false;
					BucketSet = true;
					Open = false;
					Timer = 0f;
					Prompt.enabled = false;
					Prompt.Hide();
				}
			}
		}
		else if (Near)
		{
			Yandere.Location.Show = false;
			Near = false;
		}
		if (Timer < 2f)
		{
			Timer += Time.deltaTime;
			if (BucketSet)
			{
				for (int i = 0; i < Doors.Length; i++)
				{
					Transform transform = Doors[i];
					transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(transform.localPosition.z, OriginX[i] + ((!North) ? ShiftNorth : ShiftSouth), Time.deltaTime * 3.6f));
					Rotation = Mathf.Lerp(Rotation, (!North) ? TrapSwing : (0f - TrapSwing), Time.deltaTime * 3.6f);
					transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (i != 0) ? (0f - Rotation) : Rotation, transform.localEulerAngles.z);
				}
			}
			else if (!Open)
			{
				for (int j = 0; j < Doors.Length; j++)
				{
					Transform transform2 = Doors[j];
					if (!Swinging)
					{
						transform2.localPosition = new Vector3(Mathf.Lerp(transform2.localPosition.x, ClosedPositions[j], Time.deltaTime * 3.6f), transform2.localPosition.y, transform2.localPosition.z);
						continue;
					}
					Rotation = Mathf.Lerp(Rotation, 0f, Time.deltaTime * 3.6f);
					transform2.localPosition = new Vector3(transform2.localPosition.x, transform2.localPosition.y, Mathf.Lerp(transform2.localPosition.z, OriginX[j], Time.deltaTime * 3.6f));
					transform2.localEulerAngles = new Vector3(transform2.localEulerAngles.x, (j != 0) ? (0f - Rotation) : Rotation, transform2.localEulerAngles.z);
				}
			}
			else
			{
				for (int k = 0; k < Doors.Length; k++)
				{
					Transform transform3 = Doors[k];
					if (!Swinging)
					{
						transform3.localPosition = new Vector3(Mathf.Lerp(transform3.localPosition.x, OpenPositions[k], Time.deltaTime * 3.6f), transform3.localPosition.y, transform3.localPosition.z);
						continue;
					}
					transform3.localPosition = new Vector3(transform3.localPosition.x, transform3.localPosition.y, Mathf.Lerp(transform3.localPosition.z, OriginX[k] + ((!North) ? ShiftSouth : ShiftNorth), Time.deltaTime * 3.6f));
					Rotation = Mathf.Lerp(Rotation, (!North) ? (0f - Swing) : Swing, Time.deltaTime * 3.6f);
					transform3.localEulerAngles = new Vector3(transform3.localEulerAngles.x, (k != 0) ? (0f - Rotation) : Rotation, transform3.localEulerAngles.z);
				}
			}
		}
		else if (Locked)
		{
			if (Prompt.Circle[0].fillAmount < 1f)
			{
				Prompt.Label[0].text = "     Locked";
				Prompt.Circle[0].fillAmount = 1f;
			}
			if (Yandere.Inventory.LockPick)
			{
				Prompt.HideButton[2] = false;
				if (Prompt.Circle[2].fillAmount == 0f)
				{
					Prompt.Yandere.Inventory.LockPick = false;
					Prompt.HideButton[2] = true;
					Locked = false;
				}
			}
			else if (!Prompt.HideButton[2])
			{
				Prompt.HideButton[2] = true;
			}
		}
		if (NoTrap || !Swinging || !Double)
		{
			return;
		}
		if (Yandere.PickUp != null)
		{
			if (Yandere.PickUp.Bucket != null)
			{
				if (Yandere.PickUp.GetComponent<BucketScript>().Full)
				{
					Prompt.HideButton[1] = false;
					CanSetBucket = true;
				}
				else if (CanSetBucket)
				{
					Prompt.HideButton[1] = true;
					CanSetBucket = false;
				}
			}
			else if (CanSetBucket)
			{
				Prompt.HideButton[1] = true;
				CanSetBucket = false;
			}
		}
		else if (CanSetBucket)
		{
			Prompt.HideButton[1] = true;
			CanSetBucket = false;
		}
	}

	public void OpenDoor()
	{
		Open = true;
		Timer = 0f;
		UpdateLabel();
		if (HidingSpot)
		{
			Object.Destroy(HideCollider.GetComponent<BoxCollider>());
		}
		CheckDirection();
		if (BucketSet)
		{
			Bucket.GetComponent<Rigidbody>().isKinematic = false;
			Bucket.GetComponent<Rigidbody>().useGravity = true;
			Bucket.UpdateAppearance = true;
			Bucket.Prompt.enabled = true;
			Bucket.Full = false;
			Bucket.Fly = true;
			Prompt.enabled = true;
			BucketSet = false;
		}
	}

	private void LockDoor()
	{
		Open = false;
		Prompt.Hide();
		Prompt.enabled = false;
	}

	private void CheckDirection()
	{
		North = false;
		RelativeCharacter = ((!(Student != null)) ? Yandere.transform : Student.transform);
		if (Facing == "North")
		{
			if (RelativeCharacter.position.z < base.transform.position.z)
			{
				North = true;
			}
		}
		else if (Facing == "South")
		{
			if (RelativeCharacter.position.z > base.transform.position.z)
			{
				North = true;
			}
		}
		else if (Facing == "East")
		{
			if (RelativeCharacter.position.x < base.transform.position.x)
			{
				North = true;
			}
		}
		else if (Facing == "West" && RelativeCharacter.position.x > base.transform.position.x)
		{
			North = true;
		}
		Student = null;
	}

	public void CloseDoor()
	{
		Open = false;
		Timer = 0f;
		UpdateLabel();
		if (HidingSpot)
		{
			HideCollider.gameObject.AddComponent<BoxCollider>();
			BoxCollider component = HideCollider.GetComponent<BoxCollider>();
			component.size = new Vector3(component.size.x, component.size.y, 2f);
			component.isTrigger = true;
			HideCollider.MyCollider = component;
		}
	}

	private void UpdateLabel()
	{
		if (Open)
		{
			Prompt.Label[0].text = "     Close";
		}
		else
		{
			Prompt.Label[0].text = "     Open";
		}
	}

	private void UpdatePlate()
	{
		switch (RoomID)
		{
		case 1:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0f, 0.75f);
			break;
		case 2:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0f, 0.5f);
			break;
		case 3:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0f, 0.25f);
			break;
		case 4:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0f, 0f);
			break;
		case 5:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.25f, 0.75f);
			break;
		case 6:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.25f, 0.5f);
			break;
		case 7:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.25f, 0.25f);
			break;
		case 8:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.25f, 0f);
			break;
		case 9:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.5f, 0.75f);
			break;
		case 10:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.5f, 0.5f);
			break;
		case 11:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.5f, 0.25f);
			break;
		case 12:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.5f, 0f);
			break;
		case 13:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.75f, 0.75f);
			break;
		case 14:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.75f, 0.5f);
			break;
		case 15:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.75f, 0.25f);
			break;
		case 16:
			Sign.material.mainTexture = Plates[1];
			Sign.material.mainTextureOffset = new Vector2(0.75f, 0f);
			break;
		case 17:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0f, 0.75f);
			break;
		case 18:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0f, 0.5f);
			break;
		case 19:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0f, 0.25f);
			break;
		case 20:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0f, 0f);
			break;
		case 21:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.25f, 0.75f);
			break;
		case 22:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.25f, 0.5f);
			break;
		case 23:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.25f, 0.25f);
			break;
		case 24:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.25f, 0f);
			break;
		case 25:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.5f, 0.75f);
			break;
		case 26:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.5f, 0.5f);
			break;
		case 27:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.5f, 0.25f);
			break;
		case 28:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.5f, 0f);
			break;
		case 29:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.75f, 0.75f);
			break;
		case 30:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.75f, 0.5f);
			break;
		case 31:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.75f, 0.25f);
			break;
		case 32:
			Sign.material.mainTexture = Plates[2];
			Sign.material.mainTextureOffset = new Vector2(0.75f, 0f);
			break;
		case 33:
			Sign.material.mainTexture = Plates[3];
			Sign.material.mainTextureOffset = new Vector2(0f, 0.75f);
			break;
		case 34:
			Sign.material.mainTexture = Plates[3];
			Sign.material.mainTextureOffset = new Vector2(0f, 0.5f);
			break;
		}
	}

	private void TopicCheck()
	{
		if (RoomID > 25 && RoomID < 37)
		{
			StudentManager.TutorialWindow.ShowClubMessage = true;
		}
		switch (RoomID)
		{
		case 1:
			break;
		case 2:
			break;
		case 3:
			if (!ConversationGlobals.GetTopicDiscovered(12))
			{
				ConversationGlobals.SetTopicDiscovered(12, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		case 9:
			break;
		case 10:
			break;
		case 11:
			break;
		case 12:
			break;
		case 13:
			if (!ConversationGlobals.GetTopicDiscovered(21))
			{
				ConversationGlobals.SetTopicDiscovered(21, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 14:
			break;
		case 15:
			if (!ConversationGlobals.GetTopicDiscovered(11))
			{
				ConversationGlobals.SetTopicDiscovered(11, true);
				ConversationGlobals.SetTopicDiscovered(12, true);
				ConversationGlobals.SetTopicDiscovered(13, true);
				ConversationGlobals.SetTopicDiscovered(14, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 16:
			break;
		case 17:
			break;
		case 18:
			break;
		case 19:
			break;
		case 20:
			break;
		case 21:
			break;
		case 22:
			break;
		case 23:
			break;
		case 24:
			break;
		case 25:
			break;
		case 26:
			if (!ConversationGlobals.GetTopicDiscovered(1))
			{
				ConversationGlobals.SetTopicDiscovered(1, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 27:
			if (!ConversationGlobals.GetTopicDiscovered(2))
			{
				ConversationGlobals.SetTopicDiscovered(2, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 28:
			if (!ConversationGlobals.GetTopicDiscovered(3))
			{
				ConversationGlobals.SetTopicDiscovered(3, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 29:
			if (!ConversationGlobals.GetTopicDiscovered(4))
			{
				ConversationGlobals.SetTopicDiscovered(4, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 30:
			if (!ConversationGlobals.GetTopicDiscovered(5))
			{
				ConversationGlobals.SetTopicDiscovered(5, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 31:
			if (!ConversationGlobals.GetTopicDiscovered(6))
			{
				ConversationGlobals.SetTopicDiscovered(6, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 32:
			if (!ConversationGlobals.GetTopicDiscovered(7))
			{
				ConversationGlobals.SetTopicDiscovered(7, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 33:
			break;
		case 34:
			if (!ConversationGlobals.GetTopicDiscovered(8))
			{
				ConversationGlobals.SetTopicDiscovered(8, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 35:
			if (!ConversationGlobals.GetTopicDiscovered(9))
			{
				ConversationGlobals.SetTopicDiscovered(9, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 36:
			if (!ConversationGlobals.GetTopicDiscovered(10))
			{
				ConversationGlobals.SetTopicDiscovered(10, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
			}
			break;
		case 37:
			break;
		}
	}
}
