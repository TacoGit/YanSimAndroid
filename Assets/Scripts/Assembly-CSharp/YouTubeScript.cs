using UnityEngine;

public class YouTubeScript : MonoBehaviour
{
	public float Strength;

	public bool Begin;

	private void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			Begin = true;
		}
		if (Begin)
		{
			Strength += Time.deltaTime;
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, 1.15f, 1f), Time.deltaTime * Strength);
		}
	}
}
