using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [SerializeField]private int _bulletCoolDown;
    private int _coolDownCounter;

    public void Initialize()
    {
        _coolDownCounter = 0;
    }

    private void FixedUpdate()
    {
        if (isShootBullet())
        {

        }
    }

    /// <summary>
    /// ’e‚ª”­ŽË‚³‚ê‚é‚©
    /// </summary>
    /// <returns>’e‚ª”­ŽË‚³‚ê‚é‚©</returns>
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
