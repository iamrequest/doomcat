using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Rigidbody cartSphereRB;
    public Damageable damagable;

    [Header("Player death management")]
    // Player death management
    public float onDeathTimeScale;

    [Tooltip("How long the player has to wait before being able to restart the game")]
    public float gameoverInputDelay;
    public GameObject gameoverUI1;
    public GameObject gameoverUI2;
    private bool isGameOver;

    private void Start() {
        isGameOver = false;
        gameoverUI1.SetActive(false);
        gameoverUI2.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update() {
        if (isGameOver && Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
        if (isGameOver && Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Slow time, start the death screen after some delay
    public void GameOver() {
        Time.timeScale = onDeathTimeScale;
        gameoverUI1.SetActive(true);
        StartCoroutine(OpenGameOverScreen());
    }

    private IEnumerator OpenGameOverScreen() {
        yield return new WaitForSecondsRealtime(gameoverInputDelay);

        // Return time scale
        Time.timeScale = 1f;

        // Enable player options
        isGameOver = true;
        gameoverUI2.SetActive(true);
    }
}
