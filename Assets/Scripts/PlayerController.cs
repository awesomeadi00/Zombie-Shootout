using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    [SerializeField] private float playerWalkSpeed = 5;
    [SerializeField] private float playerRunSpeed = 8;
    private bool isRunning = false;
    private bool isMoving = false;

    private bool isOnGround = true;
    private Rigidbody playerRb;
    [SerializeField] private float jumpForce = 300;
    private Animator playerAnim;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;
    private float maxBarValue = 100;
    private float currentStamina;
    private float staminaMargin = 0.1f;
    private bool outOfStamina = false;


    //Start is called before the first frame update
    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        
        currentStamina = maxBarValue;
        staminaBar.value = maxBarValue; 
        staminaBar.maxValue = maxBarValue;
        
        healthBar.maxValue = maxBarValue;
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

        //This will check if the player is moving at all, to activate the idle animation. 
        if(horizontalInput == 0 && verticalInput == 0) {isMoving = false; }
        else {isMoving = true; }

        //If the Left Shift Key Button is held down, then the player will be able to run. 
        if(Input.GetKey(KeyCode.LeftShift)) {isRunning = true; }
        else {isRunning = false; }
    }

    //Player moves forward/backwards and left/right depending on speed and input value. 
    private void MovePlayer() {
        playerAnim.SetBool("Static_b", true);

        //If the player is running, it will play the running animations and move at running speed. 
        if(isRunning && isMoving && !outOfStamina) {
            playerAnim.SetFloat("Speed_f", 0.6f);
            transform.Translate(Vector3.forward * Time.deltaTime * playerRunSpeed * verticalInput);
            transform.Translate(Vector3.right * Time.deltaTime * playerRunSpeed * horizontalInput);

            //If they run, then the stamina bar will deplete faster. If it reaches close to 0, then they cannot run: out of stamina. 
            currentStamina -= 20 * Time.deltaTime;
            if(currentStamina < staminaMargin) {outOfStamina = true; }
        }

        //Else if the player is walking, it will play the walking animations and move at walking speed. 
        else if(isMoving) {
            playerAnim.SetFloat("Speed_f", 0.3f);
            transform.Translate(Vector3.forward * Time.deltaTime * playerWalkSpeed * verticalInput);
            transform.Translate(Vector3.right * Time.deltaTime * playerWalkSpeed * horizontalInput); 
            currentStamina += 10 * Time.deltaTime;             //This will keep recharging their stamina.
            //If their current stamina has recharged close to the full bar, then they aren't of out stamina. 
            if(currentStamina > maxBarValue - staminaMargin) {
                outOfStamina = false;
            }
        }

        //Else the player is idle and will just play the idle animation. 
        else {
            playerAnim.SetFloat("Speed_f", 0f);
            currentStamina += 10 * Time.deltaTime;             //This will keep recharging their stamina.
            //If their current stamina has recharged close to the full bar, then they aren't of out stamina. 
            if(currentStamina > maxBarValue - staminaMargin) {
                outOfStamina = false;
            }
        }

        //Updates the player's stamina value on the slider UI. 
        staminaBar.value = currentStamina;
    }

    //This function resets the players y-velocity and adds an upwards force on them for a jump
    private void PlayerJump() {
        playerAnim.SetTrigger("Jump_trig");
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    //This simply checks all the collision objects that the player can collide with
    private void OnCollisionEnter(Collision other) {
        //If the other object is the ground, then it will check if the player is still on it. 
        if(other.gameObject.CompareTag("Ground")) {
            isOnGround = true;
        }

        if(other.gameObject.CompareTag("Zombie")) {
            //Remove health. 
        }
    }
}

