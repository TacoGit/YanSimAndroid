using UnityEngine;

public class MemorialSceneScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public GameObject[] Canvases;

	public UITexture[] Portraits;

	public GameObject CanvasGroup;

	public GameObject Headmaster;

	public GameObject Counselor;

	public float Speed;

	public bool Eulogized;

	public bool FadeOut;

	private void Start()
	{
		if (StudentGlobals.MemorialStudents % 2 == 0)
		{
			CanvasGroup.transform.localPosition = new Vector3(-0.5f, 0f, -2f);
		}
		int num = 0;
		int i;
		for (i = 1; i < 10; i++)
		{
			Canvases[i].SetActive(false);
		}
		i = 0;
		while (StudentGlobals.MemorialStudents > 0)
		{
			i++;
			Canvases[i].SetActive(true);
			if (StudentGlobals.MemorialStudents == 1)
			{
				num = StudentGlobals.MemorialStudent1;
			}
			else if (StudentGlobals.MemorialStudents == 2)
			{
				num = StudentGlobals.MemorialStudent2;
			}
			else if (StudentGlobals.MemorialStudents == 3)
			{
				num = StudentGlobals.MemorialStudent3;
			}
			else if (StudentGlobals.MemorialStudents == 4)
			{
				num = StudentGlobals.MemorialStudent4;
			}
			else if (StudentGlobals.MemorialStudents == 5)
			{
				num = StudentGlobals.MemorialStudent5;
			}
			else if (StudentGlobals.MemorialStudents == 6)
			{
				num = StudentGlobals.MemorialStudent6;
			}
			else if (StudentGlobals.MemorialStudents == 7)
			{
				num = StudentGlobals.MemorialStudent7;
			}
			else if (StudentGlobals.MemorialStudents == 8)
			{
				num = StudentGlobals.MemorialStudent8;
			}
			else if (StudentGlobals.MemorialStudents == 9)
			{
				num = StudentGlobals.MemorialStudent9;
			}
			string url = "file:///" + Application.streamingAssetsPath + "/Portraits/Student_" + num + ".png";
			WWW wWW = new WWW(url);
			Portraits[i].mainTexture = wWW.texture;
			StudentGlobals.MemorialStudents--;
		}
	}

	private void Update()
	{
		Speed += Time.deltaTime;
		if (Speed > 1f)
		{
			if (!Eulogized)
			{
				StudentManager.Yandere.Subtitle.UpdateLabel(SubtitleType.Eulogy, 0, 8f);
				StudentManager.Yandere.PromptBar.Label[0].text = "Continue";
				StudentManager.Yandere.PromptBar.UpdateButtons();
				StudentManager.Yandere.PromptBar.Show = true;
				Eulogized = true;
			}
			StudentManager.MainCamera.position = Vector3.Lerp(StudentManager.MainCamera.position, new Vector3(38f, 4.125f, 68.825f), (Speed - 1f) * Time.deltaTime * 0.15f);
			if (Input.GetButtonDown("A"))
			{
				StudentManager.Yandere.PromptBar.Show = false;
				FadeOut = true;
			}
		}
		if (FadeOut)
		{
			StudentManager.Clock.BloomEffect.bloomIntensity += Time.deltaTime * 10f;
			if (StudentManager.Clock.BloomEffect.bloomIntensity > 10f)
			{
				StudentManager.Yandere.Casual = !StudentManager.Yandere.Casual;
				StudentManager.Yandere.ChangeSchoolwear();
				StudentManager.Yandere.transform.position = new Vector3(12f, 0f, 72f);
				StudentManager.Yandere.transform.eulerAngles = new Vector3(0f, -90f, 0f);
				StudentManager.Yandere.HeartCamera.enabled = true;
				StudentManager.Yandere.RPGCamera.enabled = true;
				StudentManager.Yandere.CanMove = true;
				StudentManager.Yandere.HUD.alpha = 1f;
				StudentManager.Clock.UpdateBloom = true;
				StudentManager.Clock.StopTime = false;
				StudentManager.Clock.PresentTime = 450f;
				StudentManager.Clock.HourTime = 7.5f;
				StudentManager.Unstop();
				StudentManager.SkipTo8();
				Headmaster.SetActive(false);
				Counselor.SetActive(false);
				base.enabled = false;
			}
		}
	}
}
