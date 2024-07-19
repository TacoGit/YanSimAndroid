using UnityEngine;

public class BucketScript : MonoBehaviour
{
	public PhoneEventScript PhoneEvent;

	public ParticleSystem PourEffect;

	public ParticleSystem Sparkles;

	public YandereScript Yandere;

	public PickUpScript PickUp;

	public PromptScript Prompt;

	public GameObject WaterCollider;

	public GameObject BloodCollider;

	public GameObject GasCollider;

	[SerializeField]
	private GameObject BloodSpillEffect;

	[SerializeField]
	private GameObject GasSpillEffect;

	[SerializeField]
	private GameObject SpillEffect;

	[SerializeField]
	private GameObject Effect;

	[SerializeField]
	private GameObject[] Dumbbell;

	[SerializeField]
	private Transform[] Positions;

	[SerializeField]
	private Renderer Water;

	[SerializeField]
	private Renderer Blood;

	[SerializeField]
	private Renderer Gas;

	public float Bloodiness;

	public float FillSpeed = 1f;

	public float Timer;

	[SerializeField]
	private float Distance;

	[SerializeField]
	private float Rotate;

	public int Dumbbells;

	public bool UpdateAppearance;

	public bool Bleached;

	public bool Gasoline;

	public bool Dropped;

	public bool Poured;

	public bool Full;

	public bool Trap;

	public bool Fly;

	private void Start()
	{
		Water.transform.localPosition = new Vector3(Water.transform.localPosition.x, 0f, Water.transform.localPosition.z);
		Water.transform.localScale = new Vector3(0.235f, 1f, 0.14f);
		Water.material.color = new Color(Water.material.color.r, Water.material.color.g, Water.material.color.b, 0f);
		Blood.material.color = new Color(Blood.material.color.r, Blood.material.color.g, Blood.material.color.b, 0f);
		Gas.transform.localPosition = new Vector3(Gas.transform.localPosition.x, 0f, Gas.transform.localPosition.z);
		Gas.transform.localScale = new Vector3(0.235f, 1f, 0.14f);
		Gas.material.color = new Color(Gas.material.color.r, Gas.material.color.g, Gas.material.color.b, 0f);
		Yandere = GameObject.Find("YandereChan").GetComponent<YandereScript>();
	}

	private void Update()
	{
		if (PickUp.Clock.Period == 5)
		{
			PickUp.Suspicious = false;
		}
		else
		{
			PickUp.Suspicious = true;
		}
		Distance = Vector3.Distance(base.transform.position, Yandere.transform.position);
		if (Distance < 1f)
		{
			RaycastHit hitInfo;
			if (Yandere.Bucket == null)
			{
				if (base.transform.position.y > Yandere.transform.position.y - 0.1f && base.transform.position.y < Yandere.transform.position.y + 0.1f && Physics.Linecast(base.transform.position, Yandere.transform.position + Vector3.up, out hitInfo) && hitInfo.collider.gameObject == Yandere.gameObject)
				{
					Yandere.Bucket = this;
				}
			}
			else
			{
				if (Physics.Linecast(base.transform.position, Yandere.transform.position + Vector3.up, out hitInfo) && hitInfo.collider.gameObject != Yandere.gameObject)
				{
					Yandere.Bucket = null;
				}
				if (base.transform.position.y < Yandere.transform.position.y - 0.1f || base.transform.position.y > Yandere.transform.position.y + 0.1f)
				{
					Yandere.Bucket = null;
				}
			}
		}
		else if (Yandere.Bucket == this)
		{
			Yandere.Bucket = null;
		}
		if (Yandere.Bucket == this && Yandere.Dipping)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, Yandere.transform.position + Yandere.transform.forward * 0.55f, Time.deltaTime * 10f);
			Quaternion b = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z) - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
		}
		if (Yandere.PickUp != null)
		{
			if (Yandere.PickUp.JerryCan)
			{
				if (!Yandere.PickUp.Empty)
				{
					Prompt.Label[0].text = "     Pour Gasoline";
					Prompt.HideButton[0] = false;
				}
				else
				{
					Prompt.HideButton[0] = true;
				}
			}
			else if (Yandere.PickUp.Bleach)
			{
				if (Full && !Gasoline && !Bleached)
				{
					Prompt.Label[0].text = "     Pour Bleach";
					Prompt.HideButton[0] = false;
				}
				else
				{
					Prompt.HideButton[0] = true;
				}
			}
		}
		else if (Yandere.Equipped > 0)
		{
			if (!Full)
			{
				if (Yandere.EquippedWeapon.WeaponID == 12)
				{
					if (Dumbbells < 5)
					{
						Prompt.Label[0].text = "     Place Dumbbell";
						Prompt.HideButton[0] = false;
					}
					else
					{
						Prompt.HideButton[0] = true;
					}
				}
				else
				{
					Prompt.HideButton[0] = true;
				}
			}
			else
			{
				Prompt.HideButton[0] = true;
			}
		}
		else if (Dumbbells == 0)
		{
			Prompt.HideButton[0] = true;
		}
		else
		{
			Prompt.Label[0].text = "     Remove Dumbbell";
			Prompt.HideButton[0] = false;
		}
		if (Dumbbells > 0)
		{
			if (ClassGlobals.PhysicalGrade + ClassGlobals.PhysicalBonus == 0)
			{
				Prompt.Label[3].text = "     Physical Stat Too Low";
				Prompt.Circle[3].fillAmount = 1f;
			}
			else
			{
				Prompt.Label[3].text = "     Carry";
			}
		}
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			if (Prompt.Label[0].text == "     Place Dumbbell")
			{
				Dumbbells++;
				Dumbbell[Dumbbells] = Yandere.EquippedWeapon.gameObject;
				Yandere.EmptyHands();
				Dumbbell[Dumbbells].GetComponent<WeaponScript>().enabled = false;
				Dumbbell[Dumbbells].GetComponent<PromptScript>().enabled = false;
				Dumbbell[Dumbbells].GetComponent<PromptScript>().Hide();
				Dumbbell[Dumbbells].GetComponent<Collider>().enabled = false;
				Rigidbody component = Dumbbell[Dumbbells].GetComponent<Rigidbody>();
				component.useGravity = false;
				component.isKinematic = true;
				Dumbbell[Dumbbells].transform.parent = base.transform;
				Dumbbell[Dumbbells].transform.localPosition = Positions[Dumbbells].localPosition;
				Dumbbell[Dumbbells].transform.localEulerAngles = new Vector3(90f, 0f, 0f);
			}
			else if (Prompt.Label[0].text == "     Remove Dumbbell")
			{
				Yandere.EmptyHands();
				Dumbbell[Dumbbells].GetComponent<WeaponScript>().enabled = true;
				Dumbbell[Dumbbells].GetComponent<PromptScript>().enabled = true;
				Dumbbell[Dumbbells].GetComponent<WeaponScript>().Prompt.Circle[3].fillAmount = 0f;
				Rigidbody component2 = Dumbbell[Dumbbells].GetComponent<Rigidbody>();
				component2.isKinematic = false;
				Dumbbell[Dumbbells] = null;
				Dumbbells--;
			}
			else if (Prompt.Label[0].text == "     Pour Gasoline")
			{
				Yandere.PickUp.Empty = true;
				Gasoline = true;
				Fill();
			}
			else
			{
				Sparkles.Play();
				Bleached = true;
			}
		}
		if (UpdateAppearance)
		{
			if (Full)
			{
				if (!Gasoline)
				{
					Water.transform.localScale = Vector3.Lerp(Water.transform.localScale, new Vector3(0.285f, 1f, 0.17f), Time.deltaTime * 5f * FillSpeed);
					Water.transform.localPosition = new Vector3(Water.transform.localPosition.x, Mathf.Lerp(Water.transform.localPosition.y, 0.2f, Time.deltaTime * 5f * FillSpeed), Water.transform.localPosition.z);
					Water.material.color = new Color(Water.material.color.r, Water.material.color.g, Water.material.color.b, Mathf.Lerp(Water.material.color.a, 0.5f, Time.deltaTime * 5f));
				}
				else
				{
					Gas.transform.localScale = Vector3.Lerp(Gas.transform.localScale, new Vector3(0.285f, 1f, 0.17f), Time.deltaTime * 5f * FillSpeed);
					Gas.transform.localPosition = new Vector3(Gas.transform.localPosition.x, Mathf.Lerp(Gas.transform.localPosition.y, 0.2f, Time.deltaTime * 5f * FillSpeed), Gas.transform.localPosition.z);
					Gas.material.color = new Color(Gas.material.color.r, Gas.material.color.g, Gas.material.color.b, Mathf.Lerp(Gas.material.color.a, 0.5f, Time.deltaTime * 5f));
				}
			}
			else
			{
				Water.transform.localScale = Vector3.Lerp(Water.transform.localScale, new Vector3(0.235f, 1f, 0.14f), Time.deltaTime * 5f);
				Water.transform.localPosition = new Vector3(Water.transform.localPosition.x, Mathf.Lerp(Water.transform.localPosition.y, 0f, Time.deltaTime * 5f), Water.transform.localPosition.z);
				Water.material.color = new Color(Water.material.color.r, Water.material.color.g, Water.material.color.b, Mathf.Lerp(Water.material.color.a, 0f, Time.deltaTime * 5f));
				Gas.transform.localScale = Vector3.Lerp(Gas.transform.localScale, new Vector3(0.235f, 1f, 0.14f), Time.deltaTime * 5f);
				Gas.transform.localPosition = new Vector3(Gas.transform.localPosition.x, Mathf.Lerp(Gas.transform.localPosition.y, 0f, Time.deltaTime * 5f), Gas.transform.localPosition.z);
				Gas.material.color = new Color(Gas.material.color.r, Gas.material.color.g, Gas.material.color.b, Mathf.Lerp(Gas.material.color.a, 0f, Time.deltaTime * 5f));
			}
			Blood.material.color = new Color(Blood.material.color.r, Blood.material.color.g, Blood.material.color.b, Mathf.Lerp(Blood.material.color.a, Bloodiness / 100f, Time.deltaTime));
			Blood.transform.localPosition = new Vector3(Blood.transform.localPosition.x, Water.transform.localPosition.y + 0.001f, Blood.transform.localPosition.z);
			Blood.transform.localScale = Water.transform.localScale;
			Timer = Mathf.MoveTowards(Timer, 5f, Time.deltaTime);
			if (Timer == 5f)
			{
				UpdateAppearance = false;
				Timer = 0f;
			}
		}
		if (Yandere.PickUp != null)
		{
			if (Yandere.PickUp.Bucket == this)
			{
				Prompt.Hide();
				Prompt.enabled = false;
				if (Input.GetKeyDown(KeyCode.B))
				{
					UpdateAppearance = true;
					if (Bloodiness == 0f)
					{
						Bloodiness = 100f;
						Gasoline = false;
					}
					else
					{
						Bloodiness = 0f;
						Gasoline = true;
					}
				}
			}
			else if (!Trap)
			{
				Prompt.enabled = true;
			}
		}
		else if (!Trap)
		{
			Prompt.enabled = true;
		}
		if (Fly)
		{
			if (Rotate < 360f)
			{
				if (Rotate == 0f)
				{
					if (Bloodiness < 50f)
					{
						if (!Gasoline)
						{
							Effect = Object.Instantiate(SpillEffect, base.transform.position + base.transform.forward * 0.5f + base.transform.up * 0.5f, base.transform.rotation);
						}
						else
						{
							Effect = Object.Instantiate(GasSpillEffect, base.transform.position + base.transform.forward * 0.5f + base.transform.up * 0.5f, base.transform.rotation);
							Gasoline = false;
						}
					}
					else
					{
						Effect = Object.Instantiate(BloodSpillEffect, base.transform.position + base.transform.forward * 0.5f + base.transform.up * 0.5f, base.transform.rotation);
						Bloodiness = 0f;
					}
					if (Trap)
					{
						Effect.transform.LookAt(Effect.transform.position - Vector3.up);
					}
					else
					{
						Rigidbody component3 = GetComponent<Rigidbody>();
						component3.AddRelativeForce(Vector3.forward * 150f);
						component3.AddRelativeForce(Vector3.up * 250f);
						base.transform.Translate(Vector3.forward * 0.5f);
					}
				}
				Rotate += Time.deltaTime * 360f;
				base.transform.Rotate(Vector3.right * Time.deltaTime * 360f);
			}
			else
			{
				Sparkles.Stop();
				Rotate = 0f;
				Trap = false;
				Fly = false;
			}
		}
		if (Dropped && base.transform.position.y < 0.5f)
		{
			Dropped = false;
		}
	}

	public void Empty()
	{
		UpdateAppearance = true;
		Bloodiness = 0f;
		Bleached = false;
		Gasoline = false;
		Sparkles.Stop();
		Full = false;
	}

	public void Fill()
	{
		UpdateAppearance = true;
		Full = true;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (!Dropped)
		{
			return;
		}
		StudentScript component = other.gameObject.GetComponent<StudentScript>();
		if (component != null)
		{
			GetComponent<AudioSource>().Play();
			while (Dumbbells > 0)
			{
				Dumbbell[Dumbbells].GetComponent<WeaponScript>().enabled = true;
				Dumbbell[Dumbbells].GetComponent<PromptScript>().enabled = true;
				Dumbbell[Dumbbells].GetComponent<Collider>().enabled = true;
				Rigidbody component2 = Dumbbell[Dumbbells].GetComponent<Rigidbody>();
				component2.constraints = RigidbodyConstraints.None;
				component2.isKinematic = false;
				component2.useGravity = true;
				Dumbbell[Dumbbells].transform.parent = null;
				Dumbbell[Dumbbells] = null;
				Dumbbells--;
			}
			component.DeathType = DeathType.Weight;
			component.BecomeRagdoll();
			Dropped = false;
			GameObjectUtils.SetLayerRecursively(base.gameObject, 15);
		}
	}
}
