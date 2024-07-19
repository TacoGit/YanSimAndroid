using UnityEngine;

public class HeartbrokenScript : MonoBehaviour
{
	public ShoulderCameraScript ShoulderCamera;

	public HeartbrokenCursorScript Cursor;

	public CounselorScript Counselor;

	public YandereScript Yandere;

	public ClockScript Clock;

	public AudioListener Listener;

	public AudioClip[] NoticedClips;

	public string[] NoticedLines;

	public UILabel[] Letters;

	public UILabel[] Options;

	public Vector3[] Origins;

	public UISprite Background;

	public UISprite Ground;

	public Camera MainCamera;

	public UILabel Subtitle;

	public GameObject SNAP;

	public AudioClip Slam;

	public bool Headmaster;

	public bool Confessed;

	public bool Arrested;

	public bool Exposed;

	public bool Noticed = true;

	public float AudioTimer;

	public float Timer;

	public int Phase = 1;

	public int LetterID;

	public int ShakeID;

	public int GrowID;

	public int StopID;

	public int ID;

	private void Start()
	{
		if (Yandere.Bloodiness > 0f && !Yandere.RedPaint && !Yandere.Unmasked)
		{
			Arrested = true;
		}
		if (Confessed)
		{
			Letters[0].text = "S";
			Letters[1].text = "E";
			Letters[2].text = "N";
			Letters[3].text = "P";
			Letters[4].text = "A";
			Letters[5].text = "I";
			Letters[6].text = string.Empty;
			Letters[7].text = "L";
			Letters[8].text = "O";
			Letters[9].text = "S";
			Letters[10].text = "T";
			LetterID = 0;
			StopID = 11;
		}
		else if (Yandere.Attacked)
		{
			if (!Headmaster)
			{
				Letters[0].text = string.Empty;
				Letters[1].text = "C";
				Letters[2].text = "O";
				Letters[3].text = "M";
				Letters[4].text = "A";
				Letters[5].text = "T";
				Letters[6].text = "O";
				Letters[7].text = "S";
				Letters[8].text = "E";
				Letters[9].text = string.Empty;
				Letters[10].text = string.Empty;
				Letters[3].fontSize = 250;
				LetterID = 1;
				StopID = 9;
			}
			else
			{
				Letters[0].text = "?";
				Letters[1].text = "?";
				Letters[2].text = "?";
				Letters[3].text = "?";
				Letters[4].text = "?";
				Letters[5].text = "?";
				Letters[6].text = "?";
				Letters[7].text = "?";
				Letters[8].text = "?";
				Letters[9].text = "?";
				Letters[10].text = string.Empty;
				LetterID = 0;
				StopID = 10;
			}
			UILabel[] letters = Letters;
			foreach (UILabel uILabel in letters)
			{
				uILabel.transform.localPosition = new Vector3(uILabel.transform.localPosition.x + 100f, uILabel.transform.localPosition.y, uILabel.transform.localPosition.z);
			}
			SNAP.SetActive(false);
			Cursor.Options = 3;
		}
		else if (Yandere.Lost || ShoulderCamera.LookDown || ShoulderCamera.Counter)
		{
			Letters[0].text = "A";
			Letters[1].text = "P";
			Letters[2].text = "P";
			Letters[3].text = "R";
			Letters[4].text = "E";
			Letters[5].text = "H";
			Letters[6].text = "E";
			Letters[7].text = "N";
			Letters[8].text = "D";
			Letters[9].text = "E";
			Letters[10].text = "D";
			LetterID = 0;
			StopID = 11;
		}
		else if (Arrested)
		{
			Letters[0].text = string.Empty;
			Letters[1].text = "A";
			Letters[2].text = "R";
			Letters[3].text = "R";
			Letters[4].text = "E";
			Letters[5].text = "S";
			Letters[6].text = "T";
			Letters[7].text = "E";
			Letters[8].text = "D";
			Letters[9].text = string.Empty;
			Letters[10].text = string.Empty;
			UILabel[] letters2 = Letters;
			foreach (UILabel uILabel2 in letters2)
			{
				uILabel2.transform.localPosition = new Vector3(uILabel2.transform.localPosition.x + 100f, uILabel2.transform.localPosition.y, uILabel2.transform.localPosition.z);
			}
			LetterID = 1;
			StopID = 9;
		}
		else if (Counselor.Expelled || Yandere.Sprayed)
		{
			Letters[0].text = string.Empty;
			Letters[1].text = "E";
			Letters[2].text = "X";
			Letters[3].text = "P";
			Letters[4].text = "E";
			Letters[5].text = "L";
			Letters[6].text = "L";
			Letters[7].text = "E";
			Letters[8].text = "D";
			Letters[9].text = string.Empty;
			Letters[10].text = string.Empty;
			UILabel[] letters3 = Letters;
			foreach (UILabel uILabel3 in letters3)
			{
				uILabel3.transform.localPosition = new Vector3(uILabel3.transform.localPosition.x + 100f, uILabel3.transform.localPosition.y, uILabel3.transform.localPosition.z);
			}
			LetterID = 1;
			StopID = 9;
		}
		else if (Exposed)
		{
			Letters[0].text = string.Empty;
			Letters[1].text = string.Empty;
			Letters[2].text = "E";
			Letters[3].text = "X";
			Letters[4].text = "P";
			Letters[5].text = "O";
			Letters[6].text = "S";
			Letters[7].text = "E";
			Letters[8].text = "D";
			Letters[9].text = string.Empty;
			Letters[10].text = string.Empty;
			UILabel[] letters4 = Letters;
			foreach (UILabel uILabel4 in letters4)
			{
				uILabel4.transform.localPosition = new Vector3(uILabel4.transform.localPosition.x + 100f, uILabel4.transform.localPosition.y, uILabel4.transform.localPosition.z);
			}
			LetterID = 1;
			StopID = 9;
		}
		else
		{
			LetterID = 0;
			StopID = 11;
		}
		for (ID = 0; ID < Letters.Length; ID++)
		{
			UILabel uILabel5 = Letters[ID];
			uILabel5.transform.localScale = new Vector3(10f, 10f, 1f);
			uILabel5.color = new Color(uILabel5.color.r, uILabel5.color.g, uILabel5.color.b, 0f);
			Origins[ID] = uILabel5.transform.localPosition;
		}
		for (ID = 0; ID < Options.Length; ID++)
		{
			UILabel uILabel6 = Options[ID];
			uILabel6.color = new Color(uILabel6.color.r, uILabel6.color.g, uILabel6.color.b, 0f);
		}
		ID = 0;
		Subtitle.color = new Color(Subtitle.color.r, Subtitle.color.g, Subtitle.color.b, 0f);
		if (Noticed)
		{
			Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, 0f);
			Ground.color = new Color(Ground.color.r, Ground.color.g, Ground.color.b, 0f);
		}
		else
		{
			base.transform.parent.transform.position = new Vector3(base.transform.parent.transform.position.x, 100f, base.transform.parent.transform.position.z);
		}
		Clock.StopTime = true;
	}

	private void Update()
	{
		if (Noticed)
		{
			Ground.transform.eulerAngles = new Vector3(90f, 0f, 0f);
			Ground.transform.position = new Vector3(Ground.transform.position.x, Yandere.transform.position.y, Ground.transform.position.z);
		}
		Timer += Time.deltaTime;
		if (Timer > 3f)
		{
			if (Phase == 1)
			{
				if (Noticed)
				{
					UpdateSubtitle();
				}
				Phase += ((Subtitle.color.a > 0f) ? 1 : 2);
			}
			else if (Phase == 2)
			{
				AudioTimer += Time.deltaTime;
				if (AudioTimer > Subtitle.GetComponent<AudioSource>().clip.length)
				{
					Phase++;
				}
			}
		}
		if (Background.color.a < 1f)
		{
			Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, Background.color.a + Time.deltaTime);
			Ground.color = new Color(Ground.color.r, Ground.color.g, Ground.color.b, Ground.color.a + Time.deltaTime);
			if (Background.color.a >= 1f)
			{
				MainCamera.enabled = false;
			}
		}
		AudioSource component = GetComponent<AudioSource>();
		if (LetterID < StopID)
		{
			UILabel uILabel = Letters[LetterID];
			uILabel.transform.localScale = Vector3.MoveTowards(uILabel.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 100f);
			uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, uILabel.color.a + Time.deltaTime * 10f);
			if (uILabel.transform.localScale == new Vector3(1f, 1f, 1f))
			{
				component.PlayOneShot(Slam);
				LetterID++;
				if (LetterID == StopID)
				{
					ID = 0;
				}
			}
		}
		else if (Phase == 3)
		{
			if (Options[0].color.a == 0f)
			{
				Subtitle.color = new Color(Subtitle.color.r, Subtitle.color.g, Subtitle.color.b, 0f);
				component.Play();
			}
			if (ID < Options.Length)
			{
				UILabel uILabel2 = Options[ID];
				uILabel2.color = new Color(uILabel2.color.r, uILabel2.color.g, uILabel2.color.b, uILabel2.color.a + Time.deltaTime * 2f);
				if (uILabel2.color.a >= 1f)
				{
					ID++;
				}
			}
		}
		for (ShakeID = 0; ShakeID < Letters.Length; ShakeID++)
		{
			UILabel uILabel3 = Letters[ShakeID];
			Vector3 vector = Origins[ShakeID];
			uILabel3.transform.localPosition = new Vector3(vector.x + Random.Range(-5f, 5f), vector.y + Random.Range(-5f, 5f), uILabel3.transform.localPosition.z);
		}
		for (GrowID = 0; GrowID < 4; GrowID++)
		{
			UILabel uILabel4 = Options[GrowID];
			uILabel4.transform.localScale = Vector3.Lerp(uILabel4.transform.localScale, (Cursor.Selected - 1 == GrowID) ? new Vector3(1f, 1f, 1f) : new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 10f);
		}
	}

	private void UpdateSubtitle()
	{
		StudentScript component = Yandere.Senpai.GetComponent<StudentScript>();
		if (!component.Teacher && Yandere.Noticed)
		{
			Subtitle.color = new Color(Subtitle.color.r, Subtitle.color.g, Subtitle.color.b, 1f);
			GameOverType gameOverCause = component.GameOverCause;
			int num = 0;
			switch (gameOverCause)
			{
			case GameOverType.Stalking:
				num = 4;
				break;
			case GameOverType.Insanity:
				num = 3;
				break;
			case GameOverType.Weapon:
				num = 2;
				break;
			case GameOverType.Murder:
				num = 5;
				break;
			case GameOverType.Blood:
				num = 1;
				break;
			case GameOverType.Lewd:
				num = 6;
				break;
			}
			Subtitle.text = NoticedLines[num];
			Subtitle.GetComponent<AudioSource>().clip = NoticedClips[num];
			Subtitle.GetComponent<AudioSource>().Play();
		}
		else if (Headmaster)
		{
			Subtitle.color = new Color(Subtitle.color.r, Subtitle.color.g, Subtitle.color.b, 1f);
			Subtitle.text = NoticedLines[7];
			Subtitle.GetComponent<AudioSource>().clip = NoticedClips[7];
			Subtitle.GetComponent<AudioSource>().Play();
		}
	}
}
