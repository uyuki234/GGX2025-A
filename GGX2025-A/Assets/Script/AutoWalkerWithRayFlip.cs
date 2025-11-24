using UnityEngine;

public class AutoWalkerWithRayFlip : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rayDistance = 0.5f;
    public float roofRayDistance = 1f;
    public float lowerRayXOffset = 0f; 
    public float lowerRayYOffset = 0f;
    public float rayHeightOffset = 0.5f; // ���Ray��Y�I�t�Z�b�g
    public Vector2 wallRayOffset = new Vector2(0, 0);
    public Vector2 roofRayOffset = new Vector2(0, 0);
    public float jumpForce = 5f;

    public GameObject leftArrow;
    public GameObject rightArrow;

    public bool isPaused = false;

    public LayerMask wallLayer;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    private Vector2 moveDirection = Vector2.right;

    void Update()
    {

        if (!isPaused)
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);


        if (isPaused)
        {
            rb.linearVelocity = rb.linearVelocity * Vector2.up;
        }

        Vector2 originLower = (Vector2)transform.position + new Vector2(lowerRayXOffset * Mathf.Sign(moveDirection.x), lowerRayYOffset);
        Vector2 wallRay = (Vector2)transform.position + new Vector2(wallRayOffset.x * Mathf.Sign(moveDirection.x),wallRayOffset.y);
        Vector2 roofRay = (Vector2)transform.position + roofRayOffset;

        //壁判定その１(下)
        RaycastHit2D lowerHit = Physics2D.Raycast(originLower, moveDirection, rayDistance, wallLayer);

        //壁判定その２(上)
        Vector2 originUpper = originLower + Vector2.up * rayHeightOffset;
        RaycastHit2D upperHit = Physics2D.Raycast(originUpper, moveDirection, rayDistance, wallLayer);

        //キャラクター足元の壁判定、反転用
        RaycastHit2D wallHit = Physics2D.Raycast(wallRay, moveDirection, rayDistance, wallLayer);

        //天井判定
        RaycastHit2D roofHit = Physics2D.Raycast(roofRay, new Vector2(0,1), roofRayDistance, wallLayer);

        Debug.DrawRay(originLower, moveDirection * rayDistance, lowerHit ? Color.red : Color.green);
        Debug.DrawRay(originUpper, moveDirection * rayDistance, upperHit ? Color.yellow : Color.cyan);
        Debug.DrawRay(wallRay, moveDirection * rayDistance, wallHit ? Color.yellow : Color.cyan);
        Debug.DrawRay(roofRay, new Vector2(0,1) * roofRayDistance, roofHit ? Color.red : Color.green);

        //目の前が壁なら反転
       if (!wallHit)
        {
            moveDirection *= -1;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }

       //下部が地面を検知し上部が壁を検知、つまり段差があると判断し、天井に頭をぶつけないならジャンプする
        else if (!lowerHit && upperHit && roofHit) 
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }


    public void ResumeMovement(Vector2 direction)
    {
        moveDirection = direction;
        spriteRenderer.flipX = (direction.x < 0);
        isPaused = false;


    }
}
