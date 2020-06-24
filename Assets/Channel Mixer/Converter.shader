// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VChannel Mixer/Converter"
{
	Properties
	{
		_R_Invert("R_Invert", Float) = 0
		_G_Invert("G_Invert", Float) = 0
		_B_Invert("B_Invert", Float) = 0
		_A_Invert("A_Invert", Float) = 0
		_Red_Channel("Red_Channel", 2D) = "white" {}
		_Green_Channel("Green_Channel", 2D) = "white" {}
		_Blue_Channel("Blue_Channel", 2D) = "white" {}
		_Alpha_Channel("Alpha_Channel", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			uniform sampler2D _Red_Channel;
			uniform float4 _Red_Channel_ST;
			uniform float _R_Invert;
			uniform sampler2D _Green_Channel;
			uniform float4 _Green_Channel_ST;
			uniform float _G_Invert;
			uniform sampler2D _Blue_Channel;
			uniform float4 _Blue_Channel_ST;
			uniform float _B_Invert;
			uniform sampler2D _Alpha_Channel;
			uniform float4 _Alpha_Channel_ST;
			uniform float _A_Invert;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float2 uv_Red_Channel = i.ase_texcoord.xy * _Red_Channel_ST.xy + _Red_Channel_ST.zw;
				float4 tex2DNode66 = tex2D( _Red_Channel, uv_Red_Channel );
				float4 temp_cast_0 = (_R_Invert).xxxx;
				float4 lerpResult10 = lerp( tex2DNode66 , ( temp_cast_0 - tex2DNode66 ) , _R_Invert);
				float2 uv_Green_Channel = i.ase_texcoord.xy * _Green_Channel_ST.xy + _Green_Channel_ST.zw;
				float4 tex2DNode67 = tex2D( _Green_Channel, uv_Green_Channel );
				float4 temp_cast_2 = (_G_Invert).xxxx;
				float4 lerpResult40 = lerp( tex2DNode67 , ( temp_cast_2 - tex2DNode67 ) , _G_Invert);
				float2 uv_Blue_Channel = i.ase_texcoord.xy * _Blue_Channel_ST.xy + _Blue_Channel_ST.zw;
				float4 tex2DNode68 = tex2D( _Blue_Channel, uv_Blue_Channel );
				float4 temp_cast_4 = (_B_Invert).xxxx;
				float4 lerpResult47 = lerp( tex2DNode68 , ( temp_cast_4 - tex2DNode68 ) , _B_Invert);
				float2 uv_Alpha_Channel = i.ase_texcoord.xy * _Alpha_Channel_ST.xy + _Alpha_Channel_ST.zw;
				float4 tex2DNode69 = tex2D( _Alpha_Channel, uv_Alpha_Channel );
				float4 temp_cast_6 = (_A_Invert).xxxx;
				float4 lerpResult56 = lerp( tex2DNode69 , ( temp_cast_6 - tex2DNode69 ) , _A_Invert);
				float4 appendResult87 = (float4(lerpResult10.r , lerpResult40.r , lerpResult47.r , lerpResult56.r));
				
				
				finalColor = appendResult87;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=15800
-1913;9;1831;990;733.772;-61.76601;1.3;True;True
Node;AmplifyShaderEditor.SamplerNode;67;-1344.006,481.4525;Float;True;Property;_Green_Channel;Green_Channel;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;69;-1411.625,1453.619;Float;True;Property;_Alpha_Channel;Alpha_Channel;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;68;-1379.134,950.519;Float;True;Property;_Blue_Channel;Blue_Channel;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-918.4467,143.3506;Float;True;Property;_R_Invert;R_Invert;0;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-952.1144,1611.956;Float;True;Property;_A_Invert;A_Invert;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-1016.434,687.7553;Float;True;Property;_G_Invert;G_Invert;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;66;-1302.768,-10.36337;Float;True;Property;_Red_Channel;Red_Channel;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;-958.0565,1157.661;Float;True;Property;_B_Invert;B_Invert;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;11;-673.4467,94.35059;Float;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;39;-832.4315,586.7553;Float;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;46;-713.0549,1108.661;Float;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;55;-707.1128,1562.956;Float;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;56;-416,1472;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;40;-432,496;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;10;-432,0;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;47;-416,992;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DecodeFloatRGBAHlpNode;45;-970.0566,1016.66;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;87;316.9285,542.8696;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DecodeFloatRGBAHlpNode;54;-964.1145,1470.955;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DecodeFloatRGBAHlpNode;38;-946.4343,438.7553;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;63;560,544;Float;False;True;2;Float;ASEMaterialInspector;0;1;VChannel Mixer/Converter;0770190933193b94aaa3065e307002fa;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;11;0;3;0
WireConnection;11;1;66;0
WireConnection;39;0;37;0
WireConnection;39;1;67;0
WireConnection;46;0;44;0
WireConnection;46;1;68;0
WireConnection;55;0;53;0
WireConnection;55;1;69;0
WireConnection;56;0;69;0
WireConnection;56;1;55;0
WireConnection;56;2;53;0
WireConnection;40;0;67;0
WireConnection;40;1;39;0
WireConnection;40;2;37;0
WireConnection;10;0;66;0
WireConnection;10;1;11;0
WireConnection;10;2;3;0
WireConnection;47;0;68;0
WireConnection;47;1;46;0
WireConnection;47;2;44;0
WireConnection;87;0;10;0
WireConnection;87;1;40;0
WireConnection;87;2;47;0
WireConnection;87;3;56;0
WireConnection;63;0;87;0
ASEEND*/
//CHKSM=231510D8CA0031796B0FFC49198F9690657181B9