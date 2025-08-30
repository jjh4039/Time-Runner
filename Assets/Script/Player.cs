using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public Transform groundCheck; // 이중 점프 방지
    [HideInInspector] public LayerMask groundLayer; // 땅 레이어 설정

    public LineRenderer lineRenderer;
    public DistanceJoint2D joint;

    public Vector2 moveInput;
    public float jumpForce;
    public float speed;
    public bool isGrounded;
    public float groundCheckRadius;
    public Anchor connectedAnchor;


    void Awake()
    {
        GameManager.instance.password.StartCoroutine("CheckPassword");

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        // 와이어 관련
        lineRenderer = GetComponent<LineRenderer>();
        joint = GetComponent<DistanceJoint2D>();

        lineRenderer.enabled = false;
        joint.enabled = false;
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        if (joint.enabled == false) moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (joint.enabled)
        {
            // LineRenderer와 DistanceJoint2D를 비활성화하여 와이어 해제
            lineRenderer.enabled = false;
            joint.enabled = false;
            rigid.gravityScale = 2.5f;

            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x / 1.5f, rigid.linearVelocity.y);
            connectedAnchor.isWire = false; // 와이어 light 및 회전 해제 
        }
        else
        {
            if (context.performed && isGrounded)
            {
                rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, jumpForce);
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
                rigid.gravityScale = 6f; // 와이어 연결 시 중력 증가

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
        if (context.performed && GameManager.instance.password.isPasswordinput == true)
        {
            string keyName = context.control.name.Replace("Numpad", "").Replace("Keyboard", "");
            int number = int.Parse(keyName);

            if (GameManager.instance.password.myPasswordText.text.Length < 4)
            {
                GameManager.instance.password.myPasswordText.text += number.ToString();
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
    }

    void FixedUpdate()
    {
        // 와이어 액션
        if (joint.enabled)
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * -20f);
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

        if (isGrounded && rigid.gravityScale != 2f)
        {
            rigid.gravityScale = 2f;
        }
    }

    void LateUpdate()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
        animator.SetBool("Jump", !isGrounded);

        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }
}
