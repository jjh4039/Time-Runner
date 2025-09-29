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
            StartCoroutine("UpLevel", 1);
        }
    }

    IEnumerator UpLevel(int arrivalLevel)
    {
        for (int i = 0; i < 50; i++)
        {
            if (light2D.color.g <= 0.36f) light2D.color = new Color(light2D.color.r, light2D.color.g + 0.01f, light2D.color.b);
            if (light2D.color.b <= 1f) light2D.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b + 0.02f);
            
            GameManager.instance.playerLight.light2d.color = light2D.color;
            GameManager.stageColor = light2D.color;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
