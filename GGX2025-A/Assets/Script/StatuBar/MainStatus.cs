using UnityEngine;
using UnityEngine.UI;

public class MainStatus : MonoBehaviour
{
    [SerializeField] private Image Hpfill;
    [SerializeField] private Image Expfill;
    [SerializeField] private Image Energyfill_front;
    [SerializeField] private Image Energyfill_back;

    [SerializeField] private WorldRectangleSelector _wordSelector;

    private void Update()
    {
        Hpfill.fillAmount = StatusManager.Instance.currentHP/StatusManager.Instance.maxHP;
        Expfill.fillAmount = StatusManager.Instance.currentExp / StatusManager.Instance.levelupExp;
        Energyfill_front.fillAmount = _wordSelector.Slider_front;
        Energyfill_back.fillAmount = _wordSelector.Slider_back;
    }

}