shader "UnityShader/Test/Debug"
{
    subshader{
        pass{
            CGPROGRAM
            
            #pragam vertex vert;
            #pragam fragment frag;
            #include "UnityCG.cginc"
            struct v2f{
                float4 pos:SV_POSITION;
                fixed4 color:COLOR0;
            };
            v2f vert(appdata_full v){
                v2f o;
                o.pos=mul(UnityObjectToClipPos,v.vertex);
                o.color=fixed4(v.NORMAL*0.5+fixed3(0.5,0.5,0.5),1.0);
                return o;
            }
            fixed4 frag(v2f i):sv:SV_TARGET{
                return i.Color;
            }
             ENDCG 
        }       
    }
}