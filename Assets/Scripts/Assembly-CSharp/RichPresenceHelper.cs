using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RichPresenceHelper : MonoBehaviour
{
	private void Start()
	{
		Debug.Log("FORCED:RPC_DID_NOT_CONNECT");
		//CompileDictionaries();
		//_discordController = GetComponent<DiscordController>();
		//Object.DontDestroyOnLoad(base.gameObject);
		//_discordController.enabled = false;
		//_discordController.presence.state = GetSceneDescription();
		//_discordController.enabled = true;
		//DiscordRpc.UpdatePresence(ref _discordController.presence);
		//InvokeRepeating("UpdatePresence", 0f, 10f);
	}

	//private void OnLevelWasLoaded(int level)
	//{
	//	if (level == 9)
	//	{
	//		_clockScript = Object.FindObjectOfType<ClockScript>();
	//	}
	//	UpdatePresence();
	//}
//
	//private void UpdatePresence()
	//{
	//	_discordController.presence.state = GetSceneDescription();
	//	DiscordRpc.UpdatePresence(ref _discordController.presence);
	//}
//
	//private void CompileDictionaries()
	//{
	//	_weekdays.Add(1, "Monday");
	//	_weekdays.Add(2, "Tuesday");
	//	_weekdays.Add(3, "Wednesday");
	//	_weekdays.Add(4, "Thursday");
	//	_weekdays.Add(5, "Friday");
	//	_periods.Add(1, "Before Class");
	//	_periods.Add(2, "Class Time");
	//	_periods.Add(3, "Lunch Time");
	//	_periods.Add(4, "Class Time");
	//	_periods.Add(5, "Cleaning Time");
	//	_periods.Add(6, "After School");
	//	_sceneDescriptions.Add("WelcomeScene", "Launching the game!");
	//	_sceneDescriptions.Add("SponsorScene", "Checking out the sponsors!");
	//	_sceneDescriptions.Add("TitleScene", "At the title screen!");
	//	_sceneDescriptions.Add("SenpaiScene", "Customizing Senpai!");
	//	_sceneDescriptions.Add("IntroScene", "Watching the Intro!");
	//	_sceneDescriptions.Add("PhoneScene", "Texting with Info-Chan!");
	//	_sceneDescriptions.Add("CalendarScene", "Checking out the Calendar!");
	//	_sceneDescriptions.Add("HomeScene", "Chilling at home!");
	//	_sceneDescriptions.Add("LoadingScene", "Now Loading!");
	//	_sceneDescriptions.Add("SchoolScene", "At School");
	//	_sceneDescriptions.Add("YanvaniaTitleScene", "Beginning Yanvania: Senpai of the Night!");
	//	_sceneDescriptions.Add("YanvaniaScene", "Playing Yanvania: Senpai of the Night!");
	//	_sceneDescriptions.Add("LivingRoomScene", "Chatting with Kokona!");
	//	_sceneDescriptions.Add("MissionModeScene", "Acceping a mission!");
	//	_sceneDescriptions.Add("VeryFunScene", "??????????");
	//	_sceneDescriptions.Add("CreditsScene", "Viewing the credits!");
	//	_sceneDescriptions.Add("MiyukiTitleScene", "Beginning Magical Girl Pretty Miyuki!");
	//	_sceneDescriptions.Add("MiyukiGameplayScene", "Playing Magical Girl Pretty Miyuki!");
	//	_sceneDescriptions.Add("MiyukiThanksScene", "Finishing Magical Girl Pretty Miyuki!");
	//	_sceneDescriptions.Add("RhythmMinigameScene", "Jamming out with the Light Music Club!");
	//	_sceneDescriptions.Add("LifeNoteScene", "Watching an episode of Life Note!");
	//}
//
	//private string GetSceneDescription()
	//{
	//	string text = SceneManager.GetActiveScene().name;
	//	if (text != null && text == "SchoolScene")
	//	{
	//		string text2 = ((!MissionModeGlobals.MissionMode) ? string.Empty : ", Mission Mode");
	//		return string.Format("{0}, {1}, {2}, {3}{4}", _sceneDescriptions["SchoolScene"], _clockScript.TimeLabel.text, _periods[_clockScript.Period], _weekdays[_clockScript.Weekday], text2);
	//	}
	//	if (_sceneDescriptions.ContainsKey(text))
	//	{
	//		return _sceneDescriptions[text];
	//	}
	//	return "No description available yet.";
	//}
}
