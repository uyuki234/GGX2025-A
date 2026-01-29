using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] private float maxHp = 50;
    [SerializeField] private float currentHp;
    private GemCreatEnemy gemCreatEnemy;

    [SerializeField] private GameObject HPUI;
    private Slider hpSlider;

    void Start()
    {
        currentHp = maxHp;

        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
    }

    public void SetHP(float hp)
    {
        this.currentHp = hp;

        UpdateHPValue();

        if (currentHp <= 0)
        {
            // 宝石をばらまく処理
            gemCreatEnemy = GetComponent<GemCreatEnemy>();
            gemCreatEnemy.ScatterObjects();

            // ★ 爆発する敵かどうか判定（ExplosiveEnemy が付いているか）
            ExplosiveEnemy explosive = GetComponent<ExplosiveEnemy>();
            if (explosive != null)
            {
                // 爆発する敵 → 爆発演出を実行
                explosive.Die();
            }
            else
            {
                // 爆発しない敵 → 普通に消える
                Destroy(gameObject);
            }
        }
    }

    public float GetHP()
    {
        return currentHp;
    }

    public float GetMaxHP()
    {
        return maxHp;
    }

    public void HideStatusUI()
    {
        HPUI.SetActive(false);
    }

    public void UpdateHPValue()
    {
        hpSlider.value = GetHP() / GetMaxHP();
    }
}
