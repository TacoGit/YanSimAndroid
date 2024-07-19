using UnityEngine;

public class HallucinationScript : MonoBehaviour
{
	public SkinnedMeshRenderer YandereHairRenderer;

	public SkinnedMeshRenderer YandereRenderer;

	public SkinnedMeshRenderer RivalHairRenderer;

	public SkinnedMeshRenderer RivalRenderer;

	public Animation YandereAnimation;

	public Animation RivalAnimation;

	public YandereScript Yandere;

	public Material Black;

	public bool Hallucinate;

	public float Alpha;

	public float Timer;

	public int Weapon;

	public string[] WeaponName;

	private void Start()
	{
		YandereHairRenderer.material = Black;
		RivalHairRenderer.material = Black;
		YandereRenderer.materials[0] = Black;
		YandereRenderer.materials[1] = Black;
		YandereRenderer.materials[2] = Black;
		RivalRenderer.materials[0] = Black;
		RivalRenderer.materials[1] = Black;
		RivalRenderer.materials[2] = Black;
		MakeTransparent();
	}

	private void Update()
	{
		if (Yandere.Sanity < 33.33333f)
		{
			if (!Yandere.Aiming)
			{
				Timer += Time.deltaTime;
			}
			if (Timer > 6f)
			{
				Weapon = Random.Range(1, 6);
				base.transform.position = Yandere.transform.position + Yandere.transform.forward;
				base.transform.eulerAngles = Yandere.transform.eulerAngles;
				YandereAnimation["f02_" + WeaponName[Weapon] + "LowSanityA_00"].time = 0f;
				RivalAnimation["f02_" + WeaponName[Weapon] + "LowSanityB_00"].time = 0f;
				YandereAnimation.Play("f02_" + WeaponName[Weapon] + "LowSanityA_00");
				RivalAnimation.Play("f02_" + WeaponName[Weapon] + "LowSanityB_00");
				Hallucinate = true;
				Timer = 0f;
			}
		}
		if (Hallucinate)
		{
			if (YandereAnimation["f02_" + WeaponName[Weapon] + "LowSanityA_00"].time < 3f)
			{
				Alpha = Mathf.MoveTowards(Alpha, 1f, Time.deltaTime * 0.33333f);
			}
			else
			{
				Alpha = Mathf.MoveTowards(Alpha, 0f, Time.deltaTime * 0.33333f);
			}
			YandereHairRenderer.material.color = new Color(0f, 0f, 0f, Alpha);
			RivalHairRenderer.material.color = new Color(0f, 0f, 0f, Alpha);
			YandereRenderer.materials[0].color = new Color(0f, 0f, 0f, Alpha);
			YandereRenderer.materials[1].color = new Color(0f, 0f, 0f, Alpha);
			YandereRenderer.materials[2].color = new Color(0f, 0f, 0f, Alpha);
			RivalRenderer.materials[0].color = new Color(0f, 0f, 0f, Alpha);
			RivalRenderer.materials[1].color = new Color(0f, 0f, 0f, Alpha);
			RivalRenderer.materials[2].color = new Color(0f, 0f, 0f, Alpha);
			if (YandereAnimation["f02_" + WeaponName[Weapon] + "LowSanityA_00"].time == YandereAnimation["f02_" + WeaponName[Weapon] + "LowSanityA_00"].length || Yandere.Aiming)
			{
				MakeTransparent();
				Hallucinate = false;
			}
		}
	}

	private void MakeTransparent()
	{
		Alpha = 0f;
		YandereHairRenderer.material.color = new Color(0f, 0f, 0f, Alpha);
		RivalHairRenderer.material.color = new Color(0f, 0f, 0f, Alpha);
		YandereRenderer.materials[0].color = new Color(0f, 0f, 0f, Alpha);
		YandereRenderer.materials[1].color = new Color(0f, 0f, 0f, Alpha);
		YandereRenderer.materials[2].color = new Color(0f, 0f, 0f, Alpha);
		RivalRenderer.materials[0].color = new Color(0f, 0f, 0f, Alpha);
		RivalRenderer.materials[1].color = new Color(0f, 0f, 0f, Alpha);
		RivalRenderer.materials[2].color = new Color(0f, 0f, 0f, Alpha);
	}
}
