Shader "aScattering 2.0" {
	Properties {
		DirectionalityFactor ("DirectionalityFactor", Float) = 0
		SunColorIntensity ("SunColorIntensity", Float) = 0
		tint ("tint", Float) = 1
		fade ("Cloud fade height", Float) = 0
		cloudSpeed1 ("cloudSpeed1", Float) = 1
		cloudSpeed2 ("cloudSpeed2", Float) = 1
		plane_height1 ("cloud plane height 1", Float) = 1
		plane_height2 ("cloud plane height 2", Float) = 1
		noisetex ("noise texture", 2D) = "white" {}
		starTexture ("starTexture", 2D) = "white" {}
		LightDir ("LightDir", Vector) = (0,0,0,1)
		vBetaRayleigh ("vBetaRayleigh", Vector) = (0,0,0,1)
		BetaRayTheta ("BetaRayTheta", Vector) = (0,0,0,1)
		vBetaMie ("vBetaMie", Vector) = (0,0,0,1)
		BetaMieTheta ("BetaMieTheta", Vector) = (0,0,0,1)
		g_vEyePt ("g_vEyePt", Vector) = (0,0,0,1)
		g_vSunColor ("g_vSunColor", Vector) = (0,0,0,1)
		wind_direction ("winddirection", Vector) = (0,0,0,0)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "None"
}