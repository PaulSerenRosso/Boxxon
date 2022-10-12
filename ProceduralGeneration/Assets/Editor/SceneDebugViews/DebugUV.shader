// Debug the Occlusion channel. Absolute.

Shader "DebugView/Debug UV"
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
            #define _OCCLUSIONMAP

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
                half2 uvVector = input.uv;

                InputData inputData;
                InitializeInputData(input, surfaceData.normalTS, inputData);
                // End copied sections

                return float4(uvVector.x, uvVector.y, 0, 0);
            }
            ENDHLSL
        }
    }

}