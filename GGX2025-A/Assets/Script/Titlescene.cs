using UnityEngine;
using UnityEngine.SceneManagement;

public class Titlescene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    [Header("プレイヤーのタグ")]
    [SerializeField] private string targetTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("シーン名が設定されていません", gameObject);
            }
        }
    }
}