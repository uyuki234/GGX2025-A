using UnityEngine;

public class MoveSpeedGem : GemBase
{
    public override void Initialize()
    {
    }

    public override void CreateParticle()
    {
        gemParticle = Resources.Load<GameObject>("MoveSpeedParticle");
        GameObject obj = Instantiate(gemParticle, this.transform);
    }

    public override void HitPlayer()
    {
        StatusManager.Instance.moveSpeed_base++;
        StatusManager.Instance.Cal();
    }
}
