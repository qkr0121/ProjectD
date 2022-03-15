using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSetSpawn : Interact
{

    // PlayerCharcter 컴포넌트를 나타냅니다.
    private PlayerCharacter _PlayerCharacter;

    private void Start()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
    }

    IEnumerator SetSpawn()
    {
        yield return new WaitForSeconds(0.2f);
        FinishInteraction();
    }

    public override void Interaction()
    {
        // 리스폰 위치를 변경합니다.
        _PlayerCharacter.respawnPosition = transform.position;
        Debug.Log(_PlayerCharacter.respawnPosition);

        // 상호작용 키 활성화
        InputManager.Instance.SetActiveInteractJoyStick();

        StartCoroutine(SetSpawn());
    }

    public override bool UseInteractionKey()
    {
        return true;
    }
}
