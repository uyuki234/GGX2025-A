using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapToObjects : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform TileParent;

    private void Start()
    {
        tilemap.gameObject.SetActive(false);
        ConvertTilemap();
    }

    public void ConvertTilemap()
    {
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
