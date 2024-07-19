using UnityEngine;

public class HologramScript : MonoBehaviour
{
	public GameObject[] Holograms;

	public void UpdateHolograms()
	{
		GameObject[] holograms = Holograms;
		foreach (GameObject gameObject in holograms)
		{
			gameObject.SetActive(TrueFalse());
		}
	}

	private bool TrueFalse()
	{
		if (Random.value >= 0.5f)
		{
			return true;
		}
		return false;
	}
}
