using UnityEngine;

public class RagdollScript : MonoBehaviour
{
	public BloodPoolSpawnerScript BloodPoolSpawner;

	public DetectionMarkerScript DetectionMarker;

	public IncineratorScript Incinerator;

	public WoodChipperScript WoodChipper;

	public TranqCaseScript TranqCase;

	public StudentScript Student;

	public YandereScript Yandere;

	public PoliceScript Police;

	public PromptScript Prompt;

	public SkinnedMeshRenderer MyRenderer;

	public Collider BloodSpawnerCollider;

	public Animation CharacterAnimation;

	public Collider HideCollider;

	public Rigidbody[] AllRigidbodies;

	public Collider[] AllColliders;

	public Rigidbody[] Rigidbodies;

	public Transform[] SpawnPoints;

	public GameObject[] BodyParts;

	public Transform NearestLimb;

	public Transform RightBreast;

	public Transform LeftBreast;

	public Transform PelvisRoot;

	public Transform Ponytail;

	public Transform RightEye;

	public Transform LeftEye;

	public Transform HairR;

	public Transform HairL;

	public Transform[] Limb;

	public Transform Head;

	public Vector3 RightEyeOrigin;

	public Vector3 LeftEyeOrigin;

	public Vector3[] LimbAnchor;

	public GameObject Character;

	public GameObject Zs;

	public bool ElectrocutionAnimation;

	public bool MurderSuicideAnimation;

	public bool BurningAnimation;

	public bool ChokingAnimation;

	public bool AddingToCount;

	public bool MurderSuicide;

	public bool Cauterizable;

	public bool Electrocuted;

	public bool StopAnimation = true;

	public bool Decapitated;

	public bool Dismembered;

	public bool NeckSnapped;

	public bool Cauterized;

	public bool Disturbing;

	public bool Sacrifice;

	public bool Disposed;

	public bool Poisoned;

	public bool Tranquil;

	public bool Burning;

	public bool Carried;

	public bool Choking;

	public bool Dragged;

	public bool Drowned;

	public bool Falling;

	public bool Nemesis;

	public bool Settled;

	public bool Suicide;

	public bool Burned;

	public bool Dumped;

	public bool Hidden;

	public bool Pushed;

	public bool Male;

	public float AnimStartTime;

	public float SettleTimer;

	public float BreastSize;

	public float DumpTimer;

	public float EyeShrink;

	public float FallTimer;

	public int StudentID;

	public RagdollDumpType DumpType;

	public int LimbID;

	public int Frame;

	public string DumpedAnim = string.Empty;

	public string LiftAnim = string.Empty;

	public string IdleAnim = string.Empty;

	public string WalkAnim = string.Empty;

	public string RunAnim = string.Empty;

	private void Start()
	{
		ElectrocutionAnimation = false;
		MurderSuicideAnimation = false;
		BurningAnimation = false;
		ChokingAnimation = false;
		Disturbing = false;
		Physics.IgnoreLayerCollision(11, 13, true);
		Zs.SetActive(Tranquil);
		if (!Tranquil && !Poisoned && !Drowned && !Electrocuted && !Burning && !NeckSnapped)
		{
			Student.StudentManager.TutorialWindow.ShowPoolMessage = true;
			BloodPoolSpawner.gameObject.SetActive(true);
			if (Pushed)
			{
				BloodPoolSpawner.Timer = 5f;
			}
		}
		for (int i = 0; i < AllRigidbodies.Length; i++)
		{
			AllRigidbodies[i].isKinematic = false;
			AllColliders[i].enabled = true;
			if (Yandere.StudentManager.NoGravity)
			{
				AllRigidbodies[i].useGravity = false;
			}
		}
		Prompt.enabled = true;
		if (ClassGlobals.PhysicalGrade + ClassGlobals.PhysicalBonus > 0 && !Tranquil)
		{
			Prompt.HideButton[3] = false;
		}
		if (Student.Yandere.BlackHole)
		{
			DisableRigidbodies();
		}
	}

	private void Update()
	{
		if (!Dragged && !Carried && !Settled && !Yandere.PK && !Yandere.StudentManager.NoGravity)
		{
			SettleTimer += Time.deltaTime;
			if (SettleTimer > 5f)
			{
				Settled = true;
				for (int i = 0; i < AllRigidbodies.Length; i++)
				{
					AllRigidbodies[i].isKinematic = true;
					AllColliders[i].enabled = false;
				}
			}
		}
		if (DetectionMarker != null)
		{
			if (DetectionMarker.Tex.color.a > 0.1f)
			{
				DetectionMarker.Tex.color = new Color(DetectionMarker.Tex.color.r, DetectionMarker.Tex.color.g, DetectionMarker.Tex.color.b, Mathf.MoveTowards(DetectionMarker.Tex.color.a, 0f, Time.deltaTime * 10f));
			}
			else
			{
				DetectionMarker.Tex.color = new Color(DetectionMarker.Tex.color.r, DetectionMarker.Tex.color.g, DetectionMarker.Tex.color.b, 0f);
				DetectionMarker = null;
			}
		}
		if (!Dumped)
		{
			if (StopAnimation && Student.CharacterAnimation.isPlaying)
			{
				Student.CharacterAnimation.Stop();
			}
			if (!Input.GetButtonDown("LB"))
			{
				if (BloodPoolSpawner != null && BloodPoolSpawner.gameObject.activeInHierarchy && !Cauterized)
				{
					if (Yandere.PickUp != null)
					{
						if (Yandere.PickUp.Blowtorch)
						{
							if (!Cauterizable)
							{
								Prompt.Label[0].text = "     Cauterize";
								Cauterizable = true;
							}
						}
						else if (Cauterizable)
						{
							Prompt.Label[0].text = "     Dismember";
							Cauterizable = false;
						}
					}
					else if (Cauterizable)
					{
						Prompt.Label[0].text = "     Dismember";
						Cauterizable = false;
					}
				}
				if (Prompt.Circle[0].fillAmount == 0f)
				{
					Prompt.Circle[0].fillAmount = 1f;
					if (!Yandere.Chased && Yandere.Chasers == 0)
					{
						if (Cauterizable)
						{
							Prompt.Label[0].text = "     Dismember";
							BloodPoolSpawner.enabled = false;
							Cauterizable = false;
							Cauterized = true;
							Yandere.CharacterAnimation.CrossFade("f02_cauterize_00");
							Yandere.Cauterizing = true;
							Yandere.CanMove = false;
							Yandere.PickUp.GetComponent<BlowtorchScript>().enabled = true;
							Yandere.PickUp.GetComponent<AudioSource>().Play();
						}
						else
						{
							Yandere.CharacterAnimation.CrossFade("f02_dismember_00");
							Yandere.transform.LookAt(base.transform);
							Yandere.RPGCamera.transform.position = Yandere.DismemberSpot.position;
							Yandere.RPGCamera.transform.eulerAngles = Yandere.DismemberSpot.eulerAngles;
							Yandere.EquippedWeapon.Dismember();
							Yandere.RPGCamera.enabled = false;
							Yandere.TargetStudent = Student;
							Yandere.Ragdoll = base.gameObject;
							Yandere.Dismembering = true;
							Yandere.CanMove = false;
						}
					}
				}
				if (Prompt.Circle[1].fillAmount == 0f)
				{
					Prompt.Circle[1].fillAmount = 1f;
					if (!Student.FireEmitters[1].isPlaying)
					{
						if (!Dragged)
						{
							Yandere.EmptyHands();
							Prompt.AcceptingInput[1] = false;
							Prompt.Label[1].text = "     Drop";
							PickNearestLimb();
							Yandere.RagdollDragger.connectedBody = Rigidbodies[LimbID];
							Yandere.RagdollDragger.connectedAnchor = LimbAnchor[LimbID];
							Yandere.Dragging = true;
							Yandere.DragState = 0;
							Yandere.Ragdoll = base.gameObject;
							Dragged = true;
							Yandere.StudentManager.UpdateStudents();
							if (MurderSuicide)
							{
								Police.MurderSuicideScene = false;
								MurderSuicide = false;
							}
							if (Suicide)
							{
								Police.Suicide = false;
								Suicide = false;
							}
							for (int j = 0; j < Student.Ragdoll.AllRigidbodies.Length; j++)
							{
								Student.Ragdoll.AllRigidbodies[j].drag = 2f;
							}
							for (int k = 0; k < AllRigidbodies.Length; k++)
							{
								AllRigidbodies[k].isKinematic = false;
								AllColliders[k].enabled = true;
								if (Yandere.StudentManager.NoGravity)
								{
									AllRigidbodies[k].useGravity = false;
								}
							}
						}
						else
						{
							StopDragging();
						}
					}
				}
				if (Prompt.Circle[3].fillAmount == 0f)
				{
					Prompt.Circle[3].fillAmount = 1f;
					if (!Student.FireEmitters[1].isPlaying)
					{
						Yandere.EmptyHands();
						Prompt.Label[1].text = "     Drop";
						Prompt.HideButton[1] = true;
						Prompt.HideButton[3] = true;
						Prompt.enabled = false;
						Prompt.Hide();
						for (int l = 0; l < AllRigidbodies.Length; l++)
						{
							AllRigidbodies[l].isKinematic = true;
							AllColliders[l].enabled = false;
						}
						if (Male)
						{
							Rigidbody rigidbody = AllRigidbodies[0];
							rigidbody.transform.parent.transform.localPosition = new Vector3(rigidbody.transform.parent.transform.localPosition.x, 0.2f, rigidbody.transform.parent.transform.localPosition.z);
						}
						Yandere.CharacterAnimation.Play("f02_carryLiftA_00");
						Student.CharacterAnimation.Play(LiftAnim);
						BloodSpawnerCollider.enabled = false;
						PelvisRoot.localEulerAngles = new Vector3(PelvisRoot.localEulerAngles.x, 0f, PelvisRoot.localEulerAngles.z);
						Prompt.MyCollider.enabled = false;
						PelvisRoot.localPosition = new Vector3(PelvisRoot.localPosition.x, PelvisRoot.localPosition.y, 0f);
						Yandere.Ragdoll = base.gameObject;
						Yandere.CurrentRagdoll = this;
						Yandere.CanMove = false;
						Yandere.Lifting = true;
						StopAnimation = false;
						Carried = true;
						Falling = false;
						FallTimer = 0f;
					}
				}
			}
			else if (Yandere.CanMove && Dragged)
			{
				StopDragging();
			}
			if (Vector3.Distance(Yandere.transform.position, Prompt.transform.position) < 2f)
			{
				if (!Suicide && !AddingToCount)
				{
					Yandere.NearestCorpseID = StudentID;
					Yandere.NearBodies++;
					AddingToCount = true;
				}
			}
			else if (AddingToCount)
			{
				Yandere.NearBodies--;
				AddingToCount = false;
			}
			if (!Prompt.AcceptingInput[1] && Input.GetButtonUp("B"))
			{
				Prompt.AcceptingInput[1] = true;
			}
			bool flag = false;
			if (Yandere.Armed && Yandere.EquippedWeapon.WeaponID == 7 && !Student.Nemesis)
			{
				flag = true;
			}
			if (!Cauterized && Yandere.PickUp != null && Yandere.PickUp.Blowtorch && BloodPoolSpawner.gameObject.activeInHierarchy)
			{
				flag = true;
			}
			Prompt.HideButton[0] = Dragged || Carried || Tranquil || !flag;
		}
		else if (DumpType == RagdollDumpType.Incinerator)
		{
			if (DumpTimer == 0f && Yandere.Carrying)
			{
				Student.CharacterAnimation[DumpedAnim].time = 2.5f;
			}
			Student.CharacterAnimation.CrossFade(DumpedAnim);
			DumpTimer += Time.deltaTime;
			if (Student.CharacterAnimation[DumpedAnim].time >= Student.CharacterAnimation[DumpedAnim].length)
			{
				Incinerator.Corpses++;
				Incinerator.CorpseList[Incinerator.Corpses] = StudentID;
				Remove();
				base.enabled = false;
			}
		}
		else if (DumpType == RagdollDumpType.TranqCase)
		{
			if (DumpTimer == 0f && Yandere.Carrying)
			{
				Student.CharacterAnimation[DumpedAnim].time = 2.5f;
			}
			Student.CharacterAnimation.CrossFade(DumpedAnim);
			DumpTimer += Time.deltaTime;
			if (Student.CharacterAnimation[DumpedAnim].time >= Student.CharacterAnimation[DumpedAnim].length)
			{
				TranqCase.Open = false;
				if (AddingToCount)
				{
					Yandere.NearBodies--;
				}
				base.enabled = false;
			}
		}
		else if (DumpType == RagdollDumpType.WoodChipper)
		{
			if (DumpTimer == 0f && Yandere.Carrying)
			{
				Student.CharacterAnimation[DumpedAnim].time = 2.5f;
			}
			Student.CharacterAnimation.CrossFade(DumpedAnim);
			DumpTimer += Time.deltaTime;
			if (Student.CharacterAnimation[DumpedAnim].time >= Student.CharacterAnimation[DumpedAnim].length)
			{
				WoodChipper.VictimID = StudentID;
				Remove();
				base.enabled = false;
			}
		}
		if (Hidden && HideCollider == null)
		{
			Police.HiddenCorpses--;
			Hidden = false;
		}
		if (Falling)
		{
			FallTimer += Time.deltaTime;
			if (FallTimer > 2f)
			{
				BloodSpawnerCollider.enabled = true;
				FallTimer = 0f;
				Falling = false;
			}
		}
		if (Burning)
		{
			for (int m = 0; m < 3; m++)
			{
				Material material = MyRenderer.materials[m];
				material.color = Vector4.MoveTowards(material.color, new Vector4(0.1f, 0.1f, 0.1f, 1f), Time.deltaTime * 0.1f);
			}
			Student.Cosmetic.HairRenderer.material.color = Vector4.MoveTowards(Student.Cosmetic.HairRenderer.material.color, new Vector4(0.1f, 0.1f, 0.1f, 1f), Time.deltaTime * 0.1f);
			if (MyRenderer.materials[0].color == new Color(0.1f, 0.1f, 0.1f, 1f))
			{
				Burning = false;
				Burned = true;
			}
		}
		if (Burned)
		{
			Sacrifice = Vector3.Distance(Prompt.transform.position, Yandere.StudentManager.SacrificeSpot.position) < 1.5f;
		}
	}

	private void LateUpdate()
	{
		if (!Male)
		{
			if (LeftEye != null)
			{
				LeftEye.localPosition = new Vector3(LeftEye.localPosition.x, LeftEye.localPosition.y, LeftEyeOrigin.z - EyeShrink * 0.01f);
				RightEye.localPosition = new Vector3(RightEye.localPosition.x, RightEye.localPosition.y, RightEyeOrigin.z + EyeShrink * 0.01f);
				LeftEye.localScale = new Vector3(1f - EyeShrink * 0.5f, 1f - EyeShrink * 0.5f, LeftEye.localScale.z);
				RightEye.localScale = new Vector3(1f - EyeShrink * 0.5f, 1f - EyeShrink * 0.5f, RightEye.localScale.z);
			}
			if (StudentID == 81)
			{
				for (int i = 0; i < 4; i++)
				{
					Transform transform = Student.Skirt[i];
					transform.transform.localScale = new Vector3(transform.transform.localScale.x, 2f / 3f, transform.transform.localScale.z);
				}
			}
		}
		if (Decapitated)
		{
			Head.localScale = Vector3.zero;
		}
		if (!(Yandere.Ragdoll == base.gameObject))
		{
			return;
		}
		if (Yandere.DumpTimer < 1f)
		{
			if (Yandere.Lifting)
			{
				base.transform.position = Yandere.transform.position;
				base.transform.eulerAngles = Yandere.transform.eulerAngles;
			}
			else if (Carried)
			{
				base.transform.position = Yandere.transform.position;
				base.transform.eulerAngles = Yandere.transform.eulerAngles;
				float axis = Input.GetAxis("Vertical");
				float axis2 = Input.GetAxis("Horizontal");
				if (axis != 0f || axis2 != 0f)
				{
					Student.CharacterAnimation.CrossFade((!Input.GetButton("LB")) ? WalkAnim : RunAnim);
				}
				else
				{
					Student.CharacterAnimation.CrossFade(IdleAnim);
				}
				Student.CharacterAnimation[IdleAnim].time = Yandere.CharacterAnimation["f02_carryIdleA_00"].time;
				Student.CharacterAnimation[WalkAnim].time = Yandere.CharacterAnimation["f02_carryWalkA_00"].time;
				Student.CharacterAnimation[RunAnim].time = Yandere.CharacterAnimation["f02_carryRunA_00"].time;
			}
		}
		if (Carried)
		{
			if (Male)
			{
				Rigidbody rigidbody = AllRigidbodies[0];
				rigidbody.transform.parent.transform.localPosition = new Vector3(rigidbody.transform.parent.transform.localPosition.x, 0.2f, rigidbody.transform.parent.transform.localPosition.z);
			}
			if (Yandere.Struggling || Yandere.DelinquentFighting || Yandere.Sprayed)
			{
				Fall();
			}
		}
	}

	public void StopDragging()
	{
		Rigidbody[] allRigidbodies = Student.Ragdoll.AllRigidbodies;
		foreach (Rigidbody rigidbody in allRigidbodies)
		{
			rigidbody.drag = 0f;
		}
		if (ClassGlobals.PhysicalGrade + ClassGlobals.PhysicalBonus > 0 && !Tranquil)
		{
			Prompt.HideButton[3] = false;
		}
		Prompt.AcceptingInput[1] = false;
		Prompt.Circle[1].fillAmount = 1f;
		Prompt.Label[1].text = "     Drag";
		Yandere.RagdollDragger.connectedBody = null;
		Yandere.RagdollPK.connectedBody = null;
		Yandere.Dragging = false;
		Yandere.Ragdoll = null;
		Yandere.StudentManager.UpdateStudents();
		SettleTimer = 0f;
		Settled = false;
		Dragged = false;
	}

	private void PickNearestLimb()
	{
		NearestLimb = Limb[0];
		LimbID = 0;
		for (int i = 1; i < 4; i++)
		{
			Transform transform = Limb[i];
			if (Vector3.Distance(transform.position, Yandere.transform.position) < Vector3.Distance(NearestLimb.position, Yandere.transform.position))
			{
				NearestLimb = transform;
				LimbID = i;
			}
		}
	}

	public void Dump()
	{
		if (DumpType == RagdollDumpType.Incinerator)
		{
			base.transform.eulerAngles = Yandere.transform.eulerAngles;
			base.transform.position = Yandere.transform.position;
			Incinerator = Yandere.Incinerator;
			BloodPoolSpawner.enabled = false;
		}
		else if (DumpType == RagdollDumpType.TranqCase)
		{
			TranqCase = Yandere.TranqCase;
		}
		else if (DumpType == RagdollDumpType.WoodChipper)
		{
			WoodChipper = Yandere.WoodChipper;
		}
		Prompt.Hide();
		Prompt.enabled = false;
		Dumped = true;
		Rigidbody[] allRigidbodies = AllRigidbodies;
		foreach (Rigidbody rigidbody in allRigidbodies)
		{
			rigidbody.isKinematic = true;
		}
	}

	public void Fall()
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.0001f, base.transform.position.z);
		Prompt.Label[1].text = "     Drag";
		Prompt.HideButton[1] = false;
		Prompt.enabled = true;
		if (ClassGlobals.PhysicalGrade + ClassGlobals.PhysicalBonus > 0 && !Tranquil)
		{
			Prompt.HideButton[3] = false;
		}
		if (Dragged)
		{
			Yandere.RagdollDragger.connectedBody = null;
			Yandere.RagdollPK.connectedBody = null;
			Yandere.Dragging = false;
			Dragged = false;
		}
		Yandere.Ragdoll = null;
		Prompt.MyCollider.enabled = true;
		BloodPoolSpawner.NearbyBlood = 0;
		StopAnimation = true;
		SettleTimer = 0f;
		Carried = false;
		Settled = false;
		Falling = true;
		for (int i = 0; i < AllRigidbodies.Length; i++)
		{
			AllRigidbodies[i].isKinematic = false;
			AllColliders[i].enabled = true;
		}
	}

	public void QuickDismember()
	{
		for (int i = 0; i < BodyParts.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(BodyParts[i], SpawnPoints[i].position, Quaternion.identity);
			gameObject.transform.eulerAngles = SpawnPoints[i].eulerAngles;
			gameObject.GetComponent<PromptScript>().enabled = false;
			gameObject.GetComponent<PickUpScript>().enabled = false;
			gameObject.GetComponent<OutlineScript>().enabled = false;
		}
		if (BloodPoolSpawner.BloodParent == null)
		{
			BloodPoolSpawner.Start();
		}
		BloodPoolSpawner.SpawnBigPool();
		base.gameObject.SetActive(false);
	}

	public void Dismember()
	{
		if (Dismembered)
		{
			return;
		}
		Student.LiquidProjector.material.mainTexture = Student.BloodTexture;
		for (int i = 0; i < BodyParts.Length; i++)
		{
			if (Decapitated)
			{
				i++;
				Decapitated = false;
			}
			GameObject gameObject = Object.Instantiate(BodyParts[i], SpawnPoints[i].position, Quaternion.identity);
			gameObject.transform.parent = Yandere.LimbParent;
			gameObject.transform.eulerAngles = SpawnPoints[i].eulerAngles;
			BodyPartScript component = gameObject.GetComponent<BodyPartScript>();
			component.StudentID = StudentID;
			component.Sacrifice = Sacrifice;
			if (Yandere.StudentManager.NoGravity)
			{
				gameObject.GetComponent<Rigidbody>().useGravity = false;
			}
			switch (i)
			{
			case 0:
				if (!Student.OriginallyTeacher)
				{
					if (!Male)
					{
						Student.Cosmetic.FemaleHair[Student.Cosmetic.Hairstyle].transform.parent = gameObject.transform;
						if (Student.Cosmetic.FemaleAccessories[Student.Cosmetic.Accessory] != null && Student.Cosmetic.Accessory != 3 && Student.Cosmetic.Accessory != 6)
						{
							Student.Cosmetic.FemaleAccessories[Student.Cosmetic.Accessory].transform.parent = gameObject.transform;
						}
					}
					else
					{
						Transform transform = Student.Cosmetic.MaleHair[Student.Cosmetic.Hairstyle].transform;
						transform.parent = gameObject.transform;
						transform.localScale *= 1.0638298f;
						if (transform.transform.localPosition.y < -1f)
						{
							transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, transform.transform.localPosition.y - 0.095f, transform.transform.localPosition.z);
						}
						if (Student.Cosmetic.MaleAccessories[Student.Cosmetic.Accessory] != null)
						{
							Student.Cosmetic.MaleAccessories[Student.Cosmetic.Accessory].transform.parent = gameObject.transform;
						}
					}
				}
				else
				{
					Student.Cosmetic.TeacherHair[Student.Cosmetic.Hairstyle].transform.parent = gameObject.transform;
					if (Student.Cosmetic.TeacherAccessories[Student.Cosmetic.Accessory] != null)
					{
						Student.Cosmetic.TeacherAccessories[Student.Cosmetic.Accessory].transform.parent = gameObject.transform;
					}
				}
				if (Student.Club != ClubType.Photography && Student.Club < ClubType.Gaming && Student.Cosmetic.ClubAccessories[(int)Student.Club] != null)
				{
					Student.Cosmetic.ClubAccessories[(int)Student.Club].transform.parent = gameObject.transform;
					if (Student.Club == ClubType.Occult)
					{
						if (!Male)
						{
							Student.Cosmetic.ClubAccessories[(int)Student.Club].transform.localPosition = new Vector3(0f, -1.5f, 0.01f);
							Student.Cosmetic.ClubAccessories[(int)Student.Club].transform.localEulerAngles = Vector3.zero;
						}
						else
						{
							Student.Cosmetic.ClubAccessories[(int)Student.Club].transform.localPosition = new Vector3(0f, -1.42f, 0.005f);
							Student.Cosmetic.ClubAccessories[(int)Student.Club].transform.localEulerAngles = Vector3.zero;
						}
					}
				}
				gameObject.GetComponent<Renderer>().materials[0].mainTexture = Student.Cosmetic.FaceTexture;
				if (i == 0)
				{
					gameObject.transform.position += new Vector3(0f, 1f, 0f);
				}
				break;
			case 1:
				if (Student.Club == ClubType.Photography && Student.Cosmetic.ClubAccessories[(int)Student.Club] != null)
				{
					Student.Cosmetic.ClubAccessories[(int)Student.Club].transform.parent = gameObject.transform;
				}
				break;
			case 2:
				if (!Student.Male && Student.Cosmetic.Accessory == 6)
				{
					Student.Cosmetic.FemaleAccessories[Student.Cosmetic.Accessory].transform.parent = gameObject.transform;
				}
				break;
			}
		}
		if (BloodPoolSpawner.BloodParent == null)
		{
			BloodPoolSpawner.Start();
		}
		BloodPoolSpawner.SpawnBigPool();
		Police.BodyParts += 6;
		Yandere.NearBodies--;
		Police.Corpses--;
		base.gameObject.SetActive(false);
		Dismembered = true;
	}

	public void Remove()
	{
		if (AddingToCount)
		{
			Yandere.NearBodies--;
		}
		if (Poisoned)
		{
			Police.PoisonScene = false;
		}
		base.gameObject.SetActive(false);
	}

	public void DisableRigidbodies()
	{
		BloodPoolSpawner.gameObject.SetActive(false);
		for (int i = 0; i < AllRigidbodies.Length; i++)
		{
			if (AllRigidbodies[i].gameObject.GetComponent<CharacterJoint>() != null)
			{
				Object.Destroy(AllRigidbodies[i].gameObject.GetComponent<CharacterJoint>());
			}
			Object.Destroy(AllRigidbodies[i]);
			AllColliders[i].enabled = false;
		}
		Prompt.Hide();
		Prompt.enabled = false;
		base.enabled = false;
	}
}
