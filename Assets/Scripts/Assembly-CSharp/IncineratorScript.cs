using UnityEngine;

public class IncineratorScript : MonoBehaviour
{
	public YandereScript Yandere;

	public PromptScript Prompt;

	public ClockScript Clock;

	public AudioClip IncineratorActivate;

	public AudioClip IncineratorClose;

	public AudioClip IncineratorOpen;

	public AudioSource FlameSound;

	public ParticleSystem Flames;

	public ParticleSystem Smoke;

	public Transform DumpPoint;

	public Transform RightDoor;

	public Transform LeftDoor;

	public GameObject Panel;

	public UILabel TimeLabel;

	public UISprite Circle;

	public bool YandereHoldingEvidence;

	public bool Ready;

	public bool Open;

	public int DestroyedEvidence;

	public int BloodyClothing;

	public int MurderWeapons;

	public int BodyParts;

	public int Corpses;

	public int Victims;

	public int Limbs;

	public int ID;

	public float OpenTimer;

	public float Timer;

	public int[] EvidenceList;

	public int[] CorpseList;

	public int[] VictimList;

	public int[] LimbList;

	public int[] ConfirmedDead;

	private void Start()
	{
		Panel.SetActive(false);
		Prompt.enabled = true;
	}

	private void Update()
	{
		AudioSource component = GetComponent<AudioSource>();
		if (!Open)
		{
			RightDoor.transform.localEulerAngles = new Vector3(RightDoor.transform.localEulerAngles.x, Mathf.MoveTowards(RightDoor.transform.localEulerAngles.y, 0f, Time.deltaTime * 360f), RightDoor.transform.localEulerAngles.z);
			LeftDoor.transform.localEulerAngles = new Vector3(LeftDoor.transform.localEulerAngles.x, Mathf.MoveTowards(LeftDoor.transform.localEulerAngles.y, 0f, Time.deltaTime * 360f), LeftDoor.transform.localEulerAngles.z);
			if (RightDoor.transform.localEulerAngles.y < 36f)
			{
				if (RightDoor.transform.localEulerAngles.y > 0f)
				{
					component.clip = IncineratorClose;
					component.Play();
				}
				RightDoor.transform.localEulerAngles = new Vector3(RightDoor.transform.localEulerAngles.x, 0f, RightDoor.transform.localEulerAngles.z);
			}
		}
		else
		{
			if (RightDoor.transform.localEulerAngles.y == 0f)
			{
				component.clip = IncineratorOpen;
				component.Play();
			}
			RightDoor.transform.localEulerAngles = new Vector3(RightDoor.transform.localEulerAngles.x, Mathf.Lerp(RightDoor.transform.localEulerAngles.y, 135f, Time.deltaTime * 10f), RightDoor.transform.localEulerAngles.z);
			LeftDoor.transform.localEulerAngles = new Vector3(LeftDoor.transform.localEulerAngles.x, Mathf.Lerp(LeftDoor.transform.localEulerAngles.y, 135f, Time.deltaTime * 10f), LeftDoor.transform.localEulerAngles.z);
			if (RightDoor.transform.localEulerAngles.y > 134f)
			{
				RightDoor.transform.localEulerAngles = new Vector3(RightDoor.transform.localEulerAngles.x, 135f, RightDoor.transform.localEulerAngles.z);
			}
		}
		if (OpenTimer > 0f)
		{
			OpenTimer -= Time.deltaTime;
			if (OpenTimer <= 1f)
			{
				Open = false;
			}
			if (OpenTimer <= 0f)
			{
				Prompt.enabled = true;
			}
		}
		else if (!Smoke.isPlaying)
		{
			YandereHoldingEvidence = Yandere.Ragdoll != null;
			if (!YandereHoldingEvidence)
			{
				if (Yandere.PickUp != null)
				{
					YandereHoldingEvidence = Yandere.PickUp.Evidence || Yandere.PickUp.Garbage;
				}
				else
				{
					YandereHoldingEvidence = false;
				}
			}
			if (!YandereHoldingEvidence)
			{
				if (Yandere.EquippedWeapon != null)
				{
					YandereHoldingEvidence = Yandere.EquippedWeapon.MurderWeapon;
				}
				else
				{
					YandereHoldingEvidence = false;
				}
			}
			if (!YandereHoldingEvidence)
			{
				if (!Prompt.HideButton[3])
				{
					Prompt.HideButton[3] = true;
				}
			}
			else if (Prompt.HideButton[3])
			{
				Prompt.HideButton[3] = false;
			}
			if ((Yandere.Chased || Yandere.Chasers > 0 || !YandereHoldingEvidence) && !Prompt.HideButton[3])
			{
				Prompt.HideButton[3] = true;
			}
			if (Ready)
			{
				if (!Smoke.isPlaying)
				{
					if (Prompt.HideButton[0])
					{
						Prompt.HideButton[0] = false;
					}
				}
				else if (!Prompt.HideButton[0])
				{
					Prompt.HideButton[0] = true;
				}
			}
		}
		if (Prompt.Circle[3].fillAmount == 0f)
		{
			Time.timeScale = 1f;
			if (Yandere.Ragdoll != null)
			{
				Yandere.Character.GetComponent<Animation>().CrossFade((!Yandere.Carrying) ? "f02_dragIdle_00" : "f02_carryIdleA_00");
				Yandere.YandereVision = false;
				Yandere.CanMove = false;
				Yandere.Dumping = true;
				Prompt.Hide();
				Prompt.enabled = false;
				Victims++;
				VictimList[Victims] = Yandere.Ragdoll.GetComponent<RagdollScript>().StudentID;
				Open = true;
			}
			if (Yandere.PickUp != null)
			{
				if (Yandere.PickUp.BodyPart != null)
				{
					Limbs++;
					LimbList[Limbs] = Yandere.PickUp.GetComponent<BodyPartScript>().StudentID;
				}
				Yandere.PickUp.Incinerator = this;
				Yandere.PickUp.Dumped = true;
				Yandere.PickUp.Drop();
				Prompt.Hide();
				Prompt.enabled = false;
				OpenTimer = 2f;
				Ready = true;
				Open = true;
			}
			WeaponScript equippedWeapon = Yandere.EquippedWeapon;
			if (equippedWeapon != null)
			{
				DestroyedEvidence++;
				EvidenceList[DestroyedEvidence] = equippedWeapon.WeaponID;
				equippedWeapon.Incinerator = this;
				equippedWeapon.Dumped = true;
				equippedWeapon.Drop();
				Prompt.Hide();
				Prompt.enabled = false;
				OpenTimer = 2f;
				Ready = true;
				Open = true;
			}
		}
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			Panel.SetActive(true);
			Timer = 60f;
			component.clip = IncineratorActivate;
			component.Play();
			Flames.Play();
			Smoke.Play();
			Prompt.Hide();
			Prompt.enabled = false;
			Yandere.Police.IncineratedWeapons += MurderWeapons;
			Yandere.Police.BloodyClothing -= BloodyClothing;
			Yandere.Police.BloodyWeapons -= MurderWeapons;
			Yandere.Police.BodyParts -= BodyParts;
			Yandere.Police.Corpses -= Corpses;
			if (Yandere.Police.SuicideScene && Yandere.Police.Corpses == 1)
			{
				Yandere.Police.MurderScene = false;
			}
			if (Yandere.Police.Corpses == 0)
			{
				Yandere.Police.MurderScene = false;
			}
			BloodyClothing = 0;
			MurderWeapons = 0;
			BodyParts = 0;
			Corpses = 0;
			for (ID = 0; ID < 101; ID++)
			{
				if (Yandere.StudentManager.Students[CorpseList[ID]] != null)
				{
					Yandere.StudentManager.Students[CorpseList[ID]].Ragdoll.Disposed = true;
					ConfirmedDead[ID] = CorpseList[ID];
				}
			}
		}
		if (Smoke.isPlaying)
		{
			Timer -= Time.deltaTime * (Clock.TimeSpeed / 60f);
			FlameSound.volume += Time.deltaTime;
			Circle.fillAmount = 1f - Timer / 60f;
			if (Timer <= 0f)
			{
				Prompt.HideButton[0] = true;
				Prompt.enabled = true;
				Panel.SetActive(false);
				Ready = false;
				Flames.Stop();
				Smoke.Stop();
			}
		}
		else
		{
			FlameSound.volume -= Time.deltaTime;
		}
		if (Panel.activeInHierarchy)
		{
			float num = Mathf.CeilToInt(Timer * 60f);
			float num2 = Mathf.Floor(num / 60f);
			float num3 = Mathf.RoundToInt(num % 60f);
			TimeLabel.text = string.Format("{0:00}:{1:00}", num2, num3);
		}
	}

	public void SetVictimsMissing()
	{
		int[] confirmedDead = ConfirmedDead;
		foreach (int studentID in confirmedDead)
		{
			StudentGlobals.SetStudentMissing(studentID, true);
		}
	}
}
