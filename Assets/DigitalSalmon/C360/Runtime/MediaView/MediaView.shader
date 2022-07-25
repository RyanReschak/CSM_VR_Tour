Shader "Digital Salmon/C360/Media View" {
    Properties {
		_MainTex("MainTex", 2D) = "black" {}
        _Yaw ("Yaw", Float ) = 0

		[MaterialToggle] _YFlip("YFlip", Float) = 0
        [MaterialToggle] _Stereoscopic ("Stereoscopic", Float ) = 0
		[Enum(Equirectangular,0,VR180,1)] _Projection("Projection", int) = 0

		_ProjectionMaskSmoothing ("Projection Mask Smoothing", float) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        Pass {
            Name "FORWARD"
			Cull Front
			ZWrite Off
			
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct VertexInput {
                float4 vertex : POSITION;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };

			uniform sampler2D _MainTex; uniform float4 _MainTex_ST;

			uniform float _Yaw;
			uniform fixed _Stereoscopic;
			uniform int _Projection;
			uniform float _ProjectionMaskSmoothing;
			uniform fixed _YFlip;

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				o.posWorld = v.vertex;// mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex); 
                return o;
            }

			float2 projectionUv(float3 dir)
			{
				
				float pitch = (acos(-dir.g) / 3.141592654);
				float yaw = frac(((atan2(dir.r, dir.b) / 6.28318530718)) + _Yaw);

				if (_Projection == 0 && _Stereoscopic) {
					pitch /= 2;
					pitch += lerp(0.0, 0.5, unity_StereoEyeIndex);
					yaw = frac(yaw);
				}

				if (_Projection == 1) {
					yaw *= 2;
					
					if (_Stereoscopic) {
						yaw /= 2;
						int eyeIndex = unity_StereoEyeIndex;
						//eyeIndex = frac(_Time.r * 10) > 0.5 ? 1 : 0;
						yaw += lerp(0.5, 0.0, eyeIndex);
						yaw += 0.25;
					}
					else {
						yaw += 0.5;
					}
					
				}

				pitch = _YFlip == 1 ? (1 - pitch) : pitch;

				return float2(yaw, pitch);				
			}

			float2 rotate(float2 domain, float angleDegrees) {
				const float DEG2RAD = 0.01745329252;
				float s = sin(angleDegrees * DEG2RAD);
				float c = cos(angleDegrees * DEG2RAD);
				float tx = domain.x;
				float ty = domain.y;
				domain.x = (c * tx) - (s * ty);
				domain.y = (s * tx) + (c * ty);
				return domain;
			}

			float projectionMask(float3 dir) {
				if (_Projection == 1) {
					float2 dotForward = rotate(float2(0,-1), _Yaw * 360);
					float field = dot(dir, float3(dotForward.x, 0, dotForward.y)); 				
					return pow(smoothstep(0, max(0.001,_ProjectionMaskSmoothing), field),2);
				}
				return 1;
			}

            float4 frag(VertexOutput i) : COLOR {
				float3 dir = normalize(i.posWorld.rgb);
				float2 uv = projectionUv(dir);
				float mask = projectionMask(dir);
				
				return tex2D(_MainTex, frac(uv)) * mask;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
