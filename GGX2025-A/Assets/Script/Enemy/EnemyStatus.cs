using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField]private float maxHp = 50;
    private float currentHp;
    private GemCreatEnemy gemCreatEnemy;
    [SerializeField]private GameObject HPUI;
    //　HP表示用スライダー
    private Slider hpSlider;
    private float minSpeed=0;
    [Header("スピード制限値")]
    public float speedLimit=20;
    [Header("ダメージ換算の倍率")]
    public int times=2;
    [Header("enemy1であればチェック入れる")]
    public bool enemy1;
    private Renderer cubeRend;
    private Color defaultColor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
        if(enemy1){
            cubeRend = transform.Find("Sprite").GetComponent<Renderer>();
        }
        else{
            cubeRend = GetComponent<Renderer>();
        }
        defaultColor = cubeRend.material.color;

    }

    void Update(){
        Rigidbody2D rb = this.transform.GetComponent<Rigidbody2D>();
        //スピードの最小値の計測
        float speed = rb.linearVelocity.y;
        if(minSpeed > speed){
            minSpeed = speed;
        }
    }

    public void SetHP(float hp){
        this.currentHp=hp;

        UpdateHPValue();

        if(currentHp <= 0){
            //HideStatusUI();
            gemCreatEnemy = GetComponent<GemCreatEnemy>();
            gemCreatEnemy.ScatterObjects();
            Destroy(gameObject);
        }
    }

    public float GetHP(){
        return currentHp;
    }

    public float GetMaxHP(){
        return maxHp;
    }

    public void SetSpeed(float speed){
        this.minSpeed=speed;
    }

    public float GetSpeed(){
        return minSpeed;
    }

    public float DamageCalculation(float dm){
        float hp=GetHP()-dm;
        StartCoroutine(HitColor());
        return hp;
    }

    IEnumerator HitColor()
    {
      cubeRend.material.color = new Color(255f/255f, 60f/255f, 60f/255f, 220f/255f);
      //cubeRend.material.color = new Color(225f/255f, 225f/255f, 0f, 1f);
      yield return new WaitForSeconds(0.15f);
      cubeRend.material.color = defaultColor;
    }

    /*public bool GetAttacknum(){
        return attacknum;
    }

    public void SetAttacknum(bool a){
        attacknum=a;
    }*/

    public void HideStatusUI(){
        HPUI.SetActive(false);
    }

    public void UpdateHPValue() {
        hpSlider.value = GetHP() / GetMaxHP();
    }

    public void fallDamage(float ve){
        float sp=ve * -1;
        //速さ測定記録：1.5ブロックで約10、2ブロックで約12、3ブロックで約16、4ブロックで約19、5ブロックで約22
        if(sp>speedLimit){
            float damage = (sp-speedLimit)*times;
            SetHP(DamageCalculation(damage));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")&&!enemy1)
        {
            //落下ダメージの速度計り
            //Debug.Log("着地時の速度: " + minSpeed);
            //速さ測定記録：1.5ブロックで約10、2ブロックで約12、3ブロックで約16、4ブロックで約19、5ブロックで約22
            float fixedSpeed = minSpeed;
            minSpeed=0;
            fallDamage(fixedSpeed);
            //StartCoroutine(enemyStatus.FallDamageDelay(fixedSpeed));
        }
    }

}
