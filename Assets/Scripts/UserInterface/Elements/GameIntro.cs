using UnityEngine;
using System.Collections;
using System;

public class GameIntro : GameInterfaceElement {
    public AudioSource audioSource;
    public AudioClip gameIntroEnter;

    public void Update() {
        if (GameManager.Instance.ActiveState == GameState.Intro && Input.anyKeyDown) {
            GameManager.SetState(GameState.Cinematic);
            if (audioSource != null) {
                audioSource.PlayOneShot(gameIntroEnter);
            }
        }
    }

    public override void OnGameStateChange(GameState active) {
        base.OnGameStateChange(active);
    }

    public override void OnVisible() {
        base.OnVisible();
    }

    public override void OnHidden() {
        base.OnHidden();
        GameManager.SetState(GameState.Select);
    }

    public override bool IsElementVisible(GameState state) {
        return (state == GameState.Intro);
    }

    public override void OnElementVisible(GameState state) {
        
    }
}
