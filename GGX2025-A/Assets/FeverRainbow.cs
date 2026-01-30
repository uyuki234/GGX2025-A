using UnityEngine;

public class FeverRainbow : MonoBehaviour
{
    [SerializeField] GameObject[] maskObj;

    private void FixedUpdate()
    {
        if (maskObj == null) return;
        if(StatusManager.Instance.isFEVER)
        {
            foreach (var obj in maskObj)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            foreach (var obj in maskObj)
            {
                obj.SetActive(false);
            }
        }
    }
}
