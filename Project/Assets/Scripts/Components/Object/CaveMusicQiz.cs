using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMusicQiz : MonoBehaviour
{
    [Header("퀴즈1")]
    [SerializeField] private List<int> _Qiz1;

    [Header("퀴즈2")]
    [SerializeField] private List<int> _Qiz2;

    [Header("퀴즈3")]
    [SerializeField] private List<int> _Qiz3;

    [Header("문")]
    [SerializeField] private Transform _Door;

    [Header("문이 열리는 속도")]
    [SerializeField] private float _DoorOpenSpeed;

    [Header("정답 알림 구")]
    [SerializeField] private List<Renderer> _AnswerSphere;

    [Header("카메라 위치")]
    [SerializeField] private Transform _CameraPos;

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

    // 문이 열림 상태
    private bool isOpen;

    // 구의 빛남 상태
    private bool isLight;

    private void Start()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
        _PresentQiz = _Qiz1;
        qizOrder = 0;
        qizNum = 1;
        _IncColor = 0;

        _PresentDoorPosY = _Door.localPosition.y;
    }

    private void Update()
    {
        // 정답 알림 구에 불이 들어옵니다.
        if (isLight)
            LightAnswerSphere(qizNum);

        // 문이 열립니다.
        if (isOpen)
            OpenDoor();
    }

    // 퀴즈 번호를 변경합니다.
    private void ChangeQiz()
    {
        if (qizNum == 1) _PresentQiz = _Qiz1;
        else if(qizNum == 2) _PresentQiz = _Qiz2; 
        else if (qizNum == 3) _PresentQiz = _Qiz3;
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

    private void OnTriggerEnter(Collider other)
    {
        // 시작지점 진입시 상호작용 버튼을 활성화합니다.
        InputManager.Instance.SetActiveInteractJoyStick();
    }

    // 모든 문제를 맞추었을시 문이 열립니다.
    private void OpenDoor()
    {  
        if(_Door.localPosition.y - _PresentDoorPosY <= 10.0f)
            _Door.localPosition += Vector3.up * Time.deltaTime * _DoorOpenSpeed;
    }

    // 정답을 입력했을시 정답 알림 구에 빛이 나게 합니다.
    private void LightAnswerSphere(int qizNum)
    {
        _IncColor += 1;
        _AnswerSphere[qizNum - 1].material.color = new Color(_IncColor/255f, 0, _IncColor/255f);
        if (_AnswerSphere[qizNum - 1].material.color.r > 1)
        {
            isLight = false;
            _IncColor = 0;
        }
    }

    // 문제를 맞추었을시 실행할 이벤트
    IEnumerator CorrectAnswer()
    {
        yield return new WaitForSeconds(2.0f);

        // 구에 불이 들어올 수 있게 합니다.
        isLight = true;

        _PlayerCharacter.playerMovement.StopMove();
        // 카메라의 위치 문을 바라 볼수 있게 변경합니다.
        _PlayerCharacter.trackingCamera.MoveToViewPosition(_CameraPos);

        yield return new WaitForSeconds(3.0f);

        // 다음 퀴즈로 넘어갑니다.
        qizNum++;
        qizOrder = 0;
        ChangeQiz();

        // 마지막 퀴즈까지 맞추었다면 문이 열립니다.
        if (qizNum > 3)
        {
            isOpen = true;
            yield return new WaitForSeconds(6.0f);
        }

        _PlayerCharacter.playerMovement.AllowMove();
        _PlayerCharacter.trackingCamera.InitCameraPos();

    }


}

// 버튼기둥 가로로 더 크게
// 버튼기둥 높이를 올려서 버튼을 세로방향에 설치
// 문에 붙어있느 구 크기를 버튼 크기 정도로



// UI작업과 소리작업
// 처음 문에 상호작용 키 입력시 튜토리얼 시작