using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            // 상호작용 키를 활성화합니다.
            InputManager.Instance.SetActiveInteractJoyStick();

            // 스테이지르 시작합니다.
            _Stage.StartStage();
        }
    }
}
