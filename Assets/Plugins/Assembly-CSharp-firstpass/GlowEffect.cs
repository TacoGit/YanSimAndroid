using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Bloom and Glow/Glow (Deprecated)")]
public class GlowEffect : MonoBehaviour
{
	public float glowIntensity = 1.5f;

	public int blurIterations = 3;

	public float blurSpread = 0.7f;

	public Color glowTint = new Color(1f, 1f, 1f, 0f);

	public Shader compositeShader;

	private Material m_CompositeMaterial;

	public Shader blurShader;

	private Material m_BlurMaterial;

	public Shader downsampleShader;

	private Material m_DownsampleMaterial;

	protected Material compositeMaterial
	{
		get
		{
			if (m_CompositeMaterial == null)
			{
				m_CompositeMaterial = new Material(compositeShader);
				m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_CompositeMaterial;
		}
	}

	protected Material blurMaterial
	{
		get
		{
			if (m_BlurMaterial == null)
			{
				m_BlurMaterial = new Material(blurShader);
				m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_BlurMaterial;
		}
	}

	protected Material downsampleMaterial
	{
		get
		{
			if (m_DownsampleMaterial == null)
			{
				m_DownsampleMaterial = new Material(downsampleShader);
				m_DownsampleMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_DownsampleMaterial;
		}
	}

	protected void OnDisable()
	{
		if ((bool)m_CompositeMaterial)
		{
			Object.DestroyImmediate(m_CompositeMaterial);
		}
		if ((bool)m_BlurMaterial)
		{
			Object.DestroyImmediate(m_BlurMaterial);
		}
		if ((bool)m_DownsampleMaterial)
		{
			Object.DestroyImmediate(m_DownsampleMaterial);
		}
	}

	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (downsampleShader == null)
		{
			Debug.Log("No downsample shader assigned! Disabling glow.");
			base.enabled = false;
			return;
		}
		if (!blurMaterial.shader.isSupported)
		{
			base.enabled = false;
		}
		if (!compositeMaterial.shader.isSupported)
		{
			base.enabled = false;
		}
		if (!downsampleMaterial.shader.isSupported)
		{
			base.enabled = false;
		}
	}

	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * blurSpread;
		Graphics.BlitMultiTap(source, dest, blurMaterial, new Vector2(num, num), new Vector2(0f - num, num), new Vector2(num, 0f - num), new Vector2(0f - num, 0f - num));
	}

	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		downsampleMaterial.color = new Color(glowTint.r, glowTint.g, glowTint.b, glowTint.a / 4f);
		Graphics.Blit(source, dest, downsampleMaterial);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		glowIntensity = Mathf.Clamp(glowIntensity, 0f, 10f);
		blurIterations = Mathf.Clamp(blurIterations, 0, 30);
		blurSpread = Mathf.Clamp(blurSpread, 0.5f, 1f);
		int width = source.width / 4;
		int height = source.height / 4;
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
		DownSample4x(source, renderTexture);
		float num = Mathf.Clamp01((glowIntensity - 1f) / 4f);
		blurMaterial.color = new Color(1f, 1f, 1f, 0.25f + num);
		for (int i = 0; i < blurIterations; i++)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
			FourTapCone(renderTexture, temporary, i);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		Graphics.Blit(source, destination);
		BlitGlow(renderTexture, destination);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	public void BlitGlow(RenderTexture source, RenderTexture dest)
	{
		compositeMaterial.color = new Color(1f, 1f, 1f, Mathf.Clamp01(glowIntensity));
		Graphics.Blit(source, dest, compositeMaterial);
	}
}
