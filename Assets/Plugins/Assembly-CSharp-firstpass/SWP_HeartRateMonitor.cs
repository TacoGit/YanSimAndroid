using System;
using System.Collections;
using UnityEngine;

public class SWP_HeartRateMonitor : MonoBehaviour
{
	private enum SoundType
	{
		HeartBeat1 = 0,
		HeartBeat2 = 1,
		Flatline = 2
	}

	public int BeatsPerMinute = 90;

	public bool FlatLine;

	public bool ShowBlip = true;

	public GameObject Blip;

	public float BlipSize = 1f;

	public float BlipTrailStartSize = 0.2f;

	public float BlipTrailEndSize = 0.1f;

	public float BlipMonitorWidth = 40f;

	public float BlipMonitorHeightModifier = 1f;

	public bool EnableSound = true;

	public float SoundVolume = 1f;

	public AudioClip Heart1Sound;

	public AudioClip Heart2Sound;

	public AudioClip FlatlineSound;

	private bool bFlatLinePlayed;

	private float LineSpeed = 0.3f;

	private GameObject NewClone;

	private float TrailTime;

	private float BeatsPerSecond;

	private float LastUpdate;

	private Vector3 BlipOffset = Vector3.zero;

	private float DisplayXEnd;

	public Material MainMaterial;

	public Color NormalColour = new Color(0f, 1f, 0f, 1f);

	public Color MediumColour = new Color(1f, 1f, 0f, 1f);

	public Color BadColour = new Color(1f, 0f, 0f, 1f);

	public Color FlatlineColour = new Color(1f, 0f, 0f, 1f);

	public bool DeltaTime;

	private float TimeFactor;

	private void Start()
	{
		BeatsPerSecond = 60f / (float)BeatsPerMinute;
		BlipOffset = new Vector3(base.transform.position.x - BlipMonitorWidth / 2f, base.transform.position.y, base.transform.position.z);
		DisplayXEnd = BlipOffset.x + BlipMonitorWidth;
		CreateClone();
		TrailTime = NewClone.GetComponentInChildren<TrailRenderer>().time;
	}

	private void Update()
	{
		if (!DeltaTime)
		{
			TimeFactor = Time.deltaTime;
		}
		else
		{
			TimeFactor = 0.01666667f;
		}
		BeatsPerSecond = 60f / (float)BeatsPerMinute;
		BlipOffset = new Vector3(base.transform.position.x - BlipMonitorWidth / 2f, base.transform.position.y, base.transform.position.z);
		DisplayXEnd = BlipOffset.x + BlipMonitorWidth;
		if (NewClone.transform.position.x > DisplayXEnd)
		{
			if (NewClone != null)
			{
				GameObject newClone = NewClone;
				StartCoroutine(WaitThenDestroy(newClone));
			}
			CreateClone();
		}
		else if (!FlatLine)
		{
			NewClone.transform.position += new Vector3(BlipMonitorWidth * TimeFactor * LineSpeed, UnityEngine.Random.Range(-0.05f, 0.05f), 0f);
		}
		else
		{
			NewClone.transform.position += new Vector3(BlipMonitorWidth * TimeFactor * LineSpeed, 0f, 0f);
			if (!bFlatLinePlayed)
			{
				PlayHeartSound(SoundType.Flatline, SoundVolume);
				bFlatLinePlayed = true;
			}
		}
		if (BeatsPerMinute <= 0 || FlatLine)
		{
			LastUpdate = Time.time;
		}
		else if (Time.time - LastUpdate >= BeatsPerSecond)
		{
			LastUpdate = Time.time;
			StartCoroutine(PerformBlip());
		}
	}

	private IEnumerator PerformBlip()
	{
		if (bFlatLinePlayed)
		{
			bFlatLinePlayed = false;
		}
		if (!bFlatLinePlayed)
		{
			PlayHeartSound(SoundType.HeartBeat1, SoundVolume);
		}
		NewClone.transform.position = new Vector3(NewClone.transform.position.x, 10f * BlipMonitorHeightModifier + UnityEngine.Random.Range(0f, 2f * BlipMonitorHeightModifier) + BlipOffset.y, BlipOffset.z);
		yield return new WaitForSeconds(0.03f);
		NewClone.transform.position = new Vector3(NewClone.transform.position.x, -5f * BlipMonitorHeightModifier - UnityEngine.Random.Range(0f, 3f * BlipMonitorHeightModifier) + BlipOffset.y, BlipOffset.z);
		yield return new WaitForSeconds(0.02f);
		NewClone.transform.position = new Vector3(NewClone.transform.position.x, 3f * BlipMonitorHeightModifier + UnityEngine.Random.Range(0f, 2f * BlipMonitorHeightModifier) + BlipOffset.y, BlipOffset.z);
		yield return new WaitForSeconds(0.02f);
		NewClone.transform.position = new Vector3(NewClone.transform.position.x, 2f * BlipMonitorHeightModifier + UnityEngine.Random.Range(0f, 1f * BlipMonitorHeightModifier) + BlipOffset.y, BlipOffset.z);
		yield return new WaitForSeconds(0.02f);
		NewClone.transform.position = new Vector3(NewClone.transform.position.x, BlipOffset.y, BlipOffset.z);
		yield return new WaitForSeconds(0.2f);
		if (!bFlatLinePlayed)
		{
			PlayHeartSound(SoundType.HeartBeat2, SoundVolume);
		}
	}

	private void CreateClone()
	{
		NewClone = UnityEngine.Object.Instantiate(Blip, new Vector3(BlipOffset.x, BlipOffset.y, BlipOffset.z), Quaternion.identity);
		NewClone.transform.parent = base.gameObject.transform;
		NewClone.GetComponentInChildren<TrailRenderer>().startWidth = BlipTrailStartSize;
		NewClone.GetComponentInChildren<TrailRenderer>().endWidth = BlipTrailEndSize;
		NewClone.transform.localScale = new Vector3(BlipSize, BlipSize, BlipSize);
		if (ShowBlip)
		{
			NewClone.GetComponent<MeshRenderer>().enabled = true;
		}
		else
		{
			NewClone.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	private IEnumerator WaitThenDestroy(GameObject OldClone)
	{
		OldClone.GetComponent<MeshRenderer>().enabled = false;
		yield return new WaitForSeconds(TrailTime);
		UnityEngine.Object.DestroyObject(OldClone);
	}

	private void PlayHeartSound(SoundType _SoundType, float fSoundVolume)
	{
		if (EnableSound)
		{
			switch (_SoundType)
			{
			case SoundType.HeartBeat1:
				GetComponent<AudioSource>().PlayOneShot(Heart1Sound, fSoundVolume);
				break;
			case SoundType.HeartBeat2:
				GetComponent<AudioSource>().PlayOneShot(Heart2Sound, fSoundVolume);
				break;
			case SoundType.Flatline:
				GetComponent<AudioSource>().PlayOneShot(FlatlineSound, fSoundVolume);
				break;
			}
		}
	}

	public void SetHeartRateColour(Color _NewColour)
	{
		if (MainMaterial == null)
		{
			throw new ArgumentException("You are trying to change the colour without having the 'MainMaterial' set in the control.  It must be set to the 'BlipMaterial' in order to use the colour changer.");
		}
		MainMaterial.SetColor("_TintColor", _NewColour);
	}
}
