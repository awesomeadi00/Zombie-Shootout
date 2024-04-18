using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject pausedScreen;
    [SerializeField] private GameObject gameOverScreen;
    private PlayerStats playerStats;
    private AudioSource gameAudio;
    private bool paused = false;  

    void Start() {
        gameAudio = GetComponent<AudioSource>();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        SetGameSettings();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !playerStats.DeathStatus()) {
            PauseGame();
        }

        if (paused && Input.GetKeyDown(KeyCode.Return)) {
            ReturnToMenu();
        }

        if (playerStats.DeathStatus()) {
            ViewGameOverScreen();
        }
    }

    //This function will be called to pause the game. 
    public void PauseGame() {
        //This will pause the game and change the timescale to 0, meaning that time will not move and therefore nothing will move. 
        if(!paused) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            paused = true;
            pausedScreen.SetActive(true);
            Time.timeScale = 0;
        }

        //This will unpause the game and change the timescale back to 1, meaning that time will flow regularly as before. 
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            paused = false;
            pausedScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }

    //In this function, when we press the restart button, it will reload the Scene which is Prototype 5, meaning it will restart the script.
    public void ReturnToMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    //This function will simply reload the entire previous Active Scene from the beginning. 
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ViewGameOverScreen() {
        gameOverScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //This function will be called when the scene loads to start the 
    public void SetGameSettings() {
        gameAudio.volume = MainManager.Instance.volumeValue * 0.1f;
    }
}
