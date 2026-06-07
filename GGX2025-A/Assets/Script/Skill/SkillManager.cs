using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : SingletonMonoBehavior<SkillManager>
{
    [SerializeField] private SkillSelectionUI skillSelectionUI;

    [Header("スキルアイコン")]
    [SerializeField] private Sprite healIcon;
    [SerializeField] private Sprite timeIcon;
    [SerializeField] private Sprite growIcon;
    [SerializeField] private Sprite specialIcon;

    private readonly Dictionary<SkillId, int> _acquiredSkills = new();
    private readonly Dictionary<SkillId, ISkillEffect> _effects = new();
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Register(new HpRecoverSkill());
        Register(new TimeExtendSkill());
        Register(new MoveSpeedUpSkill());
        Register(new AttackUpSkill());
        Register(new ChargeEnergyUpSkill());
        Register(new ViewRangeUpSkill());
        Register(new ProximityDigDiscountSkill());
        Register(new AutoCannonSkill());
    }

    private void Register(ISkillEffect effect)
    {
        _effects[effect.Id] = effect;
    }

    public void OpenSkillSelection()
    {
        Time.timeScale = 0f;
        var candidates = GenerateCandidates(3);

        if (skillSelectionUI != null)
            skillSelectionUI.Show(candidates, OnSkillChosen);
        else
            OnSkillChosen(candidates[0]);
    }

    private void OnSkillChosen(SkillId id)
    {
        AcquireSkill(id);
        Time.timeScale = 1f;
        StatusManager.Instance.StartFever();
    }

    public void AcquireSkill(SkillId id)
    {
        if (!_acquiredSkills.ContainsKey(id)) _acquiredSkills[id] = 0;
        _acquiredSkills[id]++;

        int stack = _acquiredSkills[id];
        if (_effects.TryGetValue(id, out var effect))
        {
            effect.OnAcquired(stack);
            if (_player != null) effect.OnAttach(_player, stack);
        }
    }

    public bool HasSkill(SkillId id) => _acquiredSkills.TryGetValue(id, out int v) && v > 0;
    public int GetStack(SkillId id) => _acquiredSkills.TryGetValue(id, out int v) ? v : 0;

    // ────────────────────────────────────────────
    // カテゴリ情報

    public static SkillCategory GetCategory(SkillId id) => id switch
    {
        SkillId.HpRecover            => SkillCategory.Heal,
        SkillId.TimeExtend           => SkillCategory.Time,
        SkillId.MoveSpeedUp          => SkillCategory.Growth,
        SkillId.AttackUp             => SkillCategory.Growth,
        SkillId.ChargeEnergyUp       => SkillCategory.Growth,
        SkillId.ViewRangeUp          => SkillCategory.Growth,
        SkillId.ProximityDigDiscount => SkillCategory.Growth,
        SkillId.AutoCannon           => SkillCategory.Special,
        _                            => SkillCategory.Growth
    };

    public static Color GetCategoryColor(SkillId id) => GetCategory(id) switch
    {
        SkillCategory.Heal    => new Color(0.30f, 1.00f, 0.30f),
        SkillCategory.Time    => new Color(0.67f, 0.67f, 0.67f),
        SkillCategory.Growth  => new Color(1.00f, 0.48f, 0.00f),
        SkillCategory.Special => new Color(0.27f, 0.87f, 1.00f),
        _                     => Color.white
    };

    public Sprite GetCategorySprite(SkillId id) => GetCategory(id) switch
    {
        SkillCategory.Heal    => healIcon,
        SkillCategory.Time    => timeIcon,
        SkillCategory.Growth  => growIcon,
        SkillCategory.Special => specialIcon,
        _                     => growIcon
    };

    // ────────────────────────────────────────────
    // 表示テキスト

    public static string GetSkillName(SkillId id) => id switch
    {
        SkillId.HpRecover            => "回復",
        SkillId.TimeExtend           => "時間延長",
        SkillId.MoveSpeedUp          => "スピードアップ",
        SkillId.AttackUp             => "攻撃力アップ",
        SkillId.ChargeEnergyUp       => "エネルギー回復アップ",
        SkillId.ViewRangeUp          => "射程アップ",
        SkillId.ProximityDigDiscount => "近距離掘削",
        SkillId.AutoCannon           => "自動砲台",
        _                            => id.ToString()
    };

    public static string GetSkillDescription(SkillId id) => id switch
    {
        SkillId.HpRecover            => "最大HPの50%を回復する",
        SkillId.TimeExtend           => "残り時間を60秒延長する",
        SkillId.MoveSpeedUp          => "移動速度の補正倍率が上昇する",
        SkillId.AttackUp             => "攻撃力の補正倍率が上昇する",
        SkillId.ChargeEnergyUp       => "エネルギー回復速度の補正倍率が上昇する",
        SkillId.ViewRangeUp          => "射程の補正倍率が上昇する",
        SkillId.ProximityDigDiscount => "キャラクターに近い場所ほど掘削エネルギーが少なくなる",
        SkillId.AutoCannon           => "定期的にカーソル方向へ弾を発射し、着弾点を爆発で掘削する",
        _                            => ""
    };

    // ────────────────────────────────────────────

    private List<SkillId> GenerateCandidates(int count)
    {
        var all = new List<SkillId>((SkillId[])Enum.GetValues(typeof(SkillId)));
        for (int i = all.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (all[i], all[j]) = (all[j], all[i]);
        }
        return all.GetRange(0, Mathf.Min(count, all.Count));
    }
}
