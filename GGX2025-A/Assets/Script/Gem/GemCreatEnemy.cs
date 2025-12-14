using UnityEngine;

public class GemCreatEnemy : MonoBehaviour
{
    public int force=10;
    public int gemcount=3;
    private GemCreatRandom gemCreatRandom;

    public void ScatterObjects()
    {
        for (int i = 0; i < gemcount; i++)
        {
            // ランダムな位置と回転
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle*1f;
            Quaternion rot = Quaternion.identity;;

            // 生成
            gemCreatRandom = GetComponent<GemCreatRandom>();
            GameObject gem = Instantiate(gemCreatRandom.SampleGem(), pos, rot);

            // Rigidbodyがあればランダムな力を加える
            Rigidbody2D gemrb = gem.GetComponent<Rigidbody2D>();
            if (gemrb != null)
            {
                gemrb.AddForce(Random.insideUnitSphere * force, ForceMode2D.Impulse);
            }
        }
    }
}
