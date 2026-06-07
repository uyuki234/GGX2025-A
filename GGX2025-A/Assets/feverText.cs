using TMPro;
using UnityEngine;

public class feverText : MonoBehaviour
{
public TextMeshProUGUI tmp;

void FixedUpdate()
{
    if (StatusManager.Instance.isFEVER)
    {
        bool visible = (Mathf.FloorToInt(StatusManager.Instance.feverTime) % 2 == 0);
        tmp.enabled = visible;
    }
    else
    {
        tmp.enabled = false;
    }
}


}
