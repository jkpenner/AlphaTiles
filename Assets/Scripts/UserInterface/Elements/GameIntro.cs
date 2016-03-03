using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameIntro : GameInterfaceElement {
    public AudioSource audioSource;
    public AudioClip gameIntroEnter;

    public enum IntroState {
        AttemptingLogin,
        LoginType,
        ReadyToPlay,
    }
    public IntroState state = IntroState.AttemptingLogin;

    public GameObject tapToPlay;
    public Button facebookLoginBtn;
    public Button noLoginBtn;

    public void Start() {
        LoginManager.Instance.OnInitSuccessEvent += OnInitSuccess;
        LoginManager.Instance.OnLoginSuccessEvent += OnLoginSuccess;
        LoginManager.Instance.Initialize();

        facebookLoginBtn.onClick.AddListener(OnFacebookLoginClick);
        noLoginBtn.onClick.AddListener(OnNoLoginClick);
    }

    public void Update() {
        if (state == IntroState.ReadyToPlay) {
            if (GameManager.Instance.ActiveState == GameState.Intro && Input.anyKeyDown) {
                GameManager.SetState(GameState.Cinematic);
                if (audioSource != null) {
                    audioSource.PlayOneShot(gameIntroEnter);
                }
            }

            tapToPlay.SetActive(true);
            facebookLoginBtn.gameObject.SetActive(false);
            noLoginBtn.gameObject.SetActive(false);
        } else if (state == IntroState.LoginType) {
            tapToPlay.SetActive(false);
            facebookLoginBtn.gameObject.SetActive(true);
            noLoginBtn.gameObject.SetActive(true);
        } else {
            tapToPlay.SetActive(false);
            facebookLoginBtn.gameObject.SetActive(false);
            noLoginBtn.gameObject.SetActive(false);
        }
    }

    public override void OnGameStateChange(GameState active) {
        base.OnGameStateChange(active);
    }

    public override void OnVisible() {
        base.OnVisible();

        if (LoginManager.Instance.loginType == LoginManager.LoginType.NotLoggedIn) {
            if (!LoginManager.Instance.IsInitialized) {
                LoginManager.Instance.Initialize();
            } else {
                Debug.LogFormat("[{0}]: Login Manager is already Initialized", name);
            }

            if (!LoginManager.Instance.IsLoggedIn) {
                // Display Login Options
            } else {
                Debug.LogFormat("[{0}]: User in not logged in", name);
                // Display Tap to Play Message
            }
        } else {
            state = IntroState.ReadyToPlay;
        }
    }

    public void OnInitSuccess() {
        if (!LoginManager.Instance.IsLoggedIn) {
            state = IntroState.LoginType;
        }
    }

    public void OnLoginSuccess() {
        if (LoginManager.Instance.IsLoggedIn) {
            state = IntroState.ReadyToPlay;
            LoginManager.Instance.loginType = LoginManager.LoginType.ActiveFacebook;
        }
    }

    public void OnFacebookLoginClick() {
        if (LoginManager.Instance.IsInitialized) {
            if (!LoginManager.Instance.IsLoggedIn) {
                LoginManager.Instance.FacebookLogin();
            }
        } else {
            Debug.LogFormat("[{0}]: Tried to Login to Facebook, but API is not initialized", name);
        }
    }

    public void OnNoLoginClick() {
        LoginManager.Instance.loginType = LoginManager.LoginType.ActiveNoLogin;
        state = IntroState.ReadyToPlay;
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
