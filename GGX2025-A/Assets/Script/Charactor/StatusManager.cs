using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

    [Header("フィーバー")]
    public bool isFEVER = false;
    public int maxFeverTime = 100;
    public int feverTime = 0;

    public void Cal()
    {
        moveSpeed_effective= moveSpeed_base*moveSpeed_correction;
        attack_effective = attack_base * attack_correction;
        chargeEnergy_effective = chargeEnergy_base * chargeEnergy_correction;
        viewRange_effective = viewRange_base * viewRange_correction;

    }

    public void AddExp(int value)
    {
        if (!isFEVER)
        {
            currentExp += value;
        }
    }

    public void FixedUpdate()
    {
        if (isFEVER)
        {
            FeverCount();
            FeverPlay();
        }
        else
        {
            LevelCheck();
        }
    }

    public void FeverCount()
    {
        feverTime--;

        if (feverTime <= 0)
        {
            EndFever();
        }
    }

    public void LevelCheck()
    {
        if (currentExp < levelupExp) return;

        currentLevel++;
        currentExp = 0;
        StartFever();
    }

    private void StartFever()
    {
        isFEVER = true;
        feverTime = maxFeverTime;
    }

    private void EndFever()
    {
        isFEVER = false;
        feverTime = 0;
    }

    private void FeverPlay()
    {
        currentEnergy = maxEnergy;

    }


}
