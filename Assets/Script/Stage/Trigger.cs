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
                GameManager.instance.player.isMove = false;
                GameManager.instance.player.isRestart = true;
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
                GameManager.instance.player.playerSpaceMode = Player.PlayerSpaceMode.attack;
                StartCoroutine("CameraZoom", true);
                boxCollider2D.enabled = false;
                break;
            case 5: // 화살 스테이지 종료
                GameManager.instance.player.playerSpaceMode = Player.PlayerSpaceMode.jump;
                StartCoroutine("CameraZoom", false);
                boxCollider2D.enabled = false;
                break;
        }
    }

    IEnumerator CameraZoom(bool mode)
    {
        if (mode) // 줌 인
        {
            for (int i = 0; i < 20; i++)
            {
                GameManager.instance.cinemachine.Lens.OrthographicSize -= 0.1f;
                GameManager.instance.cinemachineFollow.FollowOffset.x -= 0.08f;
                GameManager.instance.cinemachineFollow.FollowOffset.y -= 0.07f;
                yield return new WaitForSeconds(0.01f);
            }

            // 줌 상태 카메라 값
            GameManager.instance.cinemachine.Lens.OrthographicSize = 3f;
            GameManager.instance.cinemachineFollow.FollowOffset.x = 6.4f;
            GameManager.instance.cinemachineFollow.FollowOffset.y = 0.1f;
        }
        else // 줌 아웃
        {
            for (int i = 0; i < 20; i++)
            {
                GameManager.instance.cinemachine.Lens.OrthographicSize += 0.1f;
                GameManager.instance.cinemachineFollow.FollowOffset.x += 0.08f;
                GameManager.instance.cinemachineFollow.FollowOffset.y += 0.07f;
                yield return new WaitForSeconds(0.01f);
            }

            // 기본 상태 카메라 값
            GameManager.instance.cinemachine.Lens.OrthographicSize = 5f;
            GameManager.instance.cinemachineFollow.FollowOffset.x = 8f;
            GameManager.instance.cinemachineFollow.FollowOffset.y = 1.5f;
        }
    }

    // 카메라 이동 및 리스폰, 페이드 인/아웃 
    IEnumerator TriggerOne()
    {
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
        GameManager.instance.StartCoroutine("RestartText");

        for (int i = 0; i < 20; i++)
        {
            GameManager.instance.screenAlpha.alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        GameManager.instance.screenAlpha.alpha = 0f;
    }
}
