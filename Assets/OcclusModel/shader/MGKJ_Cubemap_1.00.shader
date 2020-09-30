//2012.03.10
//2013.06.26

Shader "@MGKJ_Cubemap" 
{
	Properties 
	{
	
		_SpecColor2("Specular Color", Color) = (0,0,0,1)
		_Shininess("Shininess", Range(0,1) ) = 0.5
		
		_Color ("Main Color", Color) = (0,0,0,1)
		_MainTex ("DiffuseMap", 2D) = "white" {}
		
		_globalInfluence("Global Influence", Range(0,1) ) = 1
	    _brightness("Brightness", Range(1,5) ) = 1
	    _contrast("Contrast", Range(1,2.5) ) = 1

		_lightmap_color("Lighting Color", Color) = (1,1,1,1)
		_LightMap ("SecondMap (CompleteMap or LightMap)", 2D) = "white" {}
		_changeType ("<<<HDR Type    ***    NoHDR>>>", Range(0,1)) =0
		
//		_NormalDeepth ("Normal Deepth", Range(-1,2) ) = 0.5 

		_CubeMap ("Reflection Cubemap", Cube) = "black" {}
		
		_Reflect("Reflect Blender", range (0,1)) = 0.5
		_FresnelPower("Fresnel", range (-5,0)) = -1
		_FresnelBias ("Fresnel Bias", Range(0.015, 1)) = 0.015
		
//		_Parallax ("Height", Range (-0.05, 0.05)) = 0.02
//		_relaxedcone_relief_map ("relaxedcone", 2D	) = "white" {}

		_SelectColor ("Outline Color", Color) = (0.2,0.6,0.8,0)
		_SelectColorAlpha("Select Color Alpha",Range(0,1) ) = 0
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong_wjm finalcolor:mycolor
		#pragma target 3.0
		
		#pragma profileoption MaxTexIndirections=64
		
		#define DEPTH_BIAS
		#define BORDER_CLAMP
		float4x4 _RotateUVM;
		float _GLOBALBRIGHTNESS;
		float _GLOBALCONTRASR;
		
		
		float depth_bias;
		float border_clamp;
		
		
		float4 _SpecColor2;
		
		float _Parallax;
		float _Shininess;
		float _brightness;
		float _contrast;
		float _NormalDeepth;
		float4 _Color;
		float4 _lightmap_color;
		
		float _globalInfluence;
		float _changeType;
		
		float _Reflect;
		float _FresnelPower;
		float _FresnelBias;
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _LightMap;
		sampler2D _relaxedcone_relief_map;
		samplerCUBE _CubeMap;

		float4 _SelectColor ;
		float _SelectColorAlpha;
		
		struct Input {
			float2 uv_MainTex;
//			float2 uv_BumpMap;
			float2 uv2_LightMap;
			float3 worldRefl;
			float3 viewDir;

			INTERNAL_DATA
		};

		struct SurfaceOutput_wjm 
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Gloss;
			half Specular;
			half Alpha;
		};
		
		
		inline half4 LightingBlinnPhong_wjm_PrePass (SurfaceOutput_wjm s, half4 light)
		{
			half3 spec = light.a * s.Gloss;
			half4 c;
			c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
			c.a = s.Alpha;
			return c;
		}
		
		
		inline half4 LightingBlinnPhong_wjm (SurfaceOutput_wjm s, half3 lightDir, half3 viewDir, half atten)
		{
			half3 h = normalize (lightDir + viewDir);
			
			half diff = max (0, dot ( lightDir, s.Normal ));
				
			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0);
				
			half4 lightPower;
			lightPower.rgb = _LightColor0.rgb * diff;
			lightPower.w = spec * Luminance (_LightColor0.rgb);
			lightPower *= atten * 2.0;
				
			half4 c;
			c.rgb = (s.Albedo * lightPower.rgb + lightPower.rgb * spec*s.Gloss);
			c.a = s.Alpha;
			return c;
		}
		

		
		void mycolor (Input IN, SurfaceOutput_wjm o, inout fixed4 color)
		{
//          color *= _lightmap_color;
		  color += _SelectColor*_SelectColorAlpha;
		}


		
// setup ray pos and dir based on view vector
// and apply _Parallax bias and _Parallax factor			
		void setup_ray(Input IN,out float3 p,out float3 v)
		{
			p = float3(IN.uv_MainTex,0);
			v = normalize(IN.viewDir);

	
			v.z = abs(v.z);	
			#ifdef DEPTH_BIAS
			float db = 1.0-v.z; db*=db; db*=db; db=1.0-db*db;
			v.xy *= db;
			#endif

			v.xy *= _Parallax;
		}
		
// ray intersect _Parallax map using binary cone space leaping
// _Parallax value stored in alpha channel (black is at object surface)
// and cone ratio stored in blue channel
void ray_intersect_relaxedcone(
	sampler2D relaxedcone_relief_map,
	inout float3 p,
	inout float3 v)
{
	const int cone_steps=10;
	const int binary_steps=5;
	
	float3 p0 = p;

	v /= v.z;
	
	float dist = length(v.xy);
	
	for( int i=0;i<cone_steps;i++ )
	{
		float4 tex = tex2D(relaxedcone_relief_map, p.xy);

		float height = saturate(tex.w - p.z);
		
		float cone_ratio = tex.z;
		
		p += v * (cone_ratio * height / (dist + cone_ratio));
	}

	v *= p.z*0.5;
	p = p0 + v;

	for( int i=0;i<binary_steps;i++ )
	{
		float4 tex = tex2D(relaxedcone_relief_map, p.xy);
		v *= 0.5;
		if (p.z<tex.w)
			p+=v;
		else
			p-=v;
	}
}
		
		
		
		
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
		
		
		void surf (Input IN, inout SurfaceOutput_wjm o)
		{			
//			float3 p,v;	
//			setup_ray(IN,p,v);
//			ray_intersect_relaxedcone(_relaxedcone_relief_map,p,v);
			IN.uv_MainTex=mul(IN.uv_MainTex.xyxy, _RotateUVM).xy;
			
//			o.Normal  = tex2D(_relaxedcone_relief_map,p.xy).xyz;
			
			float4 Tex2D0=tex2D(_MainTex,IN.uv_MainTex);
//			float4 Tex2D1=pow(_brightness.xxxx +tex2D(_LightMap,(IN.uv2_LightMap).xy),_contrast.xxxx)- _brightness.xxxx;
			
			float4 Tex2D1=0;
			Tex2D1+=_lightmap_color*float4(DecodeLightmap2(tex2D(_LightMap,(IN.uv2_LightMap).xy)),1);
			
			
			float3 worldRefl = WorldReflectionVector (IN, o.Normal);
			

//			worldRefl=reflect(IN.viewDir,IN.worldRefl)
			fixed4 cubeColor = texCUBE (_CubeMap, worldRefl)*min(2*dot(Tex2D1.rgb, fixed3(0.22, 0.707, 0.071) ),1);


			float fresnel_intensity=1-clamp(pow((max(0,dot (normalize(IN.viewDir), o.Normal))),-_FresnelPower).xxxx,0,1);
			fresnel_intensity=	min(fresnel_intensity + _FresnelBias, 1.0f);
			
			float3 diffuseColor=Tex2D0*Tex2D1;
			
			float3 diffuseFinalcolor=lerp(diffuseColor,cubeColor,_Reflect*fresnel_intensity);
			
			o.Albedo=_Color.rgb*diffuseFinalcolor;
//			o.Albedo=_Color.rgb*Tex2D0;
			
			o.Emission=_lightmap_color*diffuseFinalcolor;
			
			o.Specular=_Shininess ;
			
			o.Gloss =Tex2D0.a*_SpecColor2;
			
			o.Alpha = Tex2D0.a * _Color.a;


		}
	
		ENDCG
		
	} 
	FallBack "Diffuse"
}
