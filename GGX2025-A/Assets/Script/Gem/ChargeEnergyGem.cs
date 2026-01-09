using UnityEngine;

public class ChargeEnergyGem : GemBase
{
    public override void Initialize()
    {
    }

    public override void CreateParticle()
    {
        gemParticle = Resources.Load<GameObject>("ChargeEnergyParticle");
        GameObject obj = Instantiate(gemParticle, this.transform);
    }

    public override void HitPlayer()
    {
        StatusManager.Instance.chargeEnergy_base++;
        StatusManager.Instance.Cal();
    }
}
