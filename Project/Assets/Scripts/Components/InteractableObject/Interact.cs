using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// InteractableObject 에 들어가는 추상클래스
[RequireComponent(typeof(Rigidbody))]
public abstract class Interact : MonoBehaviour
{
    
    // 상호작용이 끝났을 때 호출되는 대리자
    protected System.Action onFinishInteraction { get; set; }

    // 상호작용을 시작합니다.
    public void StartInteraction(System.Action finishInteractionEvent = null)
    {
        onFinishInteraction = finishInteractionEvent;

        Interaction();
    }

    // 상호작용을 끝냅니다.
    public void FinishInteraction()
    {
        // 끝 이벤트 실행
        onFinishInteraction?.Invoke();

        // 끝 이벤트 초기화
        onFinishInteraction = null;
    }

    public abstract void Interaction();

    public abstract bool UseInteractionKey();
}
