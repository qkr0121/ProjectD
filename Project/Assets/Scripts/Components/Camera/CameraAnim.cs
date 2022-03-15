using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnim : MonoBehaviour
{
    // PlayerCharacter 컴포넌트를 나타냅니다.
    private PlayerCharacter _PlayerCharacter;

    // Animator 컴포넌트를 나타냅니다.
    private Animator _Animator;

    // 카메라 이동 후 실행할 대리자를 나타냅니다
    public System.Action afterCameraMoveAnim { get; set; }

    private void Awake()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
        _Animator = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    // 미로를 탈출 완료 카메라 이동 애니메이션을 재생합니다.
    public void AnimEscapeMaze()
    {
        _Animator.SetTrigger("_Escape");
    }

    // 탈출 카메라 이동 애니메이션 재생후 실행할 이벤트
    private void AnimEvent_EscapeMaze()
    {
        afterCameraMoveAnim?.Invoke();
    }

}
