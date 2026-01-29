using UnityEngine;

public class ExplosiveEnemy : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    public void Die()
    {
        Debug.Log("Die() 呼ばれた！");

        if (explosionPrefab != null)
        {
            Debug.Log("爆発Prefabを生成するよ！");
            GameObject exp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            var anim = exp.GetComponent<ExplosionAnimation>();
            if (anim != null)
            {
                Debug.Log("ExplosionAnimation.Die() を呼ぶよ！");
                anim.Die();
            }
            else
            {
                Debug.Log("ExplosionAnimation が見つからない！");
            }
        }
        else
        {
            Debug.Log("explosionPrefab が null だよ！");
        }

        Destroy(gameObject);
    }
}
