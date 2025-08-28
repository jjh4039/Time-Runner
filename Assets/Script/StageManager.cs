using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject[] stagePrefabs;
    public Transform player; // �÷��̾� ������Ʈ
    public float stageClearDistance; // �������� ������ �󸶳� �������� �� ���� ���������� ��������

    [SerializeField] private GameObject currentStage; // ���� ��������
    [SerializeField] private Vector3 nextSpawnPoint; // ���� ���� ��ġ
    [SerializeField] private int stageCount = 0; // ������ �������� ��   

    void Start()
    {
        nextSpawnPoint = Vector3.zero;
        SpawnStage();
    }

    void Update()
    {
        nextSpawnPoint.y = 0; // y�� ����

        // �÷��̾ ���� ���������� ������ ������ �����ߴ��� Ȯ��
        // �÷��̾��� ��ġ(x)�� ���� ���� �������� �ָ� �ִٸ�
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
        // �������� �迭���� �������� �������� ����
        int randomIndex = Random.Range(0, stagePrefabs.Length);
        GameObject newStage = Instantiate(stagePrefabs[randomIndex], nextSpawnPoint, Quaternion.identity);

        // ������ ���������� ������ �±׸� �ٿ��ֱ�
        newStage.tag = "Stage" + stageCount;

        // ���� ���� ���� ������Ʈ
        Transform endOfStage = newStage.transform.Find("EndOfStage");
        if (endOfStage != null)
        {
            nextSpawnPoint = endOfStage.position;
        }

        stageCount++;
        GameManager.instance.StartCoroutine("TimeUp", randomIndex); // �������� Ŭ���� �ø��� �ð� �߰�
    }
}
