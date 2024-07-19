using UnityEngine;
// FIXED BY TANOS [x]
public class TalkingScript : MonoBehaviour
{
	private const float LongestTime = 100f;

	private const float LongTime = 5f;

	private const float MediumTime = 3f;

	private const float ShortTime = 2f;

	public StudentScript S;

	public WeaponScript StuckBoxCutter;

	public bool NegativeResponse;

	public bool Follow;

	public bool Grudge;

	public bool Refuse;

	public bool Fake;

	public string IdleAnim = string.Empty;

	public int ClubBonus;

	private void Update()
	{
		if (!S.Talking)
		{
			return;
		}
		if (S.Sleuthing)
		{
			ClubBonus = 5;
		}
		else
		{
			ClubBonus = 0;
		}
		if (GameGlobals.EmptyDemon)
		{
			ClubBonus = (int)S.Club * -1;
		}
		if (S.Interaction == StudentInteractionType.Idle)
		{
			if (!Fake)
			{
				if (S.Sleuthing)
				{
					IdleAnim = S.SleuthCalmAnim;
				}
				else if (S.Club == ClubType.Art && S.DialogueWheel.ClubLeader && S.Paintbrush.activeInHierarchy)
				{
					IdleAnim = "paintingIdle_00";
				}
				else if (S.Club != ClubType.Bully)
				{
					IdleAnim = S.IdleAnim;
				}
				else if (S.StudentManager.Reputation.Reputation < 33.33333f || S.Persona == PersonaType.Coward)
				{
					if (S.CurrentAction == StudentActionType.Sunbathe && S.SunbathePhase > 2)
					{
						IdleAnim = S.OriginalIdleAnim;
					}
					else
					{
						IdleAnim = S.IdleAnim;
					}
				}
				else
				{
					IdleAnim = S.CuteAnim;
				}
				S.CharacterAnimation.CrossFade(IdleAnim);
			}
			else if (IdleAnim != string.Empty)
			{
				S.CharacterAnimation.CrossFade(IdleAnim);
			}
			if (S.TalkTimer == 0f)
			{
				if (!S.DialogueWheel.AppearanceWindow.Show)
				{
					S.DialogueWheel.Impatience.fillAmount += Time.deltaTime * 0.1f;
				}
				if (S.DialogueWheel.Impatience.fillAmount > 0.5f && S.Subtitle.Timer == 0f)
				{
					if (S.StudentID == 41)
					{
						S.Subtitle.UpdateLabel(SubtitleType.Impatience, 4, 5f);
					}
					else if (S.Pestered == 0)
					{
						S.Subtitle.UpdateLabel(SubtitleType.Impatience, 0, 5f);
					}
					else
					{
						S.Subtitle.UpdateLabel(SubtitleType.Impatience, 2, 5f);
					}
				}
				if (S.DialogueWheel.Impatience.fillAmount == 1f && S.DialogueWheel.Show)
				{
					if (S.StudentID == 41)
					{
						S.Subtitle.UpdateLabel(SubtitleType.Impatience, 4, 5f);
					}
					else if (S.Pestered == 0)
					{
						S.Subtitle.UpdateLabel(SubtitleType.Impatience, 1, 5f);
					}
					else
					{
						S.Subtitle.UpdateLabel(SubtitleType.Impatience, 3, 5f);
					}
					S.WaitTimer = 0f;
					S.Pestered += 5;
					S.DialogueWheel.Pestered = true;
					S.DialogueWheel.End();
				}
			}
		}
		else if (S.Interaction == StudentInteractionType.Forgiving)
		{
			if (S.TalkTimer == 3f)
			{
				if (S.Club != ClubType.Delinquent)
				{
					S.CharacterAnimation.CrossFade(S.Nod2Anim);
					S.RepRecovery = 5f;
					if (PlayerGlobals.PantiesEquipped == 6)
					{
						S.RepRecovery += 2.5f;
					}
					if (PlayerGlobals.SocialBonus > 0)
					{
						S.RepRecovery += 2.5f;
					}
					S.PendingRep += S.RepRecovery;
					S.Reputation.PendingRep += S.RepRecovery;
					S.ID = 0;
					while (S.ID < S.Outlines.Length)
					{
						S.Outlines[S.ID].color = new Color(0f, 1f, 0f, 1f);
						S.ID++;
					}
					S.Forgave = true;
					if (S.Witnessed == StudentWitnessType.Insanity || S.Witnessed == StudentWitnessType.WeaponAndBloodAndInsanity || S.Witnessed == StudentWitnessType.WeaponAndInsanity || S.Witnessed == StudentWitnessType.BloodAndInsanity)
					{
						S.Subtitle.UpdateLabel(SubtitleType.ForgivingInsanity, 0, 3f);
					}
					else if (S.Witnessed == StudentWitnessType.Accident)
					{
						S.Subtitle.UpdateLabel(SubtitleType.ForgivingAccident, 0, 5f);
					}
					else
					{
						S.Subtitle.UpdateLabel(SubtitleType.Forgiving, 0, 3f);
					}
				}
				else
				{
					S.Subtitle.UpdateLabel(SubtitleType.Dismissive, 0, 5f);
				}
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.CharacterAnimation[S.Nod2Anim].time >= S.CharacterAnimation[S.Nod2Anim].length)
				{
					S.CharacterAnimation.CrossFade(IdleAnim);
				}
				if (S.TalkTimer <= 0f)
				{
					S.IgnoreTimer = 5f;
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.ReceivingCompliment)
		{
			if (S.TalkTimer == 3f)
			{
				if (S.Club != ClubType.Delinquent)
				{
					S.CharacterAnimation.CrossFade(S.LookDownAnim);
					if (PlayerGlobals.Reputation < -33.33333f)
					{
						S.Subtitle.UpdateLabel(SubtitleType.StudentLowCompliment, 0, 3f);
					}
					else if (PlayerGlobals.Reputation > 33.33333f)
					{
						S.Subtitle.UpdateLabel(SubtitleType.StudentHighCompliment, 0, 3f);
					}
					else
					{
						S.Subtitle.UpdateLabel(SubtitleType.StudentMidCompliment, 0, 3f);
					}
					CalculateRepBonus();
					S.Reputation.PendingRep += 1f + (float)S.RepBonus;
					S.PendingRep += 1f + (float)S.RepBonus;
					S.Complimented = true;
				}
				else
				{
					S.Subtitle.UpdateLabel(SubtitleType.Dismissive, 1, 5f);
				}
			}
			else if (Input.GetButtonDown("A"))
			{
				S.TalkTimer = 0f;
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				S.DialogueWheel.End();
			}
		}
		else if (S.Interaction == StudentInteractionType.Gossiping)
		{
			if (S.TalkTimer == 3f)
			{
				if (S.Club != ClubType.Delinquent)
				{
					S.CharacterAnimation.CrossFade(S.GossipAnim);
					S.Subtitle.UpdateLabel(SubtitleType.StudentGossip, 0, 3f);
					S.GossipBonus = 0;
					if (S.Reputation.Reputation > 33.33333f)
					{
						S.GossipBonus++;
					}
					if (PlayerGlobals.PantiesEquipped == 9)
					{
						S.GossipBonus++;
					}
					if (SchemeGlobals.DarkSecret)
					{
						S.GossipBonus++;
					}
					if (PlayerGlobals.GetStudentFriend(S.StudentID))
					{
						S.GossipBonus++;
					}
					if ((S.Male && PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus > 0) || PlayerGlobals.Seduction == 5)
					{
						S.GossipBonus++;
					}
					if (PlayerGlobals.SocialBonus > 0)
					{
						S.GossipBonus++;
					}
					StudentGlobals.SetStudentReputation(S.DialogueWheel.Victim, StudentGlobals.GetStudentReputation(S.DialogueWheel.Victim) - (1 + S.GossipBonus));
					if (S.Club != ClubType.Bully)
					{
						S.Reputation.PendingRep -= 2f;
						S.PendingRep -= 2f;
					}
					S.Gossiped = true;
					if (!ConversationGlobals.GetTopicDiscovered(19))
					{
						S.Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
						ConversationGlobals.SetTopicDiscovered(19, true);
					}
					if (!ConversationGlobals.GetTopicLearnedByStudent(19, S.StudentID))
					{
						S.Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
						ConversationGlobals.SetTopicLearnedByStudent(19, S.StudentID, true);
					}
				}
				else
				{
					S.Subtitle.UpdateLabel(SubtitleType.Dismissive, 2, 3f);
				}
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.CharacterAnimation[S.GossipAnim].time >= S.CharacterAnimation[S.GossipAnim].length)
				{
					S.CharacterAnimation.CrossFade(IdleAnim);
				}
				if (S.TalkTimer <= 0f)
				{
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.Bye)
		{
			if (S.TalkTimer == 2f)
			{
				if (S.Club != ClubType.Delinquent)
				{
					S.Subtitle.UpdateLabel(SubtitleType.StudentFarewell, 0, 2f);
				}
				else
				{
					S.Subtitle.UpdateLabel(SubtitleType.Dismissive, 3, 3f);
				}
			}
			else if (Input.GetButtonDown("A"))
			{
				S.TalkTimer = 0f;
			}
			S.CharacterAnimation.CrossFade(IdleAnim);
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				S.Pestered += 2;
				S.DialogueWheel.End();
			}
		}
		else if (S.Interaction == StudentInteractionType.GivingTask)
		{
			if (S.TalkTimer == 100f)
			{
				S.Subtitle.UpdateLabel(S.TaskLineResponseType, S.TaskPhase, S.Subtitle.GetClipLength(S.StudentID, S.TaskPhase));
				S.CharacterAnimation.CrossFade(S.TaskAnims[S.TaskPhase]);
				S.CurrentAnim = S.TaskAnims[S.TaskPhase];
				S.TalkTimer = S.Subtitle.GetClipLength(S.StudentID, S.TaskPhase);
			}
			else if (Input.GetButtonDown("A"))
			{
				S.Subtitle.Label.text = string.Empty;
				Object.Destroy(S.Subtitle.CurrentClip);
				S.TalkTimer = 0f;
			}
			if (S.CharacterAnimation[S.CurrentAnim].time >= S.CharacterAnimation[S.CurrentAnim].length)
			{
				S.CharacterAnimation.CrossFade(IdleAnim);
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				if (S.TaskPhase == 5)
				{
					S.DialogueWheel.TaskWindow.TaskComplete = true;
					TaskGlobals.SetTaskStatus(S.StudentID, 3);
					PlayerGlobals.SetStudentFriend(S.StudentID, true);
					S.Police.EndOfDay.NewFriends++;
					S.Interaction = StudentInteractionType.Idle;
					CalculateRepBonus();
					S.Reputation.PendingRep += 1f + (float)S.RepBonus;
					S.PendingRep += 1f + (float)S.RepBonus;
				}
				else if (S.TaskPhase == 4 || S.TaskPhase == 0)
				{
					S.StudentManager.TaskManager.UpdateTaskStatus();
					S.DialogueWheel.End();
				}
				else if (S.TaskPhase == 3)
				{
					S.DialogueWheel.TaskWindow.UpdateWindow(S.StudentID);
					S.Interaction = StudentInteractionType.Idle;
				}
				else
				{
					S.TaskPhase++;
					S.Subtitle.UpdateLabel(S.TaskLineResponseType, S.TaskPhase, S.Subtitle.GetClipLength(S.StudentID, S.TaskPhase));
					S.CharacterAnimation.CrossFade(S.TaskAnims[S.TaskPhase]);
					S.CurrentAnim = S.TaskAnims[S.TaskPhase];
					S.TalkTimer = S.Subtitle.GetClipLength(S.StudentID, S.TaskPhase);
				}
			}
		}
		else if (S.Interaction == StudentInteractionType.FollowingPlayer)
		{
			if (S.TalkTimer == 2f)
			{
				if (S.Club != ClubType.Delinquent)
				{
					if ((S.Clock.HourTime > 8f && S.Clock.HourTime < 13f) || (S.Clock.HourTime > 13.375f && S.Clock.HourTime < 15.5f))
					{
						S.CharacterAnimation.CrossFade(S.GossipAnim);
						S.Subtitle.UpdateLabel(SubtitleType.StudentStay, 0, 5f);
						NegativeResponse = true;
					}
					else if (S.StudentManager.LockerRoomArea.bounds.Contains(S.Yandere.transform.position) || S.StudentManager.WestBathroomArea.bounds.Contains(S.Yandere.transform.position) || S.StudentManager.EastBathroomArea.bounds.Contains(S.Yandere.transform.position) || S.StudentManager.HeadmasterArea.bounds.Contains(S.Yandere.transform.position) || S.MyRenderer.sharedMesh == S.SchoolSwimsuit || S.MyRenderer.sharedMesh == S.SwimmingTrunks)
					{
						S.CharacterAnimation.CrossFade(S.GossipAnim);
						S.Subtitle.UpdateLabel(SubtitleType.StudentStay, 1, 5f);
						NegativeResponse = true;
					}
					else
					{
						S.CharacterAnimation.CrossFade(S.Nod1Anim);
						S.Subtitle.UpdateLabel(SubtitleType.StudentFollow, 0, 2f);
						Follow = true;
					}
				}
				else
				{
					S.Subtitle.UpdateLabel(SubtitleType.Dismissive, 4, 5f);
				}
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.CharacterAnimation[S.Nod1Anim].time >= S.CharacterAnimation[S.Nod1Anim].length)
				{
					S.CharacterAnimation.CrossFade(IdleAnim);
				}
				if (S.TalkTimer <= 0f)
				{
					S.DialogueWheel.End();
					if (Follow)
					{
						S.Pathfinding.target = S.Yandere.transform;
						S.Prompt.Label[0].text = "     Stop";
						if (S.StudentID == 30)
						{
							S.StudentManager.FollowerLookAtTarget.position = S.DefaultTarget.position;
							S.StudentManager.LoveManager.Follower = S;
						}
						S.Yandere.Follower = S;
						S.Yandere.Followers++;
						S.Following = true;
						S.Hurry = false;
					}
					Follow = false;
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.GoingAway)
		{
			if (S.TalkTimer == 3f)
			{
				if (S.Club != ClubType.Delinquent)
				{
					if ((S.Clock.HourTime > 8f && S.Clock.HourTime < 13f) || (S.Clock.HourTime > 13.375f && S.Clock.HourTime < 15.5f))
					{
						S.CharacterAnimation.CrossFade(S.GossipAnim);
						S.Subtitle.UpdateLabel(SubtitleType.StudentStay, 0, 5f);
					}
					else
					{
						S.CharacterAnimation.CrossFade(S.Nod1Anim);
						S.Subtitle.UpdateLabel(SubtitleType.StudentLeave, 0, 3f);
						S.GoAway = true;
					}
				}
				else
				{
					S.Subtitle.UpdateLabel(SubtitleType.Dismissive, 5, 5f);
				}
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.CharacterAnimation[S.Nod1Anim].time >= S.CharacterAnimation[S.Nod1Anim].length)
				{
					S.CharacterAnimation.CrossFade(IdleAnim);
				}
				if (S.TalkTimer <= 0f)
				{
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.DistractingTarget)
		{
			if (S.TalkTimer == 3f)
			{
				if (S.Club != ClubType.Delinquent)
				{
					if ((S.Clock.HourTime > 8f && S.Clock.HourTime < 13f) || (S.Clock.HourTime > 13.375f && S.Clock.HourTime < 15.5f))
					{
						S.CharacterAnimation.CrossFade(S.GossipAnim);
						S.Subtitle.UpdateLabel(SubtitleType.StudentStay, 0, 5f);
					}
					else
					{
						StudentScript studentScript = S.StudentManager.Students[S.DialogueWheel.Victim];
						Grudge = false;
						if (studentScript.Club == ClubType.Delinquent || (S.Bullied && studentScript.Club == ClubType.Bully) || (studentScript.StudentID == 36 && TaskGlobals.GetTaskStatus(36) < 3))
						{
							Grudge = true;
						}
						if (studentScript.Routine && !studentScript.TargetedForDistraction && !studentScript.InEvent && !Grudge && studentScript.Indoors && studentScript.gameObject.activeInHierarchy && studentScript.ClubActivityPhase < 16)
						{
							S.CharacterAnimation.CrossFade(S.Nod1Anim);
							S.Subtitle.UpdateLabel(SubtitleType.StudentDistract, 0, 3f);
							Refuse = false;
						}
						else
						{
							S.CharacterAnimation.CrossFade(S.GossipAnim);
							if (Grudge)
							{
								S.Subtitle.UpdateLabel(SubtitleType.StudentDistractBullyRefuse, 0, 3f);
							}
							else
							{
								S.Subtitle.UpdateLabel(SubtitleType.StudentDistractRefuse, 0, 3f);
							}
							Refuse = true;
						}
					}
				}
				else
				{
					S.Subtitle.UpdateLabel(SubtitleType.Dismissive, 6, 5f);
					Refuse = true;
				}
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.CharacterAnimation[S.Nod1Anim].time >= S.CharacterAnimation[S.Nod1Anim].length)
				{
					S.CharacterAnimation.CrossFade(IdleAnim);
				}
				if (S.TalkTimer <= 0f)
				{
					S.DialogueWheel.End();
					if (!Refuse && (S.Clock.HourTime < 8f || (S.Clock.HourTime > 13f && S.Clock.HourTime < 13.375f) || S.Clock.HourTime > 15.5f) && !S.Distracting)
					{
						S.DistractionTarget = S.StudentManager.Students[S.DialogueWheel.Victim];
						S.DistractionTarget.TargetedForDistraction = true;
						S.CurrentDestination = S.DistractionTarget.transform;
						S.Pathfinding.target = S.DistractionTarget.transform;
						S.Pathfinding.speed = 4f;
						S.TargetDistance = 1f;
						S.DistractTimer = 10f;
						S.Distracting = true;
						S.Routine = false;
						S.CanTalk = false;
					}
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.PersonalGrudge)
		{
			if (S.TalkTimer == 5f)
			{
				if (S.Persona == PersonaType.Coward || S.Persona == PersonaType.Fragile)
				{
					S.Subtitle.UpdateLabel(SubtitleType.CowardGrudge, 0, 5f);
					S.CharacterAnimation.CrossFade(S.CowardAnim);
					S.TalkTimer = 5f;
				}
				else
				{
					if (!S.Male)
					{
						S.Subtitle.UpdateLabel(SubtitleType.GrudgeWarning, 0, 99f);
					}
					else if (S.Club == ClubType.Delinquent)
					{
						S.Subtitle.UpdateLabel(SubtitleType.DelinquentGrudge, 1, 99f);
					}
					else
					{
						S.Subtitle.UpdateLabel(SubtitleType.GrudgeWarning, 1, 99f);
					}
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
					S.CharacterAnimation.CrossFade(S.GrudgeAnim);
				}
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.TalkTimer <= 0f)
				{
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.ClubInfo)
		{
			if (S.TalkTimer == 100f)
			{
				S.Subtitle.UpdateLabel(S.ClubInfoResponseType, S.ClubPhase, 99f);
				S.TalkTimer = S.Subtitle.GetClubClipLength(S.Club, S.ClubPhase);
			}
			else if (Input.GetButtonDown("A"))
			{
				S.Subtitle.Label.text = string.Empty;
				Object.Destroy(S.Subtitle.CurrentClip);
				S.TalkTimer = 0f;
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				if (S.ClubPhase == 3)
				{
					S.DialogueWheel.Panel.enabled = true;
					S.DialogueWheel.Show = true;
					S.Subtitle.Label.text = string.Empty;
					S.Interaction = StudentInteractionType.Idle;
					S.TalkTimer = 0f;
				}
				else
				{
					S.ClubPhase++;
					S.Subtitle.UpdateLabel(S.ClubInfoResponseType, S.ClubPhase, 99f);
					S.TalkTimer = S.Subtitle.GetClubClipLength(S.Club, S.ClubPhase);
				}
			}
		}
		else if (S.Interaction == StudentInteractionType.ClubJoin)
		{
			if (S.TalkTimer == 100f)
			{
				if (S.ClubPhase == 1)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubJoin, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 2)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubAccept, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 3)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubRefuse, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 4)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubRejoin, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 5)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubExclusive, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 6)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubGrudge, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
			}
			else if (Input.GetButtonDown("A"))
			{
				S.Subtitle.Label.text = string.Empty;
				Object.Destroy(S.Subtitle.CurrentClip);
				S.TalkTimer = 0f;
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				if (S.ClubPhase == 1)
				{
					S.DialogueWheel.ClubWindow.Club = S.Club;
					S.DialogueWheel.ClubWindow.UpdateWindow();
					S.Subtitle.Label.text = string.Empty;
					S.Interaction = StudentInteractionType.Idle;
				}
				else
				{
					S.DialogueWheel.End();
					if (S.Club == ClubType.MartialArts)
					{
						S.ChangingBooth.CheckYandereClub();
					}
				}
			}
		}
		else if (S.Interaction == StudentInteractionType.ClubQuit)
		{
			if (S.TalkTimer == 100f)
			{
				if (S.ClubPhase == 1)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubQuit, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 2)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubConfirm, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 3)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubDeny, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
			}
			else if (Input.GetButtonDown("A"))
			{
				S.Subtitle.Label.text = string.Empty;
				Object.Destroy(S.Subtitle.CurrentClip);
				S.TalkTimer = 0f;
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				if (S.ClubPhase == 1)
				{
					S.DialogueWheel.ClubWindow.Club = S.Club;
					S.DialogueWheel.ClubWindow.Quitting = true;
					S.DialogueWheel.ClubWindow.UpdateWindow();
					S.Subtitle.Label.text = string.Empty;
					S.Interaction = StudentInteractionType.Idle;
				}
				else
				{
					S.DialogueWheel.End();
					if (S.Club == ClubType.MartialArts)
					{
						S.ChangingBooth.CheckYandereClub();
					}
					if (S.ClubPhase != 2)
					{
					}
				}
			}
		}
		else if (S.Interaction == StudentInteractionType.ClubBye)
		{
			if (S.TalkTimer == S.Subtitle.ClubFarewellClips[(int)(S.Club + ClubBonus)].length)
			{
				S.Subtitle.UpdateLabel(SubtitleType.ClubFarewell, (int)(S.Club + ClubBonus), S.Subtitle.ClubFarewellClips[(int)(S.Club + ClubBonus)].length);
			}
			else if (Input.GetButtonDown("A"))
			{
				S.TalkTimer = 0f;
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				S.DialogueWheel.End();
			}
		}
		else if (S.Interaction == StudentInteractionType.ClubActivity)
		{
			if (S.TalkTimer == 100f)
			{
				if (S.ClubPhase == 1)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubActivity, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 2)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubYes, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 3)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubNo, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 4)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubEarly, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 5)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubLate, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
			}
			else if (Input.GetButtonDown("A"))
			{
				S.Subtitle.Label.text = string.Empty;
				Object.Destroy(S.Subtitle.CurrentClip);
				S.TalkTimer = 0f;
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				if (S.ClubPhase == 1)
				{
					S.DialogueWheel.ClubWindow.Club = S.Club;
					S.DialogueWheel.ClubWindow.Activity = true;
					S.DialogueWheel.ClubWindow.UpdateWindow();
					S.Subtitle.Label.text = string.Empty;
					S.Interaction = StudentInteractionType.Idle;
				}
				else if (S.ClubPhase == 2)
				{
					S.Police.Darkness.enabled = true;
					S.Police.ClubActivity = true;
					S.Police.FadeOut = true;
					S.Subtitle.Label.text = string.Empty;
					S.Interaction = StudentInteractionType.Idle;
				}
				else
				{
					S.DialogueWheel.End();
				}
			}
		}
		else if (S.Interaction == StudentInteractionType.ClubUnwelcome)
		{
			S.CharacterAnimation.CrossFade(S.IdleAnim);
			if (S.TalkTimer == 5f)
			{
				S.Subtitle.UpdateLabel(SubtitleType.ClubUnwelcome, (int)(S.Club + ClubBonus), 99f);
				S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.TalkTimer <= 0f)
				{
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.ClubKick)
		{
			S.CharacterAnimation.CrossFade(S.IdleAnim);
			if (S.TalkTimer == 5f)
			{
				S.Subtitle.UpdateLabel(SubtitleType.ClubKick, (int)(S.Club + ClubBonus), 99f);
				S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.TalkTimer <= 0f)
				{
					S.ClubManager.DeactivateClubBenefit();
					ClubGlobals.Club = ClubType.None;
					S.DialogueWheel.End();
					S.Yandere.ClubAccessory();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.ClubGrudge)
		{
			S.CharacterAnimation.CrossFade(S.IdleAnim);
			if (S.TalkTimer == 5f)
			{
				S.Subtitle.UpdateLabel(SubtitleType.ClubGrudge, (int)(S.Club + ClubBonus), 99f);
				S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.TalkTimer <= 0f)
				{
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.ClubPractice)
		{
			if (S.TalkTimer == 100f)
			{
				if (S.ClubPhase == 1)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubPractice, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 2)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubPracticeYes, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
				else if (S.ClubPhase == 3)
				{
					S.Subtitle.UpdateLabel(SubtitleType.ClubPracticeNo, (int)(S.Club + ClubBonus), 99f);
					S.TalkTimer = S.Subtitle.CurrentClip.GetComponent<AudioSource>().clip.length;
				}
			}
			else if (Input.GetButtonDown("A"))
			{
				S.Subtitle.Label.text = string.Empty;
				Object.Destroy(S.Subtitle.CurrentClip);
				S.TalkTimer = 0f;
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				if (S.ClubPhase == 1)
				{
					S.DialogueWheel.PracticeWindow.Club = S.Club;
					S.DialogueWheel.PracticeWindow.UpdateWindow();
					S.DialogueWheel.PracticeWindow.ID = 1;
					S.Subtitle.Label.text = string.Empty;
					S.Interaction = StudentInteractionType.Idle;
				}
				else if (S.ClubPhase == 2)
				{
					S.DialogueWheel.PracticeWindow.Club = S.Club;
					S.DialogueWheel.PracticeWindow.FadeOut = true;
					S.Subtitle.Label.text = string.Empty;
					S.Interaction = StudentInteractionType.Idle;
				}
				else if (S.ClubPhase == 3)
				{
					S.DialogueWheel.End();
				}
			}
		}
		else if (S.Interaction == StudentInteractionType.NamingCrush)
		{
			if (S.TalkTimer == 3f)
			{
				if (S.DialogueWheel.Victim != S.Crush)
				{
					S.Subtitle.UpdateLabel(SubtitleType.SuitorLove, 0, 3f);
					S.CharacterAnimation.CrossFade(S.GossipAnim);
					S.CurrentAnim = S.GossipAnim;
				}
				else
				{
					DatingGlobals.SuitorProgress = 1;
					S.Yandere.LoveManager.SuitorProgress++;
					S.Subtitle.UpdateLabel(SubtitleType.SuitorLove, 1, 3f);
					S.CharacterAnimation.CrossFade(S.Nod1Anim);
					S.CurrentAnim = S.Nod1Anim;
				}
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.CharacterAnimation[S.CurrentAnim].time >= S.CharacterAnimation[S.CurrentAnim].length)
				{
					S.CharacterAnimation.CrossFade(IdleAnim);
				}
				if (S.TalkTimer <= 0f)
				{
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.ChangingAppearance)
		{
			if (S.TalkTimer == 3f)
			{
				S.Subtitle.UpdateLabel(SubtitleType.SuitorLove, 2, 3f);
				S.CharacterAnimation.CrossFade(S.Nod1Anim);
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.CharacterAnimation[S.Nod1Anim].time >= S.CharacterAnimation[S.Nod1Anim].length)
				{
					S.CharacterAnimation.CrossFade(IdleAnim);
				}
				if (S.TalkTimer <= 0f)
				{
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.Court)
		{
			if (S.TalkTimer == 3f)
			{
				if (S.Male)
				{
					S.Subtitle.UpdateLabel(SubtitleType.SuitorLove, 3, 5f);
				}
				else
				{
					S.Subtitle.UpdateLabel(SubtitleType.SuitorLove, 4, 5f);
				}
				S.CharacterAnimation.CrossFade(S.Nod1Anim);
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.CharacterAnimation[S.Nod1Anim].time >= S.CharacterAnimation[S.Nod1Anim].length)
				{
					S.CharacterAnimation.CrossFade(IdleAnim);
				}
				if (S.TalkTimer <= 0f)
				{
					S.MeetTime = S.Clock.HourTime - 1f;
					if (S.Male)
					{
						S.MeetSpot = S.StudentManager.SuitorSpot;
					}
					else
					{
						S.MeetSpot = S.StudentManager.RomanceSpot;
						S.StudentManager.LoveManager.RivalWaiting = true;
					}
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.Gift)
		{
			if (S.TalkTimer == 5f)
			{
				S.Subtitle.UpdateLabel(SubtitleType.SuitorLove, 5, 99f);
				S.CharacterAnimation.CrossFade(S.Nod1Anim);
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.CharacterAnimation[S.Nod1Anim].time >= S.CharacterAnimation[S.Nod1Anim].length)
				{
					S.CharacterAnimation.CrossFade(IdleAnim);
				}
				if (S.TalkTimer <= 0f)
				{
					S.Rose = true;
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		else if (S.Interaction == StudentInteractionType.Feeding)
		{
			Debug.Log("Feeding.");
			if (S.TalkTimer == 10f)
			{
				if (S.Club == ClubType.Delinquent)
				{
					S.CharacterAnimation.CrossFade(S.IdleAnim);
					S.Subtitle.UpdateLabel(SubtitleType.RejectFood, 1, 3f);
				}
				else if (S.Fed || S.Club == ClubType.Council)
				{
					S.CharacterAnimation.CrossFade(S.GossipAnim);
					S.Subtitle.UpdateLabel(SubtitleType.RejectFood, 0, 3f);
					S.Fed = true;
				}
				else
				{
					S.CharacterAnimation.CrossFade(S.Nod2Anim);
					S.Subtitle.UpdateLabel(SubtitleType.AcceptFood, 0, 3f);
					CalculateRepBonus();
					S.Reputation.PendingRep += 1f + (float)S.RepBonus;
					S.PendingRep += 1f + (float)S.RepBonus;
				}
			}
			else if (Input.GetButtonDown("A"))
			{
				S.TalkTimer = 0f;
			}
			if (S.CharacterAnimation[S.Nod2Anim].time >= S.CharacterAnimation[S.Nod2Anim].length)
			{
				S.CharacterAnimation.CrossFade(S.IdleAnim);
			}
			if (S.CharacterAnimation[S.GossipAnim].time >= S.CharacterAnimation[S.GossipAnim].length)
			{
				S.CharacterAnimation.CrossFade(S.IdleAnim);
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				if (!S.Fed && S.Club != ClubType.Delinquent)
				{
					S.Yandere.PickUp.FoodPieces[S.Yandere.PickUp.Food].SetActive(false);
					S.Yandere.PickUp.Food--;
					S.Fed = true;
				}
				S.DialogueWheel.End();
				S.StudentManager.UpdateStudents();
			}
		}
		else if (S.Interaction == StudentInteractionType.TaskInquiry)
		{
			if (S.TalkTimer == 10f)
			{
				S.CharacterAnimation.CrossFade("f02_embar_00");
				S.Subtitle.UpdateLabel(SubtitleType.TaskInquiry, S.StudentID - 80, 10f);
			}
			else if (Input.GetButtonDown("A"))
			{
				S.TalkTimer = 0f;
			}
			if (S.CharacterAnimation["f02_embar_00"].time >= S.CharacterAnimation["f02_embar_00"].length)
			{
				S.CharacterAnimation.CrossFade(IdleAnim);
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				S.StudentManager.TaskManager.GirlsQuestioned[S.StudentID - 80] = true;
				S.DialogueWheel.End();
			}
		}
		else if (S.Interaction == StudentInteractionType.TakingSnack)
		{
			Debug.Log("Taking snack.");
			if (S.TalkTimer == 5f)
			{
				if (S.Club == ClubType.Delinquent)
				{
					S.CharacterAnimation.CrossFade(S.IdleAnim);
					S.Subtitle.UpdateLabel(SubtitleType.RejectFood, 1, 3f);
				}
				else if (S.Fed || S.Club == ClubType.Council)
				{
					S.CharacterAnimation.CrossFade(S.GossipAnim);
					S.Subtitle.UpdateLabel(SubtitleType.RejectFood, 0, 3f);
					S.Fed = true;
				}
				else
				{
					S.CharacterAnimation.CrossFade(S.Nod2Anim);
					S.Subtitle.UpdateLabel(SubtitleType.AcceptFood, 0, 3f);
					CalculateRepBonus();
					S.Reputation.PendingRep += 1f + (float)S.RepBonus;
					S.PendingRep += 1f + (float)S.RepBonus;
				}
			}
			else if (Input.GetButtonDown("A"))
			{
				S.TalkTimer = 0f;
			}
			if (S.CharacterAnimation[S.Nod2Anim].time >= S.CharacterAnimation[S.Nod2Anim].length)
			{
				S.CharacterAnimation.CrossFade(S.IdleAnim);
			}
			if (S.CharacterAnimation[S.GossipAnim].time >= S.CharacterAnimation[S.GossipAnim].length)
			{
				S.CharacterAnimation.CrossFade(S.IdleAnim);
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.TalkTimer <= 0f)
			{
				if (!S.Fed && S.Club != ClubType.Delinquent)
				{
					PickUpScript pickUp = S.Yandere.PickUp;
					S.Yandere.EmptyHands();
					S.EmptyHands();
					pickUp.GetComponent<MeshFilter>().mesh = S.StudentManager.OpenChipBag;
					pickUp.transform.parent = S.LeftItemParent;
					pickUp.transform.localPosition = new Vector3(-0.02f, -0.075f, 0f);
					pickUp.transform.localEulerAngles = new Vector3(-15f, -15f, 30f);
					pickUp.MyRigidbody.useGravity = false;
					pickUp.MyRigidbody.isKinematic = true;
					pickUp.Prompt.Hide();
					pickUp.Prompt.enabled = false;
					pickUp.enabled = false;
					S.BagOfChips = pickUp.gameObject;
					S.EatingSnack = true;
					S.Private = true;
					S.Fed = true;
				}
				S.DialogueWheel.End();
				S.StudentManager.UpdateStudents();
			}
		}
		else if (S.Interaction == StudentInteractionType.GivingHelp)
		{
			if (S.TalkTimer == 4f)
			{
				if (S.Club == ClubType.Council || S.Club == ClubType.Delinquent)
				{
					S.CharacterAnimation.CrossFade(S.GossipAnim);
					S.Subtitle.UpdateLabel(SubtitleType.RejectHelp, 0, 4f);
				}
				else if (S.Yandere.Bloodiness > 0f)
				{
					S.CharacterAnimation.CrossFade(S.GossipAnim);
					S.Subtitle.UpdateLabel(SubtitleType.RejectHelp, 1, 4f);
				}
				else
				{
					S.CharacterAnimation.CrossFade(S.PullBoxCutterAnim);
					S.SmartPhone.SetActive(false);
					S.Subtitle.UpdateLabel(SubtitleType.GiveHelp, 0, 4f);
				}
			}
			else if (!Input.GetButtonDown("A"))
			{
			}
			if (S.CharacterAnimation[S.GossipAnim].time >= S.CharacterAnimation[S.GossipAnim].length)
			{
				S.CharacterAnimation.CrossFade(S.IdleAnim);
			}
			if (S.CharacterAnimation[S.PullBoxCutterAnim].time >= S.CharacterAnimation[S.PullBoxCutterAnim].length)
			{
				S.CharacterAnimation.CrossFade(S.IdleAnim);
			}
			S.TalkTimer -= Time.deltaTime;
			if (S.Club != ClubType.Council || S.Club != ClubType.Delinquent)
			{
				S.MoveTowardsTarget(S.Yandere.transform.position + S.Yandere.transform.forward * 0.75f);
				if (S.CharacterAnimation[S.PullBoxCutterAnim].time >= S.CharacterAnimation[S.PullBoxCutterAnim].length)
				{
					S.CharacterAnimation.CrossFade(S.IdleAnim);
					StuckBoxCutter = null;
				}
				else if (S.CharacterAnimation[S.PullBoxCutterAnim].time >= 2f)
				{
					if (StuckBoxCutter.transform.parent != S.RightEye)
					{
						StuckBoxCutter.Prompt.enabled = true;
						StuckBoxCutter.enabled = true;
						StuckBoxCutter.transform.parent = S.Yandere.PickUp.transform;
						StuckBoxCutter.transform.localPosition = new Vector3(0f, 0.19f, 0f);
						StuckBoxCutter.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
					}
				}
				else if (S.CharacterAnimation[S.PullBoxCutterAnim].time >= 1.166666f && StuckBoxCutter == null)
				{
					StuckBoxCutter = S.Yandere.PickUp.StuckBoxCutter;
					S.Yandere.PickUp.StuckBoxCutter = null;
					StuckBoxCutter.FingerprintID = S.StudentID;
					StuckBoxCutter.transform.parent = S.RightHand;
					StuckBoxCutter.transform.localPosition = new Vector3(0f, 0f, 0f);
					StuckBoxCutter.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
				}
			}
			if (S.TalkTimer <= 0f)
			{
				S.DialogueWheel.End();
				S.StudentManager.UpdateStudents();
			}
		}
		else if (S.Interaction == StudentInteractionType.SentToLocker)
		{
			if (S.TalkTimer == 5f)
			{
				if (S.Club != ClubType.Delinquent)
				{
					Refuse = false;
					if ((S.Clock.HourTime > 8f && S.Clock.HourTime < 13f) || (S.Clock.HourTime > 13.375f && S.Clock.HourTime < 15.5f))
					{
						S.CharacterAnimation.CrossFade(S.GossipAnim);
						S.Subtitle.UpdateLabel(SubtitleType.SendToLocker, 1, 5f);
						Refuse = true;
					}
					else
					{
						S.CharacterAnimation.CrossFade(S.Nod1Anim);
						S.Subtitle.UpdateLabel(SubtitleType.SendToLocker, 2, 5f);
					}
				}
				else
				{
					S.Subtitle.UpdateLabel(SubtitleType.Dismissive, 5, 5f);
				}
			}
			else
			{
				if (Input.GetButtonDown("A"))
				{
					S.TalkTimer = 0f;
				}
				if (S.CharacterAnimation[S.Nod1Anim].time >= S.CharacterAnimation[S.Nod1Anim].length)
				{
					S.CharacterAnimation.CrossFade(IdleAnim);
				}
				if (S.TalkTimer <= 0f)
				{
					if (!Refuse)
					{
						S.Pathfinding.speed = 4f;
						S.TargetDistance = 1f;
						S.SentToLocker = true;
						S.Routine = false;
						S.CanTalk = false;
					}
					S.DialogueWheel.End();
				}
			}
			S.TalkTimer -= Time.deltaTime;
		}
		if (S.StudentID == 41 && !S.DialogueWheel.ClubLeader && S.TalkTimer > 0f)
		{
			Debug.Log("Geiju response.");
			if (NegativeResponse)
			{
				Debug.Log("Negative response.");
				S.Subtitle.UpdateLabel(SubtitleType.Impatience, 6, 5f);
			}
			else
			{
				S.Subtitle.UpdateLabel(SubtitleType.Impatience, 5, 5f);
			}
		}
		if (S.Waiting)
		{
			S.WaitTimer -= Time.deltaTime;
			if (!(S.WaitTimer <= 0f))
			{
				return;
			}
			S.DialogueWheel.TaskManager.UpdateTaskStatus();
			S.Talking = false;
			S.Waiting = false;
			base.enabled = false;
			if (!Fake && !S.StudentManager.CombatMinigame.Practice)
			{
				S.Pathfinding.canSearch = true;
				S.Pathfinding.canMove = true;
				S.Obstacle.enabled = false;
				S.Alarmed = false;
				if (!S.Following && !S.Distracting && !S.Wet && !S.EatingSnack && !S.SentToLocker)
				{
					S.Routine = true;
				}
				if (!S.Following)
				{
					ParticleSystem.EmissionModule emission = S.Hearts.emission;
					emission.enabled = false;
				}
			}
			S.StudentManager.EnablePrompts();
			if (S.GoAway)
			{
				Debug.Log("This student was just told to go away.");
				S.CurrentDestination = S.StudentManager.GoAwaySpots.List[S.StudentID];
				S.Pathfinding.target = S.StudentManager.GoAwaySpots.List[S.StudentID];
				S.DistanceToDestination = 100f;
			}
		}
		else
		{
			S.targetRotation = Quaternion.LookRotation(new Vector3(S.Yandere.transform.position.x, base.transform.position.y, S.Yandere.transform.position.z) - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, S.targetRotation, 10f * Time.deltaTime);
		}
	}

	private void CalculateRepBonus()
	{
		S.RepBonus = 0;
		if (PlayerGlobals.PantiesEquipped == 3)
		{
			S.RepBonus++;
		}
		if ((S.Male && PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus > 0) || PlayerGlobals.Seduction == 5)
		{
			S.RepBonus++;
		}
		if (PlayerGlobals.SocialBonus > 0)
		{
			S.RepBonus++;
		}
		S.ChameleonCheck();
		if (S.Chameleon)
		{
			S.RepBonus++;
		}
	}
}
