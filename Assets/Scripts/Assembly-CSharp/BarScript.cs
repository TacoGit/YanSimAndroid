using UnityEngine;

public class BarScript : MonoBehaviour
{
	public float Speed;

	private void Start()
	{
		base.transform.localScale = new Vector3(0f, 1f, 1f);
	}

	private void Update()
	{
		base.transform.localScale = new Vector3(base.transform.localScale.x + Speed * Time.deltaTime, 1f, 1f);
		if ((double)base.transform.localScale.x > 0.1)
		{
			base.transform.localScale = new Vector3(0f, 1f, 1f);
		}
	}
}
