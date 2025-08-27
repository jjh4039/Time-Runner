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
        // 좌우 이동에 해당하는 X축 값만 사용
        float horizontalMovement = moveInput.x * speed * Time.deltaTime;

        // 현재 위치를 기준으로 새로운 위치 계산
        Vector3 newPosition = transform.position + new Vector3(horizontalMovement, 0, 0);

        // 위치 업데이트
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
