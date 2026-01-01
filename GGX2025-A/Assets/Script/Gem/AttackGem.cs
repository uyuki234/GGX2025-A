using UnityEngine;

public class AttackGem : GemBase
{
    public override void Initialize()
    {
    }

    public override void CreateParticle()
    {
        gemParticle = Resources.Load<GameObject>("AttackParticle");
        GameObject obj = Instantiate(gemParticle, this.transform);
    }

    public override void HitPlayer()
    {
        StatusManager.Instance.attack_base++;
        StatusManager.Instance.Cal();
    }
}
