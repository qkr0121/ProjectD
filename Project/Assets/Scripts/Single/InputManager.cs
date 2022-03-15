using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : ManagerClassBase<InputManager>
{
    // 상호작용 조이스틱을 나타냅니다.
    private VirtualJoyStick _InteractJoyStick;

    public override void InitializeManagerClass() 
    {
        _InteractJoyStick = PlayerManager.Instance.gameUI[JoyStickType.Interact];
    }

    // 상하작용 조이스틱이 클릭만 됩니다.(움직일 수 없음, 기본상태)
    public void InitInteractJoyStick()
    {
        _InteractJoyStick.isVerticalMovable = false;
    }

    // 상호작용 조이스틱이 상하로만 움직입니다.
    public void InteractJoyStickToVerticalMove()
    {
        _InteractJoyStick.isVerticalMovable = true;
    }

    // 상호작용 조이스틱을 활성화 합니다.
    public void SetActiveInteractJoyStick()
    {
        PlayerManager.Instance.gameUI[JoyStickType.Interact].GetComponent<VirtualJoyStick>().enabled = true;
    }

    // 상호작용 조이스틱을 비활성화 합니다.
    public void SetDisableInteractJoyStick()
    {
        PlayerManager.Instance.gameUI[JoyStickType.Interact].GetComponent<VirtualJoyStick>().enabled = false;
    }

}
