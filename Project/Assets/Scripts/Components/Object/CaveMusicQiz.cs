using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMusicQiz : MonoBehaviour
{
    [Header("문")]
    [SerializeField] private Transform _Door;

    [Header("문이 열리는 속도")]
    [SerializeField] private float _DoorOpenSpeed;

    [Header("정답 알림 구")]
    [SerializeField] private List<Renderer> _AnswerSphere;

    [Header("카메라 위치")]
    [SerializeField] private Transform _CameraPos;

    [Header("CaveStage")]
    [SerializeField] private CaveStage _CaveStage;

    // 플레이어 컴포넌트
    private PlayerCharacter _PlayerCharacter;

    // 현재 진행되고 있는 퀴즈
    private List<int> _PresentQiz;

    // 퀴즈 진행 순서
    private int qizOrder;

    // 퀴즈 번호
    private int qizNum;

    // 현재 문의 Y 좌표
    private float _PresentDoorPosY;

    // 색
    private float _IncColor;

    public int QizNum => qizNum;

    private void Start()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
        _PresentQiz = _CaveStage.qiz1;
        qizOrder = 0;
        qizNum = 1;
        _IncColor = 0;

        _PresentDoorPosY = _Door.localPosition.y;
    }

    // 퀴즈 번호를 변경합니다.
    private void ChangeQiz()
    {
        if (qizNum == 1) _PresentQiz = _CaveStage.qiz1;
        else if(qizNum == 2) _PresentQiz = _CaveStage.qiz2; 
        else if (qizNum == 3) _PresentQiz = _CaveStage.qiz3;
    }

    // 퀴즈에 정답을 입력합니다.
    public void TryQiz(int answer, SimpleSonarShader_Object _SimpleSonarShader_Object)
    {
        // 입력한 값과 정답이 일치하면
        if (_PresentQiz[qizOrder] == answer)
        {
            _SimpleSonarShader_Object.CorrectAnswer();
            qizOrder++;
            // 해당 퀴즈의 마지막까지 맞추었다면 다음 문제로 넘어갑니다.
            if (qizOrder == _PresentQiz.Count)
            {
                StartCoroutine(CorrectAnswer());
            }

        }
        // 정답이 아니면
        else
        {
            _SimpleSonarShader_Object.WrongAnswer();
        }
    }


    // 모든 문제를 맞추었을시 문이 열립니다.
    IEnumerator OpenDoor()
    {  
        _Door.localPosition += Vector3.up * Time.deltaTime * _DoorOpenSpeed;

        yield return null;

        if (_Door.localPosition.y - _PresentDoorPosY <= 10.0f)
            StartCoroutine(OpenDoor());
        else
        {
            StopCoroutine(OpenDoor());

            _PlayerCharacter.playerMovement.AllowMove();
            _PlayerCharacter.trackingCamera.InitCameraPos();
        }
    }

    // 정답을 입력했을시 정답 알림 구에 빛이 나게 합니다.
    IEnumerator LightAnswerSphere(int qizNum)
    {
        _IncColor += 1;
        _AnswerSphere[qizNum - 1].material.color = new Color(_IncColor/255f, 0, _IncColor/255f);

        yield return new WaitForSeconds(0.02f);

        // 구가 다켜지면 다음으로 넘어갑니다.
        if (_AnswerSphere[qizNum - 1].material.color.r > 1)
        {
            _IncColor = 0;

            // 마지막 퀴즈까지 맞추었다면 문이 열립니다.
            if (qizNum == 3)
            {
                StartCoroutine(OpenDoor());
            }
            else
            {
                _PlayerCharacter.playerMovement.AllowMove();
                _PlayerCharacter.trackingCamera.InitCameraPos();
            }
        }
        else
            StartCoroutine(LightAnswerSphere(qizNum));
    }

    // 문제를 맞추었을시 실행할 이벤트
    IEnumerator CorrectAnswer()
    {
        yield return new WaitForSeconds(2.0f);

        _PlayerCharacter.playerMovement.StopMove();
        // 카메라의 위치 문을 바라 볼수 있게 변경합니다.
        _PlayerCharacter.trackingCamera.MoveToViewPosition(_CameraPos);

        // 정답 구에 빛이 들어옵니다.
        StartCoroutine(LightAnswerSphere(qizNum));

        // 다음 퀴즈로 넘어갑니다.
        qizNum++;
        qizOrder = 0;
        ChangeQiz();
    }   
}