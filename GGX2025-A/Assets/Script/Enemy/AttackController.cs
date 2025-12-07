using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] GameObject digsquareprefab; // 生成するPrefab
    public float spawnInterval = 0.1f;
    private Vector3 pos_f;
    private Vector3 pos_s;

    // Update is called once per frame
    void Update()
    {
        // 左クリックを押した瞬間だけ呼ばれる
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos_s = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos_s.z = 0;
        }

        // 左クリックを離した瞬間だけ呼ばれる
        if(Input.GetMouseButtonUp(0)){
            Vector3 pos_f = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos_f.z = 0;
            InstansDigsquare();
        }

    }

    public void InstansDigsquare(){
        pos_f.x = (pos_f.x-pos_s.x)/2;
        pos_f.y = (pos_f.y-pos_s.y)/2;
        pos_f.z = 0;
        GameObject digsquare = Instantiate(digsquareprefab, pos_f, Quaternion.identity);
        digsquare.transform.localScale = new Vector3(Mathf.Abs(pos_f.x-pos_s.x), Mathf.Abs(pos_f.y-pos_s.y), 1f);
    }
}
