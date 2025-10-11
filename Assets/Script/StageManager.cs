using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int currentLevel; // 현재 플레이어 레벨
    public Queue<int> recentNumbers = new Queue<int>();
    public const int historySize = 3;

    [SerializeField] public GameObject[] switchLevelStagePrefabs;
    [SerializeField] public stagePrefabs[] stageArray;
    public Transform player; // 플레이어 오브젝트
    public float stageClearDistance; // 스테이지 끝에서 얼마나 떨어졌을 때 다음 스테이지를 생성할지

    [SerializeField] private GameObject currentStage; // 현재 스테이지
    [SerializeField] private Vector3 nextSpawnPoint; // 다음 생성 위치
    [SerializeField] private int stageCount = 0; // 생성된 스테이지 수   

    [System.Serializable] //반드시 필요
    public class stagePrefabs //행에 해당되는 이름
    {
        public GameObject[] stages;
    }

    void Start()
    {
        nextSpawnPoint = Vector3.zero;
        SpawnStage();
        currentLevel = 0;
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

        // SwitchLevel 스테이지 생성
        if (stageCount == 8 || stageCount == 14 || stageCount == 18) 
        {
            GameManager.instance.isTime = false;
            switch (currentLevel)
            {
                case 0:
                    newStage = Instantiate(switchLevelStagePrefabs[0], nextSpawnPoint, Quaternion.identity);
                    GameManager.instance.StartCoroutine("SwitchAlpha", 0);
                    GameManager.instance.timeMagnification = 1.2f;
                    GameManager.instance.player.lineRenderer.sharedMaterial = GameManager.instance.player.lineMaterials[1];
                    break;
                case 1:
                    newStage = Instantiate(switchLevelStagePrefabs[1], nextSpawnPoint, Quaternion.identity);
                    GameManager.instance.StartCoroutine("SwitchAlpha", 1);
                    GameManager.instance.timeMagnification = 1.5f;
                    GameManager.instance.player.lineRenderer.sharedMaterial = GameManager.instance.player.lineMaterials[2];
                    break;
                default:
                    newStage = null;
                    break;
            }
            recentNumbers.Clear();

            newStage.tag = "Stage" + stageCount;
            Transform endOfStage = newStage.transform.Find("EndOfStage");
            if (endOfStage != null)
            {
                nextSpawnPoint = endOfStage.position;
            }
            currentLevel++;
        }
        else // 스테이지 생성
        {
            GameManager.instance.isTime = true;
            int randomIndex = Random.Range(0, stageArray[currentLevel].stages.Length);

                    while (recentNumbers.Contains(randomIndex))
                    {
                        randomIndex = Random.Range(0, stageArray[currentLevel].stages.Length);
                    }
                    recentNumbers.Enqueue(randomIndex);

                    if (recentNumbers.Count > historySize)
                    {
                        recentNumbers.Dequeue();
                    }

                    string queueContents = string.Join(", ", recentNumbers.ToArray());
                    GameManager.instance.StartCoroutine("TimeUp", randomIndex); // 스테이지 클리어 시마다 시간 추가

                    // 스테이지 생성 & 스테이지 번호 전달
                    newStage = Instantiate(stageArray[currentLevel].stages[randomIndex], nextSpawnPoint, Quaternion.identity);
                    newStage.tag = "Stage" + stageCount;

                    GameManager.stageNumber = randomIndex;
                    Transform endOfStage = newStage.transform.Find("EndOfStage");
                    if (endOfStage != null)
                    {
                        nextSpawnPoint = endOfStage.position;
                    }
        }
            stageCount++;
    }
}