using UnityEngine;

public class ChargeEnergyGem : GemBase
{
    public override void Initialize()
    {

    }

    public override void HitPlayer()
    {
        StatusManager.Instance.chargeEnergy_base++;
    }
}
