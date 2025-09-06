using System;
using System.Collections;
using System.Collections.Generic;
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

    private void trigger()
    {
        switch (triggerNumber)
        {
            case 0: // 문 검사 시작
                GameManager.instance.password.StartCoroutine("FinalPassword");
                gameObject.SetActive(false);
                break;
            case 1: // 리스폰 & 2초 감소
                GameManager.instance.player.transform.position = respawnPoint.position;
                GameManager.instance.cine.transform.position = new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y, -10);
                GameManager.instance.StartCoroutine("TimeDown", 2f);
                StartCoroutine("PlayerMoveStop", 0.1f);
                break;
        }
    }

    private void MoveCamera()
    {

    }

    IEnumerator PlayerMoveStop(float time)
    {
        GameManager.instance.player.isMove = false;
        GameManager.instance.player.rigid.linearVelocity = Vector2.zero;
        GameManager.instance.player.moveInput = Vector2.zero;
        yield return new WaitForSeconds(time);
        GameManager.instance.player.isMove = true;
    }
}
