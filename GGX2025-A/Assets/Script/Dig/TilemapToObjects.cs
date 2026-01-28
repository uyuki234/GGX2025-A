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

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject tilePrefab;

    // インスペクターで設定不要（スクリプト内で動的に変えます）
    private Transform currentLayerParent;

    [Header("Position Management")]
    [SerializeField] private Vector2 rightpos = new Vector2(0, 0);
    [SerializeField] private Vector2 leftpos = new Vector2(0, 0);
    [SerializeField] private int quantity;

    [Header("Camera & Infinite Scroll")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float generateBuffer = 10f;
    [SerializeField] private int chunkGenerationCount = 10;

    [Header("Layer System")]
    [SerializeField] private Transform player;
    [SerializeField] private float layerChangeYThreshold = -10f;
    [SerializeField] private float layerDepth = 20f;
    [SerializeField] private float verticalOffset = 5f;

    // 層の管理用リスト（古い層を消すため）
    private List<GameObject> layerContainers = new List<GameObject>();
    private int layerCount = 0; // 何層目か

    private bool LRtag = true;

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // 最初の層を生成
        GenerateInitialLayer();
    }

    private void Update()
    {
        CheckNextLayer();
        CheckHorizontalGeneration();
    }

    private void CheckNextLayer()
    {
        if (player == null) return;

        if (player.position.y < layerChangeYThreshold)
        {
            StartNextLayer();
        }
    }

    private void StartNextLayer()
    {
        layerChangeYThreshold -= layerDepth;

        float newBaseX = player.position.x;
        float newBaseY = player.position.y - verticalOffset;

        rightpos = new Vector2(newBaseX, newBaseY);
        leftpos = new Vector2(newBaseX, newBaseY);

        GenerateInitialLayer();
    }

    private void GenerateInitialLayer()
    {
        // 1. 新しい層のための親オブジェクトを作成
        CreateNewLayerContainer();

        // 2. 古すぎる層を削除
        CleanupOldLayers();

        // 初期タイル生成
        zero_GenerateAndConvert();

        LRtag = true;
        if (useList)
        {
            for (int i = 0; i < quantity; i++)
            {
                GenerateOneSide(LRtag);
                LRtag = !LRtag;
            }
        }
        else if (tilemap != null)
        {
            ConvertTilemap();
        }
    }

    // --- 新機能: 層のコンテナ管理 ---

    private void CreateNewLayerContainer()
    {
        layerCount++;
        // "Layer_1", "Layer_2" のような空のGameObjectを作る
        GameObject newContainer = new GameObject($"Layer_{layerCount}");
        newContainer.transform.parent = this.transform; // 整理のためこのスクリプトの子にする

        // これから生成するタイルの親をこれに設定
        currentLayerParent = newContainer.transform;

        // リストに追加
        layerContainers.Add(newContainer);
    }

    private void CleanupOldLayers()
    {
        // 保持するのは「現在の層」と「1つ前の層」の合計2つまで
        // つまり3つ以上になったら、一番古いやつ(index 0)を消す
        if (layerContainers.Count > 2)
        {
            GameObject oldLayer = layerContainers[0];
            layerContainers.RemoveAt(0);
            Destroy(oldLayer);
        }
    }

    // --------------------------------

    private void CheckHorizontalGeneration()
    {
        if (cameraTransform == null) return;
        float camX = cameraTransform.position.x;

        if (camX + generateBuffer > rightpos.x)
        {
            for (int i = 0; i < chunkGenerationCount; i++) GenerateOneSide(true);
        }

        if (camX - generateBuffer < leftpos.x)
        {
            for (int i = 0; i < chunkGenerationCount; i++) GenerateOneSide(false);
        }
    }

    private void zero_GenerateAndConvert()
    {
        Vector3 spawnPos = new Vector3(rightpos.x, rightpos.y, 0);

        // 生成時に parent (currentLayerParent) を指定！
        GameObject mapInstanceObj = Instantiate(zerotile.gameObject, spawnPos, Quaternion.identity, currentLayerParent);

        TilePatternWidth tilePatternWidth = mapInstanceObj.GetComponent<TilePatternWidth>();
        Tilemap mapInstance = mapInstanceObj.GetComponent<Tilemap>();

        if (mapInstance != null)
        {
            rightpos.x += tilePatternWidth.patternwidth;
            tilemap = mapInstance;
            ConvertTilemap();
        }
    }

    private void GenerateOneSide(bool isRight)
    {
        if (tilemapList.Count == 0) return;

        int randomIndex = Random.Range(0, tilemapList.Count);
        Tilemap selectedPrefab = tilemapList[randomIndex];
        GameObject selectedObject = objectList[randomIndex];

        TilePatternWidth prefabWidthScript = selectedPrefab.GetComponent<TilePatternWidth>();
        float currentWidth = prefabWidthScript.patternwidth;

        Vector3 spawnPos;

        if (isRight)
        {
            spawnPos = new Vector3(rightpos.x, rightpos.y, 0);
            rightpos.x += currentWidth;
        }
        else
        {
            spawnPos = new Vector3(leftpos.x - currentWidth, leftpos.y, 0);
            leftpos.x -= currentWidth;
        }

        // parent に currentLayerParent を指定
        GameObject mapInstanceObj = Instantiate(selectedPrefab.gameObject, spawnPos, Quaternion.identity, currentLayerParent);
        if (selectedObject != null) Instantiate(selectedObject, spawnPos, Quaternion.identity, currentLayerParent);

        Tilemap mapInstance = mapInstanceObj.GetComponent<Tilemap>();

        if (mapInstance != null)
        {
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

            // parent に currentLayerParent を指定
            GameObject obj = Instantiate(tilePrefab, worldPos, Quaternion.identity, currentLayerParent);
            obj.name = "Tile_" + pos;
            obj.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}