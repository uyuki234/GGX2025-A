using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class TilemapToObjects : MonoBehaviour
{
    [Header("Basic Settings")]
    [SerializeField] private bool useList;
    [SerializeField] private GameObject zerotile;
    [SerializeField] private GameObject zeroobj;
    [SerializeField] private List<Tilemap> tilemapList;
    [SerializeField] private List<GameObject> objectList;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject tilePrefab;

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

    [Header("Optimization Settings")]
    [SerializeField] private int maxDeletionsPerFrame = 20;
    [SerializeField] private int waitFramesBeforeObjectSpawn = 3;

    private List<GameObject> layerContainers = new List<GameObject>();
    private int layerCount = 0;
    private bool LRtag = true;

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

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
        float rawX = player.position.x;
        float rawY = player.position.y - verticalOffset;
        float newBaseX = Mathf.Round(rawX / 0.5f) * 0.5f;
        float newBaseY = Mathf.Round(rawY / 0.5f) * 0.5f;
        rightpos = new Vector2(newBaseX, newBaseY);
        leftpos = new Vector2(newBaseX, newBaseY);
        GenerateInitialLayer();
    }

    private void GenerateInitialLayer()
    {
        CreateNewLayerContainer();
        StartCoroutine(CleanupOldLayersRoutine());

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
            ConvertTilemap(tilemap, currentLayerParent, null, Vector3.zero);
        }
    }

    private void CreateNewLayerContainer()
    {
        layerCount++;
        GameObject newContainer = new GameObject($"Layer_{layerCount}");
        newContainer.transform.parent = this.transform;
        currentLayerParent = newContainer.transform;
        layerContainers.Add(newContainer);
    }

    // 上層の削除
    private IEnumerator CleanupOldLayersRoutine()
    {
        if (layerContainers.Count > 2)
        {
            GameObject oldLayer = layerContainers[0];
            layerContainers.RemoveAt(0);

            if (oldLayer != null)
            {
                List<GameObject> children = new List<GameObject>();
                foreach (Transform child in oldLayer.transform) children.Add(child.gameObject);

                int count = 0;
                foreach (GameObject child in children)
                {
                    if (child == null) continue;
                    Destroy(child);
                    count++;
                    // �폜���ׂ𕪎U
                    if (count >= maxDeletionsPerFrame) { yield return null; count = 0; }
                }
                Destroy(oldLayer);
            }
        }
    }

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
        GameObject mapInstanceObj = Instantiate(zerotile.gameObject, spawnPos, Quaternion.identity, currentLayerParent);

        TilePatternWidth tilePatternWidth = mapInstanceObj.GetComponent<TilePatternWidth>();
        Tilemap mapInstance = mapInstanceObj.GetComponent<Tilemap>();

        if (mapInstance != null)
        {
            rightpos.x += tilePatternWidth.patternwidth;
            ConvertTilemap(mapInstance, currentLayerParent, zeroobj, spawnPos);
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

        GameObject mapInstanceObj = Instantiate(selectedPrefab.gameObject, spawnPos, Quaternion.identity, currentLayerParent);
        Tilemap mapInstance = mapInstanceObj.GetComponent<Tilemap>();
        if (mapInstance != null)
        {
            ConvertTilemap(mapInstance, currentLayerParent, selectedObject, spawnPos);
        }
    }

    public void ConvertTilemap(Tilemap targetMap, Transform parent, GameObject extraObject, Vector3 spawnPos)
    {
        targetMap.gameObject.SetActive(false);
        BoundsInt bounds = targetMap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (!targetMap.HasTile(pos)) continue;

            TileBase tile = targetMap.GetTile(pos);
            Tile tileData = tile as Tile;
            if (tileData == null) continue;

            Vector3 worldPos = targetMap.CellToWorld(pos) + targetMap.tileAnchor;

            GameObject obj = Instantiate(tilePrefab, worldPos, Quaternion.identity, parent);
            obj.name = "Tile_" + pos;

            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = tileData.sprite;
        }

        if (extraObject != null)
        {
            StartCoroutine(SpawnObjectAfterDelay(extraObject, spawnPos, parent));
        }

        Destroy(targetMap.gameObject);
    }

    private IEnumerator SpawnObjectAfterDelay(GameObject extraObject, Vector3 spawnPos, Transform parent)
    {
        for (int i = 0; i < waitFramesBeforeObjectSpawn; i++)
        {
            yield return null;
        }

        if (extraObject != null)
        {
            Instantiate(extraObject, spawnPos, Quaternion.identity, parent);
        }
    }
}