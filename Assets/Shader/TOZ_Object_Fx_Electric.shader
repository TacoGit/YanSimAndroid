Shader "TOZ/Object/Fx/Electric" {
	Properties {
		_Color ("Electric Color", Vector) = (1,0.5,0.5,1)
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Distance ("Sample Distance", Range(0, 5)) = 0.02
		_Speed ("Speed", Range(0, 10)) = 0.25
		_Noise ("Noise Amount", Range(-10, 10)) = 0.1
		_Height ("Wave Height", Range(0, 1)) = 0.1
		_Glow ("Glow Amount", Float) = 0.1
		_GlowHeight ("Glow Height", Float) = 15
		_GlowFallOff ("Glow Falloff", Float) = 0.05
		_GlowPower ("Glow Power", Float) = 150
		_UvXScale ("Uv X Scale", Range(-1, 2)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
}