using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 stage 클래스에 들어가는 추상클래스
// 자식으로 SavePoint를 가집니다.
public abstract class Stage : MonoBehaviour
{
    [Header("스테이지 이름")]
    [SerializeField] private string Name;

    // 처음으로 진입했는지에 대한 읽기전용 프로퍼티
    public bool firstEnter { get; private set; } = true;

    // 퀘스트를 깬 후 처음으로 NPC와 대화를 했는지 나타내는 읽기 전용 프로퍼티
    public bool firstConv { get; private set; } = true;

    protected PlayerCharacter _PlayerCharacter;

    private void Start()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
    }

    // stage 가 시작할 때 실행합니다. (UI 로 스테이지 간단하게 설명)
    public void StartStage()
    {
        // StartDialog 프리팹을 불러옵니다.
        GameObject dialogObject = Instantiate(ResourceManager.Instance.LoadResource<GameObject>("StartDialog", "Prefabs/UI/StartDialog"), GameObject.Find("GameUI").transform, true);

        // Dialog 컴포넌트를 불러옵니다.
        Dialog dialog = dialogObject.GetComponent<Dialog>();

        dialog._DialogName = Name + "StageInfo";

        // 플레이어가 움직일 수 없도록 합니다.
        _PlayerCharacter.playerMovement.StopMove();

        firstEnter = false;
    }

    // stage 가 끝났을 때 실행합니다.
    public abstract void FinishStage();

    // 캐릭터 움직임과 정보를 갱신합니다.
    protected void RenewalCharacter()
    {
        firstConv = false;
        
    }
}
