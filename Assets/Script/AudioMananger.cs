using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class AudioMananger : MonoBehaviour
{
    public static AudioMananger instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public AudioClip bgmClip2;
    public float bgmVolume;
    public AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    public AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Select, Jump, Fall, Wire, ON, OFF, Sword, Teleport, Die, Zip, Buy, NoBuy, Typing, LevelUp, Heart, Start }

    void Awake()
    {
        instance = this;

        Init();
    }

    void Init()
    {
        // 배경음
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayers");
        sfxObject.transform.parent = transform;

        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }


    public void PlayBgm(bool isPlay, float Volume)
    {
        if (isPlay)
        {
            bgmPlayer.volume = Volume;
            bgmPlayer.Play();
        }
        else
            bgmPlayer.Stop();
    }

    public int PlaySfx(Sfx sfx, float Volume, float Pitch)
    {
        for (int index = 0; index <sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].volume = Volume;
            sfxPlayers[loopIndex].pitch = Pitch;
            sfxPlayers[loopIndex].Play();

            return loopIndex;          
        }
        return 0;
    }

    public IEnumerator QuitBGM()
    {
        while (bgmPlayer.volume > 0)
        {
            bgmPlayer.volume -= 0.01f;
            yield return new WaitForSeconds(0.03f);
        }
        bgmPlayer.Stop();
    }
}
