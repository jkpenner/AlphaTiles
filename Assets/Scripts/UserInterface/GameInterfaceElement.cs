using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public abstract class GameInterfaceElement : MonoBehaviour {
    protected Animator animator;
    protected CanvasGroup canvasGroup;
    private bool _isVisible = false;

    private void Awake() {
        animator = GetComponent<Animator>();

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        _isVisible = false;

        GameManager.AddStateListener(OnGameStateChange);
    }

    public abstract bool IsElementVisible(GameState state);
    public abstract void OnElementVisible(GameState state);

    public virtual void OnGameStateChange(GameState active) {
        //Debug.Log(string.Format("[{0}]: OnGameStateChange {1}", this.name, active.ToString()));
        if (IsElementVisible(active)) {
            if (_isVisible == false) {
                Debug.Log(string.Format("[{0}]: Showing User Interface", this.name));
                if (animator != null) {
                    animator.SetTrigger("OnShow");
                }
                OnElementVisible(active);
                _isVisible = true;
            }
        } else {
            if (_isVisible == true) {
                Debug.Log(string.Format("[{0}]: Hiding User Interface", this.name));
                if (animator != null) {
                    animator.SetTrigger("OnHide");
                }
                _isVisible = false;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    public virtual void OnVisible() {
        Debug.Log(string.Format("[{0}]: On Visible", this.name));
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void OnHidden() {
        Debug.Log(string.Format("[{0}]: On Hidden", this.name));
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
