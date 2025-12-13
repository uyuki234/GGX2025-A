using UnityEngine;
using UnityEngine.UI;

public class EnergyUIManager : MonoBehaviour
{
    [SerializeField] WorldRectangleSelector wrs;
    [SerializeField] private Slider targetSlider; // ← インスペクターで指定
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float currentEnergy = 100f;


    private void Update()
    {
        targetSlider.value = wrs.currentEnergy/ wrs.maxEnergy;//100/100で1
    }

    private void Start()
    {
        // インスペクターで指定した値を反映
        targetSlider.minValue = 0f;
        targetSlider.maxValue = maxEnergy;
        targetSlider.value = currentEnergy;
    }

    public void SetEnergy(float value)
    {
        currentEnergy = Mathf.Clamp(value, 0f, maxEnergy);
        targetSlider.value = currentEnergy; // ← スライダーに反映
    }

    public void ApplyEnergy(float cost)
    {
        SetEnergy(currentEnergy - cost);
    }

    public void RecoverEnergy(float amount)
    {
        SetEnergy(currentEnergy + amount);
    }
}
