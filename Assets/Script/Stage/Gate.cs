using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class Gate : MonoBehaviour
{
    public Light2D light2D;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider2D;

    public bool isShiftable;

    public void Awake()
    {
        isShiftable = true;
    }

    public void Connected(bool isConnect)
    {
        if (isConnect)
        {
            if (isShiftable)
            {
                spriteRenderer.color = Color.white;
                light2D.falloffIntensity = 0.7f;
            }
        }
        else
        {
            if (isShiftable)
            {
                spriteRenderer.color = Color.black;
                light2D.falloffIntensity = 1f;
            }
        }
    }

    public IEnumerator Shift()
    {
        GameManager.instance.player.isMove = false;
        GameManager.instance.player.rigid.linearVelocity = Vector2.zero;
        GameManager.instance.player.moveInput = Vector2.zero;
        GameManager.instance.player.animator.SetBool("isShift", true);

        for (int i = 0; i < 40; i++)
        {
            GameManager.instance.screenAlpha.alpha += 0.025f;
            GameManager.instance.playerLight.light2d.color = new Color(GameManager.instance.playerLight.light2d.color.r, GameManager.instance.playerLight.light2d.color.g,
                GameManager.instance.playerLight.light2d.color.b, GameManager.instance.playerLight.light2d.color.a - 0.1f);
            GameManager.instance.player.spriteRenderer.color = new Color(GameManager.instance.player.spriteRenderer.color.r, GameManager.instance.player.spriteRenderer.color.g,
                GameManager.instance.player.spriteRenderer.color.b, GameManager.instance.player.spriteRenderer.color.a - 0.1f);
            yield return new WaitForSeconds(0.003f);
        }

        GameManager.instance.cinemachine.ForceCameraPosition(this.transform.position, Quaternion.identity);
        GameManager.instance.player.isMove = true;
        GameManager.instance.player.transform.position = this.transform.position; 
        GameManager.instance.screenAlpha.alpha = 1f;

        GameManager.instance.playerLight.light2d.color = new Color(GameManager.instance.playerLight.light2d.color.r, GameManager.instance.playerLight.light2d.color.g,
            GameManager.instance.playerLight.light2d.color.b, 0f);
        GameManager.instance.player.spriteRenderer.color = new Color(GameManager.instance.player.spriteRenderer.color.r, GameManager.instance.player.spriteRenderer.color.g,
    GameManager.instance.player.spriteRenderer.color.b, 1f);

        GameManager.instance.player.animator.SetBool("isShift", false);
        GameManager.instance.player.animator.SetBool("Sliding", true);

        for (int i = 0; i < 40; i++)
        {
            GameManager.instance.screenAlpha.alpha -= 0.025f;
            GameManager.instance.playerLight.light2d.color = new Color(GameManager.instance.playerLight.light2d.color.r, GameManager.instance.playerLight.light2d.color.g,
                GameManager.instance.playerLight.light2d.color.b, GameManager.instance.playerLight.light2d.color.a + 0.025f);
            yield return new WaitForSeconds(0.003f);
        }
        GameManager.instance.screenAlpha.alpha = 0f;
    }
}
