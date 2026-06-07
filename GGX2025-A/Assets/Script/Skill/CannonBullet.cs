using System.Collections;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    [SerializeField] private float speed = 18f;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private GameObject digSquarePrefab;
    [SerializeField] private Transform digParent;
    [SerializeField] private Sprite[] explosionSprites;
    [SerializeField] private float animFps = 12f;

    private Vector2 _dir;
    private Rigidbody2D _rb;

    public void Init(Vector3 direction)
    {
        _dir = direction.normalized;
        _rb = GetComponent<Rigidbody2D>();
        if (_rb != null) _rb.linearVelocity = _dir * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // 地面タグまたはGroundレイヤーに当たったら爆発
        if (col.CompareTag("Ground") || col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Vector3 pos = transform.position;

        // Enemy4Explosionと同じdigSquarePrefab方式で地形破壊
        if (digSquarePrefab != null)
        {
            Transform parent = digParent != null ? digParent : transform.parent;
            GameObject dig = Instantiate(digSquarePrefab, parent);
            dig.transform.position = pos;
            dig.transform.localScale = new Vector3(explosionRadius, explosionRadius, 1f);
            Destroy(dig, 0.2f);
        }

        // 爆発アニメーション
        if (explosionSprites != null && explosionSprites.Length > 0)
        {
            GameObject vfx = new GameObject("CannonExplosionVFX");
            vfx.transform.position = pos;
            vfx.transform.localScale = Vector3.one * explosionRadius;
            SpriteRenderer sr = vfx.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 10;
            StartCoroutine(AnimateExplosion(sr, vfx));
        }

        Destroy(gameObject);
    }

    private IEnumerator AnimateExplosion(SpriteRenderer sr, GameObject vfx)
    {
        float interval = 1f / animFps;
        foreach (Sprite s in explosionSprites)
        {
            if (sr == null) yield break;
            sr.sprite = s;
            yield return new WaitForSecondsRealtime(interval);
        }
        if (vfx != null) Destroy(vfx);
    }
}
