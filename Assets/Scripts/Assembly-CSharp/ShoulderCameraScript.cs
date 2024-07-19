using UnityEngine;

public class ShoulderCameraScript : MonoBehaviour
{
	public PauseScreenScript PauseScreen;

	public CounselorScript Counselor;

	public YandereScript Yandere;

	public RPG_Camera RPGCamera;

	public PortalScript Portal;

	public GameObject HeartbrokenCamera;

	public GameObject HUD;

	public Transform Smartphone;

	public Transform Teacher;

	public Transform ShoulderFocus;

	public Transform ShoulderPOV;

	public Transform CameraFocus;

	public Transform CameraPOV;

	public Transform NoticedFocus;

	public Transform NoticedPOV;

	public Transform StruggleFocus;

	public Transform StrugglePOV;

	public Transform Focus;

	public Vector3 LastPosition;

	public Vector3 TeacherLossFocus;

	public Vector3 TeacherLossPOV;

	public Vector3 LossFocus;

	public Vector3 LossPOV;

	public bool GoingToCounselor;

	public bool AimingCamera;

	public bool OverShoulder;

	public bool DoNotMove;

	public bool Summoning;

	public bool LookDown;

	public bool Scolding;

	public bool Struggle;

	public bool Counter;

	public bool Noticed;

	public bool Spoken;

	public bool Skip;

	public AudioClip StruggleLose;

	public AudioClip Slam;

	public float NoticedHeight;

	public float NoticedTimer;

	public float NoticedSpeed;

	public float Height;

	public float PullBackTimer;

	public float Timer;

	public int NoticedLimit;

	public int Phase;

	private void LateUpdate()
	{
		if (PauseScreen.Show)
		{
			return;
		}
		if (OverShoulder)
		{
			if (RPGCamera.enabled)
			{
				ShoulderFocus.position = RPGCamera.cameraPivot.position;
				LastPosition = base.transform.position;
				RPGCamera.enabled = false;
			}
			if (Yandere.TargetStudent.Counselor)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, ShoulderPOV.position + new Vector3(0f, -0.49f, 0f), Time.deltaTime * 10f);
			}
			else
			{
				base.transform.position = Vector3.Lerp(base.transform.position, ShoulderPOV.position, Time.deltaTime * 10f);
			}
			ShoulderFocus.position = Vector3.Lerp(ShoulderFocus.position, Yandere.TargetStudent.transform.position + Vector3.up * Height, Time.deltaTime * 10f);
			base.transform.LookAt(ShoulderFocus);
		}
		else if (AimingCamera)
		{
			base.transform.position = CameraPOV.position;
			base.transform.LookAt(CameraFocus);
		}
		else if (Noticed)
		{
			if (NoticedTimer == 0f)
			{
				GetComponent<Camera>().cullingMask &= -8193;
				StudentScript component = Yandere.Senpai.GetComponent<StudentScript>();
				if (component.Teacher)
				{
					GoingToCounselor = true;
					NoticedHeight = 1.6f;
					NoticedLimit = 6;
				}
				if (component.Club == ClubType.Council)
				{
					GoingToCounselor = true;
					NoticedHeight = 1.375f;
					NoticedLimit = 6;
				}
				else if (component.Witnessed == StudentWitnessType.Stalking)
				{
					NoticedHeight = 1.481275f;
					NoticedLimit = 7;
				}
				else
				{
					NoticedHeight = 1.375f;
					NoticedLimit = 6;
				}
				NoticedPOV.position = Yandere.Senpai.position + Yandere.Senpai.forward + Vector3.up * NoticedHeight;
				NoticedPOV.LookAt(Yandere.Senpai.position + Vector3.up * NoticedHeight);
				NoticedFocus.position = base.transform.position + base.transform.forward;
				NoticedSpeed = 10f;
			}
			NoticedTimer += Time.deltaTime;
			if (Phase == 1)
			{
				if (Input.GetButtonDown("A") && !Yandere.Attacking)
				{
					Yandere.transform.rotation = Quaternion.LookRotation(Yandere.Senpai.position - Yandere.transform.position);
					NoticedTimer += 10f;
				}
				NoticedFocus.position = Vector3.Lerp(NoticedFocus.position, Yandere.Senpai.position + Vector3.up * NoticedHeight, Time.deltaTime * 10f);
				NoticedPOV.Translate(Vector3.forward * Time.deltaTime * -0.075f);
				if (NoticedTimer > 1f && !Spoken && !Yandere.Senpai.GetComponent<StudentScript>().Teacher)
				{
					Yandere.Senpai.GetComponent<StudentScript>().DetermineSenpaiReaction();
					Spoken = true;
				}
				if (NoticedTimer > (float)NoticedLimit || Skip)
				{
					Yandere.Senpai.GetComponent<StudentScript>().Character.SetActive(false);
					GetComponent<Camera>().cullingMask |= 8192;
					NoticedPOV.position = Yandere.transform.position + Yandere.transform.forward + Vector3.up * 1.375f;
					NoticedPOV.LookAt(Yandere.transform.position + Vector3.up * 1.375f);
					NoticedFocus.position = Yandere.transform.position + Vector3.up * 1.375f;
					base.transform.position = NoticedPOV.position;
					NoticedTimer = NoticedLimit;
					Phase = 2;
					if (GoingToCounselor)
					{
						Yandere.CharacterAnimation.CrossFade("f02_disappointed_00");
					}
					else
					{
						Yandere.CharacterAnimation.CrossFade("f02_scaredIdle_00");
						Yandere.Subtitle.UpdateLabel(SubtitleType.YandereWhimper, 1, 3.5f);
						Debug.Log("We're here.");
					}
				}
			}
			else if (Phase == 2)
			{
				if (Input.GetButtonDown("A"))
				{
					NoticedTimer += 10f;
				}
				if (!GoingToCounselor)
				{
					Yandere.EyeShrink += Time.deltaTime * 0.25f;
				}
				NoticedPOV.Translate(Vector3.forward * Time.deltaTime * 0.075f);
				if (GoingToCounselor)
				{
					Yandere.CharacterAnimation.CrossFade("f02_disappointed_00");
				}
				else
				{
					Yandere.CharacterAnimation.CrossFade("f02_scaredIdle_00");
				}
				if (NoticedTimer > (float)(NoticedLimit + 4))
				{
					if (!GoingToCounselor)
					{
						NoticedPOV.Translate(Vector3.back * 2f);
						NoticedPOV.transform.position = new Vector3(NoticedPOV.transform.position.x, Yandere.transform.position.y + 1f, NoticedPOV.transform.position.z);
						NoticedSpeed = 1f;
						Yandere.Character.GetComponent<Animation>().CrossFade("f02_down_22");
						HeartbrokenCamera.SetActive(true);
						Yandere.Collapse = true;
						Phase = 3;
					}
					else
					{
						Yandere.Police.Darkness.enabled = true;
						Yandere.HUD.enabled = true;
						Yandere.HUD.alpha = 1f;
						if (Yandere.Police.Corpses - Yandere.Police.HiddenCorpses <= 0)
						{
							HUD.SetActive(false);
						}
						Phase = 4;
					}
				}
			}
			else if (Phase == 3)
			{
				NoticedFocus.transform.position = new Vector3(NoticedFocus.transform.position.x, Mathf.Lerp(NoticedFocus.transform.position.y, Yandere.transform.position.y + 1f, Time.deltaTime), NoticedFocus.transform.position.z);
			}
			else if (Phase == 4)
			{
				Yandere.Police.Darkness.color += new Color(0f, 0f, 0f, Time.deltaTime);
				NoticedPOV.Translate(Vector3.forward * Time.deltaTime * 0.075f);
				if (Yandere.Police.Darkness.color.a >= 1f)
				{
					if (Yandere.Police.Corpses - Yandere.Police.HiddenCorpses > 0)
					{
						Portal.EndDay();
					}
					else
					{
						Yandere.StudentManager.PreventAlarm();
						Counselor.Crime = Yandere.Senpai.GetComponent<StudentScript>().Witnessed;
						Counselor.MyAnimation.Play("CounselorArmsCrossed");
						Counselor.Laptop.SetActive(false);
						Counselor.Interrogating = true;
						Counselor.LookAtPlayer = true;
						Counselor.Stern = true;
						Counselor.Timer = 0f;
						base.transform.Translate(Vector3.forward * -1f);
						Yandere.Senpai.GetComponent<StudentScript>().Character.SetActive(true);
						Yandere.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
						Yandere.transform.position = new Vector3(-27.51f, 0f, 12f);
						Yandere.Police.Darkness.color = new Color(0f, 0f, 0f, 1f);
						Yandere.CharacterAnimation.Play("f02_sit_00");
						Yandere.Noticed = false;
						Yandere.Sanity = 100f;
						Physics.SyncTransforms();
						GoingToCounselor = false;
						base.enabled = false;
						NoticedTimer = 0f;
						Phase = 1;
					}
				}
			}
			if (Phase < 5)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, NoticedPOV.position, Time.deltaTime * NoticedSpeed);
				base.transform.LookAt(NoticedFocus);
			}
		}
		else if (Scolding)
		{
			if (Timer == 0f)
			{
				NoticedHeight = 1.6f;
				NoticedPOV.position = Teacher.position + Teacher.forward + Vector3.up * NoticedHeight;
				NoticedPOV.LookAt(Teacher.position + Vector3.up * NoticedHeight);
				NoticedFocus.position = Teacher.position + Vector3.up * NoticedHeight;
				NoticedSpeed = 10f;
			}
			base.transform.position = Vector3.Lerp(base.transform.position, NoticedPOV.position, Time.deltaTime * NoticedSpeed);
			base.transform.LookAt(NoticedFocus);
			Timer += Time.deltaTime;
			if (Timer > 6f)
			{
				Portal.ClassDarkness.enabled = true;
				Portal.Transition = true;
				Portal.FadeOut = true;
			}
			if (Timer > 7f)
			{
				Scolding = false;
				Timer = 0f;
			}
		}
		else if (Counter)
		{
			if (Timer == 0f)
			{
				StruggleFocus.position = base.transform.position + base.transform.forward;
				StrugglePOV.position = base.transform.position;
			}
			base.transform.position = Vector3.Lerp(base.transform.position, StrugglePOV.position, Time.deltaTime * 10f);
			base.transform.LookAt(StruggleFocus);
			Timer += Time.deltaTime;
			if (Timer > 0.5f && Phase < 2)
			{
				Yandere.CameraEffects.MurderWitnessed();
				Yandere.Jukebox.GameOver();
				Phase++;
			}
			if (Timer > 1.4f && Phase < 3)
			{
				Yandere.Subtitle.UpdateLabel(SubtitleType.TeacherAttackReaction, 1, 4f);
				Phase++;
			}
			if (Timer > 6f && Yandere.Armed)
			{
				Yandere.EquippedWeapon.Drop();
			}
			if (Timer > 6.66666f && Phase < 4)
			{
				GetComponent<AudioSource>().PlayOneShot(Slam);
				Phase++;
			}
			if (Timer > 10f && Phase < 5)
			{
				HeartbrokenCamera.SetActive(true);
				Phase++;
			}
			if (Timer < 5f)
			{
				StruggleFocus.position = Vector3.Lerp(StruggleFocus.position, Yandere.TargetStudent.transform.position + Vector3.up * 1.4f, Time.deltaTime);
				StrugglePOV.localPosition = Vector3.Lerp(StrugglePOV.localPosition, new Vector3(0.5f, 1.4f, 0.3f), Time.deltaTime);
			}
			else if (Timer < 10f)
			{
				if (Timer < 6.5f)
				{
					PullBackTimer = Mathf.MoveTowards(PullBackTimer, 1.5f, Time.deltaTime);
				}
				else
				{
					PullBackTimer = Mathf.MoveTowards(PullBackTimer, 0f, Time.deltaTime * 0.42857143f);
				}
				base.transform.Translate(Vector3.back * Time.deltaTime * 10f * PullBackTimer);
				StruggleFocus.localPosition = Vector3.Lerp(StruggleFocus.localPosition, new Vector3(0f, 0.11466666f, -0.84f), Time.deltaTime);
				StrugglePOV.localPosition = Vector3.Lerp(StrugglePOV.localPosition, new Vector3(0.6f, 0.11466666f, -0.84f), Time.deltaTime);
			}
			else
			{
				StruggleFocus.localPosition = Vector3.Lerp(StruggleFocus.localPosition, new Vector3(0f, 0.3f, -0.4f), Time.deltaTime);
				StrugglePOV.localPosition = Vector3.Lerp(StrugglePOV.localPosition, new Vector3(1.05f, 0.3f, -0.4f), Time.deltaTime);
			}
		}
		else if (Struggle)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, StrugglePOV.position, Time.deltaTime * 10f);
			base.transform.LookAt(StruggleFocus);
			if (Yandere.Lost)
			{
				StruggleFocus.localPosition = Vector3.MoveTowards(StruggleFocus.localPosition, LossFocus, Time.deltaTime);
				StrugglePOV.localPosition = Vector3.MoveTowards(StrugglePOV.localPosition, LossPOV, Time.deltaTime);
				if (Timer == 0f)
				{
					AudioSource component2 = GetComponent<AudioSource>();
					component2.clip = StruggleLose;
					component2.Play();
				}
				Timer += Time.deltaTime;
				if (Timer < 3f)
				{
					base.transform.Translate(Vector3.back * (Time.deltaTime * 10f * Timer * (3f - Timer)));
				}
				else if (!HeartbrokenCamera.activeInHierarchy)
				{
					HeartbrokenCamera.SetActive(true);
					Yandere.Jukebox.GameOver();
					base.enabled = false;
				}
			}
		}
		else if (Yandere.Attacked)
		{
			Focus.transform.parent = null;
			Focus.transform.position = Vector3.Lerp(Focus.transform.position, Yandere.Hips.position, Time.deltaTime);
			base.transform.LookAt(Focus);
		}
		else if (LookDown)
		{
			Timer += Time.deltaTime;
			if (Timer < 5f)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, Yandere.Hips.position + Vector3.up * 3f + Vector3.right * 0.1f, Time.deltaTime * Timer);
				Focus.transform.parent = null;
				Focus.transform.position = Vector3.Lerp(Focus.transform.position, Yandere.Hips.position, Time.deltaTime * Timer);
				base.transform.LookAt(Focus);
			}
			else if (!HeartbrokenCamera.activeInHierarchy)
			{
				HeartbrokenCamera.SetActive(true);
				Yandere.Jukebox.GameOver();
				base.enabled = false;
			}
		}
		else if (Summoning)
		{
			if (Phase == 1)
			{
				NoticedPOV.position = Yandere.transform.position + Yandere.transform.forward * 1.7f + Yandere.transform.right * 0.15f + Vector3.up * 1.375f;
				NoticedFocus.position = base.transform.position + base.transform.forward;
				NoticedSpeed = 10f;
				Phase++;
			}
			else if (Phase == 2)
			{
				NoticedPOV.Translate(NoticedPOV.forward * (Time.deltaTime * -0.1f));
				NoticedFocus.position = Vector3.Lerp(NoticedFocus.position, Yandere.transform.position + Yandere.transform.right * 0.15f + Vector3.up * 1.375f, Time.deltaTime * 10f);
				Timer += Time.deltaTime;
				if (Timer > 2f)
				{
					Yandere.Stand.Spawn();
					NoticedPOV.position = Yandere.transform.position + Yandere.transform.forward * 2f + Vector3.up * 2.4f;
					Timer = 0f;
					Phase++;
				}
			}
			else if (Phase == 3)
			{
				NoticedPOV.Translate(NoticedPOV.forward * (Time.deltaTime * -0.1f));
				NoticedFocus.position = Yandere.transform.position + Vector3.up * 2.4f;
				Yandere.Stand.Stand.SetActive(true);
				Timer += Time.deltaTime;
				if (Timer > 5f)
				{
					Phase++;
				}
			}
			else if (Phase == 4)
			{
				Yandere.Stand.transform.localPosition = new Vector3(Yandere.Stand.transform.localPosition.x, 0f, Yandere.Stand.transform.localPosition.z);
				Yandere.Jukebox.PlayJojo();
				Yandere.Talking = true;
				Summoning = false;
				Timer = 0f;
				Phase = 1;
			}
			base.transform.position = Vector3.Lerp(base.transform.position, NoticedPOV.position, Time.deltaTime * NoticedSpeed);
			base.transform.LookAt(NoticedFocus);
		}
		else
		{
			if ((!Yandere.Talking && !Yandere.Won) || RPGCamera.enabled)
			{
				return;
			}
			Timer += Time.deltaTime;
			if (Timer < 0.5f)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, LastPosition, Time.deltaTime * 10f);
				if (Yandere.Talking)
				{
					ShoulderFocus.position = Vector3.Lerp(ShoulderFocus.position, RPGCamera.cameraPivot.position, Time.deltaTime * 10f);
					base.transform.LookAt(ShoulderFocus);
				}
				else
				{
					StruggleFocus.position = Vector3.Lerp(StruggleFocus.position, RPGCamera.cameraPivot.position, Time.deltaTime * 10f);
					base.transform.LookAt(StruggleFocus);
				}
				return;
			}
			RPGCamera.enabled = true;
			Yandere.MyController.enabled = true;
			Yandere.Talking = false;
			if (!Yandere.Sprayed)
			{
				Yandere.CanMove = true;
			}
			Yandere.Pursuer = null;
			Yandere.Chased = false;
			Yandere.Won = false;
			Timer = 0f;
		}
	}

	public void YandereNo()
	{
		AudioSource component = GetComponent<AudioSource>();
		component.clip = StruggleLose;
		component.Play();
	}

	public void GameOver()
	{
		Yandere.Character.GetComponent<Animation>().CrossFade("f02_down_22");
		HeartbrokenCamera.SetActive(true);
		Yandere.Collapse = true;
	}
}
