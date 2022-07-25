Shader "Hidden/DigitalSalmon/GUILine"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_ColorB("ColorB", Color) = (1,1,1,1)
		_Angle("Angle", Float) = 0
		_Length("Length", Float) = 1
		_Width("Width", Float) = 1
		_Rect("Rect", Vector) = (1,1,0,0)
	}
	SubShader
	{
		Tags {  }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _Color;
			float4 _ColorB;
			float _Angle;
			float2 _Rect;
			float _Width;
			float _Length;
			float _Zoom;
			float4 _MainTex_ST;
			uniform float _ClipTop;

			float2 rotate(float2 domain, float angleRadians) {
				float s = sin(angleRadians);
				float c = cos(angleRadians);
				float tx = domain.x;
				float ty = domain.y;
				domain.x = (c * tx) - (s * ty);
				domain.y = (s * tx) + (c * ty);
				return domain;
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.screenPos = v.vertex;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{						
				float2 uv = i.uv;
				uv -= 0.5;	
				uv.x /= (_Rect.y/_Rect.x);	
				uv = rotate(uv, _Angle );
				uv.y *= _Rect.y/_Width * 2;
				uv += 0.5;

				float clipUv = uv;
				clipUv -= 0.5;
				clipUv.x /= _Length/ _Rect.y;
				clipUv += 0.5;
				float clipMask = !(clipUv.x > 1 | clipUv.x < 0);
				clipMask *= (i.vertex.y > _ClipTop);
				
				float colorDelta = clamp(clipUv.x, 0, 1);

				float2 domain = uv;
				
				float zoomVal = 0.2 / _Zoom;
				float mask = min(smoothstep(-zoomVal, zoomVal, domain.y), smoothstep(1 + zoomVal, 1- zoomVal, domain.y));
				//mask *= i.vertex.y > 49;
				
				float4 col = lerp(_Color, _ColorB, colorDelta);

				return float4(col.r, col.g, col.b, mask * col.a * clipMask);
			}
			ENDCG
		}
	}
}
