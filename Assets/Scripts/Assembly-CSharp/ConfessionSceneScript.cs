using UnityEngine;

public class ConfessionSceneScript : MonoBehaviour
{
	public Transform[] CameraDestinations;

	public StudentManagerScript StudentManager;

	public PromptBarScript PromptBar;

	public JukeboxScript Jukebox;

	public YandereScript Yandere;

	public ClockScript Clock;

	public Bloom BloomEffect;

	public StudentScript Suitor;

	public StudentScript Rival;

	public ParticleSystem MythBlossoms;

	public GameObject HeartBeatCamera;

	public GameObject ConfessionBG;

	public Transform MainCamera;

	public Transform RivalSpot;

	public Transform KissSpot;

	public string[] Text;

	public UISprite Darkness;

	public UILabel Label;

	public UIPanel Panel;

	public bool ShowLabel;

	public bool Kissing;

	public int TextPhase = 1;

	public int Phase = 1;

	public float Timer;

	private void Update()
	{
		if (Phase == 1)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
			Panel.alpha = Mathf.MoveTowards(Panel.alpha, 0f, Time.deltaTime);
			Jukebox.Volume = Mathf.MoveTowards(Jukebox.Volume, 0f, Time.deltaTime);
			if (Darkness.color.a == 1f)
			{
				Timer += Time.deltaTime;
				if (Timer > 1f)
				{
					BloomEffect.bloomIntensity = 1f;
					BloomEffect.bloomThreshhold = 0f;
					BloomEffect.bloomBlurIterations = 1;
					Suitor = StudentManager.Students[28];
					Rival = StudentManager.Students[30];
					Rival.transform.position = RivalSpot.position;
					Rival.transform.eulerAngles = RivalSpot.eulerAngles;
					Suitor.Cosmetic.MyRenderer.materials[Suitor.Cosmetic.FaceID].SetFloat("_BlendAmount", 1f);
					Suitor.transform.eulerAngles = StudentManager.SuitorConfessionSpot.eulerAngles;
					Suitor.transform.position = StudentManager.SuitorConfessionSpot.position;
					Suitor.Character.GetComponent<Animation>().Play(Suitor.IdleAnim);
					ParticleSystem.EmissionModule emission = MythBlossoms.emission;
					emission.rateOverTime = 100f;
					HeartBeatCamera.SetActive(false);
					ConfessionBG.SetActive(true);
					GetComponent<AudioSource>().Play();
					MainCamera.position = CameraDestinations[1].position;
					MainCamera.eulerAngles = CameraDestinations[1].eulerAngles;
					Timer = 0f;
					Phase++;
				}
			}
		}
		else if (Phase == 2)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime));
			if (Darkness.color.a == 0f)
			{
				if (!ShowLabel)
				{
					Label.color = new Color(Label.color.r, Label.color.g, Label.color.b, Mathf.MoveTowards(Label.color.a, 0f, Time.deltaTime));
					if (Label.color.a == 0f)
					{
						if (TextPhase < 5)
						{
							MainCamera.position = CameraDestinations[TextPhase].position;
							MainCamera.eulerAngles = CameraDestinations[TextPhase].eulerAngles;
							if (TextPhase == 4 && !Kissing)
							{
								ParticleSystem.EmissionModule emission2 = Suitor.Hearts.emission;
								emission2.enabled = true;
								emission2.rateOverTime = 10f;
								Suitor.Hearts.Play();
								ParticleSystem.EmissionModule emission3 = Rival.Hearts.emission;
								emission3.enabled = true;
								emission3.rateOverTime = 10f;
								Rival.Hearts.Play();
								Suitor.Character.transform.localScale = new Vector3(1f, 1f, 1f);
								Suitor.Character.GetComponent<Animation>().Play("kiss_00");
								Suitor.transform.position = KissSpot.position;
								Rival.Character.GetComponent<Animation>()[Rival.ShyAnim].weight = 0f;
								Rival.Character.GetComponent<Animation>().Play("f02_kiss_00");
								Kissing = true;
							}
							Label.text = Text[TextPhase];
							ShowLabel = true;
						}
						else
						{
							Phase++;
						}
					}
				}
				else
				{
					Label.color = new Color(Label.color.r, Label.color.g, Label.color.b, Mathf.MoveTowards(Label.color.a, 1f, Time.deltaTime));
					if (Label.color.a == 1f)
					{
						if (!PromptBar.Show)
						{
							PromptBar.ClearButtons();
							PromptBar.Label[0].text = "Continue";
							PromptBar.UpdateButtons();
							PromptBar.Show = true;
						}
						if (Input.GetButtonDown("A"))
						{
							TextPhase++;
							ShowLabel = false;
						}
					}
				}
			}
		}
		else if (Phase == 3)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
			if (Darkness.color.a == 1f)
			{
				Timer += Time.deltaTime;
				if (Timer > 1f)
				{
					DatingGlobals.SuitorProgress = 2;
					Suitor.Character.transform.localScale = new Vector3(0.94f, 0.94f, 0.94f);
					PromptBar.ClearButtons();
					PromptBar.UpdateButtons();
					PromptBar.Show = false;
					ConfessionBG.SetActive(false);
					Yandere.FixCamera();
					Phase++;
				}
			}
		}
		else
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime));
			Panel.alpha = Mathf.MoveTowards(Panel.alpha, 1f, Time.deltaTime);
			if (Darkness.color.a == 0f)
			{
				Yandere.RPGCamera.enabled = true;
				Yandere.CanMove = true;
				HeartBeatCamera.SetActive(true);
				ParticleSystem.EmissionModule emission4 = MythBlossoms.emission;
				emission4.rateOverTime = 20f;
				Clock.StopTime = false;
				base.enabled = false;
				Suitor.CoupleID = 28;
				Rival.CoupleID = 30;
			}
		}
		if (Kissing)
		{
			Animation component = Suitor.Character.GetComponent<Animation>();
			if (component["kiss_00"].time >= component["kiss_00"].length)
			{
				component.CrossFade(Suitor.IdleAnim);
				Rival.Character.GetComponent<Animation>().CrossFade(Rival.IdleAnim);
				Kissing = false;
			}
		}
	}
}
