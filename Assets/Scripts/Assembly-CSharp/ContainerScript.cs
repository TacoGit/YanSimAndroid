using UnityEngine;

public class ContainerScript : MonoBehaviour
{
	public Transform[] BodyPartPositions;

	public Transform WeaponSpot;

	public Transform Lid;

	public Collider GardenArea;

	public Collider NEStairs;

	public Collider NWStairs;

	public Collider SEStairs;

	public Collider SWStairs;

	public PickUpScript[] BodyParts;

	public PickUpScript BodyPart;

	public WeaponScript Weapon;

	public PromptScript Prompt;

	public string SpriteName = string.Empty;

	public bool CanDrop;

	public bool Open;

	public int Contents;

	public int ID;

	public void Start()
	{
		GardenArea = GameObject.Find("GardenArea").GetComponent<Collider>();
		NEStairs = GameObject.Find("NEStairs").GetComponent<Collider>();
		NWStairs = GameObject.Find("NWStairs").GetComponent<Collider>();
		SEStairs = GameObject.Find("SEStairs").GetComponent<Collider>();
		SWStairs = GameObject.Find("SWStairs").GetComponent<Collider>();
	}

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			Open = !Open;
			UpdatePrompts();
		}
		if (Prompt.Circle[1].fillAmount == 0f)
		{
			Prompt.Circle[1].fillAmount = 1f;
			if (Prompt.Yandere.Armed)
			{
				Weapon = Prompt.Yandere.EquippedWeapon.gameObject.GetComponent<WeaponScript>();
				Prompt.Yandere.EmptyHands();
				Weapon.transform.parent = WeaponSpot;
				Weapon.transform.localPosition = Vector3.zero;
				Weapon.transform.localEulerAngles = Vector3.zero;
				Weapon.gameObject.GetComponent<Rigidbody>().useGravity = false;
				Weapon.MyCollider.enabled = false;
				Weapon.Prompt.Hide();
				Weapon.Prompt.enabled = false;
			}
			else
			{
				BodyPart = Prompt.Yandere.PickUp;
				Prompt.Yandere.EmptyHands();
				BodyPart.transform.parent = BodyPartPositions[BodyPart.GetComponent<BodyPartScript>().Type];
				BodyPart.transform.localPosition = Vector3.zero;
				BodyPart.transform.localEulerAngles = Vector3.zero;
				BodyPart.gameObject.GetComponent<Rigidbody>().useGravity = false;
				BodyPart.MyCollider.enabled = false;
				BodyParts[BodyPart.GetComponent<BodyPartScript>().Type] = BodyPart;
			}
			Contents++;
			UpdatePrompts();
		}
		if (Prompt.Circle[3].fillAmount == 0f)
		{
			Prompt.Circle[3].fillAmount = 1f;
			if (!Open)
			{
				base.transform.parent = Prompt.Yandere.Backpack;
				base.transform.localPosition = Vector3.zero;
				base.transform.localEulerAngles = Vector3.zero;
				Prompt.Yandere.Container = this;
				Prompt.Yandere.WeaponMenu.UpdateSprites();
				Prompt.Yandere.ObstacleDetector.gameObject.SetActive(true);
				Prompt.MyCollider.enabled = false;
				Prompt.Hide();
				Prompt.enabled = false;
				Rigidbody component = GetComponent<Rigidbody>();
				component.isKinematic = true;
				component.useGravity = false;
			}
			else
			{
				if (Weapon != null)
				{
					Weapon.Prompt.Circle[3].fillAmount = -1f;
					Weapon.Prompt.enabled = true;
					Weapon = null;
				}
				else
				{
					BodyPart = null;
					ID = 1;
					while (BodyPart == null)
					{
						BodyPart = BodyParts[ID];
						BodyParts[ID] = null;
						ID++;
					}
					BodyPart.Prompt.Circle[3].fillAmount = -1f;
				}
				Contents--;
				UpdatePrompts();
			}
		}
		Lid.localEulerAngles = new Vector3(Lid.localEulerAngles.x, Lid.localEulerAngles.y, Mathf.Lerp(Lid.localEulerAngles.z, (!Open) ? 0f : 90f, Time.deltaTime * 10f));
		if (Weapon != null)
		{
			Weapon.transform.localPosition = Vector3.zero;
			Weapon.transform.localEulerAngles = Vector3.zero;
		}
		for (ID = 1; ID < BodyParts.Length; ID++)
		{
			if (BodyParts[ID] != null)
			{
				BodyParts[ID].transform.localPosition = Vector3.zero;
				BodyParts[ID].transform.localEulerAngles = Vector3.zero;
			}
		}
	}

	public void Drop()
	{
		base.transform.parent = null;
		base.transform.position = Prompt.Yandere.ObstacleDetector.transform.position + new Vector3(0f, 0.5f, 0f);
		base.transform.eulerAngles = Prompt.Yandere.ObstacleDetector.transform.eulerAngles;
		Prompt.Yandere.Container = null;
		Prompt.MyCollider.enabled = true;
		Prompt.enabled = true;
		Rigidbody component = GetComponent<Rigidbody>();
		component.isKinematic = false;
		component.useGravity = true;
	}

	public void UpdatePrompts()
	{
		if (Open)
		{
			Prompt.Label[0].text = "     Close";
			if (Contents > 0)
			{
				Prompt.Label[3].text = "     Remove";
				Prompt.HideButton[3] = false;
			}
			else
			{
				Prompt.HideButton[3] = true;
			}
			if (Prompt.Yandere.Armed)
			{
				if (!Prompt.Yandere.EquippedWeapon.Concealable)
				{
					if (Weapon == null)
					{
						Prompt.Label[1].text = "     Insert";
						Prompt.HideButton[1] = false;
					}
					else
					{
						Prompt.HideButton[1] = true;
					}
				}
				else
				{
					Prompt.HideButton[1] = true;
				}
			}
			else if (Prompt.Yandere.PickUp != null)
			{
				if (Prompt.Yandere.PickUp.BodyPart != null)
				{
					if (BodyParts[Prompt.Yandere.PickUp.gameObject.GetComponent<BodyPartScript>().Type] == null)
					{
						Prompt.Label[1].text = "     Insert";
						Prompt.HideButton[1] = false;
					}
					else
					{
						Prompt.HideButton[1] = true;
					}
				}
				else
				{
					Prompt.HideButton[1] = true;
				}
			}
			else
			{
				Prompt.HideButton[1] = true;
			}
		}
		else if (Prompt.Label[0] != null)
		{
			Prompt.Label[0].text = "     Open";
			Prompt.HideButton[1] = true;
			Prompt.Label[3].text = "     Wear";
			Prompt.HideButton[3] = false;
		}
	}
}
