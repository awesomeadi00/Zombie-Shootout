using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    private float maxBarValue = 100; 
    private float barMargin = 0.1f;
    public Slider healthBar;
    public Slider staminaBar;
    private float currentStamina;
    public bool outOfStamina = false;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxBarValue;
        staminaBar.value = maxBarValue;
        healthBar.value = maxBarValue;
        InitializeVariables();
    }

    //If they run, then the stamina bar will deplete faster. If it reaches close to 0, then they cannot run: out of stamina. 
    public void RunningStaminaDrain() {
        currentStamina -= 20 * Time.deltaTime;
        if(currentStamina < barMargin) {
            outOfStamina = true; 
        }
    }

    //If they do not run, their stamina will recharge by 10 points. If their current stamina has recharged close to the full bar, then they aren't of out stamina. 
    public void NotRunningStaminaGain() {
        currentStamina += 10 * Time.deltaTime;          
        if(currentStamina > maxBarValue - barMargin) {
            currentStamina = maxBarValue;
            outOfStamina = false;
        }
    }

    //Updates the stamina bar on the UI Canvas. 
    public void UpdateStaminaBar() {
        staminaBar.value = currentStamina;
    }

    //This is an overriden function for the player which does the same thing, but also updates the health on their health bar. 
    public override void CheckHealth()
    {
        base.CheckHealth();
        UpdateHealthBar();
    }

    //Updates the stamina bar on the UI Canvas. 
    public void UpdateHealthBar() {
        healthBar.value = health;
    }

    //A simple getter for the isDead bool. 
    public bool DeathStatus() {
        return isDead;
    }

        //Overriden zombie initialized variables. 
    public override void InitializeVariables()
    {
        maxHealth = 5;
        SetHealth(maxHealth);
        isDead = false;
    }
 }
