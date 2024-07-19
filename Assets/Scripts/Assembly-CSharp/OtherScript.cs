using UnityEngine;

public class OtherScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public JsonScript JSON;

	public UIPanel HUD;

	public GameObject Jukebox;

	public Transform Yandere;

	public Transform Other;

	public float Speed;

	private void Start()
	{
		if (JSON.Students[11].Name != "Reserved")
		{
			Application.Quit();
			Wow();
			return;
		}
		for (int i = 1; i < 101; i++)
		{
			if (JSON.Students[i].Gender == 0 && JSON.Students[i].Hairstyle == "20" && StudentManager.Students[i] != null)
			{
				StudentManager.Students[i].gameObject.SetActive(false);
			}
		}
	}

	private void Update()
	{
		if (Other.gameObject.activeInHierarchy)
		{
			Speed += Time.deltaTime * 0.01f;
			Other.position = Vector3.MoveTowards(Other.position, Yandere.position, Time.deltaTime * Speed);
			Other.LookAt(Yandere.position);
			if (Vector3.Distance(Other.position, Yandere.position) < 0.5f)
			{
				Application.Quit();
			}
		}
	}

	private void Wow()
	{
		if (Other.gameObject.activeInHierarchy)
		{
			return;
		}
		SchoolGlobals.SchoolAtmosphereSet = true;
		SchoolGlobals.SchoolAtmosphere = 0f;
		StudentManager.SetAtmosphere();
		StudentScript[] students = StudentManager.Students;
		foreach (StudentScript studentScript in students)
		{
			if (studentScript != null)
			{
				studentScript.gameObject.SetActive(false);
			}
		}
		Yandere.gameObject.GetComponent<YandereScript>().NoDebug = true;
		Other.gameObject.SetActive(true);
		Jukebox.SetActive(false);
		HUD.enabled = false;
	}
}
