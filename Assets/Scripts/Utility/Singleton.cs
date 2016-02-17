using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    static private T _instance;
    static private T Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<T>();
                if (_instance == null) {
                    GameObject singletonGO = new GameObject();
                    singletonGO.name = "(Singleton)" + typeof(T).ToString();
                    _instance = singletonGO.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
}
