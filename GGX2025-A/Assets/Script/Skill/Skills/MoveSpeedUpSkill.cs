using UnityEngine;

public class MoveSpeedUpSkill : ISkillEffect
{
    public SkillId Id => SkillId.MoveSpeedUp;

    private const float BonusPerStack = 0.1f;

    public void OnAcquired(int stack)
    {
        StatusManager.Instance.moveSpeed_correction = 1f + BonusPerStack * stack;
        StatusManager.Instance.Cal();
    }

    public void OnAttach(GameObject playerObject, int stack) { }
    public void OnDetach() { }
}
