using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InteractablePlayMusic : Interact
{
    [Header("CaveMusicQiz")]
    [SerializeField] private CaveMusicQiz caveMusicQiz;


    // 음악 퀴즈를 재생합니다.
    IEnumerator PlayMusic()
    {
        switch(caveMusicQiz.QizNum)
        {
            case 1:
                {
                    SoundManager.Instance.Play("Prefabs/Sounds/Syllable2", Sound.Effect);
                    break;   
                }
            case 2:
                {
                    SoundManager.Instance.Play("Prefabs/Sounds/Syllable3", Sound.Effect);
                    break;
                }
            case 3:
                {
                    SoundManager.Instance.Play("Prefabs/Sounds/Syllable5", Sound.Effect);
                    break;
                }
        }

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
