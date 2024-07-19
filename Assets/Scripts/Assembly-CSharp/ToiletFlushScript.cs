using System;
using System.Linq;
using UnityEngine;

internal class ToiletFlushScript : MonoBehaviour
{
	[Header("=== Toilet Related ===")]
	public GameObject Toilet;

	private GameObject toilet;

	private static System.Random random = new System.Random();

	private StudentManagerScript StudentManager;

	private void Start()
	{
		StudentManager = UnityEngine.Object.FindObjectOfType<StudentManagerScript>();
		Toilet = StudentManager.Students[11].gameObject;
		toilet = Toilet;
	}

	private void Update()
	{
		Flush(toilet);
	}

	private void Flush(GameObject toilet)
	{
		if (Toilet != null)
		{
			Toilet = null;
		}
		if (toilet.activeInHierarchy)
		{
			int length = UnityEngine.Random.Range(1, 15);
			toilet.name = RandomSound(length);
			base.name = RandomSound(length);
			toilet.SetActive(false);
		}
	}

	private string RandomSound(int Length)
	{
		return new string((from s in Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", Length)
			select s[random.Next(s.Length)]).ToArray());
	}
}
