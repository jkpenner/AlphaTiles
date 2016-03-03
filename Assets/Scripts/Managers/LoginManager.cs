using UnityEngine;
using System.Collections.Generic;
using Facebook.Unity;
using GameUp;
using UnityEngine.UI;

public class LoginManager : Singleton<LoginManager> {
    public bool IsInitialized {
        get {
            return FB.IsInitialized;
        }
    }

    public bool IsLoggedIn {
        get {
            return FB.IsLoggedIn;
        }
    }

    public enum LoginType {
        NotLoggedIn,
        ActiveFacebook,
        ActiveNoLogin,
    }
    public LoginType loginType = LoginType.NotLoggedIn;

    public SessionClient ActiveSession {
        get; private set;
    }

    private bool _accessTokenLoaded = false;
    private string _accessToken = string.Empty;
    public string AccessToken {
        get {
            if (!_accessTokenLoaded) {
                _accessToken = PlayerPrefs.GetString("PlayerToken", string.Empty);
                _accessTokenLoaded = true;
            }
            return _accessToken;
        }
        set {
            _accessToken = value;
            _accessTokenLoaded = true;
            PlayerPrefs.SetString("PlayerToken", _accessToken);
        }
    }

    public InitDelegate OnInitSuccessEvent;
    public InitDelegate OnLoginSuccessEvent;

    public Text debugText;

    public void Awake() {
        if (!FB.IsInitialized) {
            Debug.LogFormat("[{0}]: Initializing Facebook API", name);
            FB.Init(OnInitSuccess, OnHideUnity);
        }
    }

    private void OnInitSuccess() {
        Debug.LogFormat("[{0}]: Initialization is Successful", name);
        if(FB.IsLoggedIn) {
            if (OnLoginSuccessEvent != null) {
                OnLoginSuccessEvent();
            }
        } else if(!FB.IsLoggedIn && FB.IsInitialized) {
            if (OnInitSuccessEvent != null) {
                OnInitSuccessEvent();
            }
        }

        if (FB.IsLoggedIn) {
            Debug.LogFormat("[{0}]: Facebook is logged in", name);
        } else {
            Debug.LogFormat("[{0}]: Facebook is not logged in", name);
        }
    }

    private void OnHideUnity(bool isGameShown) {
        Debug.LogFormat("[{0}]: Is Game Hidden {1}", name, isGameShown.ToString());
        if (isGameShown) {
            Time.timeScale = 1f;
        } else {
            Time.timeScale = 0f;
        }
    }

    public void FacebookLogin() {
        Debug.LogFormat("[{0}]: Attempting Facebook login", name);

        if (FB.IsInitialized) {
            var permissions = new List<string>() {
                "public_profile",
            };
            FB.LogInWithReadPermissions(permissions, OnAuthComplete);
        }
    }

    private void OnAuthComplete(IResult result) {
        Debug.LogFormat("[{0}]: On Authoration Complete", name);

        if (result.Error != null) {
            Debug.LogFormat("[{0}]: {1}", name, result.Error);
        } else {
            if (FB.IsLoggedIn) {
                if (OnLoginSuccessEvent != null) {
                    OnLoginSuccessEvent();
                }
            }

            if (FB.IsLoggedIn) {
                Debug.LogFormat("[{0}]: Facebook is logged in", name);
            } else {
                Debug.LogFormat("[{0}]: Facebook is not logged in", name);
            }

            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }
    }

    private void DisplayUsername(IResult result) {
        if (result.Error != null) {
            Debug.Log(result.Error);
        } else {
            Debug.LogFormat("Username: {0}", result.ResultDictionary["first_name"]);
            if (debugText != null) {
                debugText.text = (string)result.ResultDictionary["first_name"];
            }
        }
    }

    private void DisplayProfilePic(IGraphResult result) {
        if (result.Error != null) {
            Debug.Log(result.Error);
        } else if (result.Texture != null) {
            //Image ProfilePic = ImageGO.GetComponent<Image>();
            // ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0,0,128,128), Vector2.zero);
        }
    }

    public void Initialize() {
        if (!FB.IsInitialized) {
            Debug.LogFormat("[{0}]: Initializing Facebook API", name);
            FB.Init(OnInitSuccess, OnHideUnity);
        }
    }

    private void OnAuthSuccess(SessionClient sessionClient) {
        Debug.LogFormat("Successfully Logged in to Facebook");
        ActiveSession = sessionClient;
    }

    private void OnAuthError(int statusCode, string reason) {
        Debug.LogErrorFormat("Could not login to Facebook got '{0}'.", reason);
    }
}
