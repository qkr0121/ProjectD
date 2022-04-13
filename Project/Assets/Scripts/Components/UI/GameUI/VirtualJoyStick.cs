using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class VirtualJoyStick : MonoBehaviour,
    IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("사용되는 이미지")]
    [SerializeField] private Image _VirtualJSThumbImage;

    // 조이스틱이 이동할 수 있는 범위
    [SerializeField] private float _JSRadius = 60.0f;

    // RectTransform 컴포넌트를 나타냅니다.
    private RectTransform _JSRectTransform;

    // InteractJoyStick 의 수직 이동 가능 상태를 나타냅니다.
    public bool isVerticalMovable;

    // 조이스틱 축값을 나타냅니다.
    public Vector3 inputAxis { get; private set; }

    // 조이스틱 입력 중을 나타냅니다
    public bool isInput { get; set; }

    private void Awake()
    {
        _JSRectTransform = GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        isInput = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 입력된 위치를 저장합니다.
        Vector2 inputPos = eventData.position / GameStatics.screenRatio;

        // 입력 위치에서 조이스틱 중심의 배경위치를 뺍니다.
        inputPos -= _JSRectTransform.anchoredPosition;

        // 조이스틱을 가둡니다.
        inputPos = Vector2.Distance(Vector2.zero, inputPos) < _JSRadius ?
            inputPos : inputPos.normalized * _JSRadius;

        // 수직이동이 가능한 상태면
        if(isVerticalMovable)
        {
            inputPos.x = 0.0f;
        }

        // 조이스틱 이미지 위치를 설정합니다.
        _VirtualJSThumbImage.rectTransform.anchoredPosition = inputPos;

        inputAxis = new Vector3(inputPos.x / _JSRadius, 0.0f, inputPos.y / _JSRadius);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        isInput = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 조이스틱 이미지를 중심위치로 이동시킵니다.
        _VirtualJSThumbImage.rectTransform.anchoredPosition = inputAxis = Vector2.zero;
        isInput = false;
    }
}
