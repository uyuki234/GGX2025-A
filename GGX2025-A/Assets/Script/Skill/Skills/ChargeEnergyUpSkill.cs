using UnityEngine;

public class ChargeEnergyUpSkill : ISkillEffect
{
    public SkillId Id => SkillId.ChargeEnergyUp;

    private const float BonusPerStack = 0.2f;

    public void OnAcquired(int stack)
    {
        StatusManager.Instance.chargeEnergy_correction = 1f + BonusPerStack * stack;
        StatusManager.Instance.Cal();
    }

    public void OnAttach(GameObject playerObject, int stack) { }
    public void OnDetach() { }
}
