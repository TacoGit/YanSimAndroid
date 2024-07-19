Shader "Toon/Cutoff" {
	Properties {
		_Color ("Color(RGBA)", Vector) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Shininess ("Shininess(0.0:)", Float) = 1
		_ShadowThreshold ("Shadow Threshold(0.0:1.0)", Float) = 0.5
		_ShadowColor ("Shadow Color(RGBA)", Vector) = (0,0,0,0.5)
		_ShadowSharpness ("Shadow Sharpness(0.0:)", Float) = 100
		_Cutoff ("Alpha cutoff", Range(0, 1)) = 0.9
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}