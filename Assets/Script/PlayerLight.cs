using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PlayerLight : MonoBehaviour
{

    private Light2D light;

    void Start()
    {
        light = GetComponent<Light2D>();
        StartCoroutine("OnOff");
    }

    IEnumerator OnOff()
    {
        for (int i = 0; i < 100; i++)
        {
            light.intensity += 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 100; i++)
        {
            light.intensity -= 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine("OnOff");
    }
}
