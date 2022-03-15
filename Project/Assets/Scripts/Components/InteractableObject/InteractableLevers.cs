using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLevers : Interact
{
    [Header("레버와 연결된 바위들")]
    [SerializeField] private List<GameObject> Rocks;

    [Header("카메라 위치")]
    [SerializeField] private Transform _CameraPosition;

    [Header("InteractJoyStick")]
    [SerializeField] private VirtualJoyStick _InteractionJoyStick;

    // 현재 레버와 연결된 돌 번호
    private int RockNum;

    // PlayeraCharacter 를 나타냅니다.
    private PlayerCharacter _PlayerCharacter;

    // 레버 사용중을 나타냅니다
    private bool usingLever = false;

    private void Awake()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
    }

    private void Start()
    {
        RockNum = 0;
    }

    private void Update()
    {
        UseLever();

        if (!_InteractionJoyStick.isInput && usingLever == true)
        {
            StartCoroutine("StopLever");
        }
        else
            StopCoroutine("StopLever");

    }

    // 레버를 움직입니다.
    private void UseLever()
    {
        // 레버를 위로 당기면 왼쪽으로 아래로 당기면 오른쪽으로 바위를 움직입니다.
        Rocks[RockNum].transform.position += Rocks[RockNum].transform.right * (-1.0f)
            * PlayerManager.Instance.gameUI[JoyStickType.Interact].inputAxis.z
            * Time.deltaTime;
        
    }

    // 레버의 움직임이 2초 이상 멈추면 상호작용을 종료하고
    // 레버를 다음 바위와 연결합니다.
    IEnumerator StopLever()
    {
        yield return new WaitForSeconds(2.0f);

        if(usingLever)
        {
            RockNum++;
            if (Rocks.Count <= RockNum)
                RockNum = 0;
            usingLever = false;
        }

        InputManager.Instance.InitInteractJoyStick();
        FinishInteraction();
    }

    public override void Interaction()
    {
        // 상호작용 키 변경
        InputManager.Instance.InteractJoyStickToVerticalMove();

        // 카메라의 위치를 옮깁니다.
        _PlayerCharacter.trackingCamera.MoveToViewPosition(_CameraPosition);

        // 레버를 사용중으로 변경합니다.
        usingLever = true;
    }

    public override bool UseInteractionKey()
    {
        return true;
    }
}
