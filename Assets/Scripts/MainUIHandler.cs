using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private Button menuButton; 
    [SerializeField] private GameObject pausedScreen;
    private bool paused = false;
    public bool gameIsActive = false;   

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            PauseGame();
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
            menuButton.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        //This will unpause the game and change the timescale back to 1, meaning that time will flow regularly as before. 
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            paused = false;
            pausedScreen.SetActive(false);
            menuButton.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    //In this function, when we press the restart button, it will reload the Scene which is Prototype 5, meaning it will restart the script.
    public void ReturnToMenu() {
        gameIsActive = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
