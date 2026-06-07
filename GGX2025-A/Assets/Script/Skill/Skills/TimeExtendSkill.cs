using UnityEngine;

public class TimeExtendSkill : ISkillEffect
{
    public SkillId Id => SkillId.TimeExtend;

    private const float ExtendSeconds = 60f;

    public void OnAcquired(int stack)
    {
        var timer = Object.FindFirstObjectByType<GameTimer>();
        if (timer != null) timer.AddTime(ExtendSeconds);
    }

    public void OnAttach(GameObject playerObject, int stack) { }
    public void OnDetach() { }
}
