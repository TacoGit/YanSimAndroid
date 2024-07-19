using UnityEngine;

public class ZoomScript : MonoBehaviour
{
	public CardboardBoxScript CardboardBox;

	public RPG_Camera CameraScript;

	public YandereScript Yandere;

	public float TargetZoom;

	public float Zoom;

	public float ShakeStrength;

	public float Slender;

	public float Height;

	public float Timer;

	public Vector3 Target;

	public bool OverShoulder;

	public GameObject TallHat;

	private void Update()
	{
		if (Yandere.FollowHips)
		{
			base.transform.position = new Vector3(Mathf.MoveTowards(base.transform.position.x, Yandere.Hips.position.x, Time.deltaTime), base.transform.position.y, Mathf.MoveTowards(base.transform.position.z, Yandere.Hips.position.z, Time.deltaTime));
		}
		if (Yandere.Stance.Current == StanceType.Crawling)
		{
			Height = 0.05f;
		}
		else if (Yandere.Stance.Current == StanceType.Crouching)
		{
			Height = 0.4f;
		}
		else
		{
			Height = 1f;
		}
		if (!Yandere.FollowHips)
		{
			if (Yandere.FlameDemonic)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Lerp(base.transform.localPosition.y, Height + Zoom + 0.4f, Time.deltaTime * 10f), base.transform.localPosition.z);
			}
			else if (Yandere.Slender)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Lerp(base.transform.localPosition.y, Height + Zoom + Slender, Time.deltaTime * 10f), base.transform.localPosition.z);
			}
			else if (Yandere.Stand.Stand.activeInHierarchy)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Lerp(base.transform.localPosition.y, Height - Zoom * 0.5f + Slender * 0.5f, Time.deltaTime * 10f), base.transform.localPosition.z);
			}
			else
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Lerp(base.transform.localPosition.y, Height + Zoom, Time.deltaTime * 10f), base.transform.localPosition.z);
			}
		}
		else if (!Yandere.SithLord)
		{
			base.transform.position = new Vector3(base.transform.position.x, Mathf.MoveTowards(base.transform.position.y, Yandere.Hips.position.y + Zoom, Time.deltaTime * 10f), base.transform.position.z);
		}
		else
		{
			base.transform.position = new Vector3(base.transform.position.x, Mathf.MoveTowards(base.transform.position.y, Yandere.Hips.position.y, Time.deltaTime * 10f), base.transform.position.z);
		}
		if (!Yandere.Aiming)
		{
			TargetZoom += Input.GetAxis("Mouse ScrollWheel");
		}
		if (Yandere.SithLord)
		{
			Slender = Mathf.Lerp(Slender, 2.5f, Time.deltaTime);
		}
		else if (Yandere.Slender || Yandere.Stand.Stand.activeInHierarchy || Yandere.Blasting || Yandere.PK || Yandere.Shipgirl || TallHat.activeInHierarchy || Yandere.Man.activeInHierarchy || Yandere.Pod.activeInHierarchy || Yandere.LucyHelmet.activeInHierarchy)
		{
			Slender = Mathf.Lerp(Slender, 0.5f, Time.deltaTime);
		}
		else
		{
			Slender = Mathf.Lerp(Slender, 0f, Time.deltaTime);
		}
		if (TargetZoom < 0f)
		{
			TargetZoom = 0f;
		}
		if (Yandere.Stance.Current == StanceType.Crawling)
		{
			if (TargetZoom > 0.3f)
			{
				TargetZoom = 0.3f;
			}
		}
		else if (TargetZoom > 0.4f)
		{
			TargetZoom = 0.4f;
		}
		Zoom = Mathf.Lerp(Zoom, TargetZoom, Time.deltaTime);
		if (!Yandere.Possessed)
		{
			CameraScript.distance = 2f - Zoom * 3.33333f + Slender;
			CameraScript.distanceMax = 2f - Zoom * 3.33333f + Slender;
			CameraScript.distanceMin = 2f - Zoom * 3.33333f + Slender;
			if (Yandere.TornadoHair.activeInHierarchy || CardboardBox.transform.parent == Yandere.Hips)
			{
				CameraScript.distanceMax += 3f;
			}
		}
		else
		{
			CameraScript.distance = 5f;
			CameraScript.distanceMax = 5f;
		}
		if (!Yandere.TimeSkipping)
		{
			Timer += Time.deltaTime;
			ShakeStrength = Mathf.Lerp(ShakeStrength, 1f - Yandere.Sanity * 0.01f, Time.deltaTime);
			if (Timer > 0.1f + Yandere.Sanity * 0.01f)
			{
				Target.x = Random.Range(0f - ShakeStrength, ShakeStrength);
				Target.y = base.transform.localPosition.y;
				Target.z = Random.Range(0f - ShakeStrength, ShakeStrength);
				Timer = 0f;
			}
		}
		else
		{
			Target = new Vector3(0f, base.transform.localPosition.y, 0f);
		}
		if (Yandere.RoofPush)
		{
			base.transform.position = new Vector3(Mathf.MoveTowards(base.transform.position.x, Yandere.Hips.position.x, Time.deltaTime * 10f), base.transform.position.y, Mathf.MoveTowards(base.transform.position.z, Yandere.Hips.position.z, Time.deltaTime * 10f));
		}
		else
		{
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, Target, Time.deltaTime * ShakeStrength * 0.1f);
		}
		base.transform.localPosition = new Vector3((!OverShoulder) ? 0f : 0.25f, base.transform.localPosition.y, base.transform.localPosition.z);
	}
}
