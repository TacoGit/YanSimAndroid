using UnityEngine;

public class CabinetDoorScript : MonoBehaviour
{
	public PromptScript Prompt;

	public bool Locked;

	public bool Open;

	public float Timer;

	private void Update()
	{
		if (Timer < 2f)
		{
			Timer += Time.deltaTime;
			if (Open)
			{
				base.transform.localPosition = new Vector3(Mathf.Lerp(base.transform.localPosition.x, 0.41775f, Time.deltaTime * 10f), base.transform.localPosition.y, base.transform.localPosition.z);
			}
			else
			{
				base.transform.localPosition = new Vector3(Mathf.Lerp(base.transform.localPosition.x, 0f, Time.deltaTime * 10f), base.transform.localPosition.y, base.transform.localPosition.z);
			}
		}
	}
}
