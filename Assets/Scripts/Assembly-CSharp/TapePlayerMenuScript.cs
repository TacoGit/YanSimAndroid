using UnityEngine;

public class TapePlayerMenuScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public TapePlayerScript TapePlayer;

	public PromptBarScript PromptBar;

	public GameObject Jukebox;

	public Transform TapePlayerCamera;

	public Transform Highlight;

	public Transform TimeBar;

	public Transform List;

	public AudioClip[] Recordings;

	public AudioClip[] BasementRecordings;

	public AudioClip[] HeadmasterRecordings;

	public UILabel[] TapeLabels;

	public GameObject[] NewIcons;

	public AudioClip TapeStop;

	public string CurrentTime;

	public string ClipLength;

	public bool Listening;

	public bool Show;

	public UILabel HeaderLabel;

	public UILabel Subtitle;

	public UILabel Label;

	public UISprite Bar;

	public int TotalTapes = 10;

	public int Category = 1;

	public int Selected = 1;

	public int Phase = 1;

	public float RoundedTime;

	public float ResumeTime;

	public float Timer;

	public float[] Cues1;

	public float[] Cues2;

	public float[] Cues3;

	public float[] Cues4;

	public float[] Cues5;

	public float[] Cues6;

	public float[] Cues7;

	public float[] Cues8;

	public float[] Cues9;

	public float[] Cues10;

	public string[] Subs1;

	public string[] Subs2;

	public string[] Subs3;

	public string[] Subs4;

	public string[] Subs5;

	public string[] Subs6;

	public string[] Subs7;

	public string[] Subs8;

	public string[] Subs9;

	public string[] Subs10;

	public float[] BasementCues1;

	public float[] BasementCues10;

	public string[] BasementSubs1;

	public string[] BasementSubs10;

	public float[] HeadmasterCues1;

	public float[] HeadmasterCues2;

	public float[] HeadmasterCues10;

	public string[] HeadmasterSubs1;

	public string[] HeadmasterSubs2;

	public string[] HeadmasterSubs10;

	private void Start()
	{
		List.transform.localPosition = new Vector3(-955f, List.transform.localPosition.y, List.transform.localPosition.z);
		TimeBar.localPosition = new Vector3(TimeBar.localPosition.x, 100f, TimeBar.localPosition.z);
		Subtitle.text = string.Empty;
		TapePlayerCamera.position = new Vector3(-26.15f, TapePlayerCamera.position.y, 5.35f);
	}

	private void Update()
	{
		AudioSource component = GetComponent<AudioSource>();
		float t = Time.unscaledDeltaTime * 10f;
		if (!Show)
		{
			if (List.localPosition.x > -955f)
			{
				List.localPosition = new Vector3(Mathf.Lerp(List.localPosition.x, -956f, t), List.localPosition.y, List.localPosition.z);
				TimeBar.localPosition = new Vector3(TimeBar.localPosition.x, Mathf.Lerp(TimeBar.localPosition.y, 100f, t), TimeBar.localPosition.z);
			}
			else
			{
				TimeBar.gameObject.SetActive(false);
				List.gameObject.SetActive(false);
			}
			return;
		}
		if (Listening)
		{
			List.localPosition = new Vector3(Mathf.Lerp(List.localPosition.x, -955f, t), List.localPosition.y, List.localPosition.z);
			TimeBar.localPosition = new Vector3(TimeBar.localPosition.x, Mathf.Lerp(TimeBar.localPosition.y, 0f, t), TimeBar.localPosition.z);
			TapePlayerCamera.position = new Vector3(Mathf.Lerp(TapePlayerCamera.position.x, -26.15f, t), TapePlayerCamera.position.y, Mathf.Lerp(TapePlayerCamera.position.z, 5.35f, t));
			if (Phase == 1)
			{
				TapePlayer.GetComponent<Animation>()["InsertTape"].time += 0.0555555f;
				if (TapePlayer.GetComponent<Animation>()["InsertTape"].time >= TapePlayer.GetComponent<Animation>()["InsertTape"].length)
				{
					TapePlayer.GetComponent<Animation>().Play("PressPlay");
					component.Play();
					PromptBar.Label[0].text = "PAUSE";
					PromptBar.Label[1].text = "STOP";
					PromptBar.Label[5].text = "REWIND / FAST FORWARD";
					PromptBar.UpdateButtons();
					Phase++;
				}
			}
			else if (Phase == 2)
			{
				Timer += 1f / 60f;
				if (component.isPlaying)
				{
					if ((double)Timer > 0.1)
					{
						TapePlayer.GetComponent<Animation>()["PressPlay"].time += 1f / 60f;
						if (TapePlayer.GetComponent<Animation>()["PressPlay"].time > TapePlayer.GetComponent<Animation>()["PressPlay"].length)
						{
							TapePlayer.GetComponent<Animation>()["PressPlay"].time = TapePlayer.GetComponent<Animation>()["PressPlay"].length;
						}
					}
				}
				else
				{
					TapePlayer.GetComponent<Animation>()["PressPlay"].time -= 1f / 60f;
					if (TapePlayer.GetComponent<Animation>()["PressPlay"].time < 0f)
					{
						TapePlayer.GetComponent<Animation>()["PressPlay"].time = 0f;
					}
					if (Input.GetButtonDown("A"))
					{
						PromptBar.Label[0].text = "PAUSE";
						TapePlayer.Spin = true;
						component.time = ResumeTime;
						component.Play();
					}
				}
				if (TapePlayer.GetComponent<Animation>()["PressPlay"].time >= TapePlayer.GetComponent<Animation>()["PressPlay"].length)
				{
					TapePlayer.Spin = true;
					if (component.time >= component.clip.length - 1f)
					{
						TapePlayer.GetComponent<Animation>().Play("PressEject");
						TapePlayer.Spin = false;
						if (!component.isPlaying)
						{
							component.clip = TapeStop;
							component.Play();
						}
						Subtitle.text = string.Empty;
						Phase++;
					}
					if (Input.GetButtonDown("A") && component.isPlaying)
					{
						PromptBar.Label[0].text = "PLAY";
						TapePlayer.Spin = false;
						ResumeTime = component.time;
						component.Stop();
					}
				}
				if (Input.GetButtonDown("B"))
				{
					TapePlayer.GetComponent<Animation>().Play("PressEject");
					component.clip = TapeStop;
					TapePlayer.Spin = false;
					component.Play();
					PromptBar.Label[0].text = string.Empty;
					PromptBar.Label[1].text = string.Empty;
					PromptBar.Label[5].text = string.Empty;
					PromptBar.UpdateButtons();
					Subtitle.text = string.Empty;
					Phase++;
				}
			}
			else if (Phase == 3)
			{
				TapePlayer.GetComponent<Animation>()["PressEject"].time += 1f / 60f;
				if (TapePlayer.GetComponent<Animation>()["PressEject"].time >= TapePlayer.GetComponent<Animation>()["PressEject"].length)
				{
					TapePlayer.GetComponent<Animation>().Play("InsertTape");
					TapePlayer.GetComponent<Animation>()["InsertTape"].time = TapePlayer.GetComponent<Animation>()["InsertTape"].length;
					TapePlayer.FastForward = false;
					Phase++;
				}
			}
			else if (Phase == 4)
			{
				TapePlayer.GetComponent<Animation>()["InsertTape"].time -= 0.0555555f;
				if (TapePlayer.GetComponent<Animation>()["InsertTape"].time <= 0f)
				{
					TapePlayer.Tape.SetActive(false);
					Jukebox.SetActive(true);
					Listening = false;
					Timer = 0f;
					PromptBar.Label[0].text = "PLAY";
					PromptBar.Label[1].text = "BACK";
					PromptBar.Label[4].text = "CHOOSE";
					PromptBar.Label[5].text = "CATEGORY";
					PromptBar.UpdateButtons();
				}
			}
			if (Phase == 2)
			{
				if (InputManager.DPadRight || Input.GetKey(KeyCode.RightArrow))
				{
					ResumeTime += 1.6666666f;
					component.time += 1.6666666f;
					TapePlayer.FastForward = true;
				}
				else
				{
					TapePlayer.FastForward = false;
				}
				if (InputManager.DPadLeft || Input.GetKey(KeyCode.LeftArrow))
				{
					ResumeTime -= 1.6666666f;
					component.time -= 1.6666666f;
					TapePlayer.Rewind = true;
				}
				else
				{
					TapePlayer.Rewind = false;
				}
				int num = 0;
				int num2 = 0;
				if (component.isPlaying)
				{
					num = Mathf.FloorToInt(component.time / 60f);
					num2 = Mathf.FloorToInt(component.time - (float)num * 60f);
					Bar.fillAmount = component.time / component.clip.length;
				}
				else
				{
					num = Mathf.FloorToInt(ResumeTime / 60f);
					num2 = Mathf.FloorToInt(ResumeTime - (float)num * 60f);
					Bar.fillAmount = ResumeTime / component.clip.length;
				}
				CurrentTime = string.Format("{00:00}:{1:00}", num, num2);
				Label.text = CurrentTime + " / " + ClipLength;
				if (Category == 1)
				{
					if (Selected == 1)
					{
						for (int i = 0; i < Cues1.Length; i++)
						{
							if (component.time > Cues1[i])
							{
								Subtitle.text = Subs1[i];
							}
						}
					}
					else if (Selected == 2)
					{
						for (int j = 0; j < Cues2.Length; j++)
						{
							if (component.time > Cues2[j])
							{
								Subtitle.text = Subs2[j];
							}
						}
					}
					else if (Selected == 3)
					{
						for (int k = 0; k < Cues3.Length; k++)
						{
							if (component.time > Cues3[k])
							{
								Subtitle.text = Subs3[k];
							}
						}
					}
					else if (Selected == 4)
					{
						for (int l = 0; l < Cues4.Length; l++)
						{
							if (component.time > Cues4[l])
							{
								Subtitle.text = Subs4[l];
							}
						}
					}
					else if (Selected == 5)
					{
						for (int m = 0; m < Cues5.Length; m++)
						{
							if (component.time > Cues5[m])
							{
								Subtitle.text = Subs5[m];
							}
						}
					}
					else if (Selected == 6)
					{
						for (int n = 0; n < Cues6.Length; n++)
						{
							if (component.time > Cues6[n])
							{
								Subtitle.text = Subs6[n];
							}
						}
					}
					else if (Selected == 7)
					{
						for (int num3 = 0; num3 < Cues7.Length; num3++)
						{
							if (component.time > Cues7[num3])
							{
								Subtitle.text = Subs7[num3];
							}
						}
					}
					else if (Selected == 8)
					{
						for (int num4 = 0; num4 < Cues8.Length; num4++)
						{
							if (component.time > Cues8[num4])
							{
								Subtitle.text = Subs8[num4];
							}
						}
					}
					else if (Selected == 9)
					{
						for (int num5 = 0; num5 < Cues9.Length; num5++)
						{
							if (component.time > Cues9[num5])
							{
								Subtitle.text = Subs9[num5];
							}
						}
					}
					else
					{
						if (Selected != 10)
						{
							return;
						}
						for (int num6 = 0; num6 < Cues10.Length; num6++)
						{
							if (component.time > Cues10[num6])
							{
								Subtitle.text = Subs10[num6];
							}
						}
					}
				}
				else if (Category == 2)
				{
					if (Selected == 1)
					{
						for (int num7 = 0; num7 < BasementCues1.Length; num7++)
						{
							if (component.time > BasementCues1[num7])
							{
								Subtitle.text = BasementSubs1[num7];
							}
						}
					}
					if (Selected != 10)
					{
						return;
					}
					for (int num8 = 0; num8 < BasementCues10.Length; num8++)
					{
						if (component.time > BasementCues10[num8])
						{
							Subtitle.text = BasementSubs10[num8];
						}
					}
				}
				else
				{
					if (Category != 3)
					{
						return;
					}
					if (Selected == 1)
					{
						for (int num9 = 0; num9 < HeadmasterCues1.Length; num9++)
						{
							if (component.time > HeadmasterCues1[num9])
							{
								Subtitle.text = HeadmasterSubs1[num9];
							}
						}
					}
					else if (Selected == 2)
					{
						for (int num10 = 0; num10 < HeadmasterCues2.Length; num10++)
						{
							if (component.time > HeadmasterCues2[num10])
							{
								Subtitle.text = HeadmasterSubs2[num10];
							}
						}
					}
					else
					{
						if (Selected != 10)
						{
							return;
						}
						for (int num11 = 0; num11 < HeadmasterCues10.Length; num11++)
						{
							if (component.time > HeadmasterCues10[num11])
							{
								Subtitle.text = HeadmasterSubs10[num11];
							}
						}
					}
				}
			}
			else
			{
				Label.text = "00:00 / 00:00";
				Bar.fillAmount = 0f;
			}
			return;
		}
		TapePlayerCamera.position = new Vector3(Mathf.Lerp(TapePlayerCamera.position.x, -26.2125f, t), TapePlayerCamera.position.y, Mathf.Lerp(TapePlayerCamera.position.z, 5.4125f, t));
		List.transform.localPosition = new Vector3(Mathf.Lerp(List.transform.localPosition.x, 0f, t), List.transform.localPosition.y, List.transform.localPosition.z);
		TimeBar.localPosition = new Vector3(TimeBar.localPosition.x, Mathf.Lerp(TimeBar.localPosition.y, 100f, t), TimeBar.localPosition.z);
		if (InputManager.TappedRight)
		{
			Category++;
			if (Category > 3)
			{
				Category = 1;
			}
			UpdateLabels();
		}
		else if (InputManager.TappedLeft)
		{
			Category--;
			if (Category < 1)
			{
				Category = 3;
			}
			UpdateLabels();
		}
		if (InputManager.TappedUp)
		{
			Selected--;
			if (Selected < 1)
			{
				Selected = 10;
			}
			Highlight.localPosition = new Vector3(Highlight.localPosition.x, 440f - 80f * (float)Selected, Highlight.localPosition.z);
			CheckSelection();
		}
		else if (InputManager.TappedDown)
		{
			Selected++;
			if (Selected > 10)
			{
				Selected = 1;
			}
			Highlight.localPosition = new Vector3(Highlight.localPosition.x, 440f - 80f * (float)Selected, Highlight.localPosition.z);
			CheckSelection();
		}
		else if (Input.GetButtonDown("A"))
		{
			bool flag = false;
			if (Category == 1)
			{
				if (CollectibleGlobals.GetTapeCollected(Selected))
				{
					CollectibleGlobals.SetTapeListened(Selected, true);
					flag = true;
				}
			}
			else if (Category == 2)
			{
				if (CollectibleGlobals.GetBasementTapeCollected(Selected))
				{
					CollectibleGlobals.SetBasementTapeListened(Selected, true);
					flag = true;
				}
			}
			else if (Category == 3 && CollectibleGlobals.GetHeadmasterTapeCollected(Selected))
			{
				CollectibleGlobals.SetHeadmasterTapeListened(Selected, true);
				flag = true;
			}
			if (flag)
			{
				NewIcons[Selected].SetActive(false);
				Jukebox.SetActive(false);
				Listening = true;
				Phase = 1;
				PromptBar.Label[0].text = string.Empty;
				PromptBar.Label[1].text = string.Empty;
				PromptBar.Label[4].text = string.Empty;
				PromptBar.UpdateButtons();
				TapePlayer.GetComponent<Animation>().Play("InsertTape");
				TapePlayer.Tape.SetActive(true);
				if (Category == 1)
				{
					component.clip = Recordings[Selected];
				}
				else if (Category == 2)
				{
					component.clip = BasementRecordings[Selected];
				}
				else
				{
					component.clip = HeadmasterRecordings[Selected];
				}
				component.time = 0f;
				RoundedTime = Mathf.CeilToInt(component.clip.length);
				int num12 = (int)(RoundedTime / 60f);
				int num13 = (int)(RoundedTime % 60f);
				ClipLength = string.Format("{0:00}:{1:00}", num12, num13);
			}
		}
		else if (Input.GetButtonDown("B"))
		{
			TapePlayer.Yandere.HeartCamera.enabled = true;
			TapePlayer.Yandere.RPGCamera.enabled = true;
			TapePlayer.TapePlayerCamera.enabled = false;
			TapePlayer.NoteWindow.SetActive(true);
			TapePlayer.PromptBar.ClearButtons();
			TapePlayer.Yandere.CanMove = true;
			TapePlayer.PromptBar.Show = false;
			TapePlayer.Prompt.enabled = true;
			TapePlayer.Yandere.HUD.alpha = 1f;
			Time.timeScale = 1f;
			Show = false;
		}
	}

	public void UpdateLabels()
	{
		int num = 0;
		while (num < TotalTapes)
		{
			num++;
			if (Category == 1)
			{
				HeaderLabel.text = "Mysterious Tapes";
				if (CollectibleGlobals.GetTapeCollected(num))
				{
					TapeLabels[num].text = "Mysterious Tape " + num;
					NewIcons[num].SetActive(!CollectibleGlobals.GetTapeListened(num));
				}
				else
				{
					TapeLabels[num].text = "?????";
					NewIcons[num].SetActive(false);
				}
			}
			else if (Category == 2)
			{
				HeaderLabel.text = "Basement Tapes";
				if (CollectibleGlobals.GetBasementTapeCollected(num))
				{
					TapeLabels[num].text = "Basement Tape " + num;
					NewIcons[num].SetActive(!CollectibleGlobals.GetBasementTapeListened(num));
				}
				else
				{
					TapeLabels[num].text = "?????";
					NewIcons[num].SetActive(false);
				}
			}
			else
			{
				HeaderLabel.text = "Headmaster Tapes";
				if (CollectibleGlobals.GetHeadmasterTapeCollected(num))
				{
					TapeLabels[num].text = "Headmaster Tape " + num;
					NewIcons[num].SetActive(!CollectibleGlobals.GetHeadmasterTapeListened(num));
				}
				else
				{
					TapeLabels[num].text = "?????";
					NewIcons[num].SetActive(false);
				}
			}
		}
	}

	public void CheckSelection()
	{
		if (Category == 1)
		{
			TapePlayer.PromptBar.Label[0].text = ((!CollectibleGlobals.GetTapeCollected(Selected)) ? string.Empty : "PLAY");
			TapePlayer.PromptBar.UpdateButtons();
		}
		else if (Category == 2)
		{
			TapePlayer.PromptBar.Label[0].text = ((!CollectibleGlobals.GetBasementTapeCollected(Selected)) ? string.Empty : "PLAY");
			TapePlayer.PromptBar.UpdateButtons();
		}
		else
		{
			TapePlayer.PromptBar.Label[0].text = ((!CollectibleGlobals.GetHeadmasterTapeCollected(Selected)) ? string.Empty : "PLAY");
			TapePlayer.PromptBar.UpdateButtons();
		}
	}
}
