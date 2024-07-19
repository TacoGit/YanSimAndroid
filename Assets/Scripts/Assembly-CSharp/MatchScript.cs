using UnityEngine;

public class MatchScript : MonoBehaviour
{
	public float Timer;

	public Collider MyCollider;

	private void Update()
	{
		if (!GetComponent<Rigidbody>().useGravity)
		{
			return;
		}
		base.transform.Rotate(Vector3.right * (Time.deltaTime * 360f));
		if (Timer > 0f && MyCollider.isTrigger)
		{
			MyCollider.isTrigger = false;
		}
		Timer += Time.deltaTime;
		if (Timer > 5f)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z - Time.deltaTime);
			if (base.transform.localScale.z < 0f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
