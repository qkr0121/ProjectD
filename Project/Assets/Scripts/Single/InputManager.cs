using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : ManagerClassBase<InputManager>
{
    // 상호작용 조이스틱을 나타냅니다.
    private VirtualJoyStick _InteractJoyStick;

    public override void InitializeManagerClass() 
    {
        _InteractJoyStick = PlayerManager.Instance.gameUI.interactJoyStick;
    }

    // 상호작용 조이스틱이 상하로만 움직입니다.
    public void InteractJoyStickToVerticalMove()
    {
        _InteractJoyStick.isVerticalMovable = true;
    }

    // 상호작용 조이스틱을 활성화 합니다.
    public void SetActiveInteractJoyStick()
    {
        PlayerManager.Instance.gameUI.interactJoyStick.gameObject.SetActive(true);
    }

    // 상호작용 조이스틱을 비활성화 합니다.
    public void SetDisableInteractJoyStick()
    {
        PlayerManager.Instance.gameUI.interactJoyStick.gameObject.SetActive(false);
    }

}
