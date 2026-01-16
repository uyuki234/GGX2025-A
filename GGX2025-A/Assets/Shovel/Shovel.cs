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
    private Animator anim;

    void Start()
    {
        // 親（Player）から Animator を取得
        anim = GetComponentInParent<Animator>();

        prevMousePos = GetMouseWorldPosition();
    }

    void Update()
    {
        if (shovel == null) return;

        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 dir = mousePos - prevMousePos;

        // マウスが動いた方向に向ける
        if (dir.sqrMagnitude > 0.001f)
        {
            // アニメーションを発動（必要なら）
            anim.SetTrigger("shovelTrigger");

            // 方向ベクトルを角度に変換
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset;

            // シャベルを回転
            Quaternion targetRot = Quaternion.Euler(0, 0, angle);
            shovel.rotation = Quaternion.Lerp(shovel.rotation, targetRot, smooth);

            prevMousePos = mousePos;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
