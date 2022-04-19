using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveStage : Stage
{
    /// 랜덤 동굴 선택하기 게임
    [Header("정답 동굴 순서")]
    [SerializeField] private List<Transform> _RightCaveRoad;

    [Header("동굴 문")]
    [SerializeField] private List<Collider> _DoorCollider;

    [Header("목표 지점")]
    [SerializeField] private Transform _Destination;

    // 현재 정답 동굴 길의 번호
    private int rightCaveRoadNum;

    /// 음 맞추기 게임
    [Header("퀴즈1")]
    [SerializeField] private List<int> _Qiz1;

    [Header("퀴즈2")]
    [SerializeField] private List<int> _Qiz2;

    [Header("퀴즈3")]
    [SerializeField] private List<int> _Qiz3;

    private void Start()
    {
        rightCaveRoadNum = 0;
        MoveToRightCaveRoad();
    }

    // 현재 정답 동굴 길의 번호로 목표지점을 옮기고 문을 엽니다.
    private void MoveToRightCaveRoad()
    {
        _Destination.SetParent(_RightCaveRoad[rightCaveRoadNum]);
        _Destination.localPosition = Vector3.zero;

        // 문을 엽니다.
        _DoorCollider[rightCaveRoadNum].enabled = false;
    }

    // 낙하하면 문의 위치를 바꿉니다.
    public void ChangeCaveRoad()
    {
        // 바꾸기 전 문을 닫습니다.
        _DoorCollider[rightCaveRoadNum].enabled = true;

        rightCaveRoadNum++;

        if (rightCaveRoadNum > 1) rightCaveRoadNum = 0;

        MoveToRightCaveRoad();
    }

    public override void FinishStage()
    {
        throw new System.NotImplementedException();
    }
}
