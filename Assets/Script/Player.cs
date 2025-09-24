using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.SocialPlatforms;

public class Player : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public Transform groundCheck; // 이중 점프 방지
    [HideInInspector] public LayerMask groundLayer; // 땅 레이어 설정

    public LineRenderer lineRenderer;
    public DistanceJoint2D joint;
    public ZipLine zipLine;

    public bool isMove;

    public Vector2 moveInput;
    public float[] attackAmount;
    public float jumpForce;
    public float speed;
    public bool isGrounded;
    public bool isRestart;
    public bool isZip;
    public GameObject playerAttack;
    public enum PlayerSpaceMode { jump, attack, both }
    public PlayerSpaceMode playerSpaceMode;
    public float groundCheckRadius;
    public Anchor connectedAnchor;

    void Awake()
    { 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        // 와이어 관련
        lineRenderer = GetComponent<LineRenderer>();
        joint = GetComponent<DistanceJoint2D>();
        lineRenderer.enabled = false;
        joint.enabled = false;

        // 공격 관련
        attackAmount = new float[] { 0, 0, 0 };
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        if (context.performed && isRestart)
        { 
            isMove = true;
            isRestart = false;
            speed = 12f;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // 와이어 상태에서는 와이어 해제 액션
        if (joint.enabled)
        {
            // LineRenderer와 DistanceJoint2D를 비활성화하여 와이어 해제
            lineRenderer.enabled = false;
            joint.enabled = false;
            rigid.gravityScale = 2.5f;

            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x / 1.5f, rigid.linearVelocity.y);
            connectedAnchor.isWire = false; // 와이어 light 및 회전 해제 
        }

        if (context.performed && isZip == true)
        {
            zipLine.nowStop = true;
            speed = 9f;
        }

        // 일반 점프 & 공격 액션
        else if (!isRestart) // 부활 시 점프, 공격 방지
        {
            // 점프
            if (context.performed && isGrounded && playerSpaceMode == PlayerSpaceMode.jump)
            {
                rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, jumpForce);
                GameManager.instance.db.jumpCount++;
            }

            // 공격
            if (context.performed && playerSpaceMode == PlayerSpaceMode.attack)
            {
                if (attackAmount[0] <= 0 && attackAmount[1] <= 0 && attackAmount[2] <= 0)
                {
                    animator.SetInteger("Attack", 1);
                    GameManager.instance.StartCoroutine("AttackCameraZoom", 1);
                    attackAmount[0] = 0.5f; // 공격 쿨타임 설정
                }
                else if (attackAmount[0] <= 0.25f && attackAmount[1] <= 0 && attackAmount[2] <= 0)
                {
                    animator.SetInteger("Attack", 2);
                    GameManager.instance.StartCoroutine("AttackCameraZoom", 2);
                    attackAmount[0] = 0f;
                    attackAmount[1] = 0.45f; // 공격 쿨타임 설정
                }
                else if (attackAmount[0] <= 0f && attackAmount[1] <= 0.20f && attackAmount[2] <= 0)
                {
                    animator.SetInteger("Attack", 3);
                    GameManager.instance.StartCoroutine("AttackCameraZoom", 3);
                    attackAmount[0] = 0f;
                    attackAmount[1] = 0f;
                    attackAmount[2] = 0.7f; // 공격 쿨타임 설정
                }
            }
        }
    }

    public void OnWire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Wire action performed");
            // 마우스 위치를 월드 좌표로 변환
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // Raycast를 사용하여 클릭한 위치에 있는 오브젝트 찾기
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // 만약 클릭한 오브젝트가 'GrapplePoint' 태그를 가지고 있다면
            if (hit.collider != null && hit.collider.CompareTag("GrapplePoint"))
            {
                connectedAnchor = hit.collider.GetComponent<Anchor>();
                connectedAnchor.isWire = true; // 와이어 light 및 회전 설정
                rigid.gravityScale = 10f; // 와이어 연결 시 중력 증가

                rigid.linearVelocity = rigid.linearVelocity = Vector2.zero;
                rigid.angularVelocity = 0f;
                // 조인트 활성화 및 연결
                joint.enabled = true;
                joint.connectedAnchor = hit.collider.transform.position;  // 클릭한 지점을 연결점으로 설정
                joint.distance = Vector2.Distance(transform.position, hit.collider.transform.position); // 초기 와이어 길이 설정

                // 라인렌더러 활성화 및 시작점, 끝점 설정
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position); // 와이어 시작점 (플레이어)
                lineRenderer.SetPosition(1, hit.collider.transform.position); // 와이어 끝점 (클릭한 지점)
            }
        }
    }

    public void PasswordInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.instance.password.isPasswordinput)
        {
            string keyName = context.control.name;

            // TryParse를 사용해 문자열을 숫자로 변환을 시도합니다.
            // 변환에 성공하면 'number' 변수에 값이 할당되고,
            // 실패하면 if 문 내부 코드가 실행되지 않습니다.
            int number;
            if (int.TryParse(keyName, out number))
            {
                if (GameManager.instance.password.myPasswordText.text.Length < 4)
                {
                    GameManager.instance.password.myPasswordText.text += number.ToString();
                }
            }
            else
            {
                // Numpad0 ~ Numpad9와 같이 숫자가 포함된 이름인 경우 처리
                // 문자열 끝의 숫자만 추출하여 변환을 시도합니다.
                string numericPart = new string(keyName.Where(char.IsDigit).ToArray());
                if (int.TryParse(numericPart, out number))
                {
                    if (GameManager.instance.password.myPasswordText.text.Length < 4)
                    {
                        GameManager.instance.password.myPasswordText.text += number.ToString();
                    }
                }
            }
        }
    }
    void Update()
    {
        if (joint.enabled)
        {
            // LineRenderer의 시작점 위치를 매 프레임마다 업데이트
            lineRenderer.SetPosition(0, new Vector2(transform.position.x, transform.position.y + 0.2f));
        }

        // 임시 집라인 매커니즘 + Late Update
        if (isZip)
        {
            rigid.linearVelocity = rigid.linearVelocity = Vector2.zero;
            rigid.angularVelocity = 0f;
        }
    }

    void FixedUpdate()
    {
        // isMove 상태에 따른 이동
        moveInput = (isMove) ? new Vector2(1, 0) : Vector2.zero;

        // 공격 매커니즘
        for (int i = 0; i < attackAmount.Length; i++)
        {
            if (attackAmount[i] >= 0)
            {
                attackAmount[i] -= Time.fixedDeltaTime;
            }
        }

        // 공격 작동 시간
        if (attackAmount[0] + attackAmount[1] + attackAmount[2] <= 0.55f || attackAmount[0] + attackAmount[1] + attackAmount[2] > 0f)
        {
            playerAttack.SetActive(true);
        }

        // 미공격 및 즉시 공격 비작동
        if (attackAmount[0] + attackAmount[1] + attackAmount[2] <= 0f || attackAmount[0] + attackAmount[1] + attackAmount[2] > 0.55f)
        {
            playerAttack.SetActive(false);
        }

        // 이동 가능 상태 설정
        if (attackAmount[0] <= 0 && attackAmount[1] <= 0 && attackAmount[2] <= 0)
        {
            if (!isMove && !isRestart)
            {
                animator.SetInteger("Attack", 0);
                isMove = true;
                if (playerSpaceMode == PlayerSpaceMode.attack)
                    GameManager.instance.StartCoroutine("AttackCameraZoom", 0);
            }
        }
        else if (!isRestart && isMove) // 수정 필요!
        {
            if (isMove == true)
            {
                isMove = false;
                rigid.linearVelocity = new Vector2(0f, rigid.linearVelocity.y);
            }
        }

        // 와이어 액션
        if (joint.enabled)
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * -5f);
            GetComponent<Rigidbody2D>().AddForce(transform.right * 1f);
        }

        // 기초 이동 (비와이어 상태)
        if (!joint.enabled) 
        {
            float horizontalMovement = moveInput.x * speed * Time.deltaTime;
            Vector3 newPosition = transform.position + new Vector3(horizontalMovement, 0, 0);
            transform.position = newPosition;
            rigid.linearVelocity = new Vector2(horizontalMovement, rigid.linearVelocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 땅에 있을 때 중력 초과시 기본값 복귀
        if (isGrounded && rigid.gravityScale != 2f)
        {
            rigid.gravityScale = 2f;
            speed = 12f;
        }
    }

    void LateUpdate()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
        animator.SetBool("Jump", !isGrounded);

        if (isZip)
        {
            // 렌더링 직전에 zipLine 위치에 맞춰 플레이어 위치를 조정
            transform.position = new Vector2(zipLine.zip.transform.position.x - 0.07f, zipLine.zip.transform.position.y - 1.05f);
        }

        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }
}
