using UnityEngine;

public class TallLockerScript : MonoBehaviour
{
	public GameObject[] BloodyClubUniform;

	public GameObject[] BloodyUniform;

	public GameObject[] Schoolwear;

	public bool[] Removed;

	public bool[] Bloody;

	public GameObject CleanUniform;

	public GameObject SteamCloud;

	public StudentManagerScript StudentManager;

	public RivalPhoneScript RivalPhone;

	public StudentScript Student;

	public YandereScript Yandere;

	public PromptScript Prompt;

	public Transform Hinge;

	public bool RemovingClubAttire;

	public bool DropCleanUniform;

	public bool SteamCountdown;

	public bool YandereLocker;

	public bool Swapping;

	public bool Open;

	public float Rotation;

	public float Timer;

	public int Phase = 1;

	private void Start()
	{
		Prompt.HideButton[1] = true;
		Prompt.HideButton[2] = true;
		Prompt.HideButton[3] = true;
	}

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f && !Yandere.Chased && Yandere.Chasers == 0)
		{
			Prompt.Circle[0].fillAmount = 1f;
			if (!Open)
			{
				Open = true;
				if (YandereLocker)
				{
					if (!Yandere.ClubAttire || (Yandere.ClubAttire && Yandere.Bloodiness > 0f))
					{
						if (Yandere.Bloodiness == 0f)
						{
							if (!Bloody[1])
							{
								Prompt.HideButton[1] = false;
							}
							if (!Bloody[2])
							{
								Prompt.HideButton[2] = false;
							}
							if (!Bloody[3])
							{
								Prompt.HideButton[3] = false;
							}
						}
						else if (Yandere.Schoolwear > 0)
						{
							if (!Yandere.ClubAttire)
							{
								Prompt.HideButton[Yandere.Schoolwear] = false;
							}
							else
							{
								Prompt.HideButton[1] = false;
							}
						}
					}
					else
					{
						Prompt.HideButton[1] = true;
						Prompt.HideButton[2] = true;
						Prompt.HideButton[3] = true;
					}
				}
				UpdateSchoolwear();
				Prompt.Label[0].text = "     Close";
			}
			else
			{
				Open = false;
				Prompt.HideButton[1] = true;
				Prompt.HideButton[2] = true;
				Prompt.HideButton[3] = true;
				Prompt.Label[0].text = "     Open";
			}
		}
		if (!Open)
		{
			Rotation = Mathf.Lerp(Rotation, 0f, Time.deltaTime * 10f);
			Prompt.HideButton[1] = true;
			Prompt.HideButton[2] = true;
			Prompt.HideButton[3] = true;
		}
		else
		{
			Rotation = Mathf.Lerp(Rotation, -180f, Time.deltaTime * 10f);
			if (Prompt.Circle[1].fillAmount == 0f)
			{
				Yandere.EmptyHands();
				if (Yandere.ClubAttire)
				{
					RemovingClubAttire = true;
				}
				Yandere.PreviousSchoolwear = Yandere.Schoolwear;
				if (Yandere.Schoolwear == 1)
				{
					Yandere.Schoolwear = 0;
					if (!Removed[1])
					{
						if (Yandere.Bloodiness == 0f)
						{
							DropCleanUniform = true;
						}
					}
					else
					{
						Removed[1] = false;
					}
				}
				else
				{
					Yandere.Schoolwear = 1;
					Removed[1] = true;
				}
				SpawnSteam();
			}
			else if (Prompt.Circle[2].fillAmount == 0f)
			{
				Yandere.EmptyHands();
				if (Yandere.ClubAttire)
				{
					RemovingClubAttire = true;
				}
				Yandere.PreviousSchoolwear = Yandere.Schoolwear;
				if (Yandere.Schoolwear == 1 && !Removed[1])
				{
					DropCleanUniform = true;
				}
				if (Yandere.Schoolwear == 2)
				{
					Yandere.Schoolwear = 0;
					Removed[2] = false;
				}
				else
				{
					Yandere.Schoolwear = 2;
					Removed[2] = true;
				}
				SpawnSteam();
			}
			else if (Prompt.Circle[3].fillAmount == 0f)
			{
				Yandere.EmptyHands();
				if (Yandere.ClubAttire)
				{
					RemovingClubAttire = true;
				}
				Yandere.PreviousSchoolwear = Yandere.Schoolwear;
				if (Yandere.Schoolwear == 1 && !Removed[1])
				{
					DropCleanUniform = true;
				}
				if (Yandere.Schoolwear == 3)
				{
					Yandere.Schoolwear = 0;
					Removed[3] = false;
				}
				else
				{
					Yandere.Schoolwear = 3;
					Removed[3] = true;
				}
				SpawnSteam();
			}
		}
		Hinge.localEulerAngles = new Vector3(0f, Rotation, 0f);
		if (!SteamCountdown)
		{
			return;
		}
		Timer += Time.deltaTime;
		if (Phase == 1)
		{
			if (!(Timer > 1.5f))
			{
				return;
			}
			if (YandereLocker)
			{
				if (Yandere.Gloved)
				{
					Yandere.Gloves.GetComponent<PickUpScript>().MyRigidbody.isKinematic = false;
					Yandere.Gloves.transform.localPosition = new Vector3(0f, 1f, -1f);
					Yandere.Gloves.transform.parent = null;
					Yandere.GloveAttacher.newRenderer.enabled = false;
					Yandere.Gloves.gameObject.SetActive(true);
					Yandere.Gloved = false;
					Yandere.Gloves = null;
				}
				Yandere.ChangeSchoolwear();
				if (Yandere.Bloodiness > 0f)
				{
					PickUpScript pickUpScript = null;
					if (RemovingClubAttire)
					{
						GameObject gameObject = Object.Instantiate(BloodyClubUniform[(int)ClubGlobals.Club], Yandere.transform.position + Vector3.forward * 0.5f + Vector3.up, Quaternion.identity);
						pickUpScript = gameObject.GetComponent<PickUpScript>();
						StudentManager.ChangingBooths[(int)ClubGlobals.Club].CannotChange = true;
						StudentManager.ChangingBooths[(int)ClubGlobals.Club].CheckYandereClub();
						Prompt.HideButton[1] = true;
						Prompt.HideButton[2] = true;
						Prompt.HideButton[3] = true;
						RemovingClubAttire = false;
					}
					else
					{
						GameObject gameObject2 = Object.Instantiate(BloodyUniform[Yandere.PreviousSchoolwear], Yandere.transform.position + Vector3.forward * 0.5f + Vector3.up, Quaternion.identity);
						pickUpScript = gameObject2.GetComponent<PickUpScript>();
						Prompt.HideButton[Yandere.PreviousSchoolwear] = true;
						Bloody[Yandere.PreviousSchoolwear] = true;
					}
					if (Yandere.RedPaint)
					{
						pickUpScript.RedPaint = true;
					}
				}
			}
			else
			{
				if (Student.Schoolwear == 0 && Student.StudentID == StudentManager.RivalID)
				{
					RivalPhone.gameObject.SetActive(true);
					RivalPhone.MyRenderer.material.mainTexture = Student.SmartPhone.GetComponent<Renderer>().material.mainTexture;
				}
				Student.ChangeSchoolwear();
			}
			UpdateSchoolwear();
			Phase++;
		}
		else if (Timer > 3f)
		{
			if (!YandereLocker)
			{
				Student.BathePhase++;
			}
			SteamCountdown = false;
			Phase = 1;
			Timer = 0f;
		}
	}

	public void SpawnSteam()
	{
		SteamCountdown = true;
		if (YandereLocker)
		{
			Object.Instantiate(SteamCloud, Yandere.transform.position + Vector3.up * 0.81f, Quaternion.identity);
			Yandere.Character.GetComponent<Animation>().CrossFade("f02_stripping_00");
			Yandere.Stripping = true;
			Yandere.CanMove = false;
		}
		else
		{
			GameObject gameObject = Object.Instantiate(SteamCloud, Student.transform.position + Vector3.up * 0.81f, Quaternion.identity);
			gameObject.transform.parent = Student.transform;
			Student.CharacterAnimation.CrossFade(Student.StripAnim);
			Student.Pathfinding.canSearch = false;
			Student.Pathfinding.canMove = false;
		}
	}

	public void UpdateSchoolwear()
	{
		if (DropCleanUniform)
		{
			Object.Instantiate(CleanUniform, Yandere.transform.position + Vector3.forward * -0.5f + Vector3.up, Quaternion.identity);
			DropCleanUniform = false;
		}
		if (!Bloody[1])
		{
			Schoolwear[1].SetActive(true);
		}
		if (!Bloody[2])
		{
			Schoolwear[2].SetActive(true);
		}
		if (!Bloody[3])
		{
			Schoolwear[3].SetActive(true);
		}
		Prompt.Label[1].text = "     School Uniform";
		Prompt.Label[2].text = "     School Swimsuit";
		Prompt.Label[3].text = "     Gym Uniform";
		if (YandereLocker)
		{
			if (!Yandere.ClubAttire)
			{
				if (Yandere.Schoolwear > 0)
				{
					Prompt.Label[Yandere.Schoolwear].text = "     Nude";
					if (Removed[Yandere.Schoolwear])
					{
						Schoolwear[Yandere.Schoolwear].SetActive(false);
					}
				}
			}
			else
			{
				Prompt.Label[1].text = "     Nude";
			}
		}
		else if (Student != null && Student.Schoolwear > 0)
		{
			Prompt.HideButton[Student.Schoolwear] = true;
			Schoolwear[Student.Schoolwear].SetActive(false);
			Student.Indoors = true;
		}
	}

	public void UpdateButtons()
	{
		if (!Yandere.ClubAttire || (Yandere.ClubAttire && Yandere.Bloodiness > 0f))
		{
			if (!Open)
			{
				return;
			}
			if (Yandere.Bloodiness > 0f)
			{
				Prompt.HideButton[1] = true;
				Prompt.HideButton[2] = true;
				Prompt.HideButton[3] = true;
				if (Yandere.Schoolwear > 0 && !Yandere.ClubAttire)
				{
					Prompt.HideButton[Yandere.Schoolwear] = false;
				}
				if (Yandere.ClubAttire)
				{
					Debug.Log("Don't hide Prompt 1!");
					Prompt.HideButton[1] = false;
				}
			}
			else
			{
				if (!Bloody[1])
				{
					Prompt.HideButton[1] = false;
				}
				if (!Bloody[2])
				{
					Prompt.HideButton[2] = false;
				}
				if (!Bloody[3])
				{
					Prompt.HideButton[3] = false;
				}
			}
		}
		else
		{
			Prompt.HideButton[1] = true;
			Prompt.HideButton[2] = true;
			Prompt.HideButton[3] = true;
		}
	}
}
