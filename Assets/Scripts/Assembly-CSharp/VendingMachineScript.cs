using UnityEngine;

public class VendingMachineScript : MonoBehaviour
{
	public PromptScript Prompt;

	public Transform CanSpawn;

	public GameObject[] Cans;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			GameObject gameObject = Object.Instantiate(Cans[Random.Range(0, Cans.Length)], CanSpawn.position, CanSpawn.rotation);
			gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
		}
	}
}
