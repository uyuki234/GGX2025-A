using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy3 : MonoBehaviour
{
    [SerializeField]private int _bulletCoolDown;
    private int _coolDownCounter;
    private Transform _playerTrans;
    [SerializeField] private int _fierDistans;
    private int _fierDistansCount;
    [SerializeField]private int _fierMax;
    [SerializeField]private float _bulletSpeed;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private int _viewSize;
    [SerializeField] private GameObject _bulletPrefab;
    private int _fierCount;

    public void Start()
    {
        _coolDownCounter = _bulletCoolDown;
        _playerTrans = GameObject.Find("Robot").transform;
        _coolDownCounter = 0;
    }

    private void FixedUpdate()
    {
        //‹——£‚ð‘ª‚é
        float dx = transform.position.x - _playerTrans.position.x;
        float dy = transform.position.y - _playerTrans.position.y;
        float length = Mathf.Sqrt(dx * dx + dy * dy);

        //ˆÚ“®•ûŒü‚É•ÏŠ·
        if (length != 0)
        {
            dx /= length;
            dy /= length;
        }

        //Vec2‚É
        Vector2 moveDir = new Vector2(dx, dy);

        //”ÍˆÍ‚É“ü‚Á‚Ä‚¢‚ê‚Î’e‚ð”­ŽË
        if (length < _viewSize)
        {
            if (isShootBullet())
            {
                _fierCount = _fierMax;
            }
        }

        _fierDistansCount++;

        //’e‚Ì”­ŽË
        if (_fierCount > 0 && _fierDistansCount > _fierDistans)
        {
            GameObject bulletObj = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();
            bullet.Initialize(moveDir, _bulletSpeed, _bulletDamage);

            _fierDistansCount = 0;
            _fierCount--;
        }
    }

    /// <summary>
    /// ’e‚ª”­ŽË‚³‚ê‚é‚©
    /// </summary>
    /// <returns>’e‚ª”­ŽË‚³‚ê‚é‚©</returns>
    private bool isShootBullet()
    {
        if (_fierCount > 0) return false;

        _coolDownCounter++;
        if (_coolDownCounter >= _bulletCoolDown)
        {
            _coolDownCounter = 0;
            return true;
        }
        return false;
    }
}