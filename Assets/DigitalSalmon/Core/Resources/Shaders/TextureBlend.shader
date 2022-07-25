Shader "Hidden/DigitalSalmon/UI/TextureBlend"
{
	Properties
	{
		_MainTex ("[Texture] A", 2D) = "white" {}
		_TextureB("[Texture] B", 2D) = "white" {}
		_Area ("[Rect] Area", Vector) = (0,0,0,0)
		_Alpha ("[Blend] Alpha", Float) = 1	
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			
			#include "UnityCG.cginc"
			#include "../../Shaders/DigitalSalmon.Core.cginc"
			#include "../../Shaders/DigitalSalmon.Fields.cginc"

			#pragma vertex vert_min
			#pragma fragment frag				

			uniform sampler2D _MainTex;	fixed4 _MainTex_ST;
			uniform sampler2D _TextureB;
			uniform float4 _Area;
			uniform float _Alpha;

			float2 ApplyAreaMap(float2 uv, float4 area){

				float width = area.z;
				float height = area.w;
				float2 rUV = uv;
				rUV -= float2(area.x, area.y);
				rUV /= float2(width, height);
				return rUV;
			}
			
			fixed4 frag (v2f_min i) : SV_Target
			{
				fixed4 textureA = tex2D(_MainTex, i.uv);
				fixed4 textureB = tex2D(_TextureB, ApplyAreaMap(i.uv, _Area));
				textureB.a *= _Alpha; 
				textureA.rgb = lerp(textureA.rgb, textureB.rgb, textureB.a);
				return textureA;
			}
			ENDCG
		}
	}
}
