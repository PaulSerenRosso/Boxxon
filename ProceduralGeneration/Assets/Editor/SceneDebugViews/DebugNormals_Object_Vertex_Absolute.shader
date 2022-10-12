// Debug Normals:
//  - Object-space 
//  - Vertex normals 
//  - Absolute, so some normals may be clipped to black, but the output color is accurate and can be measured.

Shader "DebugView/Debug Normals (Object,Vertex,Absolute)"
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
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                fixed3 normal : COLOR;
            };
            
            v2f vert (appdata v) 
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.normal.xyz = v.normal;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                return half4(i.normal, 1); }
            ENDCG
        }
    }
}