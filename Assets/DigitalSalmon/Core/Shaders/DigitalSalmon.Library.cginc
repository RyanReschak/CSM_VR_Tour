#ifndef DIGITALSALMON_LIBRARY_INCLUDED
#define DIGITALSALMON_LIBRARY_INCLUDED

//-----------------------------------------------------------------------------------------
// Utility:
//-----------------------------------------------------------------------------------------

float4 textureNoTile(sampler2D samp, float2 uv, float v )
{
    float2 p = floor( uv );
    float2 f = frac( uv );
	
    // derivatives (for correct mipmapping)
    float2 dFdx = ddx( uv );
    float2 dFdy = ddy( uv );
    
	float3 va = float3(0,0,0);
	float w1 = 0.0;
    float w2 = 0.0;
    for( int j=-1; j<=1; j++ )
    for( int i=-1; i<=1; i++ )
    {
        float2 g = float2( float(i),float(j) );
		float4 o = hash4( p + g );
		float2 r = g - f + o.xy;
		float d = dot(r,r);
        float w = exp(-5.0*d );
        float3 c = tex2D( samp, uv + v*o.zw, dFdx, dFdy ).xyz;
		va += w*c;
		w1 += w;
        w2 += w*w;
    }
    
    // normal averaging --> lowers contrasts
    return float4(va/w1,1);

    // contrast preserving average
    float mean = 0.3;// textureGrad( samp, uv, dFdx*16.0, dFdy*16.0 ).x;
    float3 res = mean + (va-w1*mean)/sqrt(w2);
    return float4(lerp( va/w1, res, v ),1);
}

#endif // DIGITALSALMON_LIBRARY_INCLUDED