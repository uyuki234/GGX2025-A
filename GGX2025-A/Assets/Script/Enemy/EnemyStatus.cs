using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField]private int maxHp = 50;
    [SerializeField]private int currentHp;
    [SerializeField]private int attackPower = 1;

    [SerializeField]private GameObject HPUI;
    //　HP表示用スライダー
    private Slider hpSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        //HPbar頭上に表示
        GameObject hpUIInstance = Instantiate(HPUI, transform);
        hpUIInstance.transform.localPosition = new Vector3(0, 1f, 0);
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
    }

    public void SetHP(int hp){
        this.currentHp=hp;

        UpdateHPValue();

        if(currentHp <= 0){
            HideStatusUI();
        }
    }

    public int GetHP(){
        return currentHp;
    }

    public int GetMaxHP(){
        return maxHp;
    }

    public void HideStatusUI(){
        HPUI.SetActive(false);
    }

    public void UpdateHPValue() {
        hpSlider.value = (float)GetHP() / (float)GetMaxHP();
    }
}
