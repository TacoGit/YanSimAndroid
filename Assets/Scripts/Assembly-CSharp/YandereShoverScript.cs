using UnityEngine;

public class YandereShoverScript : MonoBehaviour
{
	public YandereScript Yandere;

	public bool PreventNudity;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != 13)
		{
			return;
		}
		bool flag = false;
		if (PreventNudity)
		{
			if (Yandere.Schoolwear == 0)
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			if (Yandere.Aiming)
			{
				Yandere.StopAiming();
			}
			if (Yandere.Laughing)
			{
				Yandere.StopLaughing();
			}
			Yandere.transform.rotation = Quaternion.LookRotation(new Vector3(base.transform.position.x, Yandere.transform.position.y, base.transform.position.z) - Yandere.transform.position);
			Yandere.CharacterAnimation["f02_shoveA_01"].time = 0f;
			Yandere.CharacterAnimation.CrossFade("f02_shoveA_01");
			Yandere.YandereVision = false;
			Yandere.NearSenpai = false;
			Yandere.Degloving = false;
			Yandere.Flicking = false;
			Yandere.Punching = false;
			Yandere.CanMove = false;
			Yandere.Shoved = true;
			Yandere.EmptyHands();
			Yandere.GloveTimer = 0f;
			Yandere.h = 0f;
			Yandere.v = 0f;
			Yandere.ShoveSpeed = 2f;
		}
	}
}
