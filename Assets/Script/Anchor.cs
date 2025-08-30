using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Anchor : MonoBehaviour
{
    public bool isWire;
    public Light2D light2d;

    void Awake()
    {
        isWire = false;
        light2d = GetComponent<Light2D>();
    }

    void Update()
    {
        if (isWire)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            float newZ = currentRotation.z + 1f;

            light2d.color = Color.red;
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, newZ);
        }
        else
        {
            light2d.color = Color.white;
        }
    }
}
