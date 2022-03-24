using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableNPC : Interact
{
    [Header("TMP")]
    [SerializeField] private TextMeshPro _TMP;

    [Header("NPCCode")]
    [SerializeField] private string _NPCCode;

    [Header("카메라 위치")]
    [SerializeField] private Transform _CameraPosition;

    [Header("스테이지")]
    [SerializeField] private Stage _Stage;

    // 플레이어 캐릭터 컴포넌트를 나타냅니다.
    private PlayerCharacter _PlayerCharacter;

    // NPC 정보를 담은 NPCInfo
    private NPCInfo _NPCInfo;

    // NPCInfo 읽기 전용 프로퍼티
    public NPCInfo npcInfo => _NPCInfo;

    private void Start()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;

        _NPCInfo = ResourceManager.Instance.LoadJson<NPCInfo>(_NPCCode.ToString(), "Prefabs/Character/NPC");

        // NPC 이름 설정
        _TMP.text = _NPCInfo.name;
    }

    private void Update()
    {
        SetRotationTMP();
    }

    // NPC의 TMP 의 방향을 설정합니다.(캐릭터를 바라보게)
    private void SetRotationTMP()
    {
        _TMP.rectTransform.eulerAngles = _PlayerCharacter.transform.position.Direction(transform.position).ToAngle() * Vector3.up;

    }

    public override void Interaction()
    {
        // 조이스틱의 오브젝트를 비활성화 합니다.
        PlayerManager.Instance.gameUI[JoyStickType.Move].gameObject.SetActive(false);
        PlayerManager.Instance.gameUI[JoyStickType.Interact].gameObject.SetActive(false);

        // Dialog UI를 생성합니다.
        GameObject gameOb = Instantiate(ResourceManager.Instance.LoadResource<GameObject>("NPCDialog", "Prefabs/UI/NPCDialog"), GameObject.Find("GameUI").transform, true);

        // NPCDialog의 InteractableNPC를 설정합니다.
        gameOb.GetComponent<NPCDialog>().interactableNPC = this;

        // 카메라의 위치를 옮깁니다.
        _PlayerCharacter.trackingCamera.MoveToViewPosition(_CameraPosition);

        // 플레이어를 투명하게 합니다.
        _PlayerCharacter.playerMesh.SetActive(false);

        // 대화가 끝났을 시 실행할 것들을 추가합니다.
        onFinishInteraction += () =>
       {
           // 플레이어를 다시 보이게 합니다.
           _PlayerCharacter.playerMesh.SetActive(true);

           // NPCDialog UI 를 제거합니다.
           Destroy(gameOb);

           // 퀘스트를 깬 후 처음으로 대화를 하면 끝 이벤트를 호출합니다.
           if(_Stage.firstConv)
           {
               _Stage.FinishStage();
           }

           // 조이스틱의 오브젝트를 활성화 합니다.
           PlayerManager.Instance.gameUI[JoyStickType.Move].gameObject.SetActive(true);
           PlayerManager.Instance.gameUI[JoyStickType.Interact].gameObject.SetActive(true);
       };
    }

    public override bool UseInteractionKey()
    {
        return true;
    }
}