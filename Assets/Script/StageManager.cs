using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int currentLevel; // 현재 플레이어 레벨
    public Queue<int> recentNumbers = new Queue<int>();
    public const int historySize = 3;

    public GameObject[] switchLevelStagePrefabs;
    public GameObject[] stagePrefabs;
    public GameObject[] stage2Prefabs;
    public GameObject[] stage3Prefabs;
    public Transform player; // 플레이어 오브젝트
    public float stageClearDistance; // 스테이지 끝에서 얼마나 떨어졌을 때 다음 스테이지를 생성할지

    [SerializeField] private GameObject currentStage; // 현재 스테이지
    [SerializeField] private Vector3 nextSpawnPoint; // 다음 생성 위치
    [SerializeField] private int stageCount = 0; // 생성된 스테이지 수   

    void Start()
    {
        nextSpawnPoint = Vector3.zero;
        SpawnStage();
        currentLevel = 1;
    }

    void Update()
    {
        nextSpawnPoint.y = 0;

        // 플레이어가 다음 스테이지를 생성할 지점에 도달했는지 확인
        // 플레이어의 위치(x)가 다음 생성 지점보다 멀리 있다면
        if (player.position.x > nextSpawnPoint.x - stageClearDistance)
        {
            SpawnStage();
        }

        if (stageCount > 2)
        {
            GameObject oldStage = GameObject.FindWithTag("Stage" + (stageCount - 3));
            if (oldStage != null)
            {
                Destroy(oldStage);
            }
        }
    }

    void SpawnStage()
    {
        GameObject newStage;
        if (stageCount == 2) // SwitchLevel Prefabs 생성
        {

        }
        else // 스테이지 생성
        {
            switch (currentLevel) // 레벨에 맞는 스테이지 생성
            {
                case 1:
                    int randomIndex = Random.Range(0, stagePrefabs.Length);
                    while (recentNumbers.Contains(randomIndex))
                    {
                        randomIndex = Random.Range(0, stagePrefabs.Length);
                    }
                    recentNumbers.Enqueue(randomIndex);

                    if (recentNumbers.Count > historySize)
                    {
                        recentNumbers.Dequeue();
                    }

                    string queueContents = string.Join(", ", recentNumbers.ToArray());
                    GameManager.instance.StartCoroutine("TimeUp", randomIndex); // 스테이지 클리어 시마다 시간 추가

                    // 스테이지 생성 & 스테이지 번호 전달
                    newStage = Instantiate(stagePrefabs[randomIndex], nextSpawnPoint, Quaternion.identity);
                    newStage.tag = "Stage" + stageCount;

                    GameManager.stageNumber = randomIndex;
                    Transform endOfStage = newStage.transform.Find("EndOfStage");
                    if (endOfStage != null)
                    {
                        nextSpawnPoint = endOfStage.position;
                    }
                    break;
            }
            stageCount++;
        }
    }

}
