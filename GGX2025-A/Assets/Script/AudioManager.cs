using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("AudioManager");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<AudioManager>();
            }
            return _instance;
        }
    }

    [Header("音量設定")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 0.3f;
    [Range(0f, 1f)] public float seVolume = 1f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            _instance.masterVolume = masterVolume;
            _instance.bgmVolume = bgmVolume;
            _instance.seVolume = seVolume;
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        AudioListener.volume = masterVolume;
    }
}
