using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float maxsize = 10f;
    [SerializeField] float minsize = 4f;

    [Header("Movement Limits")]
    [SerializeField] Transform player;
    [SerializeField] Vector2 limitRange = new Vector2(20f, 15f); 

    private Vector3 dragOrigin;
    private bool isDragging = false;
    private Camera cam;

    void Start()
    {
        // 何度もCamera.mainを呼ぶのは重いのでキャッシュしておく
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        // --- 1. ズーム処理 ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minsize, maxsize);
        }

        // --- 2. ドラッグ移動処理 ---
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (Input.GetMouseButton(2) && isDragging)
        {
            Vector3 currentPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPosition;
            transform.position += difference;
        }

        if (Input.GetMouseButtonUp(2)) isDragging = false;

        // --- 3. 移動制限 (サイズ考慮) ---
        if (player != null)
        {
            Vector3 pos = transform.position;

            // カメラの縦横の「半径」を計算 (orthographicSizeは縦の半分)
            float vertExtent = cam.orthographicSize;
            float horzExtent = vertExtent * cam.aspect;

            // 「指定された制限範囲」から「カメラの大きさ」を引く
            // これにより、カメラの枠が制限線を超えない位置で止まるようになる
            float moveRangeX = Mathf.Max(0, limitRange.x - horzExtent);
            float moveRangeY = Mathf.Max(0, limitRange.y - vertExtent);

            // 計算した範囲でClamp
            pos.x = Mathf.Clamp(pos.x, player.position.x - moveRangeX, player.position.x + moveRangeX);
            pos.y = Mathf.Clamp(pos.y, player.position.y - moveRangeY, player.position.y + moveRangeY);

            transform.position = pos;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
            // この緑枠の中にカメラの映像が収まるようになります
            Gizmos.DrawWireCube(player.position, new Vector3(limitRange.x * 2, limitRange.y * 2, 0));
        }
    }
}