using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int _bulletDamage;
    private int spwanCount;
    private Camera cam;
    public void Initialize(Vector2 moveDir)
    {
        spwanCount = 0;
        this.moveDir = moveDir;
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = transform.position;
        playerPos.x -= moveDir.x * moveSpeed;
        playerPos.y -= moveDir.y * moveSpeed;
        transform.position = playerPos;
        spwanCount++;
    }

    void Update()
    {
        Vector3 pos = cam.WorldToViewportPoint(transform.position);

        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (spwanCount > 5)
        {

            if (other.gameObject.CompareTag("Player"))
            {
                StatusManager.Instance.currentHP -= _bulletDamage;
                Destroy(gameObject);
            }

        }

    }


}