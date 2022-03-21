using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour
{
    [Header("대화 텍스트")]
    [SerializeField] private Text _Text;

    [Header("버튼 텍스트")]
    [SerializeField] private Text _ButtonText;
    
    private RectTransform rectTransform;

    // dialog 인덱스를 나타냅니다
    private int dialogIndex = 0;

    // NPCInfo 컴포넌트를 나타냅니다.
    private NPCInfo _NPCInfo;

    // 상호작용하고 있는 InteractableNPC 컴포넌트를 나타냅니다.
    public InteractableNPC interactableNPC;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        InitRectTransform();
    }

    private void Start()
    {
        _NPCInfo = interactableNPC.npcInfo;
        _Text.text = _NPCInfo.dialog[dialogIndex++];
    }

    // 다음 버튼을 누릅니다.
    public void PushNextButton()
    {
        // 마지막 대화면 대화를 종료합니다.
        if (dialogIndex == _NPCInfo.dialog.Count)
        {
            // 상호작용을 종료합니다
            interactableNPC.FinishInteraction();
            return;
        }
        // 마지막 전 대화면 다음 대화로 넘어가면서 버튼의 텍스트를 바꿉니다.
        else if (dialogIndex == _NPCInfo.dialog.Count - 1)
            _ButtonText.text = "EXIT";

        // 다음 대화로 넘어갑니다.
        _Text.text = _NPCInfo.dialog[dialogIndex++];
    }

    // RectTransform 초기화
    private void InitRectTransform()
    {
        rectTransform.localScale = Vector3.one;

        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
   
}
