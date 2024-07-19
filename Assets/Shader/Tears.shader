Shader "Tears" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TearOpacity ("TearOpacity", Float) = 1
		_AnimSpeed ("AnimSpeed", Float) = 1
		_AnimFreq ("AnimFreq", Float) = 1
		_AnimPower ("AnimPower", Float) = 1
		_ShrinkSize ("Shrink", Range(0, 0.003)) = 0
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
	Fallback "Toon/Basic"
}