using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public float timeRemaining = 50f;

    void Awake()
    {
            instance = this;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime; 
    }
}
