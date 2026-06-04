using UnityEngine;
using UnityEngine.Audio;

public class BGMPlayer : MonoBehaviour
{
    public AudioSource bgmAudioSource;

    void Start()
    {
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }
}
