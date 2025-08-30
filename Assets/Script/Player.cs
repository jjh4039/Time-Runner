using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float jumpForce;
    public float speed;
    public bool isGrounded;
    public float groundCheckRadius;
    public Anchor connectedAnchor;


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
        if (joint.enabled)
        {
            // LineRenderer�� DistanceJoint2D�� ��Ȱ��ȭ�Ͽ� ���̾� ����
            lineRenderer.enabled = false;
            joint.enabled = false;
            rigid.gravityScale = 2.5f;

            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x / 1.5f, rigid.linearVelocity.y);
            connectedAnchor.isWire = false; // ���̾� light �� ȸ�� ���� 
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
            // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // Raycast�� ����Ͽ� Ŭ���� ��ġ�� �ִ� ������Ʈ ã��
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // ���� Ŭ���� ������Ʈ�� 'GrapplePoint' �±׸� ������ �ִٸ�
            if (hit.collider != null && hit.collider.CompareTag("GrapplePoint"))
            {
                connectedAnchor = hit.collider.GetComponent<Anchor>();
                connectedAnchor.isWire = true; // ���̾� light �� ȸ�� ����
                rigid.gravityScale = 10f; // ���̾� ���� �� �߷� ����

                rigid.linearVelocity = rigid.linearVelocity = Vector2.zero;
                rigid.angularVelocity = 0f;
                // ����Ʈ Ȱ��ȭ �� ����
                joint.enabled = true;
                joint.connectedAnchor = hit.collider.transform.position;  // Ŭ���� ������ ���������� ����
                joint.distance = Vector2.Distance(transform.position, hit.collider.transform.position); // �ʱ� ���̾� ���� ����

                // ���η����� Ȱ��ȭ �� ������, ���� ����
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position); // ���̾� ������ (�÷��̾�)
                lineRenderer.SetPosition(1, hit.collider.transform.position); // ���̾� ���� (Ŭ���� ����)
            }
        }
    }

    public void PasswordInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.instance.password.isPasswordinput)
        {
            string keyName = context.control.name;

            // TryParse�� ����� ���ڿ��� ���ڷ� ��ȯ�� �õ��մϴ�.
            // ��ȯ�� �����ϸ� 'number' ������ ���� �Ҵ�ǰ�,
            // �����ϸ� if �� ���� �ڵ尡 ������� �ʽ��ϴ�.
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
                // Numpad0 ~ Numpad9�� ���� ���ڰ� ���Ե� �̸��� ��� ó��
                // ���ڿ� ���� ���ڸ� �����Ͽ� ��ȯ�� �õ��մϴ�.
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
            // LineRenderer�� ������ ��ġ�� �� �����Ӹ��� ������Ʈ
            lineRenderer.SetPosition(0, new Vector2(transform.position.x, transform.position.y + 0.2f));
        }
    }

    void FixedUpdate()
    {
        // ���̾� �׼�
        if (joint.enabled)
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * -5f);
            GetComponent<Rigidbody2D>().AddForce(transform.right * 1f);
        }

        // ���� �̵� (����̾� ����)
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
