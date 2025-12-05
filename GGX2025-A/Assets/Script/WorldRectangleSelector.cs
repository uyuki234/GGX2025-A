using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorldRectangleSelector : MonoBehaviour
{
    [Header("Camera & Prefabs")]
    public Camera mainCamera;
    public GameObject selectionSquarePrefab;
    public GameObject finalSquarePrefab;
    [SerializeField] private Transform parentObject;

    [Header("UI Blockers")]
    [SerializeField] private List<RectTransform> uiBlockers;

    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;
    public float baseCost = 5f;        // ドラッグ開始時の固定消費
    public float sizeThreshold = 10f;  // サイズ閾値
    public float sizeCostRate = 0.5f;  // 閾値超過時の比例消費率

    private Vector3 startWorldPos;
    private GameObject currentSelectionSquare;
    private bool isSelecting = false;

    private float beforeDrillEnergy;
    public float requiredEnergy;

    void Update()
    {
        if (!isSelecting)
        {
            if (IsPointerOverUI()) return;
        }

        // 左クリック開始
        if (Input.GetMouseButtonDown(0))
        {
            startWorldPos = GetMouseWorldPosition();
            isSelecting = true;

            currentSelectionSquare = Instantiate(selectionSquarePrefab);

            // 掘削前のエネルギーを保持
            beforeDrillEnergy = currentEnergy;

            // 固定消費
            currentEnergy = Mathf.Clamp(currentEnergy - baseCost, 0, maxEnergy);
        }

        // ドラッグ中
        if (Input.GetMouseButton(0) && isSelecting)
        {
            Vector3 currentWorldPos = GetMouseWorldPosition();
            UpdateSquare(currentSelectionSquare, startWorldPos, currentWorldPos);

            // サイズに比例した追加消費を計算
            Vector3 size = currentWorldPos - startWorldPos;
            float areaSize = Mathf.Abs(size.x * size.y);

            requiredEnergy = baseCost;
            if (areaSize > sizeThreshold)
            {
                requiredEnergy += (areaSize - sizeThreshold) * sizeCostRate;
            }

            // エネルギー不足なら赤色に
            SpriteRenderer sr = currentSelectionSquare.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = (requiredEnergy > beforeDrillEnergy) ? Color.red : Color.white;
            }
        }

        // 左クリック離す
        if (Input.GetMouseButtonUp(0) && isSelecting)
        {
            Vector3 endWorldPos = GetMouseWorldPosition();

            if (requiredEnergy <= beforeDrillEnergy)
            {
                // 掘削確定 → エネルギー消費
                currentEnergy = Mathf.Clamp(beforeDrillEnergy - requiredEnergy, 0, maxEnergy);

                GameObject finalSquare = Instantiate(finalSquarePrefab, parentObject);
                UpdateSquare(finalSquare, startWorldPos, endWorldPos);

                BoxCollider2D col = finalSquare.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = Vector2.one;
            }
            else
            {
                Debug.Log("エネルギー不足で生成不可");
            }

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




/*using UnityEngine;
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
*/