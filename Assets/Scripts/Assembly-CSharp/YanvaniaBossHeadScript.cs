using UnityEngine;

public class YanvaniaBossHeadScript : MonoBehaviour
{
	public YanvaniaDraculaScript Dracula;

	public GameObject HitEffect;

	public float Timer;

	private void Update()
	{
		Timer -= Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (Timer <= 0f && Dracula.NewTeleportEffect == null && other.gameObject.name == "Heart")
		{
			Object.Instantiate(HitEffect, base.transform.position, Quaternion.identity);
			Timer = 1f;
			Dracula.TakeDamage();
		}
	}
}
