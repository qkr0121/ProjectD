using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    [Header("타겟")]
    [SerializeField] private GameObject _TargetObject;

    [Header("Max Pitch Value")]
    [SerializeField] private float _MaxPitchValue;

    [Header("Rotation Speed")]
    [SerializeField] private float _RotationSpeed;

    // PlayerCharacter 컴포넌트를 나타냅니다.
    private PlayerCharacter _PlayerCharacter;

    // Pitch 회전 값을 나타냅니다.
    private float _PitchRotation;

    // Yaw 회전 값을 나타냅니다.
    private float _YawRotation;

    // 카메라를 나타냅니다.
    public new Camera camera { get; private set; }

    // 부모오브젝트를 타겟오브젝트로 할것인가
    public bool isTargetParent;

    // 이 오브젝트를 루트 오브젝트로 할것인가
    public bool isRoot;

    // 카메라 조종 가능 여부를 나타냅니다.
    public bool isControlCamera;

    private void Awake()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
        camera = GetComponentInChildren<Camera>();

        if (isTargetParent)
        {
            _TargetObject = transform.parent.gameObject;
        }

    }

    private void Update()
    {
        if (!isControlCamera) return;
        if (!_PlayerCharacter.playerMovement.isMovable) return;

        AddPitch(Input.GetAxisRaw("Mouse Y"));
        AddYaw(Input.GetAxisRaw("Mouse X"));
        RotationCamera();
        MoveCamera();
    }

    // 카메라를 회전 시킵니다.
    private void RotationCamera()
    {
        transform.eulerAngles = new Vector3(_PitchRotation, _YawRotation, 0.0f);
    }

    // 카메라를 이동 시킵니다.
    private void MoveCamera()
    {
        camera.transform.position = transform.position + transform.forward * -2.0f;
        //transform.position = _TargetObject.transform.position;

    }

    // Pitch 값을 증가시킵니다.
    private void AddPitch(float value)
    {
        _PitchRotation -= value * _RotationSpeed;
        _PitchRotation = Mathf.Clamp(_PitchRotation, -_MaxPitchValue, _MaxPitchValue);
    }

    // Yaw 값을 증가 시킵니다.
    private void AddYaw(float value)
    {
        _YawRotation += value * _RotationSpeed;
    }

    // 카메라가 보는 방향으로 입력 값을 반환합니다.
    public Vector3 InputVectorToCamera(Vector3 inputVector)
    {
        Vector3 forwardVector = camera.transform.forward;
        Vector3 rightVector = camera.transform.right;

        forwardVector.y = 0.0f;
        forwardVector.Normalize();
        
        // 
        forwardVector *= inputVector.z;
        rightVector *= inputVector.x;

        return (forwardVector + rightVector).normalized;
    }

    // 카메라를 ViewPosition 으로 옮깁니다.
    public void MoveToViewPosition(Transform viewPosition)
    {
        camera.transform.SetParent(viewPosition);
        camera.transform.localPosition = Vector3.zero;
        camera.transform.localEulerAngles = Vector3.zero;
    }

    // 카메라 위치를 초기화 시킵니다.
    public void InitCameraPos()
    {
        camera.transform.SetParent(_PlayerCharacter.trackingCamera.transform);
        camera.transform.localPosition = Vector3.zero;
        camera.transform.localEulerAngles = Vector3.zero;
        MoveCamera();
    }

}
