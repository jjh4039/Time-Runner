using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int[] nextLevelInt; // 각 스테이지 레벨에 도달하기 위한 스테이지 수
    public int currentLevel; // 현재 플레이어 레벨

    public GameObject[] stagePrefabs;
    public Transform player; // 플레이어 오브젝트
    public float stageClearDistance; // 스테이지 끝에서 얼마나 떨어졌을 때 다음 스테이지를 생성할지

    [SerializeField] private GameObject currentStage; // 현재 스테이지
    [SerializeField] private Vector3 nextSpawnPoint; // 다음 생성 위치
    [SerializeField] private int stageCount = 0; // 생성된 스테이지 수   

    void Start()
    {
        nextSpawnPoint = Vector3.zero;
        SpawnStage();

        currentLevel = 0;
    }

    void Update()
    {
        nextSpawnPoint.y = 0; // y축 고정

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
        
        int randomIndex = Random.Range(0, stagePrefabs.Length);

        // 연속 같은 스테이지 방지 (구현 필요)

        // 스테이지 생성
        GameObject newStage = Instantiate(stagePrefabs[randomIndex], nextSpawnPoint, Quaternion.identity);

        // 최종 생성 스테이지 번호 전달
        GameManager.stageNumber = randomIndex;

        // 생성된 스테이지에 고유한 태그를 붙여주기
        newStage.tag = "Stage" + stageCount;

        // 다음 생성 지점 업데이트
        Transform endOfStage = newStage.transform.Find("EndOfStage");
        if (endOfStage != null)
        {
            nextSpawnPoint = endOfStage.position;
        }

        stageCount++;
        GameManager.instance.StartCoroutine("TimeUp", randomIndex); // 스테이지 클리어 시마다 시간 추가
    }
}
