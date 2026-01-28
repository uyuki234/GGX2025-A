using UnityEngine;
using UnityEngine.UI; // これが必要

public class RainbowScroll_raw : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f; // 流れる速さ
    private RawImage rawImage;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        // 現在のUV位置を取得
        Rect uv = rawImage.uvRect;

        // 横方向（x）に時間を足してズラす
        uv.x += Time.deltaTime * speed;

        // ズレた値を戻す
        rawImage.uvRect = uv;
    }
}