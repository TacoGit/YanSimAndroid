using UnityEngine;

public class TrashCompactorScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public JsonScript JSON;

	public UIPanel HUD;

	public GameObject Jukebox;

	public Transform TrashCompactorObject;

	public Transform Yandere;

	public float Speed;

	private void Start()
	{
		if (StudentManager.Students[10] != null || StudentManager.Students[11] != null)
		{
			CompactTrash();
			return;
		}
		for (int i = 1; i < 101; i++)
		{
			if (StudentManager.Students[i] != null && !StudentManager.Students[i].Male && (StudentManager.Students[i].Cosmetic.Hairstyle == 20 || StudentManager.Students[i].Cosmetic.Hairstyle == 21))
			{
				CompactTrash();
			}
		}
	}

	private void Update()
	{
		if (TrashCompactorObject.gameObject.activeInHierarchy)
		{
			Speed += Time.deltaTime * 0.01f;
			TrashCompactorObject.position = Vector3.MoveTowards(TrashCompactorObject.position, Yandere.position, Time.deltaTime * Speed);
			TrashCompactorObject.LookAt(Yandere.position);
			if (Vector3.Distance(TrashCompactorObject.position, Yandere.position) < 0.5f)
			{
				Application.Quit();
			}
		}
	}

	private void CompactTrash()
	{
		if (TrashCompactorObject.gameObject.activeInHierarchy)
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
		TrashCompactorObject.gameObject.SetActive(true);
		Jukebox.SetActive(false);
		HUD.enabled = false;
	}
}
