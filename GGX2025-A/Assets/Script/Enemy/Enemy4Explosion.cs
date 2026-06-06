using System.Collections;
using UnityEngine;

public class Enemy4Explosion : MonoBehaviour
{
    [SerializeField] private GameObject digSquarePrefab;
    [SerializeField] private Transform digParent;
    [SerializeField] public float explosionRadius = 3f;
    [SerializeField] private Sprite[] explosionSprites;
    [SerializeField] private float animFps = 10f;
    [SerializeField] private float explosionDamage = 20f;

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;

        Vector3 pos = transform.position;

        // 爆風ダメージ（robot のみ）
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dist = Vector2.Distance(pos, player.transform.position);
            if (dist <= explosionRadius)
                StatusManager.Instance.TakeDamage(explosionDamage, player);
        }

        // 周囲の地形を掘削
        if (digSquarePrefab != null)
        {
            GameObject dig = Instantiate(digSquarePrefab, digParent);
            dig.transform.position = pos;
            dig.transform.localScale = new Vector3(explosionRadius, explosionRadius, 1f);
            Destroy(dig, 0.2f);
        }

        // 爆発アニメーション表示
        if (explosionSprites != null && explosionSprites.Length > 0)
        {
            GameObject vfx = new GameObject("BombVFX");
            vfx.transform.position = pos;
            vfx.transform.localScale = Vector3.one * explosionRadius;
            SpriteRenderer sr = vfx.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 10;
            Sprite[] sprites = explosionSprites;
            float fps = animFps;
            StatusManager.Instance.StartCoroutine(AnimateExplosion(sr, vfx, sprites, fps));
        }
    }

    private static IEnumerator AnimateExplosion(SpriteRenderer sr, GameObject vfx, Sprite[] sprites, float fps)
    {
        float interval = 1f / fps;
        foreach (Sprite s in sprites)
        {
            if (sr == null) yield break;
            sr.sprite = s;
            yield return new WaitForSecondsRealtime(interval);
        }
        if (vfx != null) Destroy(vfx);
    }
}
