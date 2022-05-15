Shader "RunnerBoi/Wallpaint_Shd"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrushSize ("Brush Size", Range(1,10)) = 1
        //_BrushColor ("Brush Color", Color) = (1, 0, 0, 1)
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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BrushSize;
            //float4 _BrushColor;
            uniform float2  _PaintCoord;

            v2f vert (appdata v)
            {
                v2f o;

                o.uv = v.uv;
                o.normal = v.normal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 red = float4(1, 0, 0, 1);

                float dist = distance(i.uv, _PaintCoord);
                dist = step(_BrushSize/100.0, dist);

                fixed4 col = tex2D(_MainTex, i.uv);
                return (1 - dist) * red + (dist) * col;
            }
            ENDCG
        }
    }
}
