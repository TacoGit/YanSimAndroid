using UnityEngine;

public class PickUpScript : MonoBehaviour
{
	public RigidbodyConstraints OriginalConstraints;

	public BloodCleanerScript BloodCleaner;

	public IncineratorScript Incinerator;

	public BodyPartScript BodyPart;

	public TrashCanScript TrashCan;

	public OutlineScript[] Outline;

	public YandereScript Yandere;

	public Animation MyAnimation;

	public BucketScript Bucket;

	public PromptScript Prompt;

	public ClockScript Clock;

	public MopScript Mop;

	public Mesh ClosedBook;

	public Mesh OpenBook;

	public Rigidbody MyRigidbody;

	public Collider MyCollider;

	public MeshFilter MyRenderer;

	public Vector3 TrashPosition;

	public Vector3 TrashRotation;

	public Vector3 OriginalScale;

	public Vector3 HoldPosition;

	public Vector3 HoldRotation;

	public Color EvidenceColor;

	public Color OriginalColor;

	public bool CleaningProduct;

	public bool DisableAtStart;

	public bool LockRotation;

	public bool BeingLifted;

	public bool CanCollide;

	public bool Electronic;

	public bool Flashlight;

	public bool Suspicious;

	public bool Blowtorch;

	public bool Clothing;

	public bool Evidence;

	public bool JerryCan;

	public bool LeftHand;

	public bool RedPaint;

	public bool Garbage;

	public bool Bleach;

	public bool Dumped;

	public bool Usable;

	public bool Weight;

	public bool Salty;

	public int CarryAnimID;

	public int Strength;

	public int Period;

	public int Food;

	public float KinematicTimer;

	public float DumpTimer;

	public bool Empty = true;

	public GameObject[] FoodPieces;

	public WeaponScript StuckBoxCutter;

	private void Start()
	{
		Yandere = GameObject.Find("YandereChan").GetComponent<YandereScript>();
		Clock = GameObject.Find("Clock").GetComponent<ClockScript>();
		if (!CanCollide)
		{
			Physics.IgnoreCollision(Yandere.GetComponent<Collider>(), MyCollider);
		}
		OriginalColor = Outline[0].color;
		OriginalScale = base.transform.localScale;
		if (MyRigidbody == null)
		{
			MyRigidbody = GetComponent<Rigidbody>();
		}
		if (DisableAtStart)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void LateUpdate()
	{
		if (CleaningProduct)
		{
			if (Clock.Period == 5)
			{
				Suspicious = false;
			}
			else
			{
				Suspicious = true;
			}
		}
		if (Weight)
		{
			if (Period < Clock.Period)
			{
				Strength = ClassGlobals.PhysicalGrade + ClassGlobals.PhysicalBonus;
			}
			if (Strength == 0)
			{
				Prompt.Label[3].text = "     Physical Stat Too Low";
				Prompt.Circle[3].fillAmount = 1f;
			}
			else
			{
				Prompt.Label[3].text = "     Carry";
			}
		}
		if (Prompt.Circle[3].fillAmount == 0f)
		{
			Prompt.Circle[3].fillAmount = 1f;
			if (Weight)
			{
				if (!Yandere.Chased && Yandere.Chasers == 0)
				{
					if (Yandere.PickUp != null)
					{
						Yandere.CharacterAnimation[Yandere.CarryAnims[Yandere.PickUp.CarryAnimID]].weight = 0f;
					}
					if (Yandere.Armed)
					{
						Yandere.CharacterAnimation[Yandere.ArmedAnims[Yandere.EquippedWeapon.AnimID]].weight = 0f;
					}
					Yandere.targetRotation = Quaternion.LookRotation(new Vector3(base.transform.position.x, Yandere.transform.position.y, base.transform.position.z) - Yandere.transform.position);
					Yandere.transform.rotation = Yandere.targetRotation;
					Yandere.EmptyHands();
					base.transform.parent = Yandere.transform;
					base.transform.localPosition = new Vector3(0f, 0f, 0.79184f);
					base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					Yandere.Character.GetComponent<Animation>().Play("f02_heavyWeightLift_00");
					Yandere.HeavyWeight = true;
					Yandere.CanMove = false;
					Yandere.Lifting = true;
					MyAnimation.Play("Weight_liftUp_00");
					MyRigidbody.isKinematic = true;
					BeingLifted = true;
				}
			}
			else
			{
				BePickedUp();
			}
		}
		if (Yandere.PickUp == this)
		{
			base.transform.localPosition = HoldPosition;
			base.transform.localEulerAngles = HoldRotation;
			if (Garbage && !Yandere.StudentManager.IncineratorArea.bounds.Contains(Yandere.transform.position))
			{
				Drop();
				base.transform.position = new Vector3(-40f, 0f, 24f);
			}
		}
		if (Dumped)
		{
			DumpTimer += Time.deltaTime;
			if (DumpTimer > 1f)
			{
				if (Clothing)
				{
					Yandere.Incinerator.BloodyClothing++;
				}
				else if ((bool)BodyPart)
				{
					Yandere.Incinerator.BodyParts++;
				}
				Object.Destroy(base.gameObject);
			}
		}
		if (Yandere.PickUp != this && !MyRigidbody.isKinematic)
		{
			KinematicTimer = Mathf.MoveTowards(KinematicTimer, 5f, Time.deltaTime);
			if (KinematicTimer == 5f)
			{
				MyRigidbody.isKinematic = true;
				KinematicTimer = 0f;
			}
			if (base.transform.position.x > -71f && base.transform.position.x < -61f && base.transform.position.z > -37.5f && base.transform.position.z < -27.5f)
			{
				base.transform.position = new Vector3(-63f, 1f, -26.5f);
				KinematicTimer = 0f;
			}
			if (base.transform.position.x > -46f && base.transform.position.x < -18f && base.transform.position.z > 66f && base.transform.position.z < 78f)
			{
				base.transform.position = new Vector3(-16f, 5f, 72f);
				KinematicTimer = 0f;
			}
		}
		if (!Weight || !BeingLifted)
		{
			return;
		}
		if (Yandere.Lifting)
		{
			if (Yandere.StudentManager.Stop)
			{
				Drop();
			}
		}
		else
		{
			BePickedUp();
		}
	}

	public void BePickedUp()
	{
		if (CarryAnimID == 10)
		{
			MyRenderer.mesh = OpenBook;
			Yandere.LifeNotePen.SetActive(true);
		}
		if (MyAnimation != null)
		{
			MyAnimation.Stop();
		}
		Prompt.Circle[3].fillAmount = 1f;
		BeingLifted = false;
		if (Yandere.PickUp != null)
		{
			Yandere.PickUp.Drop();
		}
		if (Yandere.Equipped == 3)
		{
			Yandere.Weapon[3].Drop();
		}
		else if (Yandere.Equipped > 0)
		{
			Yandere.Unequip();
		}
		if (Yandere.Dragging)
		{
			Yandere.Ragdoll.GetComponent<RagdollScript>().StopDragging();
		}
		if (Yandere.Carrying)
		{
			Yandere.StopCarrying();
		}
		if (!LeftHand)
		{
			base.transform.parent = Yandere.ItemParent;
		}
		else
		{
			base.transform.parent = Yandere.LeftItemParent;
		}
		if (GetComponent<RadioScript>() != null && GetComponent<RadioScript>().On)
		{
			GetComponent<RadioScript>().TurnOff();
		}
		MyCollider.enabled = false;
		if (MyRigidbody != null)
		{
			MyRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
		if (!Usable)
		{
			Prompt.Hide();
			Prompt.enabled = false;
			Yandere.NearestPrompt = null;
		}
		else
		{
			Prompt.Carried = true;
		}
		Yandere.PickUp = this;
		Yandere.CarryAnimID = CarryAnimID;
		OutlineScript[] outline = Outline;
		foreach (OutlineScript outlineScript in outline)
		{
			outlineScript.color = new Color(0f, 0f, 0f, 1f);
		}
		if ((bool)BodyPart)
		{
			Yandere.NearBodies++;
		}
		Yandere.StudentManager.UpdateStudents();
		MyRigidbody.isKinematic = true;
		KinematicTimer = 0f;
	}

	public void Drop()
	{
		if ((bool)TrashCan)
		{
			Yandere.MyController.radius = 0.2f;
		}
		if (CarryAnimID == 10)
		{
			MyRenderer.mesh = ClosedBook;
			Yandere.LifeNotePen.SetActive(false);
		}
		if (Weight)
		{
			Yandere.IdleAnim = Yandere.OriginalIdleAnim;
			Yandere.WalkAnim = Yandere.OriginalWalkAnim;
			Yandere.RunAnim = Yandere.OriginalRunAnim;
		}
		if (BloodCleaner != null)
		{
			BloodCleaner.enabled = true;
			BloodCleaner.Pathfinding.enabled = true;
		}
		Yandere.PickUp = null;
		if ((bool)BodyPart)
		{
			base.transform.parent = Yandere.LimbParent;
		}
		else
		{
			base.transform.parent = null;
		}
		if (LockRotation)
		{
			base.transform.localEulerAngles = new Vector3(0f, base.transform.localEulerAngles.y, 0f);
		}
		if (MyRigidbody != null)
		{
			MyRigidbody.constraints = OriginalConstraints;
			MyRigidbody.isKinematic = false;
			MyRigidbody.useGravity = true;
		}
		if (Dumped)
		{
			base.transform.position = Incinerator.DumpPoint.position;
		}
		else
		{
			Prompt.enabled = true;
			MyCollider.enabled = true;
			MyCollider.isTrigger = false;
			if (!CanCollide)
			{
				Physics.IgnoreCollision(Yandere.GetComponent<Collider>(), MyCollider);
			}
		}
		Prompt.Carried = false;
		OutlineScript[] outline = Outline;
		foreach (OutlineScript outlineScript in outline)
		{
			outlineScript.color = ((!Evidence) ? OriginalColor : EvidenceColor);
		}
		base.transform.localScale = OriginalScale;
		if ((bool)BodyPart)
		{
			Yandere.NearBodies--;
		}
		Yandere.StudentManager.UpdateStudents();
		if (Clothing && Evidence)
		{
			base.transform.parent = Yandere.Police.BloodParent;
		}
	}
}
