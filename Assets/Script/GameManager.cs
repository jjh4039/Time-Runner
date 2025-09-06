using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Main")]
    static public GameManager instance;
    static public int stageIndex;
    static public Color stageColor;
    public float timeRemaining;

    [Header("Script")]
    public CinemachineCamera cine;
    public Player player;
    public Password password;
    public DB db;

    [Header("Time & UI")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI guideText;
    public TextMeshProUGUI keyGuideText;
    public TextMeshProUGUI timeGuideText;
    public CanvasGroup keyGuideAlpha;
    public CanvasGroup guideTextAlpha;
    public GameObject signPrefab;
    public GameObject[] minusPrefab;
    public RectTransform rectParent;

    void Awake()
    {
        instance = this;

        stageIndex = 0;
        stageColor = new Color(1f, 0.5f, 0.5f); // Red

    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        timeText.text = timeRemaining.ToString("F1");
    }

    IEnumerator TimeDown(float downTime)
    {

        // 텍스트 생성 (Damage)
        Instantiate(minusPrefab[(int)downTime], new Vector2(UnityEngine.Random.Range(0f, 0f), 480f), Quaternion.identity).transform.SetParent(rectParent, false);

        // 시간 감소
        timeText.color = Color.red;
        for (int i = 10; i > 0; i--)
        {
            GameManager.instance.timeRemaining -= downTime / 10;
            yield return new WaitForSeconds(0.01f);
        }
        timeText.color = Color.white;
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
        timeGuideText.text = "+" + upTime.ToString("F1") + "s";

        timeText.color = stageColor;
        for (int i = 20; i > 0; i--)
        {
            GameManager.instance.timeRemaining += upTime / 20;
            yield return new WaitForSeconds(0.02f);
        }
        timeText.color = Color.white;
    }

    void Guide(int index)
    {
        string colorCode = ColorUtility.ToHtmlStringRGB(stageColor);

        switch (index)
        {
            case 0:
                titleText.text = "Only Run";
                guideText.text = "오로지 달리세요";
                keyGuideText.text = $"달리기 : [ <color=#{colorCode}>D</color> ]";

                break;
            case 1:
                titleText.text = "Triple Jump";
                guideText.text = "3번 점프하세요";
                keyGuideText.text = $"점프 : [ <color=#{colorCode}>Space</color> ]";
                break;
            case 2:
                titleText.text = "Grapple Hook";
                guideText.text = "와이어를 연결하여 돌파하세요";
                keyGuideText.text = $"와이어 연결 : [ <color=#{colorCode}>Left Click</color> ]\n" +
                                    $"점프, 와이어 해제 : [ <color=#{colorCode}>Space</color> ]";
                break;
            case 3:
                titleText.text = "Password";
                guideText.text = "비밀번호를 입력하세요";
                keyGuideText.text = $"비밀번호 입력 : [ <color=#{colorCode}>Number Pad</color> ]";
                break;
            default:
                titleText.text = "";
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
        keyGuideAlpha.alpha = 0;

        for (int i = 30; i > 0; i--)
        {
            keyGuideAlpha.alpha += 0.05f;
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
