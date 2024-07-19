using UnityEngine;

public class DatingMinigameScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public InputManagerScript InputManager;

	public LoveManagerScript LoveManager;

	public PromptBarScript PromptBar;

	public YandereScript Yandere;

	public StudentScript Suitor;

	public StudentScript Rival;

	public PromptScript Prompt;

	public JsonScript JSON;

	public Transform AffectionSet;

	public Transform OptionSet;

	public GameObject HeartbeatCamera;

	public GameObject SeductionIcon;

	public GameObject PantyIcon;

	public Transform TopicHighlight;

	public Transform ComplimentSet;

	public Transform AffectionBar;

	public Transform Highlight;

	public Transform GiveGift;

	public Transform PeekSpot;

	public Transform[] Options;

	public Transform ShowOff;

	public Transform Topics;

	public Texture X;

	public UISprite[] OpinionIcons;

	public UISprite[] TopicIcons;

	public UITexture[] MultiplierIcons;

	public UILabel[] ComplimentLabels;

	public UISprite[] ComplimentBGs;

	public UILabel MultiplierLabel;

	public UILabel SeductionLabel;

	public UILabel TopicNameLabel;

	public UILabel DialogueLabel;

	public UIPanel DatingSimHUD;

	public UILabel WisdomLabel;

	public UISprite[] TraitBGs;

	public UISprite[] GiftBGs;

	public UITexture RoseIcon;

	public UILabel[] Labels;

	public UIPanel Panel;

	public string[] OpinionSpriteNames;

	public string[] Compliments;

	public string[] TopicNames;

	public string[] GiveGifts;

	public string[] Greetings;

	public string[] Farewells;

	public string[] Negatives;

	public string[] Positives;

	public string[] ShowOffs;

	public bool SelectingTopic;

	public bool AffectionGrow;

	public bool Complimenting;

	public bool Matchmaking;

	public bool GivingGift;

	public bool ShowingOff;

	public bool Negative;

	public bool SlideOut;

	public bool Testing;

	public float HighlightTarget;

	public float Affection;

	public float Rotation;

	public float Speed;

	public float Timer;

	public int ComplimentSelected = 1;

	public int TraitSelected = 1;

	public int TopicSelected = 1;

	public int GiftSelected = 1;

	public int Selected = 1;

	public int AffectionLevel;

	public int Multiplier;

	public int Opinion;

	public int Phase = 1;

	public int GiftColumn = 1;

	public int GiftRow = 1;

	public int Column = 1;

	public int Row = 1;

	public int Side = 1;

	public int Line = 1;

	public string CurrentAnim = string.Empty;

	public Color OriginalColor;

	private void Start()
	{
		Affection = DatingGlobals.Affection;
		AffectionBar.localScale = new Vector3(Affection / 100f, AffectionBar.localScale.y, AffectionBar.localScale.z);
		CalculateAffection();
		OriginalColor = ComplimentBGs[1].color;
		ComplimentSet.localScale = Vector3.zero;
		GiveGift.localScale = Vector3.zero;
		ShowOff.localScale = Vector3.zero;
		Topics.localScale = Vector3.zero;
		DatingSimHUD.gameObject.SetActive(false);
		DatingSimHUD.alpha = 0f;
		for (int i = 1; i < 26; i++)
		{
			if (DatingGlobals.GetTopicDiscussed(i))
			{
				UISprite uISprite = TopicIcons[i];
				uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 0.5f);
			}
		}
		for (int j = 1; j < 11; j++)
		{
			if (DatingGlobals.GetComplimentGiven(j))
			{
				UILabel uILabel = ComplimentLabels[j];
				uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0.5f);
			}
		}
		UpdateComplimentHighlight();
		UpdateTraitHighlight();
		UpdateGiftHighlight();
	}

	private void CalculateAffection()
	{
		if (Affection == 0f)
		{
			AffectionLevel = 0;
		}
		else if (Affection < 25f)
		{
			AffectionLevel = 1;
		}
		else if (Affection < 50f)
		{
			AffectionLevel = 2;
		}
		else if (Affection < 75f)
		{
			AffectionLevel = 3;
		}
		else if (Affection < 100f)
		{
			AffectionLevel = 4;
		}
		else
		{
			AffectionLevel = 5;
		}
	}

	private void Update()
	{
		if (Testing)
		{
			Prompt.enabled = true;
		}
		else if (LoveManager.RivalWaiting)
		{
			if (Rival == null)
			{
				Suitor = StudentManager.Students[28];
				Rival = StudentManager.Students[30];
			}
			if (Rival.MeetTimer > 0f && Suitor.MeetTimer > 0f)
			{
				Prompt.enabled = true;
			}
		}
		else if (Prompt.enabled)
		{
			Prompt.Hide();
			Prompt.enabled = false;
		}
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			if (!Yandere.Chased && Yandere.Chasers == 0)
			{
				Suitor.enabled = false;
				Rival.enabled = false;
				Rival.Character.GetComponent<Animation>()["f02_smile_00"].layer = 1;
				Rival.Character.GetComponent<Animation>().Play("f02_smile_00");
				Rival.Character.GetComponent<Animation>()["f02_smile_00"].weight = 0f;
				StudentManager.Clock.StopTime = true;
				Yandere.RPGCamera.enabled = false;
				HeartbeatCamera.SetActive(false);
				Yandere.Headset.SetActive(true);
				Yandere.CanMove = false;
				Yandere.EmptyHands();
				if (Yandere.YandereVision)
				{
					Yandere.ResetYandereEffects();
					Yandere.YandereVision = false;
				}
				Yandere.transform.position = PeekSpot.position;
				Yandere.transform.eulerAngles = PeekSpot.eulerAngles;
				Yandere.Character.GetComponent<Animation>().Play("f02_treePeeking_00");
				Camera.main.transform.position = new Vector3(48f, 3f, -44f);
				Camera.main.transform.eulerAngles = new Vector3(15f, 90f, 0f);
				WisdomLabel.text = "Wisdom: " + DatingGlobals.GetSuitorTrait(2);
				if (!Suitor.Rose)
				{
					RoseIcon.enabled = false;
				}
				Matchmaking = true;
				UpdateTopics();
				Time.timeScale = 1f;
			}
		}
		if (!Matchmaking)
		{
			return;
		}
		if (CurrentAnim != string.Empty && Rival.Character.GetComponent<Animation>()[CurrentAnim].time >= Rival.Character.GetComponent<Animation>()[CurrentAnim].length)
		{
			Rival.Character.GetComponent<Animation>().Play(Rival.IdleAnim);
		}
		if (Phase == 1)
		{
			Panel.alpha = Mathf.MoveTowards(Panel.alpha, 0f, Time.deltaTime);
			Timer += Time.deltaTime;
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(54f, 1.25f, -45.25f), Timer * 0.02f);
			Camera.main.transform.eulerAngles = Vector3.Lerp(Camera.main.transform.eulerAngles, new Vector3(0f, 45f, 0f), Timer * 0.02f);
			if (Timer > 5f)
			{
				Suitor.Character.GetComponent<Animation>().Play("insertEarpiece_00");
				Suitor.Character.GetComponent<Animation>()["insertEarpiece_00"].time = 0f;
				Suitor.Character.GetComponent<Animation>().Play("insertEarpiece_00");
				Suitor.Earpiece.SetActive(true);
				Camera.main.transform.position = new Vector3(45.5f, 1.25f, -44.5f);
				Camera.main.transform.eulerAngles = new Vector3(0f, -45f, 0f);
				Rotation = -45f;
				Timer = 0f;
				Phase++;
			}
		}
		else if (Phase == 2)
		{
			Timer += Time.deltaTime;
			if (Timer > 4f)
			{
				Suitor.Earpiece.transform.parent = Suitor.Head;
				Suitor.Earpiece.transform.localPosition = new Vector3(0f, -1.12f, 1.14f);
				Suitor.Earpiece.transform.localEulerAngles = new Vector3(45f, -180f, 0f);
				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(45.11f, 1.375f, -44f), (Timer - 4f) * 0.02f);
				Rotation = Mathf.Lerp(Rotation, 90f, (Timer - 4f) * 0.02f);
				Camera.main.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Rotation, Camera.main.transform.eulerAngles.z);
				if (Rotation > 89.9f)
				{
					Rival.Character.GetComponent<Animation>()["f02_turnAround_00"].time = 0f;
					Rival.Character.GetComponent<Animation>().CrossFade("f02_turnAround_00");
					AffectionBar.localScale = new Vector3(Affection / 100f, AffectionBar.localScale.y, AffectionBar.localScale.z);
					DialogueLabel.text = Greetings[AffectionLevel];
					CalculateMultiplier();
					DatingSimHUD.gameObject.SetActive(true);
					Timer = 0f;
					Phase++;
				}
			}
		}
		else if (Phase == 3)
		{
			DatingSimHUD.alpha = Mathf.MoveTowards(DatingSimHUD.alpha, 1f, Time.deltaTime);
			if (Rival.Character.GetComponent<Animation>()["f02_turnAround_00"].time >= Rival.Character.GetComponent<Animation>()["f02_turnAround_00"].length)
			{
				Rival.transform.eulerAngles = new Vector3(Rival.transform.eulerAngles.x, -90f, Rival.transform.eulerAngles.z);
				Rival.Character.GetComponent<Animation>().Play("f02_turnAround_00");
				Rival.Character.GetComponent<Animation>()["f02_turnAround_00"].time = 0f;
				Rival.Character.GetComponent<Animation>()["f02_turnAround_00"].speed = 0f;
				Rival.Character.GetComponent<Animation>().Play("f02_turnAround_00");
				Rival.Character.GetComponent<Animation>().CrossFade(Rival.IdleAnim);
				PromptBar.ClearButtons();
				PromptBar.Label[0].text = "Confirm";
				PromptBar.Label[1].text = "Back";
				PromptBar.Label[4].text = "Select";
				PromptBar.UpdateButtons();
				PromptBar.Show = true;
				Phase++;
			}
		}
		else if (Phase == 4)
		{
			if (AffectionGrow)
			{
				Affection = Mathf.MoveTowards(Affection, 100f, Time.deltaTime * 10f);
				CalculateAffection();
			}
			Rival.Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", Affection * 0.01f);
			Rival.CharacterAnimation["f02_smile_00"].weight = Affection * 0.01f;
			Highlight.localPosition = new Vector3(Highlight.localPosition.x, Mathf.Lerp(Highlight.localPosition.y, HighlightTarget, Time.deltaTime * 10f), Highlight.localPosition.z);
			for (int i = 1; i < Options.Length; i++)
			{
				Transform transform = Options[i];
				transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, (i != Selected) ? 800f : 750f, Time.deltaTime * 10f), transform.localPosition.y, transform.localPosition.z);
			}
			AffectionBar.localScale = new Vector3(Mathf.Lerp(AffectionBar.localScale.x, Affection / 100f, Time.deltaTime * 10f), AffectionBar.localScale.y, AffectionBar.localScale.z);
			if (!SelectingTopic && !Complimenting && !ShowingOff && !GivingGift)
			{
				Topics.localScale = Vector3.Lerp(Topics.localScale, Vector3.zero, Time.deltaTime * 10f);
				ComplimentSet.localScale = Vector3.Lerp(ComplimentSet.localScale, Vector3.zero, Time.deltaTime * 10f);
				ShowOff.localScale = Vector3.Lerp(ShowOff.localScale, Vector3.zero, Time.deltaTime * 10f);
				GiveGift.localScale = Vector3.Lerp(GiveGift.localScale, Vector3.zero, Time.deltaTime * 10f);
				if (InputManager.TappedUp)
				{
					Selected--;
					UpdateHighlight();
				}
				if (InputManager.TappedDown)
				{
					Selected++;
					UpdateHighlight();
				}
				if (Input.GetButtonDown("A") && Labels[Selected].color.a == 1f)
				{
					if (Selected == 1)
					{
						SelectingTopic = true;
						Negative = true;
					}
					else if (Selected == 2)
					{
						SelectingTopic = true;
						Negative = false;
					}
					else if (Selected == 3)
					{
						Complimenting = true;
					}
					else if (Selected == 4)
					{
						ShowingOff = true;
					}
					else if (Selected == 5)
					{
						GivingGift = true;
					}
					else if (Selected == 6)
					{
						PromptBar.ClearButtons();
						PromptBar.Label[0].text = "Confirm";
						PromptBar.UpdateButtons();
						CalculateAffection();
						DialogueLabel.text = Farewells[AffectionLevel];
						Phase++;
					}
				}
			}
			else if (SelectingTopic)
			{
				Topics.localScale = Vector3.Lerp(Topics.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
				if (InputManager.TappedUp)
				{
					Row--;
					UpdateTopicHighlight();
				}
				else if (InputManager.TappedDown)
				{
					Row++;
					UpdateTopicHighlight();
				}
				if (InputManager.TappedLeft)
				{
					Column--;
					UpdateTopicHighlight();
				}
				else if (InputManager.TappedRight)
				{
					Column++;
					UpdateTopicHighlight();
				}
				if (Input.GetButtonDown("A") && TopicIcons[TopicSelected].color.a == 1f)
				{
					SelectingTopic = false;
					UISprite uISprite = TopicIcons[TopicSelected];
					uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 0.5f);
					DatingGlobals.SetTopicDiscussed(TopicSelected, true);
					DetermineOpinion();
					if (!ConversationGlobals.GetTopicLearnedByStudent(Opinion, 30))
					{
						ConversationGlobals.SetTopicLearnedByStudent(Opinion, 30, true);
					}
					if (Negative)
					{
						UILabel uILabel = Labels[1];
						uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0.5f);
						if (Opinion == 2)
						{
							DialogueLabel.text = "Hey! Just so you know, I take offense to that...";
							Rival.Character.GetComponent<Animation>().CrossFade("f02_refuse_00");
							CurrentAnim = "f02_refuse_00";
							Affection -= 1f;
							CalculateAffection();
						}
						else if (Opinion == 1)
						{
							DialogueLabel.text = Negatives[AffectionLevel];
							Rival.Character.GetComponent<Animation>().CrossFade("f02_lookdown_00");
							CurrentAnim = "f02_lookdown_00";
							Affection += Multiplier;
							CalculateAffection();
						}
						else if (Opinion == 0)
						{
							DialogueLabel.text = "Um...okay.";
						}
					}
					else
					{
						UILabel uILabel2 = Labels[2];
						uILabel2.color = new Color(uILabel2.color.r, uILabel2.color.g, uILabel2.color.b, 0.5f);
						if (Opinion == 2)
						{
							DialogueLabel.text = Positives[AffectionLevel];
							Rival.Character.GetComponent<Animation>().CrossFade("f02_lookdown_00");
							CurrentAnim = "f02_lookdown_00";
							Affection += Multiplier;
							CalculateAffection();
						}
						else if (Opinion == 1)
						{
							DialogueLabel.text = "To be honest with you, I strongly disagree...";
							Rival.Character.GetComponent<Animation>().CrossFade("f02_refuse_00");
							CurrentAnim = "f02_refuse_00";
							Affection -= 1f;
							CalculateAffection();
						}
						else if (Opinion == 0)
						{
							DialogueLabel.text = "Um...all right.";
						}
					}
					if (Affection > 100f)
					{
						Affection = 100f;
					}
					else if (Affection < 0f)
					{
						Affection = 0f;
					}
				}
				if (Input.GetButtonDown("B"))
				{
					SelectingTopic = false;
				}
			}
			else if (Complimenting)
			{
				ComplimentSet.localScale = Vector3.Lerp(ComplimentSet.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
				if (InputManager.TappedUp)
				{
					Line--;
					UpdateComplimentHighlight();
				}
				else if (InputManager.TappedDown)
				{
					Line++;
					UpdateComplimentHighlight();
				}
				if (InputManager.TappedLeft)
				{
					Side--;
					UpdateComplimentHighlight();
				}
				else if (InputManager.TappedRight)
				{
					Side++;
					UpdateComplimentHighlight();
				}
				if (Input.GetButtonDown("A") && ComplimentLabels[ComplimentSelected].color.a == 1f)
				{
					UILabel uILabel3 = Labels[3];
					uILabel3.color = new Color(uILabel3.color.r, uILabel3.color.g, uILabel3.color.b, 0.5f);
					Complimenting = false;
					DialogueLabel.text = Compliments[ComplimentSelected];
					DatingGlobals.SetComplimentGiven(ComplimentSelected, true);
					if (ComplimentSelected == 1 || ComplimentSelected == 4 || ComplimentSelected == 5 || ComplimentSelected == 8 || ComplimentSelected == 9)
					{
						Rival.Character.GetComponent<Animation>().CrossFade("f02_lookdown_00");
						CurrentAnim = "f02_lookdown_00";
						Affection += Multiplier;
						CalculateAffection();
					}
					else
					{
						Rival.Character.GetComponent<Animation>().CrossFade("f02_refuse_00");
						CurrentAnim = "f02_refuse_00";
						Affection -= 1f;
						CalculateAffection();
					}
					if (Affection > 100f)
					{
						Affection = 100f;
					}
					else if (Affection < 0f)
					{
						Affection = 0f;
					}
				}
				if (Input.GetButtonDown("B"))
				{
					Complimenting = false;
				}
			}
			else if (ShowingOff)
			{
				ShowOff.localScale = Vector3.Lerp(ShowOff.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
				if (InputManager.TappedUp)
				{
					TraitSelected--;
					UpdateTraitHighlight();
				}
				else if (InputManager.TappedDown)
				{
					TraitSelected++;
					UpdateTraitHighlight();
				}
				if (Input.GetButtonDown("A"))
				{
					UILabel uILabel4 = Labels[4];
					uILabel4.color = new Color(uILabel4.color.r, uILabel4.color.g, uILabel4.color.b, 0.5f);
					ShowingOff = false;
					if (TraitSelected == 2)
					{
						Debug.Log("Wisdom trait is " + DatingGlobals.GetSuitorTrait(2) + ". Wisdom Demonstrated is " + DatingGlobals.GetTraitDemonstrated(2) + ".");
						if (DatingGlobals.GetSuitorTrait(2) > DatingGlobals.GetTraitDemonstrated(2))
						{
							DatingGlobals.SetTraitDemonstrated(2, DatingGlobals.GetTraitDemonstrated(2) + 1);
							DialogueLabel.text = ShowOffs[AffectionLevel];
							Rival.Character.GetComponent<Animation>().CrossFade("f02_lookdown_00");
							CurrentAnim = "f02_lookdown_00";
							Affection += Multiplier;
							CalculateAffection();
						}
						else if (DatingGlobals.GetSuitorTrait(2) == 0)
						{
							DialogueLabel.text = "Uh...that doesn't sound correct...";
						}
						else if (DatingGlobals.GetSuitorTrait(2) == DatingGlobals.GetTraitDemonstrated(2))
						{
							DialogueLabel.text = "Uh...you already told me about that...";
						}
					}
					else
					{
						DialogueLabel.text = "Um...well...that sort of thing doesn't really matter to me...";
					}
					if (Affection > 100f)
					{
						Affection = 100f;
					}
					else if (Affection < 0f)
					{
						Affection = 0f;
					}
				}
				if (Input.GetButtonDown("B"))
				{
					ShowingOff = false;
				}
			}
			else
			{
				if (!GivingGift)
				{
					return;
				}
				GiveGift.localScale = Vector3.Lerp(GiveGift.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
				if (InputManager.TappedUp)
				{
					GiftRow--;
					UpdateGiftHighlight();
				}
				else if (InputManager.TappedDown)
				{
					GiftRow++;
					UpdateGiftHighlight();
				}
				if (InputManager.TappedLeft)
				{
					GiftColumn--;
					UpdateGiftHighlight();
				}
				else if (InputManager.TappedRight)
				{
					GiftColumn++;
					UpdateGiftHighlight();
				}
				if (Input.GetButtonDown("A"))
				{
					if (GiftSelected == 1 && RoseIcon.enabled)
					{
						UILabel uILabel5 = Labels[5];
						uILabel5.color = new Color(uILabel5.color.r, uILabel5.color.g, uILabel5.color.b, 0.5f);
						GivingGift = false;
						DialogueLabel.text = GiveGifts[AffectionLevel];
						Rival.Character.GetComponent<Animation>().CrossFade("f02_lookdown_00");
						CurrentAnim = "f02_lookdown_00";
						Affection += Multiplier;
						CalculateAffection();
					}
					if (Affection > 100f)
					{
						Affection = 100f;
					}
					else if (Affection < 0f)
					{
						Affection = 0f;
					}
				}
				if (Input.GetButtonDown("B"))
				{
					GivingGift = false;
				}
			}
		}
		else if (Phase == 5)
		{
			Speed += Time.deltaTime * 100f;
			AffectionSet.localPosition = new Vector3(AffectionSet.localPosition.x, AffectionSet.localPosition.y + Speed, AffectionSet.localPosition.z);
			OptionSet.localPosition = new Vector3(OptionSet.localPosition.x + Speed, OptionSet.localPosition.y, OptionSet.localPosition.z);
			if (Speed > 100f && Input.GetButtonDown("A"))
			{
				Phase++;
			}
		}
		else if (Phase == 6)
		{
			DatingSimHUD.alpha = Mathf.MoveTowards(DatingSimHUD.alpha, 0f, Time.deltaTime);
			if (DatingSimHUD.alpha == 0f)
			{
				DatingSimHUD.gameObject.SetActive(false);
				Phase++;
			}
		}
		else if (Phase == 7)
		{
			if (Panel.alpha == 0f)
			{
				LoveManager.RivalWaiting = false;
				LoveManager.Courted = true;
				Suitor.enabled = true;
				Rival.enabled = true;
				Suitor.CurrentDestination = Suitor.Destinations[Suitor.Phase];
				Suitor.Pathfinding.target = Suitor.Destinations[Suitor.Phase];
				Suitor.Prompt.Label[0].text = "     Talk";
				Suitor.Pathfinding.canSearch = true;
				Suitor.Pathfinding.canMove = true;
				Suitor.Pushable = false;
				Suitor.Meeting = false;
				Suitor.Routine = true;
				Suitor.MeetTimer = 0f;
				Rival.Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", 0f);
				Rival.CurrentDestination = Rival.Destinations[Rival.Phase];
				Rival.Pathfinding.target = Rival.Destinations[Rival.Phase];
				Rival.CharacterAnimation["f02_smile_00"].weight = 0f;
				Rival.Prompt.Label[0].text = "     Talk";
				Rival.Pathfinding.canSearch = true;
				Rival.Pathfinding.canMove = true;
				Rival.Pushable = false;
				Rival.Meeting = false;
				Rival.Routine = true;
				Rival.MeetTimer = 0f;
				StudentManager.Clock.StopTime = false;
				Yandere.RPGCamera.enabled = true;
				Suitor.Earpiece.SetActive(false);
				HeartbeatCamera.SetActive(true);
				Yandere.Headset.SetActive(false);
				DatingGlobals.Affection = Affection;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
			}
			else if (Panel.alpha == 1f)
			{
				Matchmaking = false;
				Yandere.CanMove = true;
				base.gameObject.SetActive(false);
			}
			Panel.alpha = Mathf.MoveTowards(Panel.alpha, 1f, Time.deltaTime);
		}
	}

	private void LateUpdate()
	{
		if (Phase != 4)
		{
		}
	}

	private void CalculateMultiplier()
	{
		Multiplier = 5;
		if (!Suitor.Cosmetic.Eyewear[6].activeInHierarchy)
		{
			MultiplierIcons[1].mainTexture = X;
			Multiplier--;
		}
		if (!Suitor.Cosmetic.MaleAccessories[3].activeInHierarchy)
		{
			MultiplierIcons[2].mainTexture = X;
			Multiplier--;
		}
		if (!Suitor.Cosmetic.MaleHair[22].activeInHierarchy)
		{
			MultiplierIcons[3].mainTexture = X;
			Multiplier--;
		}
		if (Suitor.Cosmetic.HairColor != "Purple")
		{
			MultiplierIcons[4].mainTexture = X;
			Multiplier--;
		}
		if (PlayerGlobals.PantiesEquipped == 2)
		{
			PantyIcon.SetActive(true);
			Multiplier++;
		}
		else
		{
			PantyIcon.SetActive(false);
		}
		if (PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus > 0)
		{
			SeductionLabel.text = (PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus).ToString();
			Multiplier += PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus;
			SeductionIcon.SetActive(true);
		}
		else
		{
			SeductionIcon.SetActive(false);
		}
		MultiplierLabel.text = "Multiplier: " + Multiplier + "x";
	}

	private void UpdateHighlight()
	{
		if (Selected < 1)
		{
			Selected = 6;
		}
		else if (Selected > 6)
		{
			Selected = 1;
		}
		HighlightTarget = 450f - 100f * (float)Selected;
	}

	private void UpdateTopicHighlight()
	{
		if (Row < 1)
		{
			Row = 5;
		}
		else if (Row > 5)
		{
			Row = 1;
		}
		if (Column < 1)
		{
			Column = 5;
		}
		else if (Column > 5)
		{
			Column = 1;
		}
		TopicHighlight.localPosition = new Vector3(-375 + 125 * Column, 375 - 125 * Row, TopicHighlight.localPosition.z);
		TopicSelected = (Row - 1) * 5 + Column;
		TopicNameLabel.text = ((!ConversationGlobals.GetTopicDiscovered(TopicSelected)) ? "??????????" : TopicNames[TopicSelected]);
	}

	private void DetermineOpinion()
	{
		int[] topics = JSON.Topics[30].Topics;
		Opinion = topics[TopicSelected];
	}

	private void UpdateTopics()
	{
		for (int i = 1; i < TopicIcons.Length; i++)
		{
			UISprite uISprite = TopicIcons[i];
			if (!ConversationGlobals.GetTopicDiscovered(i))
			{
				uISprite.spriteName = 0.ToString();
				uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 0.5f);
			}
			else
			{
				uISprite.spriteName = i.ToString();
			}
		}
		for (int j = 1; j <= 25; j++)
		{
			UISprite uISprite2 = OpinionIcons[j];
			if (!ConversationGlobals.GetTopicLearnedByStudent(j, 30))
			{
				uISprite2.spriteName = "Unknown";
				continue;
			}
			int[] topics = JSON.Topics[30].Topics;
			uISprite2.spriteName = OpinionSpriteNames[topics[j]];
		}
	}

	private void UpdateComplimentHighlight()
	{
		for (int i = 1; i < TopicIcons.Length; i++)
		{
			ComplimentBGs[ComplimentSelected].color = OriginalColor;
		}
		if (Line < 1)
		{
			Line = 5;
		}
		else if (Line > 5)
		{
			Line = 1;
		}
		if (Side < 1)
		{
			Side = 2;
		}
		else if (Side > 2)
		{
			Side = 1;
		}
		ComplimentSelected = (Line - 1) * 2 + Side;
		ComplimentBGs[ComplimentSelected].color = Color.white;
	}

	private void UpdateTraitHighlight()
	{
		if (TraitSelected < 1)
		{
			TraitSelected = 3;
		}
		else if (TraitSelected > 3)
		{
			TraitSelected = 1;
		}
		for (int i = 1; i < TraitBGs.Length; i++)
		{
			TraitBGs[i].color = OriginalColor;
		}
		TraitBGs[TraitSelected].color = Color.white;
	}

	private void UpdateGiftHighlight()
	{
		for (int i = 1; i < GiftBGs.Length; i++)
		{
			GiftBGs[i].color = OriginalColor;
		}
		if (GiftRow < 1)
		{
			GiftRow = 2;
		}
		else if (GiftRow > 2)
		{
			GiftRow = 1;
		}
		if (GiftColumn < 1)
		{
			GiftColumn = 2;
		}
		else if (GiftColumn > 2)
		{
			GiftColumn = 1;
		}
		GiftSelected = (GiftRow - 1) * 2 + GiftColumn;
		GiftBGs[GiftSelected].color = Color.white;
	}
}
