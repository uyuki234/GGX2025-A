using UnityEngine;

public class SurfaceAttack : MonoBehaviour
{
    [SerializeField] int attackDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            StatusManager.Instance.TakeDamage(attackDamage, other.gameObject);
    }
}

