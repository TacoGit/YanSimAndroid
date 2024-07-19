using UnityEngine;
using UnityEngine.SceneManagement;

public class LivingRoomCutsceneScript : MonoBehaviour
{
	public ColorCorrectionCurves ColorCorrection;

	public CosmeticScript YandereCosmetic;

	public AmbientObscurance Obscurance;

	public Vignetting Vignette;

	public NoiseAndGrain Noise;

	public SkinnedMeshRenderer YandereRenderer;

	public Renderer RightEyeRenderer;

	public Renderer LeftEyeRenderer;

	public Transform FriendshipCamera;

	public Transform LivingRoomCamera;

	public Transform CutsceneCamera;

	public UIPanel EliminationPanel;

	public UISprite SubDarknessBG;

	public UISprite SubDarkness;

	public UISprite Darkness;

	public UILabel Subtitle;

	public UIPanel Panel;

	public Vector3 RightEyeOrigin;

	public Vector3 LeftEyeOrigin;

	public AudioClip DramaticBoom;

	public AudioClip RivalProtest;

	public AudioSource Jukebox;

	public GameObject Prologue;

	public GameObject Yandere;

	public GameObject Rival;

	public Transform RightEye;

	public Transform LeftEye;

	public float ShakeStrength;

	public float AnimOffset;

	public float EyeShrink;

	public float xOffset;

	public float zOffset;

	public float Timer;

	public string[] Lines;

	public float[] Times;

	public int Phase = 1;

	public int ID = 1;

	public Texture ZTR;

	public int ZTRID;

	private void Start()
	{
		YandereCosmetic.SetFemaleUniform();
		YandereCosmetic.RightWristband.SetActive(false);
		YandereCosmetic.LeftWristband.SetActive(false);
		YandereCosmetic.ThickBrows.SetActive(false);
		for (ID = 0; ID < YandereCosmetic.FemaleHair.Length; ID++)
		{
			GameObject gameObject = YandereCosmetic.FemaleHair[ID];
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		for (ID = 0; ID < YandereCosmetic.TeacherHair.Length; ID++)
		{
			GameObject gameObject2 = YandereCosmetic.TeacherHair[ID];
			if (gameObject2 != null)
			{
				gameObject2.SetActive(false);
			}
		}
		for (ID = 0; ID < YandereCosmetic.FemaleAccessories.Length; ID++)
		{
			GameObject gameObject3 = YandereCosmetic.FemaleAccessories[ID];
			if (gameObject3 != null)
			{
				gameObject3.SetActive(false);
			}
		}
		for (ID = 0; ID < YandereCosmetic.TeacherAccessories.Length; ID++)
		{
			GameObject gameObject4 = YandereCosmetic.TeacherAccessories[ID];
			if (gameObject4 != null)
			{
				gameObject4.SetActive(false);
			}
		}
		for (ID = 0; ID < YandereCosmetic.ClubAccessories.Length; ID++)
		{
			GameObject gameObject5 = YandereCosmetic.ClubAccessories[ID];
			if (gameObject5 != null)
			{
				gameObject5.SetActive(false);
			}
		}
		GameObject[] scanners = YandereCosmetic.Scanners;
		foreach (GameObject gameObject6 in scanners)
		{
			if (gameObject6 != null)
			{
				gameObject6.SetActive(false);
			}
		}
		GameObject[] flowers = YandereCosmetic.Flowers;
		foreach (GameObject gameObject7 in flowers)
		{
			if (gameObject7 != null)
			{
				gameObject7.SetActive(false);
			}
		}
		GameObject[] punkAccessories = YandereCosmetic.PunkAccessories;
		foreach (GameObject gameObject8 in punkAccessories)
		{
			if (gameObject8 != null)
			{
				gameObject8.SetActive(false);
			}
		}
		GameObject[] redCloth = YandereCosmetic.RedCloth;
		foreach (GameObject gameObject9 in redCloth)
		{
			if (gameObject9 != null)
			{
				gameObject9.SetActive(false);
			}
		}
		GameObject[] kerchiefs = YandereCosmetic.Kerchiefs;
		foreach (GameObject gameObject10 in kerchiefs)
		{
			if (gameObject10 != null)
			{
				gameObject10.SetActive(false);
			}
		}
		for (int n = 0; n < 10; n++)
		{
			YandereCosmetic.Fingernails[n].gameObject.SetActive(false);
		}
		ID = 0;
		YandereCosmetic.FemaleHair[1].SetActive(true);
		YandereCosmetic.MyRenderer.materials[2].mainTexture = YandereCosmetic.DefaultFaceTexture;
		Subtitle.text = string.Empty;
		RightEyeRenderer.material.color = new Color(0.33f, 0.33f, 0.33f, 1f);
		LeftEyeRenderer.material.color = new Color(0.33f, 0.33f, 0.33f, 1f);
		RightEyeOrigin = RightEye.localPosition;
		LeftEyeOrigin = LeftEye.localPosition;
		EliminationPanel.alpha = 0f;
		Panel.alpha = 1f;
		ColorCorrection.saturation = 1f;
		Noise.intensityMultiplier = 0f;
		Obscurance.intensity = 0f;
		Vignette.enabled = false;
		Vignette.intensity = 1f;
		Vignette.blur = 1f;
		Vignette.chromaticAberration = 1f;
	}

	private void Update()
	{
		AudioSource component = GetComponent<AudioSource>();
		if (Phase == 1)
		{
			Timer += Time.deltaTime;
			if (Timer > 1f)
			{
				Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime));
				if (Darkness.color.a == 0f && Input.GetButtonDown("A"))
				{
					Timer = 0f;
					Phase++;
				}
			}
		}
		else if (Phase == 2)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
			if (Darkness.color.a == 1f)
			{
				base.transform.parent = LivingRoomCamera;
				base.transform.localPosition = new Vector3(-0.65f, 0f, 0f);
				base.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
				Vignette.enabled = true;
				Prologue.SetActive(false);
				Phase++;
			}
		}
		else if (Phase == 3)
		{
			Timer += Time.deltaTime;
			if (Timer > 1f)
			{
				Panel.alpha = Mathf.MoveTowards(Panel.alpha, 0f, Time.deltaTime);
				if (Panel.alpha == 0f)
				{
					Yandere.GetComponent<Animation>()["FriendshipYandere"].time = 0f;
					Rival.GetComponent<Animation>()["FriendshipRival"].time = 0f;
					LivingRoomCamera.gameObject.GetComponent<Animation>().Play();
					Timer = 0f;
					Phase++;
				}
			}
		}
		else if (Phase == 4)
		{
			Timer += Time.deltaTime;
			if (Timer > 10f)
			{
				base.transform.parent = FriendshipCamera;
				base.transform.localPosition = new Vector3(-0.65f, 0f, 0f);
				base.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
				FriendshipCamera.gameObject.GetComponent<Animation>().Play();
				component.Play();
				Subtitle.text = Lines[0];
				Timer = 0f;
				Phase++;
			}
		}
		else if (Phase == 5)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Timer += 10f;
				component.time += 10f;
				FriendshipCamera.gameObject.GetComponent<Animation>()["FriendshipCameraFlat"].time += 10f;
			}
			Timer += Time.deltaTime;
			if (Timer < 166f)
			{
				Yandere.GetComponent<Animation>()["FriendshipYandere"].time = component.time + AnimOffset;
				Rival.GetComponent<Animation>()["FriendshipRival"].time = component.time + AnimOffset;
			}
			if (ID < Times.Length && component.time > Times[ID])
			{
				Subtitle.text = Lines[ID];
				ID++;
			}
			if (component.time > 54f)
			{
				Jukebox.volume = Mathf.MoveTowards(Jukebox.volume, 0f, Time.deltaTime / 5f);
				component.volume = Mathf.MoveTowards(component.volume, 0.2f, Time.deltaTime * 0.1f / 5f);
				Vignette.intensity = Mathf.MoveTowards(Vignette.intensity, 1f, Time.deltaTime * 4f / 5f);
				Vignette.blur = Vignette.intensity;
				Vignette.chromaticAberration = Vignette.intensity;
				ColorCorrection.saturation = Mathf.MoveTowards(ColorCorrection.saturation, 1f, Time.deltaTime / 5f);
				Noise.intensityMultiplier = Mathf.MoveTowards(Noise.intensityMultiplier, 0f, Time.deltaTime * 3f / 5f);
				Obscurance.intensity = Mathf.MoveTowards(Obscurance.intensity, 0f, Time.deltaTime * 3f / 5f);
				ShakeStrength = Mathf.MoveTowards(ShakeStrength, 0f, Time.deltaTime * 0.01f / 5f);
				EliminationPanel.alpha = Mathf.MoveTowards(EliminationPanel.alpha, 0f, Time.deltaTime);
				EyeShrink = Mathf.MoveTowards(EyeShrink, 0f, Time.deltaTime);
			}
			else if (component.time > 42f)
			{
				if (!Jukebox.isPlaying)
				{
					Jukebox.Play();
				}
				Jukebox.volume = Mathf.MoveTowards(Jukebox.volume, 1f, Time.deltaTime / 10f);
				component.volume = Mathf.MoveTowards(component.volume, 0.1f, Time.deltaTime * 0.1f / 10f);
				Vignette.intensity = Mathf.MoveTowards(Vignette.intensity, 5f, Time.deltaTime * 4f / 10f);
				Vignette.blur = Vignette.intensity;
				Vignette.chromaticAberration = Vignette.intensity;
				ColorCorrection.saturation = Mathf.MoveTowards(ColorCorrection.saturation, 0f, Time.deltaTime / 10f);
				Noise.intensityMultiplier = Mathf.MoveTowards(Noise.intensityMultiplier, 3f, Time.deltaTime * 3f / 10f);
				Obscurance.intensity = Mathf.MoveTowards(Obscurance.intensity, 3f, Time.deltaTime * 3f / 10f);
				ShakeStrength = Mathf.MoveTowards(ShakeStrength, 0.01f, Time.deltaTime * 0.01f / 10f);
				EyeShrink = Mathf.MoveTowards(EyeShrink, 0.9f, Time.deltaTime);
				if (component.time > 45f)
				{
					if (component.time > 54f)
					{
						EliminationPanel.alpha = Mathf.MoveTowards(EliminationPanel.alpha, 0f, Time.deltaTime);
					}
					else
					{
						EliminationPanel.alpha = Mathf.MoveTowards(EliminationPanel.alpha, 1f, Time.deltaTime);
						if (Input.GetButtonDown("X"))
						{
							component.clip = RivalProtest;
							component.volume = 1f;
							component.Play();
							Jukebox.gameObject.SetActive(false);
							Subtitle.text = "Wait, what are you doing?! That's not funny! Stop! Let me go! ...n...NO!!!";
							SubDarknessBG.color = new Color(SubDarknessBG.color.r, SubDarknessBG.color.g, SubDarknessBG.color.b, 1f);
							Phase++;
						}
					}
				}
			}
			if (Timer > 167f)
			{
				Animation component2 = Yandere.GetComponent<Animation>();
				component2["FriendshipYandere"].speed = -0.2f;
				component2.Play("FriendshipYandere");
				component2["FriendshipYandere"].time = component2["FriendshipYandere"].length;
				Subtitle.text = string.Empty;
				Phase = 10;
			}
		}
		else if (Phase == 6)
		{
			if (!component.isPlaying)
			{
				component.clip = DramaticBoom;
				component.Play();
				Subtitle.text = string.Empty;
				Phase++;
			}
		}
		else if (Phase == 7)
		{
			if (!component.isPlaying)
			{
				StudentGlobals.SetStudentKidnapped(81, false);
				StudentGlobals.SetStudentBroken(81, true);
				StudentGlobals.SetStudentKidnapped(30, true);
				StudentGlobals.SetStudentSanity(30, 100f);
				SchoolGlobals.KidnapVictim = 30;
				HomeGlobals.StartInBasement = true;
				SceneManager.LoadScene("CalendarScene");
			}
		}
		else if (Phase == 10)
		{
			SubDarkness.color = new Color(SubDarkness.color.r, SubDarkness.color.g, SubDarkness.color.b, Mathf.MoveTowards(SubDarkness.color.a, 1f, Time.deltaTime * 0.2f));
			if (SubDarkness.color.a == 1f)
			{
				StudentGlobals.SetStudentKidnapped(81, false);
				StudentGlobals.SetStudentBroken(81, true);
				SchoolGlobals.KidnapVictim = 0;
				SceneManager.LoadScene("CalendarScene");
			}
		}
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			Time.timeScale -= 1f;
		}
		if (Input.GetKeyDown(KeyCode.Equals))
		{
			Time.timeScale += 1f;
		}
		component.pitch = Time.timeScale;
	}

	private void LateUpdate()
	{
		if (Phase > 2)
		{
			base.transform.localPosition = new Vector3(-0.65f + ShakeStrength * Random.Range(-1f, 1f), ShakeStrength * Random.Range(-1f, 1f), ShakeStrength * Random.Range(-1f, 1f));
			CutsceneCamera.position = new Vector3(CutsceneCamera.position.x + xOffset, CutsceneCamera.position.y, CutsceneCamera.position.z + zOffset);
			LeftEye.localPosition = new Vector3(LeftEye.localPosition.x, LeftEye.localPosition.y, LeftEyeOrigin.z - EyeShrink * 0.01f);
			RightEye.localPosition = new Vector3(RightEye.localPosition.x, RightEye.localPosition.y, RightEyeOrigin.z + EyeShrink * 0.01f);
			LeftEye.localScale = new Vector3(1f - EyeShrink * 0.5f, 1f - EyeShrink * 0.5f, LeftEye.localScale.z);
			RightEye.localScale = new Vector3(1f - EyeShrink * 0.5f, 1f - EyeShrink * 0.5f, RightEye.localScale.z);
		}
	}
}
