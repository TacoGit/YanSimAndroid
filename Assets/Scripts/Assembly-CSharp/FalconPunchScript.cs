using UnityEngine;

public class FalconPunchScript : MonoBehaviour
{
	public GameObject FalconExplosion;

	public Rigidbody MyRigidbody;

	public Collider MyCollider;

	public float Strength = 100f;

	public float Speed = 100f;

	public bool IgnoreTime;

	public bool Shipgirl;

	public bool Bancho;

	public bool Falcon;

	public float TimeLimit = 0.5f;

	public float Timer;

	private void Update()
	{
		if (!IgnoreTime)
		{
			Timer += Time.deltaTime;
			if (Timer > TimeLimit)
			{
				MyCollider.enabled = false;
			}
		}
		if (Shipgirl)
		{
			MyRigidbody.AddForce(base.transform.forward * Speed);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("A punch collided with something.");
		if (other.gameObject.layer != 9)
		{
			return;
		}
		Debug.Log("A punch collided with something on the Characters layer.");
		StudentScript component = other.gameObject.GetComponent<StudentScript>();
		if (!(component != null))
		{
			return;
		}
		Debug.Log("A punch collided with a student.");
		if (component.StudentID > 1)
		{
			Debug.Log("A punch collided with a student and killed them.");
			Object.Instantiate(FalconExplosion, component.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
			component.DeathType = DeathType.EasterEgg;
			component.BecomeRagdoll();
			Rigidbody rigidbody = component.Ragdoll.AllRigidbodies[0];
			rigidbody.isKinematic = false;
			Vector3 vector = rigidbody.transform.position - component.Yandere.transform.position;
			Debug.Log("Direction is: " + vector);
			if (Falcon)
			{
				rigidbody.AddForce(vector * Strength);
			}
			else if (Bancho)
			{
				rigidbody.AddForce(vector.x * Strength, 5000f, vector.z * Strength);
			}
			else
			{
				rigidbody.AddForce(vector.x * Strength, 10000f, vector.z * Strength);
			}
		}
	}
}
