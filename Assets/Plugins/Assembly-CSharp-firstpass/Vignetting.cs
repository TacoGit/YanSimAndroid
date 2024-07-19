using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
public class Vignetting : PostEffectsBase
{
	public enum AberrationMode
	{
		Simple = 0,
		Advanced = 1
	}

	public AberrationMode mode;

	public float intensity = 0.375f;

	public float chromaticAberration = 0.2f;

	public float axialAberration = 0.5f;

	public float blur;

	public float blurSpread = 0.75f;

	public float luminanceDependency = 0.25f;

	public float blurDistance = 2.5f;

	public Shader vignetteShader;

	private Material vignetteMaterial;

	public Shader separableBlurShader;

	private Material separableBlurMaterial;

	public Shader chromAberrationShader;

	private Material chromAberrationMaterial;

	protected override bool CheckResources()
	{
		CheckSupport(false);
		vignetteMaterial = CheckShaderAndCreateMaterial(vignetteShader, vignetteMaterial);
		separableBlurMaterial = CheckShaderAndCreateMaterial(separableBlurShader, separableBlurMaterial);
		chromAberrationMaterial = CheckShaderAndCreateMaterial(chromAberrationShader, chromAberrationMaterial);
		if (!isSupported)
		{
			ReportAutoDisable();
		}
		return isSupported;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		int width = source.width;
		int height = source.height;
		bool flag = Mathf.Abs(blur) > 0f || Mathf.Abs(intensity) > 0f;
		float num = (float)width / (float)height;
		float num2 = 0.001953125f;
		RenderTexture renderTexture = null;
		RenderTexture renderTexture2 = null;
		RenderTexture renderTexture3 = null;
		if (flag)
		{
			renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
			if (Mathf.Abs(blur) > 0f)
			{
				renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
				Graphics.Blit(source, renderTexture2, chromAberrationMaterial, 0);
				for (int i = 0; i < 2; i++)
				{
					separableBlurMaterial.SetVector("offsets", new Vector4(0f, blurSpread * num2, 0f, 0f));
					renderTexture3 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
					Graphics.Blit(renderTexture2, renderTexture3, separableBlurMaterial);
					RenderTexture.ReleaseTemporary(renderTexture2);
					separableBlurMaterial.SetVector("offsets", new Vector4(blurSpread * num2 / num, 0f, 0f, 0f));
					renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
					Graphics.Blit(renderTexture3, renderTexture2, separableBlurMaterial);
					RenderTexture.ReleaseTemporary(renderTexture3);
				}
			}
			vignetteMaterial.SetFloat("_Intensity", intensity);
			vignetteMaterial.SetFloat("_Blur", blur);
			vignetteMaterial.SetTexture("_VignetteTex", renderTexture2);
			Graphics.Blit(source, renderTexture, vignetteMaterial, 0);
		}
		chromAberrationMaterial.SetFloat("_ChromaticAberration", chromaticAberration);
		chromAberrationMaterial.SetFloat("_AxialAberration", axialAberration);
		chromAberrationMaterial.SetVector("_BlurDistance", new Vector2(0f - blurDistance, blurDistance));
		chromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, luminanceDependency));
		if (flag)
		{
			renderTexture.wrapMode = TextureWrapMode.Clamp;
		}
		else
		{
			source.wrapMode = TextureWrapMode.Clamp;
		}
		Graphics.Blit((!flag) ? source : renderTexture, destination, chromAberrationMaterial, (mode != AberrationMode.Advanced) ? 1 : 2);
		RenderTexture.ReleaseTemporary(renderTexture);
		RenderTexture.ReleaseTemporary(renderTexture2);
	}
}
