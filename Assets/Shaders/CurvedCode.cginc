#include "UnityCG.cginc" 

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    UNITY_FOG_COORDS(1)
    float4 color : TEXCOORD2;
    float4 vertex : SV_POSITION;
};

sampler2D _MainTex;
float4 _MainTex_ST;
float _CurveStrength; // Strength of the curve for horizontal and vertical axes
float _VerticalCurveStrength; // New parameter for vertical curve strength

v2f vert(appdata v)
{
    v2f o;

    o.vertex = UnityObjectToClipPos(v.vertex);

    // Calculate distance from the camera
    float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(o.vertex.z);

    // Apply horizontal curving
    o.vertex.y -= _CurveStrength * dist * dist * _ProjectionParams.x;

    // Apply vertical curving
    o.vertex.x -= _VerticalCurveStrength * dist * dist * _ProjectionParams.x;

    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

    o.color = v.color;

    UNITY_TRANSFER_FOG(o, o.vertex);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    // Sample the texture
    fixed4 col = tex2D(_MainTex, i.uv) * i.color;
    // Apply fog
    UNITY_APPLY_FOG(i.fogCoord, col);
    return col;
}