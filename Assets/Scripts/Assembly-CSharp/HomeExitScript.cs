using UnityEngine;

public class HomeExitScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public HomeDarknessScript HomeDarkness;

	public HomeYandereScript HomeYandere;

	public HomeCameraScript HomeCamera;

	public HomeWindowScript HomeWindow;

	public Transform Highlight;

	public UILabel[] Labels;

	public int ID = 1;

	private void Start()
	{
		UILabel uILabel = Labels[2];
		uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0.5f);
		if (HomeGlobals.Night)
		{
			UILabel uILabel2 = Labels[1];
			uILabel2.color = new Color(uILabel2.color.r, uILabel2.color.g, uILabel2.color.b, 0.5f);
			uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0.5f);
		}
	}

	private void Update()
	{
		if (HomeYandere.CanMove || HomeDarkness.FadeOut)
		{
			return;
		}
		if (InputManager.TappedDown)
		{
			ID++;
			if (ID > 3)
			{
				ID = 1;
			}
			Highlight.localPosition = new Vector3(Highlight.localPosition.x, 50f - (float)ID * 50f, Highlight.localPosition.z);
		}
		if (InputManager.TappedUp)
		{
			ID--;
			if (ID < 1)
			{
				ID = 3;
			}
			Highlight.localPosition = new Vector3(Highlight.localPosition.x, 50f - (float)ID * 50f, Highlight.localPosition.z);
		}
		if (Input.GetButtonDown("A") && ID != 2 && (!HomeGlobals.Night || (HomeGlobals.Night && ID == 3)))
		{
			if (ID < 3 && SchoolGlobals.SchoolAtmosphere >= 0.5f)
			{
				HomeDarkness.Sprite.color = new Color(1f, 1f, 1f, 0f);
			}
			HomeDarkness.FadeOut = true;
			HomeWindow.Show = false;
			base.enabled = false;
		}
		if (Input.GetButtonDown("B"))
		{
			HomeCamera.Destination = HomeCamera.Destinations[0];
			HomeCamera.Target = HomeCamera.Targets[0];
			HomeYandere.CanMove = true;
			HomeWindow.Show = false;
			base.enabled = false;
		}
	}
}
