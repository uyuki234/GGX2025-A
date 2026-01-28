using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapToObjects : MonoBehaviour
{
    [Header("Basic Settings")]
    [SerializeField] private bool useList;
    [SerializeField] private GameObject zerotile;
    [SerializeField] private List<Tilemap> tilemapList;
    [SerializeField] private List<GameObject> objectList;
    [SerializeField] private List<Tilemap> existmapList;
    [SerializeField] private List<GameObject> existobjectList;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform TileParent;

    [Header("Position Management")]
    [SerializeField] private Vector2 rightpos = new Vector2(0, 0);
    [SerializeField] private Vector2 leftpos = new Vector2(0, 0); // 左側の位置管理用
    [SerializeField] private int quantity;

    [Header("Camera & Infinite Scroll")]
    [SerializeField] private Transform cameraTransform; // メインカメラをアサイン
    [SerializeField] private float generateBuffer = 10f; // カメラが端からこの距離以内に入ったら生成

    // 左右切り替え用タグ（true:右, false:左）
    private bool LRtag = true;

    private void Start()
    {
        // カメラが未設定なら自動取得
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        // 初期タイルの生成
        zero_GenerateAndConvert();

        // Start時は左右交互に生成（LRtagを使用）
        LRtag = true;

        if (useList)
        {
            for (int i = 0; i < quantity; i++)
            {
                // 現在のLRtagの方向へ生成し、タグを反転させる
                GenerateOneSide(LRtag);
                LRtag = !LRtag;
            }
        }
        else if (tilemap != null) ConvertTilemap();
        else Debug.Log("タイルが登録されていない");
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        // カメラのX座標を取得
        float camX = cameraTransform.position.x;

        // --- 右側のチェック ---
        // 「カメラ位置 + バッファ」が「現在の右端」を超えそうなら、右に追加
        if (camX + generateBuffer > rightpos.x)
        {
            GenerateOneSide(true); // 右へ強制生成
        }

        // --- 左側のチェック ---
        // 「カメラ位置 - バッファ」が「現在の左端」を超えそうなら、左に追加
        if (camX - generateBuffer < leftpos.x)
        {
            GenerateOneSide(false); // 左へ強制生成
        }
    }

    private void zero_GenerateAndConvert()
    {
        Vector3 spawnPos = new Vector3(rightpos.x, rightpos.y, 0);
        GameObject mapInstanceObj = Instantiate(zerotile.gameObject, spawnPos, Quaternion.identity, transform);

        TilePatternWidth tilePatternWidth = mapInstanceObj.GetComponent<TilePatternWidth>();
        Tilemap mapInstance = mapInstanceObj.GetComponent<Tilemap>();

        if (mapInstance != null)
        {
            existmapList.Add(mapInstance);

            // 初期タイルの分、右側の生成位置をずらす
            rightpos.x += tilePatternWidth.patternwidth;

            tilemap = mapInstance;
            ConvertTilemap();
        }
    }

    /// <summary>
    /// 指定した方向（isRight）に1つ生成するメソッド
    /// </summary>
    private void GenerateOneSide(bool isRight)
    {
        // 1. リストからランダムに選ぶ
        int randomIndex = Random.Range(0, tilemapList.Count);
        Tilemap selectedPrefab = tilemapList[randomIndex];
        GameObject selectedObject = objectList[randomIndex];

        TilePatternWidth prefabWidthScript = selectedPrefab.GetComponent<TilePatternWidth>();
        float currentWidth = prefabWidthScript.patternwidth;

        Vector3 spawnPos;

        // 2. 指定方向(isRight)に応じて位置を決定・更新
        if (isRight) // 右側に生成
        {
            spawnPos = new Vector3(rightpos.x, rightpos.y, 0);
            rightpos.x += currentWidth; // 右位置を更新
        }
        else // 左側に生成
        {
            spawnPos = new Vector3(leftpos.x - currentWidth, leftpos.y, 0);
            leftpos.x -= currentWidth; // 左位置を更新
        }

        // 3. 決定した位置に生成
        GameObject mapInstanceObj = Instantiate(selectedPrefab.gameObject, spawnPos, Quaternion.identity, transform);
        GameObject InstanceObj = Instantiate(selectedObject, spawnPos, Quaternion.identity, transform);

        Tilemap mapInstance = mapInstanceObj.GetComponent<Tilemap>();

        if (mapInstance != null)
        {
            existmapList.Add(mapInstance);
            existobjectList.Add(InstanceObj);
            tilemap = mapInstance;
            ConvertTilemap();
        }
    }

    public void ConvertTilemap()
    {
        tilemap.gameObject.SetActive(false);

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            TileBase tile = tilemap.GetTile(pos);
            Sprite sprite = (tile as Tile).sprite;

            Vector3 worldPos = tilemap.CellToWorld(pos) + tilemap.tileAnchor;

            GameObject obj = Instantiate(tilePrefab, worldPos, Quaternion.identity, TileParent);
            obj.name = "Tile_" + pos;

            obj.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}