using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class WorldRectangleSelector : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject selectionSquarePrefab;
    public GameObject finalSquarePrefab;
    [SerializeField] private Transform parentObject;

    [SerializeField] private List<RectTransform> uiBlockers; // ← UIリスト

    private Vector3 startWorldPos;
    private GameObject currentSelectionSquare;
    private bool isSelecting = false;

    void Update()
    {
            if (!isSelecting)
            {
                if (IsPointerOverUI()) return;
            }
            // UI上でクリックしている場合は選択禁止

            // 左クリック開始
            if (Input.GetMouseButtonDown(0))
            {
                startWorldPos = GetMouseWorldPosition();
                isSelecting = true;

                currentSelectionSquare = Instantiate(selectionSquarePrefab);
            }

            // ドラッグ中
            if (Input.GetMouseButton(0) && isSelecting)
            {
                Vector3 currentWorldPos = GetMouseWorldPosition();
                UpdateSquare(currentSelectionSquare, startWorldPos, currentWorldPos);
            }

            // 左クリック離す
            if (Input.GetMouseButtonUp(0) && isSelecting)
            {
                Vector3 endWorldPos = GetMouseWorldPosition();

                GameObject finalSquare = Instantiate(finalSquarePrefab, parentObject);
                UpdateSquare(finalSquare, startWorldPos, endWorldPos);

                BoxCollider2D col = finalSquare.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = Vector2.one;

                Destroy(currentSelectionSquare);
                currentSelectionSquare = null;
                isSelecting = false;
            }

            // 右クリックキャンセル
            if (Input.GetMouseButtonDown(1) && isSelecting)
            {
                Destroy(currentSelectionSquare);
                currentSelectionSquare = null;
                isSelecting = false;
            }
        
    }

    bool IsPointerOverUI()
    {
        Vector2 mousePos = Input.mousePosition;

        foreach (var rect in uiBlockers)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, mousePos, mainCamera))
            {
                return true;
            }
        }

        return false;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }

    void UpdateSquare(GameObject square, Vector3 start, Vector3 end)
    {
        Vector3 center = (start + end) / 2f;
        Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y), 1f);

        square.transform.position = center;
        square.transform.localScale = size;
    }
}
