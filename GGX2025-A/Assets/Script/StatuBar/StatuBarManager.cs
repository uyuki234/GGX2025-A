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
        //Hp,
        MoveSpeed,
        Attack,
        ChargeEnergy,
        ViewRange,
        //EnergyBarFront,
        //EnergyBarBack,
        //EXP,
        //Max
    }
}


public class SliderExample : MonoBehaviour
{
    [SerializeField] private Slider[] statuSlider; // Inspectorで設定
    //[SerializeField] private WorldRectangleSelector _wordSelector;

    private void Update()
    {
        //statuSlider[(int)UI.SliderType.Hp].maxValue = StatusManager.Instance.maxHP;
        //statuSlider[(int)UI.SliderType.Hp].value = StatusManager.Instance.currentHP;
        statuSlider[(int)UI.SliderType.MoveSpeed].value = StatusManager.Instance.moveSpeed_base;
        statuSlider[(int)UI.SliderType.Attack].value = StatusManager.Instance.attack_base;
        statuSlider[(int)UI.SliderType.ChargeEnergy].value = StatusManager.Instance.chargeEnergy_base;
        statuSlider[(int)UI.SliderType.ViewRange].value = StatusManager.Instance.viewRange_base;
        //statuSlider[(int)UI.SliderType.EnergyBarFront].value = _wordSelector.Slider_front;
        //statuSlider[(int)UI.SliderType.EnergyBarBack].value = _wordSelector.Slider_back;
        //statuSlider[(int)UI.SliderType.EXP].value = (float)StatusManager.Instance.currentExp / (float)StatusManager.Instance.levelupExp;
    }
}
