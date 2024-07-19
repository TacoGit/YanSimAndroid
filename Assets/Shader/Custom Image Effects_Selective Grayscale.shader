Shader "Custom Image Effects/Selective Grayscale" {
	Properties {
		_MainTex ("", 2D) = "white" {}
		_FilterColor ("Filter Color", Vector) = (1,0,0,1)
		_Sensitivity ("Sensitivity", Range(0, 1)) = 0.5
		_Tolerance ("Tolerance", Range(0, 1)) = 0.2
		_Desaturation ("Desaturation", Range(0, 1)) = 1
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
	Fallback "Diffuse"
}