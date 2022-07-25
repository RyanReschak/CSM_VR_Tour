Shader "Hidden/DigitalSalmon/UI/UV0" 
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"
			#include "../../Shaders/DigitalSalmon.Core.cginc"
			#include "../../Shaders/DigitalSalmon.Fields.cginc"

			#pragma vertex vert_min
			#pragma fragment frag		

			uniform sampler2D _MainTex;	float4 _MainTex_ST;
			
			fixed4 frag (v2f_img i) : SV_Target
			{
				return fixed4(i.uv.x, i.uv.y, 0, 1);
			}
			ENDCG
		}
	}
}
