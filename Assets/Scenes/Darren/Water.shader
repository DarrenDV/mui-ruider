Shader "Unlit/Water"
{
    Properties
    {
        [Header(Texture Options)]
        [Space(5)]
        _MainTex("Texture", 2D) = "white" {}
        _TexScrollSpeed("TexScrollSpeed", Range(0.0, 0.5)) = 1
        [Space(5)]
        _HeightMap("HeightMap", 2D) = "" {}
        _HeightMapScale("Height", Range(-10.0, 10.0)) = 1
        [Space(5)]
        _NormalMap("NormalMap", 2D) = "" {}
        _NormalMapStrength("NormalMapStrength", Range(-3.0, 3.0)) = 1

        [Header(Color options)]
        [Space(5)]
        _FoamColor("FoamColor", Color) = (.25, .5, .5, 1)
        _WaterColor("WaterColor", Color) = (0,0,0,0)
        _WaterAlpha("WaterAlpha", Range(0.0, 1.0)) = 1
        [Space(20)]
        
        [Header(Lighting Options)]
        [Space(5)]
        _DiffuseColor("Diffuse Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SpecularColor("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Shininess("Shininess", Float) = 10
        [Space(20)]
        
        [Header(Wave Options)]
        [Space(5)]
        _Amplitude("Amplitude", Float) = 1
        _Wavelength("Wavelength", Float) = 10
        _WaveSpeed("Speed", Float) = 1
    }
    SubShader
    {
        Tags { "WaterMode" = "Refractive" "Queue" = "Transparent" "RenderType"="Transparant" "LightMode" = "ForwardBase" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 color : COLOR0;
                float3 tangentSpaceLight : TEXCOORD1;
            };

            //Texture visual variables
            sampler2D _MainTex, _HeightMap, _NormalMap;
            float4 _MainTex_ST;
            float4 _FoamColor, _WaterColor;
            float _WaterAlpha, _TexScrollSpeed, _NormalMapStrength;
            half _HeightMapScale;

            //Vertex manipulation variables
            float _Amplitude, _Wavelength, _WaveSpeed;

            //Gouraud variables
            uniform float4 _LightColor0;
            uniform	float4 _DiffuseColor;
            uniform	float4 _SpecularColor;
            uniform	float _Shininess;

            v2f vert (appdata v)
            {
                v2f o;
                
                //Calculate waves
                float3 p = v.vertex.xyz;
                float k = 2 * UNITY_PI / _Wavelength;
                p.y = _Amplitude * sin(k * (p.x - _WaveSpeed * _Time.y));
                v.vertex.xyz = p;

                float2 uv = v.uv.xy + _Time.y * _TexScrollSpeed;
                float displacement = tex2Dlod(_HeightMap, float4(uv, 0, 0)).r;
                v.vertex.y += displacement * (_HeightMapScale * -1);


                //Normal light calculation
                float3 normal = UnityObjectToWorldNormal(v.normal);
                float3 tangent = UnityObjectToWorldNormal(v.tangent);
                float3 bitangent = cross(tangent, normal);

                o.tangentSpaceLight = float3(dot(tangent, _WorldSpaceLightPos0), dot(bitangent, _WorldSpaceLightPos0), dot(normal, _WorldSpaceLightPos0));



                //Lighting a la Gouraud
                float3 normalDir = normalize(mul(unity_ObjectToWorld, v.normal));//normal direction in world space for light calc
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);//Pos0 gives the direction vector of the first directional light in the scene
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex));//camera direction relative to the vertex

                // diffuse
                float NdotL = max(0.0, dot(normalDir, lightDir));
                float3 diffuse = _DiffuseColor * _LightColor0.rgb * NdotL;

                // reflection of LightDir by the normal
                float3 reflectedDir = reflect(-lightDir, normalDir);
                float rv = max(0.0, dot(reflectedDir, viewDir));

                float specularAmount = pow(rv, _Shininess);

                float3 specularLight = _SpecularColor.rgb * _LightColor0.rgb * specularAmount;

                o.color = float4(diffuse + specularLight, 1);





                o.vertex = UnityObjectToClipPos(v.vertex);


                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                
                //sample the texture
                float2 uv = i.uv + _Time.y * _TexScrollSpeed; //Scrolling effect so it looks more water-y
                fixed4 main = tex2D(_MainTex, uv);
                
                //Calculate water colors
                float4 foamColor = (1 - main) * _FoamColor;
                float4 waterColor = main * _WaterColor;
                float4 finalColor = foamColor + waterColor;

                //Calculate water alpha for seethrough-ness
                float waterAlpha = 0;
                if (waterColor.r > 0 || waterColor.b > 0 || waterColor.g > 0) { //Making sure the foam alpha is visible at every color
                    waterAlpha = 1;
                }

                float alpha = 1 - _WaterAlpha; //Inverse the alpha value so the inspector is less confusing
                float finalAlpha = 1 - (waterAlpha * alpha);

                //Calculating color together with gouraud specular
                float4 colorOutput = float4(finalColor.r, finalColor.g, finalColor.b, finalAlpha) * i.color;


                //Normal calculation
                float3 tangentNormal = tex2D(_NormalMap, uv) * 2 * _NormalMapStrength;
                float dotNormal = dot(tangentNormal, i.tangentSpaceLight) * 0.5f;
                float4 normalOutput = (1 - dotNormal) ;

               
                //Calculating final output (color + normals)
                //We multiply each color individually because we dont want the alpha to be effected
                float4 output = float4(colorOutput.r * normalOutput.r, colorOutput.g * normalOutput.g, colorOutput.b * normalOutput.b, colorOutput.a) 


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, waterColor);
                return output;
                //return colorOutput;
            }
            ENDCG
        }
    }
}