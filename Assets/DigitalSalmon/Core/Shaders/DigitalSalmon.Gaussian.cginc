#ifndef DIGITALSALMON_GAUSSIAN_INCLUDED
#define DIGITALSALMON_GAUSSIAN_INCLUDED

#include "DigitalSalmon.Math.cginc"

//-----------------------------------------------------------------------------------------
// Gaussian:
//-----------------------------------------------------------------------------------------

float4 gaussian3(sampler2D tex, float2 uv, float blurDistance) {

	float2 dir = float2(blurDistance, blurDistance);

	float KERNAL[3][3] = {
		{ 0.109634, 0.111842, 0.109634 },
	{ 0.111842, 0.114094, 0.111842 },
	{ 0.109634, 0.111842, 0.109634 }
	};


	float4 blurredOutput = float4(0, 0, 0, 1);

	for (int x = -1; x < 2; x++) {
		for (int y = -1; y < 2; y++) {
			blurredOutput += tex2D(tex, uv + float2(dir.x*x, dir.y*y))*KERNAL[x + 1][y + 1];
		}
	}

	return blurredOutput;
}

float4 gaussian5(sampler2D tex, float2 uv, float blurDistance) {

	float2 dir = float2(blurDistance, blurDistance);

	float KERNAL[5][5] = {
		{ 0.036894, 0.039167, 0.039956, 0.039167, 0.036894 },
	{ 0.039167, 0.041581, 0.042418, 0.041581, 0.039167 },
	{ 0.039956, 0.042418, 0.043272, 0.042418, 0.039956 },
	{ 0.039167, 0.041581, 0.042418, 0.041581, 0.0391679 },
	{ 0.036894, 0.039167, 0.039956, 0.039167, 0.036894 }
	};

	float4 blurredOutput = float4(0, 0, 0, 1);

	for (int x = -2; x < 3; x++) {
		for (int y = -2; y < 3; y++) {
			blurredOutput += tex2D(tex, uv + float2(dir.x*x, dir.y*y))*KERNAL[x + 2][y + 2];
		}
	}

	return blurredOutput;
}

float4 gaussian7(sampler2D tex, float2 uv, float blurDistance) {

	float2 dir = float2(blurDistance, blurDistance);

	//Sigma 5
	float KERNAL[7][7] = {
		{ 0.016641, 0.018385, 0.019518, 0.019911, 0.019518, 0.018385, 0.016641 },
	{ 0.018385, 0.020312, 0.021564, 0.021998, 0.021564, 0.020312, 0.018385 },
	{ 0.019518, 0.021564, 0.022893, 0.023354, 0.022893, 0.021564, 0.019518 },
	{ 0.019911, 0.021998, 0.023354, 0.023824, 0.023354, 0.021998, 0.019911 },
	{ 0.019518, 0.021564, 0.022893, 0.023354, 0.022893, 0.021564, 0.019518 },
	{ 0.018385, 0.020312, 0.021564, 0.021998, 0.021564, 0.020312, 0.018385 },
	{ 0.016641, 0.018385, 0.019518, 0.019911, 0.019518, 0.018385, 0.016641 }
	};


	float4 blurredOutput = float4(0, 0, 0, 1);

	for (int x = -3; x < 4; x++) {
		for (int y = -3; y < 4; y++) {
			blurredOutput += tex2D(tex, uv + float2(dir.x*x, dir.y*y))*KERNAL[x + 3][y + 3];
		}
	}

	return blurredOutput;
}

float4 gaussian9(sampler2D tex, float2 uv, float blurDistance) {

	float2 dir = float2(blurDistance, blurDistance);

	//Sigma 5
	float KERNAL[9][9] = {
		{ 0.008397, 0.009655, 0.010667, 0.011324, 0.011552, 0.011324, 0.010667, 0.009655, 0.008397 },
	{ 0.009655, 0.0111, 0.012264, 0.013019, 0.013282, 0.013019, 0.012264, 0.0111, 0.009655 },
	{ 0.010667, 0.012264, 0.013549, 0.014384, 0.014674, 0.014384, 0.013549, 0.012264, 0.010667 },
	{ 0.011324, 0.013019, 0.014384, 0.01527, 0.0155780, .01527, 0.014384, 0.013019, 0.011324 },
	{ 0.011552, 0.013282, 0.014674, 0.015578, 0.015891, 0.015578, 0.014674, 0.013282, 0.011552 },
	{ 0.011324, 0.013019, 0.014384, 0.01527, 0.0155780, .01527, 0.014384, 0.013019, 0.011324 },
	{ 0.010667, 0.012264, 0.013549, 0.014384, 0.014674, 0.014384, 0.013549, 0.012264, 0.010667 },
	{ 0.009655, 0.0111, 0.012264, 0.013019, 0.013282, 0.013019, 0.012264, 0.0111, 0.009655 },
	{ 0.008397, 0.009655, 0.010667, 0.011324, 0.011552, 0.011324, 0.010667, 0.009655, 0.008397 }
	};

	float4 blurredOutput = float4(0, 0, 0, 1);

	for (int x = -4; x < 5; x++) {
		for (int y = -4; y < 5; y++) {
			blurredOutput += tex2D(tex, uv + float2(dir.x*x, dir.y*y))*KERNAL[x + 4][y + 4];
		}
	}

	return blurredOutput;
}
#endif // DIGITALSALMON_GAUSSIAN_INCLUDED