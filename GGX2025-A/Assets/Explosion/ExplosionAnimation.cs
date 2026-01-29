using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    [Header("Explosive")]
    [SerializeField] private Animator animator;
    [SerializeField] private string deathTriggerName = "Death";

    private bool isDead = false;

    // 爆発アニメーションを開始する
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            // トリガーの暴発防止
            animator.ResetTrigger(deathTriggerName);
            animator.SetTrigger(deathTriggerName);
        }
        else
        {
            // Animator が無い場合は即削除
            Destroy(gameObject);
        }
    }

    // Animation Event から呼ばれる（アニメ終了時）
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
