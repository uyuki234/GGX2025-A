using UnityEngine;

public class AttackGem : GemBase
{
    public override void Initialize()
    {

    }

    public override void HitPlayer()
    {
        StatusManager.Instance.attack_base++;
        StatusManager.Instance.Cal();
    }
}
