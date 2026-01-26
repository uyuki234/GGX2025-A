using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapToObjects : MonoBehaviour
{
    [SerializeField] private bool useList;
    [SerializeField] private List<Tilemap> tilemapList;
    [SerializeField] private List<GameObject> objectList;
    [SerializeField] private List<Tilemap> existmapList;
    [SerializeField] private List<GameObject> existobjectList;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform TileParent;

    [SerializeField] private Vector2 rightpos = new Vector2(0,0);
    [SerializeField] private Vector2 leftpos = new Vector2(0,0);
    [SerializeField] private int quantity;

    private void Start()
    {

        if (useList)
        {
            for (int i = 0; i < quantity; i++)
            {
                GenerateAndConvert();
            }
        }
        else if (tilemap != null) ConvertTilemap();
        else Debug.Log("タイルが登録されていない");
    }
    private void GenerateAndConvert()
    {
        // 1. tilemapListからランダムに選ぶ
        int randomIndex = Random.Range(0, tilemapList.Count);
        Tilemap selectedPrefab = tilemapList[randomIndex];

        // 2. rightposにinstance生成
        // TilemapコンポーネントがついているGameObjectを生成します
        Vector3 spawnPos = new Vector3(rightpos.x, rightpos.y, 0);
        GameObject mapInstanceObj = Instantiate(selectedPrefab.gameObject, spawnPos, Quaternion.identity, transform);
        GameObject InstanceObj = Instantiate(objectList[randomIndex], spawnPos, Quaternion.identity, transform);

        TilePatternWidth tilePatternWidth = mapInstanceObj.GetComponent<TilePatternWidth>();

        Tilemap mapInstance = mapInstanceObj.GetComponent<Tilemap>();

        if (mapInstance != null)
        {
            // 3. existmapListの最後尾に挿入
            existmapList.Add(mapInstance);
            existobjectList.Add(InstanceObj);

            // 4. rightpos += width (横にずらすと仮定してX座標に加算)
            rightpos.x += tilePatternWidth.patternwidth;

            // 5. 定義してあるtilemapに入れ替える
            tilemap = mapInstance;

            ConvertTilemap();
        }
    }

    public void ConvertTilemap()
    {
        // 元のマップを非表示にし、これから生成する個別オブジェクトだけが見えるようにする
        tilemap.gameObject.SetActive(false);

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            TileBase tile = tilemap.GetTile(pos);
            Sprite sprite = (tile as Tile).sprite;

            Vector3 worldPos = tilemap.CellToWorld(pos) + tilemap.tileAnchor;

            GameObject obj = Instantiate(tilePrefab, worldPos, Quaternion.identity,TileParent);
            obj.name = "Tile_" + pos;

            // 見た目を合わせる
            obj.GetComponent<SpriteRenderer>().sprite = sprite;

            // もしプレハブに Collider や Script を入れておけばタイルごとに反映される
        }
    }
}
