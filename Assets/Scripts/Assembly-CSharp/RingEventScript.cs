using UnityEngine;

public class RingEventScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public YandereScript Yandere;

	public ClockScript Clock;

	public StudentScript EventStudent;

	public UILabel EventSubtitle;

	public AudioClip[] EventClip;

	public string[] EventSpeech;

	public string[] EventAnim;

	public GameObject VoiceClip;

	public bool EventActive;

	public bool EventOver;

	public float EventTime = 13.1f;

	public int EventPhase = 1;

	public Vector3 OriginalPosition;

	public Vector3 HoldingPosition;

	public Vector3 HoldingRotation;

	public float CurrentClipLength;

	public float Timer;

	public PromptScript RingPrompt;

	public Collider RingCollider;

	private void Start()
	{
		HoldingPosition = new Vector3(0.0075f, -0.0355f, 0.0175f);
		HoldingRotation = new Vector3(15f, -70f, -135f);
	}

	private void Update()
	{
		if (!Clock.StopTime && !EventActive && Clock.HourTime > EventTime)
		{
			EventStudent = StudentManager.Students[2];
			if (EventStudent != null && !EventStudent.Distracted && !EventStudent.Talking)
			{
				if (!EventStudent.WitnessedMurder && !EventStudent.Bullied)
				{
					if (EventStudent.Cosmetic.FemaleAccessories[3].activeInHierarchy)
					{
						if (SchemeGlobals.GetSchemeStage(2) < 100)
						{
							RingPrompt = EventStudent.Cosmetic.FemaleAccessories[3].GetComponent<PromptScript>();
							RingCollider = EventStudent.Cosmetic.FemaleAccessories[3].GetComponent<BoxCollider>();
							OriginalPosition = EventStudent.Cosmetic.FemaleAccessories[3].transform.localPosition;
							EventStudent.CurrentDestination = EventStudent.Destinations[EventStudent.Phase];
							EventStudent.Pathfinding.target = EventStudent.Destinations[EventStudent.Phase];
							EventStudent.Obstacle.checkTime = 99f;
							EventStudent.InEvent = true;
							EventStudent.Private = true;
							EventStudent.Prompt.Hide();
							EventActive = true;
							if (EventStudent.Following)
							{
								EventStudent.Pathfinding.canMove = true;
								EventStudent.Pathfinding.speed = 1f;
								EventStudent.Following = false;
								EventStudent.Routine = true;
								Yandere.Followers--;
								EventStudent.Subtitle.UpdateLabel(SubtitleType.StopFollowApology, 0, 3f);
								EventStudent.Prompt.Label[0].text = "     Talk";
							}
						}
						else
						{
							base.enabled = false;
						}
					}
					else
					{
						base.enabled = false;
					}
				}
				else
				{
					base.enabled = false;
				}
			}
		}
		if (!EventActive)
		{
			return;
		}
		if (EventStudent.DistanceToDestination < 0.5f)
		{
			EventStudent.Pathfinding.canSearch = false;
			EventStudent.Pathfinding.canMove = false;
		}
		if (Clock.HourTime > EventTime + 0.5f || EventStudent.WitnessedMurder || EventStudent.Splashed || EventStudent.Alarmed || EventStudent.Dying || !EventStudent.Alive)
		{
			EndEvent();
		}
		else
		{
			if (EventStudent.Pathfinding.canMove)
			{
				return;
			}
			if (EventPhase == 1)
			{
				Timer += Time.deltaTime;
				EventStudent.Character.GetComponent<Animation>().CrossFade(EventAnim[0]);
				EventPhase++;
			}
			else if (EventPhase == 2)
			{
				Timer += Time.deltaTime;
				if (Timer > EventStudent.Character.GetComponent<Animation>()[EventAnim[0]].length)
				{
					EventStudent.Character.GetComponent<Animation>().CrossFade(EventStudent.EatAnim);
					EventStudent.Bento.transform.localPosition = new Vector3(-0.025f, -0.105f, 0f);
					EventStudent.Bento.transform.localEulerAngles = new Vector3(0f, 165f, 82.5f);
					EventStudent.Chopsticks[0].SetActive(true);
					EventStudent.Chopsticks[1].SetActive(true);
					EventStudent.Bento.SetActive(true);
					EventStudent.Lid.SetActive(false);
					RingCollider.enabled = true;
					EventPhase++;
					Timer = 0f;
				}
				else if (Timer > 4f)
				{
					if (EventStudent.Cosmetic.FemaleAccessories[3] != null)
					{
						EventStudent.Cosmetic.FemaleAccessories[3].transform.parent = null;
						EventStudent.Cosmetic.FemaleAccessories[3].transform.position = new Vector3(-2.707666f, 12.4695f, -31.136f);
						EventStudent.Cosmetic.FemaleAccessories[3].transform.eulerAngles = new Vector3(-20f, 180f, 0f);
					}
				}
				else if (Timer > 2.5f)
				{
					EventStudent.Cosmetic.FemaleAccessories[3].transform.parent = EventStudent.RightHand;
					EventStudent.Cosmetic.FemaleAccessories[3].transform.localPosition = HoldingPosition;
					EventStudent.Cosmetic.FemaleAccessories[3].transform.localEulerAngles = HoldingRotation;
				}
			}
			else if (EventPhase == 3)
			{
				if (Clock.HourTime > 13.375f)
				{
					EventStudent.Bento.SetActive(false);
					EventStudent.Chopsticks[0].SetActive(false);
					EventStudent.Chopsticks[1].SetActive(false);
					if (RingCollider != null)
					{
						RingCollider.enabled = false;
					}
					if (RingPrompt != null)
					{
						RingPrompt.Hide();
						RingPrompt.enabled = false;
					}
					EventStudent.Character.GetComponent<Animation>()[EventAnim[0]].time = EventStudent.Character.GetComponent<Animation>()[EventAnim[0]].length;
					EventStudent.Character.GetComponent<Animation>()[EventAnim[0]].speed = -1f;
					EventStudent.Character.GetComponent<Animation>().CrossFade((!(EventStudent.Cosmetic.FemaleAccessories[3] != null)) ? EventAnim[1] : EventAnim[0]);
					EventPhase++;
				}
			}
			else if (EventPhase == 4)
			{
				Timer += Time.deltaTime;
				if (EventStudent.Cosmetic.FemaleAccessories[3] != null)
				{
					if (Timer > 2f)
					{
						EventStudent.Cosmetic.FemaleAccessories[3].transform.parent = EventStudent.RightHand;
						EventStudent.Cosmetic.FemaleAccessories[3].transform.localPosition = HoldingPosition;
						EventStudent.Cosmetic.FemaleAccessories[3].transform.localEulerAngles = HoldingRotation;
					}
					if (Timer > 3f)
					{
						EventStudent.Cosmetic.FemaleAccessories[3].transform.parent = EventStudent.LeftMiddleFinger;
						EventStudent.Cosmetic.FemaleAccessories[3].transform.localPosition = OriginalPosition;
						RingCollider.enabled = false;
					}
					if (Timer > 6f)
					{
						EndEvent();
					}
				}
				else if (Timer > 1.5f && Yandere.transform.position.z < 0f)
				{
					EventSubtitle.text = EventSpeech[0];
					AudioClipPlayer.Play(EventClip[0], EventStudent.transform.position + Vector3.up, 5f, 10f, out VoiceClip, out CurrentClipLength);
					EventPhase++;
				}
			}
			else if (EventPhase == 5)
			{
				Timer += Time.deltaTime;
				if (Timer > 9.5f)
				{
					EndEvent();
				}
			}
			float num = Vector3.Distance(Yandere.transform.position, EventStudent.transform.position);
			if (!(num < 11f))
			{
				return;
			}
			if (num < 10f)
			{
				float num2 = Mathf.Abs((num - 10f) * 0.2f);
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				EventSubtitle.transform.localScale = new Vector3(num2, num2, num2);
			}
			else
			{
				EventSubtitle.transform.localScale = Vector3.zero;
			}
		}
	}

	private void EndEvent()
	{
		if (!EventOver)
		{
			if (VoiceClip != null)
			{
				Object.Destroy(VoiceClip);
			}
			EventStudent.CurrentDestination = EventStudent.Destinations[EventStudent.Phase];
			EventStudent.Pathfinding.target = EventStudent.Destinations[EventStudent.Phase];
			EventStudent.Obstacle.checkTime = 1f;
			if (!EventStudent.Dying)
			{
				EventStudent.Prompt.enabled = true;
			}
			EventStudent.Pathfinding.speed = 1f;
			EventStudent.TargetDistance = 0.5f;
			EventStudent.InEvent = false;
			EventStudent.Private = false;
			EventSubtitle.text = string.Empty;
			StudentManager.UpdateStudents();
		}
		EventActive = false;
		base.enabled = false;
	}

	public void ReturnRing()
	{
		if (EventStudent.Cosmetic.FemaleAccessories[3] != null)
		{
			EventStudent.Cosmetic.FemaleAccessories[3].transform.parent = EventStudent.LeftMiddleFinger;
			EventStudent.Cosmetic.FemaleAccessories[3].transform.localPosition = OriginalPosition;
			RingCollider.enabled = false;
			RingPrompt.Hide();
			RingPrompt.enabled = false;
		}
	}
}
