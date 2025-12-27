using UnityEngine;

public class ShovelRotateByMouse : MonoBehaviour
{
    // 前フレームのマウス座標
    private Vector3 prevMousePos;

    void Start()
    {
        // 最初のフレームのマウス座標を記録
        prevMousePos = Input.mousePosition;
    }

    void Update()
    {
        // 現在のマウス座標
        Vector3 currentMousePos = Input.mousePosition;

        // マウス移動方向ベクトル
        Vector3 dir = currentMousePos - prevMousePos;

        // マウスが動いている時だけ処理
        if (dir.sqrMagnitude > 0.01f)
        {
            // ベクトルの角度を求める（2D用）
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // シャベルを回転させる（Z軸回転）
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // 現在の座標を次フレーム用に保存
        prevMousePos = currentMousePos;
    }
}
