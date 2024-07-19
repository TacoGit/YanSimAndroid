using UnityEngine;

public class HomeSenpaiShrineScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public PauseScreenScript PauseScreen;

	public HomeYandereScript HomeYandere;

	public HomeCameraScript HomeCamera;

	public HomeWindowScript HomeWindow;

	public Transform[] Destinations;

	public Transform[] Targets;

	public Transform RightDoor;

	public Transform LeftDoor;

	public UILabel NameLabel;

	public UILabel DescLabel;

	public string[] Names;

	public string[] Descs;

	public float Rotation;

	private int Rows = 5;

	private int Columns = 3;

	private int X = 1;

	private int Y = 3;

	private void Start()
	{
		UpdateText(GetCurrentIndex());
	}

	private bool InUpperHalf()
	{
		return Y < 2;
	}

	private int GetCurrentIndex()
	{
		if (InUpperHalf())
		{
			return Y;
		}
		return 2 + (X + (Y - 2) * Columns);
	}

	private void Update()
	{
		if (!HomeYandere.CanMove && !PauseScreen.Show)
		{
			if (HomeCamera.ID == 6)
			{
				Rotation = Mathf.Lerp(Rotation, 135f, Time.deltaTime * 10f);
				RightDoor.localEulerAngles = new Vector3(RightDoor.localEulerAngles.x, Rotation, RightDoor.localEulerAngles.z);
				LeftDoor.localEulerAngles = new Vector3(LeftDoor.localEulerAngles.x, 0f - Rotation, LeftDoor.localEulerAngles.z);
				if (InputManager.TappedUp)
				{
					Y = ((Y <= 0) ? (Rows - 1) : (Y - 1));
				}
				if (InputManager.TappedDown)
				{
					Y = ((Y < Rows - 1) ? (Y + 1) : 0);
				}
				if (InputManager.TappedRight && !InUpperHalf())
				{
					X = ((X < Columns - 1) ? (X + 1) : 0);
				}
				if (InputManager.TappedLeft && !InUpperHalf())
				{
					X = ((X <= 0) ? (Columns - 1) : (X - 1));
				}
				if (InUpperHalf())
				{
					X = 1;
				}
				int currentIndex = GetCurrentIndex();
				HomeCamera.Destination = Destinations[currentIndex];
				HomeCamera.Target = Targets[currentIndex];
				if (InputManager.TappedUp || InputManager.TappedDown || InputManager.TappedRight || InputManager.TappedLeft)
				{
					UpdateText(currentIndex);
				}
				if (Input.GetButtonDown("B"))
				{
					HomeCamera.Destination = HomeCamera.Destinations[0];
					HomeCamera.Target = HomeCamera.Targets[0];
					HomeYandere.CanMove = true;
					HomeYandere.gameObject.SetActive(true);
					HomeWindow.Show = false;
				}
			}
		}
		else
		{
			Rotation = Mathf.Lerp(Rotation, 0f, Time.deltaTime * 10f);
			RightDoor.localEulerAngles = new Vector3(RightDoor.localEulerAngles.x, Rotation, RightDoor.localEulerAngles.z);
			LeftDoor.localEulerAngles = new Vector3(LeftDoor.localEulerAngles.x, Rotation, LeftDoor.localEulerAngles.z);
		}
	}

	private void UpdateText(int newIndex)
	{
		NameLabel.text = Names[newIndex];
		DescLabel.text = Descs[newIndex];
	}
}
