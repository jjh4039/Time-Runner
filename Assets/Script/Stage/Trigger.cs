using UnityEngine;

public class Trigger : MonoBehaviour
{
    public int triggerNumber;
    public BoxCollider2D boxCollider2D;

    public void Awake()
    {
        switch (triggerNumber)
        {
            case 1:
                boxCollider2D = GetComponent<BoxCollider2D>();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.password.StartCoroutine("FinalPassword");
            gameObject.SetActive(false);
        }
    }
}
