Shader "Custom/AlwaysOnTopShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _DepthOffset ("Depth Offset", Float) = 0.0 // Default to no offset
    }
    SubShader
    {
        Tags {"Queue"="Overlay"} // Render last
        ZWrite Off // Turn off depth writing
        ZTest Always // Always pass depth test
        Blend SrcAlpha OneMinusSrcAlpha // Normal alpha blending
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            fixed4 _Color;
            float _DepthOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                // Apply depth offset to move the vertex slightly closer or further from the camera
                o.pos.z += _DepthOffset;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
