using UnityEngine;

public class ExplosiveEnemy : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    public void Die()
    {
        if (explosionPrefab != null)
        {
            GameObject exp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            
        }

        Destroy(gameObject);
    }
}
