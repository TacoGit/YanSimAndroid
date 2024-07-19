Shader "Hidden/FXAA III (Console)" {
	Properties {
		_MainTex ("-", 2D) = "white" {}
		_EdgeThresholdMin ("Edge threshold min", Float) = 0.125
		_EdgeThreshold ("Edge Threshold", Float) = 0.25
		_EdgeSharpness ("Edge sharpness", Float) = 4
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}