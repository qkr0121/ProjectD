using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameUIInstance : MonoBehaviour
{
    [Header("상호작용 조이스틱")]
    [SerializeField] private VirtualJoyStick _InteractJoyStick;

    [Header("상호작용 키 UI")]
    [SerializeField] private GameObject _InteractionUI;

    public VirtualJoyStick interactJoyStick => _InteractJoyStick;

    public GameObject interactUI => _InteractionUI;
}
