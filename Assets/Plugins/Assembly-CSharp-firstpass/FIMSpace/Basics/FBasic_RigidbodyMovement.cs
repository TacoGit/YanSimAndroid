using UnityEngine;

namespace FIMSpace.Basics
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Collider))]
	public class FBasic_RigidbodyMovement : FBasic_CharacterHorizontalBase
	{
		public float SkinHeight = 0.1f;

		protected float fakeYVelocity = -3f;

		public bool FacingWall;

		public bool SideingWall;

		protected float diagonalMultiplier = 0.7f;

		protected CapsuleCollider capsuleCollider;

		protected Vector3 targetVelocity = Vector3.zero;

		protected bool jumpCollisionFrameOffset;

		[Header("When we go down the slope, preventing from bumping")]
		public float PushDownYVelocity = -15f;

		public static LayerMask ChracterLayerMask = 0;

		public Rigidbody CharacterRigidbody { get; protected set; }

		protected override void Start()
		{
			base.Start();
			CharacterRigidbody = GetComponent<Rigidbody>();
			capsuleCollider = GetComponent<CapsuleCollider>();
			if ((int)ChracterLayerMask == 0)
			{
				ChracterLayerMask = ~(1 << LayerMask.NameToLayer("Water"));
			}
		}

		protected override void FixedUpdate()
		{
			if (Grounded)
			{
				float num = accelerationForward;
				float num2 = accelerationRight;
				if (num != 0f && num2 != 0f)
				{
					num *= diagonalMultiplier;
					num2 *= diagonalMultiplier;
				}
				Vector3 velocity = new Vector3(lastTargetVelocityForward.x * num, CharacterRigidbody.velocity.y, lastTargetVelocityForward.z * num);
				velocity += new Vector3(lastTargetVelocityRight.x * num2, 0f, lastTargetVelocityRight.z * num2);
				targetVelocity = velocity;
				CharacterRigidbody.velocity = velocity;
			}
			base.FixedUpdate();
		}

		protected override void Update()
		{
			base.Update();
			CheckGroundPlacement();
		}

		protected void CheckGroundPlacement()
		{
			if (!Jumped && !Grounded)
			{
				Ray ray = new Ray(base.transform.position + Vector3.up * 0.2f, Vector3.down);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, SkinHeight + 0.2f, ChracterLayerMask, QueryTriggerInteraction.Ignore))
				{
					float num = capsuleCollider.bounds.center.y - capsuleCollider.bounds.extents.y - base.transform.position.y;
					Grounded = true;
					base.transform.position = new Vector3(base.transform.position.x, hitInfo.point.y - num, base.transform.position.z);
					CharacterRigidbody.velocity = new Vector3(CharacterRigidbody.velocity.x, fakeYVelocity, CharacterRigidbody.velocity.z);
					CharacterRigidbody.AddForce(new Vector3(0f, PushDownYVelocity * 5f, 0f));
					fakeYVelocity = -3f;
				}
			}
		}

		protected override void MovementCalculations()
		{
			if (inputJump)
			{
				if (Grounded)
				{
					Jump();
				}
				inputJump = false;
			}
			base.MovementCalculations();
			if (Grounded)
			{
				fakeYVelocity = -3f;
				CharacterRigidbody.AddForce(new Vector3(0f, PushDownYVelocity, 0f));
			}
			else
			{
				CharacterRigidbody.velocity = new Vector3(CharacterRigidbody.velocity.x, fakeYVelocity, CharacterRigidbody.velocity.z);
				fakeYVelocity -= Time.fixedDeltaTime * GravityPower * CharacterRigidbody.mass;
			}
			Grounded = false;
			FacingWall = false;
			SideingWall = false;
		}

		protected override void RotationCalculations()
		{
			if (Grounded)
			{
				if (verticalValue != 0f)
				{
					targetDirection = inputDirection;
				}
				else if (horizontalValue != 0f)
				{
					targetDirection = inputDirection;
				}
				base.RotationCalculations();
			}
		}

		protected override void Jump()
		{
			fakeYVelocity = CalculateJumpYVelocity() * 1.2f;
			CharacterRigidbody.MovePosition(base.transform.position + Vector3.up * fakeYVelocity * Time.fixedDeltaTime);
			Jumped = true;
			Grounded = false;
			jumpCollisionFrameOffset = true;
		}

		private void OnCollisionStay(Collision collision)
		{
			if (jumpCollisionFrameOffset)
			{
				jumpCollisionFrameOffset = false;
				return;
			}
			float num = 0.5f;
			for (int i = 0; i < collision.contacts.Length; i++)
			{
				if (Vector3.Dot(collision.contacts[i].point - base.transform.position, base.transform.up) < num)
				{
					Grounded = true;
					Jumped = false;
					break;
				}
			}
			CheckIfFacingWall(collision);
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (Vector3.Dot(collision.contacts[0].point - base.transform.position, base.transform.up) > 1f && fakeYVelocity > -0.02f)
			{
				fakeYVelocity = -0.02f;
			}
		}

		private void CheckIfFacingWall(Collision collision)
		{
			int num = -1;
			if (accelerationForward != 0f)
			{
				for (int i = 0; i < collision.contacts.Length; i++)
				{
					float num2 = 1f;
					if (!onlyForward)
					{
						num2 *= Mathf.Sign(verticalValue);
					}
					if (Vector3.Dot(collision.contacts[i].point - base.transform.position, base.transform.forward * num2) > 0.25f)
					{
						FacingWall = true;
						num = i;
						break;
					}
				}
			}
			if (accelerationRight == 0f)
			{
				return;
			}
			for (int j = 0; j < collision.contacts.Length; j++)
			{
				if (j != num && Vector3.Dot(collision.contacts[j].point - base.transform.position, base.transform.right * Mathf.Sign(horizontalValue)) > 0.25f)
				{
					SideingWall = true;
					break;
				}
			}
		}
	}
}
