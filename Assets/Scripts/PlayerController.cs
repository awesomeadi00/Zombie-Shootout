using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;

    [SerializeField] private float playerWalkSpeed = 10;
    [SerializeField] private float playerRunSpeed = 20;
    private bool isRunning = false;

    private bool isOnGround = true;
    private Rigidbody playerRb;
    [SerializeField] private float jumpForce = 300;

    //Start is called before the first frame update
    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        GetPlayerInput();
    }

    //This is to ensure that the physics applied on the user aren't jittery
    private void FixedUpdate() {
        MovePlayer();
    }

    //Monitors the playey's input
    private void GetPlayerInput() {
        //Gets movement input from WASD
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Gets the jumping movement input from Spacebar
        if(Input.GetKeyDown(KeyCode.Space) && isOnGround) {
            PlayerJump();
            isOnGround = false;
        }

        //If the Left Shift Key Button is held down, then the player will be able to run. 
        if(Input.GetKey(KeyCode.LeftShift)) {isRunning = true; }
        else {isRunning = false; }
    }

    //Player moves forward/backwards and left/right depending on speed and input value. 
    private void MovePlayer() {
        //If Left shift isn't pressed, the player moves in walking speed
        if(isRunning == true) {
            transform.Translate(Vector3.forward * Time.deltaTime * playerRunSpeed * verticalInput);
            transform.Translate(Vector3.right * Time.deltaTime * playerRunSpeed * horizontalInput);
        }

        //Else the player will move at running speed
        else {
            transform.Translate(Vector3.forward * Time.deltaTime * playerWalkSpeed * verticalInput);
            transform.Translate(Vector3.right * Time.deltaTime * playerWalkSpeed * horizontalInput);
        }
    }

    //This function resets the players y-velocity and adds an upwards force on them for a jump
    private void PlayerJump() {
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    //This simply checks all the collision objects that the player can collide with
    private void OnCollisionEnter(Collision other) {
        //If the other object is the ground, then it will check if the player is still on it. 
        if(other.gameObject.CompareTag("Ground")) {
            isOnGround = true;
        }
    }
}
