using UnityEngine;

public class ExclamationScript : MonoBehaviour
{
	public Renderer Graphic;

	public float Alpha;

	public float Timer;

	private void Start()
	{
		base.transform.localScale = Vector3.zero;
		Graphic.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0f));
	}

	private void Update()
	{
		Timer -= Time.deltaTime;
		if (!(Timer > 0f))
		{
			return;
		}
		base.transform.LookAt(Camera.main.transform);
		if (Timer > 1.5f)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			Alpha = Mathf.Lerp(Alpha, 0.5f, Time.deltaTime * 10f);
			Graphic.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, Alpha));
			return;
		}
		if (base.transform.localScale.x > 0.1f)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
		}
		else
		{
			base.transform.localScale = Vector3.zero;
		}
		Alpha = Mathf.Lerp(Alpha, 0f, Time.deltaTime * 10f);
		Graphic.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, Alpha));
	}
}
