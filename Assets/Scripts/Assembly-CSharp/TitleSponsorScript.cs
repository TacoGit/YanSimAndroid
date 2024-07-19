using UnityEngine;

public class TitleSponsorScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public string[] SponsorURLs;

	public string[] Sponsors;

	public UILabel SponsorName;

	public Transform Highlight;

	public bool Show;

	public int Columns;

	public int Rows;

	private int Column;

	private int Row;

	public UISprite BlackSprite;

	public UISprite[] RedSprites;

	public UILabel[] Labels;

	private void Start()
	{
		base.transform.localPosition = new Vector3(1050f, base.transform.localPosition.y, base.transform.localPosition.z);
		UpdateHighlight();
		if (GameGlobals.LoveSick)
		{
			TurnLoveSick();
		}
	}

	public int GetSponsorIndex()
	{
		return Column + Row * Columns;
	}

	public bool SponsorHasWebsite(int index)
	{
		return !string.IsNullOrEmpty(SponsorURLs[index]);
	}

	private void Update()
	{
		if (!Show)
		{
			base.transform.localPosition = new Vector3(Mathf.Lerp(base.transform.localPosition.x, 1050f, Time.deltaTime * 10f), base.transform.localPosition.y, base.transform.localPosition.z);
			return;
		}
		base.transform.localPosition = new Vector3(Mathf.Lerp(base.transform.localPosition.x, 0f, Time.deltaTime * 10f), base.transform.localPosition.y, base.transform.localPosition.z);
		if (InputManager.TappedUp)
		{
			Row = ((Row <= 0) ? (Rows - 1) : (Row - 1));
		}
		if (InputManager.TappedDown)
		{
			Row = ((Row < Rows - 1) ? (Row + 1) : 0);
		}
		if (InputManager.TappedRight)
		{
			Column = ((Column < Columns - 1) ? (Column + 1) : 0);
		}
		if (InputManager.TappedLeft)
		{
			Column = ((Column <= 0) ? (Columns - 1) : (Column - 1));
		}
		if (InputManager.TappedUp || InputManager.TappedDown || InputManager.TappedRight || InputManager.TappedLeft)
		{
			UpdateHighlight();
		}
		if (Input.GetButtonDown("A"))
		{
			int sponsorIndex = GetSponsorIndex();
			if (SponsorHasWebsite(sponsorIndex))
			{
				Application.OpenURL(SponsorURLs[sponsorIndex]);
			}
		}
	}

	private void UpdateHighlight()
	{
		Highlight.localPosition = new Vector3(-384f + (float)Column * 256f, 128f - (float)Row * 256f, Highlight.localPosition.z);
		SponsorName.text = Sponsors[GetSponsorIndex()];
	}

	private void TurnLoveSick()
	{
		BlackSprite.color = Color.black;
		UISprite[] redSprites = RedSprites;
		foreach (UISprite uISprite in redSprites)
		{
			uISprite.color = new Color(1f, 0f, 0f, uISprite.color.a);
		}
		UILabel[] labels = Labels;
		foreach (UILabel uILabel in labels)
		{
			uILabel.color = new Color(1f, 0f, 0f, uILabel.color.a);
		}
	}
}
