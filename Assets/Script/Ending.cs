using KoreanTyper;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public GameObject backGroundSprite;
    public CanvasGroup screenAlpha;
    public TextMeshProUGUI[] Texts;
    public TextMeshProUGUI madeBy;

    public string[] playerInfo;
    public int playTime;

    private void Start()
    {
        madeBy.alpha = 0f;

        // 데이터 초기화
        playTime = (int)DB.instance.playTime;
        int hours = playTime / 3600;
        int minutes = (playTime % 3600) / 60;
        playerInfo = new string[3];

        playerInfo[0] = $"{hours}h {minutes}m";
        playerInfo[1] = $"{DB.instance.clearCount}";
        playerInfo[2] = $"{DB.instance.perfectCountSave * 100 / DB.instance.clearCount}";

        StartCoroutine(bgMove());
        StartCoroutine(EndingStart());
    }

    public IEnumerator bgMove()
    {
        while (true)
        {
            backGroundSprite.transform.position += new Vector3(0, 0.0065f, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator EndingStart()
    {
        foreach (TextMeshProUGUI t in Texts)
            t.text = "";

        screenAlpha.alpha = 1;

        for (int i = 0; i <= 100; i++)
        {
            screenAlpha.alpha -= 0.01f;
            yield return new WaitForSeconds(0.03f);
            if (i == 50) AudioMananger.instance.PlayBgm(true, 0.1f);
        }
        
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(TypingText());
    }

    public IEnumerator TypingText()
    {

        string[] strings = new string[6]{ "Time-Runner",
                                              "제작기간 2개월",
                                              "PlayTime : " + playerInfo[0],
                                               "Clear Stages : " + playerInfo[1],
                                                "Perfect Clear : " + playerInfo[2] + "%",
                                                "Thanks For Playing!" };

        for (int t = 0; t < Texts.Length && t < strings.Length; t++)
        {
            int strTypingLength = strings[t].GetTypingLength();

            int tmp = AudioMananger.instance.PlaySfx(AudioMananger.Sfx.Typing, 0.8f, 1f);
            AudioMananger.instance.sfxPlayers[tmp].loop = true;

            for (int i = 0; i <= strTypingLength; i++)
            {
                Texts[t].text = strings[t].Typing(i);
                if (i == 0) new WaitForSeconds(0.25f);
                else yield return new WaitForSeconds(0.1f);
            }
            AudioMananger.instance.sfxPlayers[tmp].Stop();
            yield return new WaitForSeconds(0.8f);
        }

        for (int i = 0; i <= 50; i++)
        {
            madeBy.alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(4f);

        for (int i = 0; i <= 100; i++)
        {
            screenAlpha.alpha += 0.01f;
            yield return new WaitForSeconds(0.02f);
        }

        AudioMananger.instance.StartCoroutine("QuitBGM");
    }
}
