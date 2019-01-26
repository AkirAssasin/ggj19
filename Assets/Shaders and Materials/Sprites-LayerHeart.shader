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

            #pragma vertex SpriteVert
            #pragma fragment frag

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
