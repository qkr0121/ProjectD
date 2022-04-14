using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSign : Interact
{
    [Header("스테이지")]
    [SerializeField] private string _StageName;

    private GameObject _DialogObject;

    // 스테이지 정보를 다시 알려줍니다.
    private void StageInfoRepeat()
    {
        _DialogObject = Instantiate(
            ResourceManager.Instance.LoadResource<GameObject>("StartDialog", "Prefabs/UI/StartDialog"),
            GameObject.Find("GameUI").transform,
            true);

        Dialog dialog = _DialogObject.GetComponent<Dialog>();

        dialog._DialogName = _StageName + "StageInfo";

        FinishInteraction();
    }

    public override void Interaction()
    {
        StageInfoRepeat();
    }

    public override bool UseInteractionKey()
    {
        return true;
    }
}
