using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[SerializeField]
	private UILabel loadingText;

	[SerializeField]
	private UILabel crashText;

	private float timer;

	public GameObject LightAnimation;

	public GameObject DarkAnimation;

	private void Start()
	{
		Shader.WarmupAllShaders();
		Time.timeScale = 1f;
		if (!SchoolGlobals.SchoolAtmosphereSet)
		{
			SchoolGlobals.SchoolAtmosphereSet = true;
			SchoolGlobals.SchoolAtmosphere = 1f;
		}
		if (SchoolGlobals.SchoolAtmosphere < 0.5f || GameGlobals.LoveSick)
		{
			Camera.main.backgroundColor = new Color(0f, 0f, 0f, 1f);
			loadingText.color = new Color(1f, 0f, 0f, 1f);
			crashText.color = new Color(1f, 0f, 0f, 1f);
			LightAnimation.SetActive(false);
			DarkAnimation.SetActive(true);
		}
		StartCoroutine(LoadNewScene());
	}

	private void Update()
	{
	}

	private IEnumerator LoadNewScene()
	{
		AsyncOperation async = SceneManager.LoadSceneAsync("SchoolScene");
		while (!async.isDone)
		{
			yield return null;
		}
	}
}
