#ifndef DIGITALSALMON_MATH_INCLUDED
#define DIGITALSALMON_MATH_INCLUDED

//-----------------------------------------------------------------------------------------
// Constants:
//-----------------------------------------------------------------------------------------

static const float PI = 3.14159265359;
static const float TAU = 6.28318530718;
static const float DEG2RAD = 0.01745329252;
static const float RAD2DEG = 180 / PI;
static const float MAX_FLOAT = 3.402823466e+38F;
static const float ROOT_3 = 1.732050807568877; // The aspect ratio of a regular Hexagon
static const float HALF_ROOT_3 = 0.8660254037844386; // Half the aspect ratio of a regular Hexagon

//-----------------------------------------------------------------------------------------
// Functions:
//-----------------------------------------------------------------------------------------

// Step linearly from 'low' to 'high' in domain 'x'.
float linstep(float low, float high, float x) {

	if (x < low) return 0;
	if (x > high) return 1;
	return abs((x - low) / (low - high));
}

float3 mod(float3 a, float3 b) {
	return (a - b) * floor(a / b);
}

float2 mod(float2 a, float2 b) {
	return (a - b) * floor(a / b);
}

float2 rotate(float2 domain, float angleDegrees) {
	float s = sin(angleDegrees * DEG2RAD);
	float c = cos(angleDegrees * DEG2RAD);
	float tx = domain.x;
	float ty = domain.y;
	domain.x = (c * tx) - (s * ty);
	domain.y = (s * tx) + (c * ty);
	return domain;
}

float4 interpColor(float4 a, float4 b, float t) {
	float4 col = lerp(a, b, t);
	if (a.a == 0) {
		col.rgb = b.rgb;
	}
	if (b.a == 0) {
		col.rgb = a.rgb;
	}
	return col;
}

//-----------------------------------------------------------------------------------------
// Hashing:
//-----------------------------------------------------------------------------------------

float2 hash2_2D(float2 p)
{
	p = float2(dot(p, float2(127.1, 311.7)),
		dot(p, float2(269.5, 183.3)));

	return -1.0 + 2.0*frac(sin(p)*43758.5453123);
}

float3 hash3_2D(float2 p)
{
	float3 q = float3(dot(p, float2(127.1, 311.7)),
		dot(p, float2(269.5, 183.3)),
		dot(p, float2(419.2, 371.9)));
	return frac(sin(q)*43758.5453);
}

float4 hash4_2D(float2 p) {
	return frac(sin(float4(1.0 + dot(p, float2(37.0, 17.0)),
		2.0 + dot(p, float2(11.0, 47.0)),
		3.0 + dot(p, float2(41.0, 29.0)),
		4.0 + dot(p, float2(23.0, 31.0))))*103.0);
}

//-----------------------------------------------------------------------------------------
// Noise:
//-----------------------------------------------------------------------------------------

float clouds(float2 input, float iterations) {
	if (iterations == 0)
		return 0;

	float2 ret = float2(0, 0);

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

float voronoise(float2 uv, float x, float y) {
	uv -= float2(0.5, 0.5);
	float2 p = floor(uv);
	float2 f = frac(uv);

	float k = 1.0 + 63.0*pow(1.0 - y, 4.0);

	float va = 0.0;
	float wt = 0.0;
	for (int j = -2; j <= 2; j++)
		for (int i = -2; i <= 2; i++)
		{
			float2 g = float2(float(i), float(j));
			float3 o = hash3_2D(p + g)*float3(x, x, 1.0);
			float2 r = g - f + o.xy;
			float d = dot(r, r);
			float ww = pow(1.0 - smoothstep(0.0, 1.414, sqrt(d)), k);
			va += o.z*ww;
			wt += ww;
		}
	return va / wt;
}

float voronoise(float p, float x, float y) {
	return voronoise(float2(p, 0), x, y);
}

#endif // DIGITALSALMON_MATH_INCLUDED