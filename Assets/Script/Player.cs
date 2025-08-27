using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public Vector2 moveInput;
    public float speed;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
   
    void Update()
    {
        // �¿� �̵��� �ش��ϴ� X�� ���� ���
        float horizontalMovement = moveInput.x * speed * Time.deltaTime;

        // ���� ��ġ�� �������� ���ο� ��ġ ���
        Vector3 newPosition = transform.position + new Vector3(horizontalMovement, 0, 0);

        // ��ġ ������Ʈ
        transform.position = newPosition;
    }

    void LateUpdate()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));

        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }
}
