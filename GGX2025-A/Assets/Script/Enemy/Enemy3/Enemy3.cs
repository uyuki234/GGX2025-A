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
    private int _fierCount;

    public void Initialize()
    {
        _playerTrans = GameObject.Find("Robot").transform;
        _coolDownCounter = 0;
    }

    private void FixedUpdate()
    {
        if (isShootBullet())
        {
            _fierCount = _fierMax;
        }
        _fierDistansCount++;
        if (_fierCount > 0 && _fierDistansCount > _fierDistans)
        {
            float dx = transform.position.x - _playerTrans.position.x;
            float dy = transform.position.y - _playerTrans.position.y;
            float length = Mathf.Sqrt(dx * dx + dy * dy);//’·‚³ŒvZsqrt‚Í•½•ûª(ƒ‹[ƒg)
            if (length != 0)
            {
                dx /= length;
                dy /= length;
            }
            Vector2 moveDir = new Vector2(dx, dy);
            EnemyBulletManager.Instance.CreateEnemyBullet(this.transform.position,moveDir);
            _fierDistansCount = 0;
            _fierCount--;
        }
    }

    /// <summary>
    /// ’e‚ª”­Ë‚³‚ê‚é‚©
    /// </summary>
    /// <returns>’e‚ª”­Ë‚³‚ê‚é‚©</returns>
    private bool isShootBullet()
    {
        _coolDownCounter++;
        if (_coolDownCounter >= _bulletCoolDown)
        {
            _coolDownCounter = 0;
            return true;
        }
        return false;
    }
}