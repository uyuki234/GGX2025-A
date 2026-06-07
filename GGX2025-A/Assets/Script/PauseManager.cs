using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject soundSettingsPanel;
    [SerializeField] private GameTimer gameTimer;

    private bool isPaused = false;

    public static PauseManager Instance { get; private set; }
    public bool IsPaused => isPaused;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (StatusManager.Instance != null && !StatusManager.Instance.isGame) return;
        if (gameTimer != null && gameTimer.IsFinished()) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null) pausePanel.SetActive(true);
        if (soundSettingsPanel != null) soundSettingsPanel.SetActive(false);
        if (gameTimer != null) gameTimer.PauseTimer();

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var pm = player.GetComponent<PlayerMove>();
            if (pm != null) pm.pauseButton = true;
        }
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameTimer != null) gameTimer.ResumeTimer();

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var pm = player.GetComponent<PlayerMove>();
            if (pm != null) pm.pauseButton = false;
        }
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    public void ToggleSoundSettings()
    {
        if (soundSettingsPanel != null)
            soundSettingsPanel.SetActive(!soundSettingsPanel.activeSelf);
    }
}
