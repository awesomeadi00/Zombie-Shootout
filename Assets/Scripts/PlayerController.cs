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
    [SerializeField] private float jumpForce = 300;
    private Vector3 deathPosition;

    private bool isRunning = false;
    private bool isMoving = false;
    private bool isOnGround = true;
    public bool gameOver = false;

    private Rigidbody playerRb;
    private Animator playerAnim;
    private PlayerStats playerStats;
    private AudioSource playerAudio;
    [SerializeField] private AudioClip jumpSound;

    //Start is called before the first frame update
    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        playerAudio = GetComponent<AudioSource>();
    }

    //Update is called to get the Player's Input:
    private void Update()
    {
        if(playerStats.DeathStatus() == false) {
            GetPlayerInput();
        }
    }

    //This calls the movement of the player. It's a fixed update to ensure that the movement physics aren't jittery
    private void FixedUpdate() {
        if(playerStats.DeathStatus() == false) {
            MovePlayer();
        }

        else {
            deathPosition = transform.position;
            PlayerDeath();
        }
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

        //This checks if the player is moving at all and is on the ground, then play the walking audio track. 
        if((horizontalInput > 0 || verticalInput > 0) && isOnGround && playerAudio.isPlaying == false) {
            playerAudio.volume = Random.Range(0.8f, 1);
            playerAudio.pitch = Random.Range(0.8f, 1.1f);
            playerAudio.Play();
        }

        if(Input.GetMouseButton(0) && playerStats.hasAmmoinMagazine) {WeaponShootingAnimation(); }    //If the mouse is held down then play the weapon shooting animation. 
        if(Input.GetKeyDown(KeyCode.R)) {WeaponReloadAnimation(); }                                   //If the user presses 'R', then play the reload animation. 
    }

    //Player moves forward/backwards and left/right depending on speed and input value. 
    private void MovePlayer() {
        playerAnim.SetBool("Static_b", true); //Default animation bool set so that walking/running is static and not actual movement. 

        //Controls player movement
        if(isRunning && isMoving && !playerStats.outOfStamina) {PlayerRun(); }
        else if(isMoving) {PlayerWalk(); }
        else {PlayerIdle(); }

        //Updates the player's stamina value on the slider UI. 
        playerStats.UpdateStaminaBar();
    }


    //Helper functions: ========================================================================================================================
    //This function makes the player run foward with its animation: 
    private void PlayerRun() {
        transform.Translate(Vector3.forward * Time.deltaTime * playerRunSpeed * verticalInput);
        transform.Translate(Vector3.right * Time.deltaTime * playerRunSpeed * horizontalInput);
        playerAnim.SetFloat("Speed_f", 0.6f);
        playerStats.RunningStaminaDrain();
    }

    //This function makes the player walk forward with its animation: 
    private void PlayerWalk() {
        transform.Translate(Vector3.forward * Time.deltaTime * playerWalkSpeed * verticalInput);
        transform.Translate(Vector3.right * Time.deltaTime * playerWalkSpeed * horizontalInput); 
        playerAnim.SetFloat("Speed_f", 0.3f);
        playerStats.NotRunningStaminaGain();   
    }

    //This function executes when the player is Idle:
    private void PlayerIdle() {
        playerAnim.SetFloat("Speed_f", 0f);
        playerAnim.SetInteger("WeaponType_int", 3);
        playerAnim.SetBool("Shoot_b", false);
        playerAnim.SetBool("Reload_b", false);
        playerStats.NotRunningStaminaGain();
    }
    
    //This function starts the shooting animation. 
    private void WeaponShootingAnimation() {
        playerAnim.SetBool("Shoot_b", true);
        playerAnim.SetBool("FullAuto_b", true);
    }

    //This function starts the reloading animation. 
    private void WeaponReloadAnimation() {
        playerAnim.SetBool("Shoot_b", false);
        playerAnim.SetBool("Reload_b", true);
    }

    //This function resets the players y-velocity and adds an upwards force on them for a jump
    private void PlayerJump() {
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerAnim.SetTrigger("Jump_trig");
        playerAudio.PlayOneShot(jumpSound, 0.25f);
    }

    //If the player dies, then they won't be able to move and 
    private void PlayerDeath() {
        playerAnim.SetBool("Death_b", true);
        playerAnim.SetInteger("DeathType_int", 1);
        transform.position = deathPosition;
    }

    //This simply checks all the collision objects that the player can collide with
    private void OnCollisionEnter(Collision other) {
        //If the other object is the ground, then it will check if the player is still on it. 
        if(other.gameObject.CompareTag("Ground")) {
            isOnGround = true;
        }
    }
}

