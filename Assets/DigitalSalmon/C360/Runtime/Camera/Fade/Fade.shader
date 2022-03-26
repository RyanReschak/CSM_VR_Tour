Shader "DigitalSalmon.Fade/Fade" {
	Properties 	{
		[HideInInspector] _MainTex ("Screen Texture", 2D) = "white" {}
		[HideInInspector] _Alpha("[Fade] Alpha", Float) = 0.5
		[HideInInspector] _Delta("[Fade] Delta", Float) = 0.5	
		_BaseColor("[Base] Color", Color) = (0,0,0,1)
	}
	SubShader {
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			CGPROGRAM
			#pragma vertex ds_vert_img
			#pragma fragment effect

			#include "UnityCG.cginc"


			struct ds_appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct ds_v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			ds_v2f ds_vert_img(ds_appdata v) {
				ds_v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			uniform sampler2D _MainTex;
			uniform float _Delta;
			uniform float4 _BaseColor;

			fixed4 effect(ds_v2f i) : SV_Target
			{							
				fixed4 col = tex2D(_MainTex, i.uv);
				return lerp(col, _BaseColor, _Delta);
			}
			ENDCG
		}
	}
}
