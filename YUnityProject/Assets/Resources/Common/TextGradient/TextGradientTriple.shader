Shader "UI/Text/TwoColor_LinearGradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Font Texture", 2D) = "white" {}
        _Color ("整体透明度", Color) = (1,1,1,1)
        _TextWidth ("Text实际宽度（脚本自动传）", Float) = 100.0
        
        _LeftColor ("左侧颜色", Color) = (1,0,0,1)  // 你的红色
        _RightColor ("右侧颜色", Color) = (1,1,0,1) // 你的黄色
        _GradientMidPos ("过渡中点（0~1）", Range(0.1, 0.9)) = 0.5
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
        }

        Cull Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float xRatio : TEXCOORD1; // 左→右比例（0=左，1=右）
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _TextWidth;
            float4 _LeftColor;
            float4 _RightColor;
            float _GradientMidPos;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.xRatio = (v.vertex.x + _TextWidth/2) / _TextWidth; // 精准比例
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float xRatio = saturate(i.xRatio);
                // 直接线性插值：0=左色，1=右色，中间0.5是混合色（红+黄=橙）
                fixed4 finalColor = lerp(_LeftColor, _RightColor, xRatio);

                // 叠加字体+平滑透明度
                fixed4 texColor = tex2D(_MainTex, i.uv);
                finalColor.a = smoothstep(0.1, 0.9, texColor.a) * i.color.a;
                clip(finalColor.a - 0.05);
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}