using UnityEngine;

public class FPSWalker : MonoBehaviour
{
	public float speed = 6f;

	public float jumpSpeed = 8f;

	public float gravity = 20f;

	private Vector3 moveDirection = Vector3.zero;

	private bool grounded;

	private void FixedUpdate()
	{
		if (grounded)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			moveDirection = base.transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
		}
		moveDirection.y -= gravity * Time.deltaTime;
		CharacterController component = GetComponent<CharacterController>();
		CollisionFlags collisionFlags = component.Move(moveDirection * Time.deltaTime);
		grounded = (collisionFlags & CollisionFlags.Below) != 0;
	}

	private void Awake()
	{
		CharacterController component = GetComponent<CharacterController>();
		if (component == null)
		{
			base.gameObject.AddComponent<CharacterController>();
		}
	}
}
