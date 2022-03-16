using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterAnim : MonoBehaviour
{

    // Animator 컴포넌트를 나타냅니다.
    private Animator _Animator;

    // Playercharacter 컴포넌트를 나타냅니다
    private PlayerCharacter _PlayerCharacter;

    private void Awake()
    {
        _PlayerCharacter = GetComponentInParent<PlayerCharacter>();
        _Animator = GetComponent<Animator>();
    }

    // 해당 기능을 실행하는데 필요한 최소한의 데이터만 외부에서 받아와서 사용..

    //public void SetAnimVelocity(Vector3 velocity)
    //{
    //    if (velocity.y == 0.0f)
    //    {
    //        _Animator.SetFloat("_Velocity", velocity.magnitude);
    //    }
    //    else
    //        _Animator.SetFloat("_Velocity", 0.0f);
    //}

    private void Update()
    {
        // 이동 애니메이션
        if(_PlayerCharacter.playerMovement.velocity.y == 0.0f)
        {
            _Animator.SetFloat("_Velocity", _PlayerCharacter.playerMovement.velocity.magnitude);
        }
        else
        {
            _Animator.SetFloat("_Velocity", 0.0f);
        }

        // 점프 애니메이션
        _Animator.SetBool("_IsGrounded", _PlayerCharacter.playerMovement.isGrounded);
        
    }

    // 점프 애니메이션을 재생합니다.
    public void JumpingAnim()
    {
        _Animator.Play("Jumping");
    }

    // 추락 애니메이션을 재생합니다.
    public void FallingAnim()
    {
        _Animator.Play("Falling");
    }

    // 추락 후 이벤트를 실행합니다.
    public void FallingFinishEvent()
    {
        _PlayerCharacter.playerMovement.StartCoroutine(_PlayerCharacter.playerMovement.Respawn());
        //_PlayerCharacter.playerMovement.StartCoroutine("Respawn");
    }
}
