using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StatusManager : SingletonMonoBehavior<StatusManager>
{
    public bool isGame=true;

    [Header("移動速度")]
    public float moveSpeed_base = 10;
    public float moveSpeed_correction = 1;
    public float moveSpeed_effective = 10f;

    [Header("攻撃力")]
    public float attack_base = 10;
    public float attack_correction = 1;
    public float attack_effective = 10;

    [Header("エネルギー回復速度")]
    public float chargeEnergy_base = 10;
    public float chargeEnergy_correction = 1;
    public float chargeEnergy_effective = 10;

    [Header("射程")]
    public float viewRange_base = 10;
    public float viewRange_correction = 1;
    public float viewRange_effective = 10;

    [Header("エネルギー上限")]
    public float maxEnergy;
    public float currentEnergy;

    [Header("ジャンプ力")]
    public float jumpPow;

    [Header("レベル")]
    public float currentLevel;
    public float currentExp;
    public float levelupExp;
    public float Score;

    [Header("HP")]
    public float maxHP;
    public float currentHP;

    [Header("Fever")]
    public bool isFEVER = false;
    public float maxFeverTime = 100;
    public float feverTime = 0;

    public void TakeDamage(float amount, GameObject hitObject = null)
    {
        currentHP -= amount;

        var target = hitObject;
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
            var sr = target.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) StartCoroutine(FlashRed(sr));
        }
    }

    private IEnumerator FlashRed(SpriteRenderer sr)
    {
        sr.color = new Color(1f, 60f / 255f, 60f / 255f, 1f);
        yield return new WaitForSecondsRealtime(0.15f);
        sr.color = Color.white;
    }

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
            FeverCount();;
        }
        else
        {
            LevelCheck();
        }
    }

    public void FeverCount()
    {
        feverTime = feverTime - Time.deltaTime;

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

        if (SkillManager.Instance != null)
            SkillManager.Instance.OpenSkillSelection();
        else
            StartFever();
    }

    // フィーバー前の補正値を保持（スキルボーナスをフィーバー終了後も維持するため）
    private float _preFeverMoveSpeed;
    private float _preFeverAttack;
    private float _preFeverChargeEnergy;
    private float _preFeverViewRange;

    public void StartFever()
    {
        isFEVER = true;
        feverTime = maxFeverTime;

        // 現在の補正値（スキルボーナス込み）を保存し、フィーバー倍率を乗算
        _preFeverMoveSpeed    = moveSpeed_correction;
        _preFeverAttack       = attack_correction;
        _preFeverChargeEnergy = chargeEnergy_correction;
        _preFeverViewRange    = viewRange_correction;

        moveSpeed_correction    *= 1.5f;
        attack_correction       *= 1.5f;
        chargeEnergy_correction *= 2f;
        viewRange_correction    *= 2f;
        maxEnergy *= 2;
        currentEnergy = maxEnergy;

        Cal();

        if (FeverEffect.Instance != null) FeverEffect.Instance.Play();
        if (FeverEdgeGlow.Instance != null) FeverEdgeGlow.Instance.Enable();
    }

    public void EndFever()
    {
        isFEVER = false;
        feverTime = 0;

        // フィーバー前の補正値に戻す（スキルボーナスを保持）
        moveSpeed_correction    = _preFeverMoveSpeed;
        attack_correction       = _preFeverAttack;
        chargeEnergy_correction = _preFeverChargeEnergy;
        viewRange_correction    = _preFeverViewRange;
        maxEnergy /= 2;

        Cal();

        if (FeverEdgeGlow.Instance != null) FeverEdgeGlow.Instance.Disable();
    }


}
