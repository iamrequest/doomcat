using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple singleton, for 
public class Player : MonoBehaviour {
    #region Singleton
    static private Player _instance;
    public static Player instance {
        get {
            if (_instance == null) {
                Debug.LogError("Player does not exist, but someone is trying to access it!");
            }

            return _instance;
        }
    }

    private void Awake() {
        if (_instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    #endregion

    public Transform catTransform;
}
