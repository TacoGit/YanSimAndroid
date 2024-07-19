using UnityEngine;

public class WoodChipperScript : MonoBehaviour
{
	public ParticleSystem BloodSpray;

	public PromptScript BucketPrompt;

	public YandereScript Yandere;

	public PickUpScript Bucket;

	public PromptScript Prompt;

	public AudioClip CloseAudio;

	public AudioClip ShredAudio;

	public AudioClip OpenAudio;

	public Transform BucketPoint;

	public Transform DumpPoint;

	public Transform Lid;

	public float Rotation;

	public float Timer;

	public bool Shredding;

	public bool Occupied;

	public bool Open;

	public int VictimID;

	public int Victims;

	public int ID;

	public int[] VictimList;

	private void Update()
	{
		if (Yandere.PickUp != null)
		{
			if (Yandere.PickUp.Bucket != null)
			{
				if (!Yandere.PickUp.Bucket.Full)
				{
					BucketPrompt.HideButton[0] = false;
					if (BucketPrompt.Circle[0].fillAmount == 0f)
					{
						Bucket = Yandere.PickUp;
						Yandere.EmptyHands();
						Bucket.transform.eulerAngles = BucketPoint.eulerAngles;
						Bucket.transform.position = BucketPoint.position;
						Bucket.GetComponent<Rigidbody>().useGravity = false;
						Bucket.MyCollider.enabled = false;
					}
				}
				else
				{
					BucketPrompt.HideButton[0] = true;
				}
			}
			else
			{
				BucketPrompt.HideButton[0] = true;
			}
		}
		else
		{
			BucketPrompt.HideButton[0] = true;
		}
		AudioSource component = GetComponent<AudioSource>();
		if (!Open)
		{
			Rotation = Mathf.MoveTowards(Rotation, 0f, Time.deltaTime * 360f);
			if (Rotation > -36f)
			{
				if (Rotation < 0f)
				{
					component.clip = CloseAudio;
					component.Play();
				}
				Rotation = 0f;
			}
			Lid.transform.localEulerAngles = new Vector3(Rotation, Lid.transform.localEulerAngles.y, Lid.transform.localEulerAngles.z);
		}
		else
		{
			if (Lid.transform.localEulerAngles.x == 0f)
			{
				component.clip = OpenAudio;
				component.Play();
			}
			Rotation = Mathf.MoveTowards(Rotation, -90f, Time.deltaTime * 360f);
			Lid.transform.localEulerAngles = new Vector3(Rotation, Lid.transform.localEulerAngles.y, Lid.transform.localEulerAngles.z);
		}
		if (!BloodSpray.isPlaying)
		{
			if (!Occupied)
			{
				if (Yandere.Ragdoll == null)
				{
					Prompt.HideButton[3] = true;
				}
				else
				{
					Prompt.HideButton[3] = false;
				}
			}
			else if (Bucket == null)
			{
				Prompt.HideButton[0] = true;
			}
			else if (Bucket.Bucket.Full)
			{
				Prompt.HideButton[0] = true;
			}
			else
			{
				Prompt.HideButton[0] = false;
			}
		}
		if (Prompt.Circle[3].fillAmount == 0f)
		{
			Time.timeScale = 1f;
			if (Yandere.Ragdoll != null)
			{
				if (!Yandere.Carrying)
				{
					Yandere.Character.GetComponent<Animation>().CrossFade("f02_dragIdle_00");
				}
				else
				{
					Yandere.Character.GetComponent<Animation>().CrossFade("f02_carryIdleA_00");
				}
				Yandere.YandereVision = false;
				Yandere.Chipping = true;
				Yandere.CanMove = false;
				Victims++;
				VictimList[Victims] = Yandere.Ragdoll.GetComponent<RagdollScript>().StudentID;
				Open = true;
			}
		}
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			component.clip = ShredAudio;
			component.Play();
			Prompt.HideButton[3] = false;
			Prompt.HideButton[0] = true;
			Prompt.Hide();
			Prompt.enabled = false;
			Yandere.Police.Corpses--;
			if (Yandere.Police.SuicideScene && Yandere.Police.Corpses == 1)
			{
				Yandere.Police.MurderScene = false;
			}
			if (Yandere.Police.Corpses == 0)
			{
				Yandere.Police.MurderScene = false;
			}
			Shredding = true;
			Yandere.StudentManager.Students[VictimID].Ragdoll.Disposed = true;
		}
		if (!Shredding)
		{
			return;
		}
		if (Bucket != null)
		{
			Bucket.Bucket.UpdateAppearance = true;
		}
		Timer += Time.deltaTime;
		if (Timer >= 10f)
		{
			Prompt.enabled = true;
			Shredding = false;
			Occupied = false;
			Timer = 0f;
		}
		else if (Timer >= 9f)
		{
			if (Bucket != null)
			{
				Bucket.MyCollider.enabled = true;
				Bucket.Bucket.FillSpeed = 1f;
				Bucket = null;
				BloodSpray.Stop();
			}
		}
		else if (Timer >= 0.33333f && !Bucket.Bucket.Full)
		{
			BloodSpray.GetComponent<AudioSource>().Play();
			BloodSpray.Play();
			Bucket.Bucket.Bloodiness = 100f;
			Bucket.Bucket.FillSpeed = 0.05f;
			Bucket.Bucket.Full = true;
		}
	}

	public void SetVictimsMissing()
	{
		int[] victimList = VictimList;
		foreach (int studentID in victimList)
		{
			StudentGlobals.SetStudentMissing(studentID, true);
		}
	}
}
