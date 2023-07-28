using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    //These are variables for the buttons: 
    [SerializeField] private Button startButton;
    [SerializeField] private Button instructionsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    //Boss variables: 
    [SerializeField] private Button bossButton;
    [SerializeField] private TextMeshProUGUI bossButtonText;

    //These are variables for the screens on the canvas
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject instructionsScreen;
    [SerializeField] private GameObject settingsScreen;

    private AudioSource menuAudio;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private Slider volSlider;
    [SerializeField] private Slider roundSlider;
    [SerializeField] private TextMeshProUGUI roundText;

    //Variable to check which screen the user is on, 0 = title screen, 1 = instruction screen, 2 = settings screen
    private int gameScreen;

    void Start() {
        gameScreen = 0;
        MainManager.Instance.bossActive = true;
        MainManager.Instance.roundNumber = 1;
        menuAudio = GetComponent<AudioSource>();
    }

    void Update() {
        //If it's not the boss mode: 
        if(!MainManager.Instance.bossActive) {
            bossButton.GetComponent<Image>().color = Color.red;
            bossButtonText.text = "De-activated";
        }

        //Else if it is the boss mode: 
        else if(MainManager.Instance.bossActive) {
            bossButton.GetComponent<Image>().color = Color.green;
            bossButtonText.text = "Activated";
        }

        MainManager.Instance.volumeValue = volSlider.value;
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
        MainManager.Instance.bossActive = !MainManager.Instance.bossActive;
    }

    //This function takes in the round number as an input and parses it to an integer (removes any unicode characters)
    public void SetRoundNumber() {
        MainManager.Instance.roundNumber = roundSlider.value;
        roundText.text = roundSlider.value.ToString();
    }



}
