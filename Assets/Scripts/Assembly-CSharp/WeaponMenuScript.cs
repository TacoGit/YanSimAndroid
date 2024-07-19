using UnityEngine;

public class WeaponMenuScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public InputDeviceScript InputDevice;

	public PauseScreenScript PauseScreen;

	public YandereScript Yandere;

	public InputManagerScript IM;

	public UIPanel KeyboardPanel;

	public UIPanel Panel;

	public Transform KeyboardMenu;

	public bool KeyboardShow;

	public bool Released = true;

	public bool Show;

	public UISprite[] BG;

	public UISprite[] Outline;

	public UISprite[] Item;

	public UISprite[] KeyboardBG;

	public UISprite[] KeyboardOutline;

	public UISprite[] KeyboardItem;

	public int Selected = 1;

	public Color OriginalColor;

	public Transform Button;

	public float Timer;

	private void Start()
	{
		KeyboardMenu.localScale = Vector3.zero;
		base.transform.localScale = Vector3.zero;
		OriginalColor = BG[1].color;
		UpdateSprites();
	}

	private void Update()
	{
		if (!PauseScreen.Show)
		{
			if ((Yandere.CanMove && !Yandere.Aiming) || (Yandere.Chased && !Yandere.Sprayed))
			{
				if ((IM.DPadUp && IM.TappedUp) || (IM.DPadDown && IM.TappedDown) || (IM.DPadLeft && IM.TappedLeft) || (IM.DPadRight && IM.TappedRight))
				{
					Yandere.EmptyHands();
					if (IM.DPadLeft || IM.DPadRight || IM.DPadUp || Yandere.Mask != null)
					{
						KeyboardShow = false;
						Panel.enabled = true;
						Show = true;
					}
					if (IM.DPadLeft)
					{
						Button.localPosition = new Vector3(-340f, 0f, 0f);
						Selected = 1;
					}
					else if (IM.DPadRight)
					{
						Button.localPosition = new Vector3(340f, 0f, 0f);
						Selected = 2;
					}
					else if (IM.DPadUp)
					{
						Button.localPosition = new Vector3(0f, 340f, 0f);
						Selected = 3;
					}
					else if (IM.DPadDown)
					{
						if (Selected == 4)
						{
							Button.localPosition = new Vector3(0f, -310f, 0f);
							Selected = 5;
						}
						else
						{
							Button.localPosition = new Vector3(0f, -190f, 0f);
							Selected = 4;
						}
					}
					UpdateSprites();
				}
				if (!Yandere.EasterEggMenu.activeInHierarchy && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5)))
				{
					Yandere.EmptyHands();
					KeyboardPanel.enabled = true;
					KeyboardShow = true;
					Show = false;
					Timer = 0f;
					if (Input.GetKeyDown(KeyCode.Alpha1))
					{
						Selected = 4;
						if (Yandere.Equipped > 0)
						{
							Yandere.CharacterAnimation["f02_reachForWeapon_00"].time = 0f;
							Yandere.ReachWeight = 1f;
							Yandere.Unequip();
						}
						if (Yandere.PickUp != null)
						{
							Yandere.PickUp.Drop();
						}
						Yandere.Mopping = false;
					}
					else if (Input.GetKeyDown(KeyCode.Alpha2))
					{
						Selected = 1;
						Equip();
					}
					else if (Input.GetKeyDown(KeyCode.Alpha3))
					{
						Selected = 2;
						Equip();
					}
					else if (Input.GetKeyDown(KeyCode.Alpha4))
					{
						Selected = 3;
						if (Yandere.Container != null && Yandere.ObstacleDetector.Obstacles == 0)
						{
							Yandere.ObstacleDetector.gameObject.SetActive(false);
							Yandere.Container.Drop();
							UpdateSprites();
						}
					}
					else if (Input.GetKeyDown(KeyCode.Alpha5))
					{
						Selected = 5;
						DropMask();
					}
					UpdateSprites();
				}
			}
			if (Yandere.CanMove || (Yandere.Chased && !Yandere.Sprayed))
			{
				if (!Show)
				{
					if (Input.GetAxis("DpadY") < -0.5f)
					{
						if (Yandere.Equipped > 0)
						{
							Debug.Log("Yandere-chan is currently carrying a: " + Yandere.EquippedWeapon.gameObject.name + ". Is it concealable? " + Yandere.EquippedWeapon.Concealable);
							if (Yandere.EquippedWeapon.Concealable)
							{
								Yandere.CharacterAnimation["f02_reachForWeapon_00"].time = 0f;
								Yandere.ReachWeight = 1f;
							}
							Yandere.Unequip();
						}
						if (Yandere.PickUp != null)
						{
							Yandere.PickUp.Drop();
						}
						Yandere.Mopping = false;
					}
				}
				else
				{
					if (Input.GetButtonDown("A"))
					{
						if (Selected < 3)
						{
							if (Yandere.Weapon[Selected] != null)
							{
								Equip();
							}
						}
						else if (Selected == 3)
						{
							if (Yandere.Container != null && Yandere.ObstacleDetector.Obstacles == 0)
							{
								Yandere.ObstacleDetector.gameObject.SetActive(false);
								Yandere.Container.Drop();
								UpdateSprites();
							}
						}
						else if (Selected == 5)
						{
							DropMask();
						}
						else
						{
							if (Yandere.Equipped > 0)
							{
								Yandere.Unequip();
							}
							if (Yandere.PickUp != null)
							{
								Yandere.PickUp.Drop();
							}
							Yandere.Mopping = false;
						}
					}
					if (Input.GetButtonDown("B"))
					{
						Show = false;
					}
				}
			}
		}
		if (!Show)
		{
			if (base.transform.localScale.x > 0.1f)
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
			}
			else if (Panel.enabled)
			{
				base.transform.localScale = Vector3.zero;
				Panel.enabled = false;
			}
		}
		else
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			if ((!Yandere.CanMove || Yandere.Aiming || PauseScreen.Show || InputDevice.Type == InputDeviceType.MouseAndKeyboard) && (!Yandere.Chased || Yandere.Sprayed))
			{
				Show = false;
			}
		}
		if (!KeyboardShow)
		{
			if (KeyboardMenu.localScale.x > 0.1f)
			{
				KeyboardMenu.localScale = Vector3.Lerp(KeyboardMenu.localScale, Vector3.zero, Time.deltaTime * 10f);
			}
			else if (KeyboardPanel.enabled)
			{
				KeyboardMenu.localScale = Vector3.zero;
				KeyboardPanel.enabled = false;
			}
			return;
		}
		KeyboardMenu.localScale = Vector3.Lerp(KeyboardMenu.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
		Timer += Time.deltaTime;
		if (Timer > 3f)
		{
			KeyboardShow = false;
		}
		if (!Yandere.CanMove || Yandere.Aiming || PauseScreen.Show || InputDevice.Type == InputDeviceType.Gamepad || Input.GetButton("Y"))
		{
			KeyboardShow = false;
		}
	}

	private void Equip()
	{
		if (!(Yandere.Weapon[Selected] != null))
		{
			return;
		}
		Yandere.CharacterAnimation["f02_reachForWeapon_00"].time = 0f;
		Yandere.ReachWeight = 1f;
		if (Yandere.PickUp != null)
		{
			Yandere.PickUp.Drop();
		}
		if (Yandere.Equipped == 3)
		{
			Yandere.Weapon[3].Drop();
		}
		if (Yandere.Weapon[1] != null)
		{
			Yandere.Weapon[1].gameObject.SetActive(false);
		}
		if (Yandere.Weapon[2] != null)
		{
			Yandere.Weapon[2].gameObject.SetActive(false);
		}
		Yandere.Equipped = Selected;
		Yandere.EquippedWeapon.gameObject.SetActive(true);
		if (Yandere.EquippedWeapon.Flaming)
		{
			Yandere.EquippedWeapon.FireEffect.Play();
		}
		if (!Yandere.Gloved)
		{
			Yandere.EquippedWeapon.FingerprintID = 100;
		}
		Yandere.StudentManager.UpdateStudents();
		Yandere.WeaponManager.UpdateLabels();
		if (Yandere.EquippedWeapon.Suspicious)
		{
			if (!Yandere.WeaponWarning)
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Armed);
				Yandere.WeaponWarning = true;
			}
		}
		else
		{
			Yandere.WeaponWarning = false;
		}
		AudioSource.PlayClipAtPoint(Yandere.EquippedWeapon.EquipClip, Camera.main.transform.position);
		Show = false;
	}

	public void UpdateSprites()
	{
		for (int i = 1; i < 3; i++)
		{
			UISprite uISprite = KeyboardBG[i];
			UISprite uISprite2 = BG[i];
			if (Selected == i)
			{
				uISprite.color = new Color(1f, 1f, 1f, 1f);
				uISprite2.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				uISprite.color = OriginalColor;
				uISprite2.color = OriginalColor;
			}
			UISprite uISprite3 = Item[i];
			UISprite uISprite4 = Outline[i];
			UISprite uISprite5 = KeyboardItem[i];
			UISprite uISprite6 = KeyboardOutline[i];
			if (Yandere.Weapon[i] == null)
			{
				uISprite3.color = new Color(uISprite3.color.r, uISprite3.color.g, uISprite3.color.b, 0f);
				uISprite2.color = new Color(uISprite2.color.r, uISprite2.color.g, uISprite2.color.b, 0.5f);
				uISprite4.color = new Color(uISprite4.color.r, uISprite4.color.g, uISprite4.color.b, 0.5f);
				uISprite5.color = new Color(uISprite5.color.r, uISprite5.color.g, uISprite5.color.b, 0f);
				uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 0.5f);
				uISprite6.color = new Color(uISprite6.color.r, uISprite6.color.g, uISprite6.color.b, 0.5f);
			}
			else
			{
				uISprite3.spriteName = Yandere.Weapon[i].SpriteName;
				uISprite3.color = new Color(uISprite3.color.r, uISprite3.color.g, uISprite3.color.b, 1f);
				uISprite2.color = new Color(uISprite2.color.r, uISprite2.color.g, uISprite2.color.b, 1f);
				uISprite4.color = new Color(uISprite4.color.r, uISprite4.color.g, uISprite4.color.b, 1f);
				uISprite5.spriteName = Yandere.Weapon[i].SpriteName;
				uISprite5.color = new Color(uISprite5.color.r, uISprite5.color.g, uISprite5.color.b, 1f);
				uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 1f);
				uISprite6.color = new Color(uISprite6.color.r, uISprite6.color.g, uISprite6.color.b, 1f);
			}
		}
		UISprite uISprite7 = KeyboardItem[3];
		UISprite uISprite8 = Item[3];
		UISprite uISprite9 = KeyboardBG[3];
		UISprite uISprite10 = BG[3];
		UISprite uISprite11 = Outline[3];
		UISprite uISprite12 = KeyboardOutline[3];
		if (Yandere.Container == null)
		{
			uISprite7.color = new Color(uISprite7.color.r, uISprite7.color.g, uISprite7.color.b, 0f);
			uISprite8.color = new Color(uISprite8.color.r, uISprite8.color.g, uISprite8.color.b, 0f);
			if (Selected == 3)
			{
				uISprite9.color = new Color(1f, 1f, 1f, 1f);
				uISprite10.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				uISprite9.color = OriginalColor;
				uISprite10.color = OriginalColor;
			}
			uISprite10.color = new Color(uISprite10.color.r, uISprite10.color.g, uISprite10.color.b, 0.5f);
			uISprite11.color = new Color(uISprite11.color.r, uISprite11.color.g, uISprite11.color.b, 0.5f);
			uISprite9.color = new Color(uISprite9.color.r, uISprite9.color.g, uISprite9.color.b, 0.5f);
			uISprite12.color = new Color(uISprite12.color.r, uISprite12.color.g, uISprite12.color.b, 0.5f);
		}
		else
		{
			uISprite8.color = new Color(uISprite8.color.r, uISprite8.color.g, uISprite8.color.b, 1f);
			uISprite10.color = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, 1f);
			uISprite11.color = new Color(uISprite11.color.r, uISprite11.color.g, uISprite11.color.b, 1f);
			uISprite7.spriteName = Yandere.Container.SpriteName;
			uISprite7.color = new Color(uISprite7.color.r, uISprite7.color.g, uISprite7.color.b, 1f);
			uISprite9.color = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, 1f);
			uISprite12.color = new Color(uISprite12.color.r, uISprite12.color.g, uISprite12.color.b, 1f);
		}
		UISprite uISprite13 = KeyboardItem[5];
		UISprite uISprite14 = Item[5];
		UISprite uISprite15 = KeyboardBG[5];
		UISprite uISprite16 = BG[5];
		UISprite uISprite17 = Outline[5];
		UISprite uISprite18 = KeyboardOutline[5];
		if (Yandere.Mask == null)
		{
			uISprite13.color = new Color(uISprite13.color.r, uISprite13.color.g, uISprite13.color.b, 0f);
			uISprite14.color = new Color(uISprite14.color.r, uISprite14.color.g, uISprite14.color.b, 0f);
			if (Selected == 5)
			{
				uISprite15.color = new Color(1f, 1f, 1f, 1f);
				uISprite16.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				uISprite15.color = OriginalColor;
				uISprite16.color = OriginalColor;
			}
			uISprite16.color = new Color(uISprite16.color.r, uISprite16.color.g, uISprite16.color.b, 0.5f);
			uISprite17.color = new Color(uISprite17.color.r, uISprite17.color.g, uISprite17.color.b, 0.5f);
			uISprite15.color = new Color(uISprite15.color.r, uISprite15.color.g, uISprite15.color.b, 0.5f);
			uISprite18.color = new Color(uISprite18.color.r, uISprite18.color.g, uISprite18.color.b, 0.5f);
		}
		else
		{
			uISprite13.color = new Color(uISprite13.color.r, uISprite13.color.g, uISprite13.color.b, 1f);
			uISprite14.color = new Color(uISprite14.color.r, uISprite14.color.g, uISprite14.color.b, 1f);
			uISprite16.color = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, 1f);
			uISprite17.color = new Color(uISprite17.color.r, uISprite17.color.g, uISprite17.color.b, 1f);
			uISprite13.color = new Color(uISprite13.color.r, uISprite13.color.g, uISprite13.color.b, 1f);
			uISprite15.color = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, 1f);
			uISprite18.color = new Color(uISprite18.color.r, uISprite18.color.g, uISprite18.color.b, 1f);
		}
		if (Selected == 4)
		{
			KeyboardBG[4].color = new Color(1f, 1f, 1f, 1f);
			BG[4].color = new Color(1f, 1f, 1f, 1f);
		}
		else
		{
			KeyboardBG[4].color = OriginalColor;
			BG[4].color = OriginalColor;
		}
	}

	private void DropMask()
	{
		if (Yandere.Mask != null)
		{
			StudentManager.CanAnyoneSeeYandere();
			if (!StudentManager.YandereVisible && !Yandere.Chased && Yandere.Chasers == 0)
			{
				Yandere.Mask.Drop();
				UpdateSprites();
				StudentManager.UpdateStudents();
			}
			else
			{
				Yandere.NotificationManager.CustomText = "Not now. Too suspicious.";
				Yandere.NotificationManager.DisplayNotification(NotificationType.Custom);
			}
		}
	}
}
