Shader "Hidden/Fade"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "black" {}
	}

		SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _Brightness;

			fixed4 frag(v2f_img i) : COLOR
			{
				float4 mainColor = tex2D(_MainTex, i.uv);
				float4 greyScale = dot(mainColor.argb, float4(0, 0, 0, 0)) * _Brightness;
				mainColor.argb = greyScale;

				return mainColor;
			}
			ENDCG
		}
	}

		Fallback off
}