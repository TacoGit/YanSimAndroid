using UnityEngine;

public class CheckmarkScript : MonoBehaviour
{
	public GameObject[] Checkmarks;

	public int ID;

	private void Start()
	{
		while (ID < Checkmarks.Length)
		{
			Checkmarks[ID].SetActive(false);
			ID++;
		}
		ID = 0;
	}

	private void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			ID = Random.Range(0, Checkmarks.Length);
			while (Checkmarks[ID].active)
			{
				ID = Random.Range(0, Checkmarks.Length);
			}
			Checkmarks[ID].SetActive(true);
		}
	}
}
