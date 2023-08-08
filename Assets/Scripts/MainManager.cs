using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    //Variables to pass through the menu scene: 
    public float volumeValue;
    public float mouseSensValue;

    void Awake(){
        if(Instance != null) {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
