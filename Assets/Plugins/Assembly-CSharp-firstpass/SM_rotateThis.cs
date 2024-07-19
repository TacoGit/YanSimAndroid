using UnityEngine;

public class SM_rotateThis : MonoBehaviour
{
	public float rotationSpeedX = 90f;

	public float rotationSpeedY;

	public float rotationSpeedZ;

	private void Update()
	{
		base.transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
	}
}
