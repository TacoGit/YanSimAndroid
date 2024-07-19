using UnityEngine;
using UnityEngine.SceneManagement;

public class AntiCheatScript : MonoBehaviour
{
	public GameObject Jukebox;

	public bool Check;

	private void Update()
	{
		if (Check && !GetComponent<AudioSource>().isPlaying)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "YandereChan")
		{
			Jukebox.SetActive(false);
			Check = true;
			GetComponent<AudioSource>().Play();
		}
	}
}
