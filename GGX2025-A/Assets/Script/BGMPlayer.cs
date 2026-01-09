using UnityEngine;
using UnityEngine.Audio;

public class BGMPlayer : MonoBehaviour
{
    // ここにAudio Sourceコンポーネントをドラッグ&ドロップする
    public AudioSource bgmAudioSource;

    void Start()
    {
        // ゲーム開始時に再生する
        bgmAudioSource.Play();
    }
}