using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Rigidbody2D rigid;
    public Transform groundCheck;
    public LayerMask groundLayer;

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
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, jumpForce);
        }
    }

    void Update()
    {
        float horizontalMovement = moveInput.x * speed * Time.deltaTime;
        Vector3 newPosition = transform.position + new Vector3(horizontalMovement, 0, 0);
        transform.position = newPosition;
        rigid.linearVelocity = new Vector2(horizontalMovement, rigid.linearVelocity.y);
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

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
    }
}
