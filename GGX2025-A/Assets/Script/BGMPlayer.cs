using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioSource bgmAudioSource;

    void Start()
    {
        bgmAudioSource.loop = true;
        if (!bgmAudioSource.isPlaying)
            bgmAudioSource.Play();
    }
}
