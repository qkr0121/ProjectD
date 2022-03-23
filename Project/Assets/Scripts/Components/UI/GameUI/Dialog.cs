using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [Header("대화 텍스트")]
    [SerializeField] private Text _Text;

    [Header("버튼")]
    [SerializeField] private Text _ButtonText;

    private PlayerCharacter _PlayerCharacter;

    private RectTransform rectTransform;

    // 실행 시킬 StageInfo 프리팹
    private StageInfo _StageInfo;

    // 다이어로그 인덱스를 나타냅니다.
    private int dialogindex = 0;

    // 실행시킬 dialog 프리팹 이름
    public string _DialogName;

    private void Awake()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
        rectTransform = GetComponent<RectTransform>();

        InitRectTransform();
    }

    private void Start()
    {
        // stageinfo 프리팹을 불러옵니다.
        _StageInfo = ResourceManager.Instance.LoadJson<StageInfo>(_DialogName.ToString(), "Prefabs/Character/NPC");

        // 첫번째 대사 실행
        _Text.text = _StageInfo.infoDialog[dialogindex++];
    }

    // 다음 버튼을 누릅니다.
    public void PushNextButton()
    {
        // 마지막 대사면 종료합니다.
        if(dialogindex == _StageInfo.infoDialog.Count)
        {
            // 플레이어가 다시 움직일 수 있습니다.
            _PlayerCharacter.playerMovement.AllowMove();

            // dialog 컴포넌트를 제거합니다.
            Destroy(this.gameObject);
            return;
        }
        // 마지막 전 대사면 버튼의 텍스트를 바꿉니다.
        else if(dialogindex == _StageInfo.infoDialog.Count - 1)
        {
            _ButtonText.text = "EXIT";
        }

        // 다음 대사 실행
        _Text.text = _StageInfo.infoDialog[dialogindex++];

    }

    // RectTransform 초기화
    private void InitRectTransform()
    {
        rectTransform.localScale = Vector3.one;

        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
