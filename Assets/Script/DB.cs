using UnityEngine;

public class DB : MonoBehaviour
{
    static public DB instance;

    // 누적되는 전체 플레이 데이터
    [Header("Total Data")]
    public int deathCount;
    public int perfectCount;
    public bool[] isBuy;

    void Awake()
    {
        isBuy = new bool[7] { false, false, false, false, false, false, false };
        instance = this;
    }
}