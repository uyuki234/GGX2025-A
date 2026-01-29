using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    private Animator anim;
    private ExplosionDig dig;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        dig = GetComponent<ExplosionDig>();   // ← ここで ExplosionDig を取得
    }

    // 敵が死んだ時に呼ばれる
    public void Die()
    {
        anim.SetTrigger("Die");
    }

    // Animation Event から呼ぶ（爆発判定）
    public void TriggerDig()
    {
        dig?.TriggerRectangle();
    }

    // Animation Event から呼ぶ（爆発Prefab削除）
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
