using UnityEngine;
using TMPro;

/// <summary>
/// ゲーム用タイマー兼ゲーム進行管理
/// スコア表示とリザルト処理を追加
/// </summary>
public class GameTimer : MonoBehaviour
{
    [Header("タイマー設定")]
    /// <summary>開始時間（秒）</summary>
    public float totalTime = 180f; // 初期値 3分 = 180秒

    [Header("UI設定（プレイ画面）")]
    /// <summary>タイマー表示用のTextMeshPro</summary>
    public TextMeshProUGUI timerText;
    /// <summary>スコア表示用のTextMeshPro（追加）</summary>
    public TextMeshProUGUI scoreText;

    [Header("UI設定（リザルト画面）")]
    /// <summary>ゲーム終了時に表示するパネル（追加）</summary>
    public GameObject resultPanel;
    /// <summary>リザルト画面でのスコア表示用（追加）</summary>
    public TextMeshProUGUI resultScoreText;

    /// <summary>現在の残り時間（秒）</summary>
    public float currentTime;

    /// <summary>タイマーが実行中かどうか</summary>
    private bool isRunning = false;

    /// <summary>タイマーが終了したかどうか</summary>
    private bool isFinished = false;

    void Start()
    {
        // 初期化
        currentTime = totalTime;
        isRunning = true;
        isFinished = false;

        // リザルトパネルは隠しておく
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    void Update()
    {
        // タイマーが実行中の場合
        if (isRunning && !isFinished)
        {
            float dt = Time.timeScale > 0f ? Time.deltaTime / Time.timeScale : 0f;
            currentTime -= dt;

            // 0以下になったら終了処理
            if (currentTime <= 0f || StatusManager.Instance.currentHP <= 0)
            {
                currentTime = 0f;
                OnTimerFinished();
            }
        }

        // UIを更新（タイマーとスコア）
        UpdateTimerUI();
        UpdateScoreUI();
    }

    /// <summary>
    /// タイマーUIを更新
    /// </summary>
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = GetTimeString();
        }
    }

    /// <summary>
    /// スコアUIを更新（追加機能）
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            if (StatusManager.Instance != null)
            {
                scoreText.text = StatusManager.Instance.Score.ToString()+"pt";
            }
        }
    }

    /// <summary>
    /// タイマー終了時の処理
    /// </summary>
    private void OnTimerFinished()
    {
        isRunning = false;
        isFinished = true;

        // ゲームを停止
        Time.timeScale = 0f;

        // ログ表示
        Debug.Log("タイムアップ！");

        // リザルト表示処理を実行
        ShowResult();
    }

    /// <summary>
    /// リザルト画面を表示する処理（追加機能）
    /// </summary>
    private void ShowResult()
    {
        // リザルトパネルを表示
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        // 最終スコアをリザルト画面に反映
        if (resultScoreText != null && StatusManager.Instance != null)
        {
            resultScoreText.text = "スコア：" + StatusManager.Instance.Score.ToString();
        }
        StatusManager.Instance.isGame = false;
    }

    /// <summary>
    /// タイマーを一時停止
    /// </summary>
    public void PauseTimer()
    {
        isRunning = false;
    }

    /// <summary>
    /// タイマーを再開
    /// </summary>
    public void ResumeTimer()
    {
        if (!isFinished)
        {
            isRunning = true;
        }
    }

    /// <summary>
    /// タイマーをリセット
    /// </summary>
    public void ResetTimer()
    {
        currentTime = totalTime;
        isRunning = true;
        isFinished = false;
        Time.timeScale = 1f; // ゲーム再開

        // リザルトパネルを隠す
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 残り時間を取得（MM:SS 形式）
    /// </summary>
    public string GetTimeString()
    {
        int minutes = (int)(currentTime / 60f);
        int seconds = (int)(currentTime % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// タイマーが終了したかどうかを取得
    /// </summary>
    public bool IsFinished()
    {
        return isFinished;
    }

    public void AddTime(float seconds)
    {
        currentTime += seconds;
    }

    /// <summary>
    /// 残り時間の割合を取得（0.0～1.0）
    /// </summary>
    public float GetProgress()
    {
        return Mathf.Clamp01(currentTime / totalTime);
    }
}