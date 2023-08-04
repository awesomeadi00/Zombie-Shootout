using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : CharacterStats
{
    private PlayerStats playerStats;
    [SerializeField] GameObject playerObject;
    [SerializeField] public float damage;         //1) Zombie has a damage value that they inflict on the player. 
    public float attackSpeed;                   //2) Zombie has a speed at which they attack the player. 
    public float pointValue;                      //3) Zombie has a point value that the player receives when they kill them.

    private void Start() {
        playerStats = playerObject.gameObject.GetComponent<PlayerStats>();
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
        pointValue = 50;
    }

    //This function is called in the Zombie controller, it takes the player's CharacterStats and deals damage to the player's health. 
    //This function will also take into the armour endurance on the player as well. 
    public void dealDamage(CharacterStats statsToDamage) {
        // statsToDamage.TakeDamage(damage - playerStats.CalculateArmourEndurance());           //Not working please fix this. 
        statsToDamage.TakeDamage(damage);
    }

    //For a zombie, when their health goes to 0, we Destroy them from the scene.
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
