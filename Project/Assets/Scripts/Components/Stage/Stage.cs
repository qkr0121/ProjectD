using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 stage 클래스에 들어가는 추상클래스
// 자식으로 SavePoint를 가집니다.
public abstract class Stage : MonoBehaviour
{
    
    // stage 가 시작할 때 실행합니다. (UI 로 스테이지 간단하게 설명)
    public void StartStage()
    {
        Debug.Log("start");
    }

    // stage 가 끝났을 때 실행합니다.
    public abstract void FinishStage();

}
