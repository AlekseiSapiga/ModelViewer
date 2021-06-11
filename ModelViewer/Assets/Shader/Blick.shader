Shader "Unlit/Blick"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}		
		_sizeX ("SizeX", Range(0, 16)) = 0
		_offsetX ("OffsetX", Range(-20, 20)) = 0
		_intensity ("Intensity", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { 
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True" 
			}
        
		Blend SrcAlpha One
		//Blend SrcAlpha OneMinusSrcAlpha
		
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
			half _sizeX;
			half _offsetX;
			float _intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
				float2 newUv = float2(_offsetX, 0.0) + uv * float2(_sizeX, 1.0);
				
				o.uv = newUv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _intensity;				
                return col;
            }
            ENDCG
        }
    }
}
