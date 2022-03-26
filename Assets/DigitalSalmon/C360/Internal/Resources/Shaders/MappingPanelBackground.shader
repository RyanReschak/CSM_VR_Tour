Shader "Hidden/Digital Salmon/C360/Mapping Panel Background" {
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[MaterialToggle] _Stereoscopic("Stereoscopic", float) = 0
		[Enum(Equirectangular,0,VR180,1)] _Projection ("Projection", int) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform float _Stereoscopic;
			uniform int _Projection;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
						
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv;
				if (_Projection == 0 && _Stereoscopic) {
					uv.y *= 0.5;
				}				
				if (_Projection == 1 && _Stereoscopic) {
					uv.x /= 2;
				}
				fixed4 col = tex2D(_MainTex, uv);

				return col;
			}
			ENDCG
		}
	}
}
