using UnityEngine;

public class TranqDetectorScript : MonoBehaviour
{
	public YandereScript Yandere;

	public DoorScript Door;

	public UIPanel Checklist;

	public Collider MyCollider;

	public UILabel KidnappingLabel;

	public UISprite TranquilizerIcon;

	public UISprite FollowerIcon;

	public UISprite BiologyIcon;

	public UISprite SyringeIcon;

	public UISprite DoorIcon;

	public bool StopChecking;

	public AudioClip[] TranqClips;

	private void Start()
	{
		Checklist.alpha = 0f;
	}

	private void Update()
	{
		if (!StopChecking)
		{
			if (MyCollider.bounds.Contains(Yandere.transform.position))
			{
				if (SchoolGlobals.KidnapVictim > 0)
				{
					KidnappingLabel.text = "There is no room for another prisoner in your basement.";
				}
				else
				{
					if (Yandere.Inventory.Tranquilizer || Yandere.Inventory.Sedative)
					{
						TranquilizerIcon.spriteName = "Yes";
					}
					else
					{
						TranquilizerIcon.spriteName = "No";
					}
					if (Yandere.Followers != 1)
					{
						FollowerIcon.spriteName = "No";
					}
					else if (Yandere.Follower.Male)
					{
						KidnappingLabel.text = "You cannot kidnap male students at this point in time.";
						FollowerIcon.spriteName = "No";
					}
					else
					{
						KidnappingLabel.text = "Kidnapping Checklist";
						FollowerIcon.spriteName = "Yes";
					}
					BiologyIcon.spriteName = ((ClassGlobals.BiologyGrade + ClassGlobals.BiologyBonus == 0) ? "No" : "Yes");
					if (!Yandere.Armed)
					{
						SyringeIcon.spriteName = "No";
					}
					else if (Yandere.EquippedWeapon.WeaponID != 3)
					{
						SyringeIcon.spriteName = "No";
					}
					else
					{
						SyringeIcon.spriteName = "Yes";
					}
					if (Door.Open || Door.Timer < 1f)
					{
						DoorIcon.spriteName = "No";
					}
					else
					{
						DoorIcon.spriteName = "Yes";
					}
				}
				Checklist.alpha = Mathf.MoveTowards(Checklist.alpha, 1f, Time.deltaTime);
			}
			else
			{
				Checklist.alpha = Mathf.MoveTowards(Checklist.alpha, 0f, Time.deltaTime);
			}
		}
		else
		{
			Checklist.alpha = Mathf.MoveTowards(Checklist.alpha, 0f, Time.deltaTime);
			if (Checklist.alpha == 0f)
			{
				base.enabled = false;
			}
		}
	}

	public void TranqCheck()
	{
		if (!StopChecking && KidnappingLabel.text == "Kidnapping Checklist" && TranquilizerIcon.spriteName == "Yes" && FollowerIcon.spriteName == "Yes" && BiologyIcon.spriteName == "Yes" && SyringeIcon.spriteName == "Yes" && DoorIcon.spriteName == "Yes")
		{
			AudioSource component = GetComponent<AudioSource>();
			component.clip = TranqClips[Random.Range(0, TranqClips.Length)];
			component.Play();
			Door.Prompt.Hide();
			Door.Prompt.enabled = false;
			Door.enabled = false;
			Yandere.Inventory.Tranquilizer = false;
			if (!Yandere.Follower.Male)
			{
				Yandere.CanTranq = true;
			}
			Yandere.EquippedWeapon.Type = WeaponType.Syringe;
			Yandere.AttackManager.Stealth = true;
			StopChecking = true;
		}
	}
}
