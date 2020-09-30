// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Per pixel bumped refraction.
// Uses a normal map to distort the image behind, and
// an additional texture to tint the color.

Shader "@MGKJ_Glass_Base" {
Properties {


	_MainTex ("DiffuseMap", 2D) = "white" {}

	_brightness("Brightness", Range(1,5) ) = 1
	_contrast("Contrast", Range(1,2.5) ) = 1

	_lightmap_color("Base Color", Color) = (0,0,0,1)
	_LightMap ("SecondMap (CompleteMap or LightMap)", 2D) = "white" {}

	_BumpAmt  ("Distortion", range (0,64)) = 10
	//_lightmapDistortion  ("Lightmap Distortion", range (0,0.0001)) = 0
	_cubemapDistortion  ("Cubemap Distortion", range (0,5)) = 0
	//_MainTex ("Diffuse Color (RGB)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	//添加项
	_reflect_blender("Reflect Blender", range (0,1)) = 0.5
	_fresnel_ctrl("Fresnel", range (0,2)) = 0.5
	_highlight("High Light Contrast ", range (2,20)) = 2
	_CubeMap("Reflect Cump", Cube) = "black" {}
	
	_changeType ("<<<HDR Type    ***    NoHDR>>>", Range(0,1)) =0
			
	_SelectColor ("Outline Color", Color) = (0.2,0.6,0.8,0)
	_SelectColorAlpha("Select Color Alpha",Range(0,1) ) = 0
	
	
    
}


Category {

	// We must be transparent, so other objects are drawn before this one.
	//折射物体为渲染顺序为transparent
	Tags { "Queue"="Transparent" "RenderType"="Opaque" }
	LOD 200

	SubShader {

		// This pass grabs the screen behind the object into a texture.
		// We can access the result in the next pass as _GrabTexture
		//GrabPass得到屏幕图
		GrabPass {							
			Name "BASE"
			Tags { "LightMode" = "Always" }
 		}
 		//perturb为扰动，噪波
 		// Main pass: Take the texture grabbed above and use the bumpmap to perturb it
 		// on to the screen
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			
CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
//#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

struct appdata_t {
	float4 vertex : POSITION;
	float3 normal			:NORMAL;
	float2 texcoord: TEXCOORD0;
	//添加项
	float2 texcoord1: TEXCOORD1;
};

struct v2f {
	float4 vertex : POSITION;
	float4 uvgrab : TEXCOORD0;
	float2 uvbump : TEXCOORD1;
	float2 uvmain : TEXCOORD2;
	
	//添加项
	float2 uvlightmap : TEXCOORD3;
	float3 R	:TEXCOORD4;
	//float3 N	: COLOR;
	float4 fresnel_intensity :TEXCOORD5;
};

float _BumpAmt;
float4 _BumpMap_ST;
float4 _MainTex_ST;
float _fresnel_ctrl;

		float _GLOBALBRIGHTNESS;
		float _GLOBALCONTRASR;
		
		
float _changeType;
		
		

v2f vert (appdata_t v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	#if UNITY_UV_STARTS_AT_TOP
	float scale = -1.0;
	#else
	float scale = 1.0;
	#endif
	//cubemap normal计算
	float3 V=normalize(WorldSpaceViewDir(v.vertex));
	float4 worldposition =mul(unity_ObjectToWorld,v.vertex);
	float3 N=normalize(mul((float3x3)unity_ObjectToWorld,v.normal));	
	
	o.fresnel_intensity=1-clamp(pow((max(0,dot(V,N))),_fresnel_ctrl).xxxx,0.1,0.8);
	//o.fresnel_intensity=pow((1-max(0,dot(V,N))),_fresnel_ctrl).xxxx;
	//o.fresnel_intensity=1;

	float3 I = worldposition.xyz-_WorldSpaceCameraPos;
	o.R= reflect(I,N);
	//cubemap反射方位错位问题
	o.R.x=-o.R.x;
	//得到屏幕uv
	o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
	o.uvgrab.zw = o.vertex.zw;
	
	o.uvbump = TRANSFORM_TEX( v.texcoord, _BumpMap );
	o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
	o.uvlightmap = v.texcoord1;
	return o;
}

sampler2D _GrabTexture;
float4 _GrabTexture_TexelSize;
sampler2D _BumpMap;
sampler2D _MainTex;
//添加项
float _brightness;
float _contrast;
//float _lightmapDistortion;
float _cubemapDistortion ;
sampler2D _LightMap;
samplerCUBE _CubeMap;
float _reflect_blender;
float _highlight;
float4 _lightmap_color;




inline float3 DecodeLightmap2( float4 color )
{
#if defined(SHADER_API_GLES) && defined(SHADER_API_MOBILE)
	return 2.0 * color.rgb;
#else
	// potentially faster to do the scalar multiplication
	// in parenthesis for scalar GPUs
//	return pow((8.0 * color.a),2.2) * color.rgb;
	return pow((_GLOBALBRIGHTNESS+_brightness)* lerp(8.0 * color.a,1,_changeType)* color.rgb,_contrast+_GLOBALCONTRASR);
	
#endif
}	





half4 frag( v2f i ) : COLOR
{
	// calculate perturbed coordinates
	//添加normal map
	half3 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump )).rgb; // we could optimize this by just reading the x & y without reconstructing the Z
	float2 offset = bump.rg * _BumpAmt * _GrabTexture_TexelSize.xy;
	i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
	//添加项
	
	
	//i.uvlightmap.xy=_lightmapDistortion*bump*_BumpAmt+i.uvlightmap.xy;
	i.R=float3(bump*_cubemapDistortion)+i.R;
	//最后合成
	half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
	//half4 tint = tex2D( _MainTex, i.uvmain );
	//添加项
	//_brightness
	//_contrast
	
//	float4 lihghtmapcolor= tex2D( _LightMap, i.uvlightmap );
//	lihghtmapcolor=pow(lihghtmapcolor+_brightness.xxxx,_contrast)-_brightness.xxxx+_lightmap_color;
	float4  	lihghtmapcolor=0;
	lihghtmapcolor+=_lightmap_color*float4(DecodeLightmap2(tex2D(_LightMap,(i.uvlightmap).xy)),1);

	
	
	
	float4 cubumapcolor=texCUBE(_CubeMap,i.R);
	float4 cubemapcolorhighlight=pow(cubumapcolor, floor(_highlight));//_highlight
	return lihghtmapcolor*(col * (1-i.fresnel_intensity*_reflect_blender)+_reflect_blender*i.fresnel_intensity*cubumapcolor+_reflect_blender*i.fresnel_intensity*cubemapcolorhighlight);

	//return cubumapcolor;
	//return i.fresnel_intensity;
}
ENDCG
		}
	}

	
	// ------------------------------------------------------------------
	// Fallback for older cards and Unity non-Pro
	
	SubShader {
		Blend DstColor Zero
		Pass {
			Name "BASE"
			SetTexture [_MainTex] {	combine texture }
		}
	}
}

}
