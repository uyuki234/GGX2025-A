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
                SEManager.Instance.Play(digSE);
                Destroy(this.gameObject);
            }
            else
            {
                SEManager.Instance.Play(digmissSE);
            }

        }
    }

}
