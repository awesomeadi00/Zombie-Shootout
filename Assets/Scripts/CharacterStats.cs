using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected bool isDead;

    // Start is called before the first frame update
    void Start() {InitializeVariables(); }

    public virtual void CheckHealth() {
        if(health <= 0) {
            health = 0;
            Die();
        }

        if(health >= maxHealth) {
            health = maxHealth;
        }
    }

    //Zombies and Player have different die functions
    public virtual void Die() {
        isDead = true;
    }

    public void SetHealth(int healthValueToSet) {
        health = healthValueToSet;
        CheckHealth();
    }

    public void TakeDamage(int damage) {
        int healthAfterDamage = health - damage;
        SetHealth(healthAfterDamage);
    }

    public void Heal(int heal) {
        int healthAfterHeal = health + heal;
        SetHealth(healthAfterHeal);
    }   

    //All entities will have different initialization values. 
    public virtual void InitializeVariables() {
        maxHealth = 100;
        SetHealth(maxHealth);
        isDead = false;
    }
}
