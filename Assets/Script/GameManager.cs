using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Main")]
    static public GameManager instance;
    static public int stageIndex;
    static public int stageNumber;
    static public Color stageColor;
    public float timeRemaining;
    public bool isPerfect;

    [Header("Script")]
    public CinemachineCamera cinemachine;
    public CinemachineFollow cinemachineFollow;
    public Player player;
    public PlayerLight playerLight;
    public Password password;
    public DB db;
    public StageManager stageManager;

    [Header("Time & UI")]
    public CanvasGroup screenAlpha;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI guideText;
    public TextMeshProUGUI keyGuideText;
    public TextMeshProUGUI timeGuideText;
    public TextMeshProUGUI restartText;
    public CanvasGroup keyGuideAlpha;
    public CanvasGroup guideTextAlpha;
    public GameObject signPrefab;
    public GameObject[] minusPrefab;
    public RectTransform rectParent;

    void Awake()
    {
        instance = this;

        isPerfect = false;
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
            case 4:
                upTime = 12f;
                break;
            case 5:
                upTime = 7f;
                break;
            case 6:
                upTime = 13f;
                break ;
            case 7:
                upTime = 8f;
                break;
            default:
                upTime = 0;
                break;
        }
        timeGuideText.text = "+" + upTime.ToString("F1") + "s";

        timeText.color = stageColor;
        if (stageManager.currentLevel == 1) timeText.color = new Color(1f, 0.7f, 1f);  // 레벨2 임시;
        for (int i = 20; i > 0; i--)
        {
            timeRemaining += upTime / 20;
            yield return new WaitForSeconds(0.02f);
        }
        timeText.color = Color.white;
    }

    void Guide(int index)
    {
        string colorCode = ColorUtility.ToHtmlStringRGB(stageColor);
        titleText.color = stageColor;
        if (stageManager.currentLevel == 1) titleText.color = new Color(1f, 0.7f, 1f, 0.2f); // 레벨2 임시;
        titleText.color = new Color(titleText.color.r, titleText.color.g,titleText.color.b, 0.2f);
        timeGuideText.color = titleText.color;

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
            case 4:
                titleText.text = "Lazer Quiver";
                guideText.text = "돌파하고, 파괴하세요";
                keyGuideText.text = $"공격 / 연속 공격 : [ <color=#{colorCode}>Space</color> ]";
                break;
            case 5:
                titleText.text = "Logic Gate";
                guideText.text = "스위치를 조작하여 돌파하세요";
                keyGuideText.text = $"스위치 ON : [ <color=#{colorCode}>W</color> ]\n" +
                                    $"스위치 OFF : [ <color=#{colorCode}>S</color> ]";
                break;
            case 6:
                titleText.text = "Zip-Line";
                guideText.text = "집라인을 탑승하여 돌파하세요";
                keyGuideText.text = $"집라인 해제 : [ <color=#{colorCode}>Space</color> ]";
                break;
            case 7:
                titleText.text = "Shift-Gate";
                guideText.text = "게이트를 찾고, 빠르게 이동하세요";
                keyGuideText.text = $"텔레포트 : [ <color=#{colorCode}>Shift</color> ]";
                break;
            default:
                titleText.text = "";
                break;
        }
        StartCoroutine("GuideAlpha");
    }

    // 판정검사
    IEnumerator Sign() 
    {
        if (isPerfect == true)
        {
            Instantiate(signPrefab, Vector2.zero, Quaternion.identity).transform.SetParent(rectParent, false);
            db.continuePerfect++;
            yield return new WaitForSeconds(0f);
        }
        isPerfect = true;
    }

    public IEnumerator GuideAlpha()
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

    public IEnumerator RestartText()
    {
        while (player.isRestart)
        {
            restartText.alpha = 0.7f;
            for (int i = 0; i < 30; i++)
            {
                if (!player.isRestart) break;
                restartText.alpha -= 0.015f;
                yield return new WaitForSeconds(0.01f);
            }
            for (int i = 0; i < 30; i++)
            {
                if (!player.isRestart) break;
                restartText.alpha += 0.015f;
                yield return new WaitForSeconds(0.01f);
            }
        }

        restartText.alpha = 0f;
    }

    public IEnumerator AttackCameraZoom(int attackindex)
    {
        bool stopNow = false;

        switch (attackindex)
        {
            case 0:
                float zoom = 3f - cinemachine.Lens.OrthographicSize;
                float offX = 6.4f - cinemachineFollow.FollowOffset.x;
                float offY = 0.1f - cinemachineFollow.FollowOffset.y;

                // 카메라 줌 기본값 원위치
                for (int i = 0; i < 10; i++)
                {
                    cinemachine.Lens.OrthographicSize += zoom / 10;
                    cinemachineFollow.FollowOffset.x += offX / 10;
                    cinemachineFollow.FollowOffset.y += offY / 10;
                    if (stopNow == true) break;
                    yield return new WaitForSeconds(0.01f);
                }
                // 카메라 값 초기화
                cinemachine.Lens.OrthographicSize = 3f;
                cinemachineFollow.FollowOffset.x = 6.4f;
                cinemachineFollow.FollowOffset.y = 0.1f;
                break;  
            case 1:
                // 카메라 값 초기화
                stopNow = true;

                for (int i = 0; i < 20; i++)
                {
                    cinemachine.Lens.OrthographicSize -= 0.005f;
                    cinemachineFollow.FollowOffset.x -= 0.115f;
                    yield return new WaitForSeconds(0.01f);
                }

                stopNow = false;
                break;
            case 2:
                for (int i = 0; i < 20; i++)
                {
                    cinemachine.Lens.OrthographicSize -= 0.003f;
                    cinemachineFollow.FollowOffset.x -= 0.04f;
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 3:
                for (int i = 0; i < 20; i++)
                {
                    cinemachine.Lens.OrthographicSize -= 0.003f;
                    cinemachineFollow.FollowOffset.x -= 0.045f;
                    yield return new WaitForSeconds(0.01f);
                }
                break;
        }
    }
}
