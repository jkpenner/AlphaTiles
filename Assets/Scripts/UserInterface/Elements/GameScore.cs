using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameScore : GameInterfaceElement {
    public Text isCorrect;
    public Text corrects;
    public Text incorrects;

    public override bool IsElementVisible(GameState state) {
        return state == GameState.GameOver ||
            state == GameState.GameWin;
    }

    public override void OnElementVisible(GameState state) {
        switch (state) {
            case GameState.GameWin:
                isCorrect.text = "Correct";
                animator.SetTrigger("OnCorrect");
                break;
            case GameState.GameOver:
                isCorrect.text = "Incorrect";
                animator.SetTrigger("OnIncorrect");
                break;
        }
    }

    public void Start() {
        UpdateScoreValues();
    }

    public override void OnVisible() {
        base.OnVisible();
        UpdateScoreValues();
    }

    public override void OnHidden() {
        base.OnHidden();
        UpdateScoreValues();
    }

    public void UpdateScoreValues() {
        corrects.text = "Corrects: " + ScoreManager.Instance.GetCorrect(GameBoardManager.BoardSize);
        incorrects.text = ScoreManager.Instance.GetInCorrect(GameBoardManager.BoardSize) + " :Incorrects";
    }
}
