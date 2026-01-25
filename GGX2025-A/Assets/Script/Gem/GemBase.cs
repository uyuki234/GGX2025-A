using System.Collections.Generic;
using UnityEngine;

public abstract class GemBase : MonoBehaviour
{
    public GemType type;

    [SerializeField]protected GameObject gemParticle;

    public int createCount = 0;

    private Vector3 prevPos;
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
            CreateParticle();
        }
        prevPos = transform.position;
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
            //プレイヤーのEXPを増やす
            StatusManager.Instance.currentExp++;

            //プレイヤーのEXPがレベルアップに必要なEXP以上ならレベルを上げる
            StatusManager.Instance.AddExp(1);

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