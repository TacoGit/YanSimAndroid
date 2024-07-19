using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
	public PromptScript Prompt;

	public string Name = string.Empty;

	public int ID;

	public CollectibleType CollectibleType
	{
		get
		{
			if (Name == "HeadmasterTape")
			{
				return CollectibleType.HeadmasterTape;
			}
			if (Name == "BasementTape")
			{
				return CollectibleType.BasementTape;
			}
			if (Name == "Manga")
			{
				return CollectibleType.Manga;
			}
			if (Name == "Tape")
			{
				return CollectibleType.Tape;
			}
			Debug.LogError("Unrecognized collectible \"" + Name + "\".", base.gameObject);
			return CollectibleType.Tape;
		}
	}

	private void Start()
	{
		if ((CollectibleType == CollectibleType.BasementTape && CollectibleGlobals.GetBasementTapeCollected(ID)) || (CollectibleType == CollectibleType.Manga && CollectibleGlobals.GetMangaCollected(ID)) || (CollectibleType == CollectibleType.Tape && CollectibleGlobals.GetTapeCollected(ID)))
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			if (CollectibleType == CollectibleType.HeadmasterTape)
			{
				CollectibleGlobals.SetHeadmasterTapeCollected(ID, true);
			}
			else if (CollectibleType == CollectibleType.BasementTape)
			{
				CollectibleGlobals.SetBasementTapeCollected(ID, true);
			}
			else if (CollectibleType == CollectibleType.Manga)
			{
				CollectibleGlobals.SetMangaCollected(ID, true);
			}
			else if (CollectibleType == CollectibleType.Tape)
			{
				CollectibleGlobals.SetTapeCollected(ID, true);
			}
			else
			{
				Debug.LogError("Collectible \"" + Name + "\" not implemented.", base.gameObject);
			}
			Object.Destroy(base.gameObject);
		}
	}
}
