Shader "Unlit/NewUnlitShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "Queue"="Transparent"  }
        LOD 100
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha 
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"
            

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

            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = fixed4(i.uv, 0 ,1);
                fixed2 nUV = i.uv - fixed2( 0.5, 0.5); // ovs offseted

                float outterDist = 0.03;
                float innerDist = 0.01;
                float strikeDist = 0.0003;

                if( nUV.x > -outterDist && nUV.x < -innerDist && nUV.y < strikeDist && nUV.y > -strikeDist
                ||  nUV.x > innerDist && nUV.x < outterDist && nUV.y < strikeDist && nUV.y > -strikeDist
                ||  nUV.y > -outterDist && nUV.y < -innerDist && nUV.x < strikeDist && nUV.x > -strikeDist
                ||  nUV.y > innerDist && nUV.y < outterDist && nUV.x < strikeDist && nUV.x > -strikeDist){
                    return fixed4(1,1,1,1);
                }

                return fixed4(1,1,1,0);
            }
            ENDCG
        }
    }
}