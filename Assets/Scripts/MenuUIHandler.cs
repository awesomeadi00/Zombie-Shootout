using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    //These are variables for the buttons: 
    public Button startButton;
    public Button instructionsButton;
    public Button settingsButton;
    public Button exitButton;

    //Boss variables: 
    public Button bossButton;
    public TextMeshProUGUI bossButtonText;
    private bool isBoss = true;

    //These are variables for the screens on the canvas
    public GameObject titleScreen;
    public GameObject instructionsScreen;
    public GameObject settingsScreen;

    private AudioSource menuAudio;
    public AudioClip buttonClick;
    public Slider volSlider;

    //Variable to check which screen the user is on, 0 = title screen, 1 = instruction screen, 2 = settings screen
    private int gameScreen;


    void Start() {
        gameScreen = 0;
        menuAudio = GetComponent<AudioSource>();
    }

    void Update() {
        //If it's not the boss mode: 
        if(!isBoss) {
            bossButton.GetComponent<Image>().color = Color.red;
            bossButtonText.text = "De-activated";
        }

        //Else if it is the boss mode: 
        else if(isBoss) {
            bossButton.GetComponent<Image>().color = Color.green;
            bossButtonText.text = "Activated";
        }
    }

    //This function will be called to start the Main Scene
    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    //This function will activate the instructions screen when the instructions button is pressed
    public void ViewInstructionsScreen() {
        menuAudio.PlayOneShot(buttonClick, 2.0f);
        titleScreen.SetActive(false);
        instructionsScreen.SetActive(true);
        gameScreen = 1;
    }

    //This function will activate the settings screen when the settings button is pressed
    public void ViewSettingsScreen() {
        menuAudio.PlayOneShot(buttonClick, 2.0f);
        titleScreen.SetActive(false);
        settingsScreen.SetActive(true);
        gameScreen = 2;
    }

    //This function will allow the user to quit the game. 
    public void ExitGame() {
    #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();  
    #else
        Application.Quit();  
    #endif
    }

    //This function will allow the user to navigate back to the Menu.
    public void BackButton() {
        menuAudio.PlayOneShot(buttonClick, 2.0f);
        //If the user is on the instruction screen, then make it false and return to title screen state
        if(gameScreen == 1) {
            instructionsScreen.SetActive(false);
            titleScreen.SetActive(true);
            gameScreen = 0;
        }

        //If the user is on the settings screen, then make it false and return to title screen state
        else if(gameScreen == 2) {
            settingsScreen.SetActive(false);
            titleScreen.SetActive(true);
            gameScreen = 0;
        }
    }   

    //A simple function which switches the toggle of boss mode and not boss mode (which should update through the Update() function).
    public void BossButtonControl() {
        isBoss = !isBoss;
    }





}
