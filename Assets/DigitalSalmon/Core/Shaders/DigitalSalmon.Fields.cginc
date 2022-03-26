#ifndef DIGITALSALMON_FIELDS_INCLUDED
#define DIGITALSALMON_FIELDS_INCLUDED

#include "DigitalSalmon.Math.cginc"

//-----------------------------------------------------------------------------------------
// Remapping:
//-----------------------------------------------------------------------------------------

// Remap 0->1 uv space to -0.5 -> 0.5 uv space.
float2 uvrQuad(float2 uv) {
	return uv - float2(0.5, 0.5);
}

// Offset uv space for optically centered equilateral triangle.
float2 uvrTriOpticalCenter(float2 uv) {
	float opticalOffset = 0.125; //(0.5 - (sin(30 * (pi / 180)) * 0.5)) /2
	return uv + float2(0, opticalOffset);
}

float2 uvrScaleQuad(float2 domain, float scale)
{
    domain -= 0.5;
    domain /= scale;
    domain += 0.5;
    return domain;
}

//-----------------------------------------------------------------------------------------
// Sampling:
//-----------------------------------------------------------------------------------------

// Cheap hard sampling.
float sampleHard(float field) {
	return field < 0;
}

// Simple sharp sampling.
float sampleSharp(float field) {
	return smoothstep(0, -0.001, field);
}

// Simple soft sampling.
float sampleSoft(float field) {
	return smoothstep(0,-0.005,field);
}

// Custom smoothing sampling.
float sampleSmooth(float field, float smoothing) {
	smoothing /= 100;
	return smoothstep(0, -smoothing, field);
}

// Custom smoothing sampling distributed amongst the sign threshold.
float sampleSmoothSigned(float field, float smoothing) {
	smoothing /= 100;
	return smoothstep(smoothing/2, -smoothing/2, field);
}

//-----------------------------------------------------------------------------------------
// Operations:
//-----------------------------------------------------------------------------------------

// 'a' - 'b'.
float opSubtraction(float a, float b) {
	return max(-b, a);
}

// 'a' && 'b'
float2 opUnion(float a, float b) {
	return min(a,b);
}

// 'a' > 'b' ? 'a' : 'b'
float opIntersect(float a, float b) {
	return max(a, b);
}

// repeat 'domain' by 'multiplier'.
float2 opRepeat(float2 domain, float2 multiplier) {
	domain += 0.5;
	return frac(domain*multiplier) - 0.5;
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

// 'a' && 'b' smooth. Exponential.
float opSminExp(float a, float b, float k) {
	float res = exp(-k*a) + exp(-k*b);
	return -log(res) / k;
}

// 'a' && 'b' smooth. Polynomial.
float opSminPoly(float a, float b, float k) {
	float h = clamp(0.5 + 0.5*(b - a) / k, 0.0, 1.0);
	return lerp(b, a, h) - k*h*(1.0 - h);
}

// 'a' && 'b' smooth. Power.
float opSminPow(float a, float b, float k) {
	a = pow(a, k);
	b = pow(b, k);
	return pow((a*b) / (a + b), 1.0 / k);
}

//-----------------------------------------------------------------------------------------
// Primitives:
//-----------------------------------------------------------------------------------------

// 2D Radial
float udRadial(float2 domain) {
    return (atan2(-domain.x, -domain.y) / TAU) + 0.5;
}


// 1D Radius
float sdSphere(float domain, float radius) {
	return length(domain) - radius;
}

// 2D Circle
float sdSphere(float2 domain, float radius) {
	return length(domain) - radius;
}

// 3D Sphere
float sdSphere(float3 domain, float radius) {
	return length(domain) - radius;
}

// 1D Ring
float sdRing(float domain, float radius, float thickness) {
	return opSubtraction(sdSphere(domain, radius + thickness / 2), sdSphere(domain, radius - thickness / 2));
}

// 2D Ring
float sdRing(float2 domain, float radius, float thickness) {
	return opSubtraction(sdSphere(domain, radius + thickness / 2), sdSphere(domain, radius - thickness / 2));
}

// 3D Ring
float sdRing(float3 domain, float radius, float thickness) {
	return opSubtraction(sdSphere(domain, radius + thickness / 2), sdSphere(domain, radius - thickness / 2));
}

// 2D Box - X|Y
float sdBox(float2 domain, float2 size) {
	float2 d = abs(domain) - size;
	return min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
}

// 3D Box - X|Y
float sdBox(float3 domain, float3 size) {
	float3 d = abs(domain) - size;
	return min(max(d.x, max(d.y, d.z)), 0.0) + length(max(d, 0.0));
}

// 2D Box - X|X
float sdBox(float2 domain, float size) {
	float2 d = abs(domain) - size;
	return min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
}

// 3D Box - X|X
float sdBox(float3 domain, float size) {
	float3 d = abs(domain) - size;
	return min(max(d.x, max(d.y, d.z)), 0.0) + length(max(d, 0.0));
}

// 2D Box Rounded Unsigned
float udRoundBox(float2 domain, float2 size, float radius) {
	size -= radius;
	return length(max(abs(domain) - size, 0.0)) - radius;
}

// 3D Box Rounded Unsigned
float udRoundBox(float3 domain, float3 size, float radius) {
	size -= radius;
	return length(max(abs(domain) - size, 0.0)) - radius;
}

// 2D Box Rounded Signed
float sdRoundBox(float2 domain, float2 size, float r) {
	float2 q = abs(domain) - size;
	float2 m = float2(min(q.x, q.y), max(q.x, q.y));
	float d = (m.x > 0.0) ? length(q) : m.y;
	return d - r;
}

// 2D Ellipse
float sdEllipse(float2 domain, float2 size) {
	return (length(domain / size) - 1.0) * min(size.x, size.y);
}

// 3D Ellipsoid
float sdEllipsoid(float3 domain, float3 size) {
	return (length(domain / size) - 1.0) * min(min(size.x, size.y), size.z);
}

// 2D Hexagon
float sdHex(float2 domain, float radius) {
	float2 q = abs(domain);
	return max((q.x*HALF_ROOT_3 + q.y*0.5), q.y) - radius + ((1 - HALF_ROOT_3) / 2);
}

// 3D Hexagonal Prism
float sdHexPrism(float3 domain, float radius, float height) {
	float3 q = abs(domain);
	return max(q.z - height, max((q.x*HALF_ROOT_3 + q.y*0.5), q.y) - radius);
}

// 2D Triangle
float sdTri(float2 domain, float radius) {
	float2 q = abs(domain);
	return max(q.x*HALF_ROOT_3 + domain.y*0.5, -domain.y) - radius*0.5;
}

// 3D TriangularPrism
float sdTriPrism(float3 domain, float radius, float height) {
	float3 q = abs(domain);
	return max(q.z - height, max(q.x*HALF_ROOT_3 + domain.y*0.5, -domain.y) - radius*0.5);
}

// 2D NGon (Triangle, Square, Pentagon...)
float sdNgon(float2 domain, int sides, float radius) {
	float hyp = radius * cos(PI / sides) ;

	float a = atan2(domain.x, domain.y) + PI;
	float r = TAU / float(sides);
	float d = (cos(floor(0.5 + a / r) * r - a) * length(domain)) - hyp;

	return d;
}

// 2D NGon (Rounded) internal helper function
float2 sdNgonRoundedCorners(float2 domain, float sides, float radius, float cornerRadius) {

	float theta = ((sides - 2) * 180) / ((sides) * 2);
	float a = atan2(domain.x, domain.y) + (TAU / 2);
	float p = (cornerRadius / sin(theta *  DEG2RAD));

	float2 cornerSpheres = float2(MAX_FLOAT,0);

	for (int i = 0; i < sides; i++) {

		float2 rDomain = rotate(domain, 180+ (360 * ((float) (i + 0.5)/ sides))) - float2(0, radius - p);
		
		// Field
		float a = ((float)i) / ((float)sides);
		float cornerSphere = sdSphere(rDomain, cornerRadius); 
		cornerSpheres.x = opUnion(cornerSpheres.x, cornerSphere); 

		// Mask
		float maskAngle = 90 - theta;

		float r = maskAngle / 360;

		float radialField = (PI + atan2(rDomain.x, -rDomain.y)) / TAU;

		cornerSpheres.y = max(cornerSpheres.y, radialField < r || radialField > 1 - r);
	}

	return cornerSpheres;
}

// 2D NGon (Rounded)
float2 sdNgonRounded(float2 domain, float sides, float radius, float cornerRadius) {
	
	cornerRadius = min(radius / 2, cornerRadius);
	float a = atan2(domain.x, domain.y) + (TAU / 2);
	float r = TAU / sides;
	 
	float angle = 0.5 + a / r;
	float hyp = radius * cos(PI / sides);

	float segmentId = floor(angle);
	float nGon = (cos(segmentId * r - a) * length(domain)) - hyp;

	float2 corners = sdNgonRoundedCorners(domain, sides, radius, cornerRadius);

	return corners.y ? corners.x : nGon;
}

#endif // DIGITALSALMON_FIELDS_INCLUDED
