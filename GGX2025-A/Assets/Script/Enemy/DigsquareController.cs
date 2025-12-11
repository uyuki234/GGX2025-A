using UnityEngine;
using System.Collections;

public class DigsquareController : MonoBehaviour
{
    public bool attackable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackable=true;
        StartCoroutine(DisableAttackable());
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
                hp.SetHP(hp.GetHP()-StatusManager.Instance.attack_effective); // ダメージ処理
                attackable=false;
            }
        }
    }
}
