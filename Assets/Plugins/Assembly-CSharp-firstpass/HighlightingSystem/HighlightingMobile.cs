using UnityEngine;

namespace HighlightingSystem
{
	public class HighlightingMobile : HighlightingBase
	{
		private void OnRenderImage(RenderTexture src, RenderTexture dst)
		{
			RenderHighlighting(src);
			BlitHighlighting(src, dst);
		}
	}
}
