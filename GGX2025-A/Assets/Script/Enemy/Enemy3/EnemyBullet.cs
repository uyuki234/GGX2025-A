using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 moveDir;
    [SerializeField]private float moveSpeed;
    public void Initialize(Vector2 moveDir)
    {
        this.moveDir = moveDir;
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = transform.position;
        playerPos.x -= moveDir.x * moveSpeed;
        playerPos.y -= moveDir.y * moveSpeed;
        transform.position = playerPos;
    }
}
