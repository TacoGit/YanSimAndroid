Shader "Transparent/Refractive" {
	Properties {
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
		_BumpMap ("Normal Map (RGB)", 2D) = "bump" {}
		_Mask ("Specularity (R), Shininess (G), Refraction (B)", 2D) = "black" {}
		_Color ("Color Tint", Vector) = (1,1,1,1)
		_Specular ("Specular Color", Vector) = (0,0,0,0)
		_Focus ("Focus", Range(-100, 100)) = -100
		_Shininess ("Shininess", Range(0.01, 1)) = 0.2
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
}