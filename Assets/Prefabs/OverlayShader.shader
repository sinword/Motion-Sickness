Shader "Custom/OverlayShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OverlayTex ("Overlay (RGB)", 2D) = "white" {} // Overlay texture
        _OverlayColor ("Overlay Color", Color) = (1,1,1,1) // Color for the overlay
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _OverlayTex;
        fixed4 _OverlayColor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_OverlayTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 overlayColor = tex2D(_OverlayTex, IN.uv_OverlayTex) * _OverlayColor;
            o.Albedo = lerp(baseColor.rgb, overlayColor.rgb, overlayColor.a); // Blend based on overlay alpha
            o.Alpha = baseColor.a; // Preserve the original alpha
        }
        ENDCG
    }
    FallBack "Diffuse"
}
