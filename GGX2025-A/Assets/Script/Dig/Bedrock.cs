using UnityEngine;

public class Bedrock : MonoBehaviour
{

    [SerializeField] LayerMask digsquare;
    public AudioClip digmissSE;
    public AudioClip digSE;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((digsquare.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (StatusManager.Instance.isFEVER)
            {
                AudioSource.PlayClipAtPoint(digSE, transform.position);
                Destroy(this.gameObject);
            }
            else
            {
                AudioSource.PlayClipAtPoint(digmissSE, transform.position);
            }

        }
    }

}
