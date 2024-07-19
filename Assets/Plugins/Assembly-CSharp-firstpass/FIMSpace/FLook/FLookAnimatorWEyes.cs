using UnityEngine;

namespace FIMSpace.FLook
{
	[AddComponentMenu("FImpossible Games/Look Animator/FLook Animator With Eyes")]
	public class FLookAnimatorWEyes : FLookAnimator
	{
		[Tooltip("Target on which eyes will look")]
		public Transform EyesTarget;

		[Space(4f)]
		[Tooltip("Eyes transforms / bones (origin should be in center of the sphere")]
		public Transform LeftEye;

		public bool InvertLeftEye;

		[Tooltip("Eyes transforms / bones (origin should be in center of the sphere")]
		public Transform RightEye;

		public bool InvertRightEye;

		[Tooltip("Look clamping reference rotation transform, mostly parent of eye objects")]
		public Transform HeadReference;

		public Vector3 EyesOffsetRotation;

		[Tooltip("How fast eyes should follow target")]
		[Range(0f, 1f)]
		public float EyesSpeed = 0.5f;

		[Range(0f, 1f)]
		public float EyesBlend = 1f;

		[Tooltip("In what angle eyes should go back to deafult position")]
		[Range(0f, 180f)]
		public Vector2 EyesXRange = new Vector2(-60f, 60f);

		public Vector2 EyesYRange = new Vector2(-50f, 50f);

		[Tooltip("If your eyes don't have baked keyframes in animation this value should be enabled, otherwise eyes would go crazy")]
		public bool EyesNoKeyframes = true;

		private float EyesOutOfRangeBlend = 1f;

		private Transform[] eyes;

		private Vector3[] eyeForwards;

		private Quaternion[] eyesInitLocalRotations;

		private Quaternion[] eyesLerpRotations;

		private Vector3 headForward;

		private float blend;

		protected override void Start()
		{
			base.Start();
			eyes = new Transform[0];
			if (LeftEye != null || RightEye != null)
			{
				if (LeftEye != null && RightEye != null)
				{
					eyes = new Transform[2] { LeftEye, RightEye };
				}
				else if (LeftEye != null)
				{
					eyes = new Transform[1] { LeftEye };
				}
				else
				{
					eyes = new Transform[1] { RightEye };
				}
			}
			eyeForwards = new Vector3[eyes.Length];
			eyesInitLocalRotations = new Quaternion[eyes.Length];
			eyesLerpRotations = new Quaternion[eyes.Length];
			for (int i = 0; i < eyeForwards.Length; i++)
			{
				Vector3 position = eyes[i].position + Vector3.Scale(base.transform.forward, eyes[i].transform.lossyScale);
				Vector3 position2 = eyes[i].position;
				eyeForwards[i] = (eyes[i].InverseTransformPoint(position) - eyes[i].InverseTransformPoint(position2)).normalized;
				eyesInitLocalRotations[i] = eyes[i].localRotation;
				eyesLerpRotations[i] = eyes[i].rotation;
			}
			headForward = (HeadReference.InverseTransformPoint(HeadReference.position) - HeadReference.InverseTransformPoint(HeadReference.position + base.transform.forward)).normalized;
		}

		private void OnValidate()
		{
			if (!HeadReference && ((bool)LeftEye || (bool)RightEye))
			{
				if ((bool)LeftEye)
				{
					HeadReference = LeftEye.parent;
				}
				else
				{
					HeadReference = RightEye.parent;
				}
			}
		}

		public override void LateUpdate()
		{
			if (!initialized)
			{
				return;
			}
			base.LateUpdate();
			if (EyesNoKeyframes)
			{
				for (int i = 0; i < eyeForwards.Length; i++)
				{
					eyes[i].localRotation = eyesInitLocalRotations[i];
				}
			}
			Transform transform = EyesTarget;
			if (transform == null)
			{
				transform = ((!(base.momentLookTransform != null)) ? ObjectToFollow : base.momentLookTransform);
			}
			bool flag = false;
			if (transform == null)
			{
				flag = true;
			}
			else if (EyesTarget == null && lookState != EFHeadLookState.ClampedAngle && lookState != EFHeadLookState.Following)
			{
				flag = true;
			}
			if (flag)
			{
				EyesOutOfRangeBlend = Mathf.Max(0f, EyesOutOfRangeBlend - Time.deltaTime);
			}
			else
			{
				EyesOutOfRangeBlend = Mathf.Min(1f, EyesOutOfRangeBlend + Time.deltaTime);
			}
			blend = EyesBlend * EyesOutOfRangeBlend;
			if (blend <= 0f || !(transform != null))
			{
				return;
			}
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
			Vector3 eulerAngles = Quaternion.LookRotation(transform.position - vector).eulerAngles;
			Vector3 eulerAngles2 = (HeadReference.rotation * Quaternion.FromToRotation(headForward, Vector3.forward)).eulerAngles;
			Vector2 vector2 = new Vector3(Mathf.DeltaAngle(eulerAngles.x, eulerAngles2.x), Mathf.DeltaAngle(eulerAngles.y, eulerAngles2.y));
			if (vector2.x > EyesYRange.y)
			{
				eulerAngles.x = eulerAngles2.x - EyesYRange.y;
			}
			else if (vector2.x < EyesYRange.x)
			{
				eulerAngles.x = eulerAngles2.x - EyesYRange.x;
			}
			if (vector2.y > 0f - EyesXRange.x)
			{
				eulerAngles.y = eulerAngles2.y - EyesXRange.y;
			}
			else if (vector2.y < 0f - EyesXRange.y)
			{
				eulerAngles.y = eulerAngles2.y + EyesXRange.y;
			}
			for (int j = 0; j < eyes.Length; j++)
			{
				Quaternion rotation = eyes[j].rotation;
				Quaternion rotation2 = Quaternion.Euler(eulerAngles);
				float num = 1f;
				if (eyes[j] == LeftEye)
				{
					if (InvertLeftEye)
					{
						num = -1f;
					}
				}
				else if (eyes[j] == RightEye && InvertRightEye)
				{
					num = -1f;
				}
				rotation2 *= Quaternion.FromToRotation(eyeForwards[j], Vector3.forward * num);
				rotation2 *= eyesInitLocalRotations[j];
				eyes[j].rotation = rotation2;
				eyes[j].rotation *= Quaternion.Inverse(eyesInitLocalRotations[j]);
				if (j == 0)
				{
					eyes[j].rotation *= Quaternion.Euler(EyesOffsetRotation);
				}
				else
				{
					eyes[j].rotation *= Quaternion.Euler(new Vector3(EyesOffsetRotation.x, EyesOffsetRotation.y, -180f + EyesOffsetRotation.z));
				}
				rotation2 = eyes[j].rotation;
				eyesLerpRotations[j] = Quaternion.Slerp(eyesLerpRotations[j], rotation2, Time.deltaTime * Mathf.Lerp(2f, 40f, EyesSpeed));
				eyes[j].rotation = Quaternion.Slerp(rotation, eyesLerpRotations[j], blend);
			}
		}
	}
}
