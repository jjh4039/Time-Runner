using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SwitchLevel : MonoBehaviour
{
    public Light2D light2D;
    public BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boxCollider2D.enabled = false;
            StartCoroutine("UpLevel", GameManager.instance.stageManager.currentLevel);
        }
    }

    IEnumerator UpLevel(int Level)
    {
        switch (Level)
        {
            case 1:
                for (int i = 0; i < 50; i++)
                {
                    if (light2D.color.g <= 0.36f) light2D.color = new Color(light2D.color.r, light2D.color.g + 0.01f, light2D.color.b);
                    if (light2D.color.b <= 1f) light2D.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b + 0.02f);

                    GameManager.instance.switchText.color = new Color(light2D.color.r, light2D.color.g + 0.2f, light2D.color.b, GameManager.instance.switchText.color.a);
                    GameManager.instance.subSwitchText.color = new Color(light2D.color.r, light2D.color.g + 0.2f, light2D.color.b, GameManager.instance.subSwitchText.color.a);
                    GameManager.instance.playerLight.light2d.color = light2D.color;
                    GameManager.stageColor = light2D.color;
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 2:
                for (int i = 0; i < 50; i++)
                {
                    if (light2D.color.g >= 0.4f) light2D.color = new Color(light2D.color.r, light2D.color.g - 0.01f, light2D.color.b);
                    if (light2D.color.r >= 0.3f) light2D.color = new Color(light2D.color.r - 0.02f, light2D.color.g, light2D.color.b);

                    GameManager.instance.switchText.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, GameManager.instance.switchText.color.a);
                    GameManager.instance.subSwitchText.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, GameManager.instance.subSwitchText.color.a);
                    GameManager.instance.playerLight.light2d.color = light2D.color;
                    GameManager.stageColor = light2D.color;
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 3:
                for (int i = 0; i < 50; i++)
                {
                    if (light2D.color.g <= 1f) light2D.color = new Color(light2D.color.r + 0.02f, light2D.color.g + 0.02f, light2D.color.b + 0.02f);

                    GameManager.instance.switchText.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, GameManager.instance.switchText.color.a);
                    GameManager.instance.subSwitchText.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, GameManager.instance.subSwitchText.color.a);
                    GameManager.instance.playerLight.light2d.color = light2D.color;
                    GameManager.stageColor = light2D.color;
                    yield return new WaitForSeconds(0.01f);
                }
                break;
        }
    }
}
