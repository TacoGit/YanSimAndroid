using UnityEngine;

public class TitleSaveFilesScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public TitleSaveDataScript[] SaveDatas;

	public GameObject ConfirmationWindow;

	public TitleMenuScript Menu;

	public Transform Highlight;

	public bool Show;

	public int ID = 1;

	private void Start()
	{
		base.transform.localPosition = new Vector3(1050f, base.transform.localPosition.y, base.transform.localPosition.z);
		UpdateHighlight();
	}

	private void Update()
	{
		if (!Show)
		{
			base.transform.localPosition = new Vector3(Mathf.Lerp(base.transform.localPosition.x, 1050f, Time.deltaTime * 10f), base.transform.localPosition.y, base.transform.localPosition.z);
			return;
		}
		base.transform.localPosition = new Vector3(Mathf.Lerp(base.transform.localPosition.x, 0f, Time.deltaTime * 10f), base.transform.localPosition.y, base.transform.localPosition.z);
		if (!ConfirmationWindow.activeInHierarchy)
		{
			if (InputManager.TappedDown)
			{
				ID++;
				if (ID > 3)
				{
					ID = 1;
				}
				UpdateHighlight();
			}
			if (InputManager.TappedUp)
			{
				ID--;
				if (ID < 1)
				{
					ID = 3;
				}
				UpdateHighlight();
			}
		}
		if (!(base.transform.localPosition.x < 50f))
		{
			return;
		}
		if (!ConfirmationWindow.activeInHierarchy)
		{
			if (Input.GetButtonDown("A"))
			{
				Debug.Log("ID is: " + ID);
				GameGlobals.Profile = ID;
				Debug.Log("GameGlobals.Profile is: " + GameGlobals.Profile);
				Menu.FadeOut = true;
				Menu.Fading = true;
			}
			else if (Input.GetButtonDown("X"))
			{
				ConfirmationWindow.SetActive(true);
			}
		}
		else if (Input.GetButtonDown("A"))
		{
			PlayerPrefs.SetInt("ProfileCreated_" + ID, 0);
			ConfirmationWindow.SetActive(false);
			SaveDatas[ID].Start();
		}
		else if (Input.GetButtonDown("B"))
		{
			ConfirmationWindow.SetActive(false);
		}
	}

	private void UpdateHighlight()
	{
		Highlight.localPosition = new Vector3(0f, 700f - 350f * (float)ID, 0f);
	}
}
