using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DigsquareController : MonoBehaviour
{
    private bool attackable;
    private List<GameObject> hitenemy = new List<GameObject>();

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
            if (hp != null && !hitenemy.Contains(other.gameObject))
            {
                hp.SetHP(hp.GetHP()-StatusManager.Instance.attack_effective); // ダメージ処理
                hitenemy.Add(other.gameObject);
            }
        }
    }
}
