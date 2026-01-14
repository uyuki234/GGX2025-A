using UnityEngine;

public class AutoTileCollider : MonoBehaviour
{
    [SerializeField] private float checkDistance = 1f;
    [SerializeField] private Vector2 checkSize = new Vector2(0.4f, 0.4f);
    [SerializeField] private LayerMask terrainMask;
    [SerializeField] private Sprite[] sprites;

    private SpriteRenderer sr;

    // 4方向だけに変更（上・右・下・左）
    private static readonly Vector2[] directions = new Vector2[]
    {
        new Vector2(0, 1),   // 上 → ビット 1
        new Vector2(1, 0),   // 右 → ビット 2
        new Vector2(0, -1),  // 下 → ビット 4
        new Vector2(-1, 0),  // 左 → ビット 8
    };

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        int mask = 0;

        for (int i = 0; i < 4; i++)
        {
            Vector2 checkPos = (Vector2)transform.position + directions[i] * checkDistance;

            Collider2D hit = Physics2D.OverlapBox(checkPos, checkSize, 0f, terrainMask);

            if (hit != null)
            {
                mask |= (1 << i);  // 4方向なので 1,2,4,8 のビットが立つ
            }
        }

        if (mask >= 0 && mask < sprites.Length && sprites[mask] != null)
        {
            sr.sprite = sprites[mask];
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var dir in directions)
        {
            Vector2 pos = (Vector2)transform.position + dir * checkDistance;
            Gizmos.DrawWireCube(pos, checkSize);
        }
    }
}