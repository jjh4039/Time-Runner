using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class Trigger : MonoBehaviour
{
    public int triggerNumber;
    [HideInInspector] public BoxCollider2D boxCollider2D;
    public Transform respawnPoint;
    public GameObject respawnObject;



    public void Awake()
    {
        switch (triggerNumber)
        {
            case 1: // 리스폰 지점 설정
                boxCollider2D = GetComponent<BoxCollider2D>();
                respawnPoint = respawnObject.transform;
                break;
            default:
                boxCollider2D = GetComponent<BoxCollider2D>();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            trigger();
        }
    }

    public void trigger()
    {
        switch (triggerNumber)
        {
            case 0: // 비밀번호 검사 시작
                GameManager.instance.password.StartCoroutine("FinalPassword");
                gameObject.SetActive(false);
                break;
            case 1: // 리스폰 & 2초 감소 (리스폰, 낙사)
                StartCoroutine("TriggerOne");
                GameManager.instance.isPerfect = false;
                break;
            case 2: // 1초 감소 및 삭제 (비밀번호 실패)
                GameManager.instance.StartCoroutine("TimeDown", 1f);
                GameManager.instance.isPerfect = false;
                gameObject.SetActive(false);
                break;
            case 3: // 1초 감소 (화살 피격)
                GameManager.instance.StartCoroutine("TimeDown", 1f);
                GameManager.instance.isPerfect = false;
                break;
            case 4: // 화살 스테이지 시작 
                break;
        }
    }

    // 카메라 이동 및 리스폰, 페이드 인/아웃 
    IEnumerator TriggerOne()
    {
        GameManager.instance.player.isMove = false;

        for (int i = 0; i < 20; i++)
        {
            GameManager.instance.screenAlpha.alpha += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        GameManager.instance.StartCoroutine("TimeDown", 2f);
        GameManager.instance.player.rigid.linearVelocity = Vector2.zero;
        GameManager.instance.player.moveInput = Vector2.zero;
        GameManager.instance.player.transform.position = respawnPoint.position;
        GameManager.instance.cinemachine.ForceCameraPosition(respawnPoint.position, Quaternion.identity);
        GameManager.instance.screenAlpha.alpha = 1f;

        for (int i = 0; i < 20; i++)
        {
            GameManager.instance.screenAlpha.alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        GameManager.instance.screenAlpha.alpha = 0f;
        GameManager.instance.player.isMove = true;
    }
}
