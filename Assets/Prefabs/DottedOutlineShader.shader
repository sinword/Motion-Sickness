Shader "Custom/DottedOutlineShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range (.002, 0.03)) = .005
        _DottedLineTexture ("Dotted Line Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            fixed4 _OutlineColor;
            float _OutlineWidth;

            v2f vert(appdata_t v)
            {
                // just make a copy of incoming vertex data but scaled according to normal direction
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 norm = mul((float3x3) UNITY_MATRIX_IT_MV, v.normal);
                float2 offset = _OutlineWidth * norm.xy;
                o.pos.xy += offset;
                o.uv = v.vertex.xy;
                o.color = _OutlineColor;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _DottedLineTexture;

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the main texture and the dotted line texture
                fixed4 mainTex = tex2D(_MainTex, i.uv);
                fixed4 dottedLineTex = tex2D(_DottedLineTexture, i.uv);
                return mainTex * i.color * dottedLineTex;
            }
            ENDCG
        }
    }
}
