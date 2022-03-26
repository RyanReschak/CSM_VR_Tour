Shader "Hidden/Digital Salmon/C360/Gesture Button" {
    Properties {
		_Shadow("Shadow", float) = 0
		_Highlight("Highlight", Color) = (0,0,0,1)
    }
    SubShader {
        Tags {

        }
        Pass {
            Name "FORWARD"
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
			

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
			uniform float4 _Highlight;
			uniform float _Shadow;
			uniform float4 _Clip;
			uniform float _Zoom;

            float2 TinyPlanetUV( float2 uv , float m , float p ){
            float2 oUV = uv-float2(0.5,0.5);
            float y = saturate((distance(oUV,0.0)*2.0));
            return float2(((atan2(oUV.x,oUV.y)/6.28318530718)+0.5),pow(y,p)*m);
            }
            
            float RadialField( float2 uv ){
            return 1-(distance(float2(0.5,0.5),uv) * 2);
            }
            
            float TinyPlanetMask( float field ){
			float smoothing = 0.03;
			smoothing /= _Zoom;
            return smoothstep(0.21,0.21+smoothing,field);
            }
            
            float ShadowMask( float field ){
            return smoothstep(0,0.2,field);
            }
            
            float RingMask( float field, float smoothing)
			{
				smoothing /= 100;
				smoothing /= _Zoom;
				return min(smoothstep(0.2-smoothing,0.2,field),smoothstep(0.23 + smoothing,0.23,field));
            }
		
            
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
				o.pos = UnityObjectToClipPos(v.vertex);//  mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
				float2 uv = i.uv0;
                float radialField = RadialField( uv );
               
                float ringMask = RingMask(radialField, 8);
				float shadowMask = ShadowMask(radialField);
                
				float3 col = lerp(0, _Highlight, ringMask);

				return float4(col, max(shadowMask,ringMask));
            }
            ENDCG
        }
    }
}
