using UnityEngine;

namespace HighlightingSystem
{
	public class HighlightingRenderer : HighlightingBase
	{
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture src, RenderTexture dst)
		{
			RenderHighlighting(src);
			Graphics.Blit(src, dst, HighlightingBase.blitMaterial);
		}
	}
}
