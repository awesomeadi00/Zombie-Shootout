using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    [SerializeField] private float zombieSpeed = 2;
    private GameObject playerObject;
    private Rigidbody zombieRb;
    private Animator zombieAnim;

    // Start is called before the first frame update
    void Start()
    {   
        zombieAnim = GetComponent<Animator>();
        zombieRb = GetComponent<Rigidbody>();
        playerObject = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        MoveZombie();
    }

    private void MoveZombie() {
        //Normalized doesn't allow the speed to become extremely large when the player is far away and therefore keeps it at a standard of 1 magnitude. 
        Vector3 lookDirection = (playerObject.transform.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, playerObject.transform.position, zombieSpeed * Time.deltaTime);
        transform.LookAt(playerObject.transform);
        zombieAnim.SetFloat("Speed_f", 0.6f);
    }


    //Yeah idk how to do this...
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            Debug.Log("HERE");
            zombieAnim.SetTrigger("Attack_t");
        }

        else {
            zombieAnim.SetBool("Attack_b", false);
        }
    }
}
