using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStage : Stage
{
    [Header("열릴 벽 오브젝트")]
    [SerializeField] private GameObject _WallToOpen;

    [Header("벽이 열리는 속도")]
    [SerializeField] private float _DoorOpenSpeed;

    [Header("눈보라 파티클")]
    [SerializeField] private GameObject _BlizzardParticle;

    // SnowBall 컴포넌트
    public SnowBall _SnowBall;

    // 파티클 시스템
    private ParticleSystem _Particle;

    // 처음 벽 위치
    private Vector3 _InitWallPos;

    // 파괴 시킬수 있는 상태
    private bool breaking;

    private void OnEnable()
    {
        _Particle = _BlizzardParticle.GetComponent<ParticleSystem>();

        _InitWallPos = _WallToOpen.transform.position;

        breaking = false;

        stageClear = false;

        StartCoroutine(Blizzard());
    }

    private void Update()
    {
        // 눈보라가 칠때 안전지대에 있지 않으면 눈덩이를 파괴시킵니다.
        if(breaking)
        {
            if(_SnowBall.isBreakable)
            {
                _SnowBall.InitSnowBall();
            }
        }
    }

    // 눈보라파티클
    IEnumerator Blizzard()
    {
        var emission = _Particle.emission;

        // 눈보라가 칩니다.
        _Particle.Play();
        yield return new WaitForSeconds(1.0f);

        // 파괴 가능상태로 변경합니다.
        breaking = true;

        //플레이어 속도를 늦춥니다.
        _PlayerCharacter.playerMovement.maxSpeed = 2.0f;
        emission.rateOverTime = 100.0f;

        yield return new WaitForSeconds(3.0f);

        // 눈보라가 그칩니다.
        _Particle.Stop();
        emission.rateOverTime = 10.0f;

        // 파괴 불가상태로 변경합니다.
        breaking = false;
        // 플레이어 속도를 되돌립니다.
        _PlayerCharacter.playerMovement.maxSpeed = 3.0f;

        yield return new WaitForSeconds(4.0f);

        // 스테이지를 클리어 하면 실행하지 않습니다.
        if(!stageClear)
            StartCoroutine(Blizzard());
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
