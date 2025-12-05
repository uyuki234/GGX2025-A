using UnityEngine;

public class EnemyGravityController : MonoBehaviour
{
    public float rayDistance = 0.2f;
    public Vector2 rayOffset = new Vector2(0, -0.1f);
    public LayerMask wallLayer;
    public Rigidbody2D rb;

    private bool isYFrozen = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 origin = (Vector2)transform.position + rayOffset;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayDistance, wallLayer);

        if (!hit)
        {
            if (!isYFrozen)
            {
                rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
                isYFrozen = true;
            }
        }
        else
        {
            if (isYFrozen)
            {
                rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                isYFrozen = false;
            }
        }

        Debug.DrawRay(origin, Vector2.down * rayDistance, hit ? Color.green : Color.red);
    }
}
