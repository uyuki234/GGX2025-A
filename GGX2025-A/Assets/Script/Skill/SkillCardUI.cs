using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image edgeImage;   // skil_edge.png — カテゴリ色を適用
    [SerializeField] private Image iconImage;   // カテゴリアイコン
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;

    private SkillId _skillId;
    private Action<SkillId> _onClicked;
    private Vector3 _defaultScale;
    private Color _catColor;

    private void Awake()
    {
        _defaultScale = transform.localScale;
    }

    public void Setup(SkillId id, Action<SkillId> onClicked)
    {
        _skillId   = id;
        _onClicked = onClicked;

        if (nameText != null) nameText.text = SkillManager.GetSkillName(id);
        if (descText  != null) descText.text = SkillManager.GetSkillDescription(id);

        _catColor = SkillManager.GetCategoryColor(id);
        if (edgeImage != null) edgeImage.color = _catColor;

        if (iconImage != null && SkillManager.Instance != null)
        {
            Sprite icon = SkillManager.Instance.GetCategorySprite(id);
            if (icon != null) iconImage.sprite = icon;
        }
    }

    public void OnPointerEnter(PointerEventData e)
    {
        transform.localScale = _defaultScale * 1.06f;
        if (edgeImage != null)
            edgeImage.color = Color.Lerp(_catColor, Color.white, 0.3f);
    }

    public void OnPointerExit(PointerEventData e)
    {
        transform.localScale = _defaultScale;
        if (edgeImage != null) edgeImage.color = _catColor;
    }

    public void OnPointerClick(PointerEventData e)
    {
        transform.localScale = _defaultScale;
        _onClicked?.Invoke(_skillId);
    }
}
