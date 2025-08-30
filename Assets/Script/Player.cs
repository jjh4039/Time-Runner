using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public Transform groundCheck; // ���� ���� ����
    [HideInInspector] public LayerMask groundLayer; // �� ���̾� ����

    public LineRenderer lineRenderer;
    public DistanceJoint2D joint;

    public Vector2 moveInput;
    public float jumpForce = 5f;
    public float speed;
    public bool isGrounded;
    public float groundCheckRadius = 0.2f;


    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        // ���̾� ����
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
        if (context.performed && isGrounded)
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, jumpForce);
        }

        if (joint.enabled)
        {
            // LineRenderer�� DistanceJoint2D�� ��Ȱ��ȭ�Ͽ� ���̾� ����
            lineRenderer.enabled = false;
            joint.enabled = false;

            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x / 1.5f,rigid.linearVelocity.y);
        }
    }

    public void OnWire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Wire action performed");
            // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // Raycast�� ����Ͽ� Ŭ���� ��ġ�� �ִ� ������Ʈ ã��
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // ���� Ŭ���� ������Ʈ�� 'GrapplePoint' �±׸� ������ �ִٸ�
            if (hit.collider != null && hit.collider.CompareTag("GrapplePoint"))
            {
                Debug.Log("find!");

                rigid.linearVelocity = rigid.linearVelocity = new Vector2(rigid.linearVelocity.x / 2f, 0);
                rigid.angularVelocity = 0f;
                // ����Ʈ Ȱ��ȭ �� ����
                joint.enabled = true;
                joint.connectedAnchor = hit.point; // Ŭ���� ������ ���������� ����
                joint.distance = Vector2.Distance(transform.position, hit.point); // �ʱ� ���̾� ���� ����

                // ���η����� Ȱ��ȭ �� ������, ���� ����
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position); // ���̾� ������ (�÷��̾�)
                lineRenderer.SetPosition(1, hit.point); // ���̾� ���� (Ŭ���� ����)
            }
        }
    }

    void Update()
    {
        if (joint.enabled)
        {
            // LineRenderer�� ������ ��ġ�� �� �����Ӹ��� ������Ʈ
            lineRenderer.SetPosition(0, transform.position);

            GetComponent<Rigidbody2D>().AddForce(transform.up * -10f);
            GetComponent<Rigidbody2D>().AddForce(transform.right * 1f);
        }
    }

    void FixedUpdate()
    {
        if (!joint.enabled) 
        { 
            float horizontalMovement = moveInput.x * speed * Time.deltaTime;
            Vector3 newPosition = transform.position + new Vector3(horizontalMovement, 0, 0);
            transform.position = newPosition;
            rigid.linearVelocity = new Vector2(horizontalMovement, rigid.linearVelocity.y);
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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
