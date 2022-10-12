// Debug Normals:
//  - World-space 
//  - Vertex normals. 
//  - Remaps normals from [-1, 1] to [0, 1]. (0,0,0) will be shown as gray (0.5, 0.5, 0.5). 

Shader "DebugView/Debug Normals (World,Vertex,Remapped)"
{
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent"
        }

        Pass
        {
            ZWrite On
            HLSLPROGRAM

            // This file uses the actual URP Vertex Shader, and just swaps out the fragment shader.
            
            #pragma vertex LitPassVertex
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"

            float4 frag(Varyings input) : SV_TARGET {

                // These parts copied from LitForwardPass.hlsl
                //
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                // End copied sections

                float3 normalized_normal = RangeRemap(-1, 1, input.normalWS);
                return float4(normalized_normal, 1);
            }
            ENDHLSL
        }
    }

}