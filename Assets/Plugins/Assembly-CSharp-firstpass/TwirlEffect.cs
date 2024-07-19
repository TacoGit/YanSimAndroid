using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Displacement/Twirl")]
public class TwirlEffect : ImageEffectBase
{
	public Vector2 radius = new Vector2(0.3f, 0.3f);

	public float angle = 50f;

	public Vector2 center = new Vector2(0.5f, 0.5f);

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		ImageEffects.RenderDistortion(base.material, source, destination, angle, center, radius);
	}
}
