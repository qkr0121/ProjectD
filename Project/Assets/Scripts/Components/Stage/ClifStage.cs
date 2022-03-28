using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clif Stage의 시작,진행,끝 에 관한 코드
public class ClifStage : Stage
{
    [Header("사라질 벽")]
    [SerializeField] private GameObject _WallToDisappear;

    [Header("사라질 레버")]
    [SerializeField] private GameObject _LeverToDisappear;

    [Header("등장할 다리")]
    [SerializeField] private GameObject _Bridge;

    // 사라질 벽 Material
    private Material _WallMaterial;

    // 레버 Materail
    private Material _LeverMaterial;

    // 다리 Material
    private Material _BridgeMaterial;

    // material 알파 값
    private float alpa = 1.0f;

    // Material을 변경함을 나타냅니다.
    private bool isChangeMaterial;

    private void Start()
    {
        _WallMaterial = _WallToDisappear.GetComponent<MeshRenderer>().material;
        _LeverMaterial = _LeverToDisappear.GetComponentsInChildren<MeshRenderer>()[1].material;
        _BridgeMaterial = _Bridge.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if(isChangeMaterial)
        {
            ChangeMaterial();
        }
    }

    // 스테이지가 끝나면 실행합니다(NPC와 마지막 대사후 실행)
    public override void FinishStage()
    {
        isChangeMaterial = true;

        _Bridge.SetActive(true);

        // 캐릭터 정보 갱신
        RenewalCharacter();
    }

    // Material 알파값을 변경합니다.
    private void ChangeMaterial()
    {
        // 알파값이 0이하면 실행하지 않습니다.
        if(alpa <= 0.0f)
        {
            isChangeMaterial = false;

            Destroy(_WallToDisappear);
            Destroy(_LeverToDisappear);
        }

        // 각 Material의 알파값을 변경합니다.
        _WallMaterial.color = new Color(_WallMaterial.color.r, _WallMaterial.color.g, _WallMaterial.color.b, alpa);
        _LeverMaterial.color = new Color(_LeverMaterial.color.r, _LeverMaterial.color.g, _LeverMaterial.color.b, alpa);
        _BridgeMaterial.color = new Color(_BridgeMaterial.color.r, _BridgeMaterial.color.g, _BridgeMaterial.color.b, 1 - alpa);

        alpa -= 0.004f;
    }
}
