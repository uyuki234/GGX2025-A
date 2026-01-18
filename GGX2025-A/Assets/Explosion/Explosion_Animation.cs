using UnityEngine;

public class DeathAnimationPlayer : MonoBehaviour
{
    [Header("Explosive")]
    [SerializeField] private Animator animator;
    [SerializeField] private string deathTriggerName = "Death";

    private bool isDead = false;

    // ŠO•”‚©‚çu“|‚³‚ê‚½v‚ÆŒÄ‚Ô
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger(deathTriggerName);
        }
    }
}
