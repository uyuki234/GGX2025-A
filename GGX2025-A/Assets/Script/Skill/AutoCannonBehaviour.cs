using System.Collections;
using UnityEngine;

public class AutoCannonBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private float _interval = 3f;

    private void Awake()
    {
        if (bulletPrefab == null)
            bulletPrefab = Resources.Load<GameObject>("CannonBullet");
    }
    private Coroutine _routine;

    public void UpdateStack(int stack)
    {
        _interval = Mathf.Max(0.8f, 3f - (stack - 1) * 0.5f);
        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_interval);
            FireBullet();
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab == null) return;

        // カーソルのワールド座標を取得
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 dir = (mouseWorld - transform.position).normalized;
        if (dir == Vector3.zero) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));

        var cb = bullet.GetComponent<CannonBullet>();
        if (cb != null) cb.Init(dir);
    }
}
