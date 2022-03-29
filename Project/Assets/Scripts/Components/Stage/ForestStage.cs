using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestStage : Stage
{

    [Header("사라질 Material")]
    [SerializeField] private Material _MazeMaterial;

    [Header("나타낼 Material")]
    [SerializeField] private Material _FieldMaterial;

    [Header("카메라 위치")]
    [SerializeField] private Transform _CameraPosition;

    [Header("FieldObject")]
    [SerializeField] private GameObject _FieldObject;

    [Header("미로 오브젝트")]
    [SerializeField] private GameObject _Maze;

    [Header("다음 단계로 가는 입구 collider")]
    [SerializeField] private GameObject _EnterCollider;

    // Material 의 알파 값을 나타냅니다.
    private float alpa = 1;

    // material를 원상복구합니다.
    private void OnDisable()
    {
        _MazeMaterial.color = new Color(_MazeMaterial.color.r, _MazeMaterial.color.g, _MazeMaterial.color.b, 1);
        _FieldMaterial.color = new Color(_FieldMaterial.color.r, _FieldMaterial.color.g, _FieldMaterial.color.b, 0);
    }

    // 미로가 사라지고 필드 오브젝트가 등장합니다.
    IEnumerator DisapperMaze()
    {
        while(true)
        {
            // alpa 값이 0 이하면 실행하지 않습니다.
            if (alpa <= 0.0f)
            {
                Destroy(_Maze);
                Destroy(_EnterCollider);

                yield break;
            }

            _MazeMaterial.color = new Color(_MazeMaterial.color.r, _MazeMaterial.color.g, _MazeMaterial.color.b, alpa);
            _FieldMaterial.color = new Color(_FieldMaterial.color.r, _FieldMaterial.color.g, _FieldMaterial.color.b, 1 - alpa);
            alpa -= 0.004f;

            yield return null;
        }
    }


    // 스테이지가 끝나면 실행합니다(NPC와 마지막 대사후 실행)
    public override void FinishStage()
    {
        // 카메라의 위치를 옮깁니다.
        _PlayerCharacter.trackingCamera.MoveToViewPosition(_CameraPosition);

        _FieldObject.SetActive(true);

        StartCoroutine(DisapperMaze());

        // 캐릭터 정보 갱신
        RenewalCharacter();
    }

}
