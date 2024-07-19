using UnityEngine;

public class WeaponManagerScript : MonoBehaviour
{
	public WeaponScript[] DelinquentWeapons;

	public WeaponScript[] Weapons;

	public JsonScript JSON;

	public int[] Victims;

	public int MisplacedWeapons;

	public int MurderWeapons;

	public int Fingerprints;

	public Texture Flower;

	public Texture Blood;

	public bool YandereGuilty;

	public void Start()
	{
		for (int i = 0; i < Weapons.Length; i++)
		{
			Weapons[i].GlobalID = i;
			if (WeaponGlobals.GetWeaponStatus(i) == 1)
			{
				Weapons[i].gameObject.SetActive(false);
			}
		}
	}

	public void UpdateLabels()
	{
		WeaponScript[] weapons = Weapons;
		foreach (WeaponScript weaponScript in weapons)
		{
			weaponScript.UpdateLabel();
		}
	}

	public void CheckWeapons()
	{
		MurderWeapons = 0;
		Fingerprints = 0;
		for (int i = 0; i < Victims.Length; i++)
		{
			Victims[i] = 0;
		}
		WeaponScript[] weapons = Weapons;
		foreach (WeaponScript weaponScript in weapons)
		{
			if (!(weaponScript != null) || !weaponScript.Blood.enabled || weaponScript.AlreadyExamined)
			{
				continue;
			}
			MurderWeapons++;
			if (weaponScript.FingerprintID <= 0)
			{
				continue;
			}
			Fingerprints++;
			for (int k = 0; k < weaponScript.Victims.Length; k++)
			{
				if (weaponScript.Victims[k])
				{
					Victims[k] = weaponScript.FingerprintID;
				}
			}
		}
	}

	public void CleanWeapons()
	{
		WeaponScript[] weapons = Weapons;
		foreach (WeaponScript weaponScript in weapons)
		{
			if (weaponScript != null)
			{
				weaponScript.Blood.enabled = false;
				weaponScript.FingerprintID = 0;
			}
		}
	}

	public void ChangeBloodTexture()
	{
		WeaponScript[] weapons = Weapons;
		foreach (WeaponScript weaponScript in weapons)
		{
			if (weaponScript != null)
			{
				if (!GameGlobals.CensorBlood)
				{
					weaponScript.Blood.material.mainTexture = Blood;
					weaponScript.Blood.material.SetColor("_TintColor", new Color(0.25f, 0.25f, 0.25f, 0.5f));
				}
				else
				{
					weaponScript.Blood.material.mainTexture = Flower;
					weaponScript.Blood.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));
				}
			}
		}
	}

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.Z))
		{
			return;
		}
		CheckWeapons();
		for (int i = 0; i < Victims.Length; i++)
		{
			if (Victims[i] != 0)
			{
				if (Victims[i] == 100)
				{
					Debug.Log("The student named " + JSON.Students[i].Name + " was killed by Yandere-chan!");
					continue;
				}
				Debug.Log("The student named " + JSON.Students[i].Name + " was killed by " + JSON.Students[Victims[i]].Name + "!");
			}
		}
	}

	public void TrackDumpedWeapons()
	{
		for (int i = 0; i < Weapons.Length; i++)
		{
			if (Weapons[i] == null)
			{
				Debug.Log("Weapon #" + i + " was destroyed! Setting status to 1!");
			}
		}
	}
}
