using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    [Header("Explosive")]
    [SerializeField] private Animator animator;
    [SerializeField] private string deathTriggerName = "Death";

    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger(deathTriggerName);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Animation Event ‚©‚çŒÄ‚Î‚ê‚é
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
