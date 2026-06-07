using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FeverEdgeGlow : MonoBehaviour
{
    public static FeverEdgeGlow Instance { get; private set; }

    [Header("炎コラム")]
    [SerializeField] private int flameCount = 22;
    [SerializeField] private float flameMinHeight = 80f;
    [SerializeField] private float flameMaxHeight = 300f;
    [SerializeField] private float flameBaseAlpha = 0.55f;
    [SerializeField] private Color flameColor = new Color(1f, 0.62f, 0.04f);
    [SerializeField] private float flameSpeedMultiplier = 1f;

    [Header("エッジグロー帯")]
    [SerializeField] private float glowStripHeight = 10f;
    [SerializeField] private Color glowColor = new Color(1f, 0.80f, 0.2f);
    [SerializeField] private float glowAlpha = 0.95f;
    [SerializeField] private float glowPulseSpeed = 2.5f;

    [Header("四角パーティクル")]
    [SerializeField] private float spawnInterval = 0.28f;
    [SerializeField] private float squareMinSize = 5f;
    [SerializeField] private float squareMaxSize = 24f;
    [SerializeField] private float driftSpeed = 75f;
    [SerializeField] private float squareLifetime = 2.8f;
    [SerializeField] private Color squareColor = new Color(1f, 0.82f, 0.35f);

    [Header("フェード")]
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.7f;

    private float alphaScale = 0f;
    private Coroutine fadeRoutine;
    private Coroutine spawnRoutine;

    private class FlameData
    {
        public RectTransform rt;
        public Image img;
        public float targetH;
        public float currentH;
        public float baseSpeed;
        public float baseAlpha;
        public float flickerSpeed;
        public float flickerOffset;
    }

    private readonly List<FlameData> topFlames = new();
    private readonly List<FlameData> botFlames = new();

    private static Sprite flameTopSprite;
    private static Sprite flameBotSprite;
    private Image topGlowStrip;
    private Image botGlowStrip;

    private void Awake()
    {
        Instance = this;
        BuildGlowStrips();
        BuildFlames(topFlames, true);
        BuildFlames(botFlames, false);
    }

    private void Update()
    {
        AnimateFlames(topFlames);
        AnimateFlames(botFlames);
        UpdateGlowStrips();
    }

    public void Enable()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeAlphaScale(1f, fadeInTime));
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        spawnRoutine = StartCoroutine(SpawnSquares());
    }

    public void Disable()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeAlphaScale(0f, fadeOutTime));
        if (spawnRoutine != null) { StopCoroutine(spawnRoutine); spawnRoutine = null; }
    }

    private IEnumerator FadeAlphaScale(float target, float duration)
    {
        float start = alphaScale;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            alphaScale = Mathf.Lerp(start, target, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }
        alphaScale = target;
    }

    // ────────────────────────────────────────────
    private void BuildGlowStrips()
    {
        topGlowStrip = CreateStrip(true);
        botGlowStrip = CreateStrip(false);
    }

    private Image CreateStrip(bool isTop)
    {
        var go = new GameObject(isTop ? "TopGlowStrip" : "BotGlowStrip");
        go.transform.SetParent(transform, false);
        var img = go.AddComponent<Image>();
        img.color = new Color(glowColor.r, glowColor.g, glowColor.b, 0f);
        img.raycastTarget = false;
        var rt = go.GetComponent<RectTransform>();
        if (isTop)
        {
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(0f, -glowStripHeight);
            rt.offsetMax = Vector2.zero;
        }
        else
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = new Vector2(1f, 0f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = new Vector2(0f, glowStripHeight);
        }
        return img;
    }

    private void UpdateGlowStrips()
    {
        float pulse = 0.75f + 0.25f * Mathf.Sin(Time.unscaledTime * glowPulseSpeed);
        Color c = new Color(glowColor.r, glowColor.g, glowColor.b, glowAlpha * alphaScale * pulse);
        if (topGlowStrip != null) topGlowStrip.color = c;
        if (botGlowStrip != null) botGlowStrip.color = c;
    }

    // ────────────────────────────────────────────
    private void BuildFlames(List<FlameData> list, bool isTop)
    {
        float spacing = 1920f / flameCount;
        float startX = -960f + spacing * 0.5f;

        for (int i = 0; i < flameCount; i++)
        {
            var go = new GameObject(isTop ? "FlameTop" : "FlameBot");
            go.transform.SetParent(transform, false);

            var img = go.AddComponent<Image>();
            img.sprite = isTop ? GetFlameTopSprite() : GetFlameBotSprite();
            float r = Mathf.Clamp01(flameColor.r + Random.Range(-0.05f, 0.08f));
            float g = Mathf.Clamp01(flameColor.g + Random.Range(-0.1f, 0.12f));
            img.color = new Color(r, g, flameColor.b, 0f);
            img.raycastTarget = false;

            var rt = go.GetComponent<RectTransform>();
            float ancY = isTop ? 1f : 0f;
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, ancY);
            rt.anchoredPosition = new Vector2(startX + spacing * i, 0f);

            float w = spacing + 1f; // 隙間なし（1px重ねてサブピクセルのギャップを防ぐ）
            float initH = Random.Range(flameMinHeight, flameMaxHeight);
            float startH = initH * Random.Range(0.2f, 0.8f);
            float baseA = Random.Range(flameBaseAlpha * 0.6f, flameBaseAlpha);

            rt.sizeDelta = new Vector2(w, startH);

            list.Add(new FlameData
            {
                rt = rt,
                img = img,
                targetH = initH,
                currentH = startH,
                baseSpeed = Random.Range(55f, 150f),
                baseAlpha = baseA,
                flickerSpeed = Random.Range(3.5f, 9f),
                flickerOffset = Random.Range(0f, Mathf.PI * 2f)
            });
        }
    }

    private void AnimateFlames(List<FlameData> list)
    {
        foreach (var f in list)
        {
            if (f.rt == null) continue;

            f.currentH = Mathf.MoveTowards(f.currentH, f.targetH, f.baseSpeed * flameSpeedMultiplier * Time.unscaledDeltaTime);

            if (Mathf.Abs(f.currentH - f.targetH) < 3f)
            {
                f.targetH = Random.Range(flameMinHeight, flameMaxHeight);
                f.baseSpeed = Random.Range(55f, 150f);
            }

            f.rt.sizeDelta = new Vector2(f.rt.sizeDelta.x, f.currentH * alphaScale);

            float flicker = 0.78f + 0.22f * Mathf.Sin(Time.unscaledTime * f.flickerSpeed + f.flickerOffset);
            var c = f.img.color;
            c.a = f.baseAlpha * alphaScale * flicker;
            f.img.color = c;
        }
    }

    // ────────────────────────────────────────────
    // 縦グラデーション: 根元(エッジ側)が不透明、先端が透明
    // isFromBottom=false → 上辺が不透明(Top炎用)
    // isFromBottom=true  → 下辺が不透明(Bot炎用)
    private static Sprite GetFlameTopSprite()
    {
        if (flameTopSprite != null) return flameTopSprite;
        flameTopSprite = CreateGradientSprite(false);
        return flameTopSprite;
    }

    private static Sprite GetFlameBotSprite()
    {
        if (flameBotSprite != null) return flameBotSprite;
        flameBotSprite = CreateGradientSprite(true);
        return flameBotSprite;
    }

    private static Sprite CreateGradientSprite(bool isFromBottom)
    {
        int w = 4, h = 64;
        var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;
        Color[] pixels = new Color[w * h];
        const float tipMargin = 0.12f; // 先端側をこの割合だけ完全透明にして横線アーティファクトを防ぐ
        for (int y = 0; y < h; y++)
        {
            float t = (float)y / (h - 1);
            if (isFromBottom) t = 1f - t;
            float alpha = Mathf.Clamp01((t - tipMargin) / (1f - tipMargin));
            alpha = alpha * alpha * alpha;
            for (int x = 0; x < w; x++)
                pixels[y * w + x] = new Color(1f, 1f, 1f, alpha);
        }
        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f));
    }

    // ────────────────────────────────────────────
    private IEnumerator SpawnSquares()
    {
        while (true)
        {
            if (alphaScale > 0.05f)
                StartCoroutine(AnimateSquare());
            yield return new WaitForSecondsRealtime(spawnInterval);
        }
    }

    private IEnumerator AnimateSquare()
    {
        var go = new GameObject("FeverSquare");
        go.transform.SetParent(transform, false);

        var img = go.AddComponent<Image>();
        img.color = new Color(squareColor.r, squareColor.g, squareColor.b, 0f);
        img.raycastTarget = false;

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);

        float size = Random.Range(squareMinSize, squareMaxSize);
        rt.sizeDelta = new Vector2(size, size);

        bool fromTop = Random.value > 0.5f;
        float startX = Random.Range(-900f, 900f);
        float startY = fromTop ? 560f : -560f;
        rt.anchoredPosition = new Vector2(startX, startY);

        float dirY = fromTop ? -1f : 1f;
        float dirX = Random.Range(-0.3f, 0.3f);
        Vector2 vel = new Vector2(dirX * driftSpeed, dirY * driftSpeed);

        float rotSpeed = Random.Range(-60f, 60f);
        float fadeSec = 0.4f;
        float elapsed = 0f;

        while (elapsed < squareLifetime)
        {
            elapsed += Time.unscaledDeltaTime;
            float fadeIn  = Mathf.Clamp01(elapsed / fadeSec);
            float fadeOut = Mathf.Clamp01((squareLifetime - elapsed) / fadeSec);
            img.color = new Color(squareColor.r, squareColor.g, squareColor.b,
                                  Mathf.Min(fadeIn, fadeOut) * 0.6f * alphaScale);
            rt.anchoredPosition += vel * Time.unscaledDeltaTime;
            rt.localEulerAngles = new Vector3(0f, 0f, rt.localEulerAngles.z + rotSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        Destroy(go);
    }
}
