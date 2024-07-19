using UnityEngine;

public class HomePantiesScript : MonoBehaviour
{
	public HomePantyChangerScript PantyChanger;

	public float RotationSpeed;

	public int ID;

	private void Update()
	{
		float y = ((PantyChanger.Selected != ID) ? 0f : (base.transform.eulerAngles.y + Time.deltaTime * RotationSpeed));
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, y, base.transform.eulerAngles.z);
	}
}
