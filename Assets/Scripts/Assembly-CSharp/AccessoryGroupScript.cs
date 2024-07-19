using UnityEngine;

public class AccessoryGroupScript : MonoBehaviour
{
	public GameObject[] Parts;

	public void SetPartsActive(bool active)
	{
		GameObject[] parts = Parts;
		foreach (GameObject gameObject in parts)
		{
			gameObject.SetActive(active);
		}
	}
}
