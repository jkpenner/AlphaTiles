using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameInterface : Singleton<GameInterface> {
    private Animator animator;
    private bool _isVisable;

    public Text isCorrect;
    public Text corrects;
    public Text incorrects;

    public GameObject menuPanel;

    public Button previousBtn;
    public Button replayBtn;
    public Button nextBtn;

    public AudioSource audioSource;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip buttonPressClip;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

        GameManager.Instance.OnGameStateChange +=
            OnGameStateChange;

        isCorrect.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(false);

        previousBtn.onClick.AddListener(OnPreviousClick);
        replayBtn.onClick.AddListener(OnReplayClick);
        nextBtn.onClick.AddListener(OnNextClick);        
	}

    public void UpdateScoreValues() {
        corrects.text = "Corrects: " + ScoreManager.Instance.GetCorrect(GameManager.Instance.boardSize);
        incorrects.text = ScoreManager.Instance.GetInCorrect(GameManager.Instance.boardSize) + " :Incorrects";
    }

    public void OnGameStateChange(GameState state) {
        if (state == GameState.GameOver) {
            UpdateScoreValues();
            isCorrect.gameObject.SetActive(true);
            menuPanel.gameObject.SetActive(true);

            if (audioSource != null) {
                audioSource.PlayOneShot(loseClip);
            }

            isCorrect.text = "Incorrect";
            if (GameManager.Instance.boardSize <= 2) {
                previousBtn.gameObject.SetActive(false);
            } else {
                previousBtn.gameObject.SetActive(true);
            }
            nextBtn.gameObject.SetActive(false);
            animator.SetTrigger("Show");
            _isVisable = true;
        } else if (state == GameState.GameWin) {
            UpdateScoreValues();
            isCorrect.gameObject.SetActive(true);
            menuPanel.gameObject.SetActive(true);

            if (audioSource != null) {
                audioSource.PlayOneShot(winClip);
            }

            isCorrect.text = "Correct";
            if (GameManager.Instance.boardSize <= 2) {
                previousBtn.gameObject.SetActive(false);
            } else {
                previousBtn.gameObject.SetActive(true);
            }
            nextBtn.gameObject.SetActive(true);
            animator.SetTrigger("Show");
            _isVisable = true;
        } else if (_isVisable) {
            animator.SetTrigger("Hide");
            _isVisable = false;
        }
    }

    public void OnPreviousClick() {
        GameManager.Instance.ModifyBoardSize(-1);
        GameManager.Instance.ActiveState = GameState.Cinematic;
        PlayButtonPressAudio();
    }

    public void OnReplayClick() {
        GameManager.Instance.ActiveState = GameState.Cinematic;
        PlayButtonPressAudio();
    }

    public void OnNextClick() {
        GameManager.Instance.ModifyBoardSize(1);
        GameManager.Instance.ActiveState = GameState.Cinematic;
        PlayButtonPressAudio();
    }

    private void PlayButtonPressAudio() {
        if (audioSource != null) {
            audioSource.PlayOneShot(buttonPressClip);
        }
    }

    public void OnHideComplete() {
        if (!GameManager.Instance.IsGenerating) {
            GameManager.Instance.GenerateBoard();
        }
        isCorrect.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(false);
    }

    public void OnPlayScoreAnim() {
        if (GameManager.Instance.ActiveState == GameState.GameOver) {
            Debug.Log("Play Incorrect Anim");
            animator.SetTrigger("Incorrect");
        } else if (GameManager.Instance.ActiveState == GameState.GameWin) {
            Debug.Log("Play Correct Anim");
            animator.SetTrigger("Correct");
        }
    }

    public void OnChangeScore() {
        if (GameManager.Instance.ActiveState == GameState.GameOver) {
            Debug.Log("Adding To Incorrect Score");
            ScoreManager.Instance.AddInCorrect(GameManager.Instance.boardSize);
        } else if (GameManager.Instance.ActiveState == GameState.GameWin) {
            Debug.Log("Adding to Correct Score");
            ScoreManager.Instance.AddCorrect(GameManager.Instance.boardSize);
        }
        UpdateScoreValues();
    }
}
