using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 stage 클래스에 들어가는 추상클래스
public abstract class Stage : MonoBehaviour
{
    // 세이브포인트를 나타냅니다.
    [Header("SavePoint")]
    [SerializeField] private Transform _SavePoint;

    private PlayerCharacter _PlayerCharacter;

    private void Start()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
    }

    // stage 가 시작할 때 실행합니다.
    public void StartStage()
    {
    }

    // stage 가 끝났을 때 실행합니다.
    public abstract void FinishStage();

    private void OnTriggerEnter(Collider other)
    {
        // 스테이지 진입시 savepoint를 설정합니다.
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerInteractionArea"))
        {
            _PlayerCharacter.respawnPosition = _SavePoint.position;
        }
    }
}
