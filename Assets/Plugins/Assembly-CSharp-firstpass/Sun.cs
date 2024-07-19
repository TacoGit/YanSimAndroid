using System;
using UnityEngine;

public class Sun : MonoBehaviour
{
	public GameObject mainManager;

	private skydomeScript2 skydomeScript;

	public Vector3 m_vDirection;

	public Vector3 m_vColor;

	private Vector3 sunDirection = default(Vector3);

	private Vector3 sunDirection2 = default(Vector3);

	private float SolarAzimuth;

	private float solarAltitude;

	private Vector3 sunPosition;

	public float domeRadius;

	public float m_fTheta;

	public float m_fPhi;

	private float LATITUDE_RADIANS;

	private float STD_MERIDIAN;

	private void Start()
	{
		skydomeScript = mainManager.GetComponent(typeof(skydomeScript2)) as skydomeScript2;
	}

	private void Update()
	{
		skydomeScript.LATITUDE = Mathf.Clamp(skydomeScript.LATITUDE, -90f, 90f);
		SetPosition2(skydomeScript.TIME);
		base.transform.position = sunPosition;
	}

	private void SetPosition(float fTheta, float fPhi)
	{
		m_fTheta = fTheta;
		m_fPhi = fPhi;
		float y = Mathf.Cos(m_fTheta);
		float num = Mathf.Sin(m_fTheta);
		float num2 = Mathf.Cos(m_fPhi);
		float num3 = Mathf.Sin(m_fPhi);
		m_vDirection = new Vector3(num * num2, y, num * num3);
		float num4 = (float)Math.PI * 2f - SolarAzimuth;
		sunDirection.x = domeRadius;
		sunDirection.y = num4;
		sunDirection.z = solarAltitude;
		sunPosition = sphericalToCartesian(sunDirection);
		sunDirection2 = calcDirection(m_fTheta, num4);
		m_vDirection = Vector3.Normalize(m_vDirection);
		base.transform.LookAt(sunDirection2);
		ComputeAttenuation();
	}

	private void SetPosition2(float fTime)
	{
		float jULIANDATE = skydomeScript.JULIANDATE;
		float num = skydomeScript.MERIDIAN * 15f;
		float f = (float)Math.PI / 180f * skydomeScript.LATITUDE;
		float lONGITUDE = skydomeScript.LONGITUDE;
		float num2 = fTime + 0.17f * Mathf.Sin((float)Math.PI * 4f * (jULIANDATE - 80f) / 373f) - 0.129f * Mathf.Sin((float)Math.PI * 2f * (jULIANDATE - 8f) / 355f) + 12f * (num - lONGITUDE) / (float)Math.PI;
		float f2 = 0.4093f * Mathf.Sin((float)Math.PI * 2f * (jULIANDATE - 81f) / 368f);
		float num3 = Mathf.Sin(f);
		float num4 = Mathf.Cos(f);
		float num5 = Mathf.Sin(f2);
		float num6 = Mathf.Cos(f2);
		float num7 = Mathf.Sin((float)Math.PI * num2 / 12f);
		float num8 = Mathf.Cos((float)Math.PI * num2 / 12f);
		float fTheta = (float)Math.PI / 2f - Mathf.Asin(num3 * num5 - num4 * num6 * num8);
		float fPhi = Mathf.Atan((0f - num6) * num7 / (num4 * num5 - num3 * num6 * num8));
		float y = (0f - num6) * num7;
		float x = 0f - (num4 * num5 + num3 * num6 * num8);
		SolarAzimuth = Mathf.Atan2(y, x);
		solarAltitude = Mathf.Asin(num3 * num5 - num4 * num6 * num8);
		SetPosition(fTheta, fPhi);
	}

	private Vector3 calcDirection(float thetaSun, float phiSun)
	{
		Vector3 vector = default(Vector3);
		vector.x = Mathf.Cos((float)Math.PI / 2f - thetaSun) * Mathf.Cos(phiSun);
		vector.y = Mathf.Sin((float)Math.PI / 2f - thetaSun);
		vector.z = Mathf.Cos((float)Math.PI / 2f - thetaSun) * Mathf.Sin(phiSun);
		return vector.normalized;
	}

	private Vector3 sphericalToCartesian(Vector3 sunDir)
	{
		Vector3 result = default(Vector3);
		result.y = sunDir.x * Mathf.Sin(sunDir.z);
		float num = sunDir.x * Mathf.Cos(sunDir.z);
		result.x = num * Mathf.Cos(sunDir.y);
		result.z = num * Mathf.Sin(sunDir.y);
		return result;
	}

	private void ComputeAttenuation()
	{
		float num = 0.04608366f * skydomeScript.m_fTurbidity - 0.04586026f;
		float[] array = new float[3];
		float num2 = 93.885f - m_fTheta / (float)Math.PI * 180f;
		float num3 = 1f / (Mathf.Cos(m_fTheta) + 0.15f * num2);
		float[] array2 = new float[3] { 0.65f, 0.57f, 0.475f };
		for (int i = 0; i < 3; i++)
		{
			float num4 = Mathf.Exp((0f - num3) * 0.008735f * Mathf.Pow(array2[i], -4.08f));
			if (num3 < 0f)
			{
				array[i] = 0f;
				continue;
			}
			float num5 = Mathf.Exp((0f - num3) * num * Mathf.Pow(array2[i], -1.3f));
			array[i] = num4 * num5;
		}
		RenderSettings.fogColor = new Color(array[0], array[1], array[2]);
		m_vColor = new Vector3(array[0], array[1], array[2]);
		GetComponent<Light>().color = new Color(array[0], array[1], array[2]);
	}
}
