using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Anchor : MonoBehaviour
{
    public bool isWire;
    public Light2D light2d;
    public int tmplevel;

    [SerializeField]
    private float rotationSpeed = 200f;

    void Awake()
    {
        isWire = false;
        light2d = GetComponent<Light2D>();
    }

    void Update()
    {
        if (isWire)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationAmount);

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
                case 3:
                    light2d.color = Color.white;
                    light2d.intensity = 0.9f;
                    break;
            }
        }
        else
        {
            light2d.color = Color.white;
            light2d.intensity = 0.5f;
        }
    }
}
