// Debug Normals:
//  - World-space 
//  - Fragment normals (includes normal maps). 
//  - Absolute, so some normals may be clipped to black, but the output color is accurate and can be measured.

Shader "DebugView/Debug Normals (World,Fragment,Absolute)"
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

            #define _NORMALMAP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"

            float4 frag(Varyings input) : SV_TARGET {

                // These parts copied from LitForwardPass.hlsl
                //
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                #if defined(_PARALLAXMAP)
                #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
                    half3 viewDirTS = input.viewDirTS;
                #else
                    half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, input.viewDirWS);
                #endif
                    ApplyPerPixelDisplacement(viewDirTS, input.uv);
                #endif

                SurfaceData surfaceData;
                InitializeStandardLitSurfaceData(input.uv, surfaceData);

                InputData inputData;
                InitializeInputData(input, surfaceData.normalTS, inputData);
                // End copied sections

                return float4(inputData.normalWS, 1);
            }
            ENDHLSL
        }
    }

}