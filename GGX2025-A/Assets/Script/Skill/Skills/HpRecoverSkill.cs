using UnityEngine;

public class HpRecoverSkill : ISkillEffect
{
    public SkillId Id => SkillId.HpRecover;

    public void OnAcquired(int stack)
    {
        float heal = StatusManager.Instance.maxHP * 0.5f;
        StatusManager.Instance.currentHP =
            Mathf.Clamp(StatusManager.Instance.currentHP + heal, 0f, StatusManager.Instance.maxHP);
    }

    public void OnAttach(GameObject playerObject, int stack) { }
    public void OnDetach() { }
}
