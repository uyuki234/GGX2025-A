using UnityEngine;

public class ExplosionDig : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Transform parentObject;

    [Header("Size Settings")]
    [SerializeField] private float width = 2.5f;
    [SerializeField] private float height = 1.5f;

    // Animation Event から呼ばれる
    public void TriggerRectangle()
    {
        if (squarePrefab == null) return;

        // 親なしで生成 → 後から SetParent（Persistent parent エラー回避）
        GameObject square = Instantiate(squarePrefab);
        if (parentObject != null)
        {
            square.transform.SetParent(parentObject, false);  // false = ローカル座標維持
        }

        // 爆発の中心に配置
        Vector3 center = transform.position;
        center.z = 0f;
        square.transform.position = center;

        // スケール設定
        square.transform.localScale = new Vector3(width, height, 1f);

        // BoxCollider2D を追加（なければ）
        BoxCollider2D col = square.GetComponent<BoxCollider2D>();
        if (col == null)
        {
            col = square.AddComponent<BoxCollider2D>();
        }

        col.isTrigger = true;
        col.size = new Vector2(width, height);
    }
}
