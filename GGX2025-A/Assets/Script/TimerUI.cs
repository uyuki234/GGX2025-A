using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private TextMeshProUGUI timeText;   // Hierarchy の Text (TMP) をドラッグ
    [SerializeField] private GameTimer gameTimer;        // GameTimer をドラッグ

    private void Reset()
    {
        // コンポーネントの自動補完（Inspector の Reset 時に便利）
        if (!timeText) timeText = GetComponent<TextMeshProUGUI>();
        if (!gameTimer) gameTimer = FindObjectOfType<GameTimer>();
    }

    private void Awake()
    {
        if (!timeText) timeText = GetComponent<TextMeshProUGUI>();
        if (!gameTimer) gameTimer = FindObjectOfType<GameTimer>();
    }

    private void Update()
    {
        if (!timeText || !gameTimer) return;

        // "MM:SS" の文字列をそのまま表示
        timeText.text = gameTimer.GetTimeString();
    }
}
