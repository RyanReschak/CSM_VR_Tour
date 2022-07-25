Shader "Hidden/Digital Salmon/C360/Loading Icon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Alpha ("Alpha", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

        Pass
        {
		Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
         
            #include "UnityCG.cginc"
			#include "../../../../../DigitalSalmon/Core/Shaders/DigitalSalmon.Fields.cginc" 

            struct appdata 
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;              
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			uniform float _Zoom;
			uniform float _C360Time;
			uniform float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);             
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 qUv = uvrQuad(i.uv);
				float radial = udRadial(qUv);
				radial = frac(radial - (_C360Time/2)) ;
				float smoothing = 300.0 / 120;
				smoothing /= _Zoom; 
				float ring = sampleSmooth(sdRing(qUv, 0.392, 0.017), smoothing);
				
				return float4(1, 1, 1, radial * ring * _Alpha);
            }
            ENDCG
        }
    }
}
