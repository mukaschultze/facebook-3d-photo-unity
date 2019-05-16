Shader "Hidden/DepthDumpShader"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        [HideInInspector]_CameraDepthTexture ("Depth", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
                return o;
            }

            sampler2D _MainTex;
            sampler2D _LastCameraDepthTexture;

            fixed4 frag (v2f i) : SV_Target
            {  
                float depth = tex2D(_LastCameraDepthTexture, i.uv);
                //linear depth between camera and far clipping plane
                depth = 1 - Linear01Depth(depth); 

                return float4 (depth, depth, depth, 1);
            }
            ENDCG
        }
    }
}
