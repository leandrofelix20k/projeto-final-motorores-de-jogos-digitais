Shader "Custom/URP_Outline"
{
    Properties
    {
        _MainTex("Main Texture (RGB)", 2D) = "white" {}
        _Color("Main Color", Color) = (0.13, 0.18, 0.09, 1)

        _OutlineTex("Outline Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth("Outline Width", Range(1.0,10.0)) = 1.1
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On

        // --- Pass do contorno ---
        Pass
        {
            Name "OUTLINE"
            Cull Front
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_OutlineTex);
            SAMPLER(sampler_OutlineTex);
            float4 _OutlineTex_ST;
            float4 _OutlineColor;
            float _OutlineWidth;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 scaledVertex = IN.positionOS.xyz * _OutlineWidth;
                OUT.positionCS = TransformObjectToHClip(scaledVertex);
                OUT.uv = TRANSFORM_TEX(IN.uv, _OutlineTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 texColor = SAMPLE_TEXTURE2D(_OutlineTex, sampler_OutlineTex, IN.uv);
                return texColor * _OutlineColor;
            }
            ENDHLSL
        }

        // --- Pass do objeto principal ---
        Pass
        {
            Name "OBJECT"
            Cull Back
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _Color;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                return texColor * _Color;
            }
            ENDHLSL
        }
    }
}
