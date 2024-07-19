using UnityEngine;

public class RooftopScript : MonoBehaviour
{
	public GameObject[] DumpPoints;

	public GameObject Railing;

	public GameObject Fence;

	private void Start()
	{
		if (SchoolGlobals.RoofFence)
		{
			GameObject[] dumpPoints = DumpPoints;
			foreach (GameObject gameObject in dumpPoints)
			{
				gameObject.SetActive(false);
			}
			Railing.SetActive(false);
			Fence.SetActive(true);
		}
	}
}
