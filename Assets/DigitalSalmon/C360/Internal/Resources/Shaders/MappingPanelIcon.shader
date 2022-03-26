Shader "Hidden/Digital Salmon/C360/Mapping Panel Icon" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
		_Clip("Clip", Color) = (0,0,0,0)
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
			uniform float4 _Clip;


            float2 TinyPlanetUV( float2 uv , float m , float p ){
            float2 oUV = uv-float2(0.5,0.5);
            float y = saturate((distance(oUV,0.0)*2.0));
            return float2(((atan2(oUV.x,oUV.y)/6.28318530718)+0.5),pow(y,p)*m);
            }
            
            float RadialField( float2 uv ){
			 return 1-(distance(float2(0.5,0.5),uv) * 2);
            }

			float DropShadowMask(float x)
			{
				float n = 0.3;
				return smoothstep(0,n,x);
			}

			float RingMask(float x)
			{
				float n = 0.15;
				float t = 0.08;
				float smoothing = 0.08;
				return min(smoothstep(n, n + smoothing, x), smoothstep(n + t + smoothing, n + t,x));
			}

			float PaddedRingMask(float x) {
				float n = 0.12;
				float t = 0.15;
				float smoothing = 0.08;
				return min(smoothstep(n, n + smoothing, x), smoothstep(n + t + smoothing, n + t, x));
			}
            
			float2 ClipUV(float2 uv)
			{
				//x - Left
				//y - Bottom
				//z - Right
				//w - Top
				float4 clip = _Clip;
				float width = 1 - (clip.x + clip.z);
				float height = 1 - (clip.y + clip.w);
				float2 rUV = uv;
				rUV *= float2(width, height);
				rUV += float2(clip.x, clip.y);
				return rUV;
			}

			float2 ScaleUV(float2 uv, float scale)
			{
				uv -= float2(0.5, 0.5);
				uv *= scale;
				uv += float2(0.5, 0.5);
				return uv;
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
                o.pos = UnityObjectToClipPos(v.vertex); //mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }

            float4 frag(VertexOutput i) : COLOR { 
				float2 uv = ClipUV(i.uv0);
				float radialField = RadialField(uv);
				
				float iconMask = smoothstep(0.3, 0.4, radialField);
				float4 col =  tex2D(_MainTex, clamp(ScaleUV(uv, 1.2), 0, 1)); 
				col.a = min(iconMask, col.a);
				col.rgb = lerp(float3(1,1,1), col.rgb, iconMask);

				col.rgb = lerp(col.rgb, lerp(float3(0.2, 0.6, 0.9), float3(1,1,1), _Highlight.a), PaddedRingMask(radialField));
			
				col.a = min(col.a, 1- RingMask(radialField) );
			
				col.a = max(col.a, RingMask(radialField));	

				return col;
            }
            ENDCG
        }
    }
}
