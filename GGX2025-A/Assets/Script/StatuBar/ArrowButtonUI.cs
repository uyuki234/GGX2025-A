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
    public PlayerMove player;

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
                player.SetMoveDir(-1);
                player.pauseButton = false;
                break;

                case moveBottonState.right:
                player.SetMoveDir(1);
                player.pauseButton = false;
                break;

                case moveBottonState.pause:
                player.pauseButton = true;
                break;

        }
    }
}
