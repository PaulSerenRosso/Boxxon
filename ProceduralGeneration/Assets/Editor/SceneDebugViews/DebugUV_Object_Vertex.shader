

Shader "DebugView/Debug UVs (Object,Vertex,Remapped)"
{
    SubShader 
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }

        Pass 
        {
            ZWrite On
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // vertex input: position, normal
            struct appdata 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                fixed3 normal : COLOR;
                float2 uv : TEXCOORD0;
            };
            
            v2f vert (appdata v) 
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.uv = v.uv;
                o.normal.xyz = v.normal;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                return half4(i.uv,0, 0); }
            ENDCG
        }
    }
}