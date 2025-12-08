using UnityEngine;
using UnityEngine.UI; // Å© UIópñºëOãÛä‘Çí«â¡

public class ArrowButtonUI : MonoBehaviour
{
    private enum moveBottonState
    {
        right,
        left,
        pause
    }
    [SerializeField] moveBottonState state;
    public AutoWalkerWithRayFlip player;

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnArrowClicked);
        }
        else
        {
            Debug.LogWarning("Button component not found on " + gameObject.name);
        }
    }

    void OnArrowClicked()
    {
        switch (state)
        {
            case moveBottonState.left:
                player.ResumeMovement(Vector2.left);
                break;

                case moveBottonState.right:
                player.ResumeMovement(Vector2.right);
                break;

                case moveBottonState.pause:
                player.isPaused = true;
                break;

        }
    }
}
