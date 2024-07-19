using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

[Serializable]
public class StudentJson : JsonData
{
	[SerializeField]
	private string name;

	[SerializeField]
	private int gender;

	[SerializeField]
	private int classID;

	[SerializeField]
	private int seat;

	[SerializeField]
	private ClubType club;

	[SerializeField]
	private PersonaType persona;

	[SerializeField]
	private int crush;

	[SerializeField]
	private float breastSize;

	[SerializeField]
	private int strength;

	[SerializeField]
	private string hairstyle;

	[SerializeField]
	private string color;

	[SerializeField]
	private string eyes;

	[SerializeField]
	private string eyeType;

	[SerializeField]
	private string stockings;

	[SerializeField]
	private string accessory;

	[SerializeField]
	private string info;

	[SerializeField]
	private ScheduleBlock[] scheduleBlocks;

	[SerializeField]
	private bool success;

	public static string FilePath
	{
		get
		{
			return Path.Combine(JsonData.FolderPath, "Students.json");
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public int Gender
	{
		get
		{
			return gender;
		}
	}

	public int Class
	{
		get
		{
			return classID;
		}
		set
		{
			classID = value;
		}
	}

	public int Seat
	{
		get
		{
			return seat;
		}
		set
		{
			seat = value;
		}
	}

	public ClubType Club
	{
		get
		{
			return club;
		}
	}

	public PersonaType Persona
	{
		get
		{
			return persona;
		}
		set
		{
			persona = value;
		}
	}

	public int Crush
	{
		get
		{
			return crush;
		}
	}

	public float BreastSize
	{
		get
		{
			return breastSize;
		}
		set
		{
			breastSize = value;
		}
	}

	public int Strength
	{
		get
		{
			return strength;
		}
		set
		{
			strength = value;
		}
	}

	public string Hairstyle
	{
		get
		{
			return hairstyle;
		}
		set
		{
			hairstyle = value;
		}
	}

	public string Color
	{
		get
		{
			return color;
		}
	}

	public string Eyes
	{
		get
		{
			return eyes;
		}
	}

	public string EyeType
	{
		get
		{
			return eyeType;
		}
	}

	public string Stockings
	{
		get
		{
			return stockings;
		}
	}

	public string Accessory
	{
		get
		{
			return accessory;
		}
		set
		{
			accessory = value;
		}
	}

	public string Info
	{
		get
		{
			return info;
		}
	}

	public ScheduleBlock[] ScheduleBlocks
	{
		get
		{
			return scheduleBlocks;
		}
	}

	public bool Success
	{
		get
		{
			return success;
		}
	}


	public static StudentJson[] LoadFromJson(string path)
	{
		path = Path.Combine(Application.streamingAssetsPath, "Students.json");
		string jsondata = @"[
	{""ID"":""1"",""Name"":""Taro Yamada"",""Gender"":""1"",""Class"":""32"",""Seat"":""15"",""Club"":""0"",""Persona"":""1"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""1"",""Color"":""Black"",""Eyes"":""Black"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.01_13.375_15.51_16_17.25_99_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Hangout_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Read_Sit_Eat_Sit_Clean_Read_Shoes_Stand"",""Info"":""An average student. \n \n Average grades, average looks, average life... \n \n I'm not sure what you see in him.""},
	{""ID"":""2"",""Name"":""Sakyu Basu"",""Gender"":""0"",""Class"":""22"",""Seat"":""6"",""Club"":""0"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""0"",""Hairstyle"":""10"",""Color"":""Succubus1"",""Eyes"":""Succubus1"",""EyeType"":""Round"",""Stockings"":""None"",""Accessory"":""3"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_16.5_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Socialize_Sit_Eat_Sit_Clean_Shoes_Stand"",""Info"":""Wears contact lenses. \n \n Enjoys spending time with her younger sister. \n \n Rumored to be a succubus disguised as a high school student...but only a fool would believe something like that.""},
	{""ID"":""3"",""Name"":""Inkyu Basu"",""Gender"":""0"",""Class"":""22"",""Seat"":""7"",""Club"":""0"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""11"",""Color"":""Succubus2"",""Eyes"":""Succubus2"",""EyeType"":""Round"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_16.5_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Socialize_Sit_Eat_Sit_Clean_Shoes_Stand"",""Info"":""Wears contact lenses. \n \n Enjoys spending time with her older sister. \n \n Rumored to be a vampire disguised as a high school student...but only a fool would believe something like that.""},
	{""ID"":""4"",""Name"":""Kuu Dere"",""Gender"":""0"",""Class"":""22"",""Seat"":""8"",""Club"":""0"",""Persona"":""1"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""12"",""Color"":""Clubless"",""Eyes"":""Clubless"",""EyeType"":""Serious"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_16.5_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Read_Sit_Eat_Sit_Clean_Shoes_Stand"",""Info"":""The school librarian. \n \n Strongly prefers solitude. \n \n Very rarely expresses emotions.""},
	{""ID"":""5"",""Name"":""Horuda Puresu"",""Gender"":""0"",""Class"":""22"",""Seat"":""10"",""Club"":""0"",""Persona"":""11"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""31"",""Color"":""Clubless"",""Eyes"":""Clubless"",""EyeType"":""Sad"",""Stockings"":""ShortBlack"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Read_Sit_Eat_Sit_Clean_Shoes_Stand"",""Info"":""A shy and timid girl. \n \n She is usually targeted for bullying. \n \n A temporary placeholder student. Don't get too attached to her.""},
	{""ID"":""6"",""Name"":""Kyuji Konagawa"",""Gender"":""1"",""Class"":""31"",""Seat"":""12"",""Club"":""0"",""Persona"":""1"",""Crush"":""99"",""BreastSize"":""0"",""Strength"":""1"",""Hairstyle"":""54"",""Color"":""Clubless"",""Eyes"":""Clubless"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Relax_Sit_Eat_Sit_Clean_Shoes_Stand"",""Info"":""A student who was popular and sociable until a few weeks ago, when he suddenly became very withdrawn and distant. He doesn't seem to be willing to discuss his feelings with anyone, and prefers to be alone with his thoughts.""},
	{""ID"":""7"",""Name"":""Otohiko Meichi"",""Gender"":""1"",""Class"":""11"",""Seat"":""12"",""Club"":""0"",""Persona"":""4"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""56"",""Color"":""Clubless"",""Eyes"":""Clubless"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_7.9_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Relax_Sit_Eat_Sit_Clean_Shoes_Stand"",""Info"":""A frail and sickly boy. Because of his poor health, he can often be found in the school infirmary. Usually oversleeps and arrives at school later than any other student. Sometimes ridiculed for his unusually feminine mannerisms. Known to be extraordinarily clumsy.""},
	{""ID"":""8"",""Name"":""Hazu Kashibuchi"",""Gender"":""1"",""Class"":""12"",""Seat"":""12"",""Club"":""0"",""Persona"":""4"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""57"",""Color"":""Clubless"",""Eyes"":""Clubless"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Hangout"",""ScheduleAction"":""Stand_Stand_Sew_Sit_Eat_Sit_Clean_Sew"",""Info"":""A very timid student. His worst fear is having negative social interactions, so he chooses to seclude himself from others. He is exceptionally talented at sewing, and can often be found in the Sewing Room.""},
	{""ID"":""9"",""Name"":""Toga Tabara"",""Gender"":""1"",""Class"":""32"",""Seat"":""12"",""Club"":""0"",""Persona"":""1"",""Crush"":""99"",""BreastSize"":""0"",""Strength"":""1"",""Hairstyle"":""58"",""Color"":""Clubless"",""Eyes"":""Clubless"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Eat_Sit_Clean_Shoes_Stand"",""Info"":""A student who is struggling to determine what he is going to do with his life after graduating. Last year, he spent time as a member of each club, but didn't feel like he belonged in any of them. He can often be seen wandering the school aimlessly, deep in thought, contemplating his future.""},
	{""ID"":""10"",""Name"":""Mysterious Obstacle"",""Gender"":""0"",""Class"":""21"",""Seat"":""12"",""Club"":""0"",""Persona"":""15"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""9"",""Hairstyle"":""21"",""Color"":""White"",""Eyes"":""Red"",""EyeType"":""Gentle"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.01_13.375_15.51_16_17.25_99_99"",""ScheduleDestination"":""Spawn_Locker_Follow_Seat_Follow_Seat_Follow_Follow_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Follow_Sit_Follow_Sit_Clean_Follow_Shoes_Exit"",""Info"":""""},
	{""ID"":""11"",""Name"":""Osana Najimi"",""Gender"":""0"",""Class"":""21"",""Seat"":""11"",""Club"":""0"",""Persona"":""7"",""Crush"":""1"",""BreastSize"":""1"",""Strength"":""1"",""Hairstyle"":""20"",""Color"":""Osana"",""Eyes"":""Osana"",""EyeType"":""Rival1"",""Stockings"":""Osana"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.01_13.375_15.51_16_17.25_99_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Hangout_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Socialize_Sit_Eat_Sit_Clean_Socialize_Shoes_Exit"",""Info"":""""},
	{""ID"":""12"",""Name"":""Amai Odayaka"",""Gender"":""0"",""Class"":""22"",""Seat"":""1"",""Club"":""0"",""Persona"":""0"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_Patrol_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Patrol_Sit_Clean_Patrol"",""Info"":""""},
	{""ID"":""13"",""Name"":""Kizana Sunobu"",""Gender"":""0"",""Class"":""22"",""Seat"":""2"",""Club"":""0"",""Persona"":""0"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_Patrol_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Patrol_Sit_Clean_Patrol"",""Info"":""""},
	{""ID"":""14"",""Name"":""Oka Ruto"",""Gender"":""0"",""Class"":""22"",""Seat"":""3"",""Club"":""0"",""Persona"":""0"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_Patrol_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Patrol_Sit_Clean_Patrol"",""Info"":""""},
	{""ID"":""15"",""Name"":""Asu Rito"",""Gender"":""0"",""Class"":""22"",""Seat"":""9"",""Club"":""0"",""Persona"":""0"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_Patrol_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Patrol_Sit_Clean_Patrol"",""Info"":""""},
	{""ID"":""16"",""Name"":""Muja Kina"",""Gender"":""0"",""Class"":""22"",""Seat"":""4"",""Club"":""0"",""Persona"":""0"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_Patrol_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Patrol_Sit_Clean_Patrol"",""Info"":""""},
	{""ID"":""17"",""Name"":""Mida Rana"",""Gender"":""0"",""Class"":""22"",""Seat"":""5"",""Club"":""0"",""Persona"":""0"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_Patrol_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Patrol_Sit_Clean_Patrol"",""Info"":""""},
	{""ID"":""18"",""Name"":""Osoro Shidesu"",""Gender"":""0"",""Class"":""22"",""Seat"":""12"",""Club"":""0"",""Persona"":""0"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_Patrol_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Patrol_Sit_Clean_Patrol"",""Info"":""""},
	{""ID"":""19"",""Name"":""Hanako Yamada"",""Gender"":""0"",""Class"":""12"",""Seat"":""13"",""Club"":""0"",""Persona"":""0"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_Patrol_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Patrol_Sit_Clean_Patrol"",""Info"":""""},
	{""ID"":""20"",""Name"":""Megami Saikou"",""Gender"":""0"",""Class"":""22"",""Seat"":""14"",""Club"":""0"",""Persona"":""0"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_Patrol_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Patrol_Sit_Clean_Patrol"",""Info"":""""},
	{""ID"":""21"",""Name"":""Shoku Tsuburaya"",""Gender"":""1"",""Class"":""32"",""Seat"":""1"",""Club"":""1"",""Persona"":""6"",""Crush"":""99"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""50"",""Color"":""Cooking"",""Eyes"":""Cooking"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Socialize"",""Info"":""Trusted to operate the Cooking Club when the leader of the club is not present. A connoisseur of good food, with a very discerning palate. Dreams of becoming a world-famous 5-star chef.""},
	{""ID"":""22"",""Name"":""Kenko Sukoyaka"",""Gender"":""1"",""Class"":""31"",""Seat"":""1"",""Club"":""1"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""51"",""Color"":""Cooking"",""Eyes"":""Cooking"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Socialize"",""Info"":""Committed to maintaining a healthy diet; intends to abstain from junk food for his entire life. He doesn't judge others for their dietary choices, but he evangelizes the benefits of healthy food at every opportunity.""},
	{""ID"":""23"",""Name"":""Seiyo Akanishi"",""Gender"":""1"",""Class"":""21"",""Seat"":""1"",""Club"":""1"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""52"",""Color"":""Cooking"",""Eyes"":""Cooking"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.0001_13.375_15.5001_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Socialize"",""Info"":""Ethnically and nationally Japanese, but obsessed with everything western; from western cartoons to western food. Loves to use English words in everyday speech. Believes he was a cowboy in a previous life.""},
	{""ID"":""24"",""Name"":""Ajia Ashitomi"",""Gender"":""0"",""Class"":""12"",""Seat"":""1"",""Club"":""1"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""44"",""Color"":""Cooking"",""Eyes"":""Cooking"",""EyeType"":""Smug"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Socialize"",""Info"":""Extremely proud to be Japanese, and celebrates Japanese culture at every opportunity. Dislikes the influence of western culture in Japan. Determined to master the art of preparing Japanese cuisine.""},
	{""ID"":""25"",""Name"":""Saki Miyu"",""Gender"":""0"",""Class"":""11"",""Seat"":""1"",""Club"":""1"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1.8"",""Strength"":""0"",""Hairstyle"":""45"",""Color"":""Cooking"",""Eyes"":""Cooking"",""EyeType"":""Round"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.01_13.375_15.51_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Socialize"",""Info"":""Loves sweets, candy, and desserts. Never gets fat, no matter how much junk food she eats. Kokona Haruka's best friend and closest confidant. Kokona is likely to discuss personal matters with this girl.""},
	{""ID"":""26"",""Name"":""Tsuruzo Yamazaki"",""Gender"":""1"",""Class"":""32"",""Seat"":""2"",""Club"":""2"",""Persona"":""6"",""Crush"":""99"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""46"",""Color"":""Drama"",""Eyes"":""Drama"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""Trusted by Kizana Sunobu to operate the Drama Club when she is not present. An aspiring actor with a penchant for giving melodramatic speeches, even when he's not onstage.""},
	{""ID"":""27"",""Name"":""Shozo Kurosawa"",""Gender"":""1"",""Class"":""31"",""Seat"":""2"",""Club"":""2"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""47"",""Color"":""Drama"",""Eyes"":""Drama"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""Addicted to making short amateur films - several per month. Despite his young age, he is rapidly gaining experience in directing, producing, scriptwriting, and even video editing.""},
	{""ID"":""28"",""Name"":""Riku Soma"",""Gender"":""1"",""Class"":""21"",""Seat"":""2"",""Club"":""2"",""Persona"":""6"",""Crush"":""30"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""7"",""Color"":""Drama"",""Eyes"":""Drama"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.0002_13.375_15.5002_16_17.5_99_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Shoes_Stand"",""Info"":""Comes from one of the most wealthy families in Japan. He never flaunts his money, but he has an unusually posh way of speaking, which makes his affluent origins very obvious.""},
	{""ID"":""29"",""Name"":""Tokuko Kitagawa"",""Gender"":""0"",""Class"":""12"",""Seat"":""2"",""Club"":""2"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""43"",""Color"":""Drama"",""Eyes"":""Drama"",""EyeType"":""Smug"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""An aspiring actress whose goal is to become the most famous performer in Japan. Because of her headstrong personality, she is constantly at odds with the drama club's leader.""},
	{""ID"":""30"",""Name"":""Kokona Haruka"",""Gender"":""0"",""Class"":""11"",""Seat"":""2"",""Club"":""2"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""2"",""Strength"":""0"",""Hairstyle"":""7"",""Color"":""Drama"",""Eyes"":""Drama"",""EyeType"":""Round"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.01_13.375_15.51_16_17.25_99_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club_Locker_Exit"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Shoes_Stand"",""Info"":""Admires Kizana Sunobu, leader of the Drama Club, and joined the club to learn how to be more like her. This annoys Kizana, who doesn't want anyone imitating her unique style.""},
	{""ID"":""31"",""Name"":""Shin Higaku"",""Gender"":""1"",""Class"":""32"",""Seat"":""3"",""Club"":""3"",""Persona"":""2"",""Crush"":""99"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""12"",""Color"":""Occult2"",""Eyes"":""Occult2"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Patrol_Club"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Eat_Sit_Clean_Patrol_Club"",""Info"":""Trusted by Oka Ruto to operate the Occult Club when she is not present. A soft-spoken and introverted young man who doesn't seem passionate about anything...except for black magic, ghosts, and demons.""},
	{""ID"":""32"",""Name"":""Chojo Tekina"",""Gender"":""1"",""Class"":""31"",""Seat"":""3"",""Club"":""3"",""Persona"":""4"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""13"",""Color"":""Occult4"",""Eyes"":""Occult4"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Eat_Sit_Clean_Patrol"",""Info"":""No student has ever seen the right side of his face. \n \n Some students suspect that he is using his hair to hide an unsightly scar or missing eye.""},
	{""ID"":""33"",""Name"":""Daku Atsu"",""Gender"":""1"",""Class"":""21"",""Seat"":""3"",""Club"":""3"",""Persona"":""4"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""14"",""Color"":""Occult6"",""Eyes"":""Occult6"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""2"",""ScheduleTime"":""7_7_8_13.0003_13.375_15.5003_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Eat_Sit_Clean_Patrol"",""Info"":""One of the lenses of his glasses is completely opaque. No student has ever seen his right eye. \n \n Some students suspect that he only has one eye, and prefers to wear an opaque lense over that eye rather than an eyepatch.""},
	{""ID"":""34"",""Name"":""Supana Churu"",""Gender"":""0"",""Class"":""12"",""Seat"":""3"",""Club"":""3"",""Persona"":""4"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""17"",""Color"":""Occult3"",""Eyes"":""Occult3"",""EyeType"":""Serious"",""Stockings"":""None"",""Accessory"":""1"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Eat_Sit_Clean_Patrol"",""Info"":""Claims to be wearing a medical eyepatch to correct a problem with her vision. \n \n Refuses to provide details regarding her eye condition, leading to rumors that she is lying about the reason she wears an eyepatch.""},
	{""ID"":""35"",""Name"":""Kokuma Jutsu"",""Gender"":""0"",""Class"":""11"",""Seat"":""3"",""Club"":""3"",""Persona"":""4"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""18"",""Color"":""Occult5"",""Eyes"":""Occult5"",""EyeType"":""Serious"",""Stockings"":""None"",""Accessory"":""2"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Patrol_Sit_Eat_Sit_Clean_Patrol"",""Info"":""Claims that the bandages on her face are the result of being attacked by a wild animal shortly before the school year began. \n \n There are rumors that the true reason she wears bandages is because she is regularly beaten by a family member, and was blinded in one eye during a domestic dispute.""},
	{""ID"":""36"",""Name"":""Gema Taku"",""Gender"":""1"",""Class"":""32"",""Seat"":""11"",""Club"":""11"",""Persona"":""1"",""Crush"":""81"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""48"",""Color"":""Gaming"",""Eyes"":""Gaming"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""16"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_Club_Seat_Clean_Club_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Club_Sit_Clean_Club_Club"",""Info"":""Leader of the Gaming Club. Completely obsessed with anime and video games, and has no other interests or hobbies. Has a somewhat abrasive personality; very few students at school can tolerate him.""},
	{""ID"":""37"",""Name"":""Ryuto Ippongo"",""Gender"":""1"",""Class"":""31"",""Seat"":""11"",""Club"":""11"",""Persona"":""10"",""Crush"":""38"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""8"",""Color"":""Ryuto"",""Eyes"":""Ryuto"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""1"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Miyuki_Seat_LunchSpot_Seat_Clean_Miyuki_Club"",""ScheduleAction"":""Stand_Stand_Miyuki_Sit_Eat_Sit_Clean_Miyuki_Club"",""Info"":""Secretly has a crush on Pippi Osu. Neither of them realize that they both share feelings for each other. \n Swears to his parents that he spends all of his free time studying, but is usually playing video games.""},
	{""ID"":""38"",""Name"":""Pippi Osu"",""Gender"":""0"",""Class"":""22"",""Seat"":""11"",""Club"":""11"",""Persona"":""10"",""Crush"":""37"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""8"",""Color"":""Pippi"",""Eyes"":""Pippi"",""EyeType"":""Round"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Miyuki_Seat_LunchSpot_Seat_Clean_Miyuki_Club"",""ScheduleAction"":""Stand_Stand_Miyuki_Sit_Eat_Sit_Clean_Miyuki_Club"",""Info"":""Obsessed with rhythm games. Currently, she is addicted to ''Pretty Guardian Miyuki'', an online free-to-play augmented reality role-playing mobile phone game about collecting ''waifus'' and hunting monsters.""},
	{""ID"":""39"",""Name"":""Midori Gurin"",""Gender"":""0"",""Class"":""12"",""Seat"":""11"",""Club"":""11"",""Persona"":""10"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""9"",""Color"":""Midori"",""Eyes"":""Midori"",""EyeType"":""Round"",""Stockings"":""ShortGreen"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Miyuki_Seat_LunchSpot_Seat_Clean_Miyuki_Club"",""ScheduleAction"":""Stand_Stand_Miyuki_Sit_Eat_Sit_Clean_Miyuki_Club"",""Info"":""Known for irritating her classmates and teachers by constantly asking foolish questions. Stubbornly insists that she is a video game character who can see beyond the fourth wall of the game she's allegedly in.""},
	{""ID"":""40"",""Name"":""Mai Waifu"",""Gender"":""0"",""Class"":""11"",""Seat"":""11"",""Club"":""11"",""Persona"":""10"",""Crush"":""0"",""BreastSize"":""2"",""Strength"":""0"",""Hairstyle"":""13"",""Color"":""Waifu"",""Eyes"":""Waifu"",""EyeType"":""Gentle"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_17_99"",""ScheduleDestination"":""Spawn_Locker_Miyuki_Seat_LunchSpot_Seat_Clean_Miyuki_Club"",""ScheduleAction"":""Stand_Stand_Miyuki_Sit_Eat_Sit_Clean_Miyuki_Club"",""Info"":""A young woman who is pursuing an ''anime aesthetic''. To achieve this unusual goal, she never cuts her hair, wears special contact lenses, and asks people to refer to her by a nickname inspired by otaku culture.""},
	{""ID"":""41"",""Name"":""Geiju Tsuka"",""Gender"":""1"",""Class"":""32"",""Seat"":""4"",""Club"":""4"",""Persona"":""1"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""35"",""Color"":""Art"",""Eyes"":""Art"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""13"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""Leader of the Art Club. A man of few words; speaks exclusively in one-word or two-word sentences. Obsessed with painting; does nothing else with his time. Nobody can figure out if he is a silent genius or a pretentious snob.""},
	{""ID"":""42"",""Name"":""Borupen Saishiki"",""Gender"":""1"",""Class"":""31"",""Seat"":""4"",""Club"":""4"",""Persona"":""12"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""36"",""Color"":""Art"",""Eyes"":""Art"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Spawn_Locker_Sketch_Sit_Eat_Sit_Clean_Club"",""Info"":""A brooding, melancholy boy who exclusively paints very dark and morbid subjects. Although his interests align with the Occult Club, he doesn't seem interested in joining them.""},
	{""ID"":""43"",""Name"":""Enpitsu Byoga"",""Gender"":""1"",""Class"":""21"",""Seat"":""4"",""Club"":""4"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""37"",""Color"":""Art"",""Eyes"":""Art"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.0004_13.375_15.5004_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Spawn_Locker_Sketch_Sit_Eat_Sit_Clean_Club"",""Info"":""A vain, narcisstic boy who is obsessed with beauty - especially his own. He is only interested in drawing ''beautiful'' subjects...and self-portraits.""},
	{""ID"":""44"",""Name"":""Maka Tansei"",""Gender"":""0"",""Class"":""12"",""Seat"":""4"",""Club"":""4"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""34"",""Color"":""Art"",""Eyes"":""Art"",""EyeType"":""Round"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Spawn_Locker_Sketch_Sit_Eat_Sit_Clean_Club"",""Info"":""A quirky, eccentric girl who doesn't really care what others think of her. She only draws bizarre, abstract artwork that doesn't make sense to anyone but herself.""},
	{""ID"":""45"",""Name"":""Efude Nurimono"",""Gender"":""0"",""Class"":""11"",""Seat"":""4"",""Club"":""4"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""35"",""Color"":""Art"",""Eyes"":""Art"",""EyeType"":""Round"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Spawn_Locker_Sketch_Sit_Eat_Sit_Clean_Club"",""Info"":""An anime-obsessed girl who spends all of her time drawing anime and manga characters. Didn't fit in with the Photography Club or Gaming Club, so she settled for the Art Club.""},
	{""ID"":""46"",""Name"":""Budo Masuta"",""Gender"":""1"",""Class"":""32"",""Seat"":""6"",""Club"":""6"",""Persona"":""3"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""5"",""Hairstyle"":""9"",""Color"":""MartialArt"",""Eyes"":""MartialArt"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""President of the Martial Arts Club. Inherited the club after defeating the previous president. \n \n Seems to be incapable of turning down a challenge. \n \n Always gung ho and enthusiastic. Sometimes a bit overzealous, especially about martial arts.""},
	{""ID"":""47"",""Name"":""Sho Kunin"",""Gender"":""1"",""Class"":""31"",""Seat"":""6"",""Club"":""6"",""Persona"":""3"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""3"",""Hairstyle"":""10"",""Color"":""MartialArt"",""Eyes"":""MartialArt"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""Journeyman-level disciple of Budo Masuta.""},
	{""ID"":""48"",""Name"":""Juku Ren"",""Gender"":""1"",""Class"":""21"",""Seat"":""6"",""Club"":""6"",""Persona"":""3"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""1"",""Hairstyle"":""11"",""Color"":""MartialArt"",""Eyes"":""MartialArt"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.0005_13.375_15.5005_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""Apprentice-level disciple of Budo Masuta.""},
	{""ID"":""49"",""Name"":""Mina Rai"",""Gender"":""0"",""Class"":""12"",""Seat"":""6"",""Club"":""6"",""Persona"":""3"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""4"",""Hairstyle"":""14"",""Color"":""MartialArt"",""Eyes"":""MartialArt"",""EyeType"":""Round"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""Journeyman-level disciple of Budo Masuta.""},
	{""ID"":""50"",""Name"":""Shima Shita"",""Gender"":""0"",""Class"":""11"",""Seat"":""6"",""Club"":""6"",""Persona"":""3"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""2"",""Hairstyle"":""15"",""Color"":""MartialArt"",""Eyes"":""MartialArt"",""EyeType"":""Round"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""Apprentice-level disciple of Budo Masuta.""},
	{""ID"":""51"",""Name"":""Miyuji Shan"",""Gender"":""0"",""Class"":""32"",""Seat"":""5"",""Club"":""5"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""46"",""Color"":""Music"",""Eyes"":""Music"",""EyeType"":""Round"",""Stockings"":""Music1"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_16.9_99"",""ScheduleDestination"":""Spawn_Locker_Practice_Seat_Lyrics_Seat_Clean_Practice_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Lyrics_Sit_Clean_Club_Club"",""Info"":""President of the Light Music Club. For most of her life, she was a shy, timid, quiet girl. During the previous school year, she surprised everyone by suddenly changing her appearance and personality overnight. Vocalist for her band, the ''Strawberry Thieves''. Practicing guitar.""},
	{""ID"":""52"",""Name"":""Gita Yamahato"",""Gender"":""0"",""Class"":""31"",""Seat"":""5"",""Club"":""5"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""47"",""Color"":""Music"",""Eyes"":""Music"",""EyeType"":""Round"",""Stockings"":""Music2"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_16.9_99"",""ScheduleDestination"":""Spawn_Locker_Practice_Seat_LunchSpot_Seat_Clean_Practice_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Club"",""Info"":""Former president of the Light Music Club. Established her club on a whim, and only used it as a place to hang out with friends. After Miyuji demonstrated a desire to do something meaningful with the club, Gita happily transfered ownership of the club to Miyuji. Plays guitar.""},
	{""ID"":""53"",""Name"":""Beshi Takamine"",""Gender"":""0"",""Class"":""21"",""Seat"":""5"",""Club"":""5"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""48"",""Color"":""Music"",""Eyes"":""Music"",""EyeType"":""Round"",""Stockings"":""Music3"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.0006_13.375_15.5006_16_16.9_99"",""ScheduleDestination"":""Spawn_Locker_Practice_Seat_LunchSpot_Seat_Clean_Practice_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Club"",""Info"":""The bassist of the Strawberry Thieves. A jack-of-all-trades, she can play almost any instrument, but can't play any of them at a professional level. Leaves complicated stuff to her bandmates. Willing to play backup instruments (such as the maracas) when the song calls for it.""},
	{""ID"":""54"",""Name"":""Dora Tamamoto"",""Gender"":""0"",""Class"":""12"",""Seat"":""5"",""Club"":""5"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""49"",""Color"":""Music"",""Eyes"":""Music"",""EyeType"":""Round"",""Stockings"":""Music4"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_16.9_99"",""ScheduleDestination"":""Spawn_Locker_Practice_Seat_LunchSpot_Seat_Clean_Practice_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Club"",""Info"":""The drummer of the Strawberry Thieves. Originally felt uninterested in joining a band, but is now one of the most enthusiastic members. She doesn't mind performing at the back of the stage behind her bandmates, because she becomes shy if there are too many eyes on her.""},
	{""ID"":""55"",""Name"":""Kiba Kawaito"",""Gender"":""0"",""Class"":""11"",""Seat"":""5"",""Club"":""5"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""50"",""Color"":""Music"",""Eyes"":""Music"",""EyeType"":""Round"",""Stockings"":""Music5"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_16.9_99"",""ScheduleDestination"":""Spawn_Locker_Practice_Seat_LunchSpot_Seat_Clean_Practice_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club_Club"",""Info"":""The keyboardist of the Strawberry Thieves. Strongly prefers synth instruments over traditional acoustic instruments. Firmly believes that the keytar is the coolest instrument to ever be invented. Willing to play backup instruments (such as the cowbell) when the song calls for it.""},
	{""ID"":""56"",""Name"":""Fureddo Jonzu"",""Gender"":""1"",""Class"":""32"",""Seat"":""7"",""Club"":""7"",""Persona"":""13"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""3"",""Hairstyle"":""32"",""Color"":""Photo"",""Eyes"":""Photo"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""10"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_SocialSit_Sit_Eat_Sit_Clean_SocialSit"",""Info"":""Leader of the Photography Club. Has no interest in photography, and uses the clubroom as a place to read manga, eat snacks, and goof around with friends. \n \n Prefers to spend his time slacking off, but when he actually gets serious about something, his resolve is unbreakable.""},
	{""ID"":""57"",""Name"":""Rojasu Norubiru"",""Gender"":""1"",""Class"":""31"",""Seat"":""7"",""Club"":""7"",""Persona"":""13"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""33"",""Color"":""Photo"",""Eyes"":""Photo"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""11"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_SocialSit_Sit_Eat_Sit_Clean_SocialSit"",""Info"":""The most laid-back student at school. Never takes anything seriously, and is relaxed at all times. \n \n Despite being lazy most of the time, he cares deeply about his friends, and if any of them were in danger, he would leap at the chance to protect them.""},
	{""ID"":""58"",""Name"":""Sukubi Dubidu"",""Gender"":""1"",""Class"":""21"",""Seat"":""7"",""Club"":""7"",""Persona"":""13"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""0"",""Hairstyle"":""34"",""Color"":""Photo"",""Eyes"":""Photo"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""12"",""ScheduleTime"":""7_7_8_13.0007_13.375_15.5007_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_SocialSit_Sit_Eat_Sit_Clean_SocialSit"",""Info"":""Known for taking naps and eating snacks in class. Lazy and flaky, but not a bad person. \n \n Best friends with Rojasu Norubiru, and shares many personality traits with him - including being fiercely protective of his friends.""},
	{""ID"":""59"",""Name"":""Dafuni Bureiku"",""Gender"":""0"",""Class"":""12"",""Seat"":""7"",""Club"":""7"",""Persona"":""13"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""0"",""Hairstyle"":""32"",""Color"":""Photo"",""Eyes"":""Photo"",""EyeType"":""Round"",""Stockings"":""None"",""Accessory"":""12"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_SocialSit_Sit_Eat_Sit_Clean_SocialSit"",""Info"":""Grew up with maids and butlers taking care of her every need. Highly concerned with her appearance and clothing at all times. \n \n After befriending Beruma Dinkuri in junior high, the two of them became inseperable, despite being polar opposites.""},
	{""ID"":""60"",""Name"":""Beruma Dinkuri"",""Gender"":""0"",""Class"":""11"",""Seat"":""7"",""Club"":""7"",""Persona"":""13"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""33"",""Color"":""Photo"",""Eyes"":""Photo"",""EyeType"":""Serious"",""Stockings"":""None"",""Accessory"":""13"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_SocialSit_Sit_Eat_Sit_Clean_SocialSit"",""Info"":""One of the most hard-working and intelligent students in school. Can get so obsessed with studying that she forgets to take care of herself. \n \n Her flaws are balanced by the strengths of her best friend, Dafuni Bureiku (and vice versa).""},
	{""ID"":""61"",""Name"":""Kaga Kusha"",""Gender"":""1"",""Class"":""32"",""Seat"":""8"",""Club"":""8"",""Persona"":""2"",""Crush"":""99"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""38"",""Color"":""Science"",""Eyes"":""Science"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""The most intelligent - and eccentric - student at Akademi. Possesses a genius-level IQ, but is prone to extremely bizarre behavior, and frequently expresses thoughts that frighten and concern the people around him.""},
	{""ID"":""62"",""Name"":""Horo Guramu"",""Gender"":""1"",""Class"":""31"",""Seat"":""8"",""Club"":""8"",""Persona"":""2"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""39"",""Color"":""Science"",""Eyes"":""Science"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A brilliant young man who made international news after making incredible contributions to the field of holographic technology. Even now, he continues to refine his work and bring science-fiction closer to reality.""},
	{""ID"":""63"",""Name"":""Yaku Zaishi"",""Gender"":""1"",""Class"":""21"",""Seat"":""8"",""Club"":""8"",""Persona"":""2"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""40"",""Color"":""Science"",""Eyes"":""Science"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.0008_13.375_15.5008_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A young chemist who is obsessed with combining various chemicals to observe the results. Vague and evasive whenever he is questioned about his intentions. His head-mounted device is capable of detecting and displaying his current emotions.""},
	{""ID"":""64"",""Name"":""Meka Nikaru"",""Gender"":""0"",""Class"":""12"",""Seat"":""8"",""Club"":""8"",""Persona"":""2"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""36"",""Color"":""Science"",""Eyes"":""Science"",""EyeType"":""Serious"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A young roboticist whose lifelong goal is to build a giant bipedal robot. Known for an overbearing enthusiasm for mecha anime. She has already built several tiny bipedal robots, but each one is always larger and more impressive than the previous one.""},
	{""ID"":""65"",""Name"":""Homu Kurusu"",""Gender"":""0"",""Class"":""11"",""Seat"":""8"",""Club"":""8"",""Persona"":""2"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""0"",""Hairstyle"":""37"",""Color"":""Science"",""Eyes"":""Science"",""EyeType"":""Serious"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A cold and emotionless young woman. Building a robotic replica of herself in the Science Club. Some presume that she is motivated by narcissism, but others theorize that she is attempting to build a replica of her deceased twin sister.""},
	{""ID"":""66"",""Name"":""Itachi Zametora"",""Gender"":""1"",""Class"":""32"",""Seat"":""9"",""Club"":""9"",""Persona"":""3"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""5"",""Hairstyle"":""41"",""Color"":""Sports"",""Eyes"":""Sports"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""14"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A gruff and unfriendly student. Extremely competitive; never turns down a challenge. One of the most physically fit male students at school. Never misses an opportunity to demonstrate his impressive physical strength.""},
	{""ID"":""67"",""Name"":""Hojiro Zameshiro"",""Gender"":""1"",""Class"":""31"",""Seat"":""9"",""Club"":""9"",""Persona"":""3"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""4"",""Hairstyle"":""42"",""Color"":""Sports"",""Eyes"":""Sports"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A diligent student. Has known Itachi since childhood, and views him as a rival, even though Itachi usually ignores him. Constantly strives to better himself so that one day he will finally be able to surprass his long-time rival.""},
	{""ID"":""68"",""Name"":""Unagi Denkashiza"",""Gender"":""1"",""Class"":""21"",""Seat"":""9"",""Club"":""9"",""Persona"":""3"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""3"",""Hairstyle"":""43"",""Color"":""Sports"",""Eyes"":""Sports"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.0009_13.375_15.5009_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A former delinquent. Rumored to have a history of violent behavior. Although he hasn't caused any trouble at Akademi High, most people are afraid to speak with him because of the rumors, so he has difficulty making new friends.""},
	{""ID"":""69"",""Name"":""Iruka Dorufino"",""Gender"":""1"",""Class"":""12"",""Seat"":""9"",""Club"":""9"",""Persona"":""3"",""Crush"":""99"",""BreastSize"":""0"",""Strength"":""2"",""Hairstyle"":""44"",""Color"":""Sports"",""Eyes"":""Sports"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A very quiet student who rarely speaks. Always wants to be in the gym, the track, or the pool; takes club activities very seriously. There's a rumor that he joined the Sports Club because he wanted to impress someone he has a crush on.""},
	{""ID"":""70"",""Name"":""Mantaro Sashimasu"",""Gender"":""1"",""Class"":""11"",""Seat"":""9"",""Club"":""9"",""Persona"":""3"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""1"",""Hairstyle"":""45"",""Color"":""Sports"",""Eyes"":""Sports"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Club_Seat_LunchSpot_Seat_Clean_Club"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A friendly student. He's not as strong or fast as his fellow clubmembers, so he usually gets left behind. However, despite all of that, he never gets discouraged, and he never gives up - no matter what. Views Budo Masuta as a role model.""},
	{""ID"":""71"",""Name"":""Uekiya Engeika"",""Gender"":""0"",""Class"":""32"",""Seat"":""10"",""Club"":""10"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""0"",""Hairstyle"":""38"",""Color"":""Gardening"",""Eyes"":""Gardening"",""EyeType"":""Gentle"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""The president of the Gardening Club. Known throughout the school for her kind and caring nature, which has earned her the nickname of ''Everyone's Big Sister''.""},
	{""ID"":""72"",""Name"":""Himari Fujita"",""Gender"":""0"",""Class"":""31"",""Seat"":""10"",""Club"":""10"",""Persona"":""3"",""Crush"":""0"",""BreastSize"":""1.4"",""Strength"":""0"",""Hairstyle"":""39"",""Color"":""Gardening"",""Eyes"":""Gardening"",""EyeType"":""Gentle"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A bright and cheery girl known for her undying loyalty to her friends. Will fiercely support her friends when they're in trouble, sometimes in an overbearing way.""},
	{""ID"":""73"",""Name"":""Sakura Hagiwara"",""Gender"":""0"",""Class"":""21"",""Seat"":""10"",""Club"":""10"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1.3"",""Strength"":""0"",""Hairstyle"":""40"",""Color"":""Gardening"",""Eyes"":""Gardening"",""EyeType"":""Gentle"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.001_13.375_15.501_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""Lives with an incurable heart disease, and will most likely die before her 30s. She prefers to live out her days in peace and quiet, and finds comfort in flowers.""},
	{""ID"":""74"",""Name"":""Sumire Suzuki"",""Gender"":""0"",""Class"":""12"",""Seat"":""10"",""Club"":""10"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1.2"",""Strength"":""0"",""Hairstyle"":""41"",""Color"":""Gardening"",""Eyes"":""Gardening"",""EyeType"":""Gentle"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A pure-minded girl who is as innocent as possible. Doesn't understand double-entendres, which provides her friends with an endless source of amusement.""},
	{""ID"":""75"",""Name"":""Tsubaki Uesugi"",""Gender"":""0"",""Class"":""11"",""Seat"":""10"",""Club"":""10"",""Persona"":""6"",""Crush"":""0"",""BreastSize"":""1.1"",""Strength"":""0"",""Hairstyle"":""42"",""Color"":""Gardening"",""Eyes"":""Gardening"",""EyeType"":""Gentle"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Patrol_Seat_LunchSpot_Seat_Clean_Patrol"",""ScheduleAction"":""Stand_Stand_Club_Sit_Eat_Sit_Clean_Club"",""Info"":""A very spiritual girl who has a deep fascination with Japan's history, especially the Shinto religion. Believes in the existance of ''Kami'', ancient Japanese gods.""},
	{""ID"":""76"",""Name"":""Umeji Kizuguchi"",""Gender"":""1"",""Class"":""31"",""Seat"":""15"",""Club"":""12"",""Persona"":""16"",""Crush"":""99"",""BreastSize"":""0"",""Strength"":""8"",""Hairstyle"":""27"",""Color"":""Delinquent1"",""Eyes"":""Delinquent1"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""5"",""ScheduleTime"":""7_7_8.5_13_13.5_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Hangout_Hangout"",""ScheduleAction"":""Stand_Stand_Socialize_Sit_Eat_Sit_Socialize_Socialize"",""Info"":""Osoro Shidesu, the leader of the delinquent gang, is absent from school. This student is Osoro's right-hand-man, and has been trusted with leading the delinquents while Osoro is absent.""},
	{""ID"":""77"",""Name"":""Hokuto Furukizu"",""Gender"":""1"",""Class"":""22"",""Seat"":""15"",""Club"":""12"",""Persona"":""16"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""8"",""Hairstyle"":""28"",""Color"":""Delinquent2"",""Eyes"":""Delinquent2"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""6"",""ScheduleTime"":""7_7_8.5_13_13.5_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Hangout_Hangout"",""ScheduleAction"":""Stand_Stand_Socialize_Sit_Eat_Sit_Socialize_Socialize"",""Info"":""Carries a weapon in the bag on his back. \n Will not hesitate to defend himself if he feels threatened. \n Spends most of his time in the incinerator area behind the school with his fellow delinquents.""},
	{""ID"":""78"",""Name"":""Gaku Hikitsuri"",""Gender"":""1"",""Class"":""21"",""Seat"":""15"",""Club"":""12"",""Persona"":""16"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""8"",""Hairstyle"":""29"",""Color"":""Delinquent3"",""Eyes"":""Delinquent3"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""7"",""ScheduleTime"":""7_7_8.5_13.0011_13.5_15.5011_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Hangout_Hangout"",""ScheduleAction"":""Stand_Stand_Socialize_Sit_Eat_Sit_Socialize_Socialize"",""Info"":""Carries a weapon in the bag on his back. \n Will not hesitate to defend himself if he feels threatened. \n Spends most of his time in the incinerator area behind the school with his fellow delinquents.""},
	{""ID"":""79"",""Name"":""Hayanari Tsumeato"",""Gender"":""1"",""Class"":""12"",""Seat"":""15"",""Club"":""12"",""Persona"":""16"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""8"",""Hairstyle"":""30"",""Color"":""Delinquent4"",""Eyes"":""Delinquent4"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""8"",""ScheduleTime"":""7_7_8.5_13_13.5_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Hangout_Hangout"",""ScheduleAction"":""Stand_Stand_Socialize_Sit_Eat_Sit_Socialize_Socialize"",""Info"":""Carries a weapon in the bag on his back. \n Will not hesitate to defend himself if he feels threatened. \n Spends most of his time in the incinerator area behind the school with his fellow delinquents.""},
	{""ID"":""80"",""Name"":""Dairoku Surikizu"",""Gender"":""1"",""Class"":""11"",""Seat"":""15"",""Club"":""12"",""Persona"":""16"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""8"",""Hairstyle"":""31"",""Color"":""Delinquent5"",""Eyes"":""Delinquent5"",""EyeType"":""Male"",""Stockings"":""None"",""Accessory"":""9"",""ScheduleTime"":""7_7_8.5_13_13.5_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Hangout_Seat_LunchSpot_Seat_Hangout_Hangout"",""ScheduleAction"":""Stand_Stand_Socialize_Sit_Eat_Sit_Socialize_Socialize"",""Info"":""Carries a weapon in the bag on his back. \n Will not hesitate to defend himself if he feels threatened. \n Spends most of his time in the incinerator area behind the school with his fellow delinquents.""},
	{""ID"":""81"",""Name"":""Musume Ronshaku"",""Gender"":""0"",""Class"":""32"",""Seat"":""14"",""Club"":""14"",""Persona"":""10"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""0"",""Hairstyle"":""26"",""Color"":""Ganguro1"",""Eyes"":""Ganguro1"",""EyeType"":""Smug"",""Stockings"":""Loose"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Graffiti_Seat_Bully_Seat_Hangout_LunchSpot"",""ScheduleAction"":""Stand_Stand_Graffiti_Sit_Bully_Sit_Gossip_Wait"",""Info"":""A flashy trend-setter who loves to gossip and doesn't take school too seriously. \n \n She is spoiled rotten by her doting father, who buys his daughter anything she wants. \n \n Her father runs a loan agency.""},
	{""ID"":""82"",""Name"":""Kashiko Murasaki"",""Gender"":""0"",""Class"":""31"",""Seat"":""14"",""Club"":""14"",""Persona"":""10"",""Crush"":""0"",""BreastSize"":""1.4"",""Strength"":""0"",""Hairstyle"":""27"",""Color"":""Ganguro2"",""Eyes"":""Ganguro2"",""EyeType"":""Smug"",""Stockings"":""Loose"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Graffiti_Seat_Bully_Seat_Hangout_Patrol"",""ScheduleAction"":""Stand_Stand_Graffiti_Sit_Bully_Sit_Gossip_Patrol"",""Info"":""Spends most of her time texting on her phone. \n \n Pretends to be a sweet girl, but in private, her personality is quite nasty. \n \n Secretly, her favorite activity is gossipping and spreading rumors.""},
	{""ID"":""83"",""Name"":""Hana Daidaiyama"",""Gender"":""0"",""Class"":""21"",""Seat"":""14"",""Club"":""14"",""Persona"":""10"",""Crush"":""0"",""BreastSize"":""1.3"",""Strength"":""0"",""Hairstyle"":""28"",""Color"":""Ganguro3"",""Eyes"":""Ganguro3"",""EyeType"":""Smug"",""Stockings"":""Loose"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13.0012_13.375_15.5012_16_99"",""ScheduleDestination"":""Spawn_Locker_Graffiti_Seat_Bully_Seat_Hangout_Patrol"",""ScheduleAction"":""Stand_Stand_Graffiti_Sit_Bully_Sit_Gossip_Patrol"",""Info"":""Spends most of her time playing games on her phone. \n \n Pretends to be pure and innocent, but in private, she can be extremely vulgar. \n \n Secretly, her favorite activity is looking for dirt in peoples' pasts.""},
	{""ID"":""84"",""Name"":""Kokoro Momoiro"",""Gender"":""0"",""Class"":""12"",""Seat"":""14"",""Club"":""14"",""Persona"":""10"",""Crush"":""0"",""BreastSize"":""1.2"",""Strength"":""0"",""Hairstyle"":""29"",""Color"":""Ganguro4"",""Eyes"":""Ganguro4"",""EyeType"":""Smug"",""Stockings"":""Loose"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Graffiti_Seat_Bully_Seat_Hangout_Patrol"",""ScheduleAction"":""Stand_Stand_Graffiti_Sit_Bully_Sit_Gossip_Patrol"",""Info"":""Spends most of her time taking selfies with her phone. \n \n Pretends to oppose bullying, but in private, she harasses the people she dislikes. \n \n Secretly, her favorite activity is shaming and ridiculing other people.""},
	{""ID"":""85"",""Name"":""Hoshiko Mizudori"",""Gender"":""0"",""Class"":""11"",""Seat"":""14"",""Club"":""14"",""Persona"":""10"",""Crush"":""0"",""BreastSize"":""1.1"",""Strength"":""0"",""Hairstyle"":""30"",""Color"":""Ganguro5"",""Eyes"":""Ganguro5"",""EyeType"":""Smug"",""Stockings"":""Loose"",""Accessory"":""0"",""ScheduleTime"":""7_7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Locker_Graffiti_Seat_Bully_Seat_Hangout_Patrol"",""ScheduleAction"":""Stand_Stand_Graffiti_Sit_Bully_Sit_Gossip_Patrol"",""Info"":""Spends most of her time browsing the Internet on her phone. \n \n Pretends to be a positive person, but in private, only says negative things about others. \n \n Secretly, her favorite activity is operating a hate blog on the Internet.""},
	{""ID"":""86"",""Name"":""Kuroko Kamenaga"",""Gender"":""0"",""Class"":""32"",""Seat"":""13"",""Club"":""13"",""Persona"":""8"",""Crush"":""0"",""BreastSize"":""1.25"",""Strength"":""7"",""Hairstyle"":""22"",""Color"":""Council1"",""Eyes"":""Council1"",""EyeType"":""Council1"",""Stockings"":""Council1"",""Accessory"":""8"",""ScheduleTime"":""7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Patrol_Seat_Patrol_Seat_Patrol_Hangout"",""ScheduleAction"":""Stand_Patrol_Sit_Patrol_Sit_Patrol_Relax"",""Info"":""Last year's student council president, and this year's vice president. Currently substituting for the absent president. Known for being formal at all times.""},
	{""ID"":""87"",""Name"":""Shiromi Torayoshi"",""Gender"":""0"",""Class"":""11"",""Seat"":""13"",""Club"":""13"",""Persona"":""8"",""Crush"":""0"",""BreastSize"":""0.75"",""Strength"":""7"",""Hairstyle"":""23"",""Color"":""Council2"",""Eyes"":""Council2"",""EyeType"":""Council2"",""Stockings"":""Council2"",""Accessory"":""9"",""ScheduleTime"":""7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Patrol_Seat_Patrol_Seat_Patrol_Hangout"",""ScheduleAction"":""Stand_Patrol_Sit_Patrol_Sit_Patrol_Relax"",""Info"":""Treasurer of the student council. A mysterious & enigmatic young woman. Known to be calm & relaxed at all times. Some students find her unsettling.""},
	{""ID"":""88"",""Name"":""Akane Toriyasu"",""Gender"":""0"",""Class"":""31"",""Seat"":""13"",""Club"":""13"",""Persona"":""8"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""7"",""Hairstyle"":""24"",""Color"":""Council3"",""Eyes"":""Council3"",""EyeType"":""Council3"",""Stockings"":""Council3"",""Accessory"":""10"",""ScheduleTime"":""7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Patrol_Seat_Patrol_Seat_Patrol_Hangout"",""ScheduleAction"":""Stand_Patrol_Sit_Patrol_Sit_Patrol_Relax"",""Info"":""Secretary of the student council. Seems ditzy and airheaded at first glance, but has never failed in her duties. She has a very large number of male admirers.""},
	{""ID"":""89"",""Name"":""Aoi Ryugoku"",""Gender"":""0"",""Class"":""22"",""Seat"":""13"",""Club"":""13"",""Persona"":""8"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""7"",""Hairstyle"":""25"",""Color"":""Council4"",""Eyes"":""Council4"",""EyeType"":""Council4"",""Stockings"":""Council4"",""Accessory"":""11"",""ScheduleTime"":""7_8_13_13.375_15.5_16_99"",""ScheduleDestination"":""Spawn_Patrol_Seat_Patrol_Seat_Patrol_Hangout"",""ScheduleAction"":""Stand_Patrol_Sit_Patrol_Sit_Patrol_Relax"",""Info"":""Enforcer of the student council. Charged with the task of maintaining peace throughout the school. Often uses physical intimidation to enforce rules.""},
	{""ID"":""90"",""Name"":""Nasu Kankoshi"",""Gender"":""0"",""Class"":""1"",""Seat"":""0"",""Club"":""102"",""Persona"":""9"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""6"",""Hairstyle"":""8"",""Color"":""Nurse"",""Eyes"":""Nurse"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""8"",""ScheduleTime"":""0_0_0_0_0"",""ScheduleDestination"":""Patrol_Patrol_Patrol_Patrol_Patrol"",""ScheduleAction"":""Patrol_Patrol_Patrol_Patrol_Patrol"",""Info"":""""},
	{""ID"":""91"",""Name"":""Reina Nana"",""Gender"":""0"",""Class"":""11"",""Seat"":""0"",""Club"":""100"",""Persona"":""9"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""6"",""Hairstyle"":""1"",""Color"":""Teacher"",""Eyes"":""Teacher"",""EyeType"":""Thin"",""Stockings"":""None"",""Accessory"":""1"",""ScheduleTime"":""7_8.25_13_13.375_15.5_99"",""ScheduleDestination"":""Spawn_Seat_Podium_Seat_Podium_Seat"",""ScheduleAction"":""Stand_Grade_Teach_Grade_Teach_Grade"",""Info"":""""},
	{""ID"":""92"",""Name"":""Natsuki Anna"",""Gender"":""0"",""Class"":""12"",""Seat"":""0"",""Club"":""100"",""Persona"":""9"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""6"",""Hairstyle"":""2"",""Color"":""Teacher"",""Eyes"":""Teacher"",""EyeType"":""Thin"",""Stockings"":""None"",""Accessory"":""2"",""ScheduleTime"":""7_8.25_13_13.375_15.5_99"",""ScheduleDestination"":""Spawn_Seat_Podium_Seat_Podium_Seat"",""ScheduleAction"":""Stand_Grade_Teach_Grade_Teach_Grade"",""Info"":""""},
	{""ID"":""93"",""Name"":""Rino Fuka"",""Gender"":""0"",""Class"":""21"",""Seat"":""0"",""Club"":""100"",""Persona"":""9"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""6"",""Hairstyle"":""3"",""Color"":""Teacher"",""Eyes"":""Teacher"",""EyeType"":""Thin"",""Stockings"":""None"",""Accessory"":""3"",""ScheduleTime"":""7_8.25_13_13.375_15.5_99"",""ScheduleDestination"":""Spawn_Seat_Podium_Seat_Podium_Seat"",""ScheduleAction"":""Stand_Grade_Teach_Grade_Teach_Grade"",""Info"":""""},
	{""ID"":""94"",""Name"":""Shiori Risa"",""Gender"":""0"",""Class"":""22"",""Seat"":""0"",""Club"":""100"",""Persona"":""9"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""6"",""Hairstyle"":""4"",""Color"":""Teacher"",""Eyes"":""Teacher"",""EyeType"":""Thin"",""Stockings"":""None"",""Accessory"":""4"",""ScheduleTime"":""7_8.25_13_13.375_15.5_99"",""ScheduleDestination"":""Spawn_Seat_Podium_Seat_Podium_Seat"",""ScheduleAction"":""Stand_Grade_Teach_Grade_Teach_Grade"",""Info"":""""},
	{""ID"":""95"",""Name"":""Karin Hana"",""Gender"":""0"",""Class"":""31"",""Seat"":""0"",""Club"":""100"",""Persona"":""9"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""6"",""Hairstyle"":""5"",""Color"":""Teacher"",""Eyes"":""Teacher"",""EyeType"":""Thin"",""Stockings"":""None"",""Accessory"":""5"",""ScheduleTime"":""7_8.25_13_13.375_15.5_99"",""ScheduleDestination"":""Spawn_Seat_Podium_Seat_Podium_Seat"",""ScheduleAction"":""Stand_Grade_Teach_Grade_Teach_Grade"",""Info"":""""},
	{""ID"":""96"",""Name"":""Kaho Kanon"",""Gender"":""0"",""Class"":""32"",""Seat"":""0"",""Club"":""100"",""Persona"":""9"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""6"",""Hairstyle"":""6"",""Color"":""Teacher"",""Eyes"":""Teacher"",""EyeType"":""Thin"",""Stockings"":""None"",""Accessory"":""6"",""ScheduleTime"":""7_8.25_13_13.375_15.5_99"",""ScheduleDestination"":""Spawn_Seat_Podium_Seat_Podium_Seat"",""ScheduleAction"":""Stand_Grade_Teach_Grade_Teach_Grade"",""Info"":""""},
	{""ID"":""97"",""Name"":""Kyoshi Taiso"",""Gender"":""0"",""Class"":""0"",""Seat"":""0"",""Club"":""101"",""Persona"":""9"",""Crush"":""0"",""BreastSize"":""1.5"",""Strength"":""6"",""Hairstyle"":""7"",""Color"":""Coach"",""Eyes"":""Coach"",""EyeType"":""Default"",""Stockings"":""None"",""Accessory"":""7"",""ScheduleTime"":""7_8_13_13.375_15.5_99"",""ScheduleDestination"":""Spawn_Podium_Patrol_Seat_Patrol_Podium"",""ScheduleAction"":""Stand_Stand_Patrol_Grade_Patrol_Stand"",""Info"":""""},
	{""ID"":""98"",""Name"":""Genka Kunahito"",""Gender"":""0"",""Class"":""2"",""Seat"":""0"",""Club"":""100"",""Persona"":""9"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""6"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""N/A"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""0"",""ScheduleDestination"":""Nothing"",""ScheduleAction"":""Nothing"",""Info"":""The school's guidance counselor. \n \n Misbehaving students are sent to her office. \n \n To expel a student, you must catch that student misbehaving and report them to her.""},
	{""ID"":""99"",""Name"":""Kocho Shuyona"",""Gender"":""1"",""Class"":""3"",""Seat"":""0"",""Club"":""100"",""Persona"":""9"",""Crush"":""0"",""BreastSize"":""0"",""Strength"":""99"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""N/A"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""0"",""ScheduleDestination"":""Nothing"",""ScheduleAction"":""Nothing"",""Info"":""The school's headmaster. \n \n An extremely reclusive person, he rarely leaves his office or addresses the student body in person. \n \n The center of numerous unsavory rumors, although none have been proven.""},
	{""ID"":""100"",""Name"":""Info-chan"",""Gender"":""0"",""Class"":""0"",""Seat"":""0"",""Club"":""99"",""Persona"":""99"",""Crush"":""0"",""BreastSize"":""1"",""Strength"":""99"",""Hairstyle"":""1"",""Color"":""White"",""Eyes"":""Black"",""EyeType"":""N/A"",""Stockings"":""None"",""Accessory"":""0"",""ScheduleTime"":""0"",""ScheduleDestination"":""Nothing"",""ScheduleAction"":""Nothing"",""Info"":""Trying to look up my information? Don't bother. There is nothing that you need to know about me. You're a client, and I'm a provider. That's all we need to know about each other.""}
]";
		using (WWW reader = new WWW(path))
			{
				while (!reader.isDone) { }

				GameObject sector22 = GameObject.Find("sector22");
				Text sector22Text = sector22.GetComponent<Text>();
					
				if (string.IsNullOrEmpty(reader.error))
				{
					Debug.Log("JSON read sector {!UNUSED} <" + reader.text.Length + ">");
					if (reader.text.Length > 10) sector22Text.text = "JSON read sector {!UNUSED} <" + reader.text.Length + "> *isvalid";
					else sector22Text.text = "JSON read sector <" + reader.text.Length + "> *isinvalid";
				}
				else
				{
					sector22Text.text = "JSON read error <" + reader.error + ">";
					Debug.LogError("JSON read error <" + reader.error + ">");
				}
			}

		StudentJson[] array = new StudentJson[101];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new StudentJson();
		}
		GameObject loggerr = GameObject.Find("loggerr");
		Text loggerrText = loggerr.GetComponent<Text>();
		try {
			
			// This line deserializes the JSON data from the string into an array of dictionaries.
			Dictionary<string, object>[] array2 = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(jsondata);

			loggerrText.text = "<" + "SO FAR __OK__" + ">";

			foreach (Dictionary<string, object> dictionary in array2)
			{
				int num = TFUtils.LoadInt(dictionary, "ID");

				if (num == 0) break;

				StudentJson studentJson = array[num];
				studentJson.name = TFUtils.LoadString(dictionary, "Name");
				studentJson.gender = TFUtils.LoadInt(dictionary, "Gender");
				studentJson.classID = TFUtils.LoadInt(dictionary, "Class");
				studentJson.seat = TFUtils.LoadInt(dictionary, "Seat");
				studentJson.club = (ClubType)TFUtils.LoadInt(dictionary, "Club");
				studentJson.persona = (PersonaType)TFUtils.LoadInt(dictionary, "Persona");
				studentJson.crush = TFUtils.LoadInt(dictionary, "Crush");
				studentJson.breastSize = TFUtils.LoadFloat(dictionary, "BreastSize");
				studentJson.strength = TFUtils.LoadInt(dictionary, "Strength");
				studentJson.hairstyle = TFUtils.LoadString(dictionary, "Hairstyle");
				studentJson.color = TFUtils.LoadString(dictionary, "Color");
				studentJson.eyes = TFUtils.LoadString(dictionary, "Eyes");
				studentJson.eyeType = TFUtils.LoadString(dictionary, "EyeType");
				studentJson.stockings = TFUtils.LoadString(dictionary, "Stockings");
				studentJson.accessory = TFUtils.LoadString(dictionary, "Accessory");
				studentJson.info = TFUtils.LoadString(dictionary, "Info");

				if (GameGlobals.LoveSick && studentJson.name == "Mai Waifu") studentJson.name = "Mai Wakabayashi";
				if (OptionGlobals.HighPopulation && studentJson.name == "Unknown") studentJson.name = "Random";

				float[] array3 = ConstructTempFloatArray(TFUtils.LoadString(dictionary, "ScheduleTime"));
				string[] array4 = ConstructTempStringArray(TFUtils.LoadString(dictionary, "ScheduleDestination"));
				string[] array5 = ConstructTempStringArray(TFUtils.LoadString(dictionary, "ScheduleAction"));

				studentJson.scheduleBlocks = new ScheduleBlock[array3.Length];

				for (int k = 0; k < studentJson.scheduleBlocks.Length; k++) studentJson.scheduleBlocks[k] = new ScheduleBlock(array3[k], array4[k], array5[k]);

				studentJson.success = true;
			}
		} catch (Exception e) {
			loggerrText.text = "<" + e.Message + ">";
			Debug.LogError("JSON <" + e.Message + ">");
		}
		
		for (int i = 10; i <= 20; i++)
		{
			array[i].name = "Reserved";
		}
		return array;
	}

	private static float[] ConstructTempFloatArray(string str)
	{
		string[] array = str.Split('_');
		float[] array2 = new float[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = float.Parse(array[i]);
		}
		return array2;
	}

	private static string[] ConstructTempStringArray(string str)
	{
		return str.Split('_');
	}
}
