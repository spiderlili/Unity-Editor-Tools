 Shader "Debug/CustomMaterialGUIShader"
{
    Properties
    {
        _SrcBlend("SrcBlend", float) = 1
        _DstBlend("DstBlend", float) = 1
        _DebugRange("Debug Range", Range(0, 10)) = 1
        _MainTex ("Texture", 2D) = "white" {}
        _DebugVector("Vector", Vector) = (0, 0, 0, 0)
        _SaveDebugVectorFoldoutVal01("", Vector) = (0, 0, 0, 0)
        _SaveBlendState("Blend State", float) = 0
        _DebugColor("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend [_SrcBlend][_DstBlend]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _VECTOR_ENABLED_ON
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _DebugRange;
            half4 _DebugVector;
            half4 _DebugColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return _DebugRange * _DebugColor;
                return col;
            }
            ENDCG
        }
    }
    CustomEditor "CustomShaderGUI"
}
