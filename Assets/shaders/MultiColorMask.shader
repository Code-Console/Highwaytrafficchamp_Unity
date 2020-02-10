Shader "Custom/MultiColorMask" {
	Properties {
		_Color1 ("ColorMask1", Color) = (1,1,1,1)
		_Color2 ("ColorMask2", Color) = (1,1,1,1)
		_Color3 ("ColorMask3", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MaskTex ("Mask (RGB)", 2D) = "white" {}
		_Normal ("Normal", 2D) = "bump"{}
		_Metallic ("Metallic/Roughness/AO (RGB)",2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex,_MaskTex,_Metallic,_Normal;


		struct Input {
			float2 uv_MainTex,uv_MaskTex,uv_Metallic,uv_Normal;
		};

		//half _Glossiness;
		//half _Metallic;
		fixed3 _Color1,_Color2,_Color3 ;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;
			float3 mask = tex2D (_MaskTex, IN.uv_MainTex) ;
			float cmask = min(1.0,mask.r+mask.g+mask.b);
			float3 mro = tex2D (_Metallic, IN.uv_Metallic) ;
			float4 n = tex2D (_Normal, IN.uv_Normal) ;

			c.rgb = c.rgb *(1-cmask)+(_Color1*mask.r)*+(_Color2*mask.g)+(_Color3*mask.b);
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(n);
			// Metallic and smoothness come from slider variables
			o.Metallic = mro.r;
			o.Smoothness = mro.g;
			o.Occlusion = mro.b;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
