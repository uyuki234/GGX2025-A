using System.Collections.Generic;
using UnityEngine;

public class RandomGem : MonoBehaviour
{
    [SerializeField]private List<GameObject> gemList;
    void Start()
    {
        int randomIndex = Random.Range(0, gemList.Count);

        Instantiate(gemList[randomIndex], transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
