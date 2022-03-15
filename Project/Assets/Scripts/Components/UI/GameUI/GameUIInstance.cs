using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameUIInstance : MonoBehaviour
{
    [Header("이동 조이스틱")]
    [SerializeField] private VirtualJoyStick _MoveJoyStick;

    [Header("상호작용 조이스틱")]
    [SerializeField] private VirtualJoyStick _InteractJoyStick;

    // 인덱서
    public VirtualJoyStick this[JoyStickType joyStickType] 
        => (joyStickType == JoyStickType.Move) ? _MoveJoyStick : _InteractJoyStick;

}
