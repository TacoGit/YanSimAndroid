using UnityEngine;

public class GloveScript : MonoBehaviour
{
	public PromptScript Prompt;

	public PickUpScript PickUp;

	public Collider MyCollider;

	public Projector Blood;

	private void Start()
	{
		YandereScript component = GameObject.Find("YandereChan").GetComponent<YandereScript>();
		Physics.IgnoreCollision(component.GetComponent<Collider>(), MyCollider);
		if (base.transform.position.y > 1000f)
		{
			base.transform.position = new Vector3(12f, 0f, 28f);
		}
	}

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			base.transform.parent = Prompt.Yandere.transform;
			base.transform.localPosition = new Vector3(0f, 1f, 0.25f);
			Prompt.Yandere.Gloves = this;
			Prompt.Yandere.WearGloves();
			base.gameObject.SetActive(false);
		}
		Prompt.HideButton[0] = Prompt.Yandere.Schoolwear != 1 || Prompt.Yandere.ClubAttire;
	}
}
