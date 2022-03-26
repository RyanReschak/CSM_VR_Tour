Shader "Hidden/DigitalSalmon/Core/Fade" {
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
			#pragma vertex vert_min
			#pragma fragment effect

			#include "UnityCG.cginc"
			#include "../../Shaders/DigitalSalmon.Core.cginc"

			uniform sampler2D _MainTex;
			uniform float _Alpha;
			uniform float _Delta;
			uniform float4 _BaseColor;

			fixed4 effect(v2f_min i) : SV_Target
			{							
				fixed4 col = tex2D(_MainTex, i.uv);
				return lerp(col, _BaseColor, _Delta);
			}
			ENDCG
		}
	}
}
