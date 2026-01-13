using UnityEngine;

public class MouseRange : MonoBehaviour
{
    public Transform playerTrans;
    public Transform centerObj;
    [SerializeField] GameObject circle;
    public float radius = 3f;

    private Rigidbody2D rb;
    private Vector3 targetPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        
        radius = StatusManager.Instance.viewRange_effective;

        float scale = radius / 10f;
        circle.transform.localScale=new Vector3(scale,scale,1f);
        Vector3 mousePos = Input.mousePosition;

        // カメラからの距離
        float camDist = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 worldMouse = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, camDist)
        );

        // 距離計算
        float dx = worldMouse.x - centerObj.position.x;
        float dy = worldMouse.y - centerObj.position.y;
        float len = Mathf.Sqrt(dx * dx + dy * dy);

        // 円の中に制限
        if (len > radius)
        {
            float rate = radius / len;
            worldMouse = centerObj.position + new Vector3(dx * rate, dy * rate, 0);
        }

        targetPos = worldMouse;
        
    }
    private void FixedUpdate()
    {
        rb.MovePosition(targetPos);
    }
}