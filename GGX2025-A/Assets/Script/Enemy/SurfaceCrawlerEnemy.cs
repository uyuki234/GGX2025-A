using UnityEngine;

public class SurfaceCrawlerEnemy : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private LayerMask wallLayer;

    [Header("Ray 設定")]
    [SerializeField] private float rayDistance = 0.1f;

    // 四隅のローカル座標
    [SerializeField] private Vector2 rayOffset_TopLeft;
    [SerializeField] private Vector2 rayOffset_TopRight;
    [SerializeField] private Vector2 rayOffset_BottomLeft;
    [SerializeField] private Vector2 rayOffset_BottomRight;

    private Rigidbody2D rb;
    private bool rotating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // === 四隅のグローバル座標を計算 ===
        Vector2 tl = transform.TransformPoint(rayOffset_TopLeft);
        Vector2 tr = transform.TransformPoint(rayOffset_TopRight);
        Vector2 bl = transform.TransformPoint(rayOffset_BottomLeft);
        Vector2 br = transform.TransformPoint(rayOffset_BottomRight);

        // === Raycast ===
        bool hitTL = Physics2D.Raycast(tl, transform.up, rayDistance, wallLayer);
        bool hitTR = Physics2D.Raycast(tr, transform.up, rayDistance, wallLayer);
        bool hitBL = Physics2D.Raycast(bl, -transform.up, rayDistance, wallLayer);
        bool hitBR = Physics2D.Raycast(br, -transform.up, rayDistance, wallLayer);

        // Debug 可視化
        Debug.DrawRay(tl, transform.up * rayDistance, hitTL ? Color.yellow : Color.green);
        Debug.DrawRay(tr, transform.up * rayDistance, hitTR ? Color.yellow : Color.green);
        Debug.DrawRay(bl, -transform.up * rayDistance, hitBL ? Color.red : Color.green);
        Debug.DrawRay(br, -transform.up * rayDistance, hitBR ? Color.red : Color.green);

        // === 回転判定 ===
        // 左上が外れた → 左に曲がる
        if (hitTL)
        {
            transform.Rotate(0, 0, 90f);
            return;
        }

        else if (!hitTR && !hitBR &&!hitBL&& !rotating){
            rotating = true;
            transform.Rotate(0, 0, -90f);
            return;
        }
        
        if (!hitBL && !hitBR)
        {
            rotating = true;
            // 落下方向 = 進行方向を -90°した方向 = -transform.up
            Vector2 fallDir = -transform.up;
            rb.linearVelocity = fallDir * fallSpeed;
            return;
        }

        // === 前進判定 ===
        // 下2つの Ray が両方 hit → 壁に乗っている
        // どちらかが外れたら「進行方向へ進む」
        if (hitBL || hitBR)
        {
            rb.linearVelocity = transform.right * moveSpeed;
            rotating = false;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
