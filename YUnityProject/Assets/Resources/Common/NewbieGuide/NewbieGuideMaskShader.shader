Shader "UI/NewbieGuideMaskShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Blur("边缘虚化的范围", Range(1,1000)) = 1
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255		
		_ColorMask("Color Mask", Float) = 14
		_Origin("中心/矩形范围",Vector) = (0,0,0,0) // 矩形模式下格式：(xMin, yMin, xMax, yMax)
		_MaskType("Type",Float) = 0	// 0圆形 1矩形 2带额外裁剪的矩形
		_Radius("圆角半径", Range(0, 200)) = 25 // 新增：圆角半径参数
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]
		Pass
		{
			Name "Default"
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#include "UnityCG.cginc"
#include "UnityUI.cginc"

		struct appdata_t
		{
			float4 vertex : POSITION;
			float4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			float4 worldPosition : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		fixed4 _Color;
		fixed4 _TextureSampleAdd;
		float4 _ClipRect;
		float4 _Origin;
		float _MaskType;	
		float _Blur;
		float _Radius; // 新增：声明圆角半径变量

		v2f vert(appdata_t IN)
		{
			v2f OUT;
			UNITY_SETUP_INSTANCE_ID(IN);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
			OUT.worldPosition = IN.vertex;
			OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
			OUT.texcoord = IN.texcoord;
			OUT.color = IN.color * _Color;
			return OUT;
		}

		sampler2D _MainTex;
		fixed4 frag(v2f IN) : SV_Target
		{
			half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
			float2 pos = IN.worldPosition.xy;

			if (_MaskType == 0) {
				float dis = distance(pos, _Origin.xy);
				clip(dis - (_Origin.z - _Blur));
				fixed tmp = step(dis, _Origin.z);
				color.a *= (1 - tmp) + tmp * (dis - (_Origin.z - _Blur)) / _Blur;
			}
			// 矩形模式（带圆角）
			else if (_MaskType == 1) {
				float xMin = _Origin.x;
				float yMin = _Origin.y;
				float xMax = _Origin.z;
				float yMax = _Origin.w;
				bool inRect = (pos.x >= xMin && pos.x <= xMax && pos.y >= yMin && pos.y <= yMax);

				if (inRect && _Radius > 0) {
					// 四个圆角的圆心和范围判断
					float2 centerTL = float2(xMin + _Radius, yMax - _Radius); // 左上角
					float2 centerTR = float2(xMax - _Radius, yMax - _Radius); // 右上角
					float2 centerBR = float2(xMax - _Radius, yMin + _Radius); // 右下角
					float2 centerBL = float2(xMin + _Radius, yMin + _Radius); // 左下角

					float dis = 1e6;
					// 判断点属于哪个圆角区域
					if (pos.x <= centerTL.x && pos.y >= centerTL.y) dis = distance(pos, centerTL);
					else if (pos.x >= centerTR.x && pos.y >= centerTR.y) dis = distance(pos, centerTR);
					else if (pos.x >= centerBR.x && pos.y <= centerBR.y) dis = distance(pos, centerBR);
					else if (pos.x <= centerBL.x && pos.y <= centerBL.y) dis = distance(pos, centerBL);

					// 圆角区域处理（含虚化）
					if (dis != 1e6) {
						clip(dis - (_Radius - _Blur));
						fixed tmp = step(dis, _Radius);
						color.a *= (1 - tmp) + tmp * (dis - (_Radius - _Blur)) / _Blur;
					} else {
						color.a = 0; // 矩形非圆角区域直接透明
					}
				} else if (inRect) {
					color.a = 0; // 圆角半径为0时，保持原矩形逻辑
				}
			}
			// 带额外裁剪的矩形模式（带圆角）
			else if (_MaskType == 2) {
				float xMin = _Origin.x;
				float yMin = _Origin.y;
				float xMax = _Origin.z;
				float yMax = _Origin.w;
				bool inRect = (pos.x >= xMin && pos.x <= xMax && pos.y >= yMin && pos.y <= yMax);

				if (inRect) {
					color.a = 0;
					#ifdef UNITY_UI_CLIP_RECT
					color.a *= UnityGet2DClipping(pos, _Origin);
					#endif

					// 圆角处理（与MaskType=1逻辑一致）
					if (_Radius > 0) {
						float2 centerTL = float2(xMin + _Radius, yMax - _Radius);
						float2 centerTR = float2(xMax - _Radius, yMax - _Radius);
						float2 centerBR = float2(xMax - _Radius, yMin + _Radius);
						float2 centerBL = float2(xMin + _Radius, yMin + _Radius);

						float dis = 1e6;
						if (pos.x <= centerTL.x && pos.y >= centerTL.y) dis = distance(pos, centerTL);
						else if (pos.x >= centerTR.x && pos.y >= centerTR.y) dis = distance(pos, centerTR);
						else if (pos.x >= centerBR.x && pos.y <= centerBR.y) dis = distance(pos, centerBR);
						else if (pos.x <= centerBL.x && pos.y <= centerBL.y) dis = distance(pos, centerBL);

						if (dis != 1e6) {
							clip(dis - (_Radius - _Blur));
							fixed tmp = step(dis, _Radius);
							color.a *= (1 - tmp) + tmp * (dis - (_Radius - _Blur)) / _Blur;
						}
					}
				}
			}
			return color;
		}
		ENDCG
	}
	}
}