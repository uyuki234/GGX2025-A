using UnityEngine;

public class ViewRangeGem : GemBase
{
    public override void Initialize()
    {

    }

    public override void HitPlayer()
    {
        StatusManager.Instance.viewRange_base++;
    }
}
