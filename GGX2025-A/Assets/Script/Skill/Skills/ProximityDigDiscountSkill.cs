using UnityEngine;

public class ProximityDigDiscountSkill : ISkillEffect
{
    public SkillId Id => SkillId.ProximityDigDiscount;

    // 効果は WorldRectangleSelector が SkillManager.HasSkill / GetStack でクエリして適用する
    public void OnAcquired(int stack) { }
    public void OnAttach(GameObject playerObject, int stack) { }
    public void OnDetach() { }
}
