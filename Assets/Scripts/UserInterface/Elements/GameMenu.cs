using UnityEngine;
using UnityEngine.UI;

public class GameMenu : GameInterfaceElement {
    public Button home;
    public Button restart;
    public Button next;
    public Button previous;

    public void Start() {
        home.onClick.AddListener(OnHomeClick);
        restart.onClick.AddListener(OnRestartClick);
        next.onClick.AddListener(OnNextClick);
        previous.onClick.AddListener(OnPreviousClick);
    }

    public override bool IsElementVisible(GameState state) {
        return  state == GameState.Active || 
                state == GameState.GameOver || 
                state == GameState.GameWin ||
                state == GameState.Checking;
    }

    public override void OnElementVisible(GameState state) {
        if (state == GameState.Checking) {
            canvasGroup.interactable = false;
        } else {
            canvasGroup.interactable = true;
        }

        if (GameBoardManager.BoardSize <= GameBoardManager.BoardSizeMin) {
            previous.gameObject.SetActive(false);
        } else {
            previous.gameObject.SetActive(true);
        }

        if (GameBoardManager.BoardSize >= GameBoardManager.BoardSizeMax) {
            next.gameObject.SetActive(false);
        } else {
            next.gameObject.SetActive(true);
        }
    }

    public void OnHomeClick() {
        GameManager.SetState(GameState.Intro);
    }

    public void OnRestartClick() {
        GameBoardManager.Instance.GenerateBoard(GameBoardManager.BoardSize);
    }

    public void OnNextClick() {
        GameBoardManager.Instance.GenerateBoard(GameBoardManager.BoardSize + 1);
    }

    public void OnPreviousClick() {
        GameBoardManager.Instance.GenerateBoard(GameBoardManager.BoardSize - 1);
    }
}
