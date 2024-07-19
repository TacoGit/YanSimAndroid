using UnityEngine;
using UnityEngine.SceneManagement;

public class PhoneScript : MonoBehaviour
{
	public GameObject[] RightMessage;

	public GameObject[] LeftMessage;

	public AudioClip[] VoiceClips;

	public GameObject NewMessage;

	public AudioSource Jukebox;

	public Transform OldMessages;

	public Transform Buttons;

	public Transform Panel;

	public Vignetting Vignette;

	public UISprite Darkness;

	public UISprite Sprite;

	public int[] Speaker;

	public string[] Text;

	public int[] Height;

	public AudioClip[] KidnapClip;

	public int[] KidnapSpeaker;

	public string[] KidnapText;

	public int[] KidnapHeight;

	public AudioClip[] BefriendClip;

	public int[] BefriendSpeaker;

	public string[] BefriendText;

	public int[] BefriendHeight;

	public bool FadeOut;

	public bool Auto;

	public float AutoLimit;

	public float AutoTimer;

	public float Timer;

	public int ID;

	private void Start()
	{
		Buttons.localPosition = new Vector3(Buttons.localPosition.x, -135f, Buttons.localPosition.z);
		Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 1f);
		if (EventGlobals.KidnapConversation)
		{
			VoiceClips = KidnapClip;
			Speaker = KidnapSpeaker;
			Text = KidnapText;
			Height = KidnapHeight;
			EventGlobals.BefriendConversation = true;
			EventGlobals.KidnapConversation = false;
		}
		else if (EventGlobals.BefriendConversation)
		{
			VoiceClips = BefriendClip;
			Speaker = BefriendSpeaker;
			Text = BefriendText;
			Height = BefriendHeight;
			EventGlobals.LivingRoom = true;
			EventGlobals.BefriendConversation = false;
		}
		if (GameGlobals.LoveSick)
		{
			Camera.main.backgroundColor = Color.black;
			LoveSickColorSwap();
		}
	}

	private void Update()
	{
		if (!FadeOut)
		{
			if (Timer > 0f)
			{
				Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime));
				if (Darkness.color.a == 0f)
				{
					if (!Jukebox.isPlaying)
					{
						Jukebox.Play();
					}
					if (NewMessage == null)
					{
						SpawnMessage();
					}
				}
			}
			if (NewMessage != null)
			{
				Buttons.localPosition = new Vector3(Buttons.localPosition.x, Mathf.Lerp(Buttons.localPosition.y, 0f, Time.deltaTime * 10f), Buttons.localPosition.z);
				AutoTimer += Time.deltaTime;
				if ((Auto && AutoTimer > VoiceClips[ID].length + 1f) || Input.GetButtonDown("A"))
				{
					AutoTimer = 0f;
					if (ID < Text.Length - 1)
					{
						ID++;
						SpawnMessage();
					}
					else
					{
						Darkness.color = new Color(0f, 0f, 0f, 0f);
						FadeOut = true;
					}
				}
				if (Input.GetButtonDown("X"))
				{
					FadeOut = true;
				}
			}
		}
		else
		{
			Buttons.localPosition = new Vector3(Buttons.localPosition.x, Mathf.Lerp(Buttons.localPosition.y, -135f, Time.deltaTime * 10f), Buttons.localPosition.z);
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Darkness.color.a + Time.deltaTime);
			GetComponent<AudioSource>().volume = 1f - Darkness.color.a;
			Jukebox.volume = 1f - Darkness.color.a;
			if (Darkness.color.a >= 1f)
			{
				if (!EventGlobals.BefriendConversation && !EventGlobals.LivingRoom)
				{
					SceneManager.LoadScene("CalendarScene");
				}
				else if (EventGlobals.LivingRoom)
				{
					SceneManager.LoadScene("LivingRoomScene");
				}
				else
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}
		}
		Timer += Time.deltaTime;
	}

	private void SpawnMessage()
	{
		if (NewMessage != null)
		{
			NewMessage.transform.parent = OldMessages;
			OldMessages.localPosition = new Vector3(OldMessages.localPosition.x, OldMessages.localPosition.y + (72f + (float)Height[ID] * 32f), OldMessages.localPosition.z);
		}
		AudioSource component = GetComponent<AudioSource>();
		component.clip = VoiceClips[ID];
		component.Play();
		if (Speaker[ID] == 1)
		{
			NewMessage = Object.Instantiate(LeftMessage[Height[ID]]);
			NewMessage.transform.parent = Panel;
			NewMessage.transform.localPosition = new Vector3(-225f, -375f, 0f);
			NewMessage.transform.localScale = Vector3.zero;
		}
		else
		{
			NewMessage = Object.Instantiate(RightMessage[Height[ID]]);
			NewMessage.transform.parent = Panel;
			NewMessage.transform.localPosition = new Vector3(225f, -375f, 0f);
			NewMessage.transform.localScale = Vector3.zero;
			if (Speaker == KidnapSpeaker && Height[ID] == 8)
			{
				NewMessage.GetComponent<TextMessageScript>().Attachment = true;
			}
		}
		AutoLimit = Height[ID] + 1;
		NewMessage.GetComponent<TextMessageScript>().Label.text = Text[ID];
	}

	private void LoveSickColorSwap()
	{
		GameObject[] array = Object.FindObjectsOfType<GameObject>();
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			UISprite component = gameObject.GetComponent<UISprite>();
			if (component != null && component.color != Color.black && (bool)component.transform.parent)
			{
				component.color = new Color(1f, 0f, 0f, component.color.a);
			}
			UILabel component2 = gameObject.GetComponent<UILabel>();
			if (component2 != null && component2.color != Color.black)
			{
				component2.color = new Color(1f, 0f, 0f, component2.color.a);
			}
			Darkness.color = Color.black;
		}
	}
}
