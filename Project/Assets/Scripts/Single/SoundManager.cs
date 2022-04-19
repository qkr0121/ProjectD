using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ManagerClassBase<SoundManager>
{
    private AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();


    /// <summary>
    /// 오브젝트 초기화
    /// - Sounds 오브젝트 없으면 Sounds 오브젝트와 하위에 BGM, Effect 오브젝트를 만듭니다.
    /// </summary>
    public void Init()
    {
        GameObject root = GameObject.Find("Sounds");

        if (root == null)
        {
            root = new GameObject { name = "Sounds" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Sound));
            for(int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
        }

        // BGM은 무한반복
        _audioSources[(int)Sound.Bgm].loop = true;
    }

    public void Clear()
    {
        // 재생기 전부 스탑, clip 초기화
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        // 효과음 Dictionary를 비웁니다.
        _audioClips.Clear();
    }

    // AudioClip 을 받아 재생합니다.
    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        // Clip 이 비었으면 실행하지 않습니다.
        if(audioClip == null)
            return;
        
        // BGM 을 재생할 시
        if(type == Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Sound.Bgm];
            // 다른 sound 가 실행 되고 있다면 중지시킵니다.
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        // Effect 를 재생할 시
        else
        {
            AudioSource audioSource = _audioSources[(int)Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
        
    }

    // 파일의 Path 를 받아 재생합니다.
    public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    private AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect)
    {
        AudioClip audioClip = null;

        // BGM 클립 붙이기
        if(type == Sound.Bgm)
        {
            audioClip = ResourceManager.Instance.LoadResource<AudioClip>(path);
        }
        // Effect 클립 붙이기
        else
        {
            // _audioClips에 Clip 이 있는지 확인합니다.(없으면 새로 생성해서 _audioClip에 저장한다.)
            if(_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = ResourceManager.Instance.LoadResource<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }


        return audioClip;
    }

    public override void InitializeManagerClass()
    {
        throw new System.NotImplementedException();
    }
}
