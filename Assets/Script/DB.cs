using UnityEngine;
using UnityEngine.SceneManagement;

public class DB : MonoBehaviour
{
    static public DB instance;

    // 누적되는 전체 플레이 데이터
    [Header("Total Data")]
    public float playTime; // 총 플레이 시간(초)
    public int perfectCount;
    public int perfectCountSave;
    public int clearCount;
    public bool[] isBuy;

    void Awake()
    {
        isBuy = new bool[7] { false, false, false, false, false, false, false };
        instance = this;

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        playTime += Time.deltaTime;
    }
}