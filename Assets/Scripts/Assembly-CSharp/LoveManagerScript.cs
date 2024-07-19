using UnityEngine;

public class LoveManagerScript : MonoBehaviour
{
	public ConfessionSceneScript ConfessionScene;

	public StudentManagerScript StudentManager;

	public GameObject ConfessionManager;

	public YandereScript Yandere;

	public ClockScript Clock;

	public StudentScript Follower;

	public StudentScript Suitor;

	public StudentScript Rival;

	public Transform[] Targets;

	public Transform MythHill;

	public int SuitorProgress;

	public int TotalTargets;

	public int Phase = 1;

	public int ID;

	public float AngleLimit;

	public bool HoldingHands;

	public bool RivalWaiting;

	public bool LeftNote;

	public bool Courted;

	private void Start()
	{
		SuitorProgress = DatingGlobals.SuitorProgress;
	}

	private void LateUpdate()
	{
		if (Follower != null && Follower.Alive && !Follower.InCouple)
		{
			for (ID = 0; ID < TotalTargets; ID++)
			{
				Transform transform = Targets[ID];
				if (transform != null && Follower.transform.position.y > transform.position.y - 2f && Follower.transform.position.y < transform.position.y + 2f && Vector3.Distance(Follower.transform.position, new Vector3(transform.position.x, Follower.transform.position.y, transform.position.z)) < 2.5f)
				{
					float f = Vector3.Angle(Follower.transform.forward, Follower.transform.position - new Vector3(transform.position.x, Follower.transform.position.y, transform.position.z));
					if (Mathf.Abs(f) > AngleLimit)
					{
						if (!Follower.Gush)
						{
							Follower.Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", 1f);
							Follower.GushTarget = transform;
							ParticleSystem.EmissionModule emission = Follower.Hearts.emission;
							emission.enabled = true;
							emission.rateOverTime = 5f;
							Follower.Hearts.Play();
							Follower.Gush = true;
						}
					}
					else
					{
						Follower.Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", 0f);
						ParticleSystem.EmissionModule emission2 = Follower.Hearts.emission;
						emission2.enabled = false;
						Follower.Gush = false;
					}
				}
			}
		}
		if (LeftNote)
		{
			Rival = StudentManager.Students[30];
			Suitor = StudentManager.Students[28];
			if (StudentManager.Students[StudentManager.RivalID] != null)
			{
				Rival = StudentManager.Students[StudentManager.RivalID];
				Suitor = StudentManager.Students[1];
			}
			if (Rival != null && Suitor != null && Rival.Alive && Suitor.Alive && !Rival.Dying && !Suitor.Dying && Rival.ConfessPhase == 5 && Suitor.ConfessPhase == 3)
			{
				float num = Vector3.Distance(Yandere.transform.position, MythHill.position);
				if (!Yandere.Chased && Yandere.Chasers == 0 && num > 10f && num < 25f)
				{
					Yandere.Character.GetComponent<Animation>().CrossFade(Yandere.IdleAnim);
					Yandere.RPGCamera.enabled = false;
					Yandere.CanMove = false;
					Suitor.enabled = false;
					Rival.enabled = false;
					if (StudentManager.Students[StudentManager.RivalID] != null)
					{
						ConfessionManager.SetActive(true);
					}
					else
					{
						ConfessionScene.enabled = true;
					}
					Clock.StopTime = true;
					LeftNote = false;
				}
			}
		}
		if (HoldingHands)
		{
			if (Rival == null)
			{
				Rival = StudentManager.Students[30];
			}
			if (Suitor == null)
			{
				Suitor = StudentManager.Students[28];
			}
			Rival.MyController.Move(base.transform.forward * Time.deltaTime);
			Suitor.transform.position = new Vector3(Rival.transform.position.x - 0.5f, Rival.transform.position.y, Rival.transform.position.z);
			if (Rival.transform.position.z > -50f)
			{
				Suitor.MyController.radius = 0.12f;
				Suitor.enabled = true;
				Suitor.Cosmetic.MyRenderer.materials[Suitor.Cosmetic.FaceID].SetFloat("_BlendAmount", 0f);
				ParticleSystem.EmissionModule emission3 = Suitor.Hearts.emission;
				emission3.enabled = false;
				Rival.MyController.radius = 0.12f;
				Rival.enabled = true;
				Rival.Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", 0f);
				ParticleSystem.EmissionModule emission4 = Rival.Hearts.emission;
				emission4.enabled = false;
				Suitor.HoldingHands = false;
				Rival.HoldingHands = false;
				HoldingHands = false;
			}
		}
	}

	public void CoupleCheck()
	{
		if (SuitorProgress == 2)
		{
			Rival = StudentManager.Students[30];
			Suitor = StudentManager.Students[28];
			if (Rival != null && Suitor != null)
			{
				Suitor.CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
				Rival.CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
				Suitor.Character.GetComponent<Animation>().enabled = true;
				Rival.Character.GetComponent<Animation>().enabled = true;
				Suitor.Character.GetComponent<Animation>().Play("walkHands_00");
				Suitor.transform.eulerAngles = Vector3.zero;
				Suitor.transform.position = new Vector3(-0.25f, 0f, -90f);
				Suitor.Pathfinding.canSearch = false;
				Suitor.Pathfinding.canMove = false;
				Suitor.MyController.radius = 0f;
				Suitor.enabled = false;
				Rival.Character.GetComponent<Animation>().Play("f02_walkHands_00");
				Rival.transform.eulerAngles = Vector3.zero;
				Rival.transform.position = new Vector3(0.25f, 0f, -90f);
				Rival.Pathfinding.canSearch = false;
				Rival.Pathfinding.canMove = false;
				Rival.MyController.radius = 0f;
				Rival.enabled = false;
				Physics.SyncTransforms();
				Suitor.Cosmetic.MyRenderer.materials[Suitor.Cosmetic.FaceID].SetFloat("_BlendAmount", 1f);
				ParticleSystem.EmissionModule emission = Suitor.Hearts.emission;
				emission.enabled = true;
				emission.rateOverTime = 5f;
				Suitor.Hearts.Play();
				Rival.Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", 1f);
				ParticleSystem.EmissionModule emission2 = Rival.Hearts.emission;
				emission2.enabled = true;
				emission2.rateOverTime = 5f;
				Rival.Hearts.Play();
				Suitor.HoldingHands = true;
				Rival.HoldingHands = true;
				Suitor.CoupleID = 30;
				Rival.CoupleID = 28;
				HoldingHands = true;
			}
		}
	}
}
