using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameInterface : Singleton<GameInterface> {
    private Animator animator;
    private bool _isVisable;

    public Text isCorrect;
    public Text corrects;
    public Text incorrects;

    public AudioSource audioSource;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip buttonPressClip;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

        GameManager.AddStateListener(OnGameStateChange);

        isCorrect.gameObject.SetActive(false);
	}

    public void UpdateScoreValues() {
        corrects.text = "Corrects: " + ScoreManager.Instance.GetCorrect(GameBoardManager.BoardSize);
        incorrects.text = ScoreManager.Instance.GetInCorrect(GameBoardManager.BoardSize) + " :Incorrects";
    }

    public void OnGameStateChange(GameState state) {
        if (state == GameState.GameOver) {
            UpdateScoreValues();
            isCorrect.gameObject.SetActive(true);
            PlayAudio(loseClip);
            
            isCorrect.text = "Incorrect";

            animator.SetTrigger("Show");
            _isVisable = true;
        } else if (state == GameState.GameWin) {
            UpdateScoreValues();
            ToggleMainElements(true);
            PlayAudio(winClip);

            isCorrect.text = "Correct";

            animator.SetTrigger("Show");
            _isVisable = true;
        } else if (_isVisable) {
            animator.SetTrigger("Hide");
            _isVisable = false;
        }
    }

    public void ToggleMainElements(bool value) {
        isCorrect.gameObject.SetActive(value);
    }

    private void PlayAudio(AudioClip clip) {
        if (audioSource != null) {
            audioSource.PlayOneShot(clip);
        }
    }

    public void OnHideComplete() {
        if (!GameBoardManager.Instance.IsGenerating) {
            //GameBoardManager.Instance.GenerateBoard();
        }
        isCorrect.gameObject.SetActive(false);
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
            ScoreManager.Instance.AddInCorrect(GameBoardManager.BoardSize);
        } else if (GameManager.Instance.ActiveState == GameState.GameWin) {
            Debug.Log("Adding to Correct Score");
            ScoreManager.Instance.AddCorrect(GameBoardManager.BoardSize);
        }
        UpdateScoreValues();
    }
}
