Shader "Custom/TransparentObject" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Alpha ("Alpha", Range(0,1)) = 1
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
        fixed4 _Color;
        half _Alpha;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a * _Alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}