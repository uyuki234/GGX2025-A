using UnityEngine;

public class Danmaku1 : MonoBehaviour
{
    //発射までのクールダウン
    [SerializeField] private int _bulletCoolDown;
    private float _coolDownCounter;
    //発射間隔
    [SerializeField] private int _fierDistans;
    //発射間隔のカウンター
    private float _fierDistansCount;
    //弾の生成数
    [SerializeField] private int _fierMax;
    //弾の速度
    [SerializeField] private float _bulletSpeed;
    //弾のダメージ
    [SerializeField] private int _bulletDamage;
    // 回転速度
    [SerializeField] private int _rotateSpeed;
    //最初の発射角度
    [SerializeField] private int _startRotate;
    //弾のプレファブ
    [SerializeField] private GameObject _bulletPrefab;

    private int _nowRotate;
    private int _fierCount;

    private void Start()
    {
        _coolDownCounter = _bulletCoolDown;
    }

    private void FixedUpdate()
    {


        //クールダウンを進める
        if (isShootBullet())
        {
            _nowRotate = _startRotate;
            _fierCount = _fierMax;
        }

        //距離を測る
        float rad = _nowRotate * Mathf.Deg2Rad;
        float dx = Mathf.Sin(rad);
        float dy = Mathf.Cos(rad);
        float length = Mathf.Sqrt(dx * dx + dy * dy);

        //移動方向に変換
        if (length != 0)
        {
            dx /= length;
            dy /= length;
        }

        //Vec2に
        Vector2 moveDir = new Vector2(dx, dy);

        _fierDistansCount += 1 * Time.timeScale;

        //弾の発射
        if (_fierCount > 0 && _fierDistansCount > _fierDistans)
        {
            GameObject bulletObj = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();
            bullet.Initialize(moveDir, _bulletSpeed, _bulletDamage);

            //各変数をリセット、変更
            _nowRotate += _rotateSpeed;
            _fierDistansCount = 0;
            _fierCount--;
        }
    }

    /// <summary>
    /// 弾が発射されるか
    /// </summary>
    /// <returns>弾が発射されるか</returns>
    private bool isShootBullet()
    {
        if (_fierCount > 0) return false;

        _coolDownCounter+= 1 * Time.timeScale;
        if (_coolDownCounter >= _bulletCoolDown)
        {
            _coolDownCounter = 0;
            return true;
        }
        return false;
    }
}
