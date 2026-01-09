using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour
{
    public PlayerMove playerMoves;

    void OnMouseDown()
    {
        if (playerMoves != null)
        {
            playerMoves.SetMoveDir(-1);
            Debug.Log($"{gameObject.name} がクリックされました！");
        }
    }
}
