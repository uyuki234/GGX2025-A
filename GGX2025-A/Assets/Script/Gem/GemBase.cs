using System.Collections.Generic;
using UnityEngine;

public abstract class GemBase : MonoBehaviour
{
    public GemType type;

    // 初期化処理
    public abstract void Initialize();

    //プレイヤーに触れた場合の処理
    public abstract void HitPlayer();


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーのEXPを増やす
            StatusManager.Instance.currentExp++;

            //プレイヤーのEXPがレベルアップに必要なEXP以上ならレベルを上げる
            if (StatusManager.Instance.currentExp >= StatusManager.Instance.levelupExp)
            {
                StatusManager.Instance.currentExp = 0;
                StatusManager.Instance.currentLevel++;
            }

            //宝石ごとの個別なヒット処理
            HitPlayer();

            //オブジェクトを破壊
            Destroy(gameObject);
        }
    }


}