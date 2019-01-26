// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// NOT COMPATIBLE WITH SPRITE PACKER / SPRITE ATLASES! (YET)


Shader "Sprites/Layer Heart"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        
        Stencil
             {
                Ref 2
                Comp Always
                Pass Replace

                WriteMask 2
                
            }

        CGPROGRAM

            #include "UnitySprites.cginc"

            #pragma vertex vert
            #pragma fragment frag

            v2f vert ( appdata_t IN) {
                
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);

                // WOBBLE BEGIN

                float4 worldPos = mul(IN.vertex, unity_ObjectToWorld);
                float2 samplePos = worldPos.xz;
                samplePos.x += _Time * 50;
                OUT.vertex.x += sin(samplePos.x) * 0.01;
                OUT.vertex.y += cos(samplePos.x) * 0.01;

                // WOBBLE END

                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                // OUT.vertex.x += cos(windSample) * 0.1;

                return OUT;
            }

            float _LayerHeight[8];
            fixed4 _Colors[8];

            fixed4 frag (v2f IN) : SV_Target {

                fixed4 c = SampleSpriteTexture(IN.texcoord);

                clip(c.a - 0.9); // replace with dxdy thing maybe

                for (int i = 0; i < 8; i++) {
                    
                   if (IN.texcoord.y <= _LayerHeight[i]) {
                     
                       return _Colors[i] * c.a;
                    
                    }
                    
                }

                return c * c.a;

            }

            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

        ENDCG

        }
    }
}
