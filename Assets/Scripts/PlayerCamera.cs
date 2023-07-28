using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    private Vector3 offset = new Vector3(0, 2.5f, 0);

    [SerializeField] private float mouseSensX;
    [SerializeField] private float mouseSensY;
    private float xRotation;
    private float yRotation;
    private float mouseX;
    private float mouseY;

    private void Start() {
        //This locks the cursor at the middle and it'll be invisible so you won't be able to see it. 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        //We get the input of our Mouse X and Y values and increase the speed of the movement by its sens Axis.
        mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensX;
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensY;

        //Assigns the mouse values to the rotation variables. 
        yRotation += mouseX;    
        xRotation -= mouseY;

        //We clamp the range of the x-axis rotation from [-90, 90], so the player can look from up to down in this range.  
        xRotation = Mathf.Clamp(xRotation, -90f, 40f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);            //This rotates the cameras rotation based on the x and y rotations
        playerObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);       //This rotates the player's orientation based on the y rotation
    }

    //LateUpdate so that there are less bugs when the camera follows the player. 
    void LateUpdate()
    {   
        //Updates the camera position such that it's always at the player's eyes. 
        transform.position = playerObject.transform.position + offset;        
    }
}
