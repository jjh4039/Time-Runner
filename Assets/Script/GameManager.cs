using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public float timeRemaining = 5f;

    [Header("Script")]
    public Password password;

    [Header("Time & UI")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI guideText;
    public TextMeshProUGUI subGuideText;
    public CanvasGroup guideTextAlpha;
    public GameObject signPrefab;
    public RectTransform rectParent;

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
        StartCoroutine("Sign");

        switch (index)
        {
            case 0:
                upTime = 4.5f;
                break;
            case 1:
                upTime = 5f;
                break;
            case 2:
                upTime = 6f;
                break;
            case 3:
                upTime = 12f;
                password.StartCoroutine("CheckPassword");
                break;
            default:
                upTime = 0;
                break;
        }
        subGuideText.text = "+" + upTime.ToString("F1") + "s";

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
            case 2:
                guideText.text = "Grapple Hook";
                break;
            case 3:
                guideText.text = "Password";
                break;
            default:
                guideText.text = "";
                break;
        }
        StartCoroutine("GuideAlpha");
    }
    IEnumerator Sign()
    {
        Instantiate(signPrefab, Vector2.zero, Quaternion.identity).transform.SetParent(rectParent, false);
        yield return new WaitForSeconds(0.01f);
    }

    IEnumerator GuideAlpha()
    {
        for (int i = 30; i > 0; i--)
        {
            guideTextAlpha.alpha += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        guideTextAlpha.alpha = 1;
        yield return new WaitForSeconds(1f);

        for (int i = 50; i > 0; i--)
        {
            guideTextAlpha.alpha -= 0.03f;
            yield return new WaitForSeconds(0.01f);
        }
    }

}
