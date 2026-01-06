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
    [SerializeField] private Transform gameCursorTransform;
    [SerializeField] private Collider2D cursorCol;

    [Header("UI Blockers")]
    [SerializeField] private List<RectTransform> uiBlockers;

    [Header("Energy Settings")]
    private float maxEnergy
    {
        get { return StatusManager.Instance.maxEnergy; }
        set { StatusManager.Instance.maxEnergy = value; }
    }
    private float currentEnergy
    {
        get { return StatusManager.Instance.currentEnergy; }
        set { StatusManager.Instance.currentEnergy = value; }
    }

    public float baseCost = 5f;        // ドラッグ開始時の固定消費
    public float sizeThreshold = 10f;  // サイズ閾値
    public float sizeCostRate = 0.5f;  // 閾値超過時の比例消費率

    private Vector3 startWorldPos;
    private GameObject currentSelectionSquare;
    private bool isSelecting = false;

    private float beforeDrillEnergy;
    public float requiredEnergy;
    public float minAreaToGenerate;//クリックでの生成防止用、これ未満は生成しない
    public float areaSize;

    [Header("掘削範囲の色")]
    [SerializeField] Color able;
    [SerializeField] Color notable;


    [Header("EnergyUI")]
    [SerializeField] Slider Slider_front;
    [SerializeField] Slider Slider_back;

    [Header("Time Slow")]
    [SerializeField] float slowTimeScale = 0.2f;
    [SerializeField] float normalTimeScale = 1f;


    void Update()
    {

        if (!isSelecting)
        {
            if (IsPointerOverUI()) return;
        }

        if (!isSelecting && currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Clamp(
                currentEnergy + StatusManager.Instance.chargeEnergy_effective * Time.deltaTime, 0f, maxEnergy);

            Slider_back.value = currentEnergy / maxEnergy;
            Slider_front.value = currentEnergy / maxEnergy;
        }

            // 左クリック開始
            if (Input.GetMouseButtonDown(0))
        {
            startWorldPos = GetCursorPosition();

            isSelecting = true;
            cursorCol.enabled = false;

            currentSelectionSquare = Instantiate(selectionSquarePrefab);

            // 掘削前のエネルギーを保持
            beforeDrillEnergy = currentEnergy;


        }

        // ドラッグ中
        if (Input.GetMouseButton(0) && isSelecting)
        {
            Time.timeScale = slowTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            Vector3 currentWorldPos = GetCursorPosition();
            UpdateSquare(currentSelectionSquare, startWorldPos, currentWorldPos);

            // サイズに比例した追加消費を計算
            Vector3 size = currentWorldPos - startWorldPos;
            areaSize = Mathf.Abs(size.x * size.y);

            requiredEnergy = baseCost;

            if (areaSize> minAreaToGenerate)
            {
                if (areaSize > sizeThreshold)
                {
                    requiredEnergy += (areaSize - sizeThreshold) * sizeCostRate;
                }

                // エネルギー不足なら赤色に
                SpriteRenderer sr = currentSelectionSquare.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = (requiredEnergy > beforeDrillEnergy) ? notable : able;
                }
                Slider_front.value = (currentEnergy - requiredEnergy) / maxEnergy;
            }
        }

        // 左クリック離す
        if (Input.GetMouseButtonUp(0) && isSelecting)
        {

            if (areaSize > minAreaToGenerate)
            {
                Vector3 endWorldPos = GetCursorPosition();

                if (requiredEnergy <= beforeDrillEnergy)
                {
                    // 掘削確定 → エネルギー消費
                    currentEnergy = Mathf.Clamp(beforeDrillEnergy - requiredEnergy, 0, maxEnergy);
                    Slider_back.value = currentEnergy / maxEnergy;

                    GameObject finalSquare = Instantiate(finalSquarePrefab, parentObject);
                    UpdateSquare(finalSquare, startWorldPos, endWorldPos);

                    BoxCollider2D col = finalSquare.AddComponent<BoxCollider2D>();
                    col.isTrigger = true;
                    col.size = Vector2.one;
                }
                else
                {
                    Slider_front.value = currentEnergy / maxEnergy;
                }


                Destroy(currentSelectionSquare);
                currentSelectionSquare = null;

            }

            Time.timeScale = normalTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            isSelecting = false;
            cursorCol.enabled = true;

        }

        // 右クリックキャンセル
        if (Input.GetMouseButtonDown(1) && isSelecting)
        {
            Time.timeScale = normalTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            Slider_front.value = currentEnergy / maxEnergy;
            Destroy(currentSelectionSquare);
            currentSelectionSquare = null;
            isSelecting = false;
            cursorCol.enabled = true;
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

    Vector3 GetCursorPosition()
    {
        Vector3 pos = gameCursorTransform.position;
        pos.z = 0f;
        return pos;
    }

    void UpdateSquare(GameObject square, Vector3 start, Vector3 end)
    {
        Vector3 center = (start + end) / 2f;
        Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y), 1f);

        square.transform.position = center;
        square.transform.localScale = size;
    }


}