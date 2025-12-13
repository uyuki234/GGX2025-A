using UnityEngine;

public class MoveSpeedGem : GemBase
{
    public override void Initialize()
    {

    }

    public override void HitPlayer()
    {
        StatusManager.Instance.moveSpeed_base++;
        StatusManager.Instance.Cal();
    }
}
