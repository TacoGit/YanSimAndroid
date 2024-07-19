using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDayScript : MonoBehaviour
{
	public SecuritySystemScript SecuritySystem;

	public StudentManagerScript StudentManager;

	public WeaponManagerScript WeaponManager;

	public ClubManagerScript ClubManager;

	public HeartbrokenScript Heartbroken;

	public IncineratorScript Incinerator;

	public VoidGoddessScript VoidGoddess;

	public WoodChipperScript WoodChipper;

	public ReputationScript Reputation;

	public DumpsterLidScript Dumpster;

	public CounselorScript Counselor;

	public WeaponScript MurderWeapon;

	public TranqCaseScript TranqCase;

	public YandereScript Yandere;

	public RagdollScript Corpse;

	public StudentScript Senpai;

	public StudentScript Patsy;

	public PoliceScript Police;

	public Transform EODCamera;

	public ClockScript Clock;

	public JsonScript JSON;

	public GardenHoleScript[] GardenHoles;

	public Animation[] CopAnimation;

	public GameObject MainCamera;

	public UISprite EndOfDayDarkness;

	public UILabel Label;

	public bool PreviouslyActivated;

	public bool PoliceArrived;

	public bool ClubClosed;

	public bool ClubKicked;

	public bool ErectFence;

	public bool GameOver;

	public bool Darken;

	public int ClothingWithRedPaint;

	public int FragileTarget;

	public int NewFriends;

	public int DeadPerps;

	public int Arrests;

	public int Corpses;

	public int Victims;

	public int Weapons;

	public int Phase = 1;

	public int WeaponID;

	public int ArrestID;

	public int ClubID;

	public int ID;

	public string[] ClubNames;

	public int[] VictimArray;

	public ClubType[] ClubArray;

	private SaveFile saveFile;

	public GameObject TextWindow;

	public GameObject Cops;

	public GameObject SearchingCop;

	public GameObject MurderScene;

	public GameObject ShruggingCops;

	public GameObject TabletCop;

	public GameObject SecuritySystemScene;

	public GameObject OpenTranqCase;

	public GameObject ClosedTranqCase;

	public GameObject GaudyRing;

	public GameObject AnswerSheet;

	public GameObject Fence;

	public GameObject SCP;

	public GameObject ArrestingCops;

	public GameObject Mask;

	public StudentScript KidnappedVictim;

	public Renderer TabletPortrait;

	public Transform CardboardBox;

	public void Start()
	{
		Yandere.MainCamera.gameObject.SetActive(false);
		EndOfDayDarkness.color = new Color(EndOfDayDarkness.color.r, EndOfDayDarkness.color.g, EndOfDayDarkness.color.b, 1f);
		PreviouslyActivated = true;
		GetComponent<AudioSource>().volume = 0f;
		Clock.enabled = false;
		Clock.MainLight.color = new Color(1f, 1f, 1f, 1f);
		RenderSettings.ambientLight = new Color(0.75f, 0.75f, 0.75f, 1f);
		RenderSettings.skybox.SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
		UpdateScene();
		CopAnimation[5]["idleShort_00"].speed = Random.Range(0.9f, 1.1f);
		CopAnimation[6]["idleShort_00"].speed = Random.Range(0.9f, 1.1f);
		CopAnimation[7]["idleShort_00"].speed = Random.Range(0.9f, 1.1f);
		Time.timeScale = 1f;
		for (int i = 1; i < 6; i++)
		{
			Yandere.CharacterAnimation[Yandere.CreepyIdles[i]].weight = 0f;
			Yandere.CharacterAnimation[Yandere.CreepyWalks[i]].weight = 0f;
		}
		Debug.Log("BloodParent.childCount is: " + Police.BloodParent.childCount);
		foreach (Transform item in Police.BloodParent)
		{
			PickUpScript component = item.gameObject.GetComponent<PickUpScript>();
			if (component != null && component.RedPaint)
			{
				ClothingWithRedPaint++;
			}
		}
		Debug.Log("Clothing with red paint is: " + ClothingWithRedPaint);
	}

	private void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			EndOfDayDarkness.color = new Color(0f, 0f, 0f, 1f);
			Darken = true;
		}
		if (EndOfDayDarkness.color.a == 0f && Input.GetButtonDown("A"))
		{
			Darken = true;
		}
		if (Darken)
		{
			EndOfDayDarkness.color = new Color(EndOfDayDarkness.color.r, EndOfDayDarkness.color.g, EndOfDayDarkness.color.b, Mathf.MoveTowards(EndOfDayDarkness.color.a, 1f, Time.deltaTime * 2f));
			if (EndOfDayDarkness.color.a == 1f)
			{
				if (Senpai == null)
				{
					Senpai = StudentManager.Students[1];
					Senpai.CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
					Senpai.CharacterAnimation.enabled = true;
				}
				Yandere.transform.parent = null;
				Yandere.transform.position = new Vector3(0f, 0f, -75f);
				EODCamera.localPosition = new Vector3(1f, 1.8f, -2.5f);
				EODCamera.localEulerAngles = new Vector3(22.5f, -22.5f, 0f);
				if (KidnappedVictim != null)
				{
					KidnappedVictim.gameObject.SetActive(false);
				}
				CardboardBox.parent = null;
				SearchingCop.SetActive(false);
				MurderScene.SetActive(false);
				Cops.SetActive(false);
				TabletCop.SetActive(false);
				ShruggingCops.SetActive(false);
				SecuritySystemScene.SetActive(false);
				OpenTranqCase.SetActive(false);
				ClosedTranqCase.SetActive(false);
				GaudyRing.SetActive(false);
				AnswerSheet.SetActive(false);
				Fence.SetActive(false);
				SCP.SetActive(false);
				ArrestingCops.SetActive(false);
				Mask.SetActive(false);
				Senpai.gameObject.SetActive(false);
				if (Patsy != null)
				{
					Patsy.gameObject.SetActive(false);
				}
				if (!GameOver)
				{
					Darken = false;
					UpdateScene();
				}
				else
				{
					Heartbroken.transform.parent.transform.parent = null;
					Heartbroken.transform.parent.gameObject.SetActive(true);
					Heartbroken.Noticed = false;
					Heartbroken.Arrested = true;
					MainCamera.SetActive(false);
					base.gameObject.SetActive(false);
					Time.timeScale = 1f;
				}
			}
		}
		else
		{
			EndOfDayDarkness.color = new Color(EndOfDayDarkness.color.r, EndOfDayDarkness.color.g, EndOfDayDarkness.color.b, Mathf.MoveTowards(EndOfDayDarkness.color.a, 0f, Time.deltaTime * 2f));
		}
		AudioSource component = GetComponent<AudioSource>();
		component.volume = Mathf.MoveTowards(component.volume, 1f, Time.deltaTime * 2f);
	}

	public void UpdateScene()
	{
		Label.color = new Color(0f, 0f, 0f, 1f);
		for (ID = 0; ID < WeaponManager.Weapons.Length; ID++)
		{
			if (WeaponManager.Weapons[ID] != null)
			{
				WeaponManager.Weapons[ID].gameObject.SetActive(false);
			}
		}
		if (!PoliceArrived)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			Finish();
		}
		if (Phase == 1)
		{
			CopAnimation[1]["walk_00"].speed = Random.Range(0.9f, 1.1f);
			CopAnimation[2]["walk_00"].speed = Random.Range(0.9f, 1.1f);
			CopAnimation[3]["walk_00"].speed = Random.Range(0.9f, 1.1f);
			Cops.SetActive(true);
			if (Police.PoisonScene)
			{
				Label.text = "The police and the paramedics arrive at school.";
				Phase = 103;
				return;
			}
			if (Police.DrownScene)
			{
				Label.text = "The police arrive at school.";
				Phase = 104;
				return;
			}
			if (Police.ElectroScene)
			{
				Label.text = "The police arrive at school.";
				Phase = 105;
				return;
			}
			if (Police.MurderSuicideScene)
			{
				Label.text = "The police arrive at school, and discover what appears to be the scene of a murder-suicide.";
				Phase++;
				return;
			}
			Label.text = "The police arrive at school.";
			if (Police.SuicideScene)
			{
				Phase = 102;
			}
			else
			{
				Phase++;
			}
		}
		else if (Phase == 2)
		{
			if (Police.Corpses == 0)
			{
				if (!Police.PoisonScene && !Police.SuicideScene)
				{
					if (Police.LimbParent.childCount > 0)
					{
						if (Police.LimbParent.childCount == 1)
						{
							Label.text = "The police find a severed body part at school.";
						}
						else
						{
							Label.text = "The police find multiple severed body parts at school.";
						}
						MurderScene.SetActive(true);
					}
					else
					{
						SearchingCop.SetActive(true);
						if (Police.BloodParent.childCount - ClothingWithRedPaint > 0)
						{
							Label.text = "The police find mysterious blood stains, but are unable to locate any corpses on school grounds.";
						}
						else if (ClothingWithRedPaint == 0)
						{
							Label.text = "The police are unable to locate any corpses on school grounds.";
						}
						else
						{
							Label.text = "The police find clothing that is stained with red paint, but are unable to locate any actual blood stains, and cannot locate any corpses, either.";
						}
					}
					Phase++;
				}
				else
				{
					SearchingCop.SetActive(true);
					Label.text = "The police are unable to locate any other corpses on school grounds.";
					Phase++;
				}
				return;
			}
			MurderScene.SetActive(true);
			List<string> list = new List<string>();
			RagdollScript[] corpseList = Police.CorpseList;
			foreach (RagdollScript ragdollScript in corpseList)
			{
				if (ragdollScript != null && !ragdollScript.Disposed)
				{
					VictimArray[Corpses] = ragdollScript.Student.StudentID;
					list.Add(ragdollScript.Student.Name);
					Corpses++;
				}
			}
			list.Sort();
			string text = "The police discover the corpse" + ((list.Count != 1) ? "s" : string.Empty) + " of ";
			if (list.Count == 1)
			{
				Label.text = text + list[0] + ".";
			}
			else if (list.Count == 2)
			{
				Label.text = text + list[0] + " and " + list[1] + ".";
			}
			else if (list.Count < 6)
			{
				Label.text = "The police discover multiple corpses on school grounds.";
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < list.Count - 1; j++)
				{
					stringBuilder.Append(list[j] + ", ");
				}
				stringBuilder.Append("and " + list[list.Count - 1] + ".");
				Label.text = text + stringBuilder.ToString();
			}
			else
			{
				Label.text = "The police discover more than five corpses on school grounds.";
			}
			Phase++;
		}
		else if (Phase == 3)
		{
			WeaponManager.CheckWeapons();
			if (WeaponManager.MurderWeapons == 0)
			{
				ShruggingCops.SetActive(true);
				if (Weapons == 0)
				{
					Label.text = "The police are unable to locate any murder weapons.";
					Phase += 2;
				}
				else
				{
					Phase += 2;
					UpdateScene();
				}
				return;
			}
			MurderWeapon = null;
			for (ID = 0; ID < WeaponManager.Weapons.Length; ID++)
			{
				if (MurderWeapon == null)
				{
					WeaponScript weaponScript = WeaponManager.Weapons[ID];
					if (weaponScript != null && weaponScript.Blood.enabled)
					{
						if (!weaponScript.AlreadyExamined)
						{
							weaponScript.gameObject.SetActive(true);
							weaponScript.AlreadyExamined = true;
							MurderWeapon = weaponScript;
							WeaponID = ID;
						}
						else
						{
							weaponScript.gameObject.SetActive(false);
						}
					}
				}
			}
			List<string> list2 = new List<string>();
			for (ID = 0; ID < MurderWeapon.Victims.Length; ID++)
			{
				if (MurderWeapon.Victims[ID])
				{
					list2.Add(JSON.Students[ID].Name);
				}
			}
			list2.Sort();
			Victims = list2.Count;
			string text2 = MurderWeapon.Name;
			string text3 = ((text2[text2.Length - 1] == 's') ? (text2.ToLower() + " that are") : ("a " + text2.ToLower() + " that is"));
			string text4 = "The police discover " + text3 + " stained with the blood of ";
			if (list2.Count == 1)
			{
				Label.text = text4 + list2[0] + ".";
			}
			else if (list2.Count == 2)
			{
				Label.text = text4 + list2[0] + " and " + list2[1] + ".";
			}
			else
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int k = 0; k < list2.Count - 1; k++)
				{
					stringBuilder2.Append(list2[k] + ", ");
				}
				stringBuilder2.Append("and " + list2[list2.Count - 1] + ".");
				Label.text = text4 + stringBuilder2.ToString();
			}
			Weapons++;
			Phase++;
			MurderWeapon.transform.parent = base.transform;
			MurderWeapon.transform.localPosition = new Vector3(0.6f, 1.4f, -1.5f);
			MurderWeapon.transform.localEulerAngles = new Vector3(-45f, 90f, -90f);
			MurderWeapon.MyRigidbody.useGravity = false;
			MurderWeapon.Rotate = true;
		}
		else if (Phase == 4)
		{
			if (MurderWeapon.FingerprintID == 0)
			{
				ShruggingCops.SetActive(true);
				Label.text = "The police find no fingerprints on the weapon.";
				Phase = 3;
			}
			else if (MurderWeapon.FingerprintID == 100)
			{
				TeleportYandere();
				Yandere.CharacterAnimation.Play("f02_disappointed_00");
				Label.text = "The police find Ayano's fingerprints on the weapon.";
				Phase = 100;
			}
			else
			{
				int fingerprintID = WeaponManager.Weapons[WeaponID].FingerprintID;
				TabletCop.SetActive(true);
				CopAnimation[4]["scienceTablet_00"].speed = 0f;
				TabletPortrait.material.mainTexture = VoidGoddess.Portraits[fingerprintID].mainTexture;
				Label.text = "The police find the fingerprints of " + JSON.Students[fingerprintID].Name + " on the weapon.";
				Phase = 101;
			}
		}
		else if (Phase == 5)
		{
			if (Police.PhotoEvidence > 0)
			{
				TeleportYandere();
				Yandere.CharacterAnimation.Play("f02_disappointed_00");
				Label.text = "The police find a smartphone with photographic evidence of Ayano committing a crime.";
				Phase = 100;
			}
			else
			{
				Phase++;
				UpdateScene();
			}
		}
		else if (Phase == 6)
		{
			if (SchoolGlobals.HighSecurity)
			{
				SecuritySystemScene.SetActive(true);
				if (!SecuritySystem.Evidence)
				{
					Label.text = "The police investigate the security camera recordings, but cannot find anything incriminating in the footage.";
					Phase++;
				}
				else if (!SecuritySystem.Masked)
				{
					Label.text = "The police investigate the security camera recordings, and find incriminating footage of Ayano.";
					Phase = 100;
				}
				else
				{
					Label.text = "The police investigate the security camera recordings, and find footage of a suspicious masked person.";
					Police.MaskReported = true;
					Phase++;
				}
			}
			else
			{
				Phase++;
				UpdateScene();
			}
		}
		else if (Phase == 7)
		{
			ShruggingCops.SetActive(false);
			if (Yandere.Sanity > 33.33333f)
			{
				if ((Yandere.Bloodiness > 0f && !Yandere.RedPaint) || (Yandere.Gloved && Yandere.Gloves.Blood.enabled))
				{
					if (Arrests == 0)
					{
						TeleportYandere();
						Yandere.CharacterAnimation.Play("f02_disappointed_00");
						Label.text = "The police notice that Ayano's clothing is bloody. They confirm that the blood is not hers. Ayano is unable to convince the police that she did not commit murder.";
						Phase = 100;
						return;
					}
					TeleportYandere();
					Yandere.CharacterAnimation["YandereConfessionRejected"].time = 4f;
					Yandere.CharacterAnimation.Play("YandereConfessionRejected");
					Label.text = "The police notice that Ayano's clothing is bloody. They confirm that the blood is not hers. Ayano is able to convince the police that she was splashed with blood while witnessing a murder.";
					if (!TranqCase.Occupied)
					{
						Phase = 9;
					}
					else
					{
						Phase++;
					}
				}
				else if (Police.BloodyClothing - ClothingWithRedPaint > 0)
				{
					TeleportYandere();
					Yandere.CharacterAnimation.Play("f02_disappointed_00");
					Label.text = "The police find bloody clothing that has traces of Ayano's DNA. Ayano is unable to convince the police that she did not commit murder.";
					Phase = 100;
				}
				else
				{
					TeleportYandere();
					Yandere.CharacterAnimation["YandereConfessionRejected"].time = 4f;
					Yandere.CharacterAnimation.Play("YandereConfessionRejected");
					Label.text = "The police question all students in the school, including Ayano. The police are unable to link Ayano to any crimes.";
					if (!TranqCase.Occupied)
					{
						Phase = 9;
					}
					else if (TranqCase.VictimID == ArrestID)
					{
						Phase = 9;
					}
					else
					{
						Phase++;
					}
				}
			}
			else
			{
				TeleportYandere();
				Yandere.CharacterAnimation.Play("f02_disappointed_00");
				if (Yandere.Bloodiness == 0f)
				{
					Label.text = "The police question Ayano, who exhibits extremely unusual behavior. The police decide to investigate Ayano further, and eventually learn of her crimes.";
					Phase = 100;
				}
				else
				{
					Label.text = "The police notice that Ayano is covered in blood and exhibiting extremely unusual behavior. The police decide to investigate Ayano further, and eventually learn of her crimes.";
					Phase = 100;
				}
			}
		}
		else if (Phase == 8)
		{
			KidnappedVictim = StudentManager.Students[TranqCase.VictimID];
			KidnappedVictim.CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
			KidnappedVictim.CharacterAnimation.enabled = true;
			KidnappedVictim.gameObject.SetActive(true);
			KidnappedVictim.Ragdoll.Zs.SetActive(false);
			KidnappedVictim.transform.parent = base.transform;
			KidnappedVictim.transform.localPosition = new Vector3(0f, 0.145f, 0f);
			KidnappedVictim.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
			KidnappedVictim.CharacterAnimation.Play("f02_sit_06");
			KidnappedVictim.WhiteQuestionMark.SetActive(true);
			OpenTranqCase.SetActive(true);
			Label.text = "The police discover " + JSON.Students[TranqCase.VictimID].Name + " inside of a musical instrument case. However, she is unable to recall how she got inside of the case. The police are unable to determine what happened.";
			StudentGlobals.SetStudentKidnapped(TranqCase.VictimID, false);
			StudentGlobals.SetStudentMissing(TranqCase.VictimID, false);
			TranqCase.VictimClubType = ClubType.None;
			TranqCase.VictimID = 0;
			TranqCase.Occupied = false;
			Phase++;
		}
		else if (Phase == 9)
		{
			if (Police.MaskReported)
			{
				Mask.SetActive(true);
				GameGlobals.MasksBanned = true;
				if (SecuritySystem.Masked)
				{
					Label.text = "In security camera footage, the killer was wearing a mask. As a result, the police are unable to gather meaningful information about the murderer. To prevent this from ever happening again, the Headmaster decides to ban all masks from the school from this day forward.";
				}
				else
				{
					Label.text = "Witnesses state that the killer was wearing a mask. As a result, the police are unable to gather meaningful information about the murderer. To prevent this from ever happening again, the Headmaster decides to ban all masks from the school from this day forward.";
				}
				Police.MaskReported = false;
				Phase++;
			}
			else
			{
				Phase++;
				UpdateScene();
			}
		}
		else if (Phase == 10)
		{
			Cops.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			Cops.SetActive(true);
			if (Arrests == 0)
			{
				if (DeadPerps == 0)
				{
					Label.text = "The police do not have enough evidence to perform an arrest. The police investigation ends, and students are free to leave.";
				}
				else
				{
					Label.text = "The police conclude that a murder-suicide took place, but are unable to take any further action. The police investigation ends, and students are free to leave.";
				}
			}
			else if (Arrests == 1)
			{
				Label.text = "The police believe that they have arrested the perpetrator of the crime. The police investigation ends, and students are free to leave.";
			}
			else
			{
				Label.text = "The police believe that they have arrested the perpetrators of the crimes. The police investigation ends, and students are free to leave.";
			}
			Phase++;
		}
		else if (Phase == 11)
		{
			Senpai.enabled = false;
			Senpai.transform.parent = base.transform;
			Senpai.gameObject.SetActive(true);
			Senpai.transform.localPosition = new Vector3(0f, 0f, 0f);
			Senpai.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			Senpai.EmptyHands();
			Senpai.CharacterAnimation.Play(Senpai.WalkAnim);
			Yandere.LookAt.enabled = true;
			Yandere.MyController.enabled = false;
			Yandere.transform.parent = base.transform;
			Yandere.transform.localPosition = new Vector3(2.5f, 0f, 2.5f);
			Yandere.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			Yandere.CharacterAnimation.Play(Yandere.WalkAnim);
			Physics.SyncTransforms();
			Label.text = "Ayano stalks Senpai until he has returned home safely, and then returns to her own home.";
			Phase++;
		}
		else if (Phase == 12)
		{
			if (!StudentGlobals.GetStudentDying(30) && !StudentGlobals.GetStudentDead(30) && !StudentGlobals.GetStudentArrested(30))
			{
				if (Counselor.LectureID > 0)
				{
					Yandere.gameObject.SetActive(false);
					for (int l = 1; l < 101; l++)
					{
						StudentManager.DisableStudent(l);
					}
					EODCamera.position = new Vector3(-18.5f, 1f, 6.5f);
					EODCamera.eulerAngles = new Vector3(0f, -45f, 0f);
					Counselor.Lecturing = true;
					base.enabled = false;
				}
				else
				{
					Phase++;
					UpdateScene();
				}
			}
			else
			{
				Phase++;
				UpdateScene();
			}
		}
		else if (Phase == 13)
		{
			EODCamera.localPosition = new Vector3(1f, 1.8f, -2.5f);
			EODCamera.localEulerAngles = new Vector3(22.5f, -22.5f, 0f);
			TextWindow.SetActive(true);
			if (SchemeGlobals.GetSchemeStage(2) == 3)
			{
				if (!StudentGlobals.GetStudentDying(30) && !StudentGlobals.GetStudentDead(30) && !StudentGlobals.GetStudentArrested(30))
				{
					GaudyRing.SetActive(true);
					Label.text = "Kokona discovers Sakyu's ring inside of her book bag. She returns the ring to Sakyu, who decides to stop bringing it to school.";
					SchemeGlobals.SetSchemeStage(2, 100);
				}
				else
				{
					GaudyRing.SetActive(true);
					Label.text = "Sakyu Basu's ring is permanently lost.";
					SchemeGlobals.SetSchemeStage(2, 100);
				}
			}
			else if (SchemeGlobals.GetSchemeStage(5) > 1 && SchemeGlobals.GetSchemeStage(5) < 5)
			{
				AnswerSheet.SetActive(true);
				Label.text = "A faculty member discovers that an answer sheet for an upcoming test is missing. She changes all of the questions for the test and keeps the new answer sheet with her at all times.";
				SchemeGlobals.SetSchemeStage(5, 100);
			}
			else
			{
				Phase++;
				UpdateScene();
			}
		}
		else if (Phase == 14)
		{
			ClubClosed = false;
			ClubKicked = false;
			float num = 1.2f;
			if (ClubID < ClubArray.Length)
			{
				if (!ClubGlobals.GetClubClosed(ClubArray[ClubID]))
				{
					ClubManager.CheckClub(ClubArray[ClubID]);
					if (ClubManager.ClubMembers < 5)
					{
						EODCamera.position = ClubManager.ClubVantages[ClubID].position;
						EODCamera.eulerAngles = ClubManager.ClubVantages[ClubID].eulerAngles;
						EODCamera.Translate(Vector3.forward * num, Space.Self);
						ClubGlobals.SetClubClosed(ClubArray[ClubID], true);
						Label.text = "The " + ClubNames[ClubID].ToString() + " no longer has enough members to remain operational. The school forces the club to disband.";
						ClubClosed = true;
						if (ClubGlobals.Club == ClubArray[ClubID])
						{
							ClubGlobals.Club = ClubType.None;
						}
					}
					if (ClubManager.LeaderMissing)
					{
						EODCamera.position = ClubManager.ClubVantages[ClubID].position;
						EODCamera.eulerAngles = ClubManager.ClubVantages[ClubID].eulerAngles;
						EODCamera.Translate(Vector3.forward * num, Space.Self);
						ClubGlobals.SetClubClosed(ClubArray[ClubID], true);
						Label.text = "The leader of the " + ClubNames[ClubID].ToString() + " has gone missing. The " + ClubNames[ClubID].ToString() + " cannot operate without its leader. The club disbands.";
						ClubClosed = true;
						if (ClubGlobals.Club == ClubArray[ClubID])
						{
							ClubGlobals.Club = ClubType.None;
						}
					}
					else if (ClubManager.LeaderDead)
					{
						EODCamera.position = ClubManager.ClubVantages[ClubID].position;
						EODCamera.eulerAngles = ClubManager.ClubVantages[ClubID].eulerAngles;
						EODCamera.Translate(Vector3.forward * num, Space.Self);
						ClubGlobals.SetClubClosed(ClubArray[ClubID], true);
						Label.text = "The leader of the " + ClubNames[ClubID].ToString() + " is gone. The " + ClubNames[ClubID].ToString() + " cannot operate without its leader. The club disbands.";
						ClubClosed = true;
						if (ClubGlobals.Club == ClubArray[ClubID])
						{
							ClubGlobals.Club = ClubType.None;
						}
					}
					else if (ClubManager.LeaderAshamed)
					{
						EODCamera.position = ClubManager.ClubVantages[ClubID].position;
						EODCamera.eulerAngles = ClubManager.ClubVantages[ClubID].eulerAngles;
						EODCamera.Translate(Vector3.forward * num, Space.Self);
						ClubGlobals.SetClubClosed(ClubArray[ClubID], true);
						Label.text = "The leader of the " + ClubNames[ClubID].ToString() + " has unexpectedly disbanded the club without explanation.";
						ClubClosed = true;
						ClubManager.LeaderAshamed = false;
						if (ClubGlobals.Club == ClubArray[ClubID])
						{
							ClubGlobals.Club = ClubType.None;
						}
					}
				}
				if (!ClubGlobals.GetClubClosed(ClubArray[ClubID]) && !ClubGlobals.GetClubKicked(ClubArray[ClubID]) && ClubGlobals.Club == ClubArray[ClubID])
				{
					ClubManager.CheckGrudge(ClubArray[ClubID]);
					if (ClubManager.LeaderGrudge)
					{
						EODCamera.position = ClubManager.ClubVantages[ClubID].position;
						EODCamera.eulerAngles = ClubManager.ClubVantages[ClubID].eulerAngles;
						EODCamera.Translate(Vector3.forward * num, Space.Self);
						Label.text = "Ayano receives a text message from the president of the " + ClubNames[ClubID].ToString() + ". Ayano is no longer a member of the " + ClubNames[ClubID].ToString() + ", and is not welcome in the " + ClubNames[ClubID].ToString() + " room.";
						ClubGlobals.SetClubKicked(ClubArray[ClubID], true);
						ClubGlobals.Club = ClubType.None;
						ClubKicked = true;
					}
					else if (ClubManager.ClubGrudge)
					{
						EODCamera.position = ClubManager.ClubVantages[ClubID].position;
						EODCamera.eulerAngles = ClubManager.ClubVantages[ClubID].eulerAngles;
						EODCamera.Translate(Vector3.forward * num, Space.Self);
						Label.text = "Ayano receives a text message from the president of the " + ClubNames[ClubID].ToString() + ". There is someone in the " + ClubNames[ClubID].ToString() + " who hates and fears Ayano. Ayano is no longer a member of the " + ClubNames[ClubID].ToString() + ", and is not welcome in the " + ClubNames[ClubID].ToString() + " room.";
						ClubGlobals.SetClubKicked(ClubArray[ClubID], true);
						ClubGlobals.Club = ClubType.None;
						ClubKicked = true;
					}
				}
				if (!ClubClosed && !ClubKicked)
				{
					ClubID++;
					UpdateScene();
				}
				ClubManager.LeaderAshamed = false;
			}
			else
			{
				Phase++;
				UpdateScene();
			}
		}
		else if (Phase == 15)
		{
			if (TranqCase.Occupied)
			{
				ClosedTranqCase.SetActive(true);
				Label.color = new Color(Label.color.r, Label.color.g, Label.color.b, 1f);
				Label.text = "Ayano waits until midnight, sneaks into school, and returns to the musical instrument case that contains her unconscious victim. She pushes the case back to her house and ties the victim to a chair in her basement.";
				Phase++;
			}
			else
			{
				Phase++;
				UpdateScene();
			}
		}
		else if (Phase == 16)
		{
			if (ErectFence)
			{
				Fence.SetActive(true);
				Label.text = "To prevent any other students from falling off of the school rooftop, the school erects a fence around the roof.";
				SchoolGlobals.RoofFence = true;
				ErectFence = false;
			}
			else
			{
				Phase++;
				UpdateScene();
			}
		}
		else if (Phase == 17)
		{
			if (!SchoolGlobals.HighSecurity && Police.CouncilDeath)
			{
				SCP.SetActive(true);
				Label.text = "The student council president has ordered the implementation of heightened security precautions. Security cameras and metal detectors are now present at school.";
				Police.CouncilDeath = false;
			}
			else
			{
				Phase++;
				UpdateScene();
			}
		}
		else if (Phase == 18)
		{
			Finish();
		}
		else if (Phase == 100)
		{
			Yandere.MyController.enabled = false;
			Yandere.transform.parent = base.transform;
			Yandere.transform.localPosition = new Vector3(0f, 0f, 0f);
			Yandere.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			Yandere.CharacterAnimation.Play("f02_handcuffs_00");
			Yandere.Handcuffs.SetActive(true);
			ArrestingCops.SetActive(true);
			Physics.SyncTransforms();
			Label.text = "Ayano is arrested by the police. She will never have Senpai.";
			GameOver = true;
		}
		else if (Phase == 101)
		{
			int fingerprintID2 = WeaponManager.Weapons[WeaponID].FingerprintID;
			StudentScript studentScript = StudentManager.Students[fingerprintID2];
			if (studentScript.Alive)
			{
				Patsy = StudentManager.Students[fingerprintID2];
				Patsy.CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
				Patsy.CharacterAnimation.enabled = true;
				if (Patsy.WeaponBag != null)
				{
					Patsy.WeaponBag.SetActive(false);
				}
				Patsy.EmptyHands();
				Patsy.SpeechLines.Stop();
				Patsy.Handcuffs.SetActive(true);
				Patsy.gameObject.SetActive(true);
				Patsy.Ragdoll.Zs.SetActive(false);
				Patsy.MyController.enabled = false;
				Patsy.transform.parent = base.transform;
				Patsy.transform.localPosition = new Vector3(0f, 0f, 0f);
				Patsy.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				Patsy.ShoeRemoval.enabled = false;
				if (StudentManager.Students[fingerprintID2].Male)
				{
					StudentManager.Students[fingerprintID2].CharacterAnimation.Play("handcuffs_00");
				}
				else
				{
					StudentManager.Students[fingerprintID2].CharacterAnimation.Play("f02_handcuffs_00");
				}
				ArrestingCops.SetActive(true);
				if (!studentScript.Tranquil)
				{
					Label.text = JSON.Students[fingerprintID2].Name + " is arrested by the police.";
					StudentGlobals.SetStudentArrested(fingerprintID2, true);
					Arrests++;
				}
				else
				{
					Label.text = JSON.Students[fingerprintID2].Name + " is found asleep inside of a musical instrument case. The police assume that she hid herself inside of the box after committing murder, and arrest her.";
					StudentGlobals.SetStudentArrested(fingerprintID2, true);
					ArrestID = fingerprintID2;
					TranqCase.Occupied = false;
					Arrests++;
				}
			}
			else
			{
				ShruggingCops.SetActive(true);
				bool flag = false;
				for (ID = 0; ID < VictimArray.Length; ID++)
				{
					if (VictimArray[ID] == fingerprintID2 && !studentScript.MurderSuicide)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					Label.text = JSON.Students[fingerprintID2].Name + " is dead. The police cannot perform an arrest.";
					DeadPerps++;
				}
				else
				{
					Label.text = JSON.Students[fingerprintID2].Name + "'s fingerprints are on the same weapon that killed them. The police cannot solve this mystery.";
				}
			}
			Phase = 3;
		}
		else if (Phase == 102)
		{
			if (Police.SuicideStudent.activeInHierarchy)
			{
				MurderScene.SetActive(true);
				Label.text = "The police inspect the corpse of a student who appears to have fallen to their death from the school rooftop. The police treat the incident as a murder case, and search the school for any other victims.";
				ErectFence = true;
			}
			else
			{
				ShruggingCops.SetActive(true);
				Label.text = "The police attempt to determine whether or not a student fell to their death from the school rooftop. The police are unable to reach a conclusion.";
			}
			for (ID = 0; ID < Police.CorpseList.Length; ID++)
			{
				RagdollScript ragdollScript2 = Police.CorpseList[ID];
				if (ragdollScript2 != null && ragdollScript2.Suicide)
				{
					ragdollScript2 = null;
					Police.Corpses--;
				}
			}
			Phase = 2;
		}
		else if (Phase == 103)
		{
			MurderScene.SetActive(true);
			Label.text = "The paramedics attempt to resuscitate the poisoned student, but they are unable to revive her. The police treat the incident as a murder case, and search the school for any other victims.";
			for (ID = 0; ID < Police.CorpseList.Length; ID++)
			{
				RagdollScript ragdollScript3 = Police.CorpseList[ID];
				if (ragdollScript3 != null && ragdollScript3.Poisoned)
				{
					ragdollScript3 = null;
					Police.Corpses--;
				}
			}
			Phase = 2;
		}
		else if (Phase == 104)
		{
			MurderScene.SetActive(true);
			Label.text = "The police determine that " + Police.DrownedStudentName + " died from drowning. The police treat the death as a possible murder, and search the school for any other victims.";
			for (ID = 0; ID < Police.CorpseList.Length; ID++)
			{
				RagdollScript ragdollScript4 = Police.CorpseList[ID];
				if (ragdollScript4 != null && ragdollScript4.Drowned)
				{
					ragdollScript4 = null;
					Police.Corpses--;
				}
			}
			Phase = 2;
		}
		else
		{
			if (Phase != 105)
			{
				return;
			}
			MurderScene.SetActive(true);
			Label.text = "The police determine that " + Police.ElectrocutedStudentName + " died from being electrocuted. The police treat the death as a possible murder, and search the school for any other victims.";
			for (ID = 0; ID < Police.CorpseList.Length; ID++)
			{
				RagdollScript ragdollScript5 = Police.CorpseList[ID];
				if (ragdollScript5 != null && ragdollScript5.Electrocuted)
				{
					ragdollScript5 = null;
					Police.Corpses--;
				}
			}
			Phase = 2;
		}
	}

	private void TeleportYandere()
	{
		Yandere.MyController.enabled = false;
		Yandere.transform.parent = base.transform;
		Yandere.transform.localPosition = new Vector3(0.75f, 0.33333f, -1.9f);
		Yandere.transform.localEulerAngles = new Vector3(-22.5f, 157.5f, 0f);
		Physics.SyncTransforms();
	}

	private void Finish()
	{
		PlayerGlobals.Reputation = Reputation.Reputation;
		HomeGlobals.Night = true;
		Police.KillStudents();
		if (Police.Suspended)
		{
			DateGlobals.PassDays = Police.SuspensionLength;
		}
		if (StudentManager.Students[SchoolGlobals.KidnapVictim] != null && StudentManager.Students[SchoolGlobals.KidnapVictim].Ragdoll.enabled)
		{
			SchoolGlobals.KidnapVictim = 0;
		}
		if (!TranqCase.Occupied)
		{
			SceneManager.LoadScene("HomeScene");
		}
		else
		{
			SchoolGlobals.KidnapVictim = TranqCase.VictimID;
			StudentGlobals.SetStudentKidnapped(TranqCase.VictimID, true);
			StudentGlobals.SetStudentSanity(TranqCase.VictimID, 100f);
			SceneManager.LoadScene("CalendarScene");
		}
		if (Dumpster.StudentToGoMissing > 0)
		{
			Dumpster.SetVictimMissing();
		}
		for (ID = 0; ID < GardenHoles.Length; ID++)
		{
			GardenHoles[ID].EndOfDayCheck();
		}
		Incinerator.SetVictimsMissing();
		WoodChipper.SetVictimsMissing();
		if (FragileTarget > 0)
		{
			Debug.Log("Setting target for Fragile student.");
			StudentGlobals.SetFragileTarget(FragileTarget);
			StudentGlobals.SetStudentFragileSlave(5);
		}
		if (StudentManager.ReactedToGameLeader)
		{
			SchoolGlobals.ReactedToGameLeader = true;
		}
		if (NewFriends > 0)
		{
			PlayerGlobals.Friends += NewFriends;
		}
		if (Yandere.Alerts > 0)
		{
			PlayerGlobals.Alerts += Yandere.Alerts;
		}
		SchoolGlobals.SchoolAtmosphere += (float)Arrests * 0.05f;
		if (Counselor.ExpelledDelinquents)
		{
			SchoolGlobals.SchoolAtmosphere += 0.25f;
		}
		WeaponManager.TrackDumpedWeapons();
	}
}
