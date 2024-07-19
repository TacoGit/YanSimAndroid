using UnityEngine;

public class FanCoverScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public YandereScript Yandere;

	public PromptScript Prompt;

	public StudentScript Rival;

	public SM_rotateThis Fan;

	public ParticleSystem BloodEffects;

	public Projector BloodProjector;

	public Rigidbody MyRigidbody;

	public Transform MurderSpot;

	public GameObject Explosion;

	public GameObject OfferHelp;

	public GameObject Smoke;

	public AudioClip RivalReaction;

	public AudioSource FanSFX;

	public Texture[] YandereBloodTextures;

	public Texture[] BloodTexture;

	public bool Reacted;

	public float Timer;

	public int RivalID = 11;

	public int Phase;

	private void Start()
	{
		if (StudentManager.Students[RivalID] == null)
		{
			Prompt.Hide();
			Prompt.enabled = false;
			base.enabled = false;
		}
		else
		{
			Rival = StudentManager.Students[RivalID];
		}
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, Yandere.transform.position) < 2f)
		{
			if (Yandere.Armed)
			{
				Prompt.HideButton[0] = Yandere.EquippedWeapon.WeaponID != 6 || !Rival.Meeting;
			}
			else
			{
				Prompt.HideButton[0] = true;
			}
		}
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Yandere.CharacterAnimation.CrossFade("f02_fanMurderA_00");
			Rival.CharacterAnimation.CrossFade("f02_fanMurderB_00");
			Rival.OsanaHair.GetComponent<Animation>().CrossFade("fanMurderHair");
			Yandere.EmptyHands();
			Rival.OsanaHair.transform.parent = Rival.transform;
			Rival.OsanaHair.transform.localEulerAngles = Vector3.zero;
			Rival.OsanaHair.transform.localPosition = Vector3.zero;
			Rival.OsanaHair.transform.localScale = new Vector3(1f, 1f, 1f);
			Rival.OsanaHairL.enabled = false;
			Rival.OsanaHairR.enabled = false;
			Rival.Distracted = true;
			Yandere.CanMove = false;
			Rival.Meeting = false;
			FanSFX.enabled = false;
			GetComponent<AudioSource>().Play();
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, base.transform.localEulerAngles.z + 15f);
			Rigidbody component = GetComponent<Rigidbody>();
			component.isKinematic = false;
			component.useGravity = true;
			Prompt.enabled = false;
			Prompt.Hide();
			Phase++;
		}
		if (Phase <= 0)
		{
			return;
		}
		if (Phase == 1)
		{
			Yandere.transform.rotation = Quaternion.Slerp(Yandere.transform.rotation, MurderSpot.rotation, Time.deltaTime * 10f);
			Yandere.MoveTowardsTarget(MurderSpot.position);
			if (Yandere.CharacterAnimation["f02_fanMurderA_00"].time > 3.5f && !Reacted)
			{
				AudioSource.PlayClipAtPoint(RivalReaction, Rival.transform.position + new Vector3(0f, 1f, 0f));
				Reacted = true;
			}
			if (Yandere.CharacterAnimation["f02_fanMurderA_00"].time > 5f)
			{
				Rival.LiquidProjector.material.mainTexture = Rival.BloodTexture;
				Rival.LiquidProjector.enabled = true;
				Rival.EyeShrink = 1f;
				Yandere.BloodTextures = YandereBloodTextures;
				Yandere.Bloodiness += 20f;
				BloodProjector.gameObject.SetActive(true);
				BloodProjector.material.mainTexture = BloodTexture[1];
				BloodEffects.transform.parent = Rival.Head;
				BloodEffects.transform.localPosition = new Vector3(0f, 0.1f, 0f);
				BloodEffects.Play();
				Phase++;
			}
		}
		else if (Phase < 10)
		{
			if (Phase < 6)
			{
				Timer += Time.deltaTime;
				if (Timer > 1f)
				{
					Phase++;
					if (Phase - 1 < 5)
					{
						BloodProjector.material.mainTexture = BloodTexture[Phase - 1];
						Yandere.Bloodiness += 20f;
						Timer = 0f;
					}
				}
			}
			if (Rival.CharacterAnimation["f02_fanMurderB_00"].time >= Rival.CharacterAnimation["f02_fanMurderB_00"].length)
			{
				BloodProjector.material.mainTexture = BloodTexture[5];
				Yandere.Bloodiness += 20f;
				Rival.Ragdoll.Decapitated = true;
				Rival.OsanaHair.SetActive(false);
				Rival.DeathType = DeathType.Weapon;
				Rival.BecomeRagdoll();
				BloodEffects.Stop();
				Explosion.SetActive(true);
				Smoke.SetActive(true);
				Fan.enabled = false;
				Phase = 10;
			}
		}
		else if (Yandere.CharacterAnimation["f02_fanMurderA_00"].time >= Yandere.CharacterAnimation["f02_fanMurderA_00"].length)
		{
			OfferHelp.SetActive(false);
			Yandere.CanMove = true;
			base.enabled = false;
		}
	}
}
