using UnityEngine;

public class DB : MonoBehaviour
{
    // 누적되는 전체 플레이 데이터
    [Header("Total Data")]
    public float playTimeSeconds;
    public int deathCount;
    public int perfectCount;
    public int clearCount;
    public int jumpCount;

    // 매판 초기화되는 데이터
    [Header("Play Data")]
    public int continuePerfect;

    private void Awake()
    {
        ResetPlayData();
    }

    public void Update()
    {
        playTimeSeconds += Time.deltaTime;
        if (GameManager.instance.isPerfect == false)       
        {
            continuePerfect = 0;
        }
    }
    public void ResetPlayData()
    {
        continuePerfect = 0;
    }
}
