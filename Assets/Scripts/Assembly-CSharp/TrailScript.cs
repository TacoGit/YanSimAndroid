using UnityEngine;

public class TrailScript : MonoBehaviour
{
	private void Start()
	{
		GameObject gameObject = GameObject.Find("YandereChan");
		Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GetComponent<Collider>());
		Object.Destroy(this);
	}
}
