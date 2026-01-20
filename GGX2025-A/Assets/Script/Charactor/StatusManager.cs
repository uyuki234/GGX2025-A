using UnityEngine;

public class StatusManager : SingletonMonoBehavior<StatusManager>
{
    [Header("キャラの移動速度")]
    public float moveSpeed_base = 10;
    public float moveSpeed_correction = 1;
    public float moveSpeed_effective = 10f;

    [Header("ドラッグの攻撃力")]
    public float attack_base = 10;
    public float attack_correction = 1;
    public float attack_effective = 10;

    [Header("掘削エネルギー回復速度")]
    public float chargeEnergy_base = 10;
    public float chargeEnergy_correction = 1;
    public float chargeEnergy_effective = 10;

    [Header("視界の広さ")]
    public float viewRange_base = 10;
    public float viewRange_correction = 1;
    public float viewRange_effective = 10;

    [Header("掘削エネルギー")]
    public float maxEnergy;
    public float currentEnergy;

    [Header("ジャンプ力")]
    public float jumpPow;

    [Header("レベル")]
    public float currentLevel;
    public float currentExp;
    public float levelupExp;

    [Header("キャラのHP")]
    public float maxHP;
    public float currentHP;


    // ============================
    // ▼▼▼ フィーバー追加部分 ▼▼▼
    // ============================

    [Header("フィーバー関連")]
    public bool isFever = false;          // フィーバー中か
    public float feverLimitTime = 10f;    // フィーバーの制限時間
    public float feverRemainTime = 0f;    // 残り時間

    private float originalMaxEnergy;      // 復帰用に保存


    void Update()
    {
        if (isFever)
        {
            feverRemainTime -= Time.deltaTime;

            if (feverRemainTime <= 0f)
            {
                EndFever();
            }
        }
    }

    public void AddExp(float amount)
    {
        if (isFever) return; // フィーバー中は経験値が増えない

        currentExp += amount;

        if (currentExp >= levelupExp)
        {
            currentExp = 0;
            StartFever();
        }
    }

    public void StartFever()
    {
        if (isFever) return;

        isFever = true;
        feverRemainTime = feverLimitTime;

        // エネルギー強化
        originalMaxEnergy = maxEnergy;
        maxEnergy *= 2f;
        currentEnergy = maxEnergy;

        // パーティクル再生（まだ無いのでコメントアウト）
        // feverParticle.Play();

        // UI側で虹色ゲージ・虹色テキストを表示する
        // （UI側スクリプトで isFever を参照）
    }

    public void EndFever()
    {
        isFever = false;

        // エネルギーを元に戻す
        maxEnergy = originalMaxEnergy;
        currentEnergy = maxEnergy;

        // UI側で虹色ゲージ・虹色テキストを非表示にする
    }

    // ============================
    // ▲▲▲ フィーバー追加部分 ▲▲▲
    // ============================


    public void Cal()
    {
        moveSpeed_effective = moveSpeed_base * moveSpeed_correction;
        attack_effective = attack_base * attack_correction;
        chargeEnergy_effective = chargeEnergy_base * chargeEnergy_correction;
        viewRange_effective = viewRange_base * viewRange_correction;
    }
}
