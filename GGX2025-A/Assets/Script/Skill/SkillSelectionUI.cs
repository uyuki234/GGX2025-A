using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SkillCardUI[] cards;

    private Action<SkillId> _onChosen;

    public void Show(List<SkillId> candidates, Action<SkillId> onChosen)
    {
        _onChosen = onChosen;
        panel.SetActive(true);

        for (int i = 0; i < cards.Length; i++)
        {
            if (i < candidates.Count)
            {
                var id = candidates[i];
                cards[i].gameObject.SetActive(true);
                cards[i].Setup(id, OnCardClicked);
            }
            else
            {
                cards[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnCardClicked(SkillId id)
    {
        panel.SetActive(false);
        _onChosen?.Invoke(id);
    }
}
