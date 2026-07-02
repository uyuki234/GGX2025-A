using UnityEngine;

public class SEManager : MonoBehaviour
{
    private static SEManager _instance;
    private AudioSource _audioSource;

    public static SEManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("SEManager");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<SEManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.spatialBlend = 0f;
        _audioSource.playOnAwake = false;
    }

    public void Play(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;
        float individual = AudioManager.Instance.GetSEVolume(clip);
        _audioSource.PlayOneShot(clip, AudioManager.Instance.seVolume * individual * volumeScale);
    }
}
