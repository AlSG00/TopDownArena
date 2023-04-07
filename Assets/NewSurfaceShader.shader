Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _Color("Color", Color) = (1,1,1,1)
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
        _WindSpeed("Wind Speed", Range(0,1)) = 0.5
        _WindDirection("Wind Direction", Vector) = (1,0,0,0)
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            sampler2D _BumpMap;
            float4 _Color;
            float _Cutoff;
            float _WindSpeed;
            float4 _WindDirection;

            struct Input
            {
                float2 uv_MainTex;
                float3 worldPos;
            };

            void surf(Input IN, inout SurfaceOutput o)
            {
                // Calculate wind animation
                float2 windDir = _WindDirection.xz;
                float windFactor = dot(windDir, IN.worldPos.xz) * _WindSpeed + _Time.y;
                float2 windOffset = float2(sin(windFactor), cos(windFactor)) * 0.1;

                // Sample the main texture and normal map
                float4 tex = tex2D(_MainTex, IN.uv_MainTex + windOffset);
                float3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));

                // Set surface properties
                o.Albedo = tex.rgb * _Color.rgb;
                o.Alpha = tex.a;
                o.Normal = normalize(normal);
                o.Smoothness = 0.5;
                o.Metallic = 0;
                o.Specular = 0;

                // Alpha cutoff
                clip(tex.a - _Cutoff);
            }
            ENDCG
        }

            FallBack "Diffuse"
}
