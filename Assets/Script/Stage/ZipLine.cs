using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ZipLine : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public GameObject zip;
    public Vector3 zipResetPos;
    public Light2D light2D;
    public CapsuleCollider2D capsuleCollider2D;
    public bool isInteraction;
    public float stopDistance = 0.5f; // 벽에 닿기 전에 멈출 거리
    public LayerMask switchLayer; // 벽 또는 장애물의 레이어
    public bool nowStop;

    public float startSpeed = 100f;
    public float maxSpeed = 400f;

    public float duration = 1f;

    private void Awake()
    {
        zipResetPos = zip.transform.position;
    }

    private void Update()
    {
        // 재시작 시 위치 초기화
        if (GameManager.instance.player.isRestart) zip.transform.position = zipResetPos;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInteraction = true;
            GameManager.instance.player.zipLine = this;
            GameManager.instance.player.isMove = false;
            GameManager.instance.player.isZip = true;
            StartCoroutine(MoveZipLine());
        }
    }

    IEnumerator MoveZipLine()
    {
        GameManager.instance.StartCoroutine("ZipLineCameraOffset", true);
        nowStop = false;
        light2D.enabled = true;
        GameManager.instance.player.speed = 12f;
        GameManager.instance.player.animator.SetBool("Zip", true);

        float elapsedTime = 0f;
        
        while (elapsedTime < duration && Vector3.Distance(zip.transform.position, new Vector3(endPos.position.x, endPos.position.y - 0.6f, endPos.position.z)) > 0.1f
            && !nowStop)
        {
            elapsedTime += Time.deltaTime;

            float normalizedTime = elapsedTime / duration;
            float currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, normalizedTime);

            Vector3 direction = (new Vector3(endPos.position.x, endPos.position.y - 0.6f, endPos.position.z) - zip.transform.position).normalized; // endPos에 도달하지 않았으므로

            // 벽 충돌 체크
            RaycastHit2D hit = Physics2D.Raycast(zip.transform.position, direction, currentSpeed * Time.deltaTime + stopDistance, switchLayer);
            if (hit.collider != null)
            {
                Switch isSwitch = hit.collider.GetComponent<Switch>();
                    if(isSwitch.switchMode == isSwitch.mySwitchInteraction)
                // 벽 바로 앞에서 멈추도록 위치를 조정하고 루프 종료
                zip.transform.position = hit.point;
                nowStop = true; // 루프를 종료하고 아래의 if (nowStop == false) 실행을 막습니다.
                break; // 코루틴 루프 즉시 종료
            }

            zip.transform.position += direction * currentSpeed * Time.deltaTime * 5f;
            yield return null; // 다음 프레임까지 대기
        }

        // 루프가 끝난 후 마지막 위치로 설정
        isInteraction = false;
        light2D.enabled = false;
        GameManager.instance.player.animator.SetBool("Zip", false);
        GameManager.instance.player.isZip = false;
        GameManager.instance.player.isMove = true;

        if (nowStop == false)
        {
            zip.transform.position = new Vector3(endPos.position.x, endPos.position.y - 0.6f, endPos.position.z);
            GameManager.instance.player.rigid.linearVelocity = new Vector2(GameManager.instance.player.rigid.linearVelocity.x, GameManager.instance.player.jumpForce);
            GameManager.instance.StartCoroutine("ZipLineCameraOffset", false);
        }
    }
}
