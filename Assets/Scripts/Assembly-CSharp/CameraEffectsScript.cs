using UnityEngine;

public class CameraEffectsScript : MonoBehaviour
{
	public YandereScript Yandere;

	public Vignetting Vignette;

	public UITexture MurderStreaks;

	public UITexture Streaks;

	public Bloom AlarmBloom;

	public float EffectStrength;

	public Bloom QualityBloom;

	public Vignetting QualityVignetting;

	public AntialiasingAsPostEffect QualityAntialiasingAsPostEffect;

	public bool OneCamera;

	public AudioClip MurderNoticed;

	public AudioClip SenpaiNoticed;

	public AudioClip Noticed;

	private void Start()
	{
		MurderStreaks.color = new Color(MurderStreaks.color.r, MurderStreaks.color.g, MurderStreaks.color.b, 0f);
		Streaks.color = new Color(Streaks.color.r, Streaks.color.g, Streaks.color.b, 0f);
	}

	private void Update()
	{
		if (Streaks.color.a > 0f)
		{
			AlarmBloom.bloomIntensity -= Time.deltaTime;
			Streaks.color = new Color(Streaks.color.r, Streaks.color.g, Streaks.color.b, Streaks.color.a - Time.deltaTime);
			if (Streaks.color.a <= 0f)
			{
				AlarmBloom.enabled = false;
			}
		}
		if (MurderStreaks.color.a > 0f)
		{
			MurderStreaks.color = new Color(MurderStreaks.color.r, MurderStreaks.color.g, MurderStreaks.color.b, MurderStreaks.color.a - Time.deltaTime);
		}
		EffectStrength = 1f - Yandere.Sanity * 0.01f;
		Vignette.intensity = Mathf.Lerp(Vignette.intensity, EffectStrength * 5f, Time.deltaTime);
		Vignette.blur = Mathf.Lerp(Vignette.blur, EffectStrength, Time.deltaTime);
		Vignette.chromaticAberration = Mathf.Lerp(Vignette.chromaticAberration, EffectStrength * 5f, Time.deltaTime);
	}

	public void Alarm()
	{
		AlarmBloom.bloomIntensity = 1f;
		Streaks.color = new Color(Streaks.color.r, Streaks.color.g, Streaks.color.b, 1f);
		AlarmBloom.enabled = true;
		Yandere.Jukebox.SFX.PlayOneShot(Noticed);
	}

	public void MurderWitnessed()
	{
		MurderStreaks.color = new Color(MurderStreaks.color.r, MurderStreaks.color.g, MurderStreaks.color.b, 1f);
		Yandere.Jukebox.SFX.PlayOneShot((!Yandere.Noticed) ? MurderNoticed : SenpaiNoticed);
	}

	public void DisableCamera()
	{
		if (!OneCamera)
		{
			OneCamera = true;
		}
		else
		{
			OneCamera = false;
		}
	}
}
