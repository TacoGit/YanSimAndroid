using UnityEngine;

public class BoneScript : MonoBehaviour
{
	public float Height;

	public float Origin;

	public bool Drop;

	private void Start()
	{
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Random.Range(0f, 360f), base.transform.eulerAngles.z);
		Origin = base.transform.position.y;
		GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
	}

	private void Update()
	{
		if (!Drop)
		{
			if (base.transform.position.y < Origin + 2f - 0.0001f)
			{
				base.transform.position = new Vector3(base.transform.position.x, Mathf.Lerp(base.transform.position.y, Origin + 2f, Time.deltaTime * 10f), base.transform.position.z);
			}
			else
			{
				Drop = true;
			}
			return;
		}
		Height -= Time.deltaTime;
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + Height, base.transform.position.z);
		if (base.transform.position.y < Origin - 2.155f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 9)
		{
			StudentScript component = other.gameObject.GetComponent<StudentScript>();
			if (component != null)
			{
				component.DeathType = DeathType.EasterEgg;
				component.BecomeRagdoll();
				Rigidbody rigidbody = component.Ragdoll.AllRigidbodies[0];
				rigidbody.isKinematic = false;
				rigidbody.AddForce(base.transform.up);
			}
		}
	}
}
