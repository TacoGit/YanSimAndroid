Shader "Hidden/Post FX/Uber Shader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_AutoExposure ("", 2D) = "" {}
		_BloomTex ("", 2D) = "" {}
		_Bloom_DirtTex ("", 2D) = "" {}
		_GrainTex ("", 2D) = "" {}
		_LogLut ("", 2D) = "" {}
		_UserLut ("", 2D) = "" {}
		_Vignette_Mask ("", 2D) = "" {}
		_ChromaticAberration_Spectrum ("", 2D) = "" {}
		_DitheringTex ("", 2D) = "" {}
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