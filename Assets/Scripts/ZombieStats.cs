using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : CharacterStats
{
    [SerializeField] private int damage;
    public float attackSpeed;

    private void Start() {
        InitializeVariables();
    }

    //This function will cause 5 damage to the player. 
    public void dealDamage(CharacterStats statsToDamage) {
        statsToDamage.TakeDamage(damage);
    }

    //Polyphormism with zombie, if they die, then it will destroy the game object. 
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

    //Overriden zombie initialized variables. 
    public override void InitializeVariables()
    {
        maxHealth = 50;
        SetHealth(maxHealth);
        isDead = false;
        damage = 5;
        attackSpeed = 2f;
    }
}
