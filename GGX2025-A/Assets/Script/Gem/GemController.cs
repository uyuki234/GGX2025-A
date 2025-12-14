using UnityEngine;

public class GemController : MonoBehaviour
{
    public Transform player;   // ロボット(プレイヤー)のTransform
    public float speed = 3f;   // 移動速度
    private bool startMove = false; // 移動開始フラグ

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // 2秒後に StartMove を呼ぶ
        Invoke(nameof(StartMove), 2f);
    }

    void StartMove()
    {
        startMove = true;
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
}
