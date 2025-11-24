using UnityEngine;

public class StatusManager : MonoBehaviour
{
    [Header("キャラの移動速度")]
    public float moveSpeed_base = 10;
    public float moveSpeed_correction;
    public float moveSpeed_effective;

    [Header("ドラッグの攻撃力")]
    public float attack_base = 10;
    public float attack_correction;
    public float attack_effective;

    [Header("掘削エネルギー回復速度")]
    public float chargeEnergy_base = 10;
    public float chargeEnergy_correction;
    public float chargeEnergy_effective;

    [Header("視界の広さ")]
    public float viewRange_base = 10;
    public float viewRange_correction;
    public float viewRange_effective;

    [Header("掘削エネルギーの状態")]
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


}
