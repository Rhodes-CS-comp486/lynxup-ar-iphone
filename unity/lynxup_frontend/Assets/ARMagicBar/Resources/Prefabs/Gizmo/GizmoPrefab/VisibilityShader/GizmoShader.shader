Shader "Custom/GizmoShader" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float3 viewDir : TEXCOORD0;
            };

            float4 _Color;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex.xyz));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Calculate the angle between the vertex normal and the view direction
                float angle = acos(dot(i.normal, -i.viewDir)); // Note: Use -i.viewDir to get the angle from the camera's perspective

                // Check if the angle is within the visibility range
                if (angle > radians(85.0)) discard; // Adjust for 60 degrees visibility on each side of the normal

                return _Color;
            }
            ENDCG
        }
    }
}
