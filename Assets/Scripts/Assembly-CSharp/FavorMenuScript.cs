using UnityEngine;

public class FavorMenuScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public PauseScreenScript PauseScreen;

	public ServicesScript ServicesMenu;

	public SchemesScript SchemesMenu;

	public DropsScript DropsMenu;

	public PromptBarScript PromptBar;

	public Transform Highlight;

	public int ID = 1;

	private void Update()
	{
		if (InputManager.TappedRight)
		{
			ID++;
			UpdateHighlight();
		}
		else if (InputManager.TappedLeft)
		{
			ID--;
			UpdateHighlight();
		}
		if (Input.GetButtonDown("A"))
		{
			PromptBar.ClearButtons();
			PromptBar.Label[0].text = "Accept";
			PromptBar.Label[1].text = "Exit";
			PromptBar.Label[4].text = "Choose";
			PromptBar.UpdateButtons();
			if (ID == 1)
			{
				SchemesMenu.UpdatePantyCount();
				SchemesMenu.UpdateSchemeList();
				SchemesMenu.UpdateSchemeInfo();
				SchemesMenu.gameObject.SetActive(true);
				base.gameObject.SetActive(false);
			}
			else if (ID == 2)
			{
				ServicesMenu.UpdatePantyCount();
				ServicesMenu.UpdateList();
				ServicesMenu.UpdateDesc();
				ServicesMenu.gameObject.SetActive(true);
				base.gameObject.SetActive(false);
			}
			else if (ID == 3)
			{
				DropsMenu.UpdatePantyCount();
				DropsMenu.UpdateList();
				DropsMenu.UpdateDesc();
				DropsMenu.gameObject.SetActive(true);
				base.gameObject.SetActive(false);
			}
		}
		if (Input.GetButtonDown("B"))
		{
			PromptBar.ClearButtons();
			PromptBar.Label[0].text = "Accept";
			PromptBar.Label[1].text = "Exit";
			PromptBar.Label[4].text = "Choose";
			PromptBar.UpdateButtons();
			PauseScreen.MainMenu.SetActive(true);
			PauseScreen.Sideways = false;
			PauseScreen.PressedB = true;
			base.gameObject.SetActive(false);
		}
	}

	private void UpdateHighlight()
	{
		if (ID > 3)
		{
			ID = 1;
		}
		else if (ID < 1)
		{
			ID = 3;
		}
		Highlight.transform.localPosition = new Vector3(-500f + 250f * (float)ID, Highlight.transform.localPosition.y, Highlight.transform.localPosition.z);
	}
}
