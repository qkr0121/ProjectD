using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStage : Stage
{
    [Header("열릴 벽 오브젝트")]
    [SerializeField] private GameObject _WallToOpen;

    [Header("벽이 열리는 속도")]
    [SerializeField] private float _DoorOpenSpeed;

    // 처음 벽 위치
    private Vector3 _InitWallPos;

    public bool stageClear;

    private void Start()
    {
        _InitWallPos = _WallToOpen.transform.position;
    }

    // 문을 엽니다.
    IEnumerator OpenWall()
    {
        while (true)
        {
            // 문이 다 열리면 작동하지 않습니다.
            if(_WallToOpen.transform.position.y - _InitWallPos.y >= 10.0f)
            {

                RenewalCharacter();
                yield break;
            }

            _WallToOpen.transform.position += Vector3.up * Time.deltaTime * _DoorOpenSpeed;

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
