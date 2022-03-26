Shader "Digital Salmon/C360/Base Plate"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Radius ("Radius", Range(0,1)) = 0.75
		_Softness ("Softness", Range(0.25,5)) = 1
		
		// required for UI.Mask
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}            
        
       // required for UI.Mask
		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]
         
         Pass
         {                       
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _Radius;
			uniform float _Softness;
			
			
            static const float DEG2RAD = 0.01745329252;
            
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPosition = mul (unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			// 2D Circle
            float sdSphere(float2 domain, float radius) {
                return length(domain) - radius;
            }
            
            // Custom smoothing sampling distributed amongst the sign threshold.
            float sampleSmoothSigned(float field, float smoothing) {
                smoothing /= 100;
                return smoothstep(smoothing/2, -smoothing/2, field);
            }
            
            // Remap 0->1 uv space to -0.5 -> 0.5 uv space.
            float2 uvrQuad(float2 uv) {
                return uv - float2(0.5, 0.5);
            }
            
            // 2D Box - X|Y
            float sdBox(float2 domain, float2 size) {
                float2 d = abs(domain) - size;
                return min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
            }

            
            // rotate 'domain' by 'degrees'.
            float2 opRotate(float2 domain, float degrees) {
                float s = sin(degrees * DEG2RAD);
                float c = cos(degrees * DEG2RAD);
            
                float tx = domain.x;
                float ty = domain.y;
                domain.x = (c * tx) - (s * ty);
                domain.y = (s * tx) + (c * ty);
                return domain;
            }
            

			fixed4 frag (v2f i) : SV_Target
			{
			    fixed4 col = fixed4(0,0,0,1);
				
				//Make radius slider sensible
				_Radius = (_Radius);
				
				float2 radialUV = uvrQuad(i.uv);
				
				float mask = sdSphere(radialUV, _Radius);
				mask = sampleSmoothSigned(mask, _Softness);
				col.a = mask;
				
				#ifdef UNITY_UI_ALPHACLIP
					clip(col.a - 0.001);
				#endif
				
				return col;
			}

			ENDCG
		}
	}
}
