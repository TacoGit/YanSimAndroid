using UnityEngine;

public class ReputationScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public ArmDetectorScript ArmDetector;

	public PortalScript Portal;

	public Transform CurrentRepMarker;

	public Transform PendingRepMarker;

	public UILabel PendingRepLabel;

	public ClockScript Clock;

	public float Reputation;

	public float PendingRep;

	public int CheckedRep = 1;

	public int Phase;

	public bool MissionMode;

	public GameObject FlowerVase;

	public GameObject Grafitti;

	private void Start()
	{
		if (MissionModeGlobals.MissionMode)
		{
			MissionMode = true;
		}
		Reputation = PlayerGlobals.Reputation;
		Bully();
	}

	private void Update()
	{
		if (Phase == 1)
		{
			if (Clock.PresentTime / 60f > 8.5f)
			{
				Phase++;
			}
		}
		else if (Phase == 2)
		{
			if (Clock.PresentTime / 60f > 13.5f)
			{
				Phase++;
			}
		}
		else if (Phase == 3 && Clock.PresentTime / 60f > 18f)
		{
			Phase++;
		}
		if (PendingRep < 0f)
		{
			StudentManager.TutorialWindow.ShowRepMessage = true;
		}
		if (CheckedRep < Phase && !StudentManager.Yandere.Struggling && !StudentManager.Yandere.DelinquentFighting && !StudentManager.Yandere.Pickpocketing && !StudentManager.Yandere.Noticed && !ArmDetector.SummonDemon)
		{
			UpdateRep();
			if (Reputation <= -100f)
			{
				Portal.EndDay();
			}
		}
		if (!MissionMode)
		{
			CurrentRepMarker.localPosition = new Vector3(Mathf.Lerp(CurrentRepMarker.localPosition.x, -830f + Reputation * 1.5f, Time.deltaTime * 10f), CurrentRepMarker.localPosition.y, CurrentRepMarker.localPosition.z);
			PendingRepMarker.localPosition = new Vector3(Mathf.Lerp(PendingRepMarker.localPosition.x, CurrentRepMarker.transform.localPosition.x + PendingRep * 1.5f, Time.deltaTime * 10f), PendingRepMarker.localPosition.y, PendingRepMarker.localPosition.z);
		}
		else
		{
			PendingRepMarker.localPosition = new Vector3(Mathf.Lerp(PendingRepMarker.localPosition.x, -980f + PendingRep * -3f, Time.deltaTime * 10f), PendingRepMarker.localPosition.y, PendingRepMarker.localPosition.z);
		}
		if (CurrentRepMarker.localPosition.x < -980f)
		{
			CurrentRepMarker.localPosition = new Vector3(-980f, CurrentRepMarker.localPosition.y, CurrentRepMarker.localPosition.z);
		}
		if (PendingRepMarker.localPosition.x < -980f)
		{
			PendingRepMarker.localPosition = new Vector3(-980f, PendingRepMarker.localPosition.y, PendingRepMarker.localPosition.z);
		}
		if (CurrentRepMarker.localPosition.x > -680f)
		{
			CurrentRepMarker.localPosition = new Vector3(-680f, CurrentRepMarker.localPosition.y, CurrentRepMarker.localPosition.z);
		}
		if (PendingRepMarker.localPosition.x > -680f)
		{
			PendingRepMarker.localPosition = new Vector3(-680f, PendingRepMarker.localPosition.y, PendingRepMarker.localPosition.z);
		}
		if (!MissionMode)
		{
			if (PendingRep > 0f)
			{
				PendingRepLabel.text = "+" + PendingRep;
			}
			else if (PendingRep < 0f)
			{
				PendingRepLabel.text = PendingRep.ToString();
			}
			else
			{
				PendingRepLabel.text = string.Empty;
			}
		}
		else if (PendingRep < 0f)
		{
			PendingRepLabel.text = (0f - PendingRep).ToString();
		}
		else
		{
			PendingRepLabel.text = string.Empty;
		}
	}

	private void Bully()
	{
		FlowerVase.SetActive(false);
	}

	public void UpdateRep()
	{
		Reputation += PendingRep;
		PendingRep = 0f;
		CheckedRep++;
		StudentManager.WipePendingRep();
	}
}
