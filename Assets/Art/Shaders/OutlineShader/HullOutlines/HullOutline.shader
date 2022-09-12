Shader "Outlines/HullOutline"
{
    Properties
    {
        _Thickness("Thickness", float) = 1 //the amount to extrude the outline mesh
        _Color("Color", Color) = (1, 1, 1, 1) // the color of the outline
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}
        LOD 100

        Pass
        {
            Name "Outlines"
            // cull front faces
            Cull Front

            HLSLPROGRAM
            
            //Standard URP requirements
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            // Register our functions
            #pragma vertex Vertex
            #pragma fragment Fragment

            // include logic file
            #include "HullOutlines.hlsl"

            ENDHLSL
        }
    }
}
