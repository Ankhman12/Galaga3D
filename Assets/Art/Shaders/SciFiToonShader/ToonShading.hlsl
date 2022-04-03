void ToonShading_float(in float3 Normal, in float ToonRampSmoothness, in float3 ClipSpacePos, in float3 WorldPos, in float ToonRampOffset, 
	 out float ToonRampUCoord, out float3 Direction, out float3 Color) {

	#ifdef SHADERGRAPH_PREVIEW
		ToonRampUCoord = 0.5;
		Direction = float3(0.5, 0.5, 0);
		Color = float4(1, 1, 1, 1);
	#else
		#if SHADOWS_SCREEN
			half4 shadowCoord = ComputeScreenPos(ClipSpacePos);
		#else
			half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
		#endif

		#if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
			Light light = GetMainLight(shadowCoord);
		#else
			Light light = GetMainLight();
		#endif

		half d = dot(Normal, light.direction) * 0.5 + 0.5;
		d = saturate(d);
		half toonRampUCoord = smoothstep(ToonRampOffset, ToonRampOffset + ToonRampSmoothness, d);
		toonRampUCoord *= light.shadowAttenuation * light.distanceAttenuation;
		ToonRampUCoord = toonRampUCoord//light.color * (toonRamp + ToonRampTinting).xyz;
		Direction = light.direction;
		Color = light.color;
	#endif

}