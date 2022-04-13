using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("캐릭터 최대 이동 속력")]
    [SerializeField] private float _MaxSpeed = 6.0f;

    [Header("캐릭터 최대 y축 속력")]
    [SerializeField] private float _MaxYSpeed = 10.0f;

    [Header("캐릭터 회전 속력")]
    [SerializeField] private float _RotationYawSpeed = 720.0f;

    [Header("점프 힘(속도)")]
    [SerializeField] private float _JumpVelocity = 10.0f;

    [Header("바닥을 제외할 레이어")]
    [SerializeField] private LayerMask _IgnoreGroundLayer;

    // 플레이어 캐릭터를 나타냅니다.
    private PlayerCharacter _PlayerCharacter;

    // 플레이어 캐릭터 컨트롤러를 나타냅니다.
    private CharacterController _CharacterController;

    // 입력 값을 나타냅니다.
    public Vector3 _InputVector { get; set; }

    // 카메라 방향 입력 값을 나타냅니다.
    private Vector3 _CameraDirInputVector;

    // 목표 속도를 나타냅니다.
    private Vector3 _TargetVelocity;

    // 속도를 나타냅니다.
    private Vector3 _Velocity;

    // 땅에 닿아 있음 상태를 나타냅니다
    public bool isGrounded; /*{ get; private set; }*/

    // 점프 가능 상태를 나타냅니다.
    public bool isJumpable;

    // 움직임 상태를 나타냅니다.
    public bool isMovable { get; private set; }

    // Move 사용 상태를 나타냅니다.
    private bool useMovable;

    // _Velocity 읽기 전용 프로퍼티
    public Vector3 velocity => _CharacterController.velocity;

    public float maxSpeed
    {
        get { return _MaxSpeed; }
        set { _MaxSpeed = value; }
    }

    // 오른쪽 회전 가능 상태 (-1 왼쪽 회전 불가능 1 오른쪽 회전 불가능 0 회전가능)
    public float rotatable { get; set; }

    private void Start()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
        _CharacterController = GetComponent<CharacterController>();

        isJumpable = true;
        isMovable = true;
        useMovable = true;
        rotatable = 0.0f;
    }

    private void Update()
    {
        RotationToMovement();
    }

    private void FixedUpdate()
    {
        // 입력값을 저장합니다.
        Vector3 pcInput = Vector3.zero;
        pcInput.x = Input.GetAxisRaw("Horizontal");
        pcInput.z = Input.GetAxisRaw("Vertical");
        _InputVector = pcInput;

        // 카메라 방향으로 입력값을 변경합니다.
        _CameraDirInputVector = _PlayerCharacter.trackingCamera.InputVectorToCamera(_InputVector);

        // 점프 키를 입력 시 점프를 합니다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // 가속도를 계산합니다.
        CaculateAcceleration();

        // 중력을 계산합니다.
        CaculateGravity();


        if (useMovable)
            _CharacterController.Move(_Velocity);

        UpgradeGroundState();
    }

    // 가속도를 계산합니다
    private void CaculateAcceleration()
    {
        // 목표 속도에 입력 값을 저장합니다.
        void CalculateInputVector()
        {
            _TargetVelocity.x = _CameraDirInputVector.x * _MaxSpeed * Time.deltaTime;
            _TargetVelocity.z = _CameraDirInputVector.z * _MaxSpeed * Time.deltaTime;
        }

        // 
        void CalculateAccel()
        {
            float currentTargetVectorY = _TargetVelocity.y;

            Vector3 currentTargetVelocity = (isMovable) ? _TargetVelocity : Vector3.zero;

            _Velocity = _CharacterController.velocity * Time.deltaTime;

            _Velocity.y = currentTargetVelocity.y;

            _Velocity = Vector3.MoveTowards(
                _Velocity,
                currentTargetVelocity,
                _MaxSpeed * (600.0f * 0.01f * Time.deltaTime) * Time.deltaTime);

            _Velocity.y = currentTargetVectorY;
        }

        CalculateInputVector();

        CalculateAccel();

    }

    // 이동하는 방향으로 회전합니다.
    private void RotationToMovement()
    {
        // 이동하지 않을 경우에는 회전을 하지 않습니다.
        if (_CameraDirInputVector.magnitude <= _CharacterController.minMoveDistance) return;

        // 움직일 수 없을 경우에는 회전을 하지 않습니다.
        if (!isMovable) return;

        // 목표 회전값을 저장합니다.
        float targetYawRotation = _TargetVelocity.ToAngle();

        if (rotatable == 1.0f)
        {
            if (_InputVector.x > 0.0f) return;

        }

        // 현재 회전값을 저장합니다.
        float currentYawRotation = transform.eulerAngles.y;

        // 다음 회전값을 저장합니다.
        float nextYawRotation = Mathf.MoveTowardsAngle(
            currentYawRotation,
            targetYawRotation,
            _RotationYawSpeed * Time.deltaTime);

        // 적용시킬 오일러 값을 저장합니다.
        Vector3 eulerAngle = nextYawRotation * Vector3.up;

        transform.eulerAngles = eulerAngle;

    }

    // 점프를 합니다.
    private void Jump()
    {
        // 점프를 할 수 없는 상황이면 반환합니다.
        if (isJumpable == false) return;

        // 점프를 하고 애니메이션을 실행하고 점프 불가능 상태로 만듭니다.
        if(isJumpable)
        {
            _TargetVelocity.y = 1.5f * _JumpVelocity * Time.fixedDeltaTime;
            _PlayerCharacter.playerAnim.JumpingAnim();
            isJumpable = false;
        }

    }

    // 중력 계산
    private void CaculateGravity()
    {
        // y 축 속력에 중력을 적용 시킵니다.
        _TargetVelocity.y += Physics.gravity.y * 0.05f * Time.fixedDeltaTime;

        // y 축 속력이 최대를 넘지 않도록 합니다.
        _TargetVelocity.y = Mathf.Clamp(_TargetVelocity.y, -_MaxYSpeed, _MaxYSpeed);
    }

    // 땅에 닿음 상태를 업데이트 합니다.
    private void UpgradeGroundState()
    {       
        Ray ray1 = new Ray(transform.position + _CharacterController.center, Vector3.down);

        RaycastHit hit1;

        isGrounded = Physics.SphereCast(
            ray1,
            _CharacterController.radius,
            out hit1,
            _CharacterController.center.y - _CharacterController.radius + _CharacterController.skinWidth * 2,
            ~_IgnoreGroundLayer)
            && _Velocity.y <= 0.0f;

        if (isGrounded)
        { 
            _TargetVelocity.y = 0.0f;

            // 트랩을 밟았을시 트랩을 사라지게 합니다.
            if(hit1.transform.gameObject.layer == LayerMask.NameToLayer("Trap"))
            {
                GameObject collTrapObj;
                collTrapObj = hit1.transform.gameObject;
                collTrapObj.SetActive(false);
                _PlayerCharacter.caveSoundQiz.ChangeCaveRoad();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 낭떠러지인 경우 리스폰으로 이동합니다.
        if (other.gameObject.layer == LayerMask.NameToLayer("Clif"))
        {
            // 추락 애니메이션을 재생합니다.
            _PlayerCharacter.playerAnim.FallingAnim();

            //StopMove();
        }
    }

    // 플레이어를 리스폰 지역으로 순간이동 시킵니다.
    public IEnumerator Respawn()
    {
        useMovable = false;

        transform.position = _PlayerCharacter.respawnPosition;

        yield return new WaitForSeconds(0.3f);

        AllowMove();
        useMovable = true;
    }

    public void AllowMove()
    {
        isMovable = true;
        isJumpable = true;
    }

    public void StopMove()
    {
        isMovable = false;
        isJumpable = false;
    }
}
