using UnityEngine;

public class SurfaceCrawlerEnemy : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Collider2D BoxcolForFall;

    [Header("Ray 設定")]
    [SerializeField] private float rayDistance = 0.1f;

    // 四隅のローカル座標
    [SerializeField] private Vector2 rayOffset_top;
    [SerializeField] private Vector2 rayOffset_bottom;
    [SerializeField] private Vector2 rayOffset_center;

    private Rigidbody2D rb;
    private Collider2D Collider2D;
    private bool rotating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        BoxcolForFall.enabled = false;
        SnapToGrid();
    }

    void Update()
    {
        // === 四隅のグローバル座標を計算 ===
        Vector2 top = transform.TransformPoint(rayOffset_top);
        Vector2 bottom = transform.TransformPoint(rayOffset_bottom);
        Vector2 center = transform.TransformPoint(rayOffset_center);

        // === Raycast ===
        bool hitT = Physics2D.Raycast(top, transform.up, rayDistance, wallLayer);
        bool hitB = Physics2D.Raycast(bottom, transform.up, rayDistance, wallLayer);
        bool hitC = Physics2D.Raycast(center, -transform.up, rayDistance, wallLayer);

        // Debug 可視化
        Debug.DrawRay(top, transform.up * rayDistance, hitT ? Color.yellow : Color.green);
        Debug.DrawRay(bottom, transform.up * rayDistance, hitB ? Color.yellow : Color.green);
        Debug.DrawRay(center, -transform.up * rayDistance, hitC ? Color.red : Color.green);

        

        // === 回転判定 ===
        // 上が壁を検知→回転して壁を登る
        if (hitT && !rotating) 
        {
            rotating = true;
            transform.Rotate(0, 0, 90f);
            SnapToGrid();
            return;
        }

        else if (!hitB && !rotating){
            rb.linearVelocity = Vector2.zero;
            rotating = true;
            transform.Rotate(0, 0, -90f);
            SnapToGrid();
            return;
        }


        // === 前進判定 ===
        if (hitC) 
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = transform.right * moveSpeed;
            rotating = false;
        }
        else
        {
            rotating = true;
            rb.gravityScale = fallSpeed;
            Collider2D.isTrigger = false;

            return;
        }
    }

    private void SnapToGrid()//0.5ずつの値に丸める
    {
        Vector3 pos = transform.position;

        float snappedX = Mathf.Round(pos.x / 0.5f) * 0.5f;
        float snappedY = Mathf.Round(pos.y / 0.5f) * 0.5f;

        transform.position = new Vector3(snappedX, snappedY, pos.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.gravityScale = 0;
            Collider2D.isTrigger = true;
            transform.rotation = Quaternion.identity;
        }

    }
}