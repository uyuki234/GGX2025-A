using System.Collections.Generic;
using UnityEngine;

public abstract class GemBase : MonoBehaviour
{
    public GemType type;

    [SerializeField]protected GameObject gemParticle;

    public int createCount = 0;

    private Vector3 prevPos;

    bool isBroken = false;

    // 初期化処理
    public abstract void Initialize();

    public abstract void CreateParticle();

    //プレイヤーに触れた場合の処理
    public abstract void HitPlayer();

    private void FixedUpdate()
    {
        createCount++;

        if(createCount > 100 && prevPos != transform.position)
        {
            createCount = -999999999;
            isBroken = true;
            CreateParticle();
        }
        prevPos = transform.position;

        // フィーバー中で地面が壊れていれば、プレイヤーに向かって飛ぶ
        if (isBroken && StatusManager.Instance.isFEVER)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // プレイヤーの位置を取得
                Transform player = GameObject.FindGameObjectWithTag("Player").transform;

                // プレイヤー方向のベクトルを計算
                Vector2 dir = (player.position - transform.position).normalized;

                // 力を加える
                float forcePower = 0.5f;
                rb.AddForce(dir * forcePower, ForceMode2D.Impulse);
            }
        }

    }

    private void Start()
    {
        Initialize();
        createCount = 0;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーのEXPがレベルアップに必要なEXP以上ならレベルを上げる
            StatusManager.Instance.AddExp(1);

            StatusManager.Instance.Score += 100;

            //宝石ごとの個別なヒット処理
            HitPlayer();

            //オブジェクトを破壊
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //プレイヤーのEXPを増やす
            StatusManager.Instance.AddExp(1);

            // 宝石ごとの個別なヒット処理
            HitPlayer();

            // オブジェクトを破壊
            Destroy(gameObject);
        }
    }


}