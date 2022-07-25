// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Complete360Tour/MenuButton"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_TintA("TintA", Color) = (1,1,1,1)
		_TintB("TintB", Color) = (1,1,1,1)
		
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
            Name "Default"
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                
                #include "UnityCG.cginc"
                #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
                #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
    
            struct appdata_t
        {
            float4 vertex   : POSITION;
            float4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };
    
        struct v2f
        {
            float4 vertex   : SV_POSITION;
            fixed4 color : COLOR;
            float2 texcoord  : TEXCOORD0;
            float4 worldPosition : TEXCOORD1;
            UNITY_VERTEX_OUTPUT_STEREO
        };
    
        sampler2D _MainTex;
        fixed4 _TintA;
        fixed4 _TintB;
        fixed4 _TextureSampleAdd;
        float4 _ClipRect;
        float4 _MainTex_ST;
    
        v2f vert(appdata_t v)
        {
            v2f OUT;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
            OUT.worldPosition = v.vertex;
            OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
    
            OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    
            OUT.color = v.color;
            return OUT;
        }
    
        fixed4 frag(v2f IN) : SV_Target
        {
            half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd);
    
            float mask = smoothstep(IN.color.r-0.01, IN.color.r, 1 - IN.texcoord.x);
            color.rgb *= lerp(_TintA, _TintB, mask);
            color.a *= IN.color.a;
            
    
    #ifdef UNITY_UI_ALPHACLIP
            clip(color.a - 0.001);
    #endif
    
            return color;
        }
            ENDCG
        }
	}
}
