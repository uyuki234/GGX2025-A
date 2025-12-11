using UnityEngine;

public class TRG_UR :MonoBehaviour
{
    [SerializeField] TileReGenerate TileReGenerate;
    [SerializeField] LayerMask digsquare;
    [SerializeField] bool actived=false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!actived)
        // LayerMask にそのレイヤーが含まれているかチェック
        if ((digsquare.value & (1 << collision.gameObject.layer)) != 0)
        {
            actived = true;
            TileReGenerate.tilecol_UR = false;
            TileReGenerate.regenerate();
        }
    }
}
