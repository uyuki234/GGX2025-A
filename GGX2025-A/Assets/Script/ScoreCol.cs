using UnityEngine;

public class ScoreCol : MonoBehaviour
{
    [SerializeField] int addscore;
    [SerializeField] string targetTag = "Player";
    [SerializeField] string cursorTag = "Cursor";
    [SerializeField] GameObject cursor;
    [SerializeField] Vector2 offset = new Vector2(1, 1);

    // プレイヤーの座標を一時的に保持するための変数
    private Vector2 savedPlayerPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            if (StatusManager.Instance.isFEVER)
            {
                StatusManager.Instance.Score += addscore;
                StatusManager.Instance.EndFever();

                // 座標を保存して、1秒後に movecursor を呼び出す
                savedPlayerPos = other.transform.position;
                Invoke(nameof(DelayedMove), 1.0f);
            }
        }
    }

    // 1秒後に呼ばれる中間メソッド
    private void DelayedMove()
    {
        movecursor(savedPlayerPos);
    }

    private void movecursor(Vector2 playerpos)
    {
        cursor = GameObject.FindWithTag(cursorTag);
        if (cursor == null) return;

        Collider2D col = cursor.GetComponent<Collider2D>();

        if (col != null)
        {
            col.isTrigger = true; 
            col.transform.position = playerpos + offset;
            
            // 瞬間移動後に物理判定を戻す
            col.isTrigger = false;
        }
    }
}