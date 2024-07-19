using UnityEngine;
using UnityEngine.SceneManagement;

public class ArmDetectorScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public DebugMenuScript DebugMenu;

	public JukeboxScript Jukebox;

	public YandereScript Yandere;

	public PoliceScript Police;

	public SkullScript Skull;

	public UILabel DemonSubtitle;

	public UISprite Darkness;

	public Transform[] SpawnPoints;

	public GameObject[] BodyArray;

	public GameObject[] ArmArray;

	public GameObject RiggedAccessory;

	public GameObject BloodProjector;

	public GameObject SmallDarkAura;

	public GameObject DemonDress;

	public GameObject RightFlame;

	public GameObject LeftFlame;

	public GameObject DemonArm;

	public bool SummonEmptyDemon;

	public bool SummonFlameDemon;

	public bool SummonDemon;

	public Mesh FlameDemonMesh;

	public int CorpsesCounted;

	public int ArmsSpawned;

	public int Sacrifices;

	public int Phase = 1;

	public int Bodies;

	public int Arms;

	public float Timer;

	public AudioClip FlameDemonLine;

	public AudioClip FlameActivate;

	public AudioClip DemonMusic;

	public AudioClip DemonLine;

	public AudioClip EmptyDemonLine;

	private void Start()
	{
		DemonDress.SetActive(false);
	}

	private void Update()
	{
		AudioSource component = GetComponent<AudioSource>();
		if (!SummonDemon)
		{
			for (int i = 1; i < ArmArray.Length; i++)
			{
				if (ArmArray[i] != null && ArmArray[i].transform.parent != null)
				{
					ArmArray[i] = null;
					if (i != ArmArray.Length - 1)
					{
						Shuffle(i);
					}
					Arms--;
				}
			}
			if (Arms > 9)
			{
				Yandere.Character.GetComponent<Animation>().CrossFade(Yandere.IdleAnim);
				Yandere.CanMove = false;
				SummonDemon = true;
				component.Play();
				Arms = 0;
			}
		}
		if (!SummonFlameDemon)
		{
			CorpsesCounted = 0;
			Sacrifices = 0;
			int num = 0;
			while (CorpsesCounted < Police.Corpses)
			{
				RagdollScript ragdollScript = Police.CorpseList[num];
				if (ragdollScript != null)
				{
					CorpsesCounted++;
					if (ragdollScript.Burned && ragdollScript.Sacrifice && !ragdollScript.Dragged && !ragdollScript.Carried)
					{
						Sacrifices++;
					}
				}
				num++;
			}
			if (Sacrifices > 4 && !Yandere.Chased && Yandere.Chasers == 0)
			{
				Yandere.Character.GetComponent<Animation>().CrossFade(Yandere.IdleAnim);
				Yandere.CanMove = false;
				SummonFlameDemon = true;
				component.Play();
			}
		}
		if (!SummonEmptyDemon && Bodies > 10 && !Yandere.Chased && Yandere.Chasers == 0)
		{
			Yandere.Character.GetComponent<Animation>().CrossFade(Yandere.IdleAnim);
			Yandere.CanMove = false;
			SummonEmptyDemon = true;
			component.Play();
		}
		if (SummonDemon)
		{
			if (Phase == 1)
			{
				if (ArmArray[1] != null)
				{
					for (int j = 1; j < 11; j++)
					{
						if (ArmArray[j] != null)
						{
							Object.Instantiate(SmallDarkAura, ArmArray[j].transform.position, Quaternion.identity);
							Object.Destroy(ArmArray[j]);
						}
					}
				}
				Timer += Time.deltaTime;
				if (Timer > 1f)
				{
					Timer = 0f;
					Phase++;
				}
			}
			else if (Phase == 2)
			{
				Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
				Jukebox.Volume = Mathf.MoveTowards(Jukebox.Volume, 0f, Time.deltaTime);
				if (Darkness.color.a == 1f)
				{
					SchoolGlobals.SchoolAtmosphere = 0f;
					StudentManager.SetAtmosphere();
					Yandere.transform.eulerAngles = new Vector3(0f, 180f, 0f);
					Yandere.transform.position = new Vector3(12f, 0.1f, 26f);
					DemonSubtitle.text = "...revenge...at last...";
					BloodProjector.SetActive(true);
					DemonSubtitle.color = new Color(DemonSubtitle.color.r, DemonSubtitle.color.g, DemonSubtitle.color.b, 0f);
					Skull.Prompt.Hide();
					Skull.Prompt.enabled = false;
					Skull.enabled = false;
					component.clip = DemonLine;
					component.Play();
					Phase++;
				}
			}
			else if (Phase == 3)
			{
				DemonSubtitle.transform.localPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
				DemonSubtitle.color = new Color(DemonSubtitle.color.r, DemonSubtitle.color.g, DemonSubtitle.color.b, Mathf.MoveTowards(DemonSubtitle.color.a, 1f, Time.deltaTime));
				if (DemonSubtitle.color.a == 1f && Input.GetButtonDown("A"))
				{
					Phase++;
				}
			}
			else if (Phase == 4)
			{
				DemonSubtitle.transform.localPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
				DemonSubtitle.color = new Color(DemonSubtitle.color.r, DemonSubtitle.color.g, DemonSubtitle.color.b, Mathf.MoveTowards(DemonSubtitle.color.a, 0f, Time.deltaTime));
				if (DemonSubtitle.color.a == 0f)
				{
					component.clip = DemonMusic;
					component.loop = true;
					component.Play();
					DemonSubtitle.text = string.Empty;
					Phase++;
				}
			}
			else if (Phase == 5)
			{
				Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime));
				if (Darkness.color.a == 0f)
				{
					Yandere.Character.GetComponent<Animation>().CrossFade("f02_demonSummon_00");
					Phase++;
				}
			}
			else if (Phase == 6)
			{
				Timer += Time.deltaTime;
				if (Timer > (float)ArmsSpawned)
				{
					GameObject gameObject = Object.Instantiate(DemonArm, SpawnPoints[ArmsSpawned].position, Quaternion.identity);
					gameObject.transform.parent = Yandere.transform;
					gameObject.transform.LookAt(Yandere.transform);
					gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y + 180f, gameObject.transform.localEulerAngles.z);
					ArmsSpawned++;
					gameObject.GetComponent<DemonArmScript>().IdleAnim = ((ArmsSpawned % 2 != 1) ? "DemonArmIdle" : "DemonArmIdleOld");
				}
				if (ArmsSpawned == 10)
				{
					Yandere.CanMove = true;
					Yandere.IdleAnim = "f02_demonIdle_00";
					Yandere.WalkAnim = "f02_demonWalk_00";
					Yandere.RunAnim = "f02_demonRun_00";
					Yandere.Demonic = true;
					SummonDemon = false;
				}
			}
		}
		if (SummonFlameDemon)
		{
			if (Phase == 1)
			{
				RagdollScript[] corpseList = Police.CorpseList;
				foreach (RagdollScript ragdollScript2 in corpseList)
				{
					if (ragdollScript2 != null && ragdollScript2.Burned && ragdollScript2.Sacrifice && !ragdollScript2.Dragged && !ragdollScript2.Carried)
					{
						Object.Instantiate(SmallDarkAura, ragdollScript2.Prompt.transform.position, Quaternion.identity);
						Object.Destroy(ragdollScript2.gameObject);
						Yandere.NearBodies--;
						Police.Corpses--;
					}
				}
				Phase++;
			}
			else if (Phase == 2)
			{
				Timer += Time.deltaTime;
				if (Timer > 1f)
				{
					Timer = 0f;
					Phase++;
				}
			}
			else if (Phase == 3)
			{
				Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
				Jukebox.Volume = Mathf.MoveTowards(Jukebox.Volume, 0f, Time.deltaTime);
				if (Darkness.color.a == 1f)
				{
					Yandere.transform.eulerAngles = new Vector3(0f, 180f, 0f);
					Yandere.transform.position = new Vector3(12f, 0.1f, 26f);
					DemonSubtitle.text = "You have proven your worth. Very well. I shall lend you my power.";
					DemonSubtitle.color = new Color(1f, 0f, 0f, 0f);
					Skull.Prompt.Hide();
					Skull.Prompt.enabled = false;
					Skull.enabled = false;
					component.clip = FlameDemonLine;
					component.Play();
					Phase++;
				}
			}
			else if (Phase == 4)
			{
				DemonSubtitle.transform.localPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
				DemonSubtitle.color = new Color(DemonSubtitle.color.r, DemonSubtitle.color.g, DemonSubtitle.color.b, Mathf.MoveTowards(DemonSubtitle.color.a, 1f, Time.deltaTime));
				if (DemonSubtitle.color.a == 1f && Input.GetButtonDown("A"))
				{
					Phase++;
				}
			}
			else if (Phase == 5)
			{
				DemonSubtitle.transform.localPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
				DemonSubtitle.color = new Color(DemonSubtitle.color.r, DemonSubtitle.color.g, DemonSubtitle.color.b, Mathf.MoveTowards(DemonSubtitle.color.a, 0f, Time.deltaTime));
				if (DemonSubtitle.color.a == 0f)
				{
					DemonDress.SetActive(true);
					Yandere.MyRenderer.sharedMesh = FlameDemonMesh;
					RiggedAccessory.SetActive(true);
					Yandere.FlameDemonic = true;
					Yandere.Stance.Current = StanceType.Standing;
					Yandere.Sanity = 100f;
					Yandere.MyRenderer.materials[0].mainTexture = Yandere.FaceTexture;
					Yandere.MyRenderer.materials[1].mainTexture = Yandere.NudePanties;
					Yandere.MyRenderer.materials[2].mainTexture = Yandere.NudePanties;
					DebugMenu.UpdateCensor();
					component.clip = DemonMusic;
					component.loop = true;
					component.Play();
					DemonSubtitle.text = string.Empty;
					Phase++;
				}
			}
			else if (Phase == 6)
			{
				Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime));
				if (Darkness.color.a == 0f)
				{
					Yandere.Character.GetComponent<Animation>().CrossFade("f02_demonSummon_00");
					Phase++;
				}
			}
			else if (Phase == 7)
			{
				Timer += Time.deltaTime;
				if (Timer > 5f)
				{
					component.PlayOneShot(FlameActivate);
					RightFlame.SetActive(true);
					LeftFlame.SetActive(true);
					Phase++;
				}
			}
			else if (Phase == 8)
			{
				Timer += Time.deltaTime;
				if (Timer > 10f)
				{
					Yandere.CanMove = true;
					Yandere.IdleAnim = "f02_demonIdle_00";
					Yandere.WalkAnim = "f02_demonWalk_00";
					Yandere.RunAnim = "f02_demonRun_00";
					SummonFlameDemon = false;
				}
			}
		}
		if (!SummonEmptyDemon)
		{
			return;
		}
		if (Phase == 1)
		{
			if (BodyArray[1] != null)
			{
				for (int l = 1; l < 12; l++)
				{
					if (BodyArray[l] != null)
					{
						Object.Instantiate(SmallDarkAura, BodyArray[l].transform.position, Quaternion.identity);
						Object.Destroy(BodyArray[l]);
					}
				}
			}
			Timer += Time.deltaTime;
			if (Timer > 1f)
			{
				Timer = 0f;
				Phase++;
			}
		}
		else if (Phase == 2)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
			Jukebox.Volume = Mathf.MoveTowards(Jukebox.Volume, 0f, Time.deltaTime);
			if (Darkness.color.a == 1f)
			{
				Yandere.transform.eulerAngles = new Vector3(0f, 180f, 0f);
				Yandere.transform.position = new Vector3(12f, 0.1f, 26f);
				DemonSubtitle.text = "At last...it is time to reclaim our rightful place.";
				BloodProjector.SetActive(true);
				DemonSubtitle.color = new Color(DemonSubtitle.color.r, DemonSubtitle.color.g, DemonSubtitle.color.b, 0f);
				Skull.Prompt.Hide();
				Skull.Prompt.enabled = false;
				Skull.enabled = false;
				component.clip = EmptyDemonLine;
				component.Play();
				Phase++;
			}
		}
		else if (Phase == 3)
		{
			DemonSubtitle.transform.localPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
			DemonSubtitle.color = new Color(DemonSubtitle.color.r, DemonSubtitle.color.g, DemonSubtitle.color.b, Mathf.MoveTowards(DemonSubtitle.color.a, 1f, Time.deltaTime));
			if (DemonSubtitle.color.a == 1f && Input.GetButtonDown("A"))
			{
				Phase++;
			}
		}
		else if (Phase == 4)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
			if (Darkness.color.a == 1f)
			{
				GameGlobals.EmptyDemon = true;
				SceneManager.LoadScene("LoadingScene");
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent == null)
		{
			PickUpScript component = other.gameObject.GetComponent<PickUpScript>();
			if (component != null)
			{
				BodyPartScript bodyPart = component.BodyPart;
				if (bodyPart.Sacrifice && (bodyPart.Type == 3 || bodyPart.Type == 4))
				{
					bool flag = true;
					for (int i = 1; i < 11; i++)
					{
						if (ArmArray[i] == other.gameObject)
						{
							flag = false;
						}
					}
					if (flag)
					{
						Arms++;
						if (Arms < ArmArray.Length)
						{
							ArmArray[Arms] = other.gameObject;
						}
					}
				}
			}
		}
		if (!(other.transform.parent != null) || !(other.transform.parent.parent != null) || !(other.transform.parent.parent.parent != null))
		{
			return;
		}
		StudentScript component2 = other.transform.parent.parent.parent.gameObject.GetComponent<StudentScript>();
		if (!(component2 != null) || !component2.Ragdoll.Sacrifice || !component2.Armband.activeInHierarchy)
		{
			return;
		}
		bool flag2 = true;
		for (int j = 1; j < 11; j++)
		{
			if (BodyArray[j] == other.gameObject)
			{
				flag2 = false;
			}
		}
		if (flag2)
		{
			Bodies++;
			if (Bodies < BodyArray.Length)
			{
				BodyArray[Bodies] = other.gameObject;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		PickUpScript component = other.gameObject.GetComponent<PickUpScript>();
		if (component != null && (bool)component.BodyPart)
		{
			BodyPartScript component2 = other.gameObject.GetComponent<BodyPartScript>();
			if (component2.Sacrifice && (other.gameObject.name == "FemaleRightArm(Clone)" || other.gameObject.name == "FemaleLeftArm(Clone)" || other.gameObject.name == "MaleRightArm(Clone)" || other.gameObject.name == "MaleLeftArm(Clone)" || other.gameObject.name == "SacrificialArm(Clone)"))
			{
				Arms--;
			}
		}
		if (other.transform.parent != null && other.transform.parent.parent != null && other.transform.parent.parent.parent != null)
		{
			StudentScript component3 = other.transform.parent.parent.parent.gameObject.GetComponent<StudentScript>();
			if (component3 != null && component3.Ragdoll.Sacrifice && component3.Armband.activeInHierarchy)
			{
				Bodies--;
			}
		}
	}

	private void Shuffle(int Start)
	{
		for (int i = Start; i < ArmArray.Length - 1; i++)
		{
			ArmArray[i] = ArmArray[i + 1];
		}
	}

	private void ShuffleBodies(int Start)
	{
		for (int i = Start; i < BodyArray.Length - 1; i++)
		{
			BodyArray[i] = BodyArray[i + 1];
		}
	}
}
