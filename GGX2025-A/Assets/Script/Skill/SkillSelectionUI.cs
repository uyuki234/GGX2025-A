using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SkillCardUI[] cards;

    [Header("登場アニメーション")]
    [SerializeField] private float slideOffsetY   = -1200f;
    [SerializeField] private float cardDelay      = 0.08f;
    [SerializeField] private float animDuration   = 0.45f;
    [SerializeField] private AudioClip cardAppearSE;

    private Action<SkillId> _onChosen;

    public void Show(List<SkillId> candidates, Action<SkillId> onChosen)
    {
        _onChosen = onChosen;
        panel.SetActive(true);

        for (int i = 0; i < cards.Length; i++)
        {
            if (i < candidates.Count)
            {
                cards[i].gameObject.SetActive(true);
                cards[i].Setup(candidates[i], OnCardClicked);
            }
            else
            {
                cards[i].gameObject.SetActive(false);
            }
        }

        StartCoroutine(AnimateCardsIn(candidates.Count));
    }

    private IEnumerator AnimateCardsIn(int count)
    {
        // レイアウトを確定させてから最終位置を取得
        Canvas.ForceUpdateCanvases();

        var container = cards[0].transform.parent;
        var hlg = container.GetComponent<HorizontalLayoutGroup>();

        var finalPositions = new Vector2[count];
        for (int i = 0; i < count; i++)
            finalPositions[i] = cards[i].GetComponent<RectTransform>().anchoredPosition;

        // HLGを止めてアニメーション制御へ
        if (hlg != null) hlg.enabled = false;

        for (int i = 0; i < count; i++)
        {
            var rt = cards[i].GetComponent<RectTransform>();
            rt.anchoredPosition = finalPositions[i] + new Vector2(0f, slideOffsetY);
            StartCoroutine(SlideIn(rt, finalPositions[i], i * cardDelay));
        }

        // アニメーション完了後にHLGを再有効化
        float totalTime = (count - 1) * cardDelay + animDuration + 0.05f;
        yield return new WaitForSecondsRealtime(totalTime);

        if (hlg != null) hlg.enabled = true;
    }

    private IEnumerator SlideIn(RectTransform rt, Vector2 finalPos, float delay)
    {
        if (delay > 0f) yield return new WaitForSecondsRealtime(delay);

        if (cardAppearSE != null) SEManager.Instance.Play(cardAppearSE);

        Vector2 startPos = finalPos + new Vector2(0f, slideOffsetY);
        float elapsed = 0f;

        while (elapsed < animDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / animDuration);
            float eased = 1f - Mathf.Pow(1f - t, 3f); // Ease Out Cubic
            rt.anchoredPosition = Vector2.LerpUnclamped(startPos, finalPos, eased);
            yield return null;
        }

        rt.anchoredPosition = finalPos;
    }

    private void OnCardClicked(SkillId id)
    {
        StopAllCoroutines();
        // HLGが止まったままなら再有効化
        if (cards.Length > 0)
        {
            var hlg = cards[0].transform.parent.GetComponent<HorizontalLayoutGroup>();
            if (hlg != null) hlg.enabled = true;
        }
        panel.SetActive(false);
        _onChosen?.Invoke(id);
    }
}
