Shader "Digital Salmon/C360/Hotspot"
{
	Properties
	{
		[PerRendererData] _MainTex("MainTex", 2D) = "white" {}
		_IconColor("[Icon] Color", Color) = (1,1,1,1)
		
		_BaseColor("[Base] Color", Color) = (0,0,0,1)
		_FillColor("[Fill] Color", Color) = (1,1,1,1)

		_Smoothing("[Global] Smoothing", Float) = 1
		_Alpha("[Global] Alpha", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "PreviewType"="Plane" }
		Cull Off
		ZWrite Off

		Pass
		{ 
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "../../../Core/Shaders/DigitalSalmon.Fields.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _IconColor;
		
			uniform float4 _BaseColor;
			uniform float4 _FillColor;
	

			uniform float _Smoothing;
			uniform float _Alpha;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 data : TEXCOORD1; 
				float4 data2 : TEXCOORD2;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float distance : TEXCOORD1;
				float4 data : TEXCOORD2;
				float4 data2 : TEXCOORD3;
				float4 color : TEXCOORD4;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.distance = length(mul(unity_ObjectToWorld, v.vertex) - _WorldSpaceCameraPos);
				o.data = v.data;
				o.data2 = v.data2;
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{		
				_Smoothing *= pow(i.distance,0.8);
				
				float fill = i.data.x;
				float iconAlpha = i.data.y;
				float iconScale = i.data.z;
				float innerRadius = i.data2.x;
				float outerRadius = i.data2.y;
				

				float2 quadUv = uvrQuad(i.uv);

				float ringInner = sdSphere(quadUv, innerRadius);
				float ringOuter = sdSphere(quadUv, outerRadius);
				
				float innerRingMask = sampleSmooth(ringInner, _Smoothing);
				float iconMask = sampleSmooth(ringInner+0.04, _Smoothing);
				float ringMask = sampleSmooth(opSubtraction(ringOuter, ringInner), _Smoothing);
				
				float fillField = (udRadial(quadUv) / (1 + _Smoothing/100)) - fill;
				float fillMask = sampleSmooth(fillField, _Smoothing);

				float4 ringColor = lerp(_BaseColor, _FillColor, fillMask);
				ringColor.a = min(ringColor.a, ringMask);

		    	float4 iconColor = tex2D(_MainTex, (((i.uv-0.5)*1.3)+0.5));
				//return float4(iconUv.x,iconUv.y,0,1);
				iconColor *= _IconColor;
				iconColor.a *= iconAlpha;
				iconColor.a = min(iconColor.a, iconMask);

				float3 finalColor = lerp(ringColor.rgb, iconColor.rgb, iconColor.a);
				if (ringColor.a == 0) finalColor = iconColor.rgb;

				float finalAlpha = max(iconColor.a, ringColor.a);
				finalAlpha *= _Alpha;
				
				finalAlpha *= i.color.a;

				return float4(finalColor, finalAlpha); 
			}
			ENDCG
		}
	}
}
