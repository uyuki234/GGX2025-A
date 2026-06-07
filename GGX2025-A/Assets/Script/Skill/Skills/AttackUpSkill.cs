using UnityEngine;

public class AttackUpSkill : ISkillEffect
{
    public SkillId Id => SkillId.AttackUp;

    private const float BonusPerStack = 0.1f;

    public void OnAcquired(int stack)
    {
        StatusManager.Instance.attack_correction = 1f + BonusPerStack * stack;
        StatusManager.Instance.Cal();
    }

    public void OnAttach(GameObject playerObject, int stack) { }
    public void OnDetach() { }
}
