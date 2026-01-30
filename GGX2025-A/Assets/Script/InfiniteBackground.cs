using UnityEngine;
using System.Collections.Generic;

public class InfiniteBackground : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private GameObject backgroundPrefab; // 背景のプレハブ
    [SerializeField] private Vector2 chunkSize = new Vector2(19.2f, 10.8f); // 背景1枚の幅と高さ
    [SerializeField] private int viewRadius = 1; // 自分の周囲何マスを表示するか（1なら3x3、2なら5x5）

    private Transform cameraTransform;
    private Vector2Int currentChunkCoord; // 現在カメラがいるグリッド座標
    private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();

    private void Start()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        // 初回の生成を実行
        UpdateChunks();
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        // 現在のカメラ位置から、グリッド座標を計算
        // RoundToIntで四捨五入することで、マスの中心に近づくと座標が変わるようにする
        Vector2Int newChunkCoord = new Vector2Int(
            Mathf.RoundToInt(cameraTransform.position.x / chunkSize.x),
            Mathf.RoundToInt(cameraTransform.position.y / chunkSize.y)
        );

        // グリッド座標が変わった時だけ更新処理を走らせる（軽量化）
        if (newChunkCoord != currentChunkCoord)
        {
            currentChunkCoord = newChunkCoord;
            UpdateChunks();
        }
    }

    private void UpdateChunks()
    {
        // 1. 生成処理（カメラ周辺のマスをチェック）
        for (int x = -viewRadius; x <= viewRadius; x++)
        {
            for (int y = -viewRadius; y <= viewRadius; y++)
            {
                // チェック対象の座標
                Vector2Int targetCoord = currentChunkCoord + new Vector2Int(x, y);

                // まだ生成されていなければ生成
                if (!activeChunks.ContainsKey(targetCoord))
                {
                    CreateChunk(targetCoord);
                }
            }
        }

        // 2. 削除処理（遠すぎるマスを削除）
        // 削除対象を一時リストに入れる（ループ中にDictionaryを変更できないため）
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        foreach (var kvp in activeChunks)
        {
            // 現在地との距離を計算（マンハッタン距離ではなく、直線距離または単純な範囲外チェック）
            // ここでは「表示範囲 + 1マス」以上離れたら消す余裕を持たせています
            if (Mathf.Abs(kvp.Key.x - currentChunkCoord.x) > viewRadius + 1 ||
                Mathf.Abs(kvp.Key.y - currentChunkCoord.y) > viewRadius + 1)
            {
                chunksToRemove.Add(kvp.Key);
            }
        }

        // 実際に削除を実行
        foreach (var coord in chunksToRemove)
        {
            Destroy(activeChunks[coord]);
            activeChunks.Remove(coord);
        }
    }

    private void CreateChunk(Vector2Int coord)
    {
        // グリッド座標をワールド座標に変換
        Vector3 spawnPos = new Vector3(coord.x * chunkSize.x, coord.y * chunkSize.y, 10); // 背景なのでZは奥に

        GameObject obj = Instantiate(backgroundPrefab, spawnPos, Quaternion.identity, transform);
        obj.name = $"Background_{coord.x}_{coord.y}";

        // 管理リストに追加
        activeChunks.Add(coord, obj);
    }
}