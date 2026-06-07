using UnityEngine;

public class AutoCannonSkill : ISkillEffect
{
    public SkillId Id => SkillId.AutoCannon;

    private AutoCannonBehaviour _behaviour;

    public void OnAcquired(int stack)
    {
        if (_behaviour != null) _behaviour.UpdateStack(stack);
    }

    public void OnAttach(GameObject playerObject, int stack)
    {
        if (_behaviour == null)
            _behaviour = playerObject.AddComponent<AutoCannonBehaviour>();

        _behaviour.UpdateStack(stack);
    }

    public void OnDetach()
    {
        if (_behaviour != null) Object.Destroy(_behaviour);
        _behaviour = null;
    }
}
