using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is a parent class for all types of characters in the scene: This includes: Player, Zombies, Boss
//This class mainly monitors the health of the character. 
public class CharacterStats : MonoBehaviour
{   
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected bool isDead;

    // Start is called before the first frame update
    void Start() {InitializeVariables(); }

    //Function to check the health to check if they should die or if their health goes beyond their maxHealth.
    public virtual void CheckHealth() {
        if(health <= 0) {
            health = 0;
            Die();
        }

        if(health >= maxHealth) {
            health = maxHealth;
        }
    }

    //This function declares the entity as dead. Zombies and Player have different die functions
    public virtual void Die() {
        isDead = true;
    }

    //This function declares the entity as alive. 
    public void ReInitialize() {
        health = maxHealth;
        isDead = false;
    }

    //This function takes a value and sets it as the entities health
    public void SetHealth(float healthValueToSet) {
        health = healthValueToSet;
        CheckHealth();
    }

    //This function takes a value and damages the entities health.
    public virtual void TakeDamage(float damage) {
        float healthAfterDamage = health - damage;
        SetHealth(healthAfterDamage);
    }

    //This function takes a value and heals the entities health. 
    public void Heal(float heal) {
        float healthAfterHeal = health + heal;
        SetHealth(healthAfterHeal);
    }   

    //Basic Initialization for health is 100hp and they are alive. All entities will have different initializations. 
    public virtual void InitializeVariables() {
        maxHealth = 100;
        SetHealth(maxHealth);
        isDead = false;
    }

    public float Test() {
        return health;
    }
}
