using UnityEngine;

public class GameManager : Singleton<GameManager> {
    protected GameManager() { }

    public delegate void GameStateEvent(GameState state);
    private GameStateEvent OnGameStateChange;

    private GameState _activeState = GameState.None;
    public GameState ActiveState {
        get {
            return _activeState;
        }
        set {
            if (_activeState != value) {
                _activeState = value;
                Debug.Log(string.Format("[{0}]: Setting Active State to {1}", name, ActiveState.ToString()));
                if (OnGameStateChange != null) {
                    OnGameStateChange(_activeState);
                }
            }
        }
    }

    static public void AddStateListener(GameStateEvent del) {
        Debug.Log(string.Format("[{0}]: Adding State Listener ({1})", Instance.name, del.Target.ToString()));
        Instance.OnGameStateChange += del;
	}

    static public void RemoveStateListener(GameStateEvent del) {
        Debug.Log(string.Format("[{0}]: Removing State Listener ({1})", Instance.name, del.ToString()));
        Instance.OnGameStateChange -= del;
    }

    public void Start() {
        ActiveState = GameState.Intro;
    }

    public bool IsDifficultyLocked(int diff) {
        return false;
    }

    static public void SetState(GameState state) {
        if (Instance != null) {
            Instance.ActiveState = state;
        }
    }
    
}