Shader "Custom/GPUAudience"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		Cull Off
		Lighting Off
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			
			#include "UnityCG.cginc"

			struct AudienceData
			{
				float3 Position;
				float2x2 Rotation;
			};

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			StructuredBuffer<AudienceData> AudienceDataBuffer;
			float3 AudienceMeshScale;

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			
			v2f vert (appdata_t v, uint instanceID: SV_InstanceID)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(V);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4x4 matrix_ = (float4x4)0;
				matrix_._11_22_33_44 = float4(AudienceMeshScale.xyz, 1.0);
				matrix_._14_24_34 += AudienceDataBuffer[instanceID].Position;
				v.vertex = mul(matrix_, v.vertex);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				return col;
			}
			ENDCG
		}
	}
}
