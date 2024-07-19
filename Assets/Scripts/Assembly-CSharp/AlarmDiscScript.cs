using UnityEngine;

public class AlarmDiscScript : MonoBehaviour
{
	public AudioClip[] LongFemaleScreams;

	public AudioClip[] LongMaleScreams;

	public AudioClip[] FemaleScreams;

	public AudioClip[] MaleScreams;

	public AudioClip[] DelinquentScreams;

	public StudentScript Originator;

	public RadioScript SourceRadio;

	public StudentScript Student;

	public bool StudentIsBusy;

	public bool Delinquent;

	public bool NoScream;

	public bool Shocking;

	public bool Radio;

	public bool Male;

	public bool Long;

	public int Frame;

	private void Start()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x *= 2f - SchoolGlobals.SchoolAtmosphere;
		localScale.z = localScale.x;
		base.transform.localScale = localScale;
	}

	private void Update()
	{
		if (Frame > 0)
		{
			Object.Destroy(base.gameObject);
		}
		else if (!NoScream)
		{
			if (!Long)
			{
				if (Originator != null)
				{
					Male = Originator.Male;
				}
				if (!Male)
				{
					PlayClip(FemaleScreams[Random.Range(0, FemaleScreams.Length)], base.transform.position);
				}
				else if (Delinquent)
				{
					PlayClip(DelinquentScreams[Random.Range(0, DelinquentScreams.Length)], base.transform.position);
				}
				else
				{
					PlayClip(MaleScreams[Random.Range(0, MaleScreams.Length)], base.transform.position);
				}
			}
			else if (!Male)
			{
				PlayClip(LongFemaleScreams[Random.Range(0, LongFemaleScreams.Length)], base.transform.position);
			}
			else
			{
				PlayClip(LongMaleScreams[Random.Range(0, LongMaleScreams.Length)], base.transform.position);
			}
		}
		Frame++;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != 9)
		{
			return;
		}
		Student = other.gameObject.GetComponent<StudentScript>();
		if (!(Student != null) || !Student.enabled || !(Student.DistractionSpot != new Vector3(base.transform.position.x, Student.transform.position.y, base.transform.position.z)))
		{
			return;
		}
		Object.Destroy(Student.Giggle);
		Student.InvestigationTimer = 0f;
		Student.InvestigationPhase = 0;
		Student.Investigating = false;
		Student.DiscCheck = false;
		Student.VisionDistance += 1f;
		Student.ChalkDust.Stop();
		Student.CleanTimer = 0f;
		if (!Radio)
		{
			if (!(Student != Originator))
			{
				return;
			}
			if (Student.Clock.Period == 3 && Student.BusyAtLunch)
			{
				StudentIsBusy = true;
			}
			if ((Student.StudentID == 47 || Student.StudentID == 49) && Student.StudentManager.ConvoManager.Confirmed)
			{
				StudentIsBusy = true;
			}
			if (Student.TurnOffRadio || !Student.Alive || Student.Pushed || Student.Dying || Student.Alarmed || Student.Guarding || Student.Wet || Student.Slave || Student.CheckingNote || Student.WitnessedMurder || Student.WitnessedCorpse || StudentIsBusy || Student.FocusOnYandere || Student.Persona == PersonaType.Protective || Student.Fleeing || Student.Shoving || Student.SentHome || Student.ClubActivityPhase >= 16)
			{
				return;
			}
			if (Student.Male)
			{
			}
			Student.Character.GetComponent<Animation>().CrossFade(Student.LeanAnim);
			if (Originator != null)
			{
				if (Originator.WitnessedMurder)
				{
					Debug.Log("Somebody witnessed murder, and is directing attention towards Yandere=chan.");
					Student.DistractionSpot = new Vector3(base.transform.position.x, Student.Yandere.transform.position.y, base.transform.position.z);
				}
				else if (Originator.Corpse == null)
				{
					Student.DistractionSpot = new Vector3(base.transform.position.x, Student.transform.position.y, base.transform.position.z);
				}
				else
				{
					Student.DistractionSpot = new Vector3(Originator.Corpse.transform.position.x, Student.transform.position.y, Originator.Corpse.transform.position.z);
				}
			}
			else
			{
				Student.DistractionSpot = new Vector3(base.transform.position.x, Student.transform.position.y, base.transform.position.z);
			}
			Student.DiscCheck = true;
			if (Shocking)
			{
				Student.Hesitation = 0.5f;
			}
			Student.Alarm = 200f;
			if (!NoScream)
			{
				InvestigateScream();
			}
		}
		else
		{
			if (Student.Nemesis || !Student.Alive || Student.Dying || Student.Guarding || Student.Alarmed || Student.Wet || Student.Slave || Student.WitnessedMurder || Student.WitnessedCorpse || Student.InEvent || Student.Following || Student.Distracting || Student.Actions[Student.Phase] == StudentActionType.Teaching || Student.Actions[Student.Phase] == StudentActionType.SitAndTakeNotes || Student.GoAway || !Student.Routine || Student.CheckingNote || Student.SentHome || Student.Persona == PersonaType.Protective || !(Student.CharacterAnimation != null) || !(SourceRadio.Victim == null))
			{
				return;
			}
			Student.CharacterAnimation.CrossFade(Student.LeanAnim);
			Student.Pathfinding.canSearch = false;
			Student.Pathfinding.canMove = false;
			Student.EatingSnack = false;
			Student.Radio = SourceRadio;
			Student.TurnOffRadio = true;
			Student.Routine = false;
			Student.GoAway = false;
			bool flag = false;
			if (Student.Bento.activeInHierarchy && Student.StudentID > 1)
			{
				flag = true;
			}
			Student.EmptyHands();
			if (flag)
			{
				GenericBentoScript component = Student.Bento.GetComponent<GenericBentoScript>();
				component.enabled = true;
				component.Prompt.enabled = true;
				Student.Bento.SetActive(true);
				Student.Bento.transform.parent = Student.transform;
				if (Student.Male)
				{
					Student.Bento.transform.localPosition = new Vector3(0f, 0.4266666f, -0.075f);
				}
				else
				{
					Student.Bento.transform.localPosition = new Vector3(0f, 0.461f, -0.075f);
				}
				Student.Bento.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				Student.Bento.transform.parent = null;
			}
			Student.SpeechLines.Stop();
			Student.ChalkDust.Stop();
			Student.CleanTimer = 0f;
			Student.RadioTimer = 0f;
			Student.ReadPhase = 0;
			SourceRadio.Victim = Student;
		}
	}

	private void PlayClip(AudioClip clip, Vector3 pos)
	{
		GameObject gameObject = new GameObject("TempAudio");
		gameObject.transform.position = pos;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.Play();
		Object.Destroy(gameObject, clip.length);
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.minDistance = 5f;
		audioSource.maxDistance = 10f;
		audioSource.spatialBlend = 1f;
		audioSource.volume = 0.5f;
		if (Student != null)
		{
			Student.DeathScream = gameObject;
		}
	}

	private void InvestigateScream()
	{
		if (!(Student.Giggle == null))
		{
			return;
		}
		if (Student.Clock.Period == 3 && Student.BusyAtLunch)
		{
			StudentIsBusy = true;
		}
		if (!Student.YandereVisible && !Student.Alarmed && !Student.Distracted && !Student.Wet && !Student.Slave && !Student.WitnessedMurder && !Student.WitnessedCorpse && !Student.Investigating && !Student.InEvent && !Student.Following && !Student.Confessing && !Student.Meeting && !Student.TurnOffRadio && !Student.Fleeing && !Student.Distracting && !Student.GoAway && !Student.FocusOnYandere && !StudentIsBusy && Student.Actions[Student.Phase] != StudentActionType.Teaching && Student.Actions[Student.Phase] != StudentActionType.SitAndTakeNotes && Student.Actions[Student.Phase] != StudentActionType.Graffiti && Student.Actions[Student.Phase] != StudentActionType.Bully && Student.Routine)
		{
			Student.Character.GetComponent<Animation>().CrossFade(Student.IdleAnim);
			GameObject giggle = Object.Instantiate(Student.EmptyGameObject, new Vector3(base.transform.position.x, Student.transform.position.y, base.transform.position.z), Quaternion.identity);
			Student.Giggle = giggle;
			if (Student.Pathfinding != null && !Student.Nemesis)
			{
				Student.Pathfinding.canSearch = false;
				Student.Pathfinding.canMove = false;
				Student.InvestigationPhase = 0;
				Student.InvestigationTimer = 0f;
				Student.Investigating = true;
				Student.EatingSnack = false;
				Student.SpeechLines.Stop();
				Student.ChalkDust.Stop();
				Student.DiscCheck = true;
				Student.Routine = false;
				Student.CleanTimer = 0f;
				Student.ReadPhase = 0;
				Student.StopPairing();
				Student.EmptyHands();
			}
		}
	}
}
