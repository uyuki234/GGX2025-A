using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField]private float maxHp = 50;
    private float currentHp;

    [SerializeField]private GameObject HPUI;
    //　HP表示用スライダー
    private Slider hpSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
    }

    public void SetHP(float hp){
        this.currentHp=hp;

        UpdateHPValue();

        if(currentHp <= 0){
            //HideStatusUI();
            Destroy(gameObject);
        }
    }

    public float GetHP(){
        return currentHp;
    }

    public float GetMaxHP(){
        return maxHp;
    }

    public void HideStatusUI(){
        HPUI.SetActive(false);
    }

    public void UpdateHPValue() {
        hpSlider.value = GetHP() / GetMaxHP();
    }
}
