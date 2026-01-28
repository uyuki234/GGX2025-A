using UnityEngine;
using TMPro;

/// <summary>
/// ゲーム用タイマー
/// Time.unscaledDeltaTime を使用してゲーム時間遅延の影響を受けない
/// </summary>
public class GameTimer : MonoBehaviour
{
    [Header("タイマー設定")]
    /// <summary>開始時間（秒）</summary>
    public float totalTime = 180f; // 初期値 3分 = 180秒

    [Header("UI設定")]
    /// <summary>タイマー表示用のTextMeshPro</summary>
    public TextMeshProUGUI timerText;

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
    }

    void Update()
    {
        // タイマーが実行中の場合
        if (isRunning && !isFinished)
        {
            // unscaledDeltaTime でカウント（Time.timeScale の影響を受けない）
            currentTime -= Time.unscaledDeltaTime;

            // 0以下になったら終了処理
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                OnTimerFinished();
            }
        }

        // UIを更新
        UpdateTimerUI();
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

        // リザルト表示関数を呼び出す（フック）
        OnGameOver();
    }

    /// <summary>
    /// ゲームオーバー時に呼ばれるフック
    /// 別スクリプト（UI管理など）から実装を差し込む
    /// </summary>
    private void OnGameOver()
    {
        // TODO: ここにリザルト表示処理を実装
        // 例: ResultManager.Instance.ShowResult();
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

    /// <summary>
    /// 残り時間の割合を取得（0.0～1.0）
    /// UI プログレスバーの更新などに使用
    /// </summary>
    public float GetProgress()
    {
        return Mathf.Clamp01(currentTime / totalTime);
    }
}
