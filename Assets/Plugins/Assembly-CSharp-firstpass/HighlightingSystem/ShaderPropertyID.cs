using UnityEngine;

namespace HighlightingSystem
{
	public static class ShaderPropertyID
	{
		private static bool initialized;

		public static int _MainTex { get; private set; }

		public static int _Outline { get; private set; }

		public static int _Cutoff { get; private set; }

		public static int _Intensity { get; private set; }

		public static int _OffsetScale { get; private set; }

		public static int _ZTest { get; private set; }

		public static int _StencilRef { get; private set; }

		public static int _HighlightingZWrite { get; private set; }

		public static int _HighlightingOffsetFactor { get; private set; }

		public static int _HighlightingOffsetUnits { get; private set; }

		public static void Initialize()
		{
			if (!initialized)
			{
				_MainTex = Shader.PropertyToID("_MainTex");
				_Outline = Shader.PropertyToID("_Outline");
				_Cutoff = Shader.PropertyToID("_Cutoff");
				_Intensity = Shader.PropertyToID("_Intensity");
				_OffsetScale = Shader.PropertyToID("_OffsetScale");
				_ZTest = Shader.PropertyToID("_ZTest");
				_StencilRef = Shader.PropertyToID("_StencilRef");
				_HighlightingZWrite = Shader.PropertyToID("_HighlightingZWrite");
				_HighlightingOffsetFactor = Shader.PropertyToID("_HighlightingOffsetFactor");
				_HighlightingOffsetUnits = Shader.PropertyToID("_HighlightingOffsetUnits");
				initialized = true;
			}
		}
	}
}
