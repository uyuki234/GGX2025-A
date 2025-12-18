using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //移動方向
    private int moveDir = 1;

    //今空中にいるかの判定
    private bool isFall;

    //リジットボディ
    private Rigidbody2D rb;

    //バグ防止用
    private int cantResetCount;

    [Header("Ray1 Settings")]
    [SerializeField] private Vector2 ray1Start;     //スタート位置
    [SerializeField] private Vector2 ray1Dir;       //移動方向
    [SerializeField] private float maxDistance1;    //Rayの照射距離

    [Header("Ray2 Settings")]
    [SerializeField] private Vector2 ray2Start;     //スタート位置
    [SerializeField] private Vector2 ray2Dir;       //移動方向
    [SerializeField] private float maxDistance2;    //Rayの照射距離

    [Header("Ray3 Settings")]
    [SerializeField] private Vector2 ray3Start;     //スタート位置
    [SerializeField] private Vector2 ray3Dir;       //移動方向
    [SerializeField] private float maxDistance3;    //Rayの照射距離

    [Header("Jump Settings")]
    [SerializeField] private float[] powerList;
    [SerializeField] private int checkCount = 5;

    //ジャンプできるかの判定
    private bool[] isCanJump;

    //止まっているかの判定
    private bool isStop;

    //ポーズボタン
    public bool pauseButton;

    [Header("Graphics")]
    private SpriteRenderer sr;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        isFall = false;
        cantResetCount = 0;
        checkCount = 5;
        isStop = false;
        isCanJump = new bool[checkCount];
        pauseButton = false;
    }

    void FixedUpdate()
    {
        //画像の切り替え
        if (moveDir == 1)
        {
            sr.sprite = rightSprite;
        }
        if(moveDir == -1)
        {
            sr.sprite = leftSprite;
        }

        //ストップフラグが立っていなければ移動
        if (!isStop && !pauseButton)
        {
            Vector3 pos = transform.position;
            pos.x += moveDir * 0.3f * StatusManager.Instance.moveSpeed_effective*Time.deltaTime;
            transform.position = pos;
        }

            //Ray1の処理
            Ray1();

        //Ray2の処理
        Ray2();

        //Ray3の処理
        for (int i = 0; i < checkCount; i++)
        {
            Ray3(i);
        }

        //移動可能かのチェック
        if (CheckJump() == -1 && RayHitCheck(ray1Start, ray1Dir, maxDistance1, "Ground").collider != null)
        {
            isStop = true;
        }
        else
        {
            isStop = false;
        }

        cantResetCount++;
    }

    /// <summary>
    /// ジャンプの処理
    /// </summary>
    /// <param name="power">ジャンプの強さ</param>
    private void Jump(float power)
    {
        rb.AddForce(Vector2.up * power, ForceMode2D.Impulse);
    }

    //Ray1の処理
    private void Ray1()
    {
        RaycastHit2D hit1 = RayHitCheck(ray1Start, ray1Dir, maxDistance1, "Ground");
        if (hit1.collider != null && !isFall && !isStop && !pauseButton)
        {
            //どこにジャンプ出来るかチェック
            int count = CheckJump();
            if (count != -1)
            {
                //出来るならジャンプの強さをしていし、ジャンプ
                Jump(powerList[count]);
                isFall = true;
                cantResetCount = 0;
            }

        }
    }

    //Ray2の処理
    private void Ray2()
    {
        RaycastHit2D hit2 = RayHitCheck(ray2Start, ray2Dir, maxDistance2, "Ground");

        //空中にいるかの確認
        if (hit2.collider != null && isFall && cantResetCount > 5)
        {
            isFall = false;
        }
        else
        {
            isFall = true;
        }
    }

    /// <summary>
    /// Ray3の処理
    /// </summary>
    /// <param name="count">カウント</param>
    private void Ray3(int count)
    {
        //オフセットを指定
        Vector2 offSet = new Vector2(0, (float)count / 2);
        RaycastHit2D hit3 = RayHitCheck(ray3Start + offSet, ray3Dir, maxDistance3, "Ground");
        //その場所がジャン部出来るなら該当するisCanJumpにtrueを入れる
        if (hit3.collider != null && cantResetCount > 5)
        {
            isCanJump[count] = false;
        }
        else
        {
            isCanJump[count] = true;
        }
    }

    /// <summary>
    /// ジャンプ可能かのチェック
    /// </summary>
    /// <returns>ジャンプできる高さ</returns>
    private int CheckJump()
    {
        int count = 0;
        for (int i = 0; i < checkCount; i++)
        {
            //ジャンプできる場所が、2こ連続で続くまで繰り返す
            if (isCanJump[i])
            {
                count++;
            }
            else
            {
                count = 0;
            }
            if (count >= 2)
            {
                //あればジャンプできる最初の場所を返す
                return i - 1;
            }
        }
        //無ければ-1
        return -1;
    }

    /// <summary>
    /// Rayを投射し、何に当たったかをチェック
    /// </summary>
    /// <param name="rayStart">Rayのスタート位置</param>
    /// <param name="rayDir">Rayの移動方向</param>
    /// <param name="maxDistance">Rayの長さ</param>
    /// <param name="checkTag">チェックするオブジェクトのタグ</param>
    /// <returns></returns>
    private RaycastHit2D RayHitCheck(Vector2 rayStart, Vector2 rayDir, float maxDistance, string checkTag)
    {

        //移動方向に合わせて初期位置を調整
        rayStart.x *= moveDir;

        //スタート位置を生成
        Vector2 origin2 = (Vector2)transform.position + rayStart;

        //長さを設定
        Vector2 direction2 = rayDir.normalized * moveDir;

        //デバッグ用のRayを生成
        Debug.DrawRay(origin2, direction2 * maxDistance, Color.green);

        //当たったオブジェクトを返す
        int groundMask2 = LayerMask.GetMask(checkTag);
        return Physics2D.Raycast(origin2, direction2, maxDistance, groundMask2);
    }

    public void SetMoveDir(int value)
    {
        moveDir = value;
    }
}