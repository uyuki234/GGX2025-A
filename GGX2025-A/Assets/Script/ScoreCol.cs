using UnityEngine;

public class ScoreCol : MonoBehaviour
{
    [SerializeField] int addscore;
    [SerializeField] string targetTag = "Player";

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
            }
        }
    }
}