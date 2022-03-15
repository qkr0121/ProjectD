using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCaveButton : Interact
{
    [Header("계이름")]
    [SerializeField] private int _SoundHigh;

    // CaveMusicQiz 컴포넌트를 나타냅니다.
    private CaveMusicQiz _CaveMusicQiz;

    // SimpleSonarShader_Object 컴포넌트를 나타냅니다.
    private SimpleSonarShader_Object _SimpleSonarShader_Object;

    private void Start()
    {
        _CaveMusicQiz = GetComponentInParent<CaveMusicQiz>();
        _SimpleSonarShader_Object = GetComponentInChildren<SimpleSonarShader_Object>();
    }

    // 해당 음을 재생시킵니다.(정답을 입력합니다.)
    IEnumerator PlayMusic()
    {
        _CaveMusicQiz.TryQiz(_SoundHigh, _SimpleSonarShader_Object);
        yield return new WaitForSeconds(0.5f);

        FinishInteraction();
    }

    public override void Interaction()
    {
       
        StartCoroutine(PlayMusic());
        
    }

    public override bool UseInteractionKey()
    {
        return true;
    }

    
}