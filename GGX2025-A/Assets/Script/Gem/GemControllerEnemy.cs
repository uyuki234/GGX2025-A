using UnityEngine;

public class GemControllerEnemy : MonoBehaviour
{
    Transform player;   // ロボット(プレイヤー)のTransform
    public float speed = 800f;   // 移動速度
    private bool startMove = false; // 移動開始フラグ
    //private bool contactGround = false;
    private bool enemygem = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // 2秒後に StartMove を呼ぶ
        if(enemygem){
            Invoke(nameof(StartMove), 1f);
        }
    }

    void StartMove()
    {
        startMove = true;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true; // 衝突せずにすり抜ける
        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0; // 重力を無効化
        }
    }

    void Update()
    {
        if (startMove && player != null)
        {
            // 毎フレームプレイヤーの方向へ移動
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }
    }

    /*void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // 地面に着いたら 0.5 秒後にすり抜けモードへ
            Invoke(nameof(StartMove), 0.5f);
        }
    }*/


    public void SetEnemygem(bool a){
        enemygem = a;
    }
}
