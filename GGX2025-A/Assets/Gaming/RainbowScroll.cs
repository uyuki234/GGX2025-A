using UnityEngine;

public class RainbowScroll : MonoBehaviour
{
    [Header("スクロール速度")]
    public float scrollSpeed = 0.5f;

    [Header("Fever中だけ表示する")]
    public bool isFever = false;

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
        // Fever中だけ表示
        sr.enabled = isFever;
        if (!isFever) return;

        // UVスクロール
        float offset = Time.time * scrollSpeed;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
