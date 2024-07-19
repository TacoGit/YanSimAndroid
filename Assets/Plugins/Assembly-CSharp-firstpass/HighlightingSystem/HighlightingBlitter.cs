using UnityEngine;

namespace HighlightingSystem
{
	public class HighlightingBlitter : MonoBehaviour
	{
		public HighlightingRenderer highlightingRenderer;

		private void LateUpdate()
		{
			base.enabled = highlightingRenderer != null && highlightingRenderer.enabled;
		}

		private void OnRenderImage(RenderTexture src, RenderTexture dst)
		{
			if (highlightingRenderer == null || !highlightingRenderer.enabled)
			{
				Graphics.Blit(src, dst);
				Debug.LogWarning("HighlightingSystem : HighlightingRenderer component is not assigned or not enabled. Disabling HighlightingBlitter.");
				base.enabled = false;
			}
			else
			{
				highlightingRenderer.BlitHighlighting(src, dst);
			}
		}
	}
}
