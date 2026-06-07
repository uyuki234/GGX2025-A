using UnityEngine;

public class ViewRangeUpSkill : ISkillEffect
{
    public SkillId Id => SkillId.ViewRangeUp;

    private const float BonusPerStack = 0.1f;

    public void OnAcquired(int stack)
    {
        StatusManager.Instance.viewRange_correction = 1f + BonusPerStack * stack;
        StatusManager.Instance.Cal();
    }

    public void OnAttach(GameObject playerObject, int stack) { }
    public void OnDetach() { }
}
