using System.Collections;
using UnityEngine;

public class PhotoGalleryScript : MonoBehaviour
{
	public HomeCorkboardPhotoScript[] CorkboardPhotographs;

	public StringScript[] CorkboardStrings;

	public int PhotoID;

	public StringScript String;

	public InputManagerScript InputManager;

	public PauseScreenScript PauseScreen;

	public TaskManagerScript TaskManager;

	public InputDeviceScript InputDevice;

	public PromptBarScript PromptBar;

	public HomeCursorScript Cursor;

	public YandereScript Yandere;

	public GameObject MovingPhotograph;

	public GameObject LoadingScreen;

	public GameObject Photograph;

	public GameObject StringSet;

	public Transform CorkboardPanel;

	public Transform Destination;

	public Transform Highlight;

	public Transform Gallery;

	public Transform StringParent;

	public UITexture[] Photographs;

	public UISprite[] Hearts;

	public UITexture ViewPhoto;

	public Texture NoPhoto;

	public Vector2 PreviousPosition;

	public Vector2 MouseDelta;

	public bool DoNotRaisePromptBar;

	public bool SpawnedPhotos;

	public bool MovingString;

	public bool NamingBully;

	public bool Adjusting;

	public bool CanAdjust;

	public bool Corkboard;

	public bool GotPhotos;

	public bool Viewing;

	public bool Moving;

	public bool Reset;

	public int StringPhase;

	public int Strings;

	public int Photos;

	public int Column;

	public int Row;

	public float MaxPhotoX = 4150f;

	public float MaxPhotoY = 2500f;

	private const float MaxCursorX = 4788f;

	private const float MaxCursorY = 3122f;

	private const float CorkboardAspectRatio = 1.5336323f;

	public float SpeedLimit;

	private int CurrentIndex
	{
		get
		{
			return Column + (Row - 1) * 5;
		}
	}

	private float LerpSpeed
	{
		get
		{
			return Time.unscaledDeltaTime * 10f;
		}
	}

	private float HighlightX
	{
		get
		{
			return -450f + 150f * (float)Column;
		}
	}

	private float HighlightY
	{
		get
		{
			return 225f - 75f * (float)Row;
		}
	}

	private float MovingPhotoXPercent
	{
		get
		{
			float num = 0f - MaxPhotoX;
			float maxPhotoX = MaxPhotoX;
			return (MovingPhotograph.transform.localPosition.x - num) / (maxPhotoX - num);
		}
		set
		{
			MovingPhotograph.transform.localPosition = new Vector3(0f - MaxPhotoX + 2f * (MaxPhotoX * Mathf.Clamp01(value)), MovingPhotograph.transform.localPosition.y, MovingPhotograph.transform.localPosition.z);
		}
	}

	private float MovingPhotoYPercent
	{
		get
		{
			float num = 0f - MaxPhotoY;
			float maxPhotoY = MaxPhotoY;
			return (MovingPhotograph.transform.localPosition.y - num) / (maxPhotoY - num);
		}
		set
		{
			MovingPhotograph.transform.localPosition = new Vector3(MovingPhotograph.transform.localPosition.x, 0f - MaxPhotoY + 2f * (MaxPhotoY * Mathf.Clamp01(value)), MovingPhotograph.transform.localPosition.z);
		}
	}

	private float MovingPhotoRotation
	{
		get
		{
			return MovingPhotograph.transform.localEulerAngles.z;
		}
		set
		{
			MovingPhotograph.transform.localEulerAngles = new Vector3(MovingPhotograph.transform.localEulerAngles.x, MovingPhotograph.transform.localEulerAngles.y, value);
		}
	}

	private float CursorXPercent
	{
		get
		{
			float num = -4788f;
			float num2 = 4788f;
			return (Cursor.transform.localPosition.x - num) / (num2 - num);
		}
		set
		{
			Cursor.transform.localPosition = new Vector3(-4788f + 2f * (4788f * Mathf.Clamp01(value)), Cursor.transform.localPosition.y, Cursor.transform.localPosition.z);
		}
	}

	private float CursorYPercent
	{
		get
		{
			float num = -3122f;
			float num2 = 3122f;
			return (Cursor.transform.localPosition.y - num) / (num2 - num);
		}
		set
		{
			Cursor.transform.localPosition = new Vector3(Cursor.transform.localPosition.x, -3122f + 2f * (3122f * Mathf.Clamp01(value)), Cursor.transform.localPosition.z);
		}
	}

	private void Start()
	{
		if (Cursor != null)
		{
			Cursor.gameObject.SetActive(false);
			base.enabled = false;
		}
		if (Corkboard)
		{
			StartCoroutine(GetPhotos());
		}
	}

	private void UpdatePhotoSelection()
	{
		if (Input.GetButtonDown("A"))
		{
			if (!NamingBully)
			{
				UITexture uITexture = Photographs[CurrentIndex];
				if (uITexture.mainTexture != NoPhoto)
				{
					ViewPhoto.mainTexture = uITexture.mainTexture;
					ViewPhoto.transform.position = uITexture.transform.position;
					ViewPhoto.transform.localScale = uITexture.transform.localScale;
					Destination.position = uITexture.transform.position;
					Viewing = true;
					if (!Corkboard)
					{
						for (int i = 1; i < 26; i++)
						{
							Hearts[i].gameObject.SetActive(false);
						}
					}
					CanAdjust = false;
				}
				UpdateButtonPrompts();
			}
			else
			{
				UITexture uITexture2 = Photographs[CurrentIndex];
				if (uITexture2.mainTexture != NoPhoto && PlayerGlobals.GetBullyPhoto(CurrentIndex) > 0)
				{
					Yandere.Police.EndOfDay.FragileTarget = PlayerGlobals.GetBullyPhoto(CurrentIndex);
					Yandere.StudentManager.FragileOfferHelp.Continue();
					PauseScreen.MainMenu.SetActive(true);
					Yandere.RPGCamera.enabled = true;
					base.gameObject.SetActive(false);
					PauseScreen.Show = false;
					PromptBar.Show = false;
					NamingBully = false;
					Time.timeScale = 1f;
				}
			}
		}
		if (!NamingBully && Input.GetButtonDown("B"))
		{
			PromptBar.ClearButtons();
			PromptBar.Label[0].text = "Accept";
			PromptBar.Label[1].text = "Exit";
			PromptBar.Label[4].text = "Choose";
			PromptBar.Label[5].text = "Choose";
			PromptBar.UpdateButtons();
			PauseScreen.MainMenu.SetActive(true);
			PauseScreen.Sideways = false;
			PauseScreen.PressedB = true;
			base.gameObject.SetActive(false);
			UpdateButtonPrompts();
		}
		if (Input.GetButtonDown("X"))
		{
			ViewPhoto.mainTexture = null;
			int currentIndex = CurrentIndex;
			if (Photographs[currentIndex].mainTexture != NoPhoto)
			{
				Photographs[currentIndex].mainTexture = NoPhoto;
				PlayerGlobals.SetPhoto(currentIndex, false);
				PlayerGlobals.SetSenpaiPhoto(currentIndex, false);
				TaskGlobals.SetGuitarPhoto(currentIndex, false);
				TaskGlobals.SetKittenPhoto(currentIndex, false);
				Hearts[currentIndex].gameObject.SetActive(false);
				TaskManager.UpdateTaskStatus();
			}
			UpdateButtonPrompts();
		}
		if (Corkboard)
		{
			if (Input.GetButtonDown("Y"))
			{
				CanAdjust = false;
				Cursor.gameObject.SetActive(true);
				Adjusting = true;
				UpdateButtonPrompts();
			}
		}
		else if (CanAdjust && Input.GetButtonDown("Y"))
		{
			int currentIndex2 = CurrentIndex;
			PlayerGlobals.SetSenpaiPhoto(currentIndex2, false);
			Hearts[currentIndex2].gameObject.SetActive(false);
			CanAdjust = false;
			Yandere.Sanity += 20f;
			UpdateButtonPrompts();
		}
		if (InputManager.TappedRight)
		{
			Column = ((Column >= 5) ? 1 : (Column + 1));
		}
		if (InputManager.TappedLeft)
		{
			Column = ((Column <= 1) ? 5 : (Column - 1));
		}
		if (InputManager.TappedUp)
		{
			Row = ((Row <= 1) ? 5 : (Row - 1));
		}
		if (InputManager.TappedDown)
		{
			Row = ((Row >= 5) ? 1 : (Row + 1));
		}
		bool flag = InputManager.TappedRight || InputManager.TappedLeft;
		bool flag2 = InputManager.TappedUp || InputManager.TappedDown;
		if (flag || flag2)
		{
			Highlight.transform.localPosition = new Vector3(HighlightX, HighlightY, Highlight.transform.localPosition.z);
			UpdateButtonPrompts();
		}
		ViewPhoto.transform.localScale = Vector3.Lerp(ViewPhoto.transform.localScale, new Vector3(1f, 1f, 1f), LerpSpeed);
		ViewPhoto.transform.position = Vector3.Lerp(ViewPhoto.transform.position, Destination.position, LerpSpeed);
		if (Corkboard)
		{
			Gallery.transform.localPosition = new Vector3(Gallery.transform.localPosition.x, Mathf.Lerp(Gallery.transform.localPosition.y, 0f, Time.deltaTime * 10f), Gallery.transform.localPosition.z);
		}
	}

	private void UpdatePhotoViewing()
	{
		ViewPhoto.transform.localScale = Vector3.Lerp(ViewPhoto.transform.localScale, (!Corkboard) ? new Vector3(6.5f, 6.5f, 6.5f) : new Vector3(5.8f, 5.8f, 5.8f), LerpSpeed);
		ViewPhoto.transform.localPosition = Vector3.Lerp(ViewPhoto.transform.localPosition, Vector3.zero, LerpSpeed);
		if (Corkboard && Input.GetButtonDown("A"))
		{
			GameObject gameObject = Object.Instantiate(Photograph, base.transform.position, Quaternion.identity);
			gameObject.transform.parent = CorkboardPanel;
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.GetComponent<UITexture>().mainTexture = Photographs[CurrentIndex].mainTexture;
			MovingPhotograph = gameObject;
			CanAdjust = false;
			Adjusting = true;
			Viewing = false;
			Moving = true;
			CorkboardPhotographs[Photos] = gameObject.GetComponent<HomeCorkboardPhotoScript>();
			CorkboardPhotographs[Photos].ID = CurrentIndex;
			CorkboardPhotographs[Photos].ArrayID = Photos;
			Photos++;
			UpdateButtonPrompts();
		}
		if (!Input.GetButtonDown("B"))
		{
			return;
		}
		Viewing = false;
		if (Corkboard)
		{
			Cursor.Highlight.transform.position = new Vector3(Cursor.Highlight.transform.position.x, 100f, Cursor.Highlight.transform.position.z);
			CanAdjust = true;
		}
		else
		{
			for (int i = 1; i < 26; i++)
			{
				if (PlayerGlobals.GetSenpaiPhoto(i))
				{
					Hearts[i].gameObject.SetActive(true);
					CanAdjust = true;
				}
			}
		}
		UpdateButtonPrompts();
	}

	private void UpdateCorkboardPhoto()
	{
		if (Input.GetMouseButton(1))
		{
			MovingPhotoRotation += MouseDelta.x;
		}
		else
		{
			MovingPhotograph.transform.localPosition = new Vector3(MovingPhotograph.transform.localPosition.x + MouseDelta.x * 8.66666f, MovingPhotograph.transform.localPosition.y + MouseDelta.y * 8.66666f, 0f);
		}
		if (Input.GetButton("LB"))
		{
			MovingPhotoRotation += Time.deltaTime * 100f;
		}
		if (Input.GetButton("RB"))
		{
			MovingPhotoRotation -= Time.deltaTime * 100f;
		}
		if (Input.GetButton("Y"))
		{
			MovingPhotograph.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
			if (MovingPhotograph.transform.localScale.x > 2f)
			{
				MovingPhotograph.transform.localScale = new Vector3(2f, 2f, 2f);
			}
		}
		if (Input.GetButton("X"))
		{
			MovingPhotograph.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
			if (MovingPhotograph.transform.localScale.x < 1f)
			{
				MovingPhotograph.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		Vector2 vector = new Vector2(MovingPhotograph.transform.localPosition.x, MovingPhotograph.transform.localPosition.y);
		Vector2 vector2 = new Vector2(Input.GetAxis("Horizontal") * 86.66666f * SpeedLimit, Input.GetAxis("Vertical") * 86.66666f * SpeedLimit);
		MovingPhotograph.transform.localPosition = new Vector3(Mathf.Clamp(vector.x + vector2.x, 0f - MaxPhotoX, MaxPhotoX), Mathf.Clamp(vector.y + vector2.y, 0f - MaxPhotoY, MaxPhotoY), MovingPhotograph.transform.localPosition.z);
		if (Input.GetButtonDown("A"))
		{
			Cursor.transform.localPosition = MovingPhotograph.transform.localPosition;
			Cursor.gameObject.SetActive(true);
			Moving = false;
			UpdateButtonPrompts();
			PhotoID++;
		}
	}

	private void UpdateString()
	{
		MouseDelta.x += Input.GetAxis("Horizontal") * 8.66666f * SpeedLimit;
		MouseDelta.y += Input.GetAxis("Vertical") * 8.66666f * SpeedLimit;
		Transform transform = null;
		if (StringPhase == 0)
		{
			transform = String.Origin;
			String.Target.position = String.Origin.position;
		}
		else
		{
			transform = String.Target;
		}
		transform.localPosition = new Vector3(transform.localPosition.x - MouseDelta.x * Time.deltaTime * 0.33333f, transform.localPosition.y + MouseDelta.y * Time.deltaTime * 0.33333f, 0f);
		if (transform.localPosition.x > 0.971f)
		{
			transform.localPosition = new Vector3(0.971f, transform.localPosition.y, transform.localPosition.z);
		}
		else if (transform.localPosition.x < -0.971f)
		{
			transform.localPosition = new Vector3(-0.971f, transform.localPosition.y, transform.localPosition.z);
		}
		if (transform.localPosition.y > 0.637f)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, 0.637f, transform.localPosition.z);
		}
		else if (transform.localPosition.y < -0.637f)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, -0.637f, transform.localPosition.z);
		}
		if (Input.GetButtonDown("A"))
		{
			if (StringPhase == 0)
			{
				StringPhase++;
			}
			else if (StringPhase == 1)
			{
				Cursor.transform.localPosition = transform.localPosition;
				Cursor.gameObject.SetActive(true);
				MovingString = false;
				StringPhase = 0;
				UpdateButtonPrompts();
			}
		}
	}

	private void UpdateCorkboardCursor()
	{
		Vector2 vector = new Vector2(Cursor.transform.localPosition.x, Cursor.transform.localPosition.y);
		Vector2 vector2 = new Vector2(MouseDelta.x * 8.66666f + Input.GetAxis("Horizontal") * 86.66666f * SpeedLimit, MouseDelta.y * 8.66666f + Input.GetAxis("Vertical") * 86.66666f * SpeedLimit);
		Cursor.transform.localPosition = new Vector3(Mathf.Clamp(vector.x + vector2.x, -4788f, 4788f), Mathf.Clamp(vector.y + vector2.y, -3122f, 3122f), Cursor.transform.localPosition.z);
		if (Input.GetButtonDown("A") && Cursor.Photograph != null)
		{
			Cursor.Highlight.transform.position = new Vector3(Cursor.Highlight.transform.position.x, 100f, Cursor.Highlight.transform.position.z);
			MovingPhotograph = Cursor.Photograph;
			Cursor.gameObject.SetActive(false);
			Moving = true;
			UpdateButtonPrompts();
		}
		if (Input.GetButtonDown("Y"))
		{
			GameObject gameObject = Object.Instantiate(StringSet, base.transform.position, Quaternion.identity);
			gameObject.transform.parent = StringParent;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			String = gameObject.GetComponent<StringScript>();
			Cursor.gameObject.SetActive(false);
			MovingString = true;
			CorkboardStrings[Strings] = String.GetComponent<StringScript>();
			CorkboardStrings[Strings].ArrayID = Strings;
			Strings++;
			UpdateButtonPrompts();
		}
		if (Input.GetButtonDown("B"))
		{
			if (Cursor.Photograph != null)
			{
				Cursor.Photograph = null;
			}
			Cursor.transform.localPosition = new Vector3(0f, 0f, Cursor.transform.localPosition.z);
			Cursor.Highlight.transform.position = new Vector3(Cursor.Highlight.transform.position.x, 100f, Cursor.Highlight.transform.position.z);
			CanAdjust = true;
			Cursor.gameObject.SetActive(false);
			Adjusting = false;
			UpdateButtonPrompts();
		}
		if (Input.GetButtonDown("X"))
		{
			if (Cursor.Photograph != null)
			{
				Cursor.Highlight.transform.position = new Vector3(Cursor.Highlight.transform.position.x, 100f, Cursor.Highlight.transform.position.z);
				Shuffle(Cursor.Photograph.GetComponent<HomeCorkboardPhotoScript>().ArrayID);
				Object.Destroy(Cursor.Photograph);
				Photos--;
				Cursor.Photograph = null;
				UpdateButtonPrompts();
			}
			if (Cursor.Tack != null)
			{
				Cursor.CircleHighlight.transform.position = new Vector3(Cursor.CircleHighlight.transform.position.x, 100f, Cursor.CircleHighlight.transform.position.z);
				ShuffleStrings(Cursor.Tack.transform.parent.GetComponent<StringScript>().ArrayID);
				Object.Destroy(Cursor.Tack.transform.parent.gameObject);
				Strings--;
				Cursor.Tack = null;
				UpdateButtonPrompts();
			}
		}
	}

	private void Update()
	{
		if (GotPhotos && Corkboard && !SpawnedPhotos)
		{
			SpawnPhotographs();
			SpawnStrings();
			base.enabled = false;
			base.gameObject.SetActive(false);
			PromptBar.Label[0].text = string.Empty;
			PromptBar.Label[1].text = string.Empty;
			PromptBar.Label[2].text = string.Empty;
			PromptBar.Label[3].text = string.Empty;
			PromptBar.Label[4].text = string.Empty;
			PromptBar.Label[5].text = string.Empty;
			PromptBar.UpdateButtons();
			PromptBar.Show = false;
		}
		if (!Adjusting)
		{
			if (!Viewing)
			{
				UpdatePhotoSelection();
			}
			else
			{
				UpdatePhotoViewing();
			}
		}
		else
		{
			if (Corkboard)
			{
				Gallery.transform.localPosition = new Vector3(Gallery.transform.localPosition.x, Mathf.Lerp(Gallery.transform.localPosition.y, 1000f, Time.deltaTime * 10f), Gallery.transform.localPosition.z);
			}
			MouseDelta = new Vector2(Input.mousePosition.x - PreviousPosition.x, Input.mousePosition.y - PreviousPosition.y);
			if (InputDevice.Type == InputDeviceType.MouseAndKeyboard)
			{
				SpeedLimit = 0.1f;
			}
			else
			{
				SpeedLimit = 1f;
			}
			if (Moving)
			{
				UpdateCorkboardPhoto();
			}
			else if (MovingString)
			{
				UpdateString();
			}
			else
			{
				UpdateCorkboardCursor();
			}
		}
		PreviousPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	}

	public IEnumerator GetPhotos()
	{
		if (!Corkboard)
		{
			for (int i = 1; i < 26; i++)
			{
				Hearts[i].gameObject.SetActive(false);
			}
		}
		for (int ID = 1; ID < 26; ID++)
		{
			if (!PlayerGlobals.GetPhoto(ID))
			{
				continue;
			}
			string path = "file:///" + Application.streamingAssetsPath + "/Photographs/Photo_" + ID + ".png";
			WWW www = new WWW(path);
			yield return www;
			if (www.error == null)
			{
				Photographs[ID].mainTexture = www.texture;
				if (!Corkboard && PlayerGlobals.GetSenpaiPhoto(ID))
				{
					Hearts[ID].gameObject.SetActive(true);
				}
			}
			else
			{
				Debug.Log("Could not retrieve Photo " + ID + ". Maybe it was deleted from Streaming Assets? Setting Photo " + ID + " to ''false''.");
				PlayerGlobals.SetPhoto(ID, false);
			}
		}
		LoadingScreen.SetActive(false);
		if (!Corkboard)
		{
			PauseScreen.Sideways = true;
		}
		UpdateButtonPrompts();
		base.enabled = true;
		base.gameObject.SetActive(true);
		GotPhotos = true;
	}

	public void UpdateButtonPrompts()
	{
		if (NamingBully)
		{
			UITexture uITexture = Photographs[CurrentIndex];
			if (uITexture.mainTexture != NoPhoto && PlayerGlobals.GetBullyPhoto(CurrentIndex) > 0)
			{
				if (PlayerGlobals.GetBullyPhoto(CurrentIndex) > 0)
				{
					PromptBar.Label[0].text = "Name Bully";
				}
				else
				{
					PromptBar.Label[0].text = string.Empty;
				}
			}
			else
			{
				PromptBar.Label[0].text = string.Empty;
			}
			PromptBar.Label[1].text = string.Empty;
			PromptBar.Label[2].text = string.Empty;
			PromptBar.Label[3].text = string.Empty;
			PromptBar.Label[4].text = "Move";
			PromptBar.Label[5].text = "Move";
		}
		else if (Moving || MovingString)
		{
			PromptBar.Label[0].text = "Place";
			PromptBar.Label[1].text = string.Empty;
			PromptBar.Label[2].text = string.Empty;
			PromptBar.Label[3].text = string.Empty;
			PromptBar.Label[4].text = "Move";
			PromptBar.Label[5].text = "Move";
			if (!MovingString)
			{
				PromptBar.Label[2].text = "Resize";
				PromptBar.Label[3].text = "Resize";
			}
		}
		else if (Adjusting)
		{
			if (Cursor.Photograph != null)
			{
				PromptBar.Label[0].text = "Adjust";
				PromptBar.Label[1].text = string.Empty;
				PromptBar.Label[2].text = "Remove";
				PromptBar.Label[3].text = string.Empty;
			}
			else if (Cursor.Tack != null)
			{
				PromptBar.Label[2].text = "Remove";
			}
			else
			{
				PromptBar.Label[0].text = string.Empty;
				PromptBar.Label[2].text = string.Empty;
			}
			PromptBar.Label[1].text = "Back";
			PromptBar.Label[3].text = "Place Pin";
			PromptBar.Label[4].text = "Move";
			PromptBar.Label[5].text = "Move";
		}
		else if (!Viewing)
		{
			int currentIndex = CurrentIndex;
			if (Photographs[currentIndex].mainTexture != NoPhoto)
			{
				PromptBar.Label[0].text = "View";
				PromptBar.Label[2].text = "Delete";
			}
			else
			{
				PromptBar.Label[0].text = string.Empty;
				PromptBar.Label[2].text = string.Empty;
			}
			if (!Corkboard)
			{
				PromptBar.Label[3].text = ((!PlayerGlobals.GetSenpaiPhoto(currentIndex)) ? string.Empty : "Use");
			}
			else
			{
				PromptBar.Label[3].text = "Corkboard";
			}
			PromptBar.Label[1].text = "Back";
			PromptBar.Label[4].text = "Choose";
			PromptBar.Label[5].text = "Choose";
		}
		else
		{
			if (Corkboard)
			{
				PromptBar.Label[0].text = "Place";
			}
			else
			{
				PromptBar.Label[0].text = string.Empty;
			}
			PromptBar.Label[2].text = string.Empty;
			PromptBar.Label[3].text = string.Empty;
			PromptBar.Label[4].text = string.Empty;
			PromptBar.Label[5].text = string.Empty;
		}
		PromptBar.UpdateButtons();
		PromptBar.Show = true;
	}

	private void Shuffle(int Start)
	{
		for (int i = Start; i < CorkboardPhotographs.Length - 1; i++)
		{
			if (CorkboardPhotographs[i] != null)
			{
				CorkboardPhotographs[i].ArrayID--;
				CorkboardPhotographs[i] = CorkboardPhotographs[i + 1];
			}
		}
	}

	private void ShuffleStrings(int Start)
	{
		for (int i = Start; i < CorkboardPhotographs.Length - 1; i++)
		{
			if (CorkboardStrings[i] != null)
			{
				CorkboardStrings[i].ArrayID--;
				CorkboardStrings[i] = CorkboardStrings[i + 1];
			}
		}
	}

	public void SaveAllPhotographs()
	{
		for (int i = 0; i < 100; i++)
		{
			if (CorkboardPhotographs[i] != null)
			{
				PlayerPrefs.SetInt("CorkboardPhoto_" + i + "_Exists", 1);
				PlayerPrefs.SetInt("CorkboardPhoto_" + i + "_PhotoID", CorkboardPhotographs[i].ID);
				PlayerPrefs.SetFloat("CorkboardPhoto_" + i + "_PositionX", CorkboardPhotographs[i].transform.localPosition.x);
				PlayerPrefs.SetFloat("CorkboardPhoto_" + i + "_PositionY", CorkboardPhotographs[i].transform.localPosition.y);
				PlayerPrefs.SetFloat("CorkboardPhoto_" + i + "_PositionZ", CorkboardPhotographs[i].transform.localPosition.z);
				PlayerPrefs.SetFloat("CorkboardPhoto_" + i + "_RotationX", CorkboardPhotographs[i].transform.localEulerAngles.x);
				PlayerPrefs.SetFloat("CorkboardPhoto_" + i + "_RotationY", CorkboardPhotographs[i].transform.localEulerAngles.y);
				PlayerPrefs.SetFloat("CorkboardPhoto_" + i + "_RotationZ", CorkboardPhotographs[i].transform.localEulerAngles.z);
				PlayerPrefs.SetFloat("CorkboardPhoto_" + i + "_ScaleX", CorkboardPhotographs[i].transform.localScale.x);
				PlayerPrefs.SetFloat("CorkboardPhoto_" + i + "_ScaleY", CorkboardPhotographs[i].transform.localScale.y);
				PlayerPrefs.SetFloat("CorkboardPhoto_" + i + "_ScaleZ", CorkboardPhotographs[i].transform.localScale.z);
			}
			else
			{
				PlayerPrefs.SetInt("CorkboardPhoto_" + i + "_Exists", 0);
			}
		}
	}

	public void SpawnPhotographs()
	{
		for (int i = 0; i < 100; i++)
		{
			if (PlayerPrefs.GetInt("CorkboardPhoto_" + i + "_Exists") == 1)
			{
				GameObject gameObject = Object.Instantiate(Photograph, base.transform.position, Quaternion.identity);
				gameObject.transform.parent = CorkboardPanel;
				gameObject.transform.localPosition = new Vector3(PlayerPrefs.GetFloat("CorkboardPhoto_" + i + "_PositionX"), PlayerPrefs.GetFloat("CorkboardPhoto_" + i + "_PositionY"), PlayerPrefs.GetFloat("CorkboardPhoto_" + i + "_PositionZ"));
				gameObject.transform.localEulerAngles = new Vector3(PlayerPrefs.GetFloat("CorkboardPhoto_" + i + "_RotationX"), PlayerPrefs.GetFloat("CorkboardPhoto_" + i + "_RotationY"), PlayerPrefs.GetFloat("CorkboardPhoto_" + i + "_RotationZ"));
				gameObject.transform.localScale = new Vector3(PlayerPrefs.GetFloat("CorkboardPhoto_" + i + "_ScaleX"), PlayerPrefs.GetFloat("CorkboardPhoto_" + i + "_ScaleY"), PlayerPrefs.GetFloat("CorkboardPhoto_" + i + "_ScaleZ"));
				gameObject.GetComponent<UITexture>().mainTexture = Photographs[PlayerPrefs.GetInt("CorkboardPhoto_" + i + "_PhotoID")].mainTexture;
				CorkboardPhotographs[Photos] = gameObject.GetComponent<HomeCorkboardPhotoScript>();
				CorkboardPhotographs[Photos].ID = PlayerPrefs.GetInt("CorkboardPhoto_" + i + "_PhotoID");
				CorkboardPhotographs[Photos].ArrayID = Photos;
				Photos++;
			}
		}
		SpawnedPhotos = true;
	}

	public void SaveAllStrings()
	{
		Debug.Log("Saved strings.");
		for (int i = 0; i < 100; i++)
		{
			if (CorkboardStrings[i] != null)
			{
				PlayerPrefs.SetInt("CorkboardString_" + i + "_Exists", 1);
				PlayerPrefs.SetFloat("CorkboardString_" + i + "_PositionX", CorkboardStrings[i].Origin.localPosition.x);
				PlayerPrefs.SetFloat("CorkboardString_" + i + "_PositionY", CorkboardStrings[i].Origin.localPosition.y);
				PlayerPrefs.SetFloat("CorkboardString_" + i + "_PositionZ", CorkboardStrings[i].Origin.localPosition.z);
				PlayerPrefs.SetFloat("CorkboardString2_" + i + "_PositionX", CorkboardStrings[i].Target.localPosition.x);
				PlayerPrefs.SetFloat("CorkboardString2_" + i + "_PositionY", CorkboardStrings[i].Target.localPosition.y);
				PlayerPrefs.SetFloat("CorkboardString2_" + i + "_PositionZ", CorkboardStrings[i].Target.localPosition.z);
			}
			else
			{
				PlayerPrefs.SetInt("CorkboardString_" + i + "_Exists", 0);
			}
		}
	}

	public void SpawnStrings()
	{
		for (int i = 0; i < 100; i++)
		{
			if (PlayerPrefs.GetInt("CorkboardString_" + i + "_Exists") == 1)
			{
				GameObject gameObject = Object.Instantiate(StringSet, base.transform.position, Quaternion.identity);
				gameObject.transform.parent = StringParent;
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				String = gameObject.GetComponent<StringScript>();
				String.Origin.localPosition = new Vector3(PlayerPrefs.GetFloat("CorkboardString_" + i + "_PositionX"), PlayerPrefs.GetFloat("CorkboardString_" + i + "_PositionY"), PlayerPrefs.GetFloat("CorkboardString_" + i + "_PositionZ"));
				String.Target.localPosition = new Vector3(PlayerPrefs.GetFloat("CorkboardString2_" + i + "_PositionX"), PlayerPrefs.GetFloat("CorkboardString2_" + i + "_PositionY"), PlayerPrefs.GetFloat("CorkboardString2_" + i + "_PositionZ"));
				CorkboardStrings[Strings] = String.GetComponent<StringScript>();
				CorkboardStrings[Strings].ArrayID = Strings;
				Strings++;
			}
			else
			{
				PlayerPrefs.SetInt("CorkboardString_" + i + "_Exists", 0);
			}
		}
	}
}
