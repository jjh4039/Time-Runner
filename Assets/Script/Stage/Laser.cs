using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class Laser : MonoBehaviour
{
    private Light2D light2d;
    public BoxCollider2D boxCollider2D;
    public GameObject arrowPrefab;

    public void Awake()
    {
        light2d = GetComponent<Light2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine("SummonArrow");
            boxCollider2D.enabled = false;
        }
    }

    IEnumerator SummonArrow()
    {
        Instantiate(arrowPrefab, new Vector3(transform.position.x + 34f, transform.position.y, 0), Quaternion.Euler(0, 0, 0)).transform.parent = this.transform;

        for (int i = 0; i < 10; i++)
        {
            light2d.color = new Color(1, 1 - i * 0.1f, 1 - i * 0.1f, 1 - i * 0.1f);
            yield return new WaitForSeconds(0.003f);
        }
        light2d.enabled = false;
    }
}
