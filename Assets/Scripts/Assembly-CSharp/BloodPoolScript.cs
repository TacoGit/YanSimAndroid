using UnityEngine;

public class BloodPoolScript : MonoBehaviour
{
	public float TargetSize;

	public bool Blood = true;

	public bool Grow;

	public Renderer MyRenderer;

	public Texture Flower;

	private void Start()
	{
		if (PlayerGlobals.PantiesEquipped == 7 && Blood)
		{
			TargetSize *= 0.5f;
		}
		if (GameGlobals.CensorBlood)
		{
			MyRenderer.material.color = new Color(1f, 1f, 1f, 1f);
			MyRenderer.material.mainTexture = Flower;
		}
		base.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		Vector3 position = base.transform.position;
		if (position.x > 125f || position.x < -125f || position.z > 200f || position.z < -100f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (Grow)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(TargetSize, TargetSize, TargetSize), Time.deltaTime);
			if (base.transform.localScale.x > TargetSize * 0.99f)
			{
				Grow = false;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "BloodSpawner")
		{
			Grow = true;
		}
	}
}
