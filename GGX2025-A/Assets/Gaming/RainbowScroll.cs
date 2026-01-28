using UnityEngine;

public class RainbowScroll : MonoBehaviour
{
    [Header("スクロール速度")]
    public float scrollSpeed = 0.5f;

    private Material mat;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // マテリアルを複製して使用（他オブジェクトと共有しない）
        mat = Instantiate(sr.material);
        sr.material = mat;
    }

    void Update()
    {
        // UVスクロール
        float offset = Time.time * scrollSpeed;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
