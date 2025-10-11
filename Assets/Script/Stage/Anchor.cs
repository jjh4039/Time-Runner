using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Anchor : MonoBehaviour
{
    public bool isWire;
    public Light2D light2d;
    public int tmplevel;

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

            switch //(GameManager.instance.stageManager.currentLevel)
                (tmplevel)
            {
                case 0:
                    light2d.color = Color.red;
                    break;
                case 1:
                    light2d.color = new Color(1f, 0.36f, 1f); // Magenta
                    break;
                case 2:
                    light2d.color = new Color(0.3f, 0.4f, 1f);
                    break;
            }
            
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, newZ);
        }
        else
        {
            light2d.color = Color.white;
        }
    }
}
