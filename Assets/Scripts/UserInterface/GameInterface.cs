using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameInterface : Singleton<GameInterface> {
    public Text isCorrect;

    public GameObject menuPanel;

    public Button previousBtn;
    public Button replayBtn;
    public Button nextBtn;

	// Use this for initialization
	void Start () {
        previousBtn.onClick.AddListener(OnPreviousClick);
        replayBtn.onClick.AddListener(OnReplayClick);
        nextBtn.onClick.AddListener(OnNextClick);
	}

    public void OnPreviousClick() {

    }

    public void OnReplayClick() {

    }

    public void OnNextClick() {

    }
}
