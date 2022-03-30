using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStage : Stage
{
    [Header("열릴 벽 오브젝트")]
    [SerializeField] private GameObject _WallToOpen;

    public bool stageClear;

    IEnumerator OpenWall()
    {
        while (true)
        {
            yield return null;
        }
    }

    public override void FinishStage()
    {
        // 캐릭터가 움직이지 못하도록 합니다.
        _PlayerCharacter.playerMovement.StopMove();

        // 카메라의 위치를 변경합니다.
        _PlayerCharacter.trackingCamera.MoveToViewPosition(_CameraPosition);

        StartCoroutine(OpenWall());
    }

    
}
