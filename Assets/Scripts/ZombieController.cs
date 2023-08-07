using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private float zombieSpeed;
    private float[] zombieWalkingSpeeds = {1, 2, 3, 4};
    private float zombieIdleSpeed = 0;
    [SerializeField] private float stoppingDistance = 2; 
    private float timeOfLastAttack = 0;
    public bool isAttacking = false;
    private bool isIdle = true;
    private bool canMove = true;

    private GameObject playerObject;
    private Rigidbody zombieRb;
    private Animator zombieAnim;
    private ZombieStats zombieStats;
    private AudioSource zombieAudio;
    [SerializeField] private AudioClip[] zombieGrowls;
    private bool hasGrowled = true;

    // Start is called before the first frame update
    void Start()
    {   
        zombieAudio = GetComponent<AudioSource>();
        zombieAnim = GetComponent<Animator>();
        zombieRb = GetComponent<Rigidbody>();
        zombieStats = GetComponent<ZombieStats>();
        playerObject = GameObject.Find("Player");
        StartCoroutine(IdleStart());    //Initially waits for the zombie to become active. 
    }

    void Update()
    {
        //If the zombie is not idle anymore, then he can start moving. 
        if(!isIdle) {
            MoveZombie();
        }

        if(gameObject.activeInHierarchy && hasGrowled) {
            StartCoroutine(ZombieGrowl());
        }
    }

    private void MoveZombie() {
        //If he can move and is not attacking, then he will move towards the player. 
        if(!isAttacking && canMove) {
            zombieSpeed = zombieWalkingSpeeds[Random.Range(0, zombieWalkingSpeeds.Length)];
            zombieAnim.SetFloat("Speed_f", 0.6f);
            Vector3 lookDirection = (playerObject.transform.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, playerObject.transform.position, zombieSpeed * Time.deltaTime);
            transform.LookAt(playerObject.transform);
        }

        //If the zombie is very close to the player, make him stop moving and attempt to attack the player. 
        if(CheckDistanceFromPlayer() <= stoppingDistance) {  
            canMove = false;  
            //If it has been 2 seconds once more, then the zombie will attack. 
            if(Time.time >= timeOfLastAttack + zombieStats.attackSpeed) {
                timeOfLastAttack = Time.time;
                CharacterStats playerStats = playerObject.GetComponent<CharacterStats>();
                AttackPlayer(playerStats);
            }

            //After the attack, he will stop attacking, and if he's still close, then he will remain idle before attacking again. 
            zombieAnim.SetBool("Attack_b", false);
            zombieAnim.SetBool("Attack2Walk", false);
            zombieAnim.SetBool("Attack2Idle", true);
        }

        //If the zombie is not close to the player, he can move once more and will not be attacking and will transition to walking again.
        else {
            canMove = true;
            isAttacking = false;
            zombieAnim.SetBool("Attack2Walk", true);
            zombieAnim.SetBool("Attack2Idle", false);
        } 
    }  
    
    //This function checks the distance of the zombie from the player. 
    private float CheckDistanceFromPlayer() {
        return (Vector3.Distance(playerObject.transform.position, transform.position));
    }   

    //This function will activate the attack trigger and also deal damage to the player's stats. 
    private void AttackPlayer(CharacterStats statsToDamage) {
        zombieSpeed = zombieIdleSpeed;
        zombieAnim.SetTrigger("Attack_t");
        isAttacking = true;
        zombieStats.dealDamage(statsToDamage);
    }

    //Makes the zombie stand still for 2.16 seconds when spawned as that is initial animation time length.
    IEnumerator IdleStart() {
        yield return new WaitForSeconds(2.16f);
        isIdle = false;
    }

    //This simply waits for some random time and then makes a zombie growl.
    private IEnumerator ZombieGrowl() {
        hasGrowled = false;
        float delayTime = Random.Range(5f, 10f);
        yield return new WaitForSeconds(delayTime);
        int zombieGrowlIndex = Random.Range(0, zombieGrowls.Length);
        zombieAudio.PlayOneShot(zombieGrowls[zombieGrowlIndex], 1.0f); 
        hasGrowled = true;
    }
}
