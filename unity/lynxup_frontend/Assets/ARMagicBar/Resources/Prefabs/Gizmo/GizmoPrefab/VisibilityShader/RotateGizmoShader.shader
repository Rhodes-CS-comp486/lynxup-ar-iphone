Shader "Custom/RotateGizmoShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Backface culling based on normal direction relative to the camera
                float3 viewDir = normalize(UnityWorldSpaceViewDir(i.pos));
                float visibility = dot(i.normal, viewDir);
                if (visibility < 0)
                    discard; // Culls the back half of the gizmo

                return _Color;
            }
            ENDCG

            // Depth buffer settings to always render on top
            ZWrite Off
            ZTest Always
        }
    }
}
