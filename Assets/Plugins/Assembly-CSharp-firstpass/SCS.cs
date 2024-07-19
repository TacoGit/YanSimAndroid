using UnityEngine;

public class SCS : MonoBehaviour
{
	public Transform Player;

	public float CloudsSpeed;

	private float height;

	private float heightDamping;

	private void Update()
	{
		base.transform.Rotate(0f, CloudsSpeed * Time.deltaTime, 0f);
	}

	private void LateUpdate()
	{
		if (!(Player == null))
		{
			float b = Player.position.y + height;
			float y = Mathf.Lerp(base.transform.position.y, b, heightDamping * Time.deltaTime);
			base.transform.position = new Vector3(Player.position.x, y, Player.position.z);
		}
	}
}
