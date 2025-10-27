using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScene : MonoBehaviour
{
    public TextMeshProUGUI[] selectTexts;
    public TextMeshProUGUI selectIndexText;
    public SpriteRenderer BGAlphaSprite;
    public int selectIndex;

    public void Start()
    {
        selectIndex = 0;
        BGAlphaSprite.color = new Color(BGAlphaSprite.color.r, BGAlphaSprite.color.g, BGAlphaSprite.color.b, 0.7f);
        StartCoroutine(BGAlpha());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectIndex--;
            if (selectIndex < 0) selectIndex = selectTexts.Length - 1;
            Select();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectIndex++;
            if (selectIndex >= selectTexts.Length) selectIndex = 0;
            Select();
        }
    }

    public void Select()
    {
        for (int i = 0; i < selectTexts.Length; i++)
        {
            if (i == selectIndex) selectTexts[i].color = new Color(0.72f, 0.1f, 0.1f);
            else selectTexts[i].color = new Color(0.7f, 0.7f, 0.7f);
        }

        selectIndexText.transform.position = new Vector3(selectIndexText.transform.position.x, selectTexts[selectIndex].transform.position.y + 2f, selectIndexText.transform.position.z);
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