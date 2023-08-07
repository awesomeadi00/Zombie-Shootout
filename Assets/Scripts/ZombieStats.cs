using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : CharacterStats
{
    private PlayerStats playerStats;
    [SerializeField] public float damage;         //1) Zombie has a damage value that they inflict on the player. 
    public float attackSpeed;                     //2) Zombie has a speed at which they attack the player. 
    public float zombiePointPerKill;              //3) Zombie has a point value that the player receives when they kill them.

    private void Start() {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        InitializeVariables();
    }

    //Zombies start off with 50hp, has 50points, and deal 2dmg every 2 seconds. 
    public override void InitializeVariables()
    {
        maxHealth = 50;
        SetHealth(maxHealth);
        isDead = false;
        damage = 2;
        attackSpeed = 2f;
        zombiePointPerKill = 100;
    }

    //This function is called in the Zombie controller, it takes the player's CharacterStats and deals damage to the player's health. 
    //This function will also take into the armour endurance on the player as well. 
    public void dealDamage(CharacterStats statsToDamage) {
        statsToDamage.TakeDamage(damage * (1 - playerStats.CalculateArmourEndurance()));         
    }

    //A getter function to check whether the zombie is dead or not. 
    public bool ZombieDeathStatus() {
        return isDead;
    }

    //For a zombie, when their health goes to 0, we set the object as false from the scene and add the points to the player's overall points. 
    public override void Die()
    {
        base.Die();
        Vector3 deathPosition = transform.position;
        //When the zombie dies, add the points to the player. 
        playerStats.playerPoints += zombiePointPerKill * playerStats.pointMultiplier; 
        gameObject.SetActive(false);
    }
}
