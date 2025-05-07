Shader "Custom/TextureOverlayCorrected"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _OverlayTex ("Overlay Texture", 2D) = "white" {}
        _Region1 ("Region 1 (minX, maxX, minY, maxY)", Vector) = (0,0,1,1)
        _Region2 ("Region 2 (minX, maxX, minY, maxY)", Vector) = (0,0,1,1)
        _HasOverlayTexture ("Has Overlay Texture", float) = 0
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _OverlayTex;
            float4 _OverlayTex_ST;
            float4 _Region1;
            float4 _Region2;
            fixed4 _EmissionColor; 
            float _HasOverlayTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 finalColor;

                if (_HasOverlayTexture > 0 &&
                    i.uv.x >= _Region1.x && i.uv.x <= _Region1.y
                    && i.uv.y >= _Region1.z && i.uv.y <= _Region1.w)
                {
                    float normalizedU = (i.uv.x - _Region1.x) / (_Region1.y - _Region1.x);
                    float normalizedV = (i.uv.y - _Region1.z) / (_Region1.w - _Region1.z);

                    float overlayU = _Region2.x + normalizedU * (_Region2.y - _Region2.x);
                    float overlayV = _Region2.z + normalizedV * (_Region2.w - _Region2.z);

                    fixed4 overlayColor = tex2D(_OverlayTex, TRANSFORM_TEX(float2(overlayU, overlayV), _OverlayTex));

                    finalColor = overlayColor;
                }
                else
                {
                    finalColor = tex2D(_MainTex, TRANSFORM_TEX(i.uv, _MainTex));
                }

                return finalColor;
            }
            ENDCG
        }
    }
}