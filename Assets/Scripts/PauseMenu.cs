using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool show;
    private bool lastPausedState;

    [SerializeField]
    private float showHideSpeed = 1f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        lastPausedState = LevelManager.Paused;
    }

    private void Update()
    {
        if (LevelManager.instance != null && LevelManager.instance.GameState == LevelManager.gameStates.playing)
        {
            if (LevelManager.Paused != lastPausedState)
            {
                lastPausedState = LevelManager.Paused;

                if (LevelManager.Paused)
                {
                    GameManager.Instance.UIManager.isMobilePanelActive(false);
                    show = true;
                }
                else
                {
                    GameManager.Instance.UIManager.isMobilePanelActive(true);
                    show = false;
                }
            }
        }
        else
        {
            if (show) // если мы были в состоянии show = true, и вышли из режима игры — скрыть
            {
                GameManager.Instance.UIManager.isMobilePanelActive(true);
                show = false;
            }
        }

        UpdateCanvasGroupProperties();
    }

    private void UpdateCanvasGroupProperties()
    {
        if (show)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, showHideSpeed * Time.unscaledDeltaTime);
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, showHideSpeed * Time.unscaledDeltaTime);
            canvasGroup.blocksRaycasts = false;
        }
    }
}
