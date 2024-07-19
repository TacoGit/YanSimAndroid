using UnityEngine;

public class LaptopScript : MonoBehaviour
{
	public SkinnedMeshRenderer SCPRenderer;

	public Camera LaptopCamera;

	public JukeboxScript Jukebox;

	public YandereScript Yandere;

	public AudioSource MyAudio;

	public DynamicBone Hair;

	public Transform LaptopScreen;

	public AudioClip ShutDown;

	public GameObject SCP;

	public bool React;

	public bool Off;

	public float[] Cues;

	public string[] Subs;

	public Mesh[] Uniforms;

	public int FirstFrame;

	public float Timer;

	public UILabel EventSubtitle;

	private void Start()
	{
		if (SchoolGlobals.SCP)
		{
			LaptopScreen.localScale = Vector3.zero;
			LaptopCamera.enabled = false;
			SCP.SetActive(false);
			base.enabled = false;
		}
		else
		{
			SCPRenderer.sharedMesh = Uniforms[StudentGlobals.FemaleUniform];
			Animation component = SCP.GetComponent<Animation>();
			component["f02_scp_00"].speed = 0f;
			component["f02_scp_00"].time = 0f;
			MyAudio = GetComponent<AudioSource>();
		}
	}

	private void Update()
	{
		if (FirstFrame == 2)
		{
			LaptopCamera.enabled = false;
		}
		FirstFrame++;
		if (!Off)
		{
			Animation component = SCP.GetComponent<Animation>();
			if (!React)
			{
				if (Yandere.transform.position.x > base.transform.position.x + 1f && Vector3.Distance(Yandere.transform.position, new Vector3(base.transform.position.x, 4f, base.transform.position.z)) < 2f && Yandere.Followers == 0)
				{
					EventSubtitle.transform.localScale = new Vector3(1f, 1f, 1f);
					component["f02_scp_00"].time = 0f;
					LaptopCamera.enabled = true;
					component.Play();
					Hair.enabled = true;
					Jukebox.Dip = 0.5f;
					MyAudio.Play();
					React = true;
				}
			}
			else
			{
				MyAudio.pitch = Time.timeScale;
				MyAudio.volume = 1f;
				if (Yandere.transform.position.y > base.transform.position.y + 3f || Yandere.transform.position.y < base.transform.position.y - 3f)
				{
					MyAudio.volume = 0f;
				}
				for (int i = 0; i < Cues.Length; i++)
				{
					if (MyAudio.time > Cues[i])
					{
						EventSubtitle.text = Subs[i];
					}
				}
				if (MyAudio.time >= MyAudio.clip.length - 1f || MyAudio.time == 0f)
				{
					component["f02_scp_00"].speed = 1f;
					Timer += Time.deltaTime;
				}
				else
				{
					component["f02_scp_00"].time = MyAudio.time;
				}
				if (Timer > 1f || Vector3.Distance(Yandere.transform.position, new Vector3(base.transform.position.x, 4f, base.transform.position.z)) > 5f)
				{
					TurnOff();
				}
			}
			if (Yandere.StudentManager.Clock.HourTime > 16f || Yandere.Police.FadeOut)
			{
				TurnOff();
			}
		}
		else if (LaptopScreen.localScale.x > 0.1f)
		{
			LaptopScreen.localScale = Vector3.Lerp(LaptopScreen.localScale, Vector3.zero, Time.deltaTime * 10f);
		}
		else if (base.enabled)
		{
			LaptopScreen.localScale = Vector3.zero;
			Hair.enabled = false;
			base.enabled = false;
		}
	}

	private void TurnOff()
	{
		MyAudio.clip = ShutDown;
		MyAudio.Play();
		EventSubtitle.text = string.Empty;
		SchoolGlobals.SCP = true;
		LaptopCamera.enabled = false;
		Jukebox.Dip = 1f;
		React = false;
		Off = true;
	}
}
