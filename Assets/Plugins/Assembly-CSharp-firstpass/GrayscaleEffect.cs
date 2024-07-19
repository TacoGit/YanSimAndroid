using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
public class GrayscaleEffect : ImageEffectBase
{
	public Texture textureRamp;

	public float rampOffset;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetTexture("_RampTex", textureRamp);
		base.material.SetFloat("_RampOffset", rampOffset);
		Graphics.Blit(source, destination, base.material);
	}
}
