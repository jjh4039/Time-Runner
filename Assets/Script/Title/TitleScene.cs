using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public GameObject cameraMove;
    public CanvasGroup alphaScreen;
    public TextMeshProUGUI[] selectTexts;
    public TextMeshProUGUI selectIndexText;
    public TextMeshProUGUI guideText;
    public SpriteRenderer BGAlphaSprite;
    public SpriteRenderer[] titleText;
    public bool isWarp;
    public int selectIndex;
    public AsyncOperation asyncOp;



    public void Start()
    {
        isWarp = false;
        alphaScreen.alpha = 1f;
        cameraMove.transform.position = new Vector3(-0.3f, 8f, -10f);
        selectIndex = 0;
        BGAlphaSprite.color = new Color(BGAlphaSprite.color.r, BGAlphaSprite.color.g, BGAlphaSprite.color.b, 0.7f);
        asyncOp = SceneManager.LoadSceneAsync(2);
        asyncOp.allowSceneActivation = false;
        StartCoroutine(BGAlpha());
        StartCoroutine(StartEvent());

        AudioMananger.instance.PlayBgm(true, 0.4f);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && isWarp == true) {
            switch(selectIndex)
            {
                case 0:
                    StartCoroutine(GameStart());
                    if (isWarp) AudioMananger.instance.PlaySfx(AudioMananger.Sfx.Select, 1.5f, 0.4f);
                    isWarp = false;
                    break;
                case 1:
                    // ∞°¿ÃµÂ
                    break;
                case 2:
                    Application.Quit();
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectIndex--;
            if (selectIndex < 0) selectIndex = selectTexts.Length - 1;
            if (isWarp) AudioMananger.instance.PlaySfx(AudioMananger.Sfx.Select, 0.7f, 1f);
            Select();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectIndex++;
            if (selectIndex >= selectTexts.Length) selectIndex = 0;
            if (isWarp) AudioMananger.instance.PlaySfx(AudioMananger.Sfx.Select, 0.7f, 1f);
            Select();
        }
    }

    IEnumerator GameStart()
    {
        alphaScreen.alpha = 0f;

        for (int i = 0; i < 40; i++)
        {
            alphaScreen.alpha += 0.025f;
            yield return new WaitForSeconds(0.03f);
        }

        asyncOp.allowSceneActivation = true;
    }

    public void Select()
    {
        for (int i = 0; i < selectTexts.Length; i++)
        {
            if (i == selectIndex) selectTexts[i].color = new Color(0.72f, 0.1f, 0.1f, selectTexts[i].alpha);
            else selectTexts[i].color = new Color(0.7f, 0.7f, 0.7f, selectTexts[i].alpha);
        }

        selectIndexText.transform.position = new Vector3(selectIndexText.transform.position.x, selectTexts[selectIndex].transform.position.y + 2f, selectIndexText.transform.position.z);
    }

    IEnumerator StartEvent()
    {
        for (int i = 0; i < 100; i++)
        {
            alphaScreen.alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1.5f);

        for (int k = 0; k < 200; k++)
        {
            cameraMove.transform.position -= new Vector3(0f, 0.052f - k * 0.0001f, 0f);
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(0.5f);

        for (int j = 0; j < 100; j++)
        {
            titleText[0].color = new Color(titleText[0].color.r, titleText[0].color.g, titleText[0].color.b, titleText[0].color.a + 0.01f);
            titleText[1].color = new Color(titleText[1].color.r, titleText[1].color.g, titleText[1].color.b, titleText[1].color.a + 0.02f);

            yield return new WaitForSeconds(0.01f);
        }

        selectIndex = 0;

        for (int m = 0; m < 200; m++)
        {
            Select();
            selectTexts[0].color = new Color(selectTexts[0].color.r, selectTexts[0].color.g, selectTexts[0].color.b, selectTexts[0].color.a + 0.01f);
            selectTexts[1].color = new Color(selectTexts[1].color.r, selectTexts[1].color.g, selectTexts[1].color.b, selectTexts[1].color.a + 0.0075f);
            selectTexts[2].color = new Color(selectTexts[2].color.r, selectTexts[2].color.g, selectTexts[2].color.b, selectTexts[2].color.a + 0.005f);
            selectIndexText.color = new Color(selectIndexText.color.r, selectIndexText.color.g, selectIndexText.color.b, selectIndexText.color.a + 0.01f);
            guideText.color = new Color(guideText.color.r, guideText.color.g, guideText.color.b, guideText.color.a + 0.01f);

            if (m == 10) isWarp = true;

            yield return new WaitForSeconds(0.01f);
        }
        
    }

    IEnumerator BGAlpha()
    {
        int randomIndex = UnityEngine.Random.Range(0, 2);

        if (randomIndex == 0) BGAlphaSprite.color = new Color(BGAlphaSprite.color.r, BGAlphaSprite.color.g, BGAlphaSprite.color.b, 0.7f);
        
        yield return new WaitForSeconds(0.2f);

        BGAlphaSprite.color = new Color(BGAlphaSprite.color.r, BGAlphaSprite.color.g, BGAlphaSprite.color.b, 0.75f);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(BGAlpha());
    }
}