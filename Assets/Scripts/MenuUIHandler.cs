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
    [SerializeField] private Button exitButton;

    //These are variables for the screens on the canvas
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject instructionsScreen;

    private AudioSource menuAudio;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private Slider volSlider;
    [SerializeField] private Slider pageSlider;
    [SerializeField] private GameObject[] instructionPages;

    //Variable to check which screen the user is on, 0 = title screen, 1 = instruction screen
    private int gameScreen;

    void Start() {
        gameScreen = 0;
        menuAudio = GetComponent<AudioSource>();
    }

    void Update() {
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
    }   

    public void InstructionPages() {
        if(pageSlider.value == 1) {
            instructionPages[0].SetActive(true);
            instructionPages[1].SetActive(false);
            instructionPages[2].SetActive(false);
            instructionPages[3].SetActive(false);
        }

        if(pageSlider.value == 2) {
            instructionPages[0].SetActive(false);
            instructionPages[1].SetActive(true);
            instructionPages[2].SetActive(false);
            instructionPages[3].SetActive(false);
        }

        if(pageSlider.value == 3) {
            instructionPages[0].SetActive(false);
            instructionPages[1].SetActive(false);
            instructionPages[2].SetActive(true);
            instructionPages[3].SetActive(false);
        }

        if(pageSlider.value == 4) {
            instructionPages[0].SetActive(false);
            instructionPages[1].SetActive(false);
            instructionPages[2].SetActive(false);
            instructionPages[3].SetActive(true);
        }
    }

}
