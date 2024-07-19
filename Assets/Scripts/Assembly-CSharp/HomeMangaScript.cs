using UnityEngine;

public class HomeMangaScript : MonoBehaviour
{
	private static readonly string[] SeductionStrings = new string[6] { "Innocent", "Flirty", "Charming", "Sensual", "Seductive", "Succubus" };

	private static readonly string[] NumbnessStrings = new string[6] { "Stoic", "Somber", "Detatched", "Unemotional", "Desensitized", "Dead Inside" };

	private static readonly string[] EnlightenmentStrings = new string[6] { "Asleep", "Awoken", "Mindful", "Informed", "Eyes Open", "Omniscient" };

	public InputManagerScript InputManager;

	public HomeYandereScript HomeYandere;

	public HomeCameraScript HomeCamera;

	public HomeWindowScript HomeWindow;

	public HomeDarknessScript Darkness;

	private GameObject NewManga;

	public GameObject ReadButtonGroup;

	public GameObject MysteryManga;

	public GameObject AreYouSure;

	public GameObject MangaGroup;

	public GameObject[] MangaList;

	public UILabel MangaNameLabel;

	public UILabel MangaDescLabel;

	public UILabel MangaBuffLabel;

	public UILabel RequiredLabel;

	public UILabel CurrentLabel;

	public UILabel ButtonLabel;

	public Transform MangaParent;

	public bool DestinationReached;

	public float TargetRotation;

	public float Rotation;

	public int TotalManga;

	public int Selected;

	public string Title = string.Empty;

	public GameObject[] MangaModels;

	public string[] MangaNames;

	public string[] MangaDescs;

	public string[] MangaBuffs;

	private void Start()
	{
		UpdateCurrentLabel();
		for (int i = 0; i < TotalManga; i++)
		{
			if (CollectibleGlobals.GetMangaCollected(i + 1))
			{
				NewManga = Object.Instantiate(MangaModels[i], new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 1f), Quaternion.identity);
			}
			else
			{
				NewManga = Object.Instantiate(MysteryManga, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 1f), Quaternion.identity);
			}
			NewManga.transform.parent = MangaParent;
			NewManga.GetComponent<HomeMangaBookScript>().Manga = this;
			NewManga.GetComponent<HomeMangaBookScript>().ID = i;
			NewManga.transform.localScale = new Vector3(1.45f, 1.45f, 1.45f);
			MangaParent.transform.localEulerAngles = new Vector3(MangaParent.transform.localEulerAngles.x, MangaParent.transform.localEulerAngles.y + 360f / (float)TotalManga, MangaParent.transform.localEulerAngles.z);
			MangaList[i] = NewManga;
		}
		MangaParent.transform.localEulerAngles = new Vector3(MangaParent.transform.localEulerAngles.x, 0f, MangaParent.transform.localEulerAngles.z);
		MangaParent.transform.localPosition = new Vector3(MangaParent.transform.localPosition.x, MangaParent.transform.localPosition.y, 1.8f);
		UpdateMangaLabels();
		MangaParent.transform.localScale = Vector3.zero;
		MangaParent.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (HomeWindow.Show)
		{
			if (!AreYouSure.activeInHierarchy)
			{
				MangaParent.localScale = Vector3.Lerp(MangaParent.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
				MangaParent.gameObject.SetActive(true);
				if (InputManager.TappedRight)
				{
					DestinationReached = false;
					TargetRotation += 360f / (float)TotalManga;
					Selected++;
					if (Selected > TotalManga - 1)
					{
						Selected = 0;
					}
					UpdateMangaLabels();
					UpdateCurrentLabel();
				}
				if (InputManager.TappedLeft)
				{
					DestinationReached = false;
					TargetRotation -= 360f / (float)TotalManga;
					Selected--;
					if (Selected < 0)
					{
						Selected = TotalManga - 1;
					}
					UpdateMangaLabels();
					UpdateCurrentLabel();
				}
				Rotation = Mathf.Lerp(Rotation, TargetRotation, Time.deltaTime * 10f);
				MangaParent.localEulerAngles = new Vector3(MangaParent.localEulerAngles.x, Rotation, MangaParent.localEulerAngles.z);
				if (Input.GetButtonDown("A") && ReadButtonGroup.activeInHierarchy)
				{
					MangaGroup.SetActive(false);
					AreYouSure.SetActive(true);
				}
				if (Input.GetKeyDown(KeyCode.S))
				{
					PlayerGlobals.Seduction++;
					PlayerGlobals.Numbness++;
					PlayerGlobals.Enlightenment++;
					if (PlayerGlobals.Seduction > 5)
					{
						PlayerGlobals.Seduction = 0;
						PlayerGlobals.Numbness = 0;
						PlayerGlobals.Enlightenment = 0;
					}
					UpdateCurrentLabel();
					UpdateMangaLabels();
				}
				if (Input.GetButtonDown("B"))
				{
					HomeCamera.Destination = HomeCamera.Destinations[0];
					HomeCamera.Target = HomeCamera.Targets[0];
					HomeYandere.CanMove = true;
					HomeWindow.Show = false;
				}
				if (Input.GetKeyDown(KeyCode.Space))
				{
					for (int i = 0; i < TotalManga; i++)
					{
						CollectibleGlobals.SetMangaCollected(i + 1, true);
					}
				}
				return;
			}
			if (Input.GetButtonDown("A"))
			{
				if (Selected < 5)
				{
					PlayerGlobals.Seduction++;
				}
				else if (Selected < 10)
				{
					PlayerGlobals.Numbness++;
				}
				else
				{
					PlayerGlobals.Enlightenment++;
				}
				HomeGlobals.LateForSchool = true;
				AreYouSure.SetActive(false);
				Darkness.FadeOut = true;
			}
			if (Input.GetButtonDown("B"))
			{
				MangaGroup.SetActive(true);
				AreYouSure.SetActive(false);
			}
		}
		else
		{
			MangaParent.localScale = Vector3.Lerp(MangaParent.localScale, Vector3.zero, Time.deltaTime * 10f);
			if (MangaParent.localScale.x < 0.01f)
			{
				MangaParent.gameObject.SetActive(false);
			}
		}
	}

	private void UpdateMangaLabels()
	{
		if (Selected < 5)
		{
			ReadButtonGroup.SetActive(PlayerGlobals.Seduction == Selected);
			if (CollectibleGlobals.GetMangaCollected(Selected + 1))
			{
				if (PlayerGlobals.Seduction > Selected)
				{
					RequiredLabel.text = "You have already read this manga.";
				}
				else
				{
					RequiredLabel.text = "Required Seduction Level: " + Selected;
				}
			}
			else
			{
				RequiredLabel.text = "You have not yet collected this manga.";
				ReadButtonGroup.SetActive(false);
			}
		}
		else if (Selected < 10)
		{
			ReadButtonGroup.SetActive(PlayerGlobals.Numbness == Selected - 5);
			if (CollectibleGlobals.GetMangaCollected(Selected + 1))
			{
				if (PlayerGlobals.Numbness > Selected - 5)
				{
					RequiredLabel.text = "You have already read this manga.";
				}
				else
				{
					RequiredLabel.text = "Required Numbness Level: " + (Selected - 5);
				}
			}
			else
			{
				RequiredLabel.text = "You have not yet collected this manga.";
				ReadButtonGroup.SetActive(false);
			}
		}
		else
		{
			ReadButtonGroup.SetActive(PlayerGlobals.Enlightenment == Selected - 10);
			if (CollectibleGlobals.GetMangaCollected(Selected + 1))
			{
				if (PlayerGlobals.Enlightenment > Selected - 10)
				{
					RequiredLabel.text = "You have already read this manga.";
				}
				else
				{
					RequiredLabel.text = "Required Enlightenment Level: " + (Selected - 10);
				}
			}
			else
			{
				RequiredLabel.text = "You have not yet collected this manga.";
				ReadButtonGroup.SetActive(false);
			}
		}
		if (CollectibleGlobals.GetMangaCollected(Selected + 1))
		{
			MangaNameLabel.text = MangaNames[Selected];
			MangaDescLabel.text = MangaDescs[Selected];
			MangaBuffLabel.text = MangaBuffs[Selected];
		}
		else
		{
			MangaNameLabel.text = "?????";
			MangaDescLabel.text = "?????";
			MangaBuffLabel.text = "?????";
		}
	}

	private void UpdateCurrentLabel()
	{
		if (Selected < 5)
		{
			Title = SeductionStrings[PlayerGlobals.Seduction];
			CurrentLabel.text = "Current Seduction Level: " + PlayerGlobals.Seduction + " (" + Title + ")";
		}
		else if (Selected < 10)
		{
			Title = NumbnessStrings[PlayerGlobals.Numbness];
			CurrentLabel.text = "Current Numbness Level: " + PlayerGlobals.Numbness + " (" + Title + ")";
		}
		else
		{
			Title = EnlightenmentStrings[PlayerGlobals.Enlightenment];
			CurrentLabel.text = "Current Enlightenment Level: " + PlayerGlobals.Enlightenment + " (" + Title + ")";
		}
	}
}
