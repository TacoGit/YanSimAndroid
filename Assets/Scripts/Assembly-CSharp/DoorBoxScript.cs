using UnityEngine;

public class DoorBoxScript : MonoBehaviour
{
	public UILabel Label;

	public bool Show;

	private void Update()
	{
		float y = Mathf.Lerp(base.transform.localPosition.y, (!Show) ? (-630f) : (-530f), Time.deltaTime * 10f);
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, y, base.transform.localPosition.z);
	}
}
