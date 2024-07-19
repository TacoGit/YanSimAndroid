using System;
using UnityEngine;

public class AttackManagerScript : MonoBehaviour
{
	public GameObject BloodEffect;

	private GameObject OriginalBloodEffect;

	private GameObject Victim;

	private YandereScript Yandere;

	private string VictimAnimName = string.Empty;

	private string AnimName = string.Empty;

	public bool PingPong;

	public bool Stealth;

	public bool Censor;

	public bool Loop;

	public int EffectPhase;

	public int LoopPhase;

	public float AttackTimer;

	public float Distance;

	public float Timer;

	public float LoopStart;

	public float LoopEnd;

	public Animation YandereAnim;

	public Animation VictimAnim;

	private void Awake()
	{
		Yandere = GetComponent<YandereScript>();
	}

	private void Start()
	{
		OriginalBloodEffect = BloodEffect;
	}

	public bool IsAttacking()
	{
		return Victim != null;
	}

	private float GetReachDistance(WeaponType weaponType, SanityType sanityType)
	{
		switch (weaponType)
		{
		case WeaponType.Knife:
			if (Stealth)
			{
				return 0.75f;
			}
			switch (sanityType)
			{
			case SanityType.High:
				return 1f;
			case SanityType.Medium:
				return 0.75f;
			default:
				return 0.5f;
			}
		case WeaponType.Katana:
			return (!Stealth) ? 1f : 0.5f;
		case WeaponType.Bat:
			if (Stealth)
			{
				return 0.5f;
			}
			switch (sanityType)
			{
			case SanityType.High:
				return 0.75f;
			case SanityType.Medium:
				return 1f;
			default:
				return 1f;
			}
		case WeaponType.Saw:
			return (!Stealth) ? 1f : 0.7f;
		case WeaponType.Weight:
			if (Stealth)
			{
				return 0.75f;
			}
			switch (sanityType)
			{
			case SanityType.High:
				return 0.75f;
			case SanityType.Medium:
				return 0.75f;
			default:
				return 0.75f;
			}
		case WeaponType.Syringe:
			return 0.5f;
		default:
			Debug.LogError("Weapon type \"" + weaponType.ToString() + "\" not implemented.");
			return 0f;
		}
	}

	public void Attack(GameObject victim, WeaponScript weapon)
	{
		Victim = victim;
		Yandere.FollowHips = true;
		AttackTimer = 0f;
		EffectPhase = 0;
		Yandere.Sanity = Mathf.Clamp(Yandere.Sanity, 0f, 100f);
		SanityType sanityType = Yandere.SanityType;
		string sanityString = Yandere.GetSanityString(sanityType);
		string text = weapon.GetTypePrefix();
		string text2 = ((!Yandere.TargetStudent.Male) ? "f02_" : string.Empty);
		if (!Stealth)
		{
			VictimAnimName = text2 + text + sanityString + "SanityB_00";
			if (weapon.WeaponID == 23)
			{
				text = "extin";
			}
			AnimName = "f02_" + text + sanityString + "SanityA_00";
		}
		else
		{
			VictimAnimName = text2 + text + "StealthB_00";
			if (weapon.WeaponID == 23)
			{
				text = "extin";
			}
			AnimName = "f02_" + text + "StealthA_00";
		}
		YandereAnim = Yandere.CharacterAnimation;
		YandereAnim[AnimName].time = 0f;
		YandereAnim.CrossFade(AnimName);
		VictimAnim = Yandere.TargetStudent.CharacterAnimation;
		VictimAnim[VictimAnimName].time = 0f;
		VictimAnim.CrossFade(VictimAnimName);
		AudioSource component = weapon.gameObject.GetComponent<AudioSource>();
		component.clip = weapon.GetClip(Yandere.Sanity / 100f, Stealth);
		component.time = 0f;
		component.Play();
		if (weapon.Type == WeaponType.Knife)
		{
			weapon.Flip = true;
		}
		Distance = GetReachDistance(weapon.Type, sanityType);
	}

	private void Update()
	{
		if (!IsAttacking())
		{
			return;
		}
		VictimAnim.CrossFade(VictimAnimName);
		if (Censor)
		{
			if (AttackTimer == 0f)
			{
				Yandere.Blur.enabled = true;
				Yandere.Blur.blurSize = 0f;
				Yandere.Blur.blurIterations = 0;
			}
			if (AttackTimer < YandereAnim[AnimName].length - 0.5f)
			{
				Yandere.Blur.blurSize = Mathf.MoveTowards(Yandere.Blur.blurSize, 10f, Time.deltaTime * 10f);
				if (Yandere.Blur.blurSize > (float)Yandere.Blur.blurIterations)
				{
					Yandere.Blur.blurIterations++;
				}
			}
			else
			{
				Yandere.Blur.blurSize = Mathf.Lerp(Yandere.Blur.blurSize, 0f, Time.deltaTime * 10f);
				if (Yandere.Blur.blurSize < (float)Yandere.Blur.blurIterations)
				{
					Yandere.Blur.blurIterations--;
				}
			}
		}
		AttackTimer += Time.deltaTime;
		WeaponScript equippedWeapon = Yandere.EquippedWeapon;
		SanityType sanityType = Yandere.SanityType;
		SpecialEffect(equippedWeapon, sanityType);
		if (sanityType == SanityType.Low)
		{
			LoopCheck(equippedWeapon);
		}
		SpecialEffect(equippedWeapon, sanityType);
		if (YandereAnim[AnimName].time > YandereAnim[AnimName].length - 1f / 3f)
		{
			YandereAnim.CrossFade("f02_idle_00");
			equippedWeapon.Flip = false;
		}
		if (AttackTimer > YandereAnim[AnimName].length)
		{
			if (Yandere.TargetStudent == Yandere.StudentManager.Reporter)
			{
				Yandere.StudentManager.Reporter = null;
			}
			if (!Yandere.CanTranq)
			{
				Yandere.TargetStudent.DeathType = DeathType.Weapon;
			}
			else
			{
				Yandere.TargetStudent.Tranquil = true;
				Yandere.CanTranq = false;
				Yandere.Followers--;
				equippedWeapon.Type = WeaponType.Knife;
			}
			Yandere.TargetStudent.DeathCause = equippedWeapon.WeaponID;
			Yandere.TargetStudent.BecomeRagdoll();
			Yandere.Sanity -= ((PlayerGlobals.PantiesEquipped != 10) ? 20f : 10f) * Yandere.Numbness;
			Yandere.Attacking = false;
			Yandere.FollowHips = false;
			Yandere.MyController.radius = 0.2f;
			bool flag = false;
			if (Yandere.EquippedWeapon.Type == WeaponType.Bat && Stealth)
			{
				flag = true;
			}
			if (!flag)
			{
				Yandere.EquippedWeapon.Evidence = true;
			}
			Victim = null;
			VictimAnimName = null;
			AnimName = null;
			Stealth = false;
			EffectPhase = 0;
			AttackTimer = 0f;
			Timer = 0f;
			CheckForSpecialCase(equippedWeapon);
			Yandere.Blur.enabled = false;
			Yandere.Blur.blurSize = 0f;
			if (!Yandere.Noticed)
			{
				Yandere.EquippedWeapon.MurderWeapon = true;
				Yandere.CanMove = true;
			}
			else
			{
				equippedWeapon.Drop();
			}
		}
	}

	private void SpecialEffect(WeaponScript weapon, SanityType sanityType)
	{
		BloodEffect = OriginalBloodEffect;
		if (weapon.WeaponID == 14)
		{
			BloodEffect = weapon.HeartBurst;
		}
		if (weapon.Type == WeaponType.Knife)
		{
			if (!Stealth)
			{
				switch (sanityType)
				{
				case SanityType.High:
					if (EffectPhase == 0 && YandereAnim[AnimName].time > 1.0666667f)
					{
						Yandere.Bloodiness += 20f;
						Yandere.StainWeapon();
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
						EffectPhase++;
					}
					return;
				case SanityType.Medium:
					if (EffectPhase == 0)
					{
						if (YandereAnim[AnimName].time > 2.1666667f)
						{
							Yandere.Bloodiness += 20f;
							Yandere.StainWeapon();
							UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
							EffectPhase++;
						}
					}
					else if (EffectPhase == 1 && YandereAnim[AnimName].time > 3.0333333f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
						EffectPhase++;
					}
					return;
				}
				if (EffectPhase == 0)
				{
					if (YandereAnim[AnimName].time > 2.7666667f)
					{
						Yandere.Bloodiness += 20f;
						Yandere.StainWeapon();
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 1)
				{
					if (YandereAnim[AnimName].time > 3.5333333f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 2 && YandereAnim[AnimName].time > 4.1666665f)
				{
					UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
					EffectPhase++;
				}
			}
			else if (EffectPhase == 0 && YandereAnim[AnimName].time > 29f / 30f)
			{
				Yandere.Bloodiness += 20f;
				Yandere.StainWeapon();
				UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
				EffectPhase++;
			}
		}
		else if (weapon.Type == WeaponType.Katana)
		{
			if (!Stealth)
			{
				switch (sanityType)
				{
				case SanityType.High:
					if (EffectPhase == 0 && YandereAnim[AnimName].time > 29f / 60f)
					{
						Yandere.Bloodiness += 20f;
						Yandere.StainWeapon();
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.5f, Quaternion.identity);
						EffectPhase++;
					}
					return;
				case SanityType.Medium:
					if (EffectPhase == 0)
					{
						if (YandereAnim[AnimName].time > 0.55f)
						{
							Yandere.Bloodiness += 20f;
							Yandere.StainWeapon();
							UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.5f, Quaternion.identity);
							EffectPhase++;
						}
					}
					else if (EffectPhase == 1 && YandereAnim[AnimName].time > 1.5166667f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.5f, Quaternion.identity);
						EffectPhase++;
					}
					return;
				}
				if (EffectPhase == 0)
				{
					if (YandereAnim[AnimName].time > 0.5f)
					{
						Yandere.Bloodiness += 20f;
						Yandere.StainWeapon();
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * (2f / 3f), Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 1)
				{
					if (YandereAnim[AnimName].time > 1f)
					{
						weapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 2)
				{
					if (YandereAnim[AnimName].time > 2.3333333f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * (2f / 3f), Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 3)
				{
					if (YandereAnim[AnimName].time > 2.7333333f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * (2f / 3f), Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 4)
				{
					if (YandereAnim[AnimName].time > 3.1333334f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * (2f / 3f), Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 5)
				{
					if (YandereAnim[AnimName].time > 3.5333333f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * (2f / 3f), Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 6)
				{
					if (YandereAnim[AnimName].time > 4.133333f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * (2f / 3f), Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 8 && YandereAnim[AnimName].time > 5f)
				{
					weapon.transform.localEulerAngles = Vector3.zero;
					EffectPhase++;
				}
			}
			else if (EffectPhase == 0)
			{
				if (YandereAnim[AnimName].time > 11f / 30f)
				{
					Yandere.Bloodiness += 20f;
					Yandere.StainWeapon();
					UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * (2f / 3f), Quaternion.identity);
					EffectPhase++;
				}
			}
			else if (EffectPhase == 1 && YandereAnim[AnimName].time > 1f)
			{
				UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * (1f / 3f), Quaternion.identity);
				EffectPhase++;
			}
		}
		else if (weapon.Type == WeaponType.Bat)
		{
			if (!Stealth)
			{
				switch (sanityType)
				{
				case SanityType.High:
					if (EffectPhase == 0 && YandereAnim[AnimName].time > 11f / 15f)
					{
						Yandere.Bloodiness += 20f;
						Yandere.StainWeapon();
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.5f, Quaternion.identity);
						EffectPhase++;
					}
					return;
				case SanityType.Medium:
					if (EffectPhase == 0)
					{
						if (YandereAnim[AnimName].time > 1f)
						{
							Yandere.Bloodiness += 20f;
							Yandere.StainWeapon();
							UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.5f, Quaternion.identity);
							EffectPhase++;
						}
					}
					else if (EffectPhase == 1 && YandereAnim[AnimName].time > 2.9666667f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.5f, Quaternion.identity);
						EffectPhase++;
					}
					return;
				}
				if (EffectPhase == 0)
				{
					if (YandereAnim[AnimName].time > 0.7f)
					{
						Yandere.Bloodiness += 20f;
						Yandere.StainWeapon();
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.5f, Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 1)
				{
					if (YandereAnim[AnimName].time > 3.1f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.5f, Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 2)
				{
					if (YandereAnim[AnimName].time > 3.7666667f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.5f, Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 3 && YandereAnim[AnimName].time > 4.4f)
				{
					UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.5f, Quaternion.identity);
					EffectPhase++;
				}
			}
			else
			{
				Yandere.TargetStudent.Ragdoll.NeckSnapped = true;
			}
		}
		else if (weapon.Type == WeaponType.Saw)
		{
			if (!Stealth)
			{
				switch (sanityType)
				{
				case SanityType.High:
					if (EffectPhase == 0)
					{
						if (YandereAnim[AnimName].time > 0f)
						{
							weapon.Spin = true;
							EffectPhase++;
						}
					}
					else if (EffectPhase == 1)
					{
						if (YandereAnim[AnimName].time > 11f / 15f)
						{
							Yandere.Bloodiness += 20f;
							Yandere.StainWeapon();
							weapon.BloodSpray[0].Play();
							weapon.BloodSpray[1].Play();
							EffectPhase++;
						}
					}
					else if (EffectPhase == 2 && YandereAnim[AnimName].time > 1.4333333f)
					{
						weapon.Spin = false;
						weapon.BloodSpray[0].Stop();
						weapon.BloodSpray[1].Stop();
						EffectPhase++;
					}
					return;
				case SanityType.Medium:
					if (EffectPhase == 0)
					{
						if (YandereAnim[AnimName].time > 0f)
						{
							weapon.Spin = true;
							EffectPhase++;
						}
					}
					else if (EffectPhase == 1)
					{
						if (YandereAnim[AnimName].time > 1.1f)
						{
							Yandere.Bloodiness += 20f;
							Yandere.StainWeapon();
							weapon.BloodSpray[0].Play();
							weapon.BloodSpray[1].Play();
							EffectPhase++;
						}
					}
					else if (EffectPhase == 2)
					{
						if (YandereAnim[AnimName].time > 1.4333333f)
						{
							weapon.BloodSpray[0].Stop();
							weapon.BloodSpray[1].Stop();
							EffectPhase++;
						}
					}
					else if (EffectPhase == 3)
					{
						if (YandereAnim[AnimName].time > (float)Math.PI * 113f / 150f)
						{
							weapon.BloodSpray[0].Play();
							weapon.BloodSpray[1].Play();
							EffectPhase++;
						}
					}
					else if (EffectPhase == 4 && YandereAnim[AnimName].time > 2.4f)
					{
						weapon.Spin = true;
						weapon.BloodSpray[0].Stop();
						weapon.BloodSpray[1].Stop();
						EffectPhase++;
					}
					return;
				}
				if (EffectPhase == 0)
				{
					if (YandereAnim[AnimName].time > 0f)
					{
						weapon.Spin = true;
						EffectPhase++;
					}
				}
				else if (EffectPhase == 1)
				{
					if (YandereAnim[AnimName].time > 2f / 3f)
					{
						Yandere.Bloodiness += 20f;
						Yandere.StainWeapon();
						weapon.BloodSpray[0].Play();
						weapon.BloodSpray[1].Play();
						EffectPhase++;
					}
				}
				else if (EffectPhase == 2)
				{
					if (YandereAnim[AnimName].time > 11f / 15f)
					{
						weapon.BloodSpray[0].Stop();
						weapon.BloodSpray[1].Stop();
						EffectPhase++;
					}
				}
				else if (EffectPhase == 3)
				{
					if (YandereAnim[AnimName].time > 3f)
					{
						weapon.BloodSpray[0].Play();
						weapon.BloodSpray[1].Play();
						EffectPhase++;
					}
				}
				else if (EffectPhase == 4 && YandereAnim[AnimName].time > 4.866667f)
				{
					weapon.Spin = false;
					weapon.BloodSpray[0].Stop();
					weapon.BloodSpray[1].Stop();
					EffectPhase++;
				}
			}
			else if (EffectPhase == 0 && YandereAnim[AnimName].time > 1f)
			{
				Yandere.Bloodiness += 20f;
				Yandere.StainWeapon();
				UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.right * 0.2f + weapon.transform.forward * (-1f / 15f), Quaternion.identity);
				EffectPhase++;
			}
		}
		else
		{
			if (weapon.Type != WeaponType.Weight)
			{
				return;
			}
			if (!Stealth)
			{
				switch (sanityType)
				{
				case SanityType.High:
					if (EffectPhase == 0 && YandereAnim[AnimName].time > 2f / 3f)
					{
						Yandere.Bloodiness += 20f;
						Yandere.StainWeapon();
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
						EffectPhase++;
					}
					return;
				case SanityType.Medium:
					if (EffectPhase == 0)
					{
						if (YandereAnim[AnimName].time > 1f)
						{
							Yandere.Bloodiness += 20f;
							Yandere.StainWeapon();
							UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
							EffectPhase++;
						}
					}
					else if (EffectPhase == 1 && YandereAnim[AnimName].time > 2.8333333f)
					{
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
						EffectPhase++;
					}
					return;
				}
				if (EffectPhase == 0)
				{
					if (YandereAnim[AnimName].time > 2.1666667f)
					{
						Yandere.Bloodiness += 20f;
						Yandere.StainWeapon();
						UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
						EffectPhase++;
					}
				}
				else if (EffectPhase == 1 && YandereAnim[AnimName].time > 4.1666665f)
				{
					UnityEngine.Object.Instantiate(BloodEffect, weapon.transform.position + weapon.transform.forward * 0.1f, Quaternion.identity);
					EffectPhase++;
				}
			}
			else
			{
				Yandere.TargetStudent.Ragdoll.NeckSnapped = true;
			}
		}
	}

	private void LoopCheck(WeaponScript weapon)
	{
		if (Input.GetButtonDown("X") && !Yandere.Chased && Yandere.Chasers == 0)
		{
			if (weapon.Type == WeaponType.Knife)
			{
				if (YandereAnim[AnimName].time > 3.5333333f && YandereAnim[AnimName].time < 4.1666665f)
				{
					LoopStart = 106f;
					LoopEnd = 125f;
					LoopPhase = 2;
					Loop = true;
				}
			}
			else if (weapon.Type == WeaponType.Katana)
			{
				if (YandereAnim[AnimName].time > 3.3666666f && YandereAnim[AnimName].time < 3.9f)
				{
					LoopStart = 101f;
					LoopEnd = 117f;
					LoopPhase = 5;
					Loop = true;
				}
			}
			else if (weapon.Type == WeaponType.Bat)
			{
				if (YandereAnim[AnimName].time > 3.7666667f && YandereAnim[AnimName].time < 4.4f)
				{
					LoopStart = 113f;
					LoopEnd = 132f;
					LoopPhase = 2;
					Loop = true;
				}
			}
			else if (weapon.Type == WeaponType.Saw)
			{
				if (YandereAnim[AnimName].time > 3.0333333f && YandereAnim[AnimName].time < 4.5666666f)
				{
					LoopStart = 91f;
					LoopEnd = 137f;
					LoopPhase = 3;
					PingPong = true;
				}
			}
			else if (weapon.Type == WeaponType.Weight && YandereAnim[AnimName].time > 3f && YandereAnim[AnimName].time < 4.5f)
			{
				LoopStart = 90f;
				LoopEnd = 135f;
				LoopPhase = 1;
				Loop = true;
			}
		}
		AudioSource component = weapon.gameObject.GetComponent<AudioSource>();
		if (PingPong)
		{
			if (YandereAnim[AnimName].time > LoopEnd / 30f)
			{
				component.pitch = 1f + UnityEngine.Random.Range(0.1f, -0.1f);
				component.time = LoopStart / 30f;
				VictimAnim[VictimAnimName].speed = -1f;
				YandereAnim[AnimName].speed = -1f;
				EffectPhase = LoopPhase;
				AttackTimer = 0f;
			}
			else if (YandereAnim[AnimName].time < LoopStart / 30f)
			{
				component.pitch = 1f + UnityEngine.Random.Range(0.1f, -0.1f);
				component.time = LoopStart / 30f;
				VictimAnim[VictimAnimName].speed = 1f;
				YandereAnim[AnimName].speed = 1f;
				EffectPhase = LoopPhase;
				AttackTimer = LoopStart / 30f;
				EffectPhase = LoopPhase;
				PingPong = false;
			}
		}
		if (Loop && YandereAnim[AnimName].time > LoopEnd / 30f)
		{
			component.pitch = 1f + UnityEngine.Random.Range(0.1f, -0.1f);
			component.time = LoopStart / 30f;
			VictimAnim[VictimAnimName].time = LoopStart / 30f;
			YandereAnim[AnimName].time = LoopStart / 30f;
			AttackTimer = LoopStart / 30f;
			EffectPhase = LoopPhase;
			Loop = false;
		}
	}

	private void CheckForSpecialCase(WeaponScript weapon)
	{
		if (weapon.WeaponID == 8 && GameGlobals.Paranormal)
		{
			Yandere.TargetStudent.Ragdoll.Sacrifice = true;
			weapon.Effect();
		}
	}
}
