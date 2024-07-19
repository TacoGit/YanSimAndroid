using UnityEngine;

public class HeadmasterScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public HeartbrokenScript Heartbroken;

	public YandereScript Yandere;

	public JukeboxScript Jukebox;

	public AudioClip[] HeadmasterSpeechClips;

	public AudioClip[] HeadmasterThreatClips;

	public AudioClip[] HeadmasterBoxClips;

	public AudioClip HeadmasterRelaxClip;

	public AudioClip HeadmasterAttackClip;

	public AudioClip HeadmasterCrypticClip;

	public AudioClip HeadmasterShockClip;

	public AudioClip HeadmasterPatienceClip;

	public AudioClip HeadmasterCorpseClip;

	public AudioClip HeadmasterWeaponClip;

	public AudioClip Crumple;

	public AudioClip StandUp;

	public AudioClip SitDown;

	public readonly string[] HeadmasterSpeechText = new string[6]
	{
		string.Empty,
		"Ahh...! It's...it's you!",
		"No, that would be impossible...you must be...her daughter...",
		"I'll tolerate you in my school, but not in my office.",
		"Leave at once.",
		"There is nothing for you to achieve here. Just. Get. Out."
	};

	public readonly string[] HeadmasterThreatText = new string[6]
	{
		string.Empty,
		"Not another step!",
		"You're up to no good! I know it!",
		"I'm not going to let you harm me!",
		"I'll use self-defense if I deem it necessary!",
		"This is your final warning. Get out of here...or else."
	};

	public readonly string[] HeadmasterBoxText = new string[6]
	{
		string.Empty,
		"What...in...blazes are you doing?",
		"Are you trying to re-enact something you saw in a video game?",
		"Ugh, do you really think such a stupid ploy is going to work?",
		"I know who you are. It's obvious. You're not fooling anyone.",
		"I don't have time for this tomfoolery. Leave at once!"
	};

	public readonly string HeadmasterRelaxText = "Hmm...a wise decision.";

	public readonly string HeadmasterAttackText = "You asked for it!";

	public readonly string HeadmasterCrypticText = "Mr. Saikou...the deal is off.";

	public readonly string HeadmasterWeaponText = "How dare you raise a weapon in my office!";

	public readonly string HeadmasterPatienceText = "Enough of this nonsense!";

	public readonly string HeadmasterCorpseText = "You...you murderer!";

	public UILabel HeadmasterSubtitle;

	public Animation MyAnimation;

	public AudioSource MyAudio;

	public GameObject LightningEffect;

	public GameObject Tazer;

	public Transform TazerEffectTarget;

	public Transform CardboardBox;

	public Transform Chair;

	public Quaternion targetRotation;

	public float PatienceTimer;

	public float ScratchTimer;

	public float SpeechTimer;

	public float ThreatTimer;

	public float Distance;

	public int Patience = 10;

	public int ThreatID;

	public int VoiceID;

	public int BoxID;

	public bool PlayedStandSound;

	public bool PlayedSitSound;

	public bool LostPatience;

	public bool Threatened;

	public bool Relaxing;

	public bool Shooting;

	public bool Aiming;

	public Vector3 LookAtTarget;

	public bool LookAtPlayer;

	public Transform Default;

	public Transform Head;

	private void Start()
	{
		MyAnimation["HeadmasterRaiseTazer"].speed = 2f;
		Tazer.SetActive(false);
	}

	private void Update()
	{
		if (Yandere.transform.position.y > base.transform.position.y - 1f && Yandere.transform.position.y < base.transform.position.y + 1f && Yandere.transform.position.x < 6f && Yandere.transform.position.x > -6f)
		{
			Distance = Vector3.Distance(base.transform.position, Yandere.transform.position);
			if (Shooting)
			{
				targetRotation = Quaternion.LookRotation(base.transform.position - Yandere.transform.position);
				Yandere.transform.rotation = Quaternion.Slerp(Yandere.transform.rotation, targetRotation, Time.deltaTime * 10f);
				AimWeaponAtYandere();
				AimBodyAtYandere();
			}
			else if ((double)Distance < 1.2)
			{
				AimBodyAtYandere();
				if (Yandere.CanMove && !Shooting)
				{
					Shoot();
				}
			}
			else if ((double)Distance < 2.8)
			{
				PlayedSitSound = false;
				if (!StudentManager.Clock.StopTime)
				{
					PatienceTimer -= Time.deltaTime;
				}
				if (PatienceTimer < 0f)
				{
					LostPatience = true;
					PatienceTimer = 60f;
					Patience = 0;
					Shoot();
				}
				if (!LostPatience)
				{
					LostPatience = true;
					Patience--;
					if (Patience < 1 && !Shooting)
					{
						Shoot();
					}
				}
				AimBodyAtYandere();
				Threatened = true;
				AimWeaponAtYandere();
				ThreatTimer = Mathf.MoveTowards(ThreatTimer, 0f, Time.deltaTime);
				if (ThreatTimer == 0f)
				{
					ThreatID++;
					if (ThreatID < 5)
					{
						HeadmasterSubtitle.text = HeadmasterThreatText[ThreatID];
						MyAudio.clip = HeadmasterThreatClips[ThreatID];
						MyAudio.Play();
						ThreatTimer = HeadmasterThreatClips[ThreatID].length + 1f;
					}
				}
				CheckBehavior();
			}
			else if (Distance < 10f)
			{
				PlayedStandSound = false;
				LostPatience = false;
				targetRotation = Quaternion.LookRotation(new Vector3(0f, 8f, 0f) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, Time.deltaTime * 10f);
				Chair.localPosition = Vector3.Lerp(Chair.localPosition, new Vector3(Chair.localPosition.x, Chair.localPosition.y, -4.66666f), Time.deltaTime * 1f);
				LookAtPlayer = true;
				if (!Threatened)
				{
					MyAnimation.CrossFade("HeadmasterAttention", 1f);
					ScratchTimer = 0f;
					SpeechTimer = Mathf.MoveTowards(SpeechTimer, 0f, Time.deltaTime);
					if (SpeechTimer == 0f)
					{
						if (CardboardBox.parent == null && Yandere.Mask == null)
						{
							VoiceID++;
							if (VoiceID < 6)
							{
								HeadmasterSubtitle.text = HeadmasterSpeechText[VoiceID];
								MyAudio.clip = HeadmasterSpeechClips[VoiceID];
								MyAudio.Play();
								SpeechTimer = HeadmasterSpeechClips[VoiceID].length + 1f;
							}
						}
						else
						{
							BoxID++;
							if (BoxID < 6)
							{
								HeadmasterSubtitle.text = HeadmasterBoxText[BoxID];
								MyAudio.clip = HeadmasterBoxClips[BoxID];
								MyAudio.Play();
								SpeechTimer = HeadmasterBoxClips[BoxID].length + 1f;
							}
						}
					}
				}
				else if (!Relaxing)
				{
					HeadmasterSubtitle.text = HeadmasterRelaxText;
					MyAudio.clip = HeadmasterRelaxClip;
					MyAudio.Play();
					Relaxing = true;
				}
				else
				{
					if (!PlayedSitSound)
					{
						AudioSource.PlayClipAtPoint(SitDown, base.transform.position);
						PlayedSitSound = true;
					}
					MyAnimation.CrossFade("HeadmasterLowerTazer");
					Aiming = false;
					if ((double)MyAnimation["HeadmasterLowerTazer"].time > 1.33333)
					{
						Tazer.SetActive(false);
					}
					if (MyAnimation["HeadmasterLowerTazer"].time > MyAnimation["HeadmasterLowerTazer"].length)
					{
						Threatened = false;
						Relaxing = false;
					}
				}
				CheckBehavior();
			}
			else
			{
				if (LookAtPlayer)
				{
					MyAnimation.CrossFade("HeadmasterType");
					LookAtPlayer = false;
					Threatened = false;
					Relaxing = false;
					Aiming = false;
				}
				ScratchTimer += Time.deltaTime;
				if (ScratchTimer > 10f)
				{
					MyAnimation.CrossFade("HeadmasterScratch");
					if (MyAnimation["HeadmasterScratch"].time > MyAnimation["HeadmasterScratch"].length)
					{
						MyAnimation.CrossFade("HeadmasterType");
						ScratchTimer = 0f;
					}
				}
			}
			if (!MyAudio.isPlaying)
			{
				HeadmasterSubtitle.text = string.Empty;
				if (Shooting)
				{
					Taze();
				}
			}
			if (Yandere.Attacked && Yandere.Character.GetComponent<Animation>()["f02_swingB_00"].time >= Yandere.Character.GetComponent<Animation>()["f02_swingB_00"].length * 0.85f)
			{
				MyAudio.clip = Crumple;
				MyAudio.Play();
				base.enabled = false;
			}
		}
		else
		{
			HeadmasterSubtitle.text = string.Empty;
		}
	}

	private void LateUpdate()
	{
		LookAtTarget = Vector3.Lerp(LookAtTarget, (!LookAtPlayer) ? Default.position : Yandere.Head.position, Time.deltaTime * 10f);
		Head.LookAt(LookAtTarget);
	}

	private void AimBodyAtYandere()
	{
		targetRotation = Quaternion.LookRotation(Yandere.transform.position - base.transform.position);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, Time.deltaTime * 5f);
		Chair.localPosition = Vector3.Lerp(Chair.localPosition, new Vector3(Chair.localPosition.x, Chair.localPosition.y, -5.2f), Time.deltaTime * 1f);
	}

	private void AimWeaponAtYandere()
	{
		if (!Aiming)
		{
			MyAnimation.CrossFade("HeadmasterRaiseTazer");
			if (!PlayedStandSound)
			{
				AudioSource.PlayClipAtPoint(StandUp, base.transform.position);
				PlayedStandSound = true;
			}
			if ((double)MyAnimation["HeadmasterRaiseTazer"].time > 1.166666)
			{
				Tazer.SetActive(true);
				Aiming = true;
			}
		}
		else if (MyAnimation["HeadmasterRaiseTazer"].time > MyAnimation["HeadmasterRaiseTazer"].length)
		{
			MyAnimation.CrossFade("HeadmasterAimTazer");
		}
	}

	public void Shoot()
	{
		StudentManager.YandereDying = true;
		Yandere.StopAiming();
		Yandere.StopLaughing();
		Yandere.CharacterAnimation.CrossFade("f02_readyToFight_00");
		if (Patience < 1)
		{
			HeadmasterSubtitle.text = HeadmasterPatienceText;
			MyAudio.clip = HeadmasterPatienceClip;
		}
		else if (Yandere.Armed)
		{
			HeadmasterSubtitle.text = HeadmasterWeaponText;
			MyAudio.clip = HeadmasterWeaponClip;
		}
		else if (Yandere.Carrying || Yandere.Dragging || (Yandere.PickUp != null && (bool)Yandere.PickUp.BodyPart))
		{
			HeadmasterSubtitle.text = HeadmasterCorpseText;
			MyAudio.clip = HeadmasterCorpseClip;
		}
		else
		{
			HeadmasterSubtitle.text = HeadmasterAttackText;
			MyAudio.clip = HeadmasterAttackClip;
		}
		StudentManager.StopMoving();
		Yandere.EmptyHands();
		Yandere.CanMove = false;
		MyAudio.Play();
		Shooting = true;
	}

	private void CheckBehavior()
	{
		if (!Yandere.CanMove)
		{
			return;
		}
		if (Yandere.Chased || Yandere.Chasers > 0)
		{
			if (!Shooting)
			{
				Shoot();
			}
		}
		else if (Yandere.Armed)
		{
			if (!Shooting)
			{
				Shoot();
			}
		}
		else if ((Yandere.Carrying || Yandere.Dragging || (Yandere.PickUp != null && (bool)Yandere.PickUp.BodyPart)) && !Shooting)
		{
			Shoot();
		}
	}

	public void Taze()
	{
		if (Yandere.CanMove)
		{
			StudentManager.YandereDying = true;
			Yandere.StopAiming();
			Yandere.StopLaughing();
			StudentManager.StopMoving();
			Yandere.EmptyHands();
			Yandere.CanMove = false;
		}
		Object.Instantiate(LightningEffect, TazerEffectTarget.position, Quaternion.identity);
		Object.Instantiate(LightningEffect, Yandere.Spine[3].position, Quaternion.identity);
		MyAudio.clip = HeadmasterShockClip;
		MyAudio.Play();
		Yandere.CharacterAnimation.CrossFade("f02_swingB_00");
		Yandere.CharacterAnimation["f02_swingB_00"].time = 0.5f;
		Yandere.RPGCamera.enabled = false;
		Yandere.Attacked = true;
		Heartbroken.Headmaster = true;
		Jukebox.Volume = 0f;
		Shooting = false;
	}
}
