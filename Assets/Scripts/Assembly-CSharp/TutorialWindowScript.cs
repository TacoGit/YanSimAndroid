using UnityEngine;

public class TutorialWindowScript : MonoBehaviour
{
	public YandereScript Yandere;

	public bool ShowCouncilMessage;

	public bool ShowTeacherMessage;

	public bool ShowLockerMessage;

	public bool ShowPoliceMessage;

	public bool ShowSanityMessage;

	public bool ShowSenpaiMessage;

	public bool ShowVisionMessage;

	public bool ShowWeaponMessage;

	public bool ShowBloodMessage;

	public bool ShowClassMessage;

	public bool ShowPhotoMessage;

	public bool ShowClubMessage;

	public bool ShowInfoMessage;

	public bool ShowPoolMessage;

	public bool ShowRepMessage;

	public bool IgnoreCouncil;

	public bool IgnoreTeacher;

	public bool IgnoreLocker;

	public bool IgnorePolice;

	public bool IgnoreSanity;

	public bool IgnoreSenpai;

	public bool IgnoreVision;

	public bool IgnoreWeapon;

	public bool IgnoreBlood;

	public bool IgnoreClass;

	public bool IgnorePhoto;

	public bool IgnoreClub;

	public bool IgnoreInfo;

	public bool IgnorePool;

	public bool IgnoreRep;

	public bool Hide;

	public bool Show;

	public UILabel TutorialLabel;

	public UILabel ShadowLabel;

	public UILabel TitleLabel;

	public UITexture TutorialImage;

	public string DisabledString;

	public Texture DisabledTexture;

	public string CouncilString;

	public Texture CouncilTexture;

	public string TeacherString;

	public Texture TeacherTexture;

	public string LockerString;

	public Texture LockerTexture;

	public string PoliceString;

	public Texture PoliceTexture;

	public string SanityString;

	public Texture SanityTexture;

	public string SenpaiString;

	public Texture SenpaiTexture;

	public string VisionString;

	public Texture VisionTexture;

	public string WeaponString;

	public Texture WeaponTexture;

	public string BloodString;

	public Texture BloodTexture;

	public string ClassString;

	public Texture ClassTexture;

	public string PhotoString;

	public Texture PhotoTexture;

	public string ClubString;

	public Texture ClubTexture;

	public string InfoString;

	public Texture InfoTexture;

	public string PoolString;

	public Texture PoolTexture;

	public string RepString;

	public Texture RepTexture;

	public float Timer;

	private void Start()
	{
		base.transform.localScale = new Vector3(0f, 0f, 0f);
		if (TutorialGlobals.TutorialsOff)
		{
			base.enabled = false;
			return;
		}
		IgnoreCouncil = TutorialGlobals.IgnoreCouncil;
		IgnoreTeacher = TutorialGlobals.IgnoreTeacher;
		IgnoreLocker = TutorialGlobals.IgnoreLocker;
		IgnorePolice = TutorialGlobals.IgnorePolice;
		IgnoreSanity = TutorialGlobals.IgnoreSanity;
		IgnoreSenpai = TutorialGlobals.IgnoreSenpai;
		IgnoreVision = TutorialGlobals.IgnoreVision;
		IgnoreWeapon = TutorialGlobals.IgnoreWeapon;
		IgnoreBlood = TutorialGlobals.IgnoreBlood;
		IgnoreClass = TutorialGlobals.IgnoreClass;
		IgnorePhoto = TutorialGlobals.IgnorePhoto;
		IgnoreClub = TutorialGlobals.IgnoreClub;
		IgnoreInfo = TutorialGlobals.IgnoreInfo;
		IgnorePool = TutorialGlobals.IgnorePool;
		IgnoreRep = TutorialGlobals.IgnoreRep;
	}

	private void Update()
	{
		if (Show)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1.2925f, 1.2925f, 1.2925f), Time.unscaledDeltaTime * 10f);
			if (base.transform.localScale.x > 1f)
			{
				if (Input.GetButtonDown("B"))
				{
					TutorialGlobals.TutorialsOff = true;
					TitleLabel.text = "Tutorials Disabled";
					TutorialLabel.text = DisabledString;
					TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
					TutorialImage.mainTexture = DisabledTexture;
					ShadowLabel.text = TutorialLabel.text;
				}
				else if (Input.GetButtonDown("A"))
				{
					Yandere.RPGCamera.enabled = true;
					Yandere.Blur.enabled = false;
					Time.timeScale = 1f;
					Show = false;
					Hide = true;
				}
			}
		}
		else if (Hide)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 10f);
			if (base.transform.localScale.x < 0.1f)
			{
				base.transform.localScale = new Vector3(0f, 0f, 0f);
				Hide = false;
				if (TutorialGlobals.TutorialsOff)
				{
					base.enabled = false;
				}
			}
		}
		if (!Yandere.CanMove || Yandere.Egg || Yandere.Aiming || Yandere.PauseScreen.Show || Yandere.CinematicCamera.activeInHierarchy)
		{
			return;
		}
		Timer += Time.deltaTime;
		if (!(Timer > 5f))
		{
			return;
		}
		if (!IgnoreCouncil && ShowCouncilMessage && !Show)
		{
			TutorialGlobals.IgnoreCouncil = true;
			IgnoreCouncil = true;
			TitleLabel.text = "Student Council";
			TutorialLabel.text = CouncilString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = CouncilTexture;
			SummonWindow();
		}
		if (!IgnoreTeacher && ShowTeacherMessage && !Show)
		{
			TutorialGlobals.IgnoreTeacher = true;
			IgnoreTeacher = true;
			TitleLabel.text = "Teachers";
			TutorialLabel.text = TeacherString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = TeacherTexture;
			SummonWindow();
		}
		if (!IgnoreLocker && ShowLockerMessage && !Show)
		{
			TutorialGlobals.IgnoreLocker = true;
			IgnoreLocker = true;
			TitleLabel.text = "Notes In Lockers";
			TutorialLabel.text = LockerString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = LockerTexture;
			SummonWindow();
		}
		if (!IgnorePolice && ShowPoliceMessage && !Show)
		{
			TutorialGlobals.IgnorePolice = true;
			IgnorePolice = true;
			TitleLabel.text = "Police";
			TutorialLabel.text = PoliceString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = PoliceTexture;
			SummonWindow();
		}
		if (!IgnoreSanity && ShowSanityMessage && !Show)
		{
			TutorialGlobals.IgnoreSanity = true;
			IgnoreSanity = true;
			TitleLabel.text = "Restoring Sanity";
			TutorialLabel.text = SanityString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = SanityTexture;
			SummonWindow();
		}
		if (!IgnoreSenpai && ShowSenpaiMessage && !Show)
		{
			TutorialGlobals.IgnoreSenpai = true;
			IgnoreSenpai = true;
			TitleLabel.text = "Your Senpai";
			TutorialLabel.text = SenpaiString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = SenpaiTexture;
			SummonWindow();
		}
		if (!IgnoreVision)
		{
			if (Yandere.StudentManager.WestBathroomArea.bounds.Contains(Yandere.transform.position) || Yandere.StudentManager.EastBathroomArea.bounds.Contains(Yandere.transform.position))
			{
				ShowVisionMessage = true;
			}
			if (ShowVisionMessage && !Show)
			{
				TutorialGlobals.IgnoreVision = true;
				IgnoreVision = true;
				TitleLabel.text = "Yandere Vision";
				TutorialLabel.text = VisionString;
				TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
				TutorialImage.mainTexture = VisionTexture;
				SummonWindow();
			}
		}
		if (!IgnoreWeapon)
		{
			if (Yandere.Armed)
			{
				ShowWeaponMessage = true;
			}
			if (ShowWeaponMessage && !Show)
			{
				TutorialGlobals.IgnoreWeapon = true;
				IgnoreWeapon = true;
				TitleLabel.text = "Weapons";
				TutorialLabel.text = WeaponString;
				TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
				TutorialImage.mainTexture = WeaponTexture;
				SummonWindow();
			}
		}
		if (!IgnoreBlood && ShowBloodMessage && !Show)
		{
			TutorialGlobals.IgnoreBlood = true;
			IgnoreBlood = true;
			TitleLabel.text = "Bloody Clothing";
			TutorialLabel.text = BloodString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = BloodTexture;
			SummonWindow();
		}
		if (!IgnoreClass && ShowClassMessage && !Show)
		{
			TutorialGlobals.IgnoreClass = true;
			IgnoreClass = true;
			TitleLabel.text = "Attending Class";
			TutorialLabel.text = ClassString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = ClassTexture;
			SummonWindow();
		}
		if (!IgnorePhoto)
		{
			if (Yandere.transform.position.z > -50f)
			{
				ShowPhotoMessage = true;
			}
			if (ShowPhotoMessage && !Show)
			{
				TutorialGlobals.IgnorePhoto = true;
				IgnorePhoto = true;
				TitleLabel.text = "Taking Photographs";
				TutorialLabel.text = PhotoString;
				TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
				TutorialImage.mainTexture = PhotoTexture;
				SummonWindow();
			}
		}
		if (!IgnoreClub && ShowClubMessage && !Show)
		{
			TutorialGlobals.IgnoreClub = true;
			IgnoreClub = true;
			TitleLabel.text = "Joining Clubs";
			TutorialLabel.text = ClubString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = ClubTexture;
			SummonWindow();
		}
		if (!IgnoreInfo && ShowInfoMessage && !Show)
		{
			TutorialGlobals.IgnoreInfo = true;
			IgnoreInfo = true;
			TitleLabel.text = "Info-chan's Services";
			TutorialLabel.text = InfoString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = InfoTexture;
			SummonWindow();
		}
		if (!IgnorePool && ShowPoolMessage && !Show)
		{
			TutorialGlobals.IgnorePool = true;
			IgnorePool = true;
			TitleLabel.text = "Cleaning Blood";
			TutorialLabel.text = PoolString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = PoolTexture;
			SummonWindow();
		}
		if (!IgnoreRep && ShowRepMessage && !Show)
		{
			TutorialGlobals.IgnoreRep = true;
			IgnoreRep = true;
			TitleLabel.text = "Reputation";
			TutorialLabel.text = RepString;
			TutorialLabel.text = TutorialLabel.text.Replace('@', '\n');
			TutorialImage.mainTexture = RepTexture;
			SummonWindow();
		}
	}

	private void SummonWindow()
	{
		ShadowLabel.text = TutorialLabel.text;
		Yandere.RPGCamera.enabled = false;
		Yandere.Blur.enabled = true;
		Time.timeScale = 0f;
		Show = true;
		Timer = 0f;
	}
}
