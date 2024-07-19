using UnityEngine;

public class FoldedUniformScript : MonoBehaviour
{
	public YandereScript Yandere;

	public PromptScript Prompt;

	public GameObject SteamCloud;

	public bool InPosition = true;

	public bool Clean;

	public float Timer;

	public int Type;

	private void Start()
	{
		Yandere = GameObject.Find("YandereChan").GetComponent<YandereScript>();
		if (Clean && Prompt.Button[0] != null)
		{
			Prompt.HideButton[0] = true;
		}
	}

	private void Update()
	{
		if (!Clean)
		{
			return;
		}
		InPosition = Yandere.StudentManager.LockerRoomArea.bounds.Contains(base.transform.position);
		Prompt.HideButton[0] = !Yandere.MyRenderer.sharedMesh == (bool)Yandere.Towel || Yandere.Bloodiness != 0f || !InPosition;
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Object.Instantiate(SteamCloud, Yandere.transform.position + Vector3.up * 0.81f, Quaternion.identity);
			Yandere.Character.GetComponent<Animation>().CrossFade("f02_stripping_00");
			Yandere.Stripping = true;
			Yandere.CanMove = false;
			Timer += Time.deltaTime;
		}
		if (Timer > 0f)
		{
			Timer += Time.deltaTime;
			if (Timer > 1.5f)
			{
				Yandere.Schoolwear = 1;
				Yandere.ChangeSchoolwear();
				Object.Destroy(base.gameObject);
			}
		}
	}
}
