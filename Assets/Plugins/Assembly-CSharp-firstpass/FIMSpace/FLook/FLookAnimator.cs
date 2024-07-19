using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FLook
{
	public class FLookAnimator : MonoBehaviour
	{
		public enum EFAxisFixOrder
		{
			Parental = 0,
			FromBased = 1,
			FullManual = 2,
			ZYX = 3
		}

		protected enum EFHeadLookState
		{
			Null = 0,
			Following = 1,
			OutOfMaxRotation = 2,
			ClampedAngle = 3,
			OutOfMaxDistance = 4
		}

		private class CustomBackBone
		{
			public Transform transform;

			public int index;

			public CustomBackBone(Transform t, int i)
			{
				transform = t;
				index = i;
			}
		}

		[Tooltip("Object which will be main target of look by component, it will always return to look at it when using moment targets etc.")]
		public Transform ObjectToFollow;

		private Transform preObjectToFollow;

		[Tooltip("Head bone - head of look chain")]
		public Transform LeadBone;

		[Tooltip("Base root transform - object which moves / rotates")]
		public Transform BaseTransform;

		[Range(0.1f, 1.5f)]
		[Tooltip("Speed of look rotation animation change")]
		public float RotationSpeed = 1f;

		[Range(0f, 1f)]
		[Tooltip("This variable is making rotation animation become very smooth, but it can provide errors in some extreme values - Will be upgraded in future versions to prevent of happening this errors")]
		public float UltraSmoother;

		[Tooltip("For now it's toggle, but later it might be default option - Preventing from head rotating around when using high value of ultra smoother and target object going crazy around character")]
		public bool Prevent180Error;

		public int BackBonesCount;

		[HideInInspector]
		public Transform[] BackBonesTransforms;

		private List<CustomBackBone> customBackBones = new List<CustomBackBone>();

		[Tooltip("Making script start after first frame so initialization will not catch TPose initial bones rotations, which can cause some wrong offsets for rotations")]
		public bool StartAfterTPose = true;

		private int helpTPose = 1;

		[Tooltip("When you want use curve for more custom falloff or define it by simple slider")]
		public bool CurveSpread;

		[Tooltip("Configurable rotation weight placed over back bones - when you will use for example spine bones, here you can define how much will they rotate towards target in reference to other animated bones")]
		public AnimationCurve BackBonesFalloff = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		[Tooltip("Faloff value of how weight of animation should be spread over bones")]
		[Range(0f, 1f)]
		public float FaloffValue = 0.7f;

		[Tooltip("Max distance to target object to lost interest in it (0 = infinite range) when you have moment target after exceeding distance moment target will be forgotten")]
		public float MaximumDistance;

		[Header("Limits for rotation | Horizontal: X Vertical: Y")]
		public Vector2 XRotationLimits = new Vector2(-60f, 60f);

		[Range(0f, 45f)]
		[Tooltip("Making clamp ranges elastic, so when it starts to reach clamp value it slows like muscles needs more effort")]
		public float XElasticRange = 20f;

		public Vector2 YRotationLimits = new Vector2(-50f, 50f);

		[Range(0f, 45f)]
		[Tooltip("Making clamp ranges elastic, so when it starts to reach clamp value it slows like muscles needs more effort")]
		public float YElasticRange = 15f;

		[Header("Look forward if this angle is exceeded", order = 1)]
		[Range(25f, 180f)]
		[Tooltip("If target is too much after transform's back we smooth rotating head back to default animation's rotation")]
		public float MaxRotationDiffrence = 140f;

		[Range(25f, 70f)]
		[Tooltip("If head have to rotate more than this value it's animation speed for rotating increases, slight touch on detailing animation")]
		public float DeltaAccelerationRange = 50f;

		[Range(0.1f, 2f)]
		[Tooltip("If your character moves head too fast when loosing / changing target, here you can adjust it")]
		public float ChangeTargetSpeed = 1f;

		private Vector3 LookDirection = Vector3.forward;

		private Vector3 HeadUpDirection = Vector3.up;

		[Tooltip("Defines model specific bones orientation in order to fix Quaternion.LookRotation axis usage")]
		public Vector3 ManualFromAxis = Vector3.forward;

		public Vector3 ManualToAxis = Vector3.forward;

		public Vector3 FromAuto;

		public Vector3 OffsetAuto;

		public Vector3 ReferenceLookForward;

		public Vector3 ReferenceUp;

		public Vector3 DynamicReferenceUp;

		[Tooltip("With crazy flipped axes from models done in different modelling softwares, sometimes you have to change axes order for Quaternion.LookRotation to work correctly")]
		public EFAxisFixOrder FixingPreset;

		[Tooltip("Additional degrees of rotations for head look - for simple correction, sometimes you have just to rotate head in y axis by 90 degrees")]
		public Vector3 RotationOffset = new Vector3(0f, 0f, 0f);

		[Tooltip("Additional degrees of rotations for backones - for example when you have wolf and his neck is going up in comparison to keyfarmed animation")]
		public Vector3 BackBonesAddOffset = new Vector3(0f, 0f, 0f);

		[Tooltip("[ADVANCED] Axes multiplier for custom fixing flipped armature rotations")]
		public Vector3 RotCorrectionMultiplier = new Vector3(1f, 1f, 1f);

		[Tooltip("View debug rays in scene window")]
		public bool DebugRays;

		[Range(0f, 1f)]
		[Tooltip("Blend from look animator to keyframed animation")]
		public float BlendToOriginal;

		[Tooltip("If head look seems to be calculated like it is not looking from center of head but far from bottom or over it")]
		public Vector3 LookReferenceOffset;

		[Tooltip("Freezes reference start look position in x and z axes to avoid re-reaching max rotation limits when hips etc. are rotating in animation clip")]
		public bool AnchorReferencePoint = true;

		[Tooltip("In some cases you'll want to refresh anchor position during gameplay to make it more fitting to character's animation poses")]
		public bool RefreshAnchor = true;

		[Range(0f, 1f)]
		[Tooltip("Leading bone (most cases main head bone) should be rotated more hard to target, here you can controll it")]
		public float LeadBoneWeight = 1f;

		[Tooltip("Don't set hard rotations for bones, use animation rotation and add rotation offset to bones so animation's rotations are animated correctly (useful when using attack animations for example)")]
		public bool AnimateWithSource = true;

		[Tooltip("When using above action, we need to keep remembered rotations of animation clip from first frame, with monitoring we will remember root rotations from each new animation played")]
		public bool MonitorAnimator;

		[Header("If you don't want arms to be rotated when spine", order = 1)]
		[Header("bone is rotated by script (drag & drop here)", order = 3)]
		public List<Transform> CompensationBones;

		[Range(0f, 1f)]
		public float CompensationWeight = 0.5f;

		public bool CompensatePositions;

		public bool CompensatePositionsNotAnimated;

		[Range(0f, 3f)]
		[Tooltip("When you want create strange effects. This variable will overrotate animation intensity")]
		public float WeightsMultiplier = 1f;

		public int bonesNotAnimated;

		[Tooltip("Enabling laggy movement for head and delaying position")]
		public bool BirdMode;

		[Range(0f, 1f)]
		[Tooltip("Bird mode laggy movement for neck")]
		public float LagMovement = 0.85f;

		[Range(5f, 30f)]
		public float LaggySpeed = 14f;

		[Range(0.25f, 2f)]
		public float LagProgressSpeed = 1.15f;

		[Range(0.5f, 3f)]
		public float LagFrequency = 1.1f;

		[Range(0f, 1f)]
		[Tooltip("Bird mode keeping previous position until distance is reached")]
		public float DelayPosition;

		[Tooltip("How far distance to go back should have head to move (remind movement of pigeons to yourself)")]
		public float DelayMaxDistance = 0.25f;

		[Tooltip("How quick head and neck should go back to right position after reaching distance")]
		[Range(5f, 50f)]
		public float DelayGoSpeed = 15f;

		protected EFHeadLookState lookState;

		protected Transform lookStartReferenceTransform;

		private float localAnimationWeight;

		private float deltaLerpAccelerator = 1f;

		private Vector2 lastDeltaVector = Vector2.zero;

		private float holdGoBack;

		private float changeTargetSmootherWeight;

		private float changeTargetSmootherBones;

		private Quaternion headLerpRot;

		private Quaternion headLerpRotUltra;

		private Quaternion[] lerpRotations;

		private Quaternion[] lerpRotationsUltra;

		[Range(40f, 180f)]
		[Tooltip("Slowing down rotation to target when it moves rapidly and head have big angle to rotate toward it")]
		public float BigAngleSmoother = 60f;

		private Quaternion[] defautLocalRots;

		private bool wasMomentLookTransform;

		private Quaternion[] compensationRotations = new Quaternion[0];

		private Vector3[] compensationPositions = new Vector3[0];

		private bool targetExists;

		private bool smoothingOutOfMaxRange;

		private Quaternion targetLookRotation;

		private Quaternion[] newBonesRotations;

		private Quaternion[] poseReferenceRotations;

		private Quaternion[] lerpedReferenceRotations;

		private Quaternion mainReferenceRotation;

		private Quaternion lerpedMainReferenceRotation;

		private Quaternion[] staticRotations;

		private Quaternion[] hardTargetRoatations;

		private float lagTimer;

		private float lagProgress;

		private float lerpLagProgress;

		private Vector3[] initLocalPositions;

		private Vector3[] delayStartPositions;

		private Vector3[] delayCurrentPositions;

		private Vector3[] delayLerpPositions;

		private Quaternion[] lagStartRotations;

		private Quaternion[] lagTargetRotations;

		private Quaternion[] lagTargetRotationsLerp;

		private Quaternion[] preLagTargetRotations;

		protected Transform anchorHelper;

		private Animator animator;

		private int lastClipHash;

		protected bool initialized;

		public float[] RotationWeights { get; private set; }

		public Vector3 LastLookStartPosition { get; private set; }

		public Transform momentLookTransform { get; private set; }

		private void Reset()
		{
			FindBaseTransform();
		}

		private void Init()
		{
			if (initialized)
			{
				return;
			}
			if (LeadBone == null)
			{
				Debug.LogError(base.gameObject.name + " don't have assigned lead bone!");
				return;
			}
			lookState = EFHeadLookState.Null;
			momentLookTransform = null;
			CalculateBonesRotationsWeights();
			lookStartReferenceTransform = LeadBone.transform;
			headLerpRot = LeadBone.rotation;
			GetAdditionalBones();
			Quaternion rotation = BaseTransform.rotation;
			BaseTransform.rotation = Quaternion.identity;
			BaseTransform.rotation = rotation;
			defautLocalRots = new Quaternion[BackBonesTransforms.Length + 1];
			defautLocalRots[0] = LeadBone.localRotation;
			for (int i = 1; i < defautLocalRots.Length; i++)
			{
				defautLocalRots[i] = BackBonesTransforms[i - 1].localRotation;
			}
			animator = GetComponentInChildren<Animator>();
			staticRotations = new Quaternion[BackBonesCount + 1];
			mainReferenceRotation = BaseTransform.rotation;
			staticRotations[0] = LeadBone.localRotation;
			if (BackBonesCount > 0)
			{
				for (int j = 0; j < staticRotations.Length - 1; j++)
				{
					staticRotations[j + 1] = GetParentBone(j).localRotation;
				}
			}
			ComputeBonesRotationsFixVariables();
			if (CompensationBones == null)
			{
				CompensationBones = new List<Transform>();
			}
			anchorHelper = new GameObject(base.name + "-LookAnimator-AnchorHelper").transform;
			anchorHelper.SetParent(BaseTransform, true);
			anchorHelper.position = LeadBone.position;
			lagTimer = 0f;
			lagProgress = 1f;
			lerpLagProgress = 1f;
			initLocalPositions = new Vector3[BackBonesCount + 1];
			delayStartPositions = new Vector3[BackBonesCount + 1];
			delayLerpPositions = new Vector3[BackBonesCount + 1];
			delayCurrentPositions = new Vector3[BackBonesCount + 1];
			lagStartRotations = new Quaternion[BackBonesCount + 1];
			lagTargetRotations = new Quaternion[BackBonesCount + 1];
			lagTargetRotationsLerp = new Quaternion[BackBonesCount + 1];
			preLagTargetRotations = new Quaternion[BackBonesCount + 1];
			initLocalPositions[0] = LeadBone.localPosition;
			delayStartPositions[0] = LeadBone.position;
			delayLerpPositions[0] = delayStartPositions[0];
			delayCurrentPositions[0] = delayLerpPositions[0];
			lagTargetRotations[0] = LeadBone.rotation;
			lagTargetRotationsLerp[0] = LeadBone.rotation;
			preLagTargetRotations[0] = LeadBone.rotation;
			if (BackBonesCount > 0)
			{
				for (int k = 0; k < staticRotations.Length - 1; k++)
				{
					initLocalPositions[k + 1] = GetParentBone(k).localPosition;
					delayStartPositions[k + 1] = GetParentBone(k).position;
					delayLerpPositions[k + 1] = delayStartPositions[k + 1];
					delayCurrentPositions[k + 1] = delayLerpPositions[k + 1];
					lagStartRotations[k + 1] = GetParentBone(k).rotation;
					lagTargetRotations[k + 1] = GetParentBone(k).rotation;
					lagTargetRotationsLerp[k + 1] = GetParentBone(k).rotation;
					preLagTargetRotations[k + 1] = GetParentBone(k).rotation;
				}
			}
			if (AnimateWithSource)
			{
				for (int l = 0; l < lagStartRotations.Length - 1; l++)
				{
					lagStartRotations[l] = Quaternion.identity;
					lagTargetRotationsLerp[l] = lagStartRotations[l];
					lagTargetRotations[l] = lagStartRotations[l];
					preLagTargetRotations[l] = lagStartRotations[l];
				}
				lagStartRotations[newBonesRotations.Length - 1] = Quaternion.identity;
				lagTargetRotationsLerp[newBonesRotations.Length - 1] = lagStartRotations[newBonesRotations.Length - 1];
				lagTargetRotations[newBonesRotations.Length - 1] = lagStartRotations[newBonesRotations.Length - 1];
				preLagTargetRotations[newBonesRotations.Length - 1] = lagStartRotations[newBonesRotations.Length - 1];
			}
			initialized = true;
		}

		protected virtual void Start()
		{
			if (!StartAfterTPose)
			{
				Init();
			}
		}

		private void ComputeBonesRotationsFixVariables()
		{
			if (BaseTransform != null)
			{
				Quaternion rotation = BaseTransform.rotation;
				BaseTransform.rotation = Quaternion.identity;
				FromAuto = LeadBone.rotation * -Vector3.forward;
				float angle = Quaternion.Angle(Quaternion.identity, LeadBone.rotation);
				OffsetAuto = Quaternion.AngleAxis(angle, (LeadBone.rotation * Quaternion.Inverse(Quaternion.FromToRotation(FromAuto, LookDirection))).eulerAngles.normalized).eulerAngles;
				BaseTransform.rotation = rotation;
				ReferenceLookForward = Quaternion.Inverse(LeadBone.parent.rotation) * BaseTransform.rotation * LookDirection.normalized;
				ReferenceUp = Quaternion.Inverse(LeadBone.parent.rotation) * BaseTransform.rotation * HeadUpDirection.normalized;
			}
			else
			{
				Debug.LogWarning("Base Transform isn't defined, so we can't use auto correction!");
			}
		}

		private void Update()
		{
			if (!initialized)
			{
				if (StartAfterTPose)
				{
					helpTPose--;
					if (helpTPose < -1)
					{
						Init();
					}
				}
			}
			else if (RefreshAnchor)
			{
				helpTPose--;
				if (helpTPose < -6)
				{
					Quaternion rotation = BaseTransform.rotation;
					BaseTransform.rotation = Quaternion.identity;
					anchorHelper.position = LeadBone.position;
					RefreshAnchor = false;
					BaseTransform.rotation = rotation;
				}
			}
			else
			{
				helpTPose = -1;
			}
		}

		public virtual void LateUpdate()
		{
			if (!initialized || Time.deltaTime <= 0f)
			{
				return;
			}
			if (poseReferenceRotations == null)
			{
				RememberCurrentBonesRotations(true);
			}
			if (BlendToOriginal >= 1f && localAnimationWeight < 0.025f)
			{
				return;
			}
			if (localAnimationWeight < 0.025f && lookState != EFHeadLookState.OutOfMaxRotation && lookState != EFHeadLookState.OutOfMaxDistance)
			{
				ResetBonesLerps();
			}
			BasicCalculations();
			if (ObjectToFollow != preObjectToFollow && ObjectToFollow != null)
			{
				SmoothChangeTarget(0.2f, 0.3f, true);
			}
			if (MaximumDistance > 0f)
			{
				if ((bool)momentLookTransform)
				{
					float num = Vector3.Distance(LastLookStartPosition, momentLookTransform.position);
					if (num > MaximumDistance)
					{
						momentLookTransform = null;
					}
				}
				else if ((bool)ObjectToFollow)
				{
					if (lookState != EFHeadLookState.OutOfMaxDistance)
					{
						float num2 = Vector3.Distance(LastLookStartPosition, ObjectToFollow.transform.position);
						if (num2 > MaximumDistance)
						{
							SmoothChangeTarget(0.3f, 0.3f);
							lookState = EFHeadLookState.OutOfMaxDistance;
						}
					}
					else
					{
						float num3 = Vector3.Distance(LastLookStartPosition, ObjectToFollow.transform.position);
						if (num3 <= MaximumDistance)
						{
							lookState = EFHeadLookState.Null;
						}
					}
				}
			}
			if (lookState != EFHeadLookState.OutOfMaxDistance)
			{
				if (!targetExists && ObjectToFollow == null && lookState != 0)
				{
					SmoothChangeTarget(0.1f, 0.1f);
				}
				if (targetExists && ObjectToFollow == null)
				{
					targetExists = false;
					lookState = EFHeadLookState.Null;
					SmoothChangeTarget(0.35f, changeTargetSmootherBones);
				}
				if (!ObjectToFollow)
				{
					if (lookState != 0)
					{
						SmoothChangeTarget(0.75f, 0.2f, true);
						lookState = EFHeadLookState.Null;
					}
				}
				else if (lookState == EFHeadLookState.Null)
				{
					SmoothChangeTarget(0.4f, 0.3f, true);
				}
			}
			changeTargetSmootherWeight = Mathf.Min(1f, changeTargetSmootherWeight + Time.deltaTime * 0.6f);
			changeTargetSmootherBones = Mathf.Min(1f, changeTargetSmootherBones + Time.deltaTime * 0.6f);
			EFHeadLookState eFHeadLookState = lookState;
			CalculateLookAnimation();
			if (lookState == EFHeadLookState.ClampedAngle || lookState == EFHeadLookState.Following)
			{
				if (eFHeadLookState == EFHeadLookState.OutOfMaxRotation)
				{
					SmoothChangeTarget(0.1f, 0.3f, true);
				}
				localAnimationWeight = Mathf.Lerp(localAnimationWeight, 1.05f * (1f - BlendToOriginal), Time.deltaTime * 25f * RotationSpeed * changeTargetSmootherWeight * deltaLerpAccelerator);
			}
			else
			{
				localAnimationWeight = Mathf.Max(0f, Mathf.Lerp(localAnimationWeight, -0.02f, Time.deltaTime * 15f * RotationSpeed * changeTargetSmootherWeight));
			}
			ChangeBonesRotations();
			preObjectToFollow = ObjectToFollow;
			if (BirdMode)
			{
				CalculateBirdMode();
			}
		}

		private void BasicCalculations()
		{
			if (CompensationBones.Count > 0)
			{
				compensationRotations = new Quaternion[CompensationBones.Count];
				compensationPositions = new Vector3[CompensationBones.Count];
				for (int i = 0; i < CompensationBones.Count; i++)
				{
					if (CompensationBones[i] != null)
					{
						compensationRotations[i] = CompensationBones[i].rotation;
						compensationPositions[i] = CompensationBones[i].position;
					}
				}
			}
			if (AnimateWithSource && (bool)animator && MonitorAnimator)
			{
				if (!animator.IsInTransition(0))
				{
					int shortNameHash = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
					if (shortNameHash != lastClipHash)
					{
						RememberCurrentBonesRotations();
					}
					lastClipHash = shortNameHash;
				}
				for (int j = 0; j < lerpedReferenceRotations.Length; j++)
				{
					lerpedReferenceRotations[j] = Quaternion.Slerp(lerpedReferenceRotations[j], poseReferenceRotations[j], Time.deltaTime * 7f);
				}
				lerpedMainReferenceRotation = Quaternion.Slerp(lerpedMainReferenceRotation, mainReferenceRotation, Time.deltaTime * 7f);
			}
			if (bonesNotAnimated > 0)
			{
				LeadBone.localRotation = defautLocalRots[0];
				for (int k = 1; k < defautLocalRots.Length; k++)
				{
					BackBonesTransforms[k - 1].localRotation = defautLocalRots[k];
				}
			}
		}

		private void ChangeBonesRotations()
		{
			for (int i = 0; i < BackBonesTransforms.Length; i++)
			{
				GetParentBone(i).rotation = newBonesRotations[i];
			}
			LeadBone.rotation = newBonesRotations[newBonesRotations.Length - 1];
			if (!((float)compensationRotations.Length > 0f))
			{
				return;
			}
			float t = 0f;
			if (CompensatePositions)
			{
				t = CompensationWeight;
			}
			for (int j = 0; j < CompensationBones.Count; j++)
			{
				if (CompensationBones[j] != null)
				{
					CompensationBones[j].rotation = Quaternion.Slerp(CompensationBones[j].rotation, compensationRotations[j], CompensationWeight);
					CompensationBones[j].position = Vector3.Lerp(CompensationBones[j].position, compensationPositions[j], t);
				}
			}
		}

		private Vector3? GetTargetPosition()
		{
			Vector3? result = null;
			if ((bool)momentLookTransform)
			{
				result = momentLookTransform.position;
			}
			if (!momentLookTransform)
			{
				if (wasMomentLookTransform)
				{
					SmoothChangeTarget(0.1f, 0.1f, true);
					wasMomentLookTransform = false;
				}
				if ((bool)ObjectToFollow)
				{
					result = ObjectToFollow.position;
				}
			}
			return result;
		}

		private Vector3 CalculateTargetLookRotation(Vector3 lookPos)
		{
			Vector3 zero = Vector3.zero;
			Vector3 vector;
			if (!AnchorReferencePoint)
			{
				if (lookStartReferenceTransform == null)
				{
					if (BackBonesTransforms.Length > 0)
					{
						lookStartReferenceTransform = BackBonesTransforms[0];
					}
					else
					{
						lookStartReferenceTransform = LeadBone;
					}
				}
				vector = lookStartReferenceTransform.position + lookStartReferenceTransform.TransformVector(LookReferenceOffset);
			}
			else
			{
				vector = anchorHelper.position + BaseTransform.TransformVector(LookReferenceOffset);
			}
			LastLookStartPosition = vector;
			if (FixingPreset != 0)
			{
				return CalculateLimitationAndStuff(Quaternion.LookRotation(lookPos - vector).eulerAngles);
			}
			return LookRotationParental(vector, lookPos).eulerAngles;
		}

		private void CalculateLookAnimation()
		{
			Vector3? targetPosition = GetTargetPosition();
			Vector3 zero = Vector3.zero;
			Vector3 rotations = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			if (holdGoBack <= 0f)
			{
				if (targetPosition.HasValue)
				{
					zero = targetPosition.Value;
					rotations = CalculateTargetLookRotation(zero);
				}
			}
			else
			{
				holdGoBack -= Time.deltaTime;
				if (targetPosition.HasValue)
				{
					rotations = CalculateTargetLookRotation(LeadBone.transform.position + BaseTransform.forward * 5f);
				}
			}
			if (lookState != EFHeadLookState.OutOfMaxRotation)
			{
				rotations = ConvertFlippedAxes(rotations);
				targetLookRotation = Quaternion.Euler(rotations);
				quaternion = Quaternion.Euler(BackBonesAddOffset);
			}
			if (!AnimateWithSource)
			{
				newBonesRotations = new Quaternion[BackBonesTransforms.Length + 1];
				Quaternion quaternion2;
				if (UltraSmoother < 1f)
				{
					for (int i = 0; i < BackBonesTransforms.Length; i++)
					{
						Quaternion rotation = GetParentBone(i).rotation;
						quaternion2 = LinearInterpolateRotation(Quaternion.identity, Quaternion.Inverse(rotation) * targetLookRotation * quaternion, RotationWeights[i + 1] * WeightsMultiplier);
						hardTargetRoatations[i] = rotation * quaternion2;
						lerpRotations[i] = LinearInterpolateRotation(lerpRotations[i], quaternion2, Time.deltaTime * 20f * RotationSpeed * changeTargetSmootherBones);
						newBonesRotations[i] = LinearInterpolateRotation(rotation, rotation * lerpRotations[i], localAnimationWeight);
						if (BirdMode)
						{
							newBonesRotations[i] = LinearInterpolateRotation(rotation, Quaternion.Slerp(newBonesRotations[i], lagTargetRotationsLerp[i], LagMovement), localAnimationWeight);
						}
					}
					quaternion2 = LinearInterpolateRotation(Quaternion.identity, Quaternion.Inverse(LeadBone.rotation) * targetLookRotation, LeadBoneWeight);
					hardTargetRoatations[hardTargetRoatations.Length - 1] = LeadBone.rotation * quaternion2;
					float num = 1f;
					if (Prevent180Error)
					{
						if (FromAuto.z > 0.9f)
						{
							Vector3 eulerAngles = (headLerpRot * Quaternion.Inverse(BaseTransform.rotation)).eulerAngles;
							Vector3 eulerAngles2 = (LeadBone.rotation * quaternion2 * Quaternion.Inverse(BaseTransform.rotation)).eulerAngles;
							float t = Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones;
							headLerpRot = Quaternion.Euler(Mathf.LerpAngle(eulerAngles.x, eulerAngles2.x, t), Mathf.Lerp(eulerAngles.y, eulerAngles2.y, t), Mathf.LerpAngle(eulerAngles.z, eulerAngles2.z, t)) * BaseTransform.rotation;
						}
						else
						{
							float t2 = Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones;
							Vector3 vector = new Vector3(0f, BaseTransform.eulerAngles.y, 0f);
							vector.y = vector.y - 90f - vector.y * 2f;
							Vector3 vector2 = headLerpRot.eulerAngles + vector;
							Vector3 vector3 = (LeadBone.rotation * quaternion2).eulerAngles + vector;
							Vector3 vector4 = new Vector3(Mathf.LerpAngle(vector2.x, vector3.x, t2), Mathf.Lerp(AnglePositive(vector2.y), AnglePositive(vector3.y), t2), Mathf.LerpAngle(vector2.z, vector3.z, t2));
							headLerpRot = Quaternion.Euler(vector4 - vector);
						}
					}
					else
					{
						headLerpRot = LinearInterpolateRotation(headLerpRot, LeadBone.rotation * quaternion2, Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones * num);
					}
					newBonesRotations[newBonesRotations.Length - 1] = LinearInterpolateRotation(LeadBone.rotation, headLerpRot, localAnimationWeight);
					if (BirdMode)
					{
						newBonesRotations[newBonesRotations.Length - 1] = LinearInterpolateRotation(LeadBone.rotation, Quaternion.Slerp(newBonesRotations[newBonesRotations.Length - 1], lagTargetRotationsLerp[newBonesRotations.Length - 1], LagMovement), localAnimationWeight);
					}
				}
				if (!(UltraSmoother > 0f))
				{
					return;
				}
				for (int j = 0; j < BackBonesTransforms.Length; j++)
				{
					Quaternion rotation2 = GetParentBone(j).rotation;
					quaternion2 = LinearInterpolateRotation(rotation2, targetLookRotation * quaternion, RotationWeights[j + 1] * WeightsMultiplier);
					hardTargetRoatations[j] = rotation2 * quaternion2;
					lerpRotationsUltra[j] = LinearInterpolateRotation(lerpRotationsUltra[j], quaternion2, Time.deltaTime * 20f * RotationSpeed * changeTargetSmootherBones);
					if (UltraSmoother < 1f)
					{
						newBonesRotations[j] = LinearInterpolateRotation(newBonesRotations[j], LinearInterpolateRotation(rotation2, lerpRotationsUltra[j], localAnimationWeight), UltraSmoother);
					}
					else
					{
						newBonesRotations[j] = LinearInterpolateRotation(newBonesRotations[j], lerpRotationsUltra[j], UltraSmoother);
					}
				}
				quaternion2 = LinearInterpolateRotation(LeadBone.rotation, targetLookRotation, LeadBoneWeight);
				hardTargetRoatations[hardTargetRoatations.Length - 1] = LeadBone.rotation * quaternion2;
				float num2 = 1f;
				if (Prevent180Error)
				{
					if (FromAuto.z > 0.9f)
					{
						Vector3 eulerAngles3 = (headLerpRotUltra * Quaternion.Inverse(BaseTransform.rotation)).eulerAngles;
						Vector3 eulerAngles4 = (quaternion2 * Quaternion.Inverse(BaseTransform.rotation)).eulerAngles;
						float t3 = Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones;
						headLerpRotUltra = Quaternion.Euler(Mathf.LerpAngle(eulerAngles3.x, eulerAngles4.x, t3), Mathf.Lerp(eulerAngles3.y, eulerAngles4.y, t3), Mathf.LerpAngle(eulerAngles3.z, eulerAngles4.z, t3)) * BaseTransform.rotation;
					}
					else
					{
						float t4 = Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones;
						Vector3 vector5 = new Vector3(0f, BaseTransform.eulerAngles.y, 0f);
						vector5.y = vector5.y - 90f - vector5.y * 2f;
						Vector3 vector6 = headLerpRotUltra.eulerAngles + vector5;
						Vector3 vector7 = quaternion2.eulerAngles + vector5;
						Vector3 vector8 = new Vector3(Mathf.LerpAngle(vector6.x, vector7.x, t4), Mathf.Lerp(AnglePositive(vector6.y), AnglePositive(vector7.y), t4), Mathf.LerpAngle(vector6.z, vector7.z, t4));
						headLerpRotUltra = Quaternion.Euler(vector8 - vector5);
					}
				}
				else
				{
					headLerpRotUltra = Quaternion.Lerp(headLerpRotUltra, quaternion2, Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones * num2);
				}
				if (UltraSmoother < 1f)
				{
					Quaternion end = LinearInterpolateRotation(LeadBone.rotation, headLerpRotUltra, localAnimationWeight);
					newBonesRotations[newBonesRotations.Length - 1] = LinearInterpolateRotation(newBonesRotations[newBonesRotations.Length - 1], end, UltraSmoother * 0.5f);
				}
				else
				{
					newBonesRotations[newBonesRotations.Length - 1] = LinearInterpolateRotation(LeadBone.rotation, headLerpRotUltra, localAnimationWeight);
				}
				return;
			}
			newBonesRotations = new Quaternion[BackBonesTransforms.Length + 1];
			Quaternion quaternion3 = BaseTransform.rotation * Quaternion.Inverse(lerpedMainReferenceRotation);
			Quaternion rotation4;
			Quaternion quaternion4;
			if (UltraSmoother < 1f)
			{
				for (int num3 = BackBonesTransforms.Length - 1; num3 >= 0; num3--)
				{
					Quaternion rotation3 = GetParentBone(num3).rotation;
					rotation4 = quaternion3 * lerpedReferenceRotations[num3 + 1];
					quaternion4 = LinearInterpolateRotation(Quaternion.identity, targetLookRotation * quaternion * Quaternion.Inverse(rotation4), RotationWeights[num3 + 1] * WeightsMultiplier);
					hardTargetRoatations[num3] = quaternion4;
					lerpRotations[num3] = LinearInterpolateRotation(lerpRotations[num3], quaternion4, Time.deltaTime * 20f * RotationSpeed * changeTargetSmootherBones);
					newBonesRotations[num3] = LinearInterpolateRotation(rotation3, lerpRotations[num3] * rotation3, localAnimationWeight);
					if (BirdMode)
					{
						newBonesRotations[num3] = LinearInterpolateRotation(rotation3, Quaternion.Slerp(newBonesRotations[num3], lagTargetRotationsLerp[num3] * rotation3, LagMovement), localAnimationWeight);
					}
				}
				float num4 = 1f;
				rotation4 = quaternion3 * lerpedReferenceRotations[0];
				quaternion4 = LinearInterpolateRotation(Quaternion.identity, targetLookRotation * Quaternion.Inverse(rotation4), LeadBoneWeight);
				hardTargetRoatations[hardTargetRoatations.Length - 1] = quaternion4;
				if (Prevent180Error)
				{
					if (FromAuto.z > 0.9f)
					{
						Vector3 eulerAngles5 = headLerpRot.eulerAngles;
						Vector3 eulerAngles6 = quaternion4.eulerAngles;
						float t5 = Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones;
						Vector3 euler = new Vector3(Mathf.LerpAngle(eulerAngles5.x, eulerAngles6.x, t5), Mathf.Lerp(FLogicMethods.WrapAngle(eulerAngles5.y), FLogicMethods.WrapAngle(eulerAngles6.y), t5), Mathf.LerpAngle(eulerAngles5.z, eulerAngles6.z, t5));
						headLerpRot = Quaternion.Euler(euler);
					}
					else
					{
						float t6 = Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones;
						Vector3 vector9 = new Vector3(0f, 180f, 0f);
						Vector3 vector10 = headLerpRot.eulerAngles + vector9;
						Vector3 vector11 = quaternion4.eulerAngles + vector9;
						Vector3 vector12 = new Vector3(Mathf.LerpAngle(vector10.x, vector11.x, t6), Mathf.Lerp(AnglePositive(vector10.y), AnglePositive(vector11.y), t6), Mathf.LerpAngle(vector10.z, vector11.z, t6));
						headLerpRot = Quaternion.Euler(vector12 - vector9);
					}
				}
				else
				{
					headLerpRot = LinearInterpolateRotation(headLerpRot, quaternion4, Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones * num4);
				}
				newBonesRotations[newBonesRotations.Length - 1] = LinearInterpolateRotation(LeadBone.rotation, headLerpRot * LeadBone.rotation, localAnimationWeight);
				if (BirdMode)
				{
					newBonesRotations[newBonesRotations.Length - 1] = LinearInterpolateRotation(LeadBone.rotation, Quaternion.Slerp(newBonesRotations[newBonesRotations.Length - 1], lagTargetRotationsLerp[newBonesRotations.Length - 1] * LeadBone.rotation, LagMovement), localAnimationWeight);
				}
			}
			if (!(UltraSmoother > 0f))
			{
				return;
			}
			Quaternion quaternion5;
			Quaternion quaternion6;
			for (int num5 = BackBonesTransforms.Length - 1; num5 >= 0; num5--)
			{
				Quaternion rotation5 = GetParentBone(num5).rotation;
				rotation4 = quaternion3 * lerpedReferenceRotations[num5 + 1];
				quaternion4 = LinearInterpolateRotation(rotation4, targetLookRotation * quaternion, RotationWeights[num5 + 1] * WeightsMultiplier);
				quaternion5 = quaternion4 * Quaternion.Inverse(rotation4);
				quaternion6 = quaternion5 * rotation5;
				hardTargetRoatations[num5] = quaternion6;
				lerpRotationsUltra[num5] = LinearInterpolateRotation(lerpRotationsUltra[num5], quaternion6, Time.deltaTime * 20f * RotationSpeed * changeTargetSmootherBones);
				if (UltraSmoother < 1f)
				{
					newBonesRotations[num5] = LinearInterpolateRotation(newBonesRotations[num5], LinearInterpolateRotation(rotation5, lerpRotationsUltra[num5], localAnimationWeight), UltraSmoother);
				}
				else
				{
					newBonesRotations[num5] = LinearInterpolateRotation(rotation5, lerpRotationsUltra[num5], UltraSmoother);
				}
			}
			rotation4 = quaternion3 * lerpedReferenceRotations[0];
			quaternion4 = LinearInterpolateRotation(rotation4, targetLookRotation, LeadBoneWeight);
			quaternion5 = quaternion4 * Quaternion.Inverse(rotation4);
			quaternion6 = quaternion5 * LeadBone.rotation;
			hardTargetRoatations[hardTargetRoatations.Length - 1] = quaternion6;
			float num6 = 1f;
			if (Prevent180Error)
			{
				if (FromAuto.z > 0.9f)
				{
					Vector3 eulerAngles7 = (headLerpRotUltra * Quaternion.Inverse(BaseTransform.rotation)).eulerAngles;
					Vector3 eulerAngles8 = (quaternion6 * Quaternion.Inverse(BaseTransform.rotation)).eulerAngles;
					float t7 = Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones;
					headLerpRotUltra = Quaternion.Euler(Mathf.LerpAngle(eulerAngles7.x, eulerAngles8.x, t7), Mathf.Lerp(eulerAngles7.y, eulerAngles8.y, t7), Mathf.LerpAngle(eulerAngles7.z, eulerAngles8.z, t7)) * BaseTransform.rotation;
				}
				else
				{
					float t8 = Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones;
					Vector3 vector13 = new Vector3(0f, BaseTransform.eulerAngles.y, 0f);
					vector13.y = vector13.y - 90f - vector13.y * 2f;
					Vector3 vector14 = headLerpRotUltra.eulerAngles + vector13;
					Vector3 vector15 = quaternion6.eulerAngles + vector13;
					Vector3 vector16 = new Vector3(Mathf.LerpAngle(vector14.x, vector15.x, t8), Mathf.Lerp(AnglePositive(vector14.y), AnglePositive(vector15.y), t8), Mathf.LerpAngle(vector14.z, vector15.z, t8));
					headLerpRotUltra = Quaternion.Euler(vector16 - vector13);
				}
			}
			else
			{
				headLerpRotUltra = LinearInterpolateRotation(headLerpRotUltra, quaternion6, Time.deltaTime * 10f * RotationSpeed * changeTargetSmootherBones * num6);
			}
			if (UltraSmoother < 1f)
			{
				Quaternion end2 = LinearInterpolateRotation(LeadBone.rotation, headLerpRotUltra, localAnimationWeight);
				newBonesRotations[newBonesRotations.Length - 1] = LinearInterpolateRotation(newBonesRotations[newBonesRotations.Length - 1], end2, UltraSmoother * 0.5f);
			}
			else
			{
				newBonesRotations[newBonesRotations.Length - 1] = LinearInterpolateRotation(LeadBone.rotation, headLerpRotUltra, localAnimationWeight);
			}
		}

		private Vector3 WrapVector(Vector3 v)
		{
			return new Vector3(FLogicMethods.WrapAngle(v.x), FLogicMethods.WrapAngle(v.y), FLogicMethods.WrapAngle(v.z));
		}

		private Quaternion LookRotationParental(Vector3 startLookPosition, Vector3 targetPosition)
		{
			if (!AnimateWithSource)
			{
				LeadBone.localRotation = staticRotations[0];
				for (int i = 0; i < BackBonesTransforms.Length; i++)
				{
					BackBonesTransforms[i].localRotation = staticRotations[i + 1];
				}
			}
			Vector3 vector = Quaternion.Inverse(LeadBone.parent.rotation) * (targetPosition - startLookPosition).normalized;
			float y = AngleAroundAxis(ReferenceLookForward, vector, ReferenceUp);
			Vector3 axis = Vector3.Cross(ReferenceUp, vector);
			Vector3 firstDirection = vector - Vector3.Project(vector, ReferenceUp);
			float x = AngleAroundAxis(firstDirection, vector, axis);
			Vector3 vector2 = CalculateLimitationAndStuff(new Vector2(x, y));
			x = vector2.x;
			y = vector2.y;
			Vector3 axis2 = Vector3.Cross(ReferenceUp, ReferenceLookForward);
			vector = Quaternion.AngleAxis(y, ReferenceUp) * Quaternion.AngleAxis(x, axis2) * ReferenceLookForward;
			Vector3 tangent = ReferenceUp;
			Vector3.OrthoNormalize(ref vector, ref tangent);
			Vector3 normal = vector;
			DynamicReferenceUp = tangent;
			Vector3.OrthoNormalize(ref normal, ref DynamicReferenceUp);
			Quaternion quaternion = LeadBone.parent.rotation * Quaternion.LookRotation(normal, DynamicReferenceUp);
			quaternion *= Quaternion.Inverse(LeadBone.parent.rotation * Quaternion.LookRotation(ReferenceLookForward, ReferenceUp));
			quaternion = LeadBone.parent.rotation * Quaternion.LookRotation(normal, DynamicReferenceUp);
			quaternion *= Quaternion.Inverse(LeadBone.parent.rotation * Quaternion.LookRotation(ReferenceLookForward, ReferenceUp));
			return quaternion * LeadBone.rotation;
		}

		private void CalculateBirdMode()
		{
			UltraSmoother = 0f;
			float num = Quaternion.Angle(lagTargetRotationsLerp[newBonesRotations.Length - 1], hardTargetRoatations[hardTargetRoatations.Length - 1]);
			float num2 = ((!(num < 60f)) ? Mathf.Lerp(1.5f, 3f, Mathf.InverseLerp(50f, 110f, num)) : Mathf.Lerp(1f, 1.5f, Mathf.InverseLerp(30f, 50f, num)));
			lagTimer -= Time.deltaTime * num2;
			lerpLagProgress = Mathf.Lerp(lerpLagProgress, lagProgress, Time.deltaTime * LaggySpeed);
			if (lagProgress > 0f)
			{
				if (lagTimer < 0f)
				{
					lagProgress -= UnityEngine.Random.Range(0.4f, 0.8f) * Time.deltaTime * 50f * LagProgressSpeed;
				}
			}
			else if (lagProgress <= 0f && lagTimer < 0f)
			{
				ResetBirdTargeting();
			}
			if (lagTimer < 0f)
			{
				lagTimer = UnityEngine.Random.Range(0.15f, 0.35f) / LagFrequency;
			}
			for (int i = 0; i < lagTargetRotationsLerp.Length - 1; i++)
			{
				lagTargetRotationsLerp[i] = Quaternion.Slerp(lagTargetRotationsLerp[i], Quaternion.Slerp(lagStartRotations[i], lagTargetRotations[i], 1f - lagProgress), Time.deltaTime * LaggySpeed);
			}
			lagTargetRotationsLerp[newBonesRotations.Length - 1] = Quaternion.Slerp(lagTargetRotationsLerp[newBonesRotations.Length - 1], Quaternion.Slerp(lagStartRotations[newBonesRotations.Length - 1], lagTargetRotations[newBonesRotations.Length - 1], 1f - lagProgress), Time.deltaTime * LaggySpeed);
			if (!(DelayPosition > 0f))
			{
				return;
			}
			LeadBone.localPosition = initLocalPositions[0];
			for (int j = 1; j < initLocalPositions.Length; j++)
			{
				BackBonesTransforms[j - 1].transform.localPosition = initLocalPositions[j];
			}
			float num3 = Vector3.Distance(Vector3.Scale(delayStartPositions[0], new Vector3(1f, 0f, 1f)), Vector3.Scale(LeadBone.position, new Vector3(1f, 0f, 1f)));
			float num4 = Mathf.Abs(delayStartPositions[0].y - LeadBone.position.y);
			if (num3 > DelayMaxDistance || num4 > DelayMaxDistance / 1.65f)
			{
				delayStartPositions[0] = LeadBone.position;
				for (int k = 1; k < initLocalPositions.Length; k++)
				{
					delayStartPositions[k] = BackBonesTransforms[k - 1].position;
				}
			}
			for (int l = 0; l < delayLerpPositions.Length; l++)
			{
				delayLerpPositions[l] = Vector3.Lerp(delayLerpPositions[l], delayStartPositions[l], Time.deltaTime * DelayGoSpeed);
			}
			Vector3[] array = new Vector3[BackBonesTransforms.Length + 1];
			for (int m = 1; m < array.Length; m++)
			{
				array[m - 1] = BackBonesTransforms[m - 1].position;
			}
			array[0] = LeadBone.position;
			for (int n = 0; n < BackBonesTransforms.Length; n++)
			{
				BackBonesTransforms[n].position = Vector3.Lerp(array[n], delayLerpPositions[n + 1], RotationWeights[n] * DelayPosition * localAnimationWeight);
				delayCurrentPositions[n + 1] = BackBonesTransforms[n].position;
			}
			LeadBone.position = Vector3.Lerp(array[0], delayLerpPositions[0], DelayPosition * localAnimationWeight);
			delayCurrentPositions[0] = LeadBone.position;
		}

		private void ResetBirdTargeting()
		{
			lagProgress = 1f;
			lagTimer = -0.1f;
			lerpLagProgress = 1f;
			if (!AnimateWithSource)
			{
				for (int i = 0; i < lagStartRotations.Length - 1; i++)
				{
					lagStartRotations[i] = lagTargetRotations[i];
				}
				lagStartRotations[newBonesRotations.Length - 1] = lagTargetRotations[newBonesRotations.Length - 1];
			}
			else
			{
				for (int j = 0; j < lagStartRotations.Length - 1; j++)
				{
					lagStartRotations[j] = lagTargetRotations[j];
				}
				lagStartRotations[newBonesRotations.Length - 1] = lagTargetRotations[newBonesRotations.Length - 1];
			}
			for (int k = 0; k < lagTargetRotations.Length - 1; k++)
			{
				lagTargetRotations[k] = hardTargetRoatations[k];
			}
			lagTargetRotations[newBonesRotations.Length - 1] = hardTargetRoatations[newBonesRotations.Length - 1];
		}

		private Vector2 CalculateLimitationAndStuff(Vector3 angles)
		{
			if (lookState == EFHeadLookState.OutOfMaxDistance)
			{
				return Vector2.zero;
			}
			Vector3 vector = angles;
			Vector3 vector2 = BaseTransform.rotation.eulerAngles;
			if (FixingPreset == EFAxisFixOrder.Parental)
			{
				vector2 = Vector3.zero;
			}
			Vector2 vector3 = new Vector3(Mathf.DeltaAngle(vector.x, vector2.x), Mathf.DeltaAngle(vector.y, vector2.y));
			float num = Mathf.Abs(vector3.y);
			float num2 = MaxRotationDiffrence;
			if (Mathf.Abs(XRotationLimits.x) > MaxRotationDiffrence)
			{
				num2 = Mathf.Abs(XRotationLimits.x);
			}
			if (Mathf.Abs(XRotationLimits.y) > MaxRotationDiffrence)
			{
				num2 = Mathf.Abs(XRotationLimits.y);
			}
			bool flag = false;
			if (Prevent180Error && Mathf.Sign(lastDeltaVector.y) != Mathf.Sign(vector3.y) && Mathf.Abs(lastDeltaVector.y) > Mathf.Max(90f, MaxRotationDiffrence))
			{
				lookState = EFHeadLookState.OutOfMaxRotation;
				targetLookRotation = LeadBone.rotation;
				holdGoBack = 0.135f;
				flag = true;
			}
			if (!flag)
			{
				if (num > num2)
				{
					lookState = EFHeadLookState.OutOfMaxRotation;
					targetLookRotation = LeadBone.rotation;
				}
				else
				{
					if (lookState == EFHeadLookState.OutOfMaxRotation)
					{
						SmoothChangeTarget(0.5f, 0.3f, true);
					}
					lookState = EFHeadLookState.Following;
					smoothingOutOfMaxRange = false;
				}
			}
			if (num > DeltaAccelerationRange)
			{
				deltaLerpAccelerator = Mathf.Lerp(1f, 1.5f, Mathf.InverseLerp(DeltaAccelerationRange, num2, num));
			}
			if (lookState == EFHeadLookState.Following)
			{
				Vector2 vector4 = lastDeltaVector - vector3;
				if (Mathf.Abs(vector4.y) > BigAngleSmoother)
				{
					float num3 = Mathf.Lerp(0.55f, 0.3f, Mathf.InverseLerp(BigAngleSmoother, 180f, vector4.y)) * 0.8f;
					SmoothChangeTarget(num3 * 0.9f, num3, true);
				}
				if (vector3.x > YRotationLimits.y)
				{
					float num4 = 0f;
					if (YElasticRange > 0f)
					{
						num4 = Mathf.Abs(vector3.x - YRotationLimits.y);
						num4 = FEasing.EaseOutCubic(0f, YElasticRange, num4 / (180f - YRotationLimits.y));
					}
					vector.x = vector2.x - YRotationLimits.y - num4;
					lookState = EFHeadLookState.ClampedAngle;
				}
				else if (vector3.x < YRotationLimits.x)
				{
					float num5 = 0f;
					if (YElasticRange > 0f)
					{
						num5 = Mathf.Abs(vector3.x - YRotationLimits.x);
						num5 = FEasing.EaseOutCubic(0f, YElasticRange, num5 / (180f - YRotationLimits.x));
					}
					vector.x = vector2.x - YRotationLimits.x + num5;
					lookState = EFHeadLookState.ClampedAngle;
				}
				if (vector3.y > 0f - XRotationLimits.x)
				{
					float num6 = 0f;
					if (XElasticRange > 0f)
					{
						num6 = Mathf.Abs(vector3.y + XRotationLimits.x);
						num6 = FEasing.EaseOutCubic(0f, XElasticRange, num6 / (180f + XRotationLimits.x));
					}
					vector.y = vector2.y - XRotationLimits.y - num6;
					lookState = EFHeadLookState.ClampedAngle;
				}
				else if (vector3.y < 0f - XRotationLimits.y)
				{
					float num7 = 0f;
					if (XElasticRange > 0f)
					{
						num7 = Mathf.Abs(vector3.y + XRotationLimits.y);
						num7 = FEasing.EaseOutCubic(0f, XElasticRange, num7 / (180f + XRotationLimits.y));
					}
					vector.y = vector2.y + XRotationLimits.y + num7;
					lookState = EFHeadLookState.ClampedAngle;
				}
			}
			else if (!smoothingOutOfMaxRange)
			{
				smoothingOutOfMaxRange = true;
				SmoothChangeTarget(0.1f, 0.1f, true);
			}
			lastDeltaVector = vector3;
			return vector;
		}

		public float AnglePositive(float angle)
		{
			angle %= 360f;
			if (angle < 0f)
			{
				angle += 360f;
			}
			return angle;
		}

		public void SwitchLooking(bool? enableLooking = null, float transitionTime = 0.2f, Action callback = null)
		{
			bool flag = true;
			if (!enableLooking.HasValue)
			{
				if (BlendToOriginal < 0.5f)
				{
					flag = false;
				}
			}
			else if (enableLooking == false)
			{
				flag = false;
			}
			if (flag)
			{
				localAnimationWeight = 0f;
				SmoothChangeTarget(0.3f, 1f);
				newBonesRotations = new Quaternion[lerpRotations.Length + 1];
				for (int i = 0; i < lerpRotations.Length; i++)
				{
					lerpRotations[i] = GetParentBone(i).rotation;
					newBonesRotations[i] = lerpRotations[i];
				}
				headLerpRot = LeadBone.rotation;
				for (int j = 0; j < lerpRotationsUltra.Length; j++)
				{
					lerpRotationsUltra[j] = GetParentBone(j).rotation;
				}
				headLerpRot = LeadBone.rotation;
				newBonesRotations[newBonesRotations.Length - 1] = headLerpRot;
			}
			StopAllCoroutines();
			StartCoroutine(SwitchLookingTransition(transitionTime, flag, callback));
		}

		public void SetLookTarget(Transform transform)
		{
			ObjectToFollow = transform;
			momentLookTransform = null;
		}

		public GameObject SetMomentLookTarget(Transform parent = null, Vector3? position = null, float? destroyTimer = 3f)
		{
			GameObject gameObject = new GameObject(base.transform.gameObject.name + "-MomentLookTarget");
			if (parent != null)
			{
				gameObject.transform.SetParent(parent);
				if (position.HasValue)
				{
					gameObject.transform.localPosition = position.Value;
				}
				else
				{
					gameObject.transform.localPosition = Vector3.zero;
				}
			}
			else if (position.HasValue)
			{
				gameObject.transform.position = position.Value;
			}
			momentLookTransform = gameObject.transform;
			wasMomentLookTransform = true;
			SmoothChangeTarget(0.1f, 0.1f, true);
			if (destroyTimer.HasValue)
			{
				UnityEngine.Object.Destroy(gameObject, destroyTimer.Value);
			}
			return gameObject;
		}

		public void ForceDestroyMomentTarget()
		{
			if ((bool)momentLookTransform)
			{
				UnityEngine.Object.Destroy(momentLookTransform.gameObject);
			}
		}

		public void SmoothChangeTarget(float value, float bonesSmoother = 0.8f, bool overrideIfSlower = false)
		{
			if (BirdMode)
			{
				changeTargetSmootherWeight = 0.8f;
				changeTargetSmootherBones = 0.9f;
				return;
			}
			if (!overrideIfSlower)
			{
				changeTargetSmootherWeight = value * ChangeTargetSpeed;
				changeTargetSmootherBones = bonesSmoother * ChangeTargetSpeed;
				return;
			}
			float num = changeTargetSmootherWeight;
			if (changeTargetSmootherWeight > value)
			{
				num = value;
			}
			float num2 = changeTargetSmootherBones;
			if (changeTargetSmootherBones > bonesSmoother)
			{
				num2 = bonesSmoother;
			}
			changeTargetSmootherWeight = num * ChangeTargetSpeed;
			changeTargetSmootherBones = num2 * ChangeTargetSpeed;
		}

		public void RememberCurrentBonesRotations(bool resetLerpRotations = false)
		{
			if (resetLerpRotations)
			{
				poseReferenceRotations = new Quaternion[BackBonesCount + 1];
				lerpedReferenceRotations = new Quaternion[poseReferenceRotations.Length];
			}
			if (poseReferenceRotations == null)
			{
				poseReferenceRotations = new Quaternion[BackBonesCount + 1];
			}
			if (lerpedReferenceRotations == null)
			{
				lerpedReferenceRotations = new Quaternion[poseReferenceRotations.Length];
			}
			mainReferenceRotation = BaseTransform.rotation;
			poseReferenceRotations[0] = LeadBone.rotation;
			if (BackBonesCount > 0)
			{
				for (int i = 0; i < poseReferenceRotations.Length - 1; i++)
				{
					poseReferenceRotations[i + 1] = GetParentBone(i).rotation;
				}
			}
			if (resetLerpRotations)
			{
				for (int j = 0; j < lerpedReferenceRotations.Length; j++)
				{
					lerpedReferenceRotations[j] = poseReferenceRotations[j];
				}
				lerpedMainReferenceRotation = mainReferenceRotation;
			}
		}

		private Vector3 ConvertFlippedAxes(Vector3 rotations)
		{
			rotations += RotationOffset;
			if (FixingPreset != 0)
			{
				if (FixingPreset == EFAxisFixOrder.FromBased)
				{
					rotations += OffsetAuto;
					rotations = (Quaternion.Euler(rotations) * Quaternion.FromToRotation(FromAuto, LookDirection)).eulerAngles;
				}
				else
				{
					if (FixingPreset == EFAxisFixOrder.FullManual)
					{
						rotations.x *= RotCorrectionMultiplier.x;
						rotations.y *= RotCorrectionMultiplier.y;
						rotations.z *= RotCorrectionMultiplier.z;
						return (Quaternion.Euler(rotations) * Quaternion.FromToRotation(ManualFromAxis, ManualToAxis)).eulerAngles;
					}
					if (FixingPreset == EFAxisFixOrder.ZYX)
					{
						return Quaternion.Euler(rotations.z, rotations.y - 90f, 0f - rotations.x - 90f).eulerAngles;
					}
				}
			}
			return rotations;
		}

		public static float AngleAroundAxis(Vector3 firstDirection, Vector3 secondDirection, Vector3 axis)
		{
			firstDirection -= Vector3.Project(firstDirection, axis);
			secondDirection -= Vector3.Project(secondDirection, axis);
			float num = Vector3.Angle(firstDirection, secondDirection);
			return num * (float)((!(Vector3.Dot(axis, Vector3.Cross(firstDirection, secondDirection)) < 0f)) ? 1 : (-1));
		}

		private Transform GetParentBone(int index)
		{
			if (BackBonesTransforms.Length == 0)
			{
				return null;
			}
			if (index < 0)
			{
				index = 0;
			}
			if (index > BackBonesTransforms.Length - 1)
			{
				index = BackBonesTransforms.Length - 1;
			}
			return BackBonesTransforms[index];
		}

		private void GetAdditionalBones(bool resetRotation = false)
		{
			if (LeadBone == null)
			{
				if (base.gameObject.activeInHierarchy && Application.isPlaying)
				{
					Debug.LogError("Assign lead bone first! (" + base.transform.name + ")");
				}
				return;
			}
			for (int i = 0; i < BackBonesTransforms.Length; i++)
			{
				if (i == 0)
				{
					BackBonesTransforms[i] = LeadBone.parent;
					continue;
				}
				if ((bool)BackBonesTransforms[i - 1].parent)
				{
					BackBonesTransforms[i] = BackBonesTransforms[i - 1].parent;
					continue;
				}
				Debug.LogError("Error during getting parents of LookTarget! (no parent?)");
				return;
			}
			for (int num = customBackBones.Count - 1; num >= 0; num--)
			{
				if (customBackBones[num].index < BackBonesTransforms.Length)
				{
					BackBonesTransforms[customBackBones[num].index] = customBackBones[num].transform;
				}
				else
				{
					customBackBones.RemoveAt(num);
				}
			}
			if (!resetRotation)
			{
				ResetBonesLerps(true);
			}
			else
			{
				ResetBonesLerps();
			}
		}

		private void ResetBonesLerps(bool onlyIfNull = false)
		{
			bool flag = true;
			if (onlyIfNull && lerpRotations != null)
			{
				flag = false;
			}
			if (flag)
			{
				lerpRotations = new Quaternion[BackBonesTransforms.Length];
				for (int i = 0; i < lerpRotations.Length; i++)
				{
					lerpRotations[i] = Quaternion.identity;
				}
				if (AnimateWithSource)
				{
					headLerpRot = Quaternion.identity;
				}
				else
				{
					headLerpRot = LeadBone.rotation;
				}
				hardTargetRoatations = new Quaternion[BackBonesTransforms.Length + 1];
				newBonesRotations = new Quaternion[BackBonesTransforms.Length + 1];
				for (int j = 0; j < newBonesRotations.Length - 1; j++)
				{
					newBonesRotations[j] = BackBonesTransforms[j].rotation;
					hardTargetRoatations[j] = BackBonesTransforms[j].rotation;
				}
				newBonesRotations[newBonesRotations.Length - 1] = LeadBone.rotation;
				hardTargetRoatations[newBonesRotations.Length - 1] = LeadBone.rotation;
			}
			flag = true;
			if (onlyIfNull && lerpRotationsUltra != null)
			{
				flag = false;
			}
			if (flag)
			{
				lerpRotationsUltra = new Quaternion[BackBonesTransforms.Length];
				for (int k = 0; k < lerpRotationsUltra.Length; k++)
				{
					lerpRotationsUltra[k] = Quaternion.identity;
				}
				headLerpRotUltra = LeadBone.rotation;
			}
		}

		private void CalculateBonesRotationsWeights()
		{
			if (BackBonesCount == 0)
			{
				RotationWeights = new float[0];
				return;
			}
			float num = 0f;
			RotationWeights = new float[BackBonesTransforms.Length + 1];
			if (BackBonesTransforms.Length != 0)
			{
				if (BackBonesFalloff.length < 2 || !CurveSpread)
				{
					float num2 = 1f;
					float num3 = 0.75f;
					float num4 = num2;
					float[] array = new float[RotationWeights.Length];
					array[0] = num2 * num3 * 0.65f;
					num4 -= array[0];
					for (int i = 1; i < array.Length - 1; i++)
					{
						num4 -= (array[i] = num4 / (1f + (1f - num3)) * num3);
					}
					array[array.Length - 1] = num4;
					num4 = 0f;
					for (int j = 0; j < array.Length; j++)
					{
						num += array[j];
					}
					float b = 1f / (float)BackBonesTransforms.Length;
					for (int k = 0; k < RotationWeights.Length; k++)
					{
						RotationWeights[k] = Mathf.LerpUnclamped(array[k], b, FaloffValue * 1.25f);
					}
				}
				else
				{
					num = 0f;
					float num5 = 1f;
					float num6 = 1f / (float)RotationWeights.Length;
					for (int l = 0; l < RotationWeights.Length; l++)
					{
						RotationWeights[l] = BackBonesFalloff.Evaluate(num6 * (float)l) / num5;
						num += RotationWeights[l];
					}
					for (int m = 0; m < RotationWeights.Length; m++)
					{
						RotationWeights[m] /= num;
					}
				}
			}
			else
			{
				RotationWeights = new float[1];
				RotationWeights[0] = 1f;
			}
		}

		private IEnumerator SwitchLookingTransition(float transitionTime, bool enableAnimation, Action callback = null)
		{
			float time = 0f;
			float startBlend = BlendToOriginal;
			while (time < transitionTime)
			{
				time += Time.deltaTime;
				float progress = time / transitionTime;
				if (enableAnimation)
				{
					BlendToOriginal = Mathf.Lerp(startBlend, 0f, progress);
				}
				else
				{
					BlendToOriginal = Mathf.Lerp(startBlend, 1f, progress);
				}
				yield return null;
			}
			if (callback != null)
			{
				callback();
			}
		}

		private void OnValidate()
		{
			if (BackBonesTransforms == null)
			{
				BackBonesTransforms = new Transform[BackBonesCount];
			}
			if (BackBonesCount != BackBonesTransforms.Length)
			{
				BackBonesTransforms = new Transform[BackBonesCount];
			}
			if (Application.isPlaying)
			{
				GetAdditionalBones();
			}
			CalculateBonesRotationsWeights();
		}

		private void OnDestroy()
		{
			if (Application.isPlaying && (bool)anchorHelper)
			{
				UnityEngine.Object.Destroy(anchorHelper.gameObject);
			}
		}

		public void UpdateForCustomInspector()
		{
			OnValidate();
			GetAdditionalBones(true);
		}

		public void AddCustomBackbone(Transform t, int index)
		{
			for (int i = 0; i < customBackBones.Count; i++)
			{
				if (customBackBones[i].index == index)
				{
					customBackBones.RemoveAt(i);
					if (t == null)
					{
						return;
					}
					break;
				}
			}
			if (t != null)
			{
				customBackBones.Add(new CustomBackBone(t, index));
			}
		}

		public void FindBaseTransform()
		{
			BaseTransform = base.transform;
			if (!GetComponentInChildren<Animator>() && !GetComponentInChildren<Animation>())
			{
				Debug.LogWarning(base.gameObject.name + " don't have animator, is it root transform for your character?");
			}
		}

		private Quaternion LinearInterpolateRotation(Quaternion start, Quaternion end, float value)
		{
			return Quaternion.Slerp(start, end, value);
		}
	}
}
