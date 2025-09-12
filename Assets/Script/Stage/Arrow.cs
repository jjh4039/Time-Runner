using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int arrowSpeed;
    public CapsuleCollider2D capsuleCollider2D;
    public Trigger trigger;

    public void Awake()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        trigger = GetComponent<Trigger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            trigger.trigger();
        }

        if (collision.CompareTag("PlayerAttack"))
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - 0.01f * arrowSpeed, transform.position.y, 0);
    }
}
