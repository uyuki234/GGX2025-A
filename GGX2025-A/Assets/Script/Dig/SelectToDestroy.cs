using UnityEngine;

public class SelectToDestroy : MonoBehaviour
{
    [SerializeField] LayerMask digsquare;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((digsquare.value & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
