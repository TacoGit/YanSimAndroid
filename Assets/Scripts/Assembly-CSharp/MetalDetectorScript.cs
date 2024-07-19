using UnityEngine;

public class MetalDetectorScript : MonoBehaviour
{
	public MissionModeScript MissionMode;

	public YandereScript Yandere;

	public PromptScript Prompt;

	public ParticleSystem PepperSprayEffect;

	public AudioSource MyAudio;

	public AudioClip PepperSpraySFX;

	public AudioClip Alarm;

	public Collider MyCollider;

	public float SprayTimer;

	public bool Spraying;

	private void Start()
	{
		MyAudio = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (Yandere.Armed)
		{
			if (Yandere.EquippedWeapon.WeaponID == 6)
			{
				Prompt.enabled = true;
				if (Prompt.Circle[0].fillAmount == 0f)
				{
					MyAudio.Play();
					MyCollider.enabled = false;
					Prompt.Hide();
					Prompt.enabled = false;
					base.enabled = false;
				}
			}
			else if (Prompt.enabled)
			{
				Prompt.Hide();
				Prompt.enabled = false;
			}
		}
		else if (Prompt.enabled)
		{
			Prompt.Hide();
			Prompt.enabled = false;
		}
		if (Spraying)
		{
			SprayTimer += Time.deltaTime;
			if ((double)SprayTimer > 0.66666)
			{
				if (Yandere.Armed)
				{
					Yandere.EquippedWeapon.Drop();
				}
				Yandere.EmptyHands();
				PepperSprayEffect.Play();
				Spraying = false;
			}
		}
		MyAudio.volume -= Time.deltaTime * 0.01f;
	}

	private void OnTriggerStay(Collider other)
	{
		bool flag = false;
		if (MissionMode.GameOverID != 0 || other.gameObject.layer != 13)
		{
			return;
		}
		for (int i = 1; i < 4; i++)
		{
			WeaponScript weaponScript = Yandere.Weapon[i];
			flag |= weaponScript != null && weaponScript.Metal;
			if (!flag && Yandere.Container != null && Yandere.Container.Weapon != null)
			{
				weaponScript = Yandere.Container.Weapon;
				flag = weaponScript.Metal;
			}
			if (!flag && Yandere.PickUp != null && Yandere.PickUp.TrashCan != null && Yandere.PickUp.TrashCan.Weapon)
			{
				weaponScript = Yandere.PickUp.TrashCan.Item.GetComponent<WeaponScript>();
				flag = weaponScript.Metal;
			}
		}
		if (!flag || Yandere.Inventory.IDCard)
		{
			return;
		}
		if (MissionMode.enabled)
		{
			MissionMode.GameOverID = 16;
			MissionMode.GameOver();
			MissionMode.Phase = 4;
			base.enabled = false;
		}
		else if (!Yandere.Sprayed)
		{
			MyAudio.clip = Alarm;
			MyAudio.loop = true;
			MyAudio.Play();
			MyAudio.volume = 0.1f;
			AudioSource.PlayClipAtPoint(PepperSpraySFX, base.transform.position);
			if (Yandere.Aiming)
			{
				Yandere.StopAiming();
			}
			PepperSprayEffect.transform.position = new Vector3(base.transform.position.x, Yandere.transform.position.y + 1.8f, Yandere.transform.position.z);
			Spraying = true;
			Yandere.CharacterAnimation.CrossFade("f02_sprayed_00");
			Yandere.FollowHips = true;
			Yandere.Punching = false;
			Yandere.CanMove = false;
			Yandere.Sprayed = true;
			Yandere.StudentManager.YandereDying = true;
			Yandere.StudentManager.StopMoving();
			Yandere.Blur.blurIterations = 1;
			Yandere.Jukebox.Volume = 0f;
			Time.timeScale = 1f;
		}
	}
}
