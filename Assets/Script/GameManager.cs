using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public float timeRemaining = 5f;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI guideText;
    public CanvasGroup guideTextAlpha;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        timeText.text = timeRemaining.ToString("F1");
    }

    IEnumerator TimeUp(int index)
    {
        float upTime = 0;
        Guide(index);

        switch (index)
        {
            case 0:
                upTime = 20;
                break;
            case 1:
                upTime = 30;
                break;
            default:
                upTime = 0;
                break;
        }

        timeText.color = Color.softRed;
        for (int i = 20; i > 0; i--)
        {
            GameManager.instance.timeRemaining += upTime / 20;
            yield return new WaitForSeconds(0.02f);
        }
        timeText.color = Color.white;
    }

    void Guide(int index)
    {
        switch (index)
        {
            case 0:
                guideText.text = "Only Run";
                break;
            case 1:
                guideText.text = "Triple Jump";
                break;
            default:
                guideText.text = "";
                break;
        }
        StartCoroutine("GuideAlpha");
    }

    IEnumerator GuideAlpha()
    {
        guideTextAlpha.alpha = 1;
        yield return new WaitForSeconds(2f);

        for (int i = 50; i > 0; i--)
        {
            guideTextAlpha.alpha -= 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
    }

}
