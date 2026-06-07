using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FeverEffect : MonoBehaviour
{
    public static FeverEffect Instance { get; private set; }

    [SerializeField] private Color ringColor = new Color(1f, 0.85f, 0f);
    [SerializeField] private int ringCount = 4;
    [SerializeField] private float ringDuration = 0.75f;
    [SerializeField] private float ringDelay = 0.12f;
    [SerializeField] private float maxSizePx = 2500f;
    [SerializeField] private float flashDuration = 0.35f;

    private static Sprite cachedRingSprite;

    private void Awake()
    {
        Instance = this;
    }

    public void Play()
    {
        StartCoroutine(PlayAll());
    }

    private IEnumerator PlayAll()
    {
        StartCoroutine(AnimateFlash());
        for (int i = 0; i < ringCount; i++)
            StartCoroutine(AnimateRing(i * ringDelay));
        yield return null;
    }

    // 画面全体を一瞬ゴールドにフラッシュ
    private IEnumerator AnimateFlash()
    {
        var go = new GameObject("FeverFlash");
        go.transform.SetParent(transform, false);
        var img = go.AddComponent<Image>();
        img.raycastTarget = false;
        img.color = new Color(ringColor.r, ringColor.g, ringColor.b, 0f);

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / flashDuration;
            float alpha = Mathf.Sin(t * Mathf.PI) * 0.4f;
            img.color = new Color(ringColor.r, ringColor.g, ringColor.b, alpha);
            yield return null;
        }
        Destroy(go);
    }

    // 中心から外に広がって消えるリング
    private IEnumerator AnimateRing(float delay)
    {
        if (delay > 0f) yield return new WaitForSecondsRealtime(delay);

        var go = new GameObject("FeverRing");
        go.transform.SetParent(transform, false);
        var img = go.AddComponent<Image>();
        img.sprite = GetRingSprite();
        img.raycastTarget = false;

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;

        float elapsed = 0f;
        while (elapsed < ringDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / ringDuration;
            // ease-out: 最初は速く、外に行くほど減速
            float eased = 1f - (1f - t) * (1f - t);
            float size = maxSizePx * eased;
            rt.sizeDelta = new Vector2(size, size);
            img.color = new Color(ringColor.r, ringColor.g, ringColor.b, (1f - t) * 0.9f);
            yield return null;
        }
        Destroy(go);
    }

    // ドーナツ形テクスチャを1度だけ生成してキャッシュ
    private static Sprite GetRingSprite()
    {
        if (cachedRingSprite != null) return cachedRingSprite;

        int size = 256;
        float half = size * 0.5f;
        float outerR = half;
        float innerR = half * 0.75f;
        float softPx = 5f;

        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Mathf.Sqrt((x - half) * (x - half) + (y - half) * (y - half));
                float outerAlpha = Mathf.Clamp01((outerR - dist) / softPx);
                float innerAlpha = Mathf.Clamp01((dist - innerR) / softPx);
                pixels[y * size + x] = new Color(1f, 1f, 1f, Mathf.Min(outerAlpha, innerAlpha));
            }
        }
        tex.SetPixels(pixels);
        tex.Apply();

        cachedRingSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return cachedRingSprite;
    }
}
