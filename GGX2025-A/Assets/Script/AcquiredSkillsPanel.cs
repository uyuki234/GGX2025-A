using TMPro;
using UnityEngine;

public class AcquiredSkillsPanel : MonoBehaviour
{
    [SerializeField] private Transform listContainer;
    [SerializeField] private TextMeshProUGUI emptyText;
    [SerializeField] private TMP_FontAsset rowFont;

    private void OnEnable()
    {
        Refresh();
    }

    private void Refresh()
    {
        for (int i = listContainer.childCount - 1; i >= 0; i--)
            Destroy(listContainer.GetChild(i).gameObject);

        if (SkillManager.Instance == null) return;

        var skills = SkillManager.Instance.GetAcquiredSkills();

        if (emptyText != null)
            emptyText.gameObject.SetActive(skills.Count == 0);

        foreach (var kv in skills)
        {
            var row = new GameObject($"Row_{kv.Key}");
            row.transform.SetParent(listContainer, false);

            var txt = row.AddComponent<TextMeshProUGUI>();
            txt.text = kv.Value > 1
                ? $"{SkillManager.GetSkillName(kv.Key)}  ×{kv.Value}"
                : SkillManager.GetSkillName(kv.Key);
            txt.fontSize = 32f;
            txt.color = SkillManager.GetCategoryColor(kv.Key);
            if (rowFont != null) txt.font = rowFont;

            var rt = row.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(360f, 48f);
        }
    }
}
