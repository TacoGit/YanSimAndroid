using UnityEngine;

public class TagScript : MonoBehaviour
{
	public UISprite Sprite;

	public Camera UICamera;

	public Camera MainCameraCamera;

	public Transform MainCamera;

	public Transform Target;

	private void Start()
	{
		Sprite.color = new Color(1f, 0f, 0f, 0f);
		MainCameraCamera = MainCamera.GetComponent<Camera>();
	}

	private void Update()
	{
		if (Target != null)
		{
			float num = Vector3.Angle(MainCamera.forward, MainCamera.position - Target.position);
			if (num > 90f)
			{
				Vector2 vector = MainCameraCamera.WorldToScreenPoint(Target.position);
				base.transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector.x, vector.y, 1f));
			}
		}
	}
}
