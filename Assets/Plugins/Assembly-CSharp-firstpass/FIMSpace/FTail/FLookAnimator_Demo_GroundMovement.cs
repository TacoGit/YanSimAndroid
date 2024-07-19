using UnityEngine;

namespace FIMSpace.FTail
{
	public class FLookAnimator_Demo_GroundMovement : MonoBehaviour
	{
		[Header("Check my other Package 'Ground Fitter' for", order = 0)]
		[Space(-7f, order = 1)]
		[Header("more customizable ground fit movement", order = 2)]
		public float RotationYAxis;

		[Range(1f, 30f)]
		public float FittingSpeed = 6f;

		public float RaycastHeightOffset = 0.5f;

		public float RaycastCheckRange = 5f;

		public float LookAheadRaycast;

		public float AheadBlend = 0.5f;

		public float YOffset;

		[Space(8f)]
		public LayerMask GroundLayerMask = 1;

		public bool RelativeLookUp = true;

		[Range(0f, 1f)]
		public float RelativeLookUpBias = 0.25f;

		protected Quaternion helperRotation = Quaternion.identity;

		protected float delta;

		protected bool fittingEnabled = true;

		[Header("> Movement <")]
		public float BaseSpeed = 3f;

		public float RotateToTargetSpeed = 6f;

		public float SprintingSpeed = 10f;

		protected float ActiveSpeed;

		public float AccelerationSpeed = 10f;

		public float DecelerationSpeed = 10f;

		public float JumpPower = 7f;

		public float gravity = 15f;

		public bool MultiplySprintAnimation;

		internal float YVelocity;

		protected bool inAir;

		protected float gravityOffset;

		internal bool MoveForward;

		internal bool Sprint;

		internal float RotationOffset;

		protected string lastAnim = string.Empty;

		protected Animator animator;

		protected bool animatorHaveAnimationSpeedProp;

		protected float initialYOffset;

		protected Vector3 holdJumpPosition;

		protected float freezeJumpYPosition;

		private bool oneAnimation;

		public RaycastHit LastRaycast { get; protected set; }

		private Vector3 GetUpVector()
		{
			if (RelativeLookUp)
			{
				return Vector3.Lerp(base.transform.up, Vector3.up, RelativeLookUpBias);
			}
			return Vector3.up;
		}

		protected virtual void FitToGround()
		{
			RaycastHit hitInfo = default(RaycastHit);
			if (LookAheadRaycast != 0f)
			{
				Physics.Raycast(base.transform.position + GetUpVector() * RaycastHeightOffset + base.transform.forward * LookAheadRaycast, -GetUpVector(), out hitInfo, RaycastCheckRange + YOffset, GroundLayerMask, QueryTriggerInteraction.Ignore);
			}
			RefreshLastRaycast();
			if ((bool)LastRaycast.transform)
			{
				Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, LastRaycast.normal);
				if ((bool)hitInfo.transform)
				{
					Quaternion b = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
					quaternion = Quaternion.Lerp(quaternion, b, AheadBlend);
				}
				helperRotation = Quaternion.Slerp(helperRotation, quaternion, delta * FittingSpeed);
			}
			else
			{
				helperRotation = Quaternion.Slerp(helperRotation, Quaternion.identity, delta * FittingSpeed);
			}
			RotationCalculations();
			if ((bool)LastRaycast.transform)
			{
				base.transform.position = LastRaycast.point + Vector3.up * YOffset;
			}
		}

		internal void RotationCalculations()
		{
			Quaternion rotation = helperRotation * Quaternion.AngleAxis(RotationYAxis, Vector3.up);
			base.transform.rotation = rotation;
		}

		internal RaycastHit CastRay()
		{
			RaycastHit hitInfo;
			Physics.Raycast(base.transform.position + GetUpVector() * RaycastHeightOffset, -GetUpVector(), out hitInfo, RaycastCheckRange + Mathf.Abs(YOffset), GroundLayerMask, QueryTriggerInteraction.Ignore);
			return hitInfo;
		}

		internal void RefreshLastRaycast()
		{
			LastRaycast = CastRay();
		}

		protected virtual void InitMovement()
		{
			animator = GetComponentInChildren<Animator>();
			if ((bool)animator)
			{
				if (HasParameter(animator, "AnimationSpeed"))
				{
					animatorHaveAnimationSpeedProp = true;
				}
				if (!animator.HasState(0, Animator.StringToHash("Idle")))
				{
					oneAnimation = true;
				}
			}
			RotationYAxis = base.transform.rotation.eulerAngles.y;
			initialYOffset = YOffset;
			RefreshLastRaycast();
			RotationOffset = 0f;
			Sprint = false;
			MoveForward = false;
		}

		protected virtual void UpdateMovement()
		{
			delta = Time.deltaTime;
			HandleInput();
			HandleGravity();
			HandleAnimations();
			HandleTransforming();
		}

		protected virtual void HandleInput()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Jump();
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{
					Sprint = true;
				}
				else
				{
					Sprint = false;
				}
				RotationOffset = 0f;
				if (Input.GetKey(KeyCode.A))
				{
					RotationOffset = -90f;
				}
				if (Input.GetKey(KeyCode.D))
				{
					RotationOffset = 90f;
				}
				if (Input.GetKey(KeyCode.S))
				{
					RotationOffset = 180f;
				}
				MoveForward = true;
			}
			else
			{
				Sprint = false;
				MoveForward = false;
			}
		}

		protected virtual void HandleGravity()
		{
			if (fittingEnabled)
			{
				if (YOffset > initialYOffset)
				{
					YOffset += YVelocity * delta;
				}
				else
				{
					YOffset = initialYOffset;
				}
			}
			else
			{
				YOffset += YVelocity * delta;
			}
			if (inAir)
			{
				YVelocity -= gravity * delta;
			}
			if (fittingEnabled)
			{
				if (!LastRaycast.transform)
				{
					if (!inAir)
					{
						inAir = true;
						holdJumpPosition = base.transform.position;
						freezeJumpYPosition = holdJumpPosition.y;
						YVelocity = -1f;
						fittingEnabled = false;
					}
				}
				else if (YVelocity > 0f)
				{
					inAir = true;
				}
			}
			if (!inAir)
			{
				return;
			}
			if (fittingEnabled)
			{
				fittingEnabled = false;
			}
			if (YVelocity < 0f)
			{
				RaycastHit raycastHit = CastRay();
				if ((bool)raycastHit.transform && base.transform.position.y + YVelocity * delta <= raycastHit.point.y + initialYOffset + 0.05f)
				{
					YOffset -= raycastHit.point.y - freezeJumpYPosition;
					HitGround();
				}
				return;
			}
			RaycastHit raycastHit2 = CastRay();
			if ((bool)raycastHit2.transform && raycastHit2.point.y - 0.1f > base.transform.position.y)
			{
				YOffset = initialYOffset;
				YVelocity = -1f;
				HitGround();
			}
		}

		protected virtual void HandleAnimations()
		{
			if (ActiveSpeed > 0.15f)
			{
				if (Sprint)
				{
					CrossfadeTo("Run");
				}
				else
				{
					CrossfadeTo("Walk");
				}
			}
			else
			{
				CrossfadeTo("Idle");
			}
			if (oneAnimation)
			{
				if (animatorHaveAnimationSpeedProp)
				{
					if (inAir)
					{
						FAnimatorMethods.LerpFloatValue(animator, "AnimationSpeed", 0f, 30f);
					}
					else
					{
						FAnimatorMethods.LerpFloatValue(animator, "AnimationSpeed", (!MultiplySprintAnimation) ? Mathf.Min(1f, ActiveSpeed / BaseSpeed) : (ActiveSpeed / BaseSpeed), 30f);
					}
				}
			}
			else if (animatorHaveAnimationSpeedProp)
			{
				if (inAir)
				{
					FAnimatorMethods.LerpFloatValue(animator, "AnimationSpeed");
				}
				else
				{
					FAnimatorMethods.LerpFloatValue(animator, "AnimationSpeed", (!MultiplySprintAnimation) ? Mathf.Min(1f, ActiveSpeed / BaseSpeed) : (ActiveSpeed / BaseSpeed));
				}
			}
		}

		protected void RefreshHitGroundVars(RaycastHit hit)
		{
			holdJumpPosition = hit.point;
			freezeJumpYPosition = hit.point.y;
			YOffset = Mathf.Abs(LastRaycast.point.y - base.transform.position.y);
		}

		protected virtual void HandleTransforming()
		{
			if (fittingEnabled)
			{
				if ((bool)LastRaycast.transform)
				{
					base.transform.position = LastRaycast.point + YOffset * Vector3.up;
					holdJumpPosition = base.transform.position;
					freezeJumpYPosition = holdJumpPosition.y;
				}
				else
				{
					inAir = true;
				}
			}
			else
			{
				holdJumpPosition.y = freezeJumpYPosition + YOffset;
			}
			if (MoveForward)
			{
				if (!fittingEnabled)
				{
					RotationYAxis = Mathf.LerpAngle(RotationYAxis, Camera.main.transform.eulerAngles.y + RotationOffset, delta * RotateToTargetSpeed * 0.15f);
					RotationCalculations();
				}
				else
				{
					RotationYAxis = Mathf.LerpAngle(RotationYAxis, Camera.main.transform.eulerAngles.y + RotationOffset, delta * RotateToTargetSpeed);
				}
				if (!Sprint)
				{
					ActiveSpeed = Mathf.Lerp(ActiveSpeed, BaseSpeed, delta * AccelerationSpeed);
				}
				else
				{
					ActiveSpeed = Mathf.Lerp(ActiveSpeed, SprintingSpeed, delta * AccelerationSpeed);
				}
			}
			else if (ActiveSpeed > 0f)
			{
				ActiveSpeed = Mathf.Lerp(ActiveSpeed, -0.01f, delta * DecelerationSpeed);
			}
			else
			{
				ActiveSpeed = 0f;
			}
			holdJumpPosition += base.transform.forward * ActiveSpeed * delta;
			base.transform.position = holdJumpPosition;
		}

		protected virtual void HitGround()
		{
			RefreshLastRaycast();
			fittingEnabled = true;
			inAir = false;
			freezeJumpYPosition = 0f;
		}

		public virtual void Jump()
		{
			YVelocity = JumpPower;
			YOffset += JumpPower * Time.deltaTime / 2f;
		}

		protected virtual void CrossfadeTo(string animation, float transitionTime = 0.25f)
		{
			if (!animator || oneAnimation)
			{
				return;
			}
			if (!animator.HasState(0, Animator.StringToHash(animation)))
			{
				if (!(animation == "Run"))
				{
					return;
				}
				animation = "Walk";
			}
			if (lastAnim != animation)
			{
				animator.CrossFadeInFixedTime(animation, transitionTime);
				lastAnim = animation;
			}
		}

		public static bool HasParameter(Animator animator, string paramName)
		{
			AnimatorControllerParameter[] parameters = animator.parameters;
			foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
			{
				if (animatorControllerParameter.name == paramName)
				{
					return true;
				}
			}
			return false;
		}

		protected virtual void Start()
		{
			InitMovement();
		}

		protected virtual void Update()
		{
			delta = Time.deltaTime;
			if (fittingEnabled)
			{
				FitToGround();
			}
			UpdateMovement();
		}
	}
}
