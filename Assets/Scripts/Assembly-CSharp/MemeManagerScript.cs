using UnityEngine;

public class MemeManagerScript : MonoBehaviour
{
	[SerializeField]
	private GameObject[] Memes;

	private void Start()
	{
		if (GameGlobals.LoveSick)
		{
			GameObject[] memes = Memes;
			foreach (GameObject gameObject in memes)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
