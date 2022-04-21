using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class SavePoint : MonoBehaviour
{
    [Header("해당 스테이지")]
    [SerializeField] private Stage _Stage;

    private PlayerCharacter _PlayerCharacter;

    private void Start()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 세이브포인트를 지나가면
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerInteractionArea"))
        {
            // 리스폰 위치를 변경합니다.
            _PlayerCharacter.respawnPosition = transform.position;

            _Stage.gameObject.SetActive(true);

            // 처음 스테이지에 진입함을 알립니다.
            if(_Stage.firstEnter)
                _Stage.StartStage();
        }
    }
}
