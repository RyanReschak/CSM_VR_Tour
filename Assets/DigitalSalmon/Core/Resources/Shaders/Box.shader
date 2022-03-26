Shader "Hidden/DigitalSalmon/UI/Box" 
{
	Properties
	{
		_BaseColor("[Base] Color", Color) = (0,0,0,0)
		_OutlineColor("[Outline] Color", Color) = (0,0,0,0)
		_OutlineInfo("[Outline] Info", Vector) = (0,0,0,0) // R: Canvas Width/Size, G: Outline Width, A: Smoothing
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

			uniform float4	_BaseColor;
			uniform float4	_OutlineColor;
			uniform float4	_OutlineInfo;
			
			fixed4 frag (v2f_min i) : SV_Target
			{	
				float canvasWidth = _OutlineInfo.r;
				float outlineThickness = _OutlineInfo.g;
				float softness = _OutlineInfo.a * canvasWidth ;
				
				float2 fieldUv = uvrQuad(i.uv);
				float field = sdBox(fieldUv,0.5 - (outlineThickness / canvasWidth));

				float mask = sampleSmooth(field, softness);

				return interpColor(_OutlineColor, _BaseColor, mask);;
			}
			ENDCG
		}
	}
}
