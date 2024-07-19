using System;
using UnityEngine;

public class skydomeScript2 : MonoBehaviour
{
	public Light sunLight;

	public GameObject SkyDome;

	public Camera cam;

	private Sun sunlightScript;

	public bool debug;

	public float JULIANDATE = 150f;

	public float LONGITUDE;

	public float LATITUDE;

	public float MERIDIAN;

	public float TIME = 8f;

	public float m_fTurbidity = 2f;

	public float cloudSpeed1 = 1f;

	public float cloudSpeed2 = 1.5f;

	public float cloudHeight1 = 12f;

	public float cloudHeight2 = 13f;

	public float cloudTint = 1f;

	private Vector4 vBetaRayleigh = default(Vector4);

	private Vector4 vBetaMie = default(Vector4);

	private Vector3 m_vBetaRayTheta = default(Vector3);

	private Vector3 m_vBetaMieTheta = default(Vector3);

	public float m_fRayFactor = 1000f;

	public float m_fMieFactor = 0.7f;

	public float m_fDirectionalityFactor = 0.6f;

	public float m_fSunColorIntensity = 1f;

	private void Start()
	{
		sunlightScript = sunLight.GetComponent(typeof(Sun)) as Sun;
	}

	private void Update()
	{
		calcAtmosphere();
		if (Input.GetKeyDown("tab"))
		{
		}
		Vector3 vector = sunLight.transform.TransformDirection(-Vector3.forward);
		Vector3 position = cam.transform.position;
		SkyDome.GetComponent<Renderer>().material.SetVector("vBetaRayleigh", vBetaRayleigh);
		SkyDome.GetComponent<Renderer>().material.SetVector("BetaRayTheta", m_vBetaRayTheta);
		SkyDome.GetComponent<Renderer>().material.SetVector("vBetaMie", vBetaMie);
		SkyDome.GetComponent<Renderer>().material.SetVector("BetaMieTheta", m_vBetaMieTheta);
		SkyDome.GetComponent<Renderer>().material.SetVector("g_vEyePt", position);
		SkyDome.GetComponent<Renderer>().material.SetVector("LightDir", vector);
		SkyDome.GetComponent<Renderer>().material.SetVector("g_vSunColor", sunlightScript.m_vColor);
		SkyDome.GetComponent<Renderer>().material.SetFloat("DirectionalityFactor", m_fDirectionalityFactor);
		SkyDome.GetComponent<Renderer>().material.SetFloat("SunColorIntensity", m_fSunColorIntensity);
		SkyDome.GetComponent<Renderer>().material.SetFloat("tint", cloudTint);
		SkyDome.GetComponent<Renderer>().material.SetFloat("cloudSpeed1", cloudSpeed1);
		SkyDome.GetComponent<Renderer>().material.SetFloat("cloudSpeed2", cloudSpeed2);
		SkyDome.GetComponent<Renderer>().material.SetFloat("plane_height1", cloudHeight1);
		SkyDome.GetComponent<Renderer>().material.SetFloat("plane_height2", cloudHeight2);
	}

	private void calcAtmosphere()
	{
		calcRay();
		CalculateMieCoeff();
	}

	private void calcRay()
	{
		float num = m_fRayFactor * (Mathf.Pow((float)Math.PI, 2f) * Mathf.Pow(0.0005801565f, 2f) * 6.105f) / 1.4646475E+26f;
		m_vBetaRayTheta.x = num / (2f * Mathf.Pow(6.5E-07f, 4f));
		m_vBetaRayTheta.y = num / (2f * Mathf.Pow(5.7E-07f, 4f));
		m_vBetaRayTheta.z = num / (2f * Mathf.Pow(4.75E-07f, 4f));
		vBetaRayleigh.x = 8f * num / (3f * Mathf.Pow(6.5E-07f, 4f));
		vBetaRayleigh.y = 8f * num / (3f * Mathf.Pow(5.7E-07f, 4f));
		vBetaRayleigh.z = 8f * num / (3f * Mathf.Pow(4.75E-07f, 4f));
	}

	private void CalculateMieCoeff()
	{
		float[] array = new float[3] { 0.685f, 0.679f, 0.67f };
		float num = (0.6544f * m_fTurbidity - 0.651f) * 1E-16f;
		float num2 = m_fMieFactor * 0.434f * num * 4f * (float)Math.PI * (float)Math.PI;
		m_vBetaMieTheta.x = num2 / (2f * Mathf.Pow(6.5E-07f, 2f));
		m_vBetaMieTheta.y = num2 / (2f * Mathf.Pow(5.7E-07f, 2f));
		m_vBetaMieTheta.z = num2 / (2f * Mathf.Pow(4.75E-07f, 2f));
		vBetaMie.x = array[0] * num2 / Mathf.Pow(6.5E-07f, 2f);
		vBetaMie.y = array[1] * num2 / Mathf.Pow(5.7E-07f, 2f);
		vBetaMie.z = array[2] * num2 / Mathf.Pow(4.75E-07f, 2f);
		float num3 = 0.434f * num * (float)Math.PI * ((float)Math.PI * 2f) * ((float)Math.PI * 2f);
		vBetaMie *= num3;
	}

	private void OnGUI()
	{
		if (debug)
		{
			GUILayout.BeginArea(new Rect(0f, 10f, 600f, 400f));
			GUILayout.BeginHorizontal();
			GUILayout.Label("Time : " + TIME, GUILayout.Width(200f));
			TIME = GUILayout.HorizontalSlider(TIME, 0f, 23f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("SunColorIntensity : " + m_fSunColorIntensity, GUILayout.Width(200f));
			m_fSunColorIntensity = GUILayout.HorizontalSlider(m_fSunColorIntensity, 0f, 2f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("DirectionalityFactor : " + m_fDirectionalityFactor, GUILayout.Width(200f));
			m_fDirectionalityFactor = GUILayout.HorizontalSlider(m_fDirectionalityFactor, 0f, 1f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Rayleigh multiplier : " + m_fRayFactor, GUILayout.Width(200f));
			m_fRayFactor = GUILayout.HorizontalSlider(m_fRayFactor, 0f, 10000f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Mie multiplier : " + m_fMieFactor, GUILayout.Width(200f));
			m_fMieFactor = GUILayout.HorizontalSlider(m_fMieFactor, 0f, 5f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("cloudTint : " + cloudTint, GUILayout.Width(200f));
			cloudTint = GUILayout.HorizontalSlider(cloudTint, 0f, 2f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("cloudSpeed1 : " + cloudSpeed1, GUILayout.Width(200f));
			cloudSpeed1 = GUILayout.HorizontalSlider(cloudSpeed1, 0f, 6f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("cloudSpeed2 : " + cloudSpeed2, GUILayout.Width(200f));
			cloudSpeed2 = GUILayout.HorizontalSlider(cloudSpeed2, 0f, 6f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("cloudHeight1 : " + cloudHeight1, GUILayout.Width(200f));
			cloudHeight1 = GUILayout.HorizontalSlider(cloudHeight1, 10f, 20f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("cloudHeight2 : " + cloudHeight2, GUILayout.Width(200f));
			cloudHeight2 = GUILayout.HorizontalSlider(cloudHeight2, 10f, 20f);
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}
}
