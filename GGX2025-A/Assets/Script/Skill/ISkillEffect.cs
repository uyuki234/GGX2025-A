using UnityEngine;

public interface ISkillEffect
{
    SkillId Id { get; }
    void OnAcquired(int stack);
    void OnAttach(GameObject playerObject, int stack);
    void OnDetach();
}
