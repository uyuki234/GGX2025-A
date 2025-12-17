using UnityEngine;
using UnityEngine.Rendering;

public class MouseInput : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 prev;
    private float moveX;
    private bool isMove;
    [SerializeField] PlayerMove playerMove;

    void Start()
    {
        pos = Input.mousePosition;
        prev = pos;
        isMove = false;
    }

    void Update()
    {
        prev = pos;
        pos = Input.mousePosition;
        moveX = pos.x - prev.x;

        if (Input.GetMouseButtonDown(2))
        {
            isMove = false;
        }

        if (Input.GetMouseButton(2))
        {
            if(moveX > 35)
            {
                isMove=true;
                playerMove.SetMoveDir(1);
            }
            if (moveX < -35)
            {
                isMove=true;
                playerMove.SetMoveDir(-1);
            }
        }
        if (Input.GetMouseButtonUp(2))
        {
            if (!isMove)
            {
                playerMove.SetMoveDir(0);
            }
        }

    }




}
