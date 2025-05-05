Shader "Hidden/visualArrows" {
    SubShader {
        Tags { "RenderType" = "Transparent" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 UV       : TEXCOORD0;
            };

            struct v2f {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
            }; 
            StructuredBuffer<float3> YQrKFlBLknJwYAaiIvvJ;
            StructuredBuffer<float3> xmTdDnxUVSUdzezTxjIx;

            float4x4 getMatrix(uint instanceID) {
                float3 lookAtVector = xmTdDnxUVSUdzezTxjIx[instanceID];
                float lookLength = saturate(length(lookAtVector));
                float3 coordRight   = normalize(cross(float3(0.0,1.0,0.0), (lookAtVector)));
                float3 coordUp      = normalize(cross((lookAtVector), coordRight));
                float3 scale        = lookLength > 0.1? float3(1,1,1) * lookLength : 0;
                float3 position     = YQrKFlBLknJwYAaiIvvJ[instanceID];

                float4x4  scaleMatrix = float4x4(
                scale.x,    0,        0,       position.x,
                0,    scale.y,     0,       position.y,
                0,       0,     scale.z,    position.z,
                0,       0,        0,              1.0
                );

                lookAtVector = normalize(lookAtVector);
                float4x4 lookAtMatrix = float4x4(
                coordRight.x,  coordUp.x,  lookAtVector.x,  0.0,
                coordRight.y,  coordUp.y,  lookAtVector.y,  0.0,
                coordRight.z,  coordUp.z,  lookAtVector.z,  0.0,
                0.0,        0.0,             0.0,  1.0
                );

                return mul(scaleMatrix, lookAtMatrix);
            }

            v2f vert(appdata_t i, uint instanceID: SV_InstanceID) {
                v2f o;

                float3 lookAtVector = xmTdDnxUVSUdzezTxjIx[instanceID];
                float lookLength = length(lookAtVector);

                float4x4 mat = getMatrix(instanceID);
                float4 pos = mul(mat, i.vertex);
                o.vertex = UnityObjectToClipPos(pos);
                o.color = 
                (
                lookLength < 1 ? lerp(float4(0,1,0,0), float4(1,0,0,1),lookLength) 
                : lerp(float4(1,0,0,1), float4(1,0,1,1),lookLength / 10.0)
                )
                * (i.UV.y);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                return i.color;
            }

            ENDCG
        }
    }
}

