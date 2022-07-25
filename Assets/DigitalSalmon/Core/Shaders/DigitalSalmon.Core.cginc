#ifndef DIGITALSALMON_CORE_INCLUDED
#define DIGITALSALMON_CORE_INCLUDED

struct appdata_min
{
	float4 uv		: TEXCOORD0;
	float4 vertex	: POSITION;
};

struct appdata_particle
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float4 color : COLOR;
};

struct v2f_standard {
	float2 uv : TEXCOORD0;
	UNITY_FOG_COORDS(1)
		float4 vertex : SV_POSITION;
};

struct v2f_min
{
	float2 uv		: TEXCOORD0;
	float4 vertex	: SV_POSITION;
};

struct v2f_particle
{
	float2 uv : TEXCOORD0;
	float4 vertex : SV_POSITION;
	float4 color : COLOR;
};

v2f_min vert_min(appdata_min v)
{
	v2f_min o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = v.uv;
	return o;
}

v2f_standard vert_standard(appdata_full  v) {
	v2f_standard o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	UNITY_TRANSFER_FOG(o, o.vertex);
	o.uv = v.texcoord;
	return o;
}

v2f_particle vert_particle(appdata_particle v)
{
	v2f_particle o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = v.uv;
	o.color = v.color;
	return o;
}


#endif // DIGITALSALMON_CORE_INCLUDED
