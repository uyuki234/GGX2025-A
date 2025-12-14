using UnityEngine;

public class TileReGenerate : MonoBehaviour
{
    [SerializeField] GameObject tilemask_point;
    [SerializeField] GameObject tilemask_rect;
    [SerializeField] GameObject tilemask_L;
    [SerializeField] GameObject tilemask_square;
    [SerializeField] GameObject tilemask_check;
    [SerializeField] GameObject currentmask;

    public bool tilecol_UL = true;//左上
    public bool tilecol_UR = true;//右上
    public bool tilecol_LL = true;//左下
    public bool tilecol_LR = true;//右下

    [SerializeField] int pattern;

    public AudioClip digSE;


    public void regenerate()
    {
        if (currentmask != null)
            Destroy(currentmask);

        // プレハブと回転角度を取得
        (GameObject prefab, float rotZ) = SelectPrefabAndRotation();

        // 指定した回転角で生成
        currentmask = Instantiate(
            prefab,
            transform.position,
            Quaternion.Euler(0, 0, rotZ),
            transform
        );
    }
    private (GameObject prefab, float rotZ) SelectPrefabAndRotation()
    {
        pattern = 0;
        if (tilecol_UL) pattern += 1;
        if (tilecol_UR) pattern += 2;
        if (tilecol_LL) pattern += 4;
        if (tilecol_LR) pattern += 8;

        if (pattern == 0)
        {
            AudioSource.PlayClipAtPoint(digSE, transform.position);
            ParticleController.Instance.PlayDestroyEffect(transform.position);
            Destroy(gameObject);
        }


        switch (pattern)
        {
            // 左上
            case 1:
                return (tilemask_point, -90f);

            // 右上
            case 2:
                return (tilemask_point, -180f);

            //上二つ
            case 3: 
                return (tilemask_rect, -180f);

            //左下
            case 4:
                return (tilemask_point, 0f);

            //左二つ
            case 5:
                return (tilemask_rect, -90f);

            //左下と右上
            case 6:
                return (tilemask_check, 0f);

            //右下以外
            case 7:
                return (tilemask_L, -90f);

            //右下
            case 8:
                return (tilemask_point, 90f);

            //左上と右下
            case 9:
                return (tilemask_check, 90f);

            //右二つ
            case 10:
                return (tilemask_rect, 90f);

            //左下以外
            case 11:
                return (tilemask_L, 180f);

            //下二つ
            case 12:
                return (tilemask_rect, 0f);

            //右上以外
            case 13:
                return (tilemask_L, 0f);

            //左上以外
            case 14:
                return (tilemask_L, 90f);

            //全部あり
            default:
                return (tilemask_square, 0f);
        }
    }
}
