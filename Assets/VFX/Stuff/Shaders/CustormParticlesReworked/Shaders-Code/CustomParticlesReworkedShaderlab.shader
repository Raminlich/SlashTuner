Shader "Unlit/CustomParticlesReworkedShaderlab"
{
    Properties

    {
        _MainTex ("MainTexture", 2D) = "white" {}
        _AlphaMask ("AlphMask", 2D) = "white" {}
        [HDR]_HDRColor ("HDR Color", Color) = (1,1,1,1)
        [AllIn1VfxGradient] _ColorRampTexGradient("Color Ramp Gradient", 2D) = "white" {} //50


        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcBlend ("SrcFactor", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstBlend ("DstFactor", Float) = 1

        _TilingOffsetMain ("Tiling and Offset Main", Vector) = (1, 1, 0, 0)
        _TilingOffsetMask ("Tiling and Offset Mask", Vector) = (1, 1, 0, 0)

        _ColorRampBlend ("Color Ramp Blend", Range(0, 1)) = 1 // 51
        _ColorRampLuminosity("Color Ramp luminosity", Range(-1, 1)) = 0 //49



    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        Blend [_SrcBlend] [_DstBlend]

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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

                // UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            // Connection Variables

            sampler2D _MainTex;
            sampler2D _AlphaMask;
            sampler2D _ColorRampTexGradient;

            float4 _HDRColor;
            float4 _MainTex_ST;
            float4 _AlphaMask_ST;
            float4 _TilingOffsetMain;
            float4 _TilingOffsetMask;

            half _ColorRampLuminosity, _ColorRampBlend;


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            //MyFunctions:
            // float4 textureSliding()
            // {
            //     return 12;
            // }


            //looks like main()?
            fixed4 frag(v2f i) : SV_Target
            {
                float2 tiledUV = i.uv.xy * _TilingOffsetMain.xy + _TilingOffsetMain.zw;
                float2 tiledUVMask = i.uv.xy * _TilingOffsetMask.xy + _TilingOffsetMask.zw;

                //apply frace to reapeat:
                tiledUV = frac(tiledUV);
                tiledUVMask = frac(tiledUVMask);

                float4 sampledMainTex = tex2D(_MainTex, tiledUV);
                float4 sampledAlphaMask = tex2D(_AlphaMask, tiledUVMask);

                float4 result = sampledMainTex * sampledAlphaMask;


                float luminance = 0.3 * result.r + 0.59 * result.g + 0.11 * result.b;
                float colorRampLuminance = saturate(luminance + _ColorRampLuminosity);
                float4 colorRampRes = tex2D(_ColorRampTexGradient, half2(colorRampLuminance, 0));
                result.rgb = lerp(result.rgb, colorRampRes.rgb, _ColorRampBlend);
                result.a = lerp(result.a, saturate(result.a * colorRampRes.a), _ColorRampBlend);


                return result * _HDRColor ;
            }
            ENDCG
        }
    }
}