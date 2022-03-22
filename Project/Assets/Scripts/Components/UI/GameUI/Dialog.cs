using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [Header("대화 텍스트")]
    [SerializeField] private Text _Text;

    [Header("버튼")]
    [SerializeField] private Button _Button;

    [Header("Dialog 프리펩 이름")]
    [SerializeField] private string _DialogName;

    private RectTransform rectTransform;

    // 실행 시킬 다이어 로그 프리팹
    private List<string> _Dialog;

    // 다이어로그 인덱스를 나타냅니다.
    private int dialogindex = 0;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        _Dialog = ResourceManager.Instance.LoadJson<List<string>>(_DialogName, "Prefabs/Character/NPC");
    }

    private void Start()
    {
    }

    private void InitRectTransform()
    {
        rectTransform.localScale = Vector3.one;

        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
    }
}
