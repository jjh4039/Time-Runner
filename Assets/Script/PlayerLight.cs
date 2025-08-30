using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PlayerLight : MonoBehaviour
{
    private Light2D light2d;

    void Start()
    {
        light2d = GetComponent<Light2D>();
        StartCoroutine("OnOff");
    }

    IEnumerator OnOff()
    {
        for (int i = 0; i < 100; i++)
        {
            light2d.intensity += 0.015f;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 100; i++)
        {
            light2d.intensity -= 0.015f;
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine("OnOff");
    }
}
