using UnityEngine;

public class DelinquentVoicesScript : MonoBehaviour
{
	public YandereScript Yandere;

	public RadioScript Radio;

	public SubtitleScript Subtitle;

	public float Timer;

	public int RandomID;

	public int LastID;

	private void Start()
	{
		Timer = 5f;
	}

	private void Update()
	{
		if (!Radio.MyAudio.isPlaying || !Yandere.CanMove || !(Vector3.Distance(Yandere.transform.position, base.transform.position) < 5f))
		{
			return;
		}
		Timer = Mathf.MoveTowards(Timer, 0f, Time.deltaTime);
		if (Timer != 0f)
		{
			return;
		}
		if (Yandere.Container == null)
		{
			while (RandomID == LastID)
			{
				RandomID = Random.Range(0, Subtitle.DelinquentAnnoyClips.Length);
			}
			LastID = RandomID;
			Subtitle.UpdateLabel(SubtitleType.DelinquentAnnoy, RandomID, 3f);
		}
		else
		{
			while (RandomID == LastID)
			{
				RandomID = Random.Range(0, Subtitle.DelinquentCaseClips.Length);
			}
			LastID = RandomID;
			Subtitle.UpdateLabel(SubtitleType.DelinquentCase, RandomID, 3f);
		}
		Timer = 5f;
	}
}
