using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InteractableStageZone : Interact
{
    [Header("사라지게 할 Material")]
    [SerializeField] private Material _Material;

    [Header("나타나게 할 Material")]
    [SerializeField] private Material _FieldMaterial;

    [Header("카메라 위치")]
    [SerializeField] private Transform _CameraPosition;

    [Header("탈출 시 뒤를 막을 Collider")]
    [SerializeField] private BoxCollider _BlockCollider;

    [Header("미로 오브젝트")]
    [SerializeField] private GameObject _Maze;

    [Header("다음 단계로 가는 입구 collider")]
    [SerializeField] private GameObject _EnterCollider;

    // 플레이어 캐릭터를 나타냅니다.
    private PlayerCharacter _PlayerCharacter;

    // Material 의 알파 값을 나타냅니다.
    private float alpa = 1;

    // Material 변경 상태를 나타냅니다.
    private bool isChangeMaterial = false;

    private Coroutine finishCoroutine;

    private void Awake()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
    }

    private void FixedUpdate()
    {
        if (isChangeMaterial)
            DisapperMaze();        
    }

    // 미로가 사라지고 필드 오브젝트가 등장합니다.
    private void DisapperMaze()
    {
        if (alpa <= 0.0f)
        {
            FinishInteraction();
            if (finishCoroutine != null)
            {
                StopCoroutine(finishCoroutine);
                finishCoroutine = null;
            }

            finishCoroutine = StartCoroutine(FinishEvent());
            return;
        }

        _Material.color = new Color(_Material.color.r, _Material.color.g, _Material.color.b, alpa);
        _FieldMaterial.color = new Color(_FieldMaterial.color.r, _FieldMaterial.color.g, _FieldMaterial.color.b, 1 - alpa);
        alpa -= 0.004f;
    }
    
    // 상호작용 끝 이벤트 후 실행할 내용
    IEnumerator FinishEvent()
    {
        isChangeMaterial = false;
        
        yield return new WaitForSeconds(3.0f);

        Destroy(_Maze);
        Destroy(_EnterCollider);

        _Material.color = new Color(_Material.color.r, _Material.color.g, _Material.color.b, 1);

    }

    // 상호작용시 실행할 내용
    public override void Interaction()
    {
        // 카메라의 위치를 옮깁니다.
        _PlayerCharacter.trackingCamera.MoveToViewPosition(_CameraPosition);

        isChangeMaterial = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            _BlockCollider.gameObject.SetActive(true);
    }

    public override bool UseInteractionKey()
    {
        return false;
    }

}
