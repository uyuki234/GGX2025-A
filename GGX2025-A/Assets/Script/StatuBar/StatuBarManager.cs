using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スライダーのタイプ
/// </summary>
namespace UI
{
    public enum SliderType
    {
        None = -1,
        Hp,
        MoveSpeed,
        Attack,
        ChargeEnergy,
        ViewRange,
        Max
    }
}


public class SliderExample : MonoBehaviour
{
    [SerializeField] private Slider[] statuSlider; // Inspectorで設定

    private void Update()
    {
        statuSlider[(int)UI.SliderType.Hp].maxValue = StatusManager.Instance.maxHP;
        statuSlider[(int)UI.SliderType.Hp].value = StatusManager.Instance.currentHP;

        statuSlider[(int)UI.SliderType.MoveSpeed].value = StatusManager.Instance.moveSpeed_base;
        statuSlider[(int)UI.SliderType.Attack].value = StatusManager.Instance.attack_base;
        statuSlider[(int)UI.SliderType.ChargeEnergy].value = StatusManager.Instance.chargeEnergy_base;
        statuSlider[(int)UI.SliderType.ViewRange].value = StatusManager.Instance.viewRange_base;
    }
}
