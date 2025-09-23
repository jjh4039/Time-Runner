using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.InputSystem.iOS.LowLevel;
using UnityEngine.Rendering.Universal;
using UnityEditor.Experimental.GraphView;

public class Switch : MonoBehaviour
{
    [HideInInspector] public bool switchMode;
    public bool mySwitchInteraction;
    public Light2D light2D;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider2D;

    public void Awake()
    {
        switchMode = false;
        light2D = GetComponent<Light2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Update()
    {
        if (light2D.intensity >= 0.9)
        {
            boxCollider2D.enabled = true;
        }
        else
        {
            boxCollider2D.enabled = false;
        }
    }

    public void SwitchOn(InputAction.CallbackContext context)
    {
        if (context.performed && switchMode == false)
        {
            switchMode = true;

            if (switchMode == mySwitchInteraction)
            {
                StartCoroutine(Interaction(true));
            }
            else
            {
                StartCoroutine(Interaction(false));
            }
        }
    }

    public void SwitchOff(InputAction.CallbackContext context)
    {
        if (context.performed && switchMode == true)
        {
            switchMode = false;

            if (switchMode == mySwitchInteraction)
            {
                StartCoroutine(Interaction(true));
            }
            else
            {
                StartCoroutine(Interaction(false));
            }
        }
    }

    IEnumerator Interaction(bool myinteraction)
    {
        if (myinteraction)
        {
            for (int i = 0; i < 20; i++)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + 0.015f);
                light2D.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, light2D.color.a + 0.045f);
                light2D.intensity += 0.04f;
                yield return new WaitForSeconds(0.007f);
            }
            
        }
        else
        {
            for (int i = 0; i < 20; i++)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.015f);
                light2D.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, light2D.color.a - 0.045f);
                light2D.intensity -= 0.04f;
                yield return new WaitForSeconds(0.007f);
            }
        }
    }
}
