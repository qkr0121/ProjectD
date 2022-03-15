using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatics
{
	// 캔버스 원본 사이즈에 대한 읽기 전용 프로퍼티
	public static (float width, float heigth) screenSize => (1600.0f, 900.0f);

	// 화면 비율에 대한 읽기 전용 프로퍼티
	public static float screenRatio => Screen.width / screenSize.width;
	
}
