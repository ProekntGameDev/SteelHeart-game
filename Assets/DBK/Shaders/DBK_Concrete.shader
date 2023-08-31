// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DBK LWRP/Concrete"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[PerRendererData]_Color("Color", Int) = 0
		_MainTex("Color Theme", 2D) = "white" {}
		_DamageNM("Damage NM", 2D) = "bump" {}
		_DamageColor("Damage Color", 2D) = "white" {}
		_DamageAmountSmooth("Damage Amount Smooth", Range( 0 , 17)) = 61.17647
		_DamageAmount("Damage Amount", Range( 0 , 1)) = 0.6
		_DamageBrightness("Damage Brightness", Range( 0 , 4)) = 0
		_DamageMultiplier("Damage Multiplier", Range( 0 , 1)) = 0.1946161
		_ConcreteDamageTiling("Concrete Damage Tiling", Range( 0 , 5)) = 0
		_ConcreteDamageScale("Concrete Damage Scale", Range( 1 , 2)) = 0
		_ConcreteNM("Concrete NM", 2D) = "bump" {}
		_PaintEdgesMultiply("Paint Edges Multiply", Range( 0 , 1)) = 1
		_ConcreteBareRange("Concrete Bare Range", Range( 0 , 1)) = 0
		_ConcreteBareSmooth("Concrete Bare Smooth", Range( 0 , 1)) = 0
		_EdgesOverlay("Edges Overlay", Range( 0 , 1)) = 0
		_DirtOverlay("Dirt Overlay", Range( 0.02 , 1)) = 0
		_TransitionNM("Transition NM", 2D) = "bump" {}
		_TransitionBrightness("Transition Brightness", Range( 0.5 , 2)) = 0
		_TransitionScale("Transition Scale", Range( 0 , 2)) = 0
		_TransitionAmount("Transition Amount", Range( 0 , 0.05)) = 0
		_RGBAMaskA("RGBA Mask A", 2D) = "white" {}
		_RGBAMaskB("RGBA Mask B", 2D) = "white" {}
		_DirtContrast("Dirt Contrast", Range( 0 , 2)) = 0
		_DirtRange("Dirt Range", Range( 0 , 1)) = 0
		[MaxGay]_DirtSmooth("Dirt Smooth", Range( 0 , 1)) = 0
		_MainSmoothness("Main Smoothness", Range( 0 , 1)) = 0
		_DirtSmoothness("Dirt Smoothness", Range( 0 , 3)) = 0
		_DamageSmoothness("Damage Smoothness", Range( 0 , 1)) = 0
		_BareSmoothness("Bare Smoothness", Range( 0 , 1)) = 0
		[Toggle(_USECUSTOMCOLOR_ON)] _UseCustomColor("Use Custom Color", Float) = 0
		_CustomColor("Custom Color", Int) = 0
		_BareBrightness("Bare Brightness", Range( 0 , 2)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
		Cull Back
		HLSLINCLUDE
		#pragma target 2.0

		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature _USECUSTOMCOLOR_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RGBAMaskB_ST;
			float4 _TransitionNM_ST;
			float4 _ConcreteNM_ST;
			float4 _RGBAMaskA_ST;
			float _ConcreteDamageTiling;
			float _DirtSmoothness;
			float _MainSmoothness;
			float _ConcreteDamageScale;
			float _TransitionScale;
			float _DirtContrast;
			float _DirtSmooth;
			float _DirtRange;
			float _DirtOverlay;
			float _EdgesOverlay;
			float _ConcreteBareSmooth;
			float _BareSmoothness;
			float _ConcreteBareRange;
			float _TransitionAmount;
			float _DamageAmountSmooth;
			float _DamageMultiplier;
			float _DamageAmount;
			float _TransitionBrightness;
			float _BareBrightness;
			int _CustomColor;
			int _Color;
			float _DamageBrightness;
			float _PaintEdgesMultiply;
			float _DamageSmoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _DamageColor;
			sampler2D _RGBAMaskB;
			sampler2D _MainTex;
			sampler2D _RGBAMaskA;
			sampler2D _ConcreteNM;
			sampler2D _TransitionNM;
			sampler2D _DamageNM;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord7.xy = v.ase_texcoord.xy;
				o.ase_texcoord7.zw = v.ase_texcoord3.xy;
				o.ase_color = v.ase_color;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord1 = v.texcoord1;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord3 = v.ase_texcoord3;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord3 = patch[0].ase_texcoord3 * bary.x + patch[1].ase_texcoord3 * bary.y + patch[2].ase_texcoord3 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				float3 WorldNormal = normalize( IN.tSpace0.xyz );
				float3 WorldTangent = IN.tSpace1.xyz;
				float3 WorldBiTangent = IN.tSpace2.xyz;
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				#if SHADER_HINT_NICE_QUALITY
					WorldViewDirection = SafeNormalize( WorldViewDirection );
				#endif

				float2 temp_cast_0 = (_ConcreteDamageTiling).xx;
				float2 uv0385 = IN.ase_texcoord7.xy * temp_cast_0 + float2( 0,0 );
				float2 ConcreteTileUV386 = uv0385;
				float4 tex2DNode185 = tex2D( _DamageColor, ConcreteTileUV386 );
				float2 uv_RGBAMaskB = float4(IN.ase_texcoord7.xy,0,0).xy * _RGBAMaskB_ST.xy + _RGBAMaskB_ST.zw;
				float4 tex2DNode636 = tex2D( _RGBAMaskB, uv_RGBAMaskB );
				float ConcreteNewAlpha640 = tex2DNode636.a;
				#ifdef _USECUSTOMCOLOR_ON
				float staticSwitch635 = (float)_CustomColor;
				#else
				float staticSwitch635 = (float)_Color;
				#endif
				float2 appendResult613 = (float2(( staticSwitch635 * 0.015625 ) , -0.33));
				float2 uv3611 = IN.ase_texcoord7.zw * float2( 1,1 ) + appendResult613;
				float4 BareConcrete622 = tex2D( _MainTex, uv3611 );
				float4 temp_output_559_0 = ( ConcreteNewAlpha640 * BareConcrete622 * _BareBrightness );
				float HeightMask58 = saturate(pow(((( 1.0 - tex2DNode636.r )*( _DamageAmount * ( ( IN.ase_color.g * _DamageMultiplier ) * tex2DNode636.g ) ))*4)+(( _DamageAmount * ( ( IN.ase_color.g * _DamageMultiplier ) * tex2DNode636.g ) )*2),_DamageAmountSmooth));
				float HeightBricks203 = HeightMask58;
				float BricksStep210 = step( 0.1 , ( ( 1.0 - HeightBricks203 ) * step( _TransitionAmount , HeightBricks203 ) ) );
				float4 lerpResult459 = lerp( ( tex2DNode185 * _DamageBrightness ) , ( ( temp_output_559_0 + tex2DNode185 ) * _TransitionBrightness * ConcreteNewAlpha640 ) , BricksStep210);
				float2 uv3612 = float4(IN.ase_texcoord7.zw,0,0).xy * float2( 1,1 ) + ( half2( 0.015625,0 ) * staticSwitch635 );
				float4 PaintColor619 = tex2D( _MainTex, uv3612 );
				float HeightGreen564 = tex2DNode636.b;
				float3 temp_cast_3 = (( 1.0 - HeightGreen564 )).xxx;
				float VertexBlue392 = IN.ase_color.b;
				float2 uv_RGBAMaskA = float4(IN.ase_texcoord7.xy,0,0).xy * _RGBAMaskA_ST.xy + _RGBAMaskA_ST.zw;
				float4 tex2DNode325 = tex2D( _RGBAMaskA, uv_RGBAMaskA );
				float DirtMaskBlue413 = tex2DNode325.b;
				float3 temp_cast_4 = (( 1.0 - ( ( 1.0 - VertexBlue392 ) * DirtMaskBlue413 ) )).xxx;
				float temp_output_426_0 = saturate( ( 1.0 - ( ( distance( temp_cast_3 , temp_cast_4 ) - _ConcreteBareRange ) / max( _ConcreteBareSmooth , 1E-05 ) ) ) );
				float PaintEdges465 = temp_output_426_0;
				float PaintEdgesSelection471 = step( 0.1 , ( ( 1.0 - PaintEdges465 ) * step( 0.0 , PaintEdges465 ) ) );
				float4 lerpResult397 = lerp( PaintColor619 , ( temp_output_559_0 * ( 1.0 - ( PaintEdgesSelection471 * _PaintEdgesMultiply ) ) ) , temp_output_426_0);
				float temp_output_228_0 = pow( tex2DNode325.r , _DirtOverlay );
				float PaintSelection313 = ( ( 1.0 - HeightBricks203 ) * ( 1.0 - BricksStep210 ) );
				float4 lerpResult117 = lerp( lerpResult459 , ( ( lerpResult397 + ( lerpResult397 * _EdgesOverlay * tex2DNode325.g ) ) * temp_output_228_0 ) , PaintSelection313);
				float2 appendResult628 = (float2(( staticSwitch635 * 0.015625 ) , -0.66));
				float2 uv3627 = float4(IN.ase_texcoord7.zw,0,0).xy * float2( 1,1 ) + appendResult628;
				float4 DirtColor630 = tex2D( _MainTex, uv3627 );
				float VertexRed336 = IN.ase_color.r;
				float3 temp_cast_5 = (( VertexRed336 * ( 1.0 - tex2DNode325.g ) )).xxx;
				float3 temp_cast_6 = (( 1.0 - HeightGreen564 )).xxx;
				float temp_output_309_0 = ( ( tex2DNode325.a * saturate( ( 1.0 - ( ( distance( temp_cast_5 , temp_cast_6 ) - _DirtRange ) / max( _DirtSmooth , 1E-05 ) ) ) ) ) * _DirtContrast );
				float4 lerpResult308 = lerp( lerpResult117 , ( DirtColor630 * ConcreteNewAlpha640 ) , temp_output_309_0);
				float DamageAlpha599 = tex2DNode185.a;
				float lerpResult596 = lerp( ConcreteNewAlpha640 , DamageAlpha599 , PaintSelection313);
				float4 Albedo323 = ( lerpResult308 * lerpResult596 );
				
				float2 uv_ConcreteNM = float4(IN.ase_texcoord7.xy,0,0).xy * _ConcreteNM_ST.xy + _ConcreteNM_ST.zw;
				float2 uv_TransitionNM = float4(IN.ase_texcoord7.xy,0,0).xy * _TransitionNM_ST.xy + _TransitionNM_ST.zw;
				float3 lerpResult1 = lerp( UnpackNormalScale( tex2D( _ConcreteNM, uv_ConcreteNM ), 1.0f ) , UnpackNormalScale( tex2D( _TransitionNM, uv_TransitionNM ), _TransitionScale ) , BricksStep210);
				float3 lerpResult68 = lerp( lerpResult1 , UnpackNormalScale( tex2D( _DamageNM, ConcreteTileUV386 ), _ConcreteDamageScale ) , HeightBricks203);
				float3 Normals320 = lerpResult68;
				
				float PaintSmoothness351 = temp_output_228_0;
				float DirtSmothness366 = temp_output_309_0;
				float ConcreteDamage375 = tex2DNode185.r;
				float lerpResult349 = lerp( ( ( PaintSmoothness351 * _MainSmoothness ) * ( 1.0 - ( DirtSmothness366 * _DirtSmoothness ) ) * ( 1.0 - ( PaintEdges465 * _BareSmoothness ) ) ) , ( ConcreteDamage375 * _DamageSmoothness ) , ( 1.0 - PaintSelection313 ));
				float Smoothness359 = lerpResult349;
				
				float3 Albedo = Albedo323.rgb;
				float3 Normal = Normals320;
				float3 Emission = 0;
				float3 Specular = 0.5;
				float Metallic = 0;
				float Smoothness = Smoothness359;
				float Occlusion = 1;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					inputData.normalWS = normalize(TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal )));
				#else
					#if !SHADER_HINT_NICE_QUALITY
						inputData.normalWS = WorldNormal;
					#else
						inputData.normalWS = normalize( WorldNormal );
					#endif
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, IN.lightmapUVOrVertexSH.xyz, inputData.normalWS );
				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				half4 color = UniversalFragmentPBR(
					inputData, 
					Albedo, 
					Metallic, 
					Specular, 
					Smoothness, 
					Occlusion, 
					Emission, 
					Alpha);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, WorldNormal ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RGBAMaskB_ST;
			float4 _TransitionNM_ST;
			float4 _ConcreteNM_ST;
			float4 _RGBAMaskA_ST;
			float _ConcreteDamageTiling;
			float _DirtSmoothness;
			float _MainSmoothness;
			float _ConcreteDamageScale;
			float _TransitionScale;
			float _DirtContrast;
			float _DirtSmooth;
			float _DirtRange;
			float _DirtOverlay;
			float _EdgesOverlay;
			float _ConcreteBareSmooth;
			float _BareSmoothness;
			float _ConcreteBareRange;
			float _TransitionAmount;
			float _DamageAmountSmooth;
			float _DamageMultiplier;
			float _DamageAmount;
			float _TransitionBrightness;
			float _BareBrightness;
			int _CustomColor;
			int _Color;
			float _DamageBrightness;
			float _PaintEdgesMultiply;
			float _DamageSmoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			
			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );
				
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RGBAMaskB_ST;
			float4 _TransitionNM_ST;
			float4 _ConcreteNM_ST;
			float4 _RGBAMaskA_ST;
			float _ConcreteDamageTiling;
			float _DirtSmoothness;
			float _MainSmoothness;
			float _ConcreteDamageScale;
			float _TransitionScale;
			float _DirtContrast;
			float _DirtSmooth;
			float _DirtRange;
			float _DirtOverlay;
			float _EdgesOverlay;
			float _ConcreteBareSmooth;
			float _BareSmoothness;
			float _ConcreteBareRange;
			float _TransitionAmount;
			float _DamageAmountSmooth;
			float _DamageMultiplier;
			float _DamageAmount;
			float _TransitionBrightness;
			float _BareBrightness;
			int _CustomColor;
			int _Color;
			float _DamageBrightness;
			float _PaintEdgesMultiply;
			float _DamageSmoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature _USECUSTOMCOLOR_ON


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RGBAMaskB_ST;
			float4 _TransitionNM_ST;
			float4 _ConcreteNM_ST;
			float4 _RGBAMaskA_ST;
			float _ConcreteDamageTiling;
			float _DirtSmoothness;
			float _MainSmoothness;
			float _ConcreteDamageScale;
			float _TransitionScale;
			float _DirtContrast;
			float _DirtSmooth;
			float _DirtRange;
			float _DirtOverlay;
			float _EdgesOverlay;
			float _ConcreteBareSmooth;
			float _BareSmoothness;
			float _ConcreteBareRange;
			float _TransitionAmount;
			float _DamageAmountSmooth;
			float _DamageMultiplier;
			float _DamageAmount;
			float _TransitionBrightness;
			float _BareBrightness;
			int _CustomColor;
			int _Color;
			float _DamageBrightness;
			float _PaintEdgesMultiply;
			float _DamageSmoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _DamageColor;
			sampler2D _RGBAMaskB;
			sampler2D _MainTex;
			sampler2D _RGBAMaskA;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord2.zw = v.ase_texcoord3.xy;
				o.ase_color = v.ase_color;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord3 = v.ase_texcoord3;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord3 = patch[0].ase_texcoord3 * bary.x + patch[1].ase_texcoord3 * bary.y + patch[2].ase_texcoord3 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 temp_cast_0 = (_ConcreteDamageTiling).xx;
				float2 uv0385 = IN.ase_texcoord2.xy * temp_cast_0 + float2( 0,0 );
				float2 ConcreteTileUV386 = uv0385;
				float4 tex2DNode185 = tex2D( _DamageColor, ConcreteTileUV386 );
				float2 uv_RGBAMaskB = float4(IN.ase_texcoord2.xy,0,0).xy * _RGBAMaskB_ST.xy + _RGBAMaskB_ST.zw;
				float4 tex2DNode636 = tex2D( _RGBAMaskB, uv_RGBAMaskB );
				float ConcreteNewAlpha640 = tex2DNode636.a;
				#ifdef _USECUSTOMCOLOR_ON
				float staticSwitch635 = (float)_CustomColor;
				#else
				float staticSwitch635 = (float)_Color;
				#endif
				float2 appendResult613 = (float2(( staticSwitch635 * 0.015625 ) , -0.33));
				float2 uv3611 = IN.ase_texcoord2.zw * float2( 1,1 ) + appendResult613;
				float4 BareConcrete622 = tex2D( _MainTex, uv3611 );
				float4 temp_output_559_0 = ( ConcreteNewAlpha640 * BareConcrete622 * _BareBrightness );
				float HeightMask58 = saturate(pow(((( 1.0 - tex2DNode636.r )*( _DamageAmount * ( ( IN.ase_color.g * _DamageMultiplier ) * tex2DNode636.g ) ))*4)+(( _DamageAmount * ( ( IN.ase_color.g * _DamageMultiplier ) * tex2DNode636.g ) )*2),_DamageAmountSmooth));
				float HeightBricks203 = HeightMask58;
				float BricksStep210 = step( 0.1 , ( ( 1.0 - HeightBricks203 ) * step( _TransitionAmount , HeightBricks203 ) ) );
				float4 lerpResult459 = lerp( ( tex2DNode185 * _DamageBrightness ) , ( ( temp_output_559_0 + tex2DNode185 ) * _TransitionBrightness * ConcreteNewAlpha640 ) , BricksStep210);
				float2 uv3612 = float4(IN.ase_texcoord2.zw,0,0).xy * float2( 1,1 ) + ( half2( 0.015625,0 ) * staticSwitch635 );
				float4 PaintColor619 = tex2D( _MainTex, uv3612 );
				float HeightGreen564 = tex2DNode636.b;
				float3 temp_cast_3 = (( 1.0 - HeightGreen564 )).xxx;
				float VertexBlue392 = IN.ase_color.b;
				float2 uv_RGBAMaskA = float4(IN.ase_texcoord2.xy,0,0).xy * _RGBAMaskA_ST.xy + _RGBAMaskA_ST.zw;
				float4 tex2DNode325 = tex2D( _RGBAMaskA, uv_RGBAMaskA );
				float DirtMaskBlue413 = tex2DNode325.b;
				float3 temp_cast_4 = (( 1.0 - ( ( 1.0 - VertexBlue392 ) * DirtMaskBlue413 ) )).xxx;
				float temp_output_426_0 = saturate( ( 1.0 - ( ( distance( temp_cast_3 , temp_cast_4 ) - _ConcreteBareRange ) / max( _ConcreteBareSmooth , 1E-05 ) ) ) );
				float PaintEdges465 = temp_output_426_0;
				float PaintEdgesSelection471 = step( 0.1 , ( ( 1.0 - PaintEdges465 ) * step( 0.0 , PaintEdges465 ) ) );
				float4 lerpResult397 = lerp( PaintColor619 , ( temp_output_559_0 * ( 1.0 - ( PaintEdgesSelection471 * _PaintEdgesMultiply ) ) ) , temp_output_426_0);
				float temp_output_228_0 = pow( tex2DNode325.r , _DirtOverlay );
				float PaintSelection313 = ( ( 1.0 - HeightBricks203 ) * ( 1.0 - BricksStep210 ) );
				float4 lerpResult117 = lerp( lerpResult459 , ( ( lerpResult397 + ( lerpResult397 * _EdgesOverlay * tex2DNode325.g ) ) * temp_output_228_0 ) , PaintSelection313);
				float2 appendResult628 = (float2(( staticSwitch635 * 0.015625 ) , -0.66));
				float2 uv3627 = float4(IN.ase_texcoord2.zw,0,0).xy * float2( 1,1 ) + appendResult628;
				float4 DirtColor630 = tex2D( _MainTex, uv3627 );
				float VertexRed336 = IN.ase_color.r;
				float3 temp_cast_5 = (( VertexRed336 * ( 1.0 - tex2DNode325.g ) )).xxx;
				float3 temp_cast_6 = (( 1.0 - HeightGreen564 )).xxx;
				float temp_output_309_0 = ( ( tex2DNode325.a * saturate( ( 1.0 - ( ( distance( temp_cast_5 , temp_cast_6 ) - _DirtRange ) / max( _DirtSmooth , 1E-05 ) ) ) ) ) * _DirtContrast );
				float4 lerpResult308 = lerp( lerpResult117 , ( DirtColor630 * ConcreteNewAlpha640 ) , temp_output_309_0);
				float DamageAlpha599 = tex2DNode185.a;
				float lerpResult596 = lerp( ConcreteNewAlpha640 , DamageAlpha599 , PaintSelection313);
				float4 Albedo323 = ( lerpResult308 * lerpResult596 );
				
				
				float3 Albedo = Albedo323.rgb;
				float3 Emission = 0;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999

			#pragma enable_d3d11_debug_symbols
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_2D

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature _USECUSTOMCOLOR_ON


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RGBAMaskB_ST;
			float4 _TransitionNM_ST;
			float4 _ConcreteNM_ST;
			float4 _RGBAMaskA_ST;
			float _ConcreteDamageTiling;
			float _DirtSmoothness;
			float _MainSmoothness;
			float _ConcreteDamageScale;
			float _TransitionScale;
			float _DirtContrast;
			float _DirtSmooth;
			float _DirtRange;
			float _DirtOverlay;
			float _EdgesOverlay;
			float _ConcreteBareSmooth;
			float _BareSmoothness;
			float _ConcreteBareRange;
			float _TransitionAmount;
			float _DamageAmountSmooth;
			float _DamageMultiplier;
			float _DamageAmount;
			float _TransitionBrightness;
			float _BareBrightness;
			int _CustomColor;
			int _Color;
			float _DamageBrightness;
			float _PaintEdgesMultiply;
			float _DamageSmoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _DamageColor;
			sampler2D _RGBAMaskB;
			sampler2D _MainTex;
			sampler2D _RGBAMaskA;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord2.zw = v.ase_texcoord3.xy;
				o.ase_color = v.ase_color;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord3 = v.ase_texcoord3;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord3 = patch[0].ase_texcoord3 * bary.x + patch[1].ase_texcoord3 * bary.y + patch[2].ase_texcoord3 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 temp_cast_0 = (_ConcreteDamageTiling).xx;
				float2 uv0385 = IN.ase_texcoord2.xy * temp_cast_0 + float2( 0,0 );
				float2 ConcreteTileUV386 = uv0385;
				float4 tex2DNode185 = tex2D( _DamageColor, ConcreteTileUV386 );
				float2 uv_RGBAMaskB = float4(IN.ase_texcoord2.xy,0,0).xy * _RGBAMaskB_ST.xy + _RGBAMaskB_ST.zw;
				float4 tex2DNode636 = tex2D( _RGBAMaskB, uv_RGBAMaskB );
				float ConcreteNewAlpha640 = tex2DNode636.a;
				#ifdef _USECUSTOMCOLOR_ON
				float staticSwitch635 = (float)_CustomColor;
				#else
				float staticSwitch635 = (float)_Color;
				#endif
				float2 appendResult613 = (float2(( staticSwitch635 * 0.015625 ) , -0.33));
				float2 uv3611 = IN.ase_texcoord2.zw * float2( 1,1 ) + appendResult613;
				float4 BareConcrete622 = tex2D( _MainTex, uv3611 );
				float4 temp_output_559_0 = ( ConcreteNewAlpha640 * BareConcrete622 * _BareBrightness );
				float HeightMask58 = saturate(pow(((( 1.0 - tex2DNode636.r )*( _DamageAmount * ( ( IN.ase_color.g * _DamageMultiplier ) * tex2DNode636.g ) ))*4)+(( _DamageAmount * ( ( IN.ase_color.g * _DamageMultiplier ) * tex2DNode636.g ) )*2),_DamageAmountSmooth));
				float HeightBricks203 = HeightMask58;
				float BricksStep210 = step( 0.1 , ( ( 1.0 - HeightBricks203 ) * step( _TransitionAmount , HeightBricks203 ) ) );
				float4 lerpResult459 = lerp( ( tex2DNode185 * _DamageBrightness ) , ( ( temp_output_559_0 + tex2DNode185 ) * _TransitionBrightness * ConcreteNewAlpha640 ) , BricksStep210);
				float2 uv3612 = float4(IN.ase_texcoord2.zw,0,0).xy * float2( 1,1 ) + ( half2( 0.015625,0 ) * staticSwitch635 );
				float4 PaintColor619 = tex2D( _MainTex, uv3612 );
				float HeightGreen564 = tex2DNode636.b;
				float3 temp_cast_3 = (( 1.0 - HeightGreen564 )).xxx;
				float VertexBlue392 = IN.ase_color.b;
				float2 uv_RGBAMaskA = float4(IN.ase_texcoord2.xy,0,0).xy * _RGBAMaskA_ST.xy + _RGBAMaskA_ST.zw;
				float4 tex2DNode325 = tex2D( _RGBAMaskA, uv_RGBAMaskA );
				float DirtMaskBlue413 = tex2DNode325.b;
				float3 temp_cast_4 = (( 1.0 - ( ( 1.0 - VertexBlue392 ) * DirtMaskBlue413 ) )).xxx;
				float temp_output_426_0 = saturate( ( 1.0 - ( ( distance( temp_cast_3 , temp_cast_4 ) - _ConcreteBareRange ) / max( _ConcreteBareSmooth , 1E-05 ) ) ) );
				float PaintEdges465 = temp_output_426_0;
				float PaintEdgesSelection471 = step( 0.1 , ( ( 1.0 - PaintEdges465 ) * step( 0.0 , PaintEdges465 ) ) );
				float4 lerpResult397 = lerp( PaintColor619 , ( temp_output_559_0 * ( 1.0 - ( PaintEdgesSelection471 * _PaintEdgesMultiply ) ) ) , temp_output_426_0);
				float temp_output_228_0 = pow( tex2DNode325.r , _DirtOverlay );
				float PaintSelection313 = ( ( 1.0 - HeightBricks203 ) * ( 1.0 - BricksStep210 ) );
				float4 lerpResult117 = lerp( lerpResult459 , ( ( lerpResult397 + ( lerpResult397 * _EdgesOverlay * tex2DNode325.g ) ) * temp_output_228_0 ) , PaintSelection313);
				float2 appendResult628 = (float2(( staticSwitch635 * 0.015625 ) , -0.66));
				float2 uv3627 = float4(IN.ase_texcoord2.zw,0,0).xy * float2( 1,1 ) + appendResult628;
				float4 DirtColor630 = tex2D( _MainTex, uv3627 );
				float VertexRed336 = IN.ase_color.r;
				float3 temp_cast_5 = (( VertexRed336 * ( 1.0 - tex2DNode325.g ) )).xxx;
				float3 temp_cast_6 = (( 1.0 - HeightGreen564 )).xxx;
				float temp_output_309_0 = ( ( tex2DNode325.a * saturate( ( 1.0 - ( ( distance( temp_cast_5 , temp_cast_6 ) - _DirtRange ) / max( _DirtSmooth , 1E-05 ) ) ) ) ) * _DirtContrast );
				float4 lerpResult308 = lerp( lerpResult117 , ( DirtColor630 * ConcreteNewAlpha640 ) , temp_output_309_0);
				float DamageAlpha599 = tex2DNode185.a;
				float lerpResult596 = lerp( ConcreteNewAlpha640 , DamageAlpha599 , PaintSelection313);
				float4 Albedo323 = ( lerpResult308 * lerpResult596 );
				
				
				float3 Albedo = Albedo323.rgb;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				half4 color = half4( Albedo, Alpha );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				return color;
			}
			ENDHLSL
		}
		
	}
	/*ase_lod*/
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	
	
}
/*ASEBEGIN
Version=18200
1920;77;1680;999;-2596.944;770.6134;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;51;-1064.214,719.5595;Inherit;False;2780.445;600.6805;;10;50;53;54;56;336;58;57;392;638;639;Heightmap;1,0.5185516,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;343;-844.5211,-2925.485;Inherit;False;5315.048;1831.45;;68;308;117;193;241;314;194;459;235;375;253;462;351;238;366;397;228;237;185;387;229;453;309;496;345;297;455;198;497;559;456;498;197;382;533;192;190;532;438;473;337;542;556;555;413;325;326;461;566;558;463;573;560;581;576;584;595;596;599;600;602;620;623;631;583;632;642;643;645;Albedo;0.5774003,0.9528302,0.3460751,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;326;-371.3883,-2092.395;Inherit;True;Property;_RGBAMaskA;RGBA Mask A;21;0;Create;True;0;0;False;0;False;None;f6bcb3be2ffc7eb439937251e6b1e988;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.VertexColorNode;50;224.1169,940.6334;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;450;-2985.556,-2885.635;Inherit;False;2005.231;1187.968;;18;471;469;468;467;470;465;452;561;426;425;524;424;432;444;446;525;427;565;Paint Selection;0.7075472,0.3504361,0.3504361,1;0;0
Node;AmplifyShaderEditor.SamplerNode;325;16.00373,-2092.268;Inherit;True;Property;_PlasterDirtMasksRGBA;PlasterDirtMasksRGBA;18;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;392;536.7787,1179.014;Inherit;False;VertexBlue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;427;-2906.865,-2630.066;Inherit;False;392;VertexBlue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;413;500.1527,-1884.864;Inherit;False;DirtMaskBlue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;636;-310.4765,737.7122;Inherit;True;Property;_RGBAMaskB;RGBA Mask B;22;0;Create;True;0;0;False;0;False;-1;None;0cc5a2d9845f1c243a311cd5fe2d190e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;446;-2902.865,-2533.066;Inherit;False;413;DirtMaskBlue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;564;22.88916,840.8734;Inherit;False;HeightGreen;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;525;-2684.865,-2628.066;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;444;-2486.99,-2623.873;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;565;-2626.775,-2776.177;Inherit;False;564;HeightGreen;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;625;-1425.272,-4039.146;Inherit;False;2568.047;856.42;Comment;20;630;626;627;628;629;619;614;612;609;622;610;618;611;608;613;616;624;635;633;621;UV3 Selection;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;432;-2337.99,-2521.934;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;424;-2735.879,-2396.239;Inherit;False;Property;_ConcreteBareRange;Concrete Bare Range;13;0;Create;True;0;0;False;0;False;0;0.65;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;425;-2731.513,-2316.262;Inherit;False;Property;_ConcreteBareSmooth;Concrete Bare Smooth;14;0;Create;True;0;0;False;0;False;0;0.267;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;524;-2304.521,-2702.86;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;633;-1384.895,-3752.026;Inherit;False;Property;_CustomColor;Custom Color;32;0;Create;True;0;0;False;0;False;0;38;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;621;-1358.73,-3868.998;Inherit;False;Property;_Color;Color;0;1;[PerRendererData];Create;True;0;0;True;0;False;0;2;0;1;INT;0
Node;AmplifyShaderEditor.FunctionNode;426;-2114.349,-2513.202;Inherit;False;Color Mask;-1;;40;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.17;False;5;FLOAT;0.37;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;635;-1100.895,-3764.026;Inherit;False;Property;_UseCustomColor;Use Custom Color;31;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;639;499.3073,1014.933;Float;False;Property;_DamageMultiplier;Damage Multiplier;7;0;Create;True;0;0;False;0;False;0.1946161;0.7882353;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;624;-748.9025,-3489.662;Inherit;False;Constant;_3Row;3 Row;33;0;Create;True;0;0;False;0;False;0.015625;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;646;838.7871,848.5275;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;616;-435.9728,-3619.55;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;638;776.1578,942.7782;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;465;-1894.736,-2155.875;Inherit;False;PaintEdges;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;467;-1669.907,-2147.911;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;470;-1636.256,-1901.299;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;867.0561,1093.596;Float;False;Property;_DamageAmount;Damage Amount;5;0;Create;True;0;0;False;0;False;0.6;0.9176471;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;998.6397,939.9022;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;613;-273.7428,-3498.714;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;-0.33;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;637;778.4453,769.0186;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;611;-20.92506,-3535.662;Inherit;False;3;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;1232.292,932.6437;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;980.3121,807.3334;Float;False;Property;_DamageAmountSmooth;Damage Amount Smooth;4;0;Create;True;0;0;False;0;False;61.17647;14.4;0;17;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;608;-98.76552,-3807.893;Inherit;True;Property;_MainTex;Color Theme;1;0;Create;False;0;0;False;0;False;None;bf84d265e341525459ab1fd8a003c547;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;468;-1499.428,-2157.3;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HeightMapBlendNode;58;1428.719,760.1623;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;469;-1349.85,-2164.142;Inherit;False;2;0;FLOAT;0.1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;610;-708.1978,-3979.334;Half;False;Constant;_Vector1;Vector 1;19;0;Create;True;0;0;False;1;;False;0.015625,0;0.125,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;618;387.3604,-3660.392;Inherit;True;Property;_TextureSample0;Texture Sample 0;26;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;202;1897.457,743.8547;Inherit;False;1954.646;466.8429;Plaster Step Selection;11;313;210;311;312;208;310;207;205;217;204;203;;0.4111784,0.7731025,0.8301887,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;640;-24.75436,937.2447;Inherit;False;ConcreteNewAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;622;830.5748,-3656.555;Inherit;False;BareConcrete;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;204;1946.457,1067.808;Inherit;False;Property;_TransitionAmount;Transition Amount;20;0;Create;True;0;0;False;0;False;0;0.002035487;0;0.05;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;471;-1227.576,-2157.067;Inherit;False;PaintEdgesSelection;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;203;1944.796,798.5302;Inherit;False;HeightBricks;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;322;-834.6306,-118.503;Inherit;False;2032.363;606.4921;;13;388;385;211;390;386;320;68;69;1;318;5;10;384;Normals;0.374466,0.3976796,0.8018868,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;609;-340.6774,-3973.446;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;576;-419.2856,-2893.895;Inherit;False;640;ConcreteNewAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;205;2244.29,808.0647;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;217;2286.682,1062.168;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;642;-400.785,-2740.774;Inherit;False;Property;_BareBrightness;Bare Brightness;33;0;Create;True;0;0;False;0;False;0;0.9882354;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;623;-393.0129,-2807.152;Inherit;False;622;BareConcrete;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;438;-681.5153,-2483.459;Inherit;False;Property;_PaintEdgesMultiply;Paint Edges Multiply;11;0;Create;True;0;0;False;0;False;1;0.4832942;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;452;-1749.414,-2659.501;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;473;-686.924,-2585.307;Inherit;False;471;PaintEdgesSelection;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;388;-781.1663,366.371;Inherit;False;Property;_ConcreteDamageTiling;Concrete Damage Tiling;8;0;Create;True;0;0;False;0;False;0;2;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;612;-22.03163,-3989.146;Inherit;False;3;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.09,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;614;375.3826,-3858.775;Inherit;True;Property;_TextureSample2;Texture Sample 2;25;0;Create;True;0;0;False;0;False;-1;None;None;True;2;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;207;2467.219,813.6757;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;559;-104.1947,-2840.807;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;385;-418.7953,360.3796;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;561;-1690.95,-2692.242;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;498;-353.4064,-2585.442;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;629;-470.3221,-3302.495;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;310;2444.201,1028.932;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;555;435.0137,-1414.073;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;208;2652.036,805.0824;Inherit;False;2;0;FLOAT;0.1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;386;-72.57118,259.6019;Inherit;False;ConcreteTileUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;455;220.5075,-2649.866;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;619;799.4938,-3838.051;Inherit;False;PaintColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;558;145.715,-2893.092;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;497;-171.6965,-2579.778;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;336;526.0731,1098.703;Inherit;False;VertexRed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;556;461.9948,-1301.102;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;311;3051.162,1003.929;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;628;-248.9602,-3309.63;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;-0.66;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;581;241.5017,-2907.721;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;453;279.455,-2588.108;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;496;159.205,-2743.118;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;620;65.21793,-2403.951;Inherit;False;619;PaintColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;387;525.9183,-2630.249;Inherit;False;386;ConcreteTileUV;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;312;3153.359,783.3036;Inherit;False;443;336;Paint Only Selection;3;248;247;249;;0.6886792,0.1396849,0.1396849,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;345;418.8734,-2877.006;Inherit;True;Property;_DamageColor;Damage Color;3;0;Create;True;0;0;False;0;False;None;3920578cb267cd947bed7ce40996407b;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;210;2822.709,801.2153;Inherit;False;BricksStep;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;185;788.7803,-2873.829;Inherit;True;Property;_TextureSample3;Texture Sample 3;7;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;397;361.1146,-2428.254;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;248;3195.877,850.8003;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;542;2097.979,-1328.817;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;573;1467.588,-2902.599;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;566;2280.302,-1678.833;Inherit;False;564;HeightGreen;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;337;2091.108,-1423.223;Inherit;False;336;VertexRed;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;249;3189.187,1015.923;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;627;-9.043535,-3338.578;Inherit;False;3;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;237;93.86633,-2242.06;Inherit;False;Property;_EdgesOverlay;Edges Overlay;15;0;Create;True;0;0;False;0;False;0;0.408;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;626;383.7579,-3447.177;Inherit;True;Property;_TextureSample4;Texture Sample 4;26;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;533;2490.914,-1390.691;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;190;2361.993,-1509.822;Inherit;False;Property;_DirtSmooth;Dirt Smooth;26;0;Create;True;0;0;False;1;MaxGay;False;0;0.6242941;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;532;2630.837,-1673.9;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;192;2355.597,-1586.802;Inherit;False;Property;_DirtRange;Dirt Range;25;0;Create;True;0;0;False;0;False;0;0.5948824;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;229;518.7689,-1989.24;Inherit;False;Property;_DirtOverlay;Dirt Overlay;16;0;Create;True;0;0;False;0;False;0;0.358;0.02;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;463;1381.912,-2626.238;Inherit;False;Property;_DamageBrightness;Damage Brightness;6;0;Create;True;0;0;False;0;False;0;1.959415;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;382;386.283,-1738.186;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;584;1504.524,-2786.599;Inherit;False;Property;_TransitionBrightness;Transition Brightness;18;0;Create;True;0;0;False;0;False;0;1.447059;0.5;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;461;1561.276,-2917.22;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;247;3419.877,833.3038;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;238;547.7704,-2267.425;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;641;1576.457,-3021.11;Inherit;False;640;ConcreteNewAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;197;2848.624,-1624.246;Inherit;False;Color Mask;-1;;41;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.17;False;5;FLOAT;0.37;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;228;808.4739,-2066.395;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;462;1674.31,-2672.868;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;456;526.0421,-1706.674;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;583;1878.691,-2849.085;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;253;1916.575,-2534.884;Inherit;False;210;BricksStep;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;313;3636.438,830.4044;Inherit;False;PaintSelection;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;235;768.0305,-2426.405;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;630;864.9965,-3412.91;Inherit;False;DirtColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;241;1000.254,-2384.722;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;599;1128.825,-2637.655;Inherit;False;DamageAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;3196.941,-1742.624;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;631;2976.117,-2285.534;Inherit;False;630;DirtColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;297;3185.221,-1576.04;Inherit;False;Property;_DirtContrast;Dirt Contrast;24;0;Create;True;0;0;False;0;False;0;1.036299;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;314;2132.273,-2199.164;Inherit;False;313;PaintSelection;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;645;2986.735,-2111.857;Inherit;False;640;ConcreteNewAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;459;2193.608,-2750.884;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;3425.458,-2312.99;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;600;3772.426,-2625.251;Inherit;False;599;DamageAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;643;3764.541,-2861.783;Inherit;False;640;ConcreteNewAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;117;2448.875,-2399.208;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;602;3768.583,-2526.466;Inherit;False;313;PaintSelection;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;309;3520.83,-2042.956;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;308;3718.466,-2385.212;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;596;4106.605,-2645.598;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;595;4291.26,-2367.991;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;379;-828.7544,-944.3882;Inherit;False;1810.135;721.3423;Comment;19;551;552;359;349;360;368;378;376;357;358;373;374;550;362;370;356;371;369;549;Smoothness;0.1273585,0.9137321,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;354;3094.271,-384.3109;Inherit;False;187;120;Albedo;1;324;;0.9150943,0.7239427,0.0302154,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;323;4541.163,-2375.295;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;380;3084.404,-107.9644;Inherit;False;197;118;Smoothness;1;352;;0,0.9377248,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;355;3087.642,-239.3842;Inherit;False;186;119;Normals;1;321;;0.1405495,0.07742971,0.5660378,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;369;-672.4628,-671.7433;Inherit;False;366;DirtSmothness;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;549;-785.2884,-447.7238;Inherit;False;465;PaintEdges;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;351;1025.042,-2063.2;Inherit;False;PaintSmoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;371;-749.4629,-591.7435;Inherit;False;Property;_DirtSmoothness;Dirt Smoothness;28;0;Create;True;0;0;False;0;False;0;1.071279;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;356;-533.1183,-896.3882;Inherit;False;351;PaintSmoothness;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;378;211.1526,-559.79;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;370;-405.4632,-631.7433;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;384;-789.8114,187.6435;Inherit;False;Property;_TransitionScale;Transition Scale;19;0;Create;True;0;0;False;0;False;0;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;362;-528.7033,-807.2021;Inherit;False;Property;_MainSmoothness;Main Smoothness;27;0;Create;True;0;0;False;0;False;0;0.426;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-464.3829,-78.50299;Inherit;True;Property;_ConcreteNM;Concrete NM;10;0;Create;True;0;0;False;0;False;-1;None;e2f2024fc17d02a4cae64316bfc62502;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;373;-230.4632,-698.7433;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;390;-43.17294,345.2984;Inherit;False;Property;_ConcreteDamageScale;Concrete Damage Scale;9;0;Create;True;0;0;False;0;False;0;1.297345;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-474.2265,120.6707;Inherit;True;Property;_TransitionNM;Transition NM;17;0;Create;True;0;0;False;0;False;-1;None;4ea5083d09e1aa747b5ad9a32ad2cb74;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;550;-341.3781,-518.4382;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;358;84.65071,-309.4792;Inherit;False;313;PaintSelection;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;357;-103.8612,-495.7261;Inherit;False;Property;_DamageSmoothness;Damage Smoothness;29;0;Create;True;0;0;False;0;False;0;0.515;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;376;-89.8474,-421.7902;Inherit;False;375;ConcreteDamage;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;374;-167.6093,-855.8204;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;552;-820.3799,-337.2308;Inherit;False;Property;_BareSmoothness;Bare Smoothness;30;0;Create;True;0;0;False;0;False;0;0.6447374;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;211;-89.08717,116.7589;Inherit;False;210;BricksStep;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;318;524.5555,310.242;Inherit;False;203;HeightBricks;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;360;313.8417,-332.3753;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;69;347.6044,96.60011;Inherit;True;Property;_DamageNM;Damage NM;2;0;Create;True;0;0;False;0;False;-1;None;381eef27937e5464e93f48a9a73d286d;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;194;2643.268,-2150.049;Inherit;False;Property;_DirtColor;Dirt Color;23;0;Create;True;0;0;False;0;False;0,0,0,0;0.545,0.5013297,0.432185,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;368;26.53687,-792.7433;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;68;703.727,-65.83978;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;359;738.3801,-618.6752;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;366;3819.677,-2053.669;Inherit;False;DirtSmothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;324;3110.271,-343.311;Inherit;False;323;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;352;3093.404,-68.96437;Inherit;False;359;Smoothness;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;560;-694.3607,-2877.616;Inherit;False;Property;_ConcreteBare;Concrete Bare;12;0;Create;True;0;0;False;0;False;0.9339623,0.8198519,0.2687344,0;0.874,0.8665618,0.825651,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;320;985.6238,-73.41331;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;632;3246.348,-1988.04;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;1;64.80053,-47.85182;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;321;3100.642,-200.3842;Inherit;False;320;Normals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;375;1138.823,-2735.265;Inherit;False;ConcreteDamage;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;349;422.4991,-630.3298;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;551;-501.3771,-391.3548;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;662;3389.253,-214.7079;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;661;3389.253,-214.7079;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;663;3389.253,-214.7079;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;664;3389.253,-214.7079;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;659;3389.253,-214.7079;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;660;3409.253,-226.7079;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;DBK LWRP/Concrete;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;16;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;;0;0;Standard;33;Workflow;1;Surface;0;  Refraction Model;0;  Blend;0;Two Sided;1;Transmission;0;  Transmission Shadow;0.5,False,-1;Translucency;0;  Translucency Strength;1,False,-1;  Normal Distortion;0.5,False,-1;  Scattering;2,False,-1;  Direct;0.9,False,-1;  Ambient;0.1,False,-1;  Shadow;0.5,False,-1;Cast Shadows;1;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;1;Built-in Fog;1;Meta Pass;1;Override Baked GI;0;Extra Pre Pass;0;DOTS Instancing;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Vertex Position,InvertActionOnDeselection;1;0;6;False;True;True;True;True;True;False;;0
WireConnection;325;0;326;0
WireConnection;392;0;50;3
WireConnection;413;0;325;3
WireConnection;564;0;636;3
WireConnection;525;0;427;0
WireConnection;444;0;525;0
WireConnection;444;1;446;0
WireConnection;432;0;444;0
WireConnection;524;0;565;0
WireConnection;426;1;432;0
WireConnection;426;3;524;0
WireConnection;426;4;424;0
WireConnection;426;5;425;0
WireConnection;635;1;621;0
WireConnection;635;0;633;0
WireConnection;646;0;636;2
WireConnection;616;0;635;0
WireConnection;616;1;624;0
WireConnection;638;0;50;2
WireConnection;638;1;639;0
WireConnection;465;0;426;0
WireConnection;467;0;465;0
WireConnection;470;1;465;0
WireConnection;54;0;638;0
WireConnection;54;1;646;0
WireConnection;613;0;616;0
WireConnection;637;0;636;1
WireConnection;611;1;613;0
WireConnection;56;0;53;0
WireConnection;56;1;54;0
WireConnection;468;0;467;0
WireConnection;468;1;470;0
WireConnection;58;0;637;0
WireConnection;58;1;56;0
WireConnection;58;2;57;0
WireConnection;469;1;468;0
WireConnection;618;0;608;0
WireConnection;618;1;611;0
WireConnection;640;0;636;4
WireConnection;622;0;618;0
WireConnection;471;0;469;0
WireConnection;203;0;58;0
WireConnection;609;0;610;0
WireConnection;609;1;635;0
WireConnection;205;0;203;0
WireConnection;217;0;204;0
WireConnection;217;1;203;0
WireConnection;452;0;426;0
WireConnection;612;1;609;0
WireConnection;614;0;608;0
WireConnection;614;1;612;0
WireConnection;207;0;205;0
WireConnection;207;1;217;0
WireConnection;559;0;576;0
WireConnection;559;1;623;0
WireConnection;559;2;642;0
WireConnection;385;0;388;0
WireConnection;561;0;452;0
WireConnection;498;0;473;0
WireConnection;498;1;438;0
WireConnection;629;0;635;0
WireConnection;629;1;624;0
WireConnection;310;0;203;0
WireConnection;555;0;325;2
WireConnection;208;1;207;0
WireConnection;386;0;385;0
WireConnection;455;0;561;0
WireConnection;619;0;614;0
WireConnection;558;0;559;0
WireConnection;497;0;498;0
WireConnection;336;0;50;1
WireConnection;556;0;555;0
WireConnection;311;0;310;0
WireConnection;628;0;629;0
WireConnection;581;0;558;0
WireConnection;453;0;455;0
WireConnection;496;0;559;0
WireConnection;496;1;497;0
WireConnection;210;0;208;0
WireConnection;185;0;345;0
WireConnection;185;1;387;0
WireConnection;397;0;620;0
WireConnection;397;1;496;0
WireConnection;397;2;453;0
WireConnection;248;0;311;0
WireConnection;542;0;556;0
WireConnection;573;0;581;0
WireConnection;249;0;210;0
WireConnection;627;1;628;0
WireConnection;626;0;608;0
WireConnection;626;1;627;0
WireConnection;533;0;337;0
WireConnection;533;1;542;0
WireConnection;532;0;566;0
WireConnection;382;0;325;4
WireConnection;461;0;573;0
WireConnection;461;1;185;0
WireConnection;247;0;248;0
WireConnection;247;1;249;0
WireConnection;238;0;397;0
WireConnection;238;1;237;0
WireConnection;238;2;325;2
WireConnection;197;1;532;0
WireConnection;197;3;533;0
WireConnection;197;4;192;0
WireConnection;197;5;190;0
WireConnection;228;0;325;1
WireConnection;228;1;229;0
WireConnection;462;0;185;0
WireConnection;462;1;463;0
WireConnection;456;0;382;0
WireConnection;583;0;461;0
WireConnection;583;1;584;0
WireConnection;583;2;641;0
WireConnection;313;0;247;0
WireConnection;235;0;397;0
WireConnection;235;1;238;0
WireConnection;630;0;626;0
WireConnection;241;0;235;0
WireConnection;241;1;228;0
WireConnection;599;0;185;4
WireConnection;198;0;456;0
WireConnection;198;1;197;0
WireConnection;459;0;462;0
WireConnection;459;1;583;0
WireConnection;459;2;253;0
WireConnection;193;0;631;0
WireConnection;193;1;645;0
WireConnection;117;0;459;0
WireConnection;117;1;241;0
WireConnection;117;2;314;0
WireConnection;309;0;198;0
WireConnection;309;1;297;0
WireConnection;308;0;117;0
WireConnection;308;1;193;0
WireConnection;308;2;309;0
WireConnection;596;0;643;0
WireConnection;596;1;600;0
WireConnection;596;2;602;0
WireConnection;595;0;308;0
WireConnection;595;1;596;0
WireConnection;323;0;595;0
WireConnection;351;0;228;0
WireConnection;378;0;376;0
WireConnection;378;1;357;0
WireConnection;370;0;369;0
WireConnection;370;1;371;0
WireConnection;373;0;370;0
WireConnection;10;5;384;0
WireConnection;550;0;551;0
WireConnection;374;0;356;0
WireConnection;374;1;362;0
WireConnection;360;0;358;0
WireConnection;69;1;386;0
WireConnection;69;5;390;0
WireConnection;368;0;374;0
WireConnection;368;1;373;0
WireConnection;368;2;550;0
WireConnection;68;0;1;0
WireConnection;68;1;69;0
WireConnection;68;2;318;0
WireConnection;359;0;349;0
WireConnection;366;0;309;0
WireConnection;320;0;68;0
WireConnection;1;0;5;0
WireConnection;1;1;10;0
WireConnection;1;2;211;0
WireConnection;375;0;185;1
WireConnection;349;0;368;0
WireConnection;349;1;378;0
WireConnection;349;2;360;0
WireConnection;551;0;549;0
WireConnection;551;1;552;0
WireConnection;660;0;324;0
WireConnection;660;1;321;0
WireConnection;660;4;352;0
ASEEND*/
//CHKSM=4FE32331F4784A5EF1DA0BCEE5ED09B7CE0DF284