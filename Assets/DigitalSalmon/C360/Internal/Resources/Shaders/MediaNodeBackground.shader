Shader "Hidden/Digital Salmon/C360/Media Node Background" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
		_Shadow("Shadow", float) = 0
		_Highlight("Highlight", Color) = (0,0,0,1)
		_Clip("Clip", Color) = (0,0,0,0)
		_InnerShadow("InnerShadow", float) = 0
		[MaterialToggle] _Stereoscopic("Stereoscopic", float) = 0
		[Enum(Equirectangular,0,VR180,1)] _Projection("Projection", int) = 0
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
			uniform float _InnerShadow;
			uniform float _Stereoscopic;
			uniform float _Projection;

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
				float smoothing = 0.03 / _Zoom;
				return smoothstep(0.21,0.21 + smoothing,field);
            }
            
            float ShadowMask( float field ){
				return smoothstep(0,0.2,field);
            }
            
            float RingMask( float field ){
				float smoothing = 0.03 / _Zoom;			
				return min(smoothstep(0.2-smoothing,0.2,field),smoothstep(0.23+smoothing,0.23,field));
            }

			float InnerShadowMask(float field) {
				return smoothstep(0.4, 0.2, field);
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
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

			float Simplex1D(float input, float iterations) {
				if (iterations == 0)
					return 0;

				float ret = 0;

				for (int i = 0; i < iterations; ++i)
				{
					float2 p = floor(input * (i + 1));
					float2 f = frac(input * (i + 1));
					f = f * f * (3.0 - 2.0 * f);
					float n = p.x + p.y * 57.0;
					float4 noise = float4(n, n + 1, n + 57.0, n + 58.0);
					noise = frac(sin(noise)*437.585453);
					ret += lerp(lerp(noise.x, noise.y, f.x), lerp(noise.z, noise.w, f.x), f.y) * (iterations / (i + 1));
				}
				return ret / (iterations);
			}

			float4 BlurredTexSample(sampler2D tex, float2 uv, float blurDistance, float sampleCount)
			{
				sampleCount = (int)sampleCount;
				blurDistance /= 100;
				blurDistance /= _Zoom;
				float4 v = 0;
				for (int i = 0; i < sampleCount; i++)
				{
					float n = ((float)i / sampleCount);
					float noise = Simplex1D(n, 2) - 0.5;
					float2 sampleUV = uv + (float2(cos(noise * 3.14 * 2), sin(noise * 3.14 * 2)) * n * blurDistance);
					v += tex2D(tex, sampleUV);
				}

				v /= sampleCount;

				return v;
			}

            float4 frag(VertexOutput i) : COLOR {
				float2 uv = ClipUV(i.uv0);	
				float4 media = float4(0, 0, 0, 0);

				if (_Projection == 0) {
					float2 tinyPlanetUV = TinyPlanetUV(uv, 1.2, 1.0);
					tinyPlanetUV.y *= _Stereoscopic ? 0.5 : 1;
					media = BlurredTexSample(_MainTex, tinyPlanetUV, 1, 6);
				}
				else if (_Projection == 1) {
					float2 vr180uv = uv;
					vr180uv.x *= _Stereoscopic ? 0.5 : 1;
					media = tex2D(_MainTex, vr180uv);
				}

                float radialField = RadialField( uv );
                float tinyPlanetMask = TinyPlanetMask( radialField );
                float ringMask = RingMask( radialField );
     
                float3 color = lerp(0, lerp(media.rgb,0, InnerShadowMask(radialField) * _InnerShadow), tinyPlanetMask);

                float shadowAlpha = ShadowMask(radialField) * _Shadow * 0.5;
				float3 col = lerp(color,_Highlight,ringMask);
				float alpha = max(shadowAlpha, max(tinyPlanetMask,ringMask));
				return float4(col.r,col.g,col.b,alpha);
            }
            ENDCG
        }
    }
}
