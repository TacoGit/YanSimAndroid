Shader "Unlit/Transparent Colored" {
	Properties {
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	//DummyShaderTextExporter // Shader disabled by tanos
	SubShader{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
#pragma surface surf Standard alpha:fade
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