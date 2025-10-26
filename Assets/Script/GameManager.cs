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
    public float heart;
    public float finalPerFloat;
    public bool isPerfect;
    public bool isTime;
    public bool isFinal;
    public float timeMagnification = 1f;

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
    public TextMeshProUGUI switchText;
    public TextMeshProUGUI subSwitchText;
    public TextMeshProUGUI finalPerText;
    public CanvasGroup switchTextAlpha;
    public CanvasGroup keyGuideAlpha;
    public CanvasGroup guideTextAlpha;
    public CanvasGroup finalPerTextAlpha;
    public GameObject signPrefab;
    public GameObject[] minusPrefab;
    public RectTransform rectParent;
    public GameObject[] finalChecker;
    public Material[] gateMaterial;
    public GameObject gateEffect;


    void Awake()
    {
        instance = this;

        isPerfect = false;
        stageIndex = 0;
        stageColor = new Color(1f, 0.5f, 0.5f); // Red
    }

    void Update()
    {
        if (isTime) timeRemaining -= Time.deltaTime * timeMagnification;
        if (!isFinal) timeText.text = timeRemaining.ToString("F1");
        else timeText.text = "♥ : " + heart;

        // 최종 스테이지 퍼센트 계산
        if (finalPerTextAlpha.alpha >= 1f && player.transform.position.x >= finalChecker[0].transform.position.x)
        {
            float goal = finalChecker[0].transform.position.x - finalChecker[1].transform.position.x;
            float playerStartDistance = finalChecker[0].transform.position.x - player.transform.position.x;

            float goalDistance = Mathf.Abs(finalChecker[0].transform.position.x - finalChecker[1].transform.position.x);
            float movedDistance = Mathf.Abs(finalChecker[0].transform.position.x - player.transform.position.x);

            finalPerFloat = (movedDistance / goalDistance) * 100f;
            if (finalPerFloat >= 100f) finalPerFloat = 100f;

            finalPerText.text = finalPerFloat.ToString("F1") + "%";
        }
    }

    IEnumerator TimeDown(float downTime)
    {

        // 텍스트 생성 (Damage)
        if (!isFinal) Instantiate(minusPrefab[(int)downTime], new Vector2(UnityEngine.Random.Range(0f, 0f), 480f), Quaternion.identity).transform.SetParent(rectParent, false);

        // 시간 감소
        timeText.color = Color.red;
        if (!isFinal)
            for (int i = 10; i > 0; i--)
            {
            GameManager.instance.timeRemaining -= downTime / 10;
            yield return new WaitForSeconds(0.01f);
            }
        else
        {
            GameManager.instance.heart -= 1;
        }
        timeText.color = Color.white;
    }

    IEnumerator TimeUp(int index)
    {
        float upTime = 0;
        Guide(index);
        StartCoroutine("Sign");

        switch (stageManager.currentLevel)
        {
            case 0: // 레벨1 시간
                switch (index)
                {
                    case 0:
                        upTime = 5f;
                        break;
                    case 1:
                        upTime = 7f;
                        break;
                    case 2:
                        upTime = 7f;
                        break;
                    case 3:
                        upTime = 14f;
                        password.StartCoroutine("CheckPassword");
                        break;
                    case 4:
                        upTime = 14f;
                        break;
                    case 5:
                        upTime = 9f;
                        break;
                    case 6:
                        upTime = 12f;
                        break;
                    case 7:
                        upTime = 9f;
                        break;
                    default:
                        upTime = 0;
                        break;
                }
                break;
            case 1: // 레벨2 시간
                switch (index)
                {
                    case 0:
                        upTime = 16f;
                        break;
                    case 1:
                        upTime = 18f;
                        break;
                    case 2:
                        upTime = 14f;
                        break;
                    case 3:
                        upTime = 14f;
                        break;
                    case 4:
                        upTime = 18f;
                        break;
                    default:
                        upTime = 0;
                        break;
                }
                break;
            case 2: // 레벨3 시간
                switch (index)
                {
                    case 0:
                        upTime = 42f;
                        break;
                    case 1:
                        upTime = 38f;
                        break;
                    case 2:
                        upTime = 44f;
                        break;
                    default:
                        upTime = 0;
                        break;
                }
                break;
        }
        
        timeGuideText.text = "+" + upTime.ToString("F1") + "s";

        timeText.color = stageColor;
        if (stageManager.currentLevel == 1) timeText.color = new Color(1f, 0.7f, 1f);  // 레벨2 임시
        if (stageManager.currentLevel == 2) timeText.color = new Color(0.3f, 0.6f, 1f);  // 레벨3 임시

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
        if (stageManager.currentLevel == 1) titleText.color = new Color(1f, 0.7f, 1f, 0.2f); // 레벨2 임시
        if (stageManager.currentLevel == 2) titleText.color = new Color(0.3f, 0.5f, 1f, 0.2f); // 레벨3 임시
        titleText.color = new Color(titleText.color.r, titleText.color.g,titleText.color.b, 0.2f);
        timeGuideText.color = titleText.color;
        switch (stageManager.currentLevel)
        {
            case 0: // 레벨1
                switch (index)
                {
                    case 0:
                        titleText.text = "Only Run";
                        guideText.text = "오로지 달리세요";
                        keyGuideText.text = $"스테이지 기믹 : [ ]";
                        break;
                    case 1:
                        titleText.text = "Triple Jump";
                        guideText.text = "3번 점프하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ ]";
                        break;
                    case 2:
                        titleText.text = "Grapple Hook";
                        guideText.text = "와이어를 연결하여 돌파하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Wire</color> ]";
                        break;
                    case 3:
                        titleText.text = "Password";
                        guideText.text = "비밀번호를 입력하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Password</color> ]";
                        break;
                    case 4:
                        titleText.text = "Lazer Quiver";
                        guideText.text = "돌파하고, 파괴하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Attack</color> ]";
                        break;
                    case 5:
                        titleText.text = "Logic Gate";
                        guideText.text = "스위치를 조작하여 돌파하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Switch</color> ]";
                        break;
                    case 6:
                        titleText.text = "Zip-Line";
                        guideText.text = "집라인을 탑승하여 돌파하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Zip-Line</color> ]";
                        break;
                    case 7:
                        titleText.text = "Shift-Gate";
                        guideText.text = "게이트를 찾고, 빠르게 이동하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Gate</color> ]";
                        break;
                    default:
                        titleText.text = "";
                        break;
                }
                break;
            case 1: // 레벨2
                switch (index)
                {
                    case 0:
                        titleText.text = "Wire Action";
                        guideText.text = "와이어를 연결하고, 연속으로 점프하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Wire</color> ]";
                        break;
                    case 1:
                        titleText.text = "Logic Leap";
                        guideText.text = "스위치로 벽과 발판을 조작하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Switch</color> ]";
                        break;
                    case 2:
                        titleText.text = "Control Circuit";
                        guideText.text = "스위치를 조작하여 집라인을 유지하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Switch, Zip-Line</color> ]";
                        break;
                    case 3:
                        titleText.text = "Phase Track";
                        guideText.text = "게이트로 이동하고, 와이어를 탑승하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Gate, Wire</color> ]";
                        break;
                    case 4:
                        titleText.text = "S.Z.L.";
                        guideText.text = "다양한 집라인에 탑승하세요";
                        keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Zip-Line</color> ]";
                        break;
                }
                break;
            
            case 2:
            switch (index)
            {
                case 0:
                    titleText.text = ".Zip";
                    guideText.text = "집라인을 극한으로 활용하세요";
                    keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Zip-Line, Switch</color> ]";
                    break;
                case 1:
                    titleText.text = "Shift Road";
                    guideText.text = "끊임없이 게이트를 찾아 이동하세요";
                    keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Gate, Zip-Line, Switch, Wire</color> ]";
                    break;
                case 2:
                    titleText.text = "Switch.";
                    guideText.text = "오직 스위치지만.. 집중하세요";
                    keyGuideText.text = $"스테이지 기믹 : [ <color=#{colorCode}>Switch</color> ]";
                    break;
            }
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
            timeRemaining += 0.5f;
            db.perfectCount += 1;
            yield return new WaitForSeconds(0f);
        }
        isPerfect = true;
    }

    public IEnumerator SwitchAlpha(int index)
    {
        switch (index)
        {
           case 0:
                switchText.text = "시간의 흐름이 빨라집니다.";
                subSwitchText.text = "x1.2";
                break;
            case 1:
                switchText.text = "시간이 끝을 향해 흐릅니다..";
                subSwitchText.text = "x1.5";
                break;
            case 2:
                switchText.text = "시간의 진실에 도달했습니다.";
                subSwitchText.text = "♥ = 10s";
                break;
            default:
                break;
        }

        switchTextAlpha.alpha = 0;
        yield return new WaitForSeconds(0.2f);

        for (int i = 40; i > 0; i--)
        {
            switchTextAlpha.alpha += 0.03f;
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 50; i > 0; i--)
        {
            switchTextAlpha.alpha -= 0.03f;
            yield return new WaitForSeconds(0.02f);
        }
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
        string OriginalFormat = "[ <color=#{0}>D</color> ] 키로 다시 달리기";
        string hexColorCode = ColorUtility.ToHtmlStringRGB(stageColor);
        string newText = string.Format(OriginalFormat, hexColorCode);

        restartText.text = newText;
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
    public IEnumerator ZipLineCameraOffset(bool isZip)
    {
        if (isZip)
            cinemachineFollow.FollowOffset.y -= 1f;
        else
        {
            for (int i = 0; i < 20; i++)
            {
                cinemachineFollow.FollowOffset.y += 0.05f;
                yield return new WaitForFixedUpdate();
            }
        }

        yield return new WaitForFixedUpdate();
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
