using UnityEngine;
using UnityEngine.UI;

public class ExcavationEnergyUI : MonoBehaviour
{
    [SerializeField] private Image beforeGauge; // 掘削前（半透明）
    [SerializeField] private Image afterGauge;  // 掘削後（通常）
    [SerializeField] private float maxEnergy = 100f;

    private float currentEnergy = 100f;
    private float previewCost = 0f;

    // 外部から参照できるプロパティ
    public float CurrentEnergy => currentEnergy;

    // 掘削前プレビュー更新
    public void UpdatePreview(float cost)
    {
        previewCost = cost;
        float previewEnergy = currentEnergy - previewCost;
        float beforeValue = Mathf.Clamp01(previewEnergy / maxEnergy);

        beforeGauge.fillAmount = beforeValue;
        beforeGauge.color = GetColor(previewEnergy, 0.5f); // 半透明
    }

    // 掘削確定（エネルギー消費）
    public void ApplyEnergy(float cost)
    {
        currentEnergy -= cost;
        currentEnergy = Mathf.Max(currentEnergy, 0f);
        UpdateGauge();
    }

    // エネルギー回復処理
    public void RecoverEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
        UpdateGauge();
    }

    // 初期化
    public void SetEnergy(float value)
    {
        currentEnergy = Mathf.Clamp(value, 0f, maxEnergy);
        UpdateGauge();
    }

    // 共通ゲージ更新処理
    private void UpdateGauge()
    {
        float afterValue = currentEnergy / maxEnergy;
        afterGauge.fillAmount = afterValue;
        afterGauge.color = GetColor(currentEnergy, 1f); // 通常色
    }

    // 色判定（緑・黄・赤）
    private Color GetColor(float energy, float alpha)
    {
        if (energy <= 0)
            return new Color(1f, 0f, 0f, alpha); // 赤
        else if (energy < maxEnergy * 0.3f)
            return new Color(1f, 1f, 0f, alpha); // 黄
        else
            return new Color(0f, 1f, 0f, alpha); // 緑
    }
}
