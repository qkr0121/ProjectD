using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SnowBall : Interact
{
    [Header("눈덩이 부모 오브젝트")]
    [SerializeField] private Transform _ParentOb;

    [Header("출발 지점")]
    [SerializeField] private Transform _StartPoint;

    [Header("도착 지점까지 거리")]
    [SerializeField] private float _MapDistant;

    [Header("눈덩이 최고 크기")]
    [SerializeField] private float _MaxScale;

    [Header("눈덩이 최소 크기")]
    [SerializeField] private float _MinScale;

    [Header("감지할 벽 레이어")]
    [SerializeField] private LayerMask _WallLayer;

    [Header("눈덩이 초기 위치")]
    [SerializeField] private Transform _InitSnowballPos;

    [Header("눈덩이 초기 부모오브젝트")]
    [SerializeField] private GameObject _InitParentObj;

    // PlayerCharacter 컴포넌트를 나타냅니다.
    private PlayerCharacter _PlayerCharacter;

    // SnowStage 컴포넌트를 나타냅니다.
    private SnowStage _SnowStage;

    // 플레이어가 눈덩이를 굴린 최대 거리를 나타냅니다.
    private float _MaxMoveDistant;

    // 현재 플레이어와 출발지점 사이의 거리를 나타냅니다.
    private float _MoveDistant;

    // 눈덩이가 커져야할 크기
    private float scale;

    private Vector3 detectWallPosition;

    private Vector3 DetectWallPosition;

    // 굴릴 수 있는 상태를 나타냅니다.
    private bool isRoll;

    private bool isWall;

    private bool reward;

    private void Start()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
        _SnowStage = GetComponentInParent<SnowStage>();
        scale = 0.0f;
    }

    private void Update()
    {

        Roll();
        SetSnowBallPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 장애물과 닿았을 경우 초기위치로 되돌아 갑니다.
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            // 초기위치에 새로 생성합니다.
            Instantiate(this, _InitSnowballPos.position,_InitSnowballPos.rotation,_InitParentObj.transform);

            // 지금 굴리고 있는 눈덩이를 파괴합니다.
            Destroy(this.gameObject);
        }
        // 벽에 닿았을 경우 해당 위치를 저장합니다.
        else if(other.gameObject.layer == LayerMask.NameToLayer("Wall") && reward)
        {
            DetectWallPosition = _PlayerCharacter.transform.position;
            reward = false;
        }
        // 목표지점에 도착했을 경우 스테이지가 클리어 됩니다.
        else if(other.gameObject.layer == LayerMask.NameToLayer("TargetPoint"))
        {
            gameObject.SetActive(false);
            _SnowStage.stageClear = true;
            Debug.Log(_SnowStage.stageClear);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 벽에 닿았을 경우 이동할 수 없습니다.
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _PlayerCharacter.transform.position = DetectWallPosition;

            // 회전 가능 상태를 감지합니다.
            DetectForRotation(false);
            DetectForRotation(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        reward = true;
        _PlayerCharacter.playerMovement.rotatable = 0.0f;
    }

    // 눈덩이 위치와 크기를 조정합니다.
    private void SetSnowBallPosition()
    {
        if (!isRoll) return;

        // 눈덩이 위치
        transform.localPosition =
            new Vector3(
                0.0f,
                transform.localScale.y / 2.0f,
                transform.localScale.z / 1.5f);

        // 눈덩이 크기
        scale = (_MaxScale - _MinScale) * (_MaxMoveDistant / _MapDistant);

        if (scale >= _MaxScale - _MinScale) return;

        Vector3 snowballScale =
            new Vector3(
                0.8f + scale,
                0.8f + scale,
                0.8f + scale);

        transform.localScale = snowballScale;
    }

    // 눈덩이를 굴립니다.
    private void Roll()
    {
        if (!isRoll) return;

        // 레이캐스트 시작 위치를 나타냅니다. 
        Vector3 rayPosition = new Vector3(transform.position.x, 0.1f, transform.position.z);

        Ray ray = new Ray(rayPosition, -Vector3.right);

        RaycastHit hit;

        int layermasck = 1 << LayerMask.NameToLayer("StartPoint");

        // 눈덩이의 위치를 계산합니다.
        if(Physics.Raycast(ray, out hit, 100.0f, layermasck))
        {
            _MoveDistant = hit.distance;
        }

        // 눈덩이의 최고거리를 넘었으면 거리를 갱신해줍니다.
        _MaxMoveDistant = (_MaxMoveDistant < _MoveDistant) ? _MoveDistant : _MaxMoveDistant;
        
    }

    // 벽에 닿았을 때 회전 가능 상태를 나타냅니다.
    private void DetectForRotation(bool reverse)
    {
        // 공과 플레이어 사이의 거리를 나타냅니다.
        Vector3 distantToBall = transform.position - _PlayerCharacter.transform.position;
        distantToBall.y = 0.0f;

        // 플레이어에서 공의 접점 방향의 벡터를 나타냅니다.
        Vector3 contactPosition = distantToBall.Tangent(distantToBall.magnitude, 0.5f * (0.8f + scale), reverse);

        // 접점 벡터를 플레이어 방향만큼 회전 시키기 위한 각도를 나타냅니다.
        float degree = _PlayerCharacter.transform.forward.ToAngle() + contactPosition.ToAngle();
        float radian = (90 - degree) * Mathf.PI / 180;

        // 플레이어 방향을 고려한 접점벡터
        contactPosition = new Vector3(Mathf.Cos(radian), 0.0f, Mathf.Sin(radian));

        // 시작위치를 나타냅니다.
        Vector3 startPosition = 
            new Vector3(
                _PlayerCharacter.transform.position.x,
                0.5f * (0.8f + scale),
                _PlayerCharacter.transform.position.z);

        Ray ray = new Ray(startPosition, contactPosition);

        isWall = Physics.Raycast(
            ray,
            (distantToBall.magnitude + 0.5f * (0.8f + scale)),
            _WallLayer);

        if(isWall)
        {
            // reverse 가 true 면 왼쪽 false 면 오른쪽 회전을 불가능 하게 합니다.
            _PlayerCharacter.playerMovement.rotatable = reverse ? -1.0f : 1.0f;
        }
    }

    IEnumerator SetRoll()
    {
        transform.SetParent(_ParentOb);
        isRoll = true;

        yield return new WaitForSeconds(0.1f);

        FinishInteraction();
    }

    public override void Interaction()
    {
        StartCoroutine(SetRoll());
    }

    public override bool UseInteractionKey()
    {
        return true;
    }
}
