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

    [Header("벽 Material")]
    [SerializeField] private Material _WallMaterial;

    [Header("레버 Material")]
    [SerializeField] private Material _LeverMaterial;

    [Header("다리 Material")]
    [SerializeField] private Material _BridgeMaterial;

    // material 알파 값
    private float alpa = 1.0f;

    // Material을 변경함을 나타냅니다.
    private bool isChangeMaterial;

    private void Update()
    {
        if(isChangeMaterial)
        {
            ChangeMaterial();
        }
    }

    private void OnDisable()
    {
        _WallMaterial.color = new Color(_WallMaterial.color.r, _WallMaterial.color.g, _WallMaterial.color.b, 1);
        _LeverMaterial.color = new Color(_LeverMaterial.color.r, _LeverMaterial.color.g, _LeverMaterial.color.b, 1);
        _BridgeMaterial.color = new Color(_BridgeMaterial.color.r, _BridgeMaterial.color.g, _BridgeMaterial.color.b, 0);
    }

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
