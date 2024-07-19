using UnityEngine;

public class YanvaniaCandlestickScript : MonoBehaviour
{
	public GameObject DestroyedCandlestick;

	public bool Destroyed;

	public AudioClip Break;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 19 && !Destroyed)
		{
			GameObject gameObject = Object.Instantiate(DestroyedCandlestick, base.transform.position, Quaternion.identity);
			gameObject.transform.localScale = base.transform.localScale;
			Destroyed = true;
			AudioClipPlayer.Play2D(Break, base.transform.position);
			Object.Destroy(base.gameObject);
		}
	}
}
