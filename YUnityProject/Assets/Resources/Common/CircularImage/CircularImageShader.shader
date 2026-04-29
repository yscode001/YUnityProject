Shader "UI/CircularImageShader"
{
    Properties
    {
        _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15
        _Radius ("Radius", Range(0, 0.5)) = 0.5
    }

    SubShader
    {
        LOD 100
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Offset -1, -1
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.texcoord;
                fixed4 col = tex2D(_MainTex, uv) * i.color;

                // 优化：先裁掉透明，减少运算
                if (col.a <= 0.01) discard;

                float2 p = uv - 0.5f;

                if (_Radius >= 0.5f)
                {
                    // 正圆（最快算法）
                    float dist = dot(p, p);
                    if (dist > 0.25f) col.a = 0;
                }
                else
                {
                    // 圆角矩形（最优数学）
                    float2 ap = abs(p);
                    float2 c = ap - (0.5f - _Radius);
                    c = max(c, 0.0f);
                    float dist = dot(c, c);

                    if (dist > _Radius * _Radius)
                        col.a = 0;
                }

                return col;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}