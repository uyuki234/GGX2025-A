using System.Runtime.CompilerServices;
using UnityEngine;

public class DeleteBullet : MonoBehaviour
{
    private int count = 0;
    private void FixedUpdate()
    {
        count++;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (count <= 1)
        {
            if (other.CompareTag("Bullet"))
            {
                Destroy(other.gameObject);
            }
        }
        if(count > 1)
        {
            Destroy(this);
        }
    }
}
