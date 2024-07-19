using UnityEngine;

public class HomeMangaBookScript : MonoBehaviour
{
	public HomeMangaScript Manga;

	public float RotationSpeed;

	public int ID;

	private void Start()
	{
		base.transform.eulerAngles = new Vector3(90f, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
	}

	private void Update()
	{
		float y = ((Manga.Selected != ID) ? 0f : (base.transform.eulerAngles.y + Time.deltaTime * RotationSpeed));
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, y, base.transform.eulerAngles.z);
	}
}
