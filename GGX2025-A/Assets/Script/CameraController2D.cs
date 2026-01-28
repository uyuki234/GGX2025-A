using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float maxsize=10f;
    [SerializeField] float minsize=4f;

    private Vector3 dragOrigin;
    private bool isDragging = false;

    void Update()
    {
        // 拡大縮小
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minsize, maxsize);
        }

        // 右クリック or 中ボタンドラッグでカメラ移動
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (Input.GetMouseButton(2) && isDragging)
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPosition;
            transform.position += difference;
        }

        if (Input.GetMouseButtonUp(2))
        {
            isDragging = false;
        }
    }
}
