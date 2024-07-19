using System;

[Serializable]
public class EventSaveData
{
	public bool befriendConversation;

	public bool event1;

	public bool event2;

	public bool kidnapConversation;

	public bool livingRoom;

	public static EventSaveData ReadFromGlobals()
	{
		EventSaveData eventSaveData = new EventSaveData();
		eventSaveData.befriendConversation = EventGlobals.BefriendConversation;
		eventSaveData.event1 = EventGlobals.Event1;
		eventSaveData.event2 = EventGlobals.Event2;
		eventSaveData.kidnapConversation = EventGlobals.KidnapConversation;
		eventSaveData.livingRoom = EventGlobals.LivingRoom;
		return eventSaveData;
	}

	public static void WriteToGlobals(EventSaveData data)
	{
		EventGlobals.BefriendConversation = data.befriendConversation;
		EventGlobals.Event1 = data.event1;
		EventGlobals.Event2 = data.event2;
		EventGlobals.KidnapConversation = data.kidnapConversation;
		EventGlobals.LivingRoom = data.livingRoom;
	}
}
