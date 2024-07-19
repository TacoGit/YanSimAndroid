Shader "Toon/Lighted Outline Overlay Multiple Textures EyesAnim" {
	Properties {
		_Color ("Main Color", Vector) = (0.5,0.5,0.5,1)
		_OutlineColor ("Outline Color", Vector) = (0,0,0,1)
		_Outline ("Outline width", Range(0.002, 0.03)) = 0.005
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_OverlayTex ("Overlay Texture (RGBA)", 2D) = "white" {}
		_BlendAmount ("Blend Amount", Range(0, 1)) = 0
		_OverlayTex1 ("Overlay Texture 1 (RGBA)", 2D) = "white" {}
		_BlendAmount1 ("Blend Amount 1", Range(0, 1)) = 0
		_AnimSpeed ("AnimSpeed", Float) = 1
		_AnimPower ("AnimPower", Range(1, 0.994)) = 0.997
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
	Fallback "Toon/Basic"
}