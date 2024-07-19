using UnityEngine;

public class PromptScript : MonoBehaviour
{
	public PauseScreenScript PauseScreen;

	public StudentScript MyStudent;

	public YandereScript Yandere;

	public GameObject[] ButtonObject;

	public GameObject SpeakerObject;

	public GameObject CircleObject;

	public GameObject LabelObject;

	public PromptParentScript PromptParent;

	public Collider MyCollider;

	public Camera UICamera;

	public bool[] AcceptingInput;

	public bool[] ButtonActive;

	public bool[] HideButton;

	public UISprite[] Button;

	public UISprite[] Circle;

	public UILabel[] Label;

	public UISprite Speaker;

	public UISprite Square;

	public float[] OffsetX;

	public float[] OffsetY;

	public float[] OffsetZ;

	public string[] Text;

	public PromptOwnerType OwnerType;

	public bool DisableAtStart;

	public bool Suspicious;

	public bool Debugging;

	public bool SquareSet;

	public bool Carried;

	public bool InSight;

	public bool NoCheck;

	public bool Attack;

	public bool InView;

	public bool Weapon;

	public bool Noisy;

	public bool Local = true;

	public float RelativePosition;

	public float MaximumDistance = 5f;

	public float MinimumDistance;

	public float DistanceSqr;

	public float Height;

	public int ButtonHeld;

	public int BloodMask;

	public int Priority;

	public int ID;

	public GameObject YandereObject;

	public Transform RaycastTarget;

	public float MinimumDistanceSqr;

	public float MaximumDistanceSqr;

	public float Timer;

	public bool Student;

	public bool Door;

	public bool Hidden;

	private void Awake()
	{
		MinimumDistanceSqr = MinimumDistance;
		MaximumDistanceSqr = MaximumDistance;
		DistanceSqr = float.PositiveInfinity;
		OwnerType = DecideOwnerType();
		if (RaycastTarget == null)
		{
			RaycastTarget = base.transform;
		}
		if (OffsetZ.Length == 0)
		{
			OffsetZ = new float[4];
		}
		if (Yandere == null)
		{
			YandereObject = GameObject.Find("YandereChan");
			if (YandereObject != null)
			{
				Yandere = YandereObject.GetComponent<YandereScript>();
			}
		}
		if (!(Yandere != null))
		{
			return;
		}
		PauseScreen = GameObject.Find("PauseScreen").GetComponent<PauseScreenScript>();
		PromptParent = GameObject.Find("PromptParent").GetComponent<PromptParentScript>();
		UICamera = GameObject.Find("UI Camera").GetComponent<Camera>();
		if (Noisy)
		{
			Speaker = Object.Instantiate(SpeakerObject, base.transform.position, Quaternion.identity).GetComponent<UISprite>();
			Speaker.transform.parent = PromptParent.transform;
			Speaker.transform.localScale = new Vector3(1f, 1f, 1f);
			Speaker.transform.localEulerAngles = Vector3.zero;
			Speaker.enabled = false;
		}
		Square = Object.Instantiate(PromptParent.SquareObject, base.transform.position, Quaternion.identity).GetComponent<UISprite>();
		Square.transform.parent = PromptParent.transform;
		Square.transform.localScale = new Vector3(1f, 1f, 1f);
		Square.transform.localEulerAngles = Vector3.zero;
		Color color = Square.color;
		color.a = 0f;
		Square.color = color;
		Square.enabled = false;
		for (ID = 0; ID < 4; ID++)
		{
			if (ButtonActive[ID])
			{
				Button[ID] = Object.Instantiate(ButtonObject[ID], base.transform.position, Quaternion.identity).GetComponent<UISprite>();
				UISprite uISprite = Button[ID];
				uISprite.transform.parent = PromptParent.transform;
				uISprite.transform.localScale = new Vector3(1f, 1f, 1f);
				uISprite.transform.localEulerAngles = Vector3.zero;
				uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 0f);
				uISprite.enabled = false;
				Circle[ID] = Object.Instantiate(CircleObject, base.transform.position, Quaternion.identity).GetComponent<UISprite>();
				UISprite uISprite2 = Circle[ID];
				uISprite2.transform.parent = PromptParent.transform;
				uISprite2.transform.localScale = new Vector3(1f, 1f, 1f);
				uISprite2.transform.localEulerAngles = Vector3.zero;
				uISprite2.color = new Color(uISprite2.color.r, uISprite2.color.g, uISprite2.color.b, 0f);
				uISprite2.enabled = false;
				Label[ID] = Object.Instantiate(LabelObject, base.transform.position, Quaternion.identity).GetComponent<UILabel>();
				UILabel uILabel = Label[ID];
				uILabel.transform.parent = PromptParent.transform;
				uILabel.transform.localScale = new Vector3(1f, 1f, 1f);
				uILabel.transform.localEulerAngles = Vector3.zero;
				uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0f);
				uILabel.enabled = false;
				if (Suspicious)
				{
					uILabel.color = new Color(1f, 0f, 0f, 0f);
				}
				uILabel.text = "     " + Text[ID];
			}
			AcceptingInput[ID] = true;
		}
		BloodMask = 2;
		BloodMask |= 512;
		BloodMask |= 8192;
		BloodMask |= 16384;
		BloodMask |= 65536;
		BloodMask |= 2097152;
		BloodMask = ~BloodMask;
	}

	private void Start()
	{
		if (DisableAtStart)
		{
			Hide();
			base.enabled = false;
		}
	}

	private PromptOwnerType DecideOwnerType()
	{
		if (GetComponent<DoorScript>() != null)
		{
			return PromptOwnerType.Door;
		}
		return PromptOwnerType.Unknown;
	}

	private bool AllowedWhenCrouching(PromptOwnerType ownerType)
	{
		return ownerType == PromptOwnerType.Door;
	}

	private bool AllowedWhenCrawling(PromptOwnerType ownerType)
	{
		return false;
	}

	private void Update()
	{
		if (!PauseScreen.Show)
		{
			if (InView)
			{
				if (MyStudent == null)
				{
					Vector3 vector = new Vector3(base.transform.position.x, Yandere.transform.position.y, base.transform.position.z);
					DistanceSqr = (vector - Yandere.transform.position).sqrMagnitude;
				}
				else
				{
					DistanceSqr = MyStudent.DistanceToPlayer;
				}
				if (DistanceSqr < MaximumDistanceSqr)
				{
					NoCheck = true;
					bool flag = Yandere.Stance.Current == StanceType.Crouching;
					bool flag2 = Yandere.Stance.Current == StanceType.Crawling;
					if (Yandere.CanMove && (!flag || AllowedWhenCrouching(OwnerType)) && (!flag2 || AllowedWhenCrawling(OwnerType)) && !Yandere.Aiming && !Yandere.Mopping && !Yandere.NearSenpai)
					{
						RaycastHit hitInfo;
						if (Physics.Linecast(Yandere.Eyes.position + Vector3.down * Height, RaycastTarget.position, out hitInfo, BloodMask))
						{
							InSight = hitInfo.collider == MyCollider;
						}
						if (Carried || InSight)
						{
							SquareSet = false;
							Hidden = false;
							Vector2 vector2 = Vector2.zero;
							for (ID = 0; ID < 4; ID++)
							{
								if (ButtonActive[ID])
								{
									float num = Vector3.Angle(Yandere.MainCamera.transform.forward, Yandere.MainCamera.transform.position - base.transform.position);
									if (num > 90f)
									{
										if (Local)
										{
											Vector2 vector3 = Camera.main.WorldToScreenPoint(base.transform.position + base.transform.right * OffsetX[ID] + base.transform.up * OffsetY[ID] + base.transform.forward * OffsetZ[ID]);
											Button[ID].transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector3.x, vector3.y, 1f));
											Circle[ID].transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector3.x, vector3.y, 1f));
											if (!SquareSet)
											{
												Square.transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector3.x, vector3.y, 1f));
												SquareSet = true;
											}
											Vector2 vector4 = Camera.main.WorldToScreenPoint(base.transform.position + base.transform.right * OffsetX[ID] + base.transform.up * OffsetY[ID] + base.transform.forward * OffsetZ[ID]);
											Label[ID].transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector4.x + OffsetX[ID], vector4.y, 1f));
											RelativePosition = vector3.x;
										}
										else
										{
											vector2 = Camera.main.WorldToScreenPoint(base.transform.position + new Vector3(OffsetX[ID], OffsetY[ID], OffsetZ[ID]));
											Button[ID].transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, 1f));
											Circle[ID].transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, 1f));
											if (!SquareSet)
											{
												Square.transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, 1f));
												SquareSet = true;
											}
											Vector2 vector5 = Camera.main.WorldToScreenPoint(base.transform.position + new Vector3(OffsetX[ID], OffsetY[ID], OffsetZ[ID]));
											Label[ID].transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector5.x + OffsetX[ID], vector5.y, 1f));
											RelativePosition = vector2.x;
										}
										if (!HideButton[ID])
										{
											Square.enabled = true;
											Square.color = new Color(Square.color.r, Square.color.g, Square.color.b, 1f);
										}
									}
								}
							}
							if (Noisy)
							{
								Speaker.transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y + 40f, 1f));
							}
							if (DistanceSqr < MinimumDistanceSqr)
							{
								if (Yandere.NearestPrompt == null)
								{
									Yandere.NearestPrompt = this;
								}
								else if (Mathf.Abs(RelativePosition - (float)Screen.width * 0.5f) < Mathf.Abs(Yandere.NearestPrompt.RelativePosition - (float)Screen.width * 0.5f))
								{
									Yandere.NearestPrompt = this;
								}
								if (Yandere.NearestPrompt == this)
								{
									Square.enabled = false;
									Square.color = new Color(Square.color.r, Square.color.g, Square.color.b, 0f);
									for (ID = 0; ID < 4; ID++)
									{
										if (ButtonActive[ID])
										{
											if (!Button[ID].enabled)
											{
												Button[ID].enabled = true;
												Circle[ID].enabled = true;
												Label[ID].enabled = true;
											}
											Button[ID].color = new Color(1f, 1f, 1f, 1f);
											Circle[ID].color = new Color(0.5f, 0.5f, 0.5f, 1f);
											Color color = Label[ID].color;
											color.a = 1f;
											Label[ID].color = color;
											if (Speaker != null)
											{
												Speaker.enabled = true;
												Color color2 = Speaker.color;
												color2.a = 1f;
												Speaker.color = color2;
											}
										}
									}
									if (Input.GetButton("A"))
									{
										ButtonHeld = 1;
									}
									else if (Input.GetButton("B"))
									{
										ButtonHeld = 2;
									}
									else if (Input.GetButton("X"))
									{
										ButtonHeld = 3;
									}
									else if (Input.GetButton("Y"))
									{
										ButtonHeld = 4;
									}
									else
									{
										ButtonHeld = 0;
									}
									if (ButtonHeld > 0)
									{
										for (ID = 0; ID < 4; ID++)
										{
											if (((ButtonActive[ID] && ID != ButtonHeld - 1) || HideButton[ID]) && Circle[ID] != null)
											{
												Circle[ID].fillAmount = 1f;
											}
										}
										if (ButtonActive[ButtonHeld - 1] && !HideButton[ButtonHeld - 1] && AcceptingInput[ButtonHeld - 1] && !Yandere.Attacking)
										{
											Circle[ButtonHeld - 1].color = new Color(1f, 1f, 1f, 1f);
											if (!Attack)
											{
												Circle[ButtonHeld - 1].fillAmount -= Time.deltaTime * 2f;
											}
											else
											{
												Circle[ButtonHeld - 1].fillAmount = 0f;
											}
											ID = 0;
										}
									}
									else
									{
										for (ID = 0; ID < 4; ID++)
										{
											if (ButtonActive[ID])
											{
												Circle[ID].fillAmount = 1f;
											}
										}
									}
								}
								else
								{
									Square.color = new Color(Square.color.r, Square.color.g, Square.color.b, 1f);
									for (ID = 0; ID < 4; ID++)
									{
										if (ButtonActive[ID])
										{
											UISprite uISprite = Button[ID];
											UISprite uISprite2 = Circle[ID];
											UILabel uILabel = Label[ID];
											uISprite.enabled = false;
											uISprite2.enabled = false;
											uILabel.enabled = false;
											Color color3 = uISprite.color;
											Color color4 = uISprite2.color;
											Color color5 = uILabel.color;
											color3.a = 0f;
											color4.a = 0f;
											color5.a = 0f;
											uISprite.color = color3;
											uISprite2.color = color4;
											uILabel.color = color5;
										}
									}
									if (Speaker != null)
									{
										Speaker.enabled = false;
										Color color6 = Speaker.color;
										color6.a = 0f;
										Speaker.color = color6;
									}
								}
							}
							else
							{
								if (Yandere.NearestPrompt == this)
								{
									Yandere.NearestPrompt = null;
								}
								Square.color = new Color(Square.color.r, Square.color.g, Square.color.b, 1f);
								for (ID = 0; ID < 4; ID++)
								{
									if (ButtonActive[ID])
									{
										UISprite uISprite3 = Button[ID];
										UISprite uISprite4 = Circle[ID];
										UILabel uILabel2 = Label[ID];
										uISprite4.fillAmount = 1f;
										uISprite3.enabled = false;
										uISprite4.enabled = false;
										uILabel2.enabled = false;
										Color color7 = uISprite3.color;
										Color color8 = uISprite4.color;
										Color color9 = uILabel2.color;
										color7.a = 0f;
										color8.a = 0f;
										color9.a = 0f;
										uISprite3.color = color7;
										uISprite4.color = color8;
										uILabel2.color = color9;
									}
								}
								if (Speaker != null)
								{
									Speaker.enabled = false;
									Color color10 = Speaker.color;
									color10.a = 0f;
									Speaker.color = color10;
								}
							}
							Color color11 = Square.color;
							color11.a = 1f;
							Square.color = color11;
							for (ID = 0; ID < 4; ID++)
							{
								if (ButtonActive[ID] && HideButton[ID])
								{
									UISprite uISprite5 = Button[ID];
									UISprite uISprite6 = Circle[ID];
									UILabel uILabel3 = Label[ID];
									uISprite5.enabled = false;
									uISprite6.enabled = false;
									uILabel3.enabled = false;
									Color color12 = uISprite5.color;
									Color color13 = uISprite6.color;
									Color color14 = uILabel3.color;
									color12.a = 0f;
									color13.a = 0f;
									color14.a = 0f;
									uISprite5.color = color12;
									uISprite6.color = color13;
									uILabel3.color = color14;
									if (Speaker != null)
									{
										Speaker.enabled = false;
										Color color15 = Speaker.color;
										color15.a = 0f;
										Speaker.color = color15;
									}
								}
							}
						}
						else
						{
							Hide();
						}
					}
					else
					{
						Hide();
					}
				}
				else
				{
					if (Debugging)
					{
						Debug.Log("Yandere-chan is too far away.");
					}
					Hide();
				}
			}
			else
			{
				DistanceSqr = float.PositiveInfinity;
				Hide();
			}
		}
		else
		{
			Hide();
		}
	}

	private void OnBecameVisible()
	{
		InView = true;
	}

	private void OnBecameInvisible()
	{
		InView = false;
		Hide();
	}

	public void Hide()
	{
		if (Hidden)
		{
			return;
		}
		NoCheck = false;
		Hidden = true;
		if (!(Yandere != null))
		{
			return;
		}
		if (Yandere.NearestPrompt == this)
		{
			Yandere.NearestPrompt = null;
		}
		if (Square.enabled)
		{
			Square.enabled = false;
			Square.color = new Color(Square.color.r, Square.color.g, Square.color.b, 0f);
		}
		for (ID = 0; ID < 4; ID++)
		{
			if (ButtonActive[ID])
			{
				UISprite uISprite = Button[ID];
				if (uISprite.enabled)
				{
					UISprite uISprite2 = Circle[ID];
					UILabel uILabel = Label[ID];
					uISprite2.fillAmount = 1f;
					uISprite.enabled = false;
					uISprite2.enabled = false;
					uILabel.enabled = false;
					uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 0f);
					uISprite2.color = new Color(uISprite2.color.r, uISprite2.color.g, uISprite2.color.b, 0f);
					uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0f);
				}
			}
		}
		if (Speaker != null)
		{
			Speaker.enabled = false;
			Speaker.color = new Color(Speaker.color.r, Speaker.color.g, Speaker.color.b, 0f);
		}
	}
}
