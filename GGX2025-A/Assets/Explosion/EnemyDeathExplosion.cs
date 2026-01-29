using UnityEngine;

public class EnemyDeathExplosion : MonoBehaviour
{
    [SerializeField] private GameObject enemy;   // 監視対象の敵
    private bool hasExploded = false;

    private void Update()
    {
        if (!hasExploded && enemy == null)
        {
            hasExploded = true;

            // 爆発アニメーションを再生
            GetComponent<ExplosionAnimation>()?.Die();
        }
    }

    public void SetEnemy(GameObject target)
    {
        enemy = target;
    }
}
