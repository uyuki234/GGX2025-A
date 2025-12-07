using UnityEngine;
using System.Collections;

public class DigsquareController : MonoBehaviour
{
    public bool attackable;
    [SerializeField] int damage = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DisableAttackable());
        Destroy(gameObject,0.1f);
    }

    IEnumerator DisableAttackable()
    {
        yield return new WaitForSeconds(0.1f);
        attackable = false;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(attackable && other.CompareTag("Enemy")){
            EnemyStatus hp = other.GetComponent<EnemyStatus>();
            if (hp != null)
            {
                hp.SetHP(hp.GetHP()-damage); // ダメージ処理
            }
        }
    }
}
