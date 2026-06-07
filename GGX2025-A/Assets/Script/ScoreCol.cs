using UnityEngine;

public class ScoreCol : MonoBehaviour
{
    [SerializeField] int addscore;
    [SerializeField] string targetTag = "Player";

    // �v���C���[�̍��W���ꎞ�I�ɕێ����邽�߂̕ϐ�
    private Vector2 savedPlayerPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            if (StatusManager.Instance.isFEVER)
            {
                StatusManager.Instance.Score += addscore;
                StatusManager.Instance.EndFever();

                if (SkillManager.Instance != null)
                    SkillManager.Instance.OpenSkillSelection();
            }
        }
    }
}