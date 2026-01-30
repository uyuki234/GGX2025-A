using TMPro;
using UnityEngine;

public class feverText : MonoBehaviour
{
public TextMeshProUGUI tmp;

void FixedUpdate()
{
    if (StatusManager.Instance.isFEVER)
    {
        bool visible = ((StatusManager.Instance.feverTime / 30) % 2 == 0);
        tmp.enabled = visible;
    }
    else
    {
        tmp.enabled = false;
    }
}


}
