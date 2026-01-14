using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 moveDir;
    private float moveSpeed;
    private int _bulletDamage;
    private int spwanCount;
    private Camera cam;
    [SerializeField] bool notAffectedGround=true;
    [SerializeField] WorldRectangleSelector wrs;
    public void Initialize(Vector2 moveDir,float moveSpeed,int damage)
    {
        spwanCount = 0;
        this.moveDir = moveDir;
        this.moveSpeed = moveSpeed;
        this._bulletDamage = damage;
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = transform.position;
        playerPos.x -= moveDir.x * moveSpeed * Time.deltaTime * 60;
        playerPos.y -= moveDir.y * moveSpeed * Time.deltaTime * 60;
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

            else if (!notAffectedGround&& other.gameObject.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }

        }

    }


}