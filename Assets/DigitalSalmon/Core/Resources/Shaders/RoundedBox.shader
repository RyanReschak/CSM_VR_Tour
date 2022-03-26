Shader "Hidden/DigitalSalmon/UI/RoundedBox" 
{
	Properties
	{
		_BaseColor("[Base] Color", Color) = (0,0,0,0)
		_OutlineColor("[Outline] Color", Color) = (0,0,0,0)
		_OutlineInfo("[Outline] Info", Vector) = (0,0,0,0) // R: Canvas Width/Size, G: Outline Width, B: Corner Radius, A: Smoothing	
	}
	SubShader
	{
		Pass
		{
			//Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			
			#include "UnityCG.cginc"
			#include "../../Shaders/DigitalSalmon.Core.cginc"
			#include "../../Shaders/DigitalSalmon.Fields.cginc"

			#pragma vertex vert_min
			#pragma fragment frag

			uniform float4 _BaseColor;
			uniform float4 _OutlineColor;
			uniform float4 _OutlineInfo;
			
			fixed4 frag (v2f_min i) : SV_Target
			{
				float canvasWidth = _OutlineInfo.r;
				float outlineThickness = _OutlineInfo.g / canvasWidth;
				float outerRadius = max(0, min(_OutlineInfo.b, canvasWidth/2) /canvasWidth);
				
				float smoothing = _OutlineInfo.a * canvasWidth / 10;
				
				float2 domain = uvrQuad(i.uv);

				float outerField = sdRoundBox(domain, 0.5 - outerRadius, outerRadius);
				float innerField = outerField + outlineThickness;

				float innerMask = sampleSmooth(innerField, smoothing);
				float outerMask = sampleSmooth(outerField, smoothing);

				float4 col = interpColor(_OutlineColor, _BaseColor, innerMask);
				col.a = min(col.a, outerMask);

				return col;
			}
			ENDCG
		}
	}
}
