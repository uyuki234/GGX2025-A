using UnityEngine;

public class GemCreatRandom : MonoBehaviour
{
    public GameObject[] gemPrefabs;

    public GameObject SampleGem(){
        int index = Random.Range(0,gemPrefabs.Length);
        return gemPrefabs[index];
    }
}
