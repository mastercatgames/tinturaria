
Shader "Sprites/GlowLevels"
{
Properties
{
[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
_AlphaIntensity_Fade_1("_AlphaIntensity_Fade_1", Range(0, 3)) = 1
_TintRGBA_Color_1("_TintRGBA_Color_1", COLOR) = (1,1,1,1)
_AlphaIntensity_Fade_2("_AlphaIntensity_Fade_2", Range(0, 3)) = 1
_TintRGBA_Color_2("_TintRGBA_Color_2", COLOR) = (1,1,1,1)
_OperationBlend_Fade_1("_OperationBlend_Fade_1", Range(0, 1)) = 1
_SpriteFade("SpriteFade", Range(0, 1)) = 1.0

// required for UI.Mask
[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
[HideInInspector]_Stencil("Stencil ID", Float) = 0
[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
[HideInInspector]_ColorMask("Color Mask", Float) = 15

}

SubShader
{

Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True"}
ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off

// required for UI.Mask
Stencil
{
Ref [_Stencil]
Comp [_StencilComp]
Pass [_StencilOp]
ReadMask [_StencilReadMask]
WriteMask [_StencilWriteMask]
}

Pass
{

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

struct appdata_t{
float4 vertex   : POSITION;
float4 color    : COLOR;
float2 texcoord : TEXCOORD0;
};

struct v2f
{
float2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
float4 color    : COLOR;
};

sampler2D _MainTex;
float _SpriteFade;
float _AlphaIntensity_Fade_1;
float4 _TintRGBA_Color_1;
float _AlphaIntensity_Fade_2;
float4 _TintRGBA_Color_2;
float _OperationBlend_Fade_1;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}


float4 TintRGBA(float4 txt, float4 color)
{
float3 tint = dot(txt.rgb, float3(.222, .707, .071));
tint.rgb *= color.rgb;
txt.rgb = lerp(txt.rgb,tint.rgb,color.a);
return txt;
}
float4 OperationBlend(float4 origin, float4 overlay, float blend)
{
float4 o = origin; 
o.a = overlay.a + origin.a * (1 - overlay.a);
o.rgb = (overlay.rgb * overlay.a + origin.rgb * origin.a * (1 - overlay.a)) / (o.a+0.0000001);
o.a = saturate(o.a);
o = lerp(origin, o, blend);
return o;
}

float4 AlphaIntensity(float4 txt,float fade)
{
if (txt.a < 1) txt.a = lerp(0, txt.a, fade);
return txt;
}

float4 frag (v2f i) : COLOR
{
float4 _MainTex_1 = tex2D(_MainTex, i.texcoord);
float4 AlphaIntensity_1 = AlphaIntensity(_MainTex_1,_AlphaIntensity_Fade_1);
float4 TintRGBA_1 = TintRGBA(AlphaIntensity_1,_TintRGBA_Color_1);
float4 _MainTex_2 = tex2D(_MainTex, i.texcoord);
float4 AlphaIntensity_2 = AlphaIntensity(_MainTex_2,_AlphaIntensity_Fade_2);
float4 TintRGBA_2 = TintRGBA(AlphaIntensity_2,_TintRGBA_Color_2);
float4 OperationBlend_1 = OperationBlend(TintRGBA_2, TintRGBA_1, _OperationBlend_Fade_1); 
float4 FinalResult = OperationBlend_1;
FinalResult.rgb *= i.color.rgb;
FinalResult.a = FinalResult.a * _SpriteFade * i.color.a;
return FinalResult;
}

ENDCG
}
}
Fallback "Sprites/Default"
}
