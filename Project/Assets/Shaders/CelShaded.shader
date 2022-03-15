// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CelShaded"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		[Normal]_Normal("Normal", 2D) = "bump" {}

		[Header(Specular)]
		_SpecularMap("Specular map", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HDR]_SpecularColor("Specular color", Color) = (0,0,0,1)

		[Header(Emission)]
		[HDR]_Emission("Emission", Color) = (0,0,0,1)
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque"  "Queue" = "Geometry+1" "IsEmissive" = "true" }

			Cull back

			CGPROGRAM
			#pragma surface surf CelShaded fullforwardshadows
			#pragma shader_feature SHADOWED_RIM
			#pragma target 3.0

			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _Normal;

			sampler2D _SpecularMap;
			half _Glossiness;
			fixed4 _SpecularColor;

			fixed4 _Emission;

			fixed4 _OutlineColor;
			fixed _OutlineWidth;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_Normal;
				float2 uv_SpecularMap;
			};

			struct SurfaceOutputCelShaded
			{
				fixed3 Albedo;
				fixed3 Normal;
				float Smoothness;
				half3 Emission;
				fixed Alpha;
			};

			half4 LightingCelShaded(SurfaceOutputCelShaded s, half3 lightDir, half3 viewDir, half atten) {

				float3 refl = reflect(normalize(lightDir), s.Normal);
				float vDotRefl = dot(viewDir, -refl);
				float3 specular = _SpecularColor.rgb * step(1 - s.Smoothness, vDotRefl);

				half shadow = round(atten);

				half3 col = (s.Albedo + specular) * _LightColor0;

				half4 c;
				c.rgb = col * shadow;
				c.a = s.Alpha;
				return c;
			}

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputCelShaded o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));
				o.Smoothness = tex2D(_SpecularMap, IN.uv_SpecularMap).x * _Glossiness;
				o.Emission = o.Albedo * _Emission;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
