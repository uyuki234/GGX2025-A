using UnityEngine;

public class ViewRangeGem : GemBase
{
    public override void Initialize()
    {
    }

    public override void CreateParticle()
    {
        gemParticle = Resources.Load<GameObject>("ViewRangeParticle");

        GameObject obj = Instantiate(gemParticle, this.transform);
    }

    public override void HitPlayer()
    {
        StatusManager.Instance.viewRange_base++;
        StatusManager.Instance.Cal();

    }
}
