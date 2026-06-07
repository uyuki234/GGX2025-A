using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsUI : MonoBehaviour
{
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider seSlider;

    private void Start()
    {
        if (masterSlider != null) masterSlider.onValueChanged.AddListener(OnMasterChanged);
        if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        if (seSlider != null) seSlider.onValueChanged.AddListener(OnSEChanged);
    }

    private void OnEnable()
    {
        if (AudioManager.Instance == null) return;
        if (masterSlider != null) masterSlider.SetValueWithoutNotify(AudioManager.Instance.masterVolume);
        if (bgmSlider != null) bgmSlider.SetValueWithoutNotify(AudioManager.Instance.bgmVolume);
        if (seSlider != null) seSlider.SetValueWithoutNotify(AudioManager.Instance.seVolume);
    }

    public void OnMasterChanged(float val) { if (AudioManager.Instance != null) AudioManager.Instance.masterVolume = val; }
    public void OnBGMChanged(float val) { if (AudioManager.Instance != null) AudioManager.Instance.bgmVolume = val; }
    public void OnSEChanged(float val) { if (AudioManager.Instance != null) AudioManager.Instance.seVolume = val; }
}
