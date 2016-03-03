using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameDiffSelect : GameInterfaceElement {
    public Button diff02;
    public Button diff03;
    public Button diff04;
    public Button diff05;
    public Button diff06;
    public Button diff07;
    public Button diff08;
    public Button diff09;
    public Button diff10;
    public Button diff11;
    public Button diff12;
    public Button diff13;

    public AudioSource audioSource;
    public AudioClip selectSuccess;
    public AudioClip selectFailure;

    public void Start() {
        diff02.onClick.AddListener(OnDiff02Click);
        diff03.onClick.AddListener(OnDiff03Click);
        diff04.onClick.AddListener(OnDiff04Click);
        diff05.onClick.AddListener(OnDiff05Click);
        diff06.onClick.AddListener(OnDiff06Click);
        diff07.onClick.AddListener(OnDiff07Click);
        diff08.onClick.AddListener(OnDiff08Click);
        diff09.onClick.AddListener(OnDiff09Click);
        diff10.onClick.AddListener(OnDiff10Click);
        diff11.onClick.AddListener(OnDiff11Click);
        diff12.onClick.AddListener(OnDiff12Click);
        diff13.onClick.AddListener(OnDiff13Click);
    }

    public override void OnHidden() {
        base.OnHidden();
        GameBoardManager.Instance.GenerateBoard();
    }

    public override bool IsElementVisible(GameState state) {
        return (state == GameState.Select);
    }

    public override void OnElementVisible(GameState state) {

    }

    public void StartDifficulty(int diff) {
        Debug.Log(string.Format("[{0}]: Setting Game Difficulty to {1}", this.name, diff));
        GameManager.SetState(GameState.Cinematic);
        GameBoardManager.Instance.SetBoardSize(diff);
    }

    public void PlaySelectFailureClip() {
        if (audioSource != null) {
            audioSource.PlayOneShot(selectFailure);
        }
    }

    public void PlaySelectSuccessClip() {
        if (audioSource != null) {
            audioSource.PlayOneShot(selectSuccess);
        }
    }

    public void OnDiff02Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(2)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(2);
        }
    }

    public void OnDiff03Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(3)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(3);
        }
    }

    public void OnDiff04Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(4)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(4);
        }
    }

    public void OnDiff05Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(5)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(5);
        }
    }

    public void OnDiff06Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(6)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(6);
        }
    }

    public void OnDiff07Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(7)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(7);
        }
    }

    public void OnDiff08Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(8)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(8);
        }
    }

    public void OnDiff09Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(9)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(9);
        }
    }

    public void OnDiff10Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(10)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(10);
        }
    }

    public void OnDiff11Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(11)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(11);
        }
    }

    public void OnDiff12Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(12)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(12);
        }
    }

    public void OnDiff13Click() {
        Debug.Log(string.Format("[{0}]: Click Difficulty Button", this.name));
        if (GameManager.Instance.IsDifficultyLocked(13)) {
            PlaySelectFailureClip();
        } else {
            PlaySelectSuccessClip();
            StartDifficulty(13);
        }
    }
}
