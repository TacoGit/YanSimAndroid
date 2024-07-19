using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeYandereScript : MonoBehaviour
{
	public CharacterController MyController;

	public AudioSource MyAudio;

	public HomeVideoGamesScript HomeVideoGames;

	public HomeCameraScript HomeCamera;

	public UISprite HomeDarkness;

	public GameObject CutsceneYandere;

	public GameObject Controller;

	public GameObject Character;

	public GameObject Disc;

	public float WalkSpeed;

	public float RunSpeed;

	public bool CanMove;

	public AudioClip MiyukiReaction;

	public AudioClip DiscScratch;

	public Renderer PonytailRenderer;

	public Renderer PigtailR;

	public Renderer PigtailL;

	public Renderer Drills;

	public Transform Ponytail;

	public Transform HairR;

	public Transform HairL;

	public bool HidePony;

	public int Hairstyle;

	public int VictimID;

	public float Timer;

	public SkinnedMeshRenderer MyRenderer;

	public Texture[] UniformTextures;

	public Texture FaceTexture;

	public Mesh[] Uniforms;

	public Texture PajamaTexture;

	public Mesh PajamaMesh;

	private void Start()
	{
		if (CutsceneYandere != null)
		{
			CutsceneYandere.GetComponent<Animation>()["f02_midoriTexting_00"].speed = 0.1f;
		}
		if (SceneManager.GetActiveScene().name == "HomeScene")
		{
			if (!YanvaniaGlobals.DraculaDefeated && !HomeGlobals.MiyukiDefeated)
			{
				base.transform.position = Vector3.zero;
				base.transform.eulerAngles = Vector3.zero;
				if (!HomeGlobals.Night)
				{
					ChangeSchoolwear();
					StartCoroutine(ApplyCustomCostume());
				}
				else
				{
					WearPajamas();
				}
			}
			else if (HomeGlobals.StartInBasement)
			{
				HomeGlobals.StartInBasement = false;
				base.transform.position = new Vector3(0f, -135f, 0f);
				base.transform.eulerAngles = Vector3.zero;
			}
			else if (HomeGlobals.MiyukiDefeated)
			{
				base.transform.position = new Vector3(1f, 0f, 0f);
				base.transform.eulerAngles = new Vector3(0f, 90f, 0f);
				Character.GetComponent<Animation>().Play("f02_discScratch_00");
				Controller.transform.localPosition = new Vector3(0.09425f, 0.0095f, 0.01878f);
				Controller.transform.localEulerAngles = new Vector3(0f, 0f, -180f);
				HomeCamera.Destination = HomeCamera.Destinations[5];
				HomeCamera.Target = HomeCamera.Targets[5];
				Disc.SetActive(true);
				WearPajamas();
				MyAudio.clip = MiyukiReaction;
			}
			else
			{
				base.transform.position = new Vector3(1f, 0f, 0f);
				base.transform.eulerAngles = new Vector3(0f, 90f, 0f);
				Character.GetComponent<Animation>().Play("f02_discScratch_00");
				Controller.transform.localPosition = new Vector3(0.09425f, 0.0095f, 0.01878f);
				Controller.transform.localEulerAngles = new Vector3(0f, 0f, -180f);
				HomeCamera.Destination = HomeCamera.Destinations[5];
				HomeCamera.Target = HomeCamera.Targets[5];
				Disc.SetActive(true);
				WearPajamas();
			}
		}
		Time.timeScale = 1f;
		UpdateHair();
	}

	private void Update()
	{
		if (!Disc.activeInHierarchy)
		{
			Animation component = Character.GetComponent<Animation>();
			if (CanMove)
			{
				MyController.Move(Physics.gravity * 0.01f);
				float axis = Input.GetAxis("Vertical");
				float axis2 = Input.GetAxis("Horizontal");
				Vector3 vector = Camera.main.transform.TransformDirection(Vector3.forward);
				vector.y = 0f;
				vector = vector.normalized;
				Vector3 vector2 = new Vector3(vector.z, 0f, 0f - vector.x);
				Vector3 vector3 = axis2 * vector2 + axis * vector;
				if (vector3 != Vector3.zero)
				{
					Quaternion b = Quaternion.LookRotation(vector3);
					base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, Time.deltaTime * 10f);
				}
				if (axis != 0f || axis2 != 0f)
				{
					if (Input.GetButton("LB"))
					{
						component.CrossFade("f02_run_00");
						MyController.Move(base.transform.forward * RunSpeed * Time.deltaTime);
					}
					else
					{
						component.CrossFade("f02_newWalk_00");
						MyController.Move(base.transform.forward * WalkSpeed * Time.deltaTime);
					}
				}
				else
				{
					component.CrossFade("f02_idleShort_00");
				}
			}
			else
			{
				component.CrossFade("f02_idleShort_00");
			}
		}
		else if (HomeDarkness.color.a == 0f)
		{
			if (Timer == 0f)
			{
				MyAudio.Play();
			}
			else if (Timer > MyAudio.clip.length + 1f)
			{
				YanvaniaGlobals.DraculaDefeated = false;
				HomeGlobals.MiyukiDefeated = false;
				Disc.SetActive(false);
				HomeVideoGames.Quit();
			}
			Timer += Time.deltaTime;
		}
		Rigidbody component2 = GetComponent<Rigidbody>();
		if (component2 != null)
		{
			component2.velocity = Vector3.zero;
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
			UpdateHair();
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			SchoolGlobals.KidnapVictim = VictimID;
			StudentGlobals.SetStudentSanity(VictimID, 100f);
			SchemeGlobals.SetSchemeStage(6, 5);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			StudentGlobals.MaleUniform = 1;
			StudentGlobals.FemaleUniform = 1;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			StudentGlobals.MaleUniform = 2;
			StudentGlobals.FemaleUniform = 2;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		else if (Input.GetKeyDown(KeyCode.F3))
		{
			StudentGlobals.MaleUniform = 3;
			StudentGlobals.FemaleUniform = 3;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		else if (Input.GetKeyDown(KeyCode.F4))
		{
			StudentGlobals.MaleUniform = 4;
			StudentGlobals.FemaleUniform = 4;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		else if (Input.GetKeyDown(KeyCode.F5))
		{
			StudentGlobals.MaleUniform = 5;
			StudentGlobals.FemaleUniform = 5;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		else if (Input.GetKeyDown(KeyCode.F6))
		{
			StudentGlobals.MaleUniform = 6;
			StudentGlobals.FemaleUniform = 6;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if (base.transform.position.y < -10f)
		{
			base.transform.position = new Vector3(base.transform.position.x, -10f, base.transform.position.z);
		}
	}

	private void LateUpdate()
	{
		if (HidePony)
		{
			Ponytail.parent.transform.localScale = new Vector3(1f, 1f, 0.93f);
			Ponytail.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
			HairR.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
			HairL.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
		}
	}

	private void UpdateHair()
	{
		PigtailR.transform.parent.transform.parent.transform.localScale = new Vector3(1f, 0.75f, 1f);
		PigtailL.transform.parent.transform.parent.transform.localScale = new Vector3(1f, 0.75f, 1f);
		PigtailR.gameObject.SetActive(false);
		PigtailL.gameObject.SetActive(false);
		Drills.gameObject.SetActive(false);
		HidePony = true;
		Hairstyle++;
		if (Hairstyle > 7)
		{
			Hairstyle = 1;
		}
		if (Hairstyle == 1)
		{
			HidePony = false;
			Ponytail.localScale = new Vector3(1f, 1f, 1f);
			HairR.localScale = new Vector3(1f, 1f, 1f);
			HairL.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (Hairstyle == 2)
		{
			PigtailR.gameObject.SetActive(true);
		}
		else if (Hairstyle == 3)
		{
			PigtailL.gameObject.SetActive(true);
		}
		else if (Hairstyle == 4)
		{
			PigtailR.gameObject.SetActive(true);
			PigtailL.gameObject.SetActive(true);
		}
		else if (Hairstyle == 5)
		{
			PigtailR.gameObject.SetActive(true);
			PigtailL.gameObject.SetActive(true);
			HidePony = false;
			Ponytail.localScale = new Vector3(1f, 1f, 1f);
			HairR.localScale = new Vector3(1f, 1f, 1f);
			HairL.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (Hairstyle == 6)
		{
			PigtailR.gameObject.SetActive(true);
			PigtailL.gameObject.SetActive(true);
			PigtailR.transform.parent.transform.parent.transform.localScale = new Vector3(2f, 2f, 2f);
			PigtailL.transform.parent.transform.parent.transform.localScale = new Vector3(2f, 2f, 2f);
		}
		else if (Hairstyle == 7)
		{
			Drills.gameObject.SetActive(true);
		}
	}

	private void ChangeSchoolwear()
	{
		MyRenderer.sharedMesh = Uniforms[StudentGlobals.FemaleUniform];
		MyRenderer.materials[0].mainTexture = UniformTextures[StudentGlobals.FemaleUniform];
		MyRenderer.materials[1].mainTexture = UniformTextures[StudentGlobals.FemaleUniform];
		MyRenderer.materials[2].mainTexture = FaceTexture;
		StartCoroutine(ApplyCustomCostume());
	}

	private void WearPajamas()
	{
		MyRenderer.sharedMesh = PajamaMesh;
		MyRenderer.materials[0].mainTexture = PajamaTexture;
		MyRenderer.materials[1].mainTexture = PajamaTexture;
		MyRenderer.materials[2].mainTexture = FaceTexture;
		StartCoroutine(ApplyCustomFace());
	}

	private IEnumerator ApplyCustomCostume()
	{
		if (StudentGlobals.FemaleUniform == 1)
		{
			WWW CustomUniform = new WWW("file:///" + Application.streamingAssetsPath + "/CustomUniform.png");
			yield return CustomUniform;
			if (CustomUniform.error == null)
			{
				MyRenderer.materials[0].mainTexture = CustomUniform.texture;
				MyRenderer.materials[1].mainTexture = CustomUniform.texture;
			}
		}
		else if (StudentGlobals.FemaleUniform == 2)
		{
			WWW CustomLong = new WWW("file:///" + Application.streamingAssetsPath + "/CustomLong.png");
			yield return CustomLong;
			if (CustomLong.error == null)
			{
				MyRenderer.materials[0].mainTexture = CustomLong.texture;
				MyRenderer.materials[1].mainTexture = CustomLong.texture;
			}
		}
		else if (StudentGlobals.FemaleUniform == 3)
		{
			WWW CustomSweater = new WWW("file:///" + Application.streamingAssetsPath + "/CustomSweater.png");
			yield return CustomSweater;
			if (CustomSweater.error == null)
			{
				MyRenderer.materials[0].mainTexture = CustomSweater.texture;
				MyRenderer.materials[1].mainTexture = CustomSweater.texture;
			}
		}
		else if (StudentGlobals.FemaleUniform == 4 || StudentGlobals.FemaleUniform == 5)
		{
			WWW CustomBlazer = new WWW("file:///" + Application.streamingAssetsPath + "/CustomBlazer.png");
			yield return CustomBlazer;
			if (CustomBlazer.error == null)
			{
				MyRenderer.materials[0].mainTexture = CustomBlazer.texture;
				MyRenderer.materials[1].mainTexture = CustomBlazer.texture;
			}
		}
		StartCoroutine(ApplyCustomFace());
	}

	private IEnumerator ApplyCustomFace()
	{
		WWW CustomFace = new WWW("file:///" + Application.streamingAssetsPath + "/CustomFace.png");
		yield return CustomFace;
		if (CustomFace.error == null)
		{
			MyRenderer.materials[2].mainTexture = CustomFace.texture;
			FaceTexture = CustomFace.texture;
		}
		WWW CustomHair = new WWW("file:///" + Application.streamingAssetsPath + "/CustomHair.png");
		yield return CustomHair;
		if (CustomHair.error == null)
		{
			PonytailRenderer.material.mainTexture = CustomHair.texture;
			PigtailR.material.mainTexture = CustomHair.texture;
			PigtailL.material.mainTexture = CustomHair.texture;
		}
		WWW CustomDrills = new WWW("file:///" + Application.streamingAssetsPath + "/CustomDrills.png");
		yield return CustomDrills;
		if (CustomDrills.error == null)
		{
			Drills.materials[0].mainTexture = CustomDrills.texture;
			Drills.materials[1].mainTexture = CustomDrills.texture;
			Drills.materials[2].mainTexture = CustomDrills.texture;
		}
	}
}
