using UnityEngine;

public class ExplosionDig : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Transform parentObject;

    [Header("Size Settings")]
    [SerializeField] private float width = 5f;
    [SerializeField] private float height = 3f;

    void OnDestroy()
    {
        GenerateRectangle();
    }

    void GenerateRectangle()
    {
        if (squarePrefab == null) return;

        GameObject square = Instantiate(squarePrefab, parentObject);

        Vector3 center = transform.position;
        center.z = 0f;

        square.transform.position = center;
        square.transform.localScale = new Vector3(width, height, 1f);

        BoxCollider2D col = square.GetComponent<BoxCollider2D>();
        if (col == null)
        {
            col = square.AddComponent<BoxCollider2D>();
        }

        col.isTrigger = true;
        col.size = new Vector2(width, height);
    }
}
