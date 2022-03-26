Shader "Hidden/DigitalSalmon/ChannelSelect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[Enum(R,0,G,1,B,2,A,3)] _Channel("[RGBA] Channel", Float) = 0
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
			uniform int _Channel;		
			
			fixed4 frag (v2f_min i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				if (_Channel == 3){
					return float4(col.a,col.a,col.a,1);
				}

				return float4(col.r * (_Channel == 0), col.g * (_Channel == 1), col.b * (_Channel == 2), 1); 
			}
			ENDCG
		}
	}
}
