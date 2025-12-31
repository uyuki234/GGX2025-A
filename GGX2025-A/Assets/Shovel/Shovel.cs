using UnityEngine;

public class ShovelController : MonoBehaviour
{
    [Header("シャベルのTransform")]
    public Transform shovel;

    [Header("回転のスムーズさ（0.0〜1.0）")]
    [Range(0f, 1f)]
    public float smooth = 0.25f;

    [Header("スプライトの向き補正（例：上向きなら-90）")]
    public float angleOffset = -90f;

    private Vector3 prevMousePos;

    void Start()
    {
        prevMousePos = GetMouseWorldPosition();
    }

    void Update()
    {
        if (shovel == null) return;

        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 dir = mousePos - prevMousePos;

        // マウスが動いている時だけ回転
        if (dir.sqrMagnitude > 0.001f)
        {
            Vector3 normalizedDir = dir.normalized;
            float angle = Mathf.Atan2(normalizedDir.y, normalizedDir.x) * Mathf.Rad2Deg + angleOffset;

            Quaternion targetRot = Quaternion.Euler(0, 0, angle);
            shovel.rotation = Quaternion.Lerp(shovel.rotation, targetRot, smooth);

            // マウスが動いた時だけ更新
            prevMousePos = mousePos;
        }
    }

    // カメラのZ補正付きマウス座標取得
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z); // Z補正
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
