using UnityEngine;

public class ShutterScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public TaskManagerScript TaskManager;

	public PauseScreenScript PauseScreen;

	public StudentInfoScript StudentInfo;

	public PromptBarScript PromptBar;

	public SubtitleScript Subtitle;

	public SchemesScript Schemes;

	public StudentScript Student;

	public YandereScript Yandere;

	public StudentScript FaceStudent;

	public RenderTexture SmartphoneScreen;

	public Camera SmartphoneCamera;

	public Transform TextMessages;

	public Transform ErrorWindow;

	public Camera MainCamera;

	public UILabel PhotoDescLabel;

	public UISprite Sprite;

	public GameObject NotificationManager;

	public GameObject BullyPhotoCollider;

	public GameObject PhotoDescription;

	public GameObject HeartbeatCamera;

	public GameObject CameraButtons;

	public GameObject NewMessage;

	public GameObject PhotoIcons;

	public GameObject MainMenu;

	public GameObject SubPanel;

	public GameObject Message;

	public GameObject Panel;

	public GameObject ViolenceX;

	public GameObject PantiesX;

	public GameObject SenpaiX;

	public GameObject BullyX;

	public GameObject InfoX;

	public bool AirGuitarShot;

	public bool DisplayError;

	public bool MissionMode;

	public bool KittenShot;

	public bool FreeSpace;

	public bool TakePhoto;

	public bool TookPhoto;

	public bool Snapping;

	public bool Close;

	public bool Disguise;

	public bool Nemesis;

	public bool NotFace;

	public bool Skirt;

	public RaycastHit hit;

	public float ReactionDistance;

	public float PenaltyTimer;

	public float Timer;

	private float currentPercent;

	public int TargetStudent;

	public int NemesisShots;

	public int Frame;

	public int Slot;

	public int ID;

	public AudioSource MyAudio;

	public Transform SelfieRayParent;

	public int OnlyPhotography
	{
		get
		{
			return 65537;
		}
	}

	public int OnlyCharacters
	{
		get
		{
			return 513;
		}
	}

	public int OnlyRagdolls
	{
		get
		{
			return 2049;
		}
	}

	public int OnlyBlood
	{
		get
		{
			return 16385;
		}
	}

	private void Start()
	{
		if (MissionModeGlobals.MissionMode)
		{
			MissionMode = true;
		}
		ErrorWindow.transform.localScale = Vector3.zero;
		CameraButtons.SetActive(false);
		PhotoIcons.SetActive(false);
		Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, 0f);
	}

	private void Update()
	{
		if (!Yandere.Selfie)
		{
			Debug.DrawRay(SmartphoneCamera.transform.position, SmartphoneCamera.transform.TransformDirection(Vector3.forward) * 10f, Color.green);
		}
		else
		{
			Debug.DrawRay(SmartphoneCamera.transform.position, SelfieRayParent.TransformDirection(Vector3.forward) * 10f, Color.green);
		}
		if (Snapping)
		{
			if (Close)
			{
				currentPercent += 60f * Time.unscaledDeltaTime;
				while (currentPercent >= 1f)
				{
					Frame = Mathf.Min(Frame + 1, 8);
					currentPercent -= 1f;
				}
				Sprite.spriteName = "Shutter" + Frame;
				if (Frame == 8)
				{
					StudentManager.GhostChan.gameObject.SetActive(true);
					PhotoDescription.SetActive(false);
					PhotoDescLabel.text = string.Empty;
					StudentManager.GhostChan.Look();
					CheckPhoto();
					if (PhotoDescLabel.text == string.Empty)
					{
						PhotoDescLabel.text = "Cannot determine subject of photo. Try again.";
					}
					PhotoDescription.SetActive(true);
					SmartphoneCamera.targetTexture = null;
					Yandere.PhonePromptBar.Show = false;
					NotificationManager.SetActive(false);
					HeartbeatCamera.SetActive(false);
					Yandere.SelfieGuide.SetActive(false);
					MainCamera.enabled = false;
					PhotoIcons.SetActive(true);
					SubPanel.SetActive(false);
					Panel.SetActive(false);
					Close = false;
					PromptBar.ClearButtons();
					PromptBar.Label[0].text = "Save";
					PromptBar.Label[1].text = "Delete";
					if (!Yandere.RivalPhone)
					{
						PromptBar.Label[2].text = "Send";
					}
					PromptBar.UpdateButtons();
					PromptBar.Show = true;
					Time.timeScale = 0.0001f;
				}
			}
			else
			{
				currentPercent += 60f * Time.unscaledDeltaTime;
				while (currentPercent >= 1f)
				{
					Frame = Mathf.Max(Frame - 1, 1);
					currentPercent -= 1f;
				}
				Sprite.spriteName = "Shutter" + Frame;
				if (Frame == 1)
				{
					Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, 0f);
					Snapping = false;
				}
			}
		}
		else if (Yandere.Aiming)
		{
			TargetStudent = 0;
			Timer += Time.deltaTime;
			if (Timer > 0.5f)
			{
				Vector3 direction = (Yandere.Selfie ? SelfieRayParent.TransformDirection(Vector3.forward) : SmartphoneCamera.transform.TransformDirection(Vector3.forward));
				if (Physics.Raycast(SmartphoneCamera.transform.position, direction, out hit, float.PositiveInfinity, OnlyPhotography))
				{
					if (hit.collider.gameObject.name == "Face")
					{
						GameObject gameObject = hit.collider.gameObject.transform.root.gameObject;
						FaceStudent = gameObject.GetComponent<StudentScript>();
						if (FaceStudent != null)
						{
							TargetStudent = FaceStudent.StudentID;
							if (TargetStudent > 1)
							{
								ReactionDistance = 1.66666f;
							}
							else
							{
								ReactionDistance = FaceStudent.VisionDistance;
							}
							bool flag = FaceStudent.ShoeRemoval.enabled;
							if (!FaceStudent.Alarmed && !FaceStudent.Dying && !FaceStudent.Distracted && !FaceStudent.InEvent && !FaceStudent.Wet && FaceStudent.Schoolwear > 0 && !FaceStudent.Fleeing && !FaceStudent.Following && !flag && !FaceStudent.HoldingHands && FaceStudent.Actions[FaceStudent.Phase] != StudentActionType.Mourn && !FaceStudent.Guarding && !FaceStudent.Confessing && !FaceStudent.DiscCheck && !FaceStudent.TurnOffRadio && !FaceStudent.Investigating && !FaceStudent.Distracting && Vector3.Distance(Yandere.transform.position, gameObject.transform.position) < ReactionDistance && FaceStudent.CanSeeObject(Yandere.gameObject, Yandere.transform.position + Vector3.up))
							{
								if (MissionMode)
								{
									PenaltyTimer += Time.deltaTime;
									if (PenaltyTimer > 1f)
									{
										FaceStudent.Reputation.PendingRep -= -10f;
										PenaltyTimer = 0f;
									}
								}
								if (!FaceStudent.CameraReacting)
								{
									if (FaceStudent.enabled && !FaceStudent.Stop)
									{
										if ((FaceStudent.DistanceToDestination < 5f && FaceStudent.Actions[FaceStudent.Phase] == StudentActionType.Graffiti) || (FaceStudent.DistanceToDestination < 5f && FaceStudent.Actions[FaceStudent.Phase] == StudentActionType.Bully))
										{
											FaceStudent.PhotoPatience = 0f;
											FaceStudent.KilledMood = true;
											FaceStudent.Ignoring = true;
											PenaltyTimer = 1f;
											Penalize();
										}
										else if (FaceStudent.PhotoPatience > 0f)
										{
											if (FaceStudent.StudentID > 1)
											{
												if ((Yandere.Bloodiness > 0f && !Yandere.Paint) || (double)Yandere.Sanity < 33.33333)
												{
													FaceStudent.Alarm += 200f;
												}
												else
												{
													FaceStudent.CameraReact();
												}
											}
											else
											{
												FaceStudent.Alarm += Time.deltaTime * (100f / FaceStudent.DistanceToPlayer) * FaceStudent.Paranoia * FaceStudent.Perception * FaceStudent.DistanceToPlayer * 2f;
												FaceStudent.YandereVisible = true;
											}
										}
										else
										{
											Penalize();
										}
									}
								}
								else
								{
									FaceStudent.PhotoPatience = Mathf.MoveTowards(FaceStudent.PhotoPatience, 0f, Time.deltaTime);
									if (FaceStudent.PhotoPatience > 0f)
									{
										FaceStudent.CameraPoseTimer = 1f;
										if (MissionMode)
										{
											FaceStudent.PhotoPatience = 0f;
										}
									}
								}
							}
						}
					}
					else if (hit.collider.gameObject.name == "Panties" || hit.collider.gameObject.name == "Skirt")
					{
						GameObject gameObject2 = hit.collider.gameObject.transform.root.gameObject;
						if (Physics.Raycast(SmartphoneCamera.transform.position, direction, out hit, float.PositiveInfinity, OnlyCharacters))
						{
							if (Vector3.Distance(Yandere.transform.position, gameObject2.transform.position) < 5f)
							{
								if (hit.collider.gameObject == gameObject2)
								{
									if (!Yandere.Lewd)
									{
										Yandere.NotificationManager.DisplayNotification(NotificationType.Lewd);
									}
									Yandere.Lewd = true;
								}
								else
								{
									Yandere.Lewd = false;
								}
							}
							else
							{
								Yandere.Lewd = false;
							}
						}
					}
					else
					{
						Yandere.Lewd = false;
					}
				}
				else
				{
					Yandere.Lewd = false;
				}
			}
		}
		else
		{
			Timer = 0f;
		}
		if (TookPhoto)
		{
			ResumeGameplay();
		}
		if (!DisplayError)
		{
			if (PhotoIcons.activeInHierarchy && !Snapping && !TextMessages.gameObject.activeInHierarchy)
			{
				if (Input.GetButtonDown("A"))
				{
					if (!Yandere.RivalPhone)
					{
						bool flag2 = !BullyX.activeInHierarchy;
						bool flag3 = !SenpaiX.activeInHierarchy;
						PromptBar.transform.localPosition = new Vector3(PromptBar.transform.localPosition.x, -627f, PromptBar.transform.localPosition.z);
						PromptBar.ClearButtons();
						PromptBar.Show = false;
						PhotoIcons.SetActive(false);
						ID = 0;
						FreeSpace = false;
						while (ID < 26)
						{
							ID++;
							if (!PlayerGlobals.GetPhoto(ID))
							{
								FreeSpace = true;
								Slot = ID;
								ID = 26;
							}
						}
						if (FreeSpace)
						{
							ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/Photographs/Photo_" + Slot + ".png");
							TookPhoto = true;
							Debug.Log("Setting Photo " + Slot + " to ''true''.");
							PlayerGlobals.SetPhoto(Slot, true);
							if (flag2)
							{
								Debug.Log("Saving a bully photo!");
								int studentID = BullyPhotoCollider.transform.parent.gameObject.GetComponent<StudentScript>().StudentID;
								if (StudentManager.Students[studentID].Club != ClubType.Bully)
								{
									PlayerGlobals.SetBullyPhoto(Slot, studentID);
								}
								else
								{
									PlayerGlobals.SetBullyPhoto(Slot, StudentManager.Students[studentID].DistractionTarget.StudentID);
								}
							}
							if (flag3)
							{
								PlayerGlobals.SetSenpaiPhoto(Slot, true);
							}
							if (AirGuitarShot)
							{
								TaskGlobals.SetGuitarPhoto(Slot, true);
								TaskManager.UpdateTaskStatus();
							}
							if (KittenShot)
							{
								TaskGlobals.SetKittenPhoto(Slot, true);
								TaskManager.UpdateTaskStatus();
							}
						}
						else
						{
							DisplayError = true;
						}
					}
					else if (!PantiesX.activeInHierarchy)
					{
						StudentManager.CommunalLocker.RivalPhone.LewdPhotos = true;
						SchemeGlobals.SetSchemeStage(4, 3);
						Schemes.UpdateInstructions();
						ResumeGameplay();
					}
				}
				if (!Yandere.RivalPhone && Input.GetButtonDown("X"))
				{
					Panel.SetActive(true);
					MainMenu.SetActive(false);
					PauseScreen.Show = true;
					PauseScreen.Panel.enabled = true;
					PromptBar.ClearButtons();
					PromptBar.Label[1].text = "Exit";
					if (PantiesX.activeInHierarchy)
					{
						PromptBar.Label[3].text = "Interests";
					}
					else
					{
						PromptBar.Label[3].text = string.Empty;
					}
					PromptBar.UpdateButtons();
					if (!InfoX.activeInHierarchy)
					{
						PauseScreen.Sideways = true;
						StudentGlobals.SetStudentPhotographed(Student.StudentID, true);
						for (ID = 0; ID < Student.Outlines.Length; ID++)
						{
							Student.Outlines[ID].enabled = true;
						}
						StudentInfo.UpdateInfo(Student.StudentID);
						StudentInfo.gameObject.SetActive(true);
					}
					else if (!TextMessages.gameObject.activeInHierarchy)
					{
						PauseScreen.Sideways = false;
						TextMessages.gameObject.SetActive(true);
						SpawnMessage();
					}
				}
				if (Input.GetButtonDown("B"))
				{
					ResumeGameplay();
				}
			}
			else if (PhotoIcons.activeInHierarchy && Input.GetButtonDown("B"))
			{
				ResumeGameplay();
			}
		}
		else
		{
			float t = Time.unscaledDeltaTime * 10f;
			ErrorWindow.transform.localScale = Vector3.Lerp(ErrorWindow.transform.localScale, new Vector3(1f, 1f, 1f), t);
			if (Input.GetButtonDown("A"))
			{
				ResumeGameplay();
			}
		}
	}

	public void Snap()
	{
		ErrorWindow.transform.localScale = Vector3.zero;
		Yandere.HandCamera.gameObject.SetActive(false);
		Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, 1f);
		MyAudio.Play();
		Snapping = true;
		Close = true;
		Frame = 0;
	}

	private void CheckPhoto()
	{
		InfoX.SetActive(true);
		BullyX.SetActive(true);
		SenpaiX.SetActive(true);
		PantiesX.SetActive(true);
		ViolenceX.SetActive(true);
		AirGuitarShot = false;
		KittenShot = false;
		Nemesis = false;
		NotFace = false;
		Skirt = false;
		Vector3 direction = (Yandere.Selfie ? SelfieRayParent.TransformDirection(Vector3.forward) : SmartphoneCamera.transform.TransformDirection(Vector3.forward));
		if (Physics.Raycast(SmartphoneCamera.transform.position, direction, out hit, float.PositiveInfinity, OnlyPhotography))
		{
			Debug.Log("Took a picture of " + hit.collider.gameObject.name);
			Debug.Log("The root is " + hit.collider.gameObject.transform.root.name);
			if (hit.collider.gameObject.name == "Panties")
			{
				Student = hit.collider.gameObject.transform.root.gameObject.GetComponent<StudentScript>();
				PhotoDescLabel.text = "Photo of: " + Student.Name + "'s Panties";
				PantiesX.SetActive(false);
			}
			else if (hit.collider.gameObject.name == "Face")
			{
				if (hit.collider.gameObject.tag == "Nemesis")
				{
					PhotoDescLabel.text = "Photo of: Nemesis";
					Nemesis = true;
					NemesisShots++;
				}
				else if (hit.collider.gameObject.tag == "Disguise")
				{
					PhotoDescLabel.text = "Photo of: ?????";
					Disguise = true;
				}
				else
				{
					Student = hit.collider.gameObject.transform.root.gameObject.GetComponent<StudentScript>();
					if (Student.StudentID == 1)
					{
						PhotoDescLabel.text = "Photo of: Senpai";
						SenpaiX.SetActive(false);
					}
					else
					{
						PhotoDescLabel.text = "Photo of: " + Student.Name;
						InfoX.SetActive(false);
					}
				}
			}
			else if (hit.collider.gameObject.name == "NotFace")
			{
				PhotoDescLabel.text = "Photo of: Blocked Face";
				NotFace = true;
			}
			else if (hit.collider.gameObject.name == "Skirt")
			{
				PhotoDescLabel.text = "Photo of: Skirt";
				Skirt = true;
			}
			if (hit.collider.transform.root.gameObject.name == "Student_51 (Miyuji Shan)" && StudentManager.Students[51].AirGuitar.isPlaying)
			{
				AirGuitarShot = true;
				PhotoDescription.SetActive(true);
				PhotoDescLabel.text = "Photo of: Miyuji's True Nature?";
			}
			if (hit.collider.gameObject.name == "Kitten")
			{
				KittenShot = true;
				PhotoDescription.SetActive(true);
				PhotoDescLabel.text = "Photo of: Kitten";
				if (!ConversationGlobals.GetTopicDiscovered(20))
				{
					ConversationGlobals.SetTopicDiscovered(20, true);
					Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				}
			}
			if (hit.collider.gameObject.tag == "Bully")
			{
				PhotoDescLabel.text = "Photo of: Student Speaking With Bully";
				BullyPhotoCollider = hit.collider.gameObject;
				BullyX.SetActive(false);
			}
		}
		if (Physics.Raycast(SmartphoneCamera.transform.position, direction, out hit, float.PositiveInfinity, OnlyRagdolls) && hit.collider.gameObject.layer == 11)
		{
			PhotoDescLabel.text = "Photo of: Corpse";
			ViolenceX.SetActive(false);
		}
		if (Physics.Raycast(SmartphoneCamera.transform.position, SmartphoneCamera.transform.TransformDirection(Vector3.forward), out hit, float.PositiveInfinity, OnlyBlood) && hit.collider.gameObject.layer == 14)
		{
			PhotoDescLabel.text = "Photo of: Blood";
			ViolenceX.SetActive(false);
		}
	}

	private void SpawnMessage()
	{
		if (NewMessage != null)
		{
			Object.Destroy(NewMessage);
		}
		NewMessage = Object.Instantiate(Message);
		NewMessage.transform.parent = TextMessages;
		NewMessage.transform.localPosition = new Vector3(-225f, -275f, 0f);
		NewMessage.transform.localEulerAngles = Vector3.zero;
		NewMessage.transform.localScale = new Vector3(1f, 1f, 1f);
		bool flag = false;
		if (hit.collider != null && hit.collider.gameObject.name == "Kitten")
		{
			flag = true;
		}
		string empty = string.Empty;
		int num = 0;
		if (flag)
		{
			empty = "Why are you showing me this? I don't care.";
			num = 2;
		}
		else if (!InfoX.activeInHierarchy)
		{
			empty = "I recognize this person. Here's some information about them.";
			num = 3;
		}
		else if (!PantiesX.activeInHierarchy)
		{
			if (Student != null)
			{
				if (!PlayerGlobals.GetStudentPantyShot(Student.Name))
				{
					PlayerGlobals.SetStudentPantyShot(Student.Name, true);
					if (Student.Nemesis)
					{
						empty = "Wait...I recognize those panties! This person is extremely dangerous! Avoid her at all costs!";
					}
					else if (Student.Club == ClubType.Bully || Student.Club == ClubType.Council || Student.Club == ClubType.Nurse || Student.StudentID == 20)
					{
						empty = "A high value target! " + Student.Name + "'s panties were in high demand. I owe you a big favor for this one.";
						PlayerGlobals.PantyShots += 5;
					}
					else
					{
						empty = "Excellent! Now I have a picture of " + Student.Name + "'s panties. I owe you a favor for this one.";
						PlayerGlobals.PantyShots++;
					}
					num = 5;
				}
				else if (!Student.Nemesis)
				{
					empty = "I already have a picture of " + Student.Name + "'s panties. I don't need this shot.";
					num = 4;
				}
				else
				{
					empty = "You are in danger. Avoid her.";
					num = 2;
				}
			}
			else
			{
				empty = "How peculiar. I don't recognize these panties.";
				num = 2;
			}
		}
		else if (!ViolenceX.activeInHierarchy)
		{
			empty = "Good work, but don't send me this stuff. I have no use for it.";
			num = 3;
		}
		else if (!SenpaiX.activeInHierarchy)
		{
			if (PlayerGlobals.SenpaiShots == 0)
			{
				empty = "I don't need any pictures of your Senpai.";
				num = 2;
			}
			else if (PlayerGlobals.SenpaiShots == 1)
			{
				empty = "I know how you feel about this person, but I have no use for these pictures.";
				num = 4;
			}
			else if (PlayerGlobals.SenpaiShots == 2)
			{
				empty = "Okay, I get it, you love your Senpai, and you love taking pictures of your Senpai. I still don't need these shots.";
				num = 5;
			}
			else if (PlayerGlobals.SenpaiShots == 3)
			{
				empty = "You're spamming my inbox. Cut it out.";
				num = 2;
			}
			else
			{
				empty = "...";
				num = 1;
			}
			PlayerGlobals.SenpaiShots++;
		}
		else if (!BullyX.activeInHierarchy)
		{
			empty = "I have no interest in this.";
			num = 2;
		}
		else if (NotFace)
		{
			empty = "Do you want me to identify this person? Please get me a clear shot of their face.";
			num = 4;
		}
		else if (Skirt)
		{
			empty = "Is this supposed to be a panty shot? My clients are picky. The panties need to be in the EXACT center of the shot.";
			num = 5;
		}
		else if (Nemesis)
		{
			if (NemesisShots == 1)
			{
				empty = "Strange. I have no profile for this student.";
				num = 2;
			}
			else if (NemesisShots == 2)
			{
				empty = "...wait. I think I know who she is.";
				num = 2;
			}
			else if (NemesisShots == 3)
			{
				empty = "You are in danger. Avoid her.";
				num = 2;
			}
			else if (NemesisShots == 4)
			{
				empty = "Do not engage.";
				num = 1;
			}
			else
			{
				empty = "I repeat: Do. Not. Engage.";
				num = 2;
			}
		}
		else if (Disguise)
		{
			empty = "Something about that student seems...wrong.";
			num = 2;
		}
		else
		{
			empty = "I don't get it. What are you trying to show me? Make sure the subject is in the EXACT center of the photo.";
			num = 5;
		}
		NewMessage.GetComponent<UISprite>().height = 36 + 36 * num;
		NewMessage.GetComponent<TextMessageScript>().Label.text = empty;
	}

	private void ResumeGameplay()
	{
		ErrorWindow.transform.localScale = Vector3.zero;
		SmartphoneCamera.targetTexture = SmartphoneScreen;
		StudentManager.GhostChan.gameObject.SetActive(false);
		Yandere.HandCamera.gameObject.SetActive(true);
		NotificationManager.SetActive(true);
		PauseScreen.CorrectingTime = true;
		HeartbeatCamera.SetActive(true);
		TextMessages.gameObject.SetActive(false);
		StudentInfo.gameObject.SetActive(false);
		MainCamera.enabled = true;
		PhotoIcons.SetActive(false);
		PauseScreen.Show = false;
		SubPanel.SetActive(true);
		MainMenu.SetActive(true);
		Yandere.CanMove = true;
		DisplayError = false;
		Panel.SetActive(true);
		Time.timeScale = 1f;
		TakePhoto = false;
		TookPhoto = false;
		Yandere.PhonePromptBar.Panel.enabled = true;
		Yandere.PhonePromptBar.Show = true;
		PromptBar.ClearButtons();
		PromptBar.Show = false;
		if (NewMessage != null)
		{
			Object.Destroy(NewMessage);
		}
		if (!Yandere.CameraEffects.OneCamera)
		{
			Yandere.MainCamera.clearFlags = CameraClearFlags.Skybox;
			Yandere.MainCamera.farClipPlane = OptionGlobals.DrawDistance;
		}
		Yandere.UpdateSelfieStatus();
	}

	public void Penalize()
	{
		PenaltyTimer += Time.deltaTime;
		if (!(PenaltyTimer >= 1f))
		{
			return;
		}
		Subtitle.UpdateLabel(SubtitleType.PhotoAnnoyance, 0, 3f);
		if (MissionMode)
		{
			if (FaceStudent.TimesAnnoyed < 5)
			{
				FaceStudent.TimesAnnoyed++;
			}
			else
			{
				FaceStudent.RepDeduction = 0f;
				FaceStudent.RepLoss = 20f;
				FaceStudent.Reputation.PendingRep -= FaceStudent.RepLoss * FaceStudent.Paranoia;
				FaceStudent.PendingRep -= FaceStudent.RepLoss * FaceStudent.Paranoia;
			}
		}
		else
		{
			FaceStudent.RepDeduction = 0f;
			FaceStudent.RepLoss = 1f;
			FaceStudent.CalculateReputationPenalty();
			if (FaceStudent.RepDeduction >= 0f)
			{
				FaceStudent.RepLoss -= FaceStudent.RepDeduction;
			}
			FaceStudent.Reputation.PendingRep -= FaceStudent.RepLoss * FaceStudent.Paranoia;
			FaceStudent.PendingRep -= FaceStudent.RepLoss * FaceStudent.Paranoia;
		}
		PenaltyTimer = 0f;
	}
}
