using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum ActionType { Resume, GoToTitle, ToggleSoundSettings }

    [SerializeField] private float hoverScale = 1.15f;
    public ActionType buttonAction;

    private Vector3 defaultScale;

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = defaultScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = defaultScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PauseManager.Instance == null) return;
        switch (buttonAction)
        {
            case ActionType.Resume: PauseManager.Instance.Resume(); break;
            case ActionType.GoToTitle: PauseManager.Instance.GoToTitle(); break;
            case ActionType.ToggleSoundSettings: PauseManager.Instance.ToggleSoundSettings(); break;
        }
    }
}
