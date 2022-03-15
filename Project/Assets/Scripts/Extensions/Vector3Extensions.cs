using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
	// Y 축을 기준으로 하는 각도를 반환합니다.
	public static float ToAngle(this Vector3 dirVector, bool doNormalize = false)
	{
		if (doNormalize) dirVector.Normalize();
		return Mathf.Atan2(dirVector.x, dirVector.z) * Mathf.Rad2Deg;
	}

	// from 에서 to 로의 방향을 반환합니다.
	public static Vector3 Direction(this Vector3 from, Vector3 to)
	{
		return (to - from).normalized;

	}

	// 접점벡터를 반환합니다.(reverse 가 true 일시 반대 방향(왼쪽)을 반환)
	public static Vector3 Tangent(this Vector3 standardVector, float distant, float radius, bool reverse = false)
    {
		float cos = radius / distant;
		float sin = Mathf.Sqrt(1.0f - cos * cos);

		if(reverse)
			return new Vector3(-1 * radius * cos, 0.0f, -1 * radius * sin + distant);
		else
			return new Vector3(radius * cos, 0.0f, -1 * radius * sin + distant);

	}
}
