Shader "CG/S16_MyCoal"                                                   // ← 1번째 줄
{
    Properties                                                           // ← 추가 (SubShader 위)
    {
        _BaseColor ("Base Color", Color) = (0, 0, 0, 1)
        [HDR] _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _BaseColor;                                           // ← 추가
            float4 _EmissionColor;                                       // ← 추가

            float4 vert(float3 positionOS : POSITION) : SV_POSITION
            {
                return TransformObjectToHClip(positionOS);
            }

            float4 frag() : SV_Target
            {
                return float4(_BaseColor.rgb + _EmissionColor.rgb, 1);   // ← 변경
            }
            ENDHLSL
        }
    }
}