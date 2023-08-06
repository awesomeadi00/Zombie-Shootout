using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : CharacterStats
{
    //Zombie gameobjects: 
    private ZombieStats zombieStats;
    [SerializeField] GameObject zombiePrefab;

    //Player UI Variables: 
    private float maxBarValue = 100; 
    private float barMargin = 0.1f;
    public Slider healthBar;
    public Slider staminaBar;
    public TextMeshProUGUI playerPointsText;

    //Player Stats Values: 
    public float playerPoints;
    public float staminaEndurance;
    public float armourEndurance;
    public float damageBoost;
    public float pointMultiplier;
    private float currentStamina;
    private float inflictingDamage;
    public bool outOfStamina = false;

    void Start() {
        zombieStats = zombiePrefab.gameObject.GetComponent<ZombieStats>();
        InitializeVariables();
    }  

    //This simply updates the player's points on the screen at all times. 
    void Update() {
        playerPointsText.text = playerPoints.ToString();
    }

    public override void InitializeVariables()
    {
        base.InitializeVariables();     //1) Player Health set to 100
        playerPoints = 0;               //2) Player Points set to 0
        inflictingDamage = 1;           //3) Player can damage zombies 1hp per hit

        //All perk values set to default.
        staminaEndurance = 0;
        armourEndurance = 0;
        damageBoost = 0;
        pointMultiplier = 1;

        //Stamina and Health UI Values set. 
        currentStamina = maxBarValue;
        staminaBar.value = maxBarValue;
        healthBar.value = maxBarValue;
    }

    //When the player runs, then the stamina bar will deplete faster. If it reaches close to 0, then they cannot run: out of stamina. 
    public void RunningStaminaDrain() {
        // currentStamina -= (20 + CalculateStaminaEndurance()) * Time.deltaTime;       //This is not working so please fix. 
        currentStamina -= 20 * Time.deltaTime;
        if(currentStamina < barMargin) {
            outOfStamina = true; 
        }
    }

    //When the player does not run, their stamina will recharge. If their current stamina has recharged close to the full bar, then they aren't of out stamina. 
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

    //This function has a formula which calculates how much should be added to staminaEndurance playerStats. 
    //This is because decay rate of player's stamina is 20points, hence the max cutoff should be at 20.
    //This funciton has a smooth exponential curve with an asymptote at 20, meaning the player will never actually counter its stamina's decay rate. 
    public float CalculateStaminaEndurance() {
        float staminaEnduranceToBeAdded = Mathf.Pow(-0.99f, (staminaEndurance + (Mathf.Log(20)/Mathf.Log(0.99f)))) + 20;
        return staminaEnduranceToBeAdded;
    }


    //This function calculates the armour reduction value based on the damage inflicted from a zombie.  
    //The important thing to note is that the player armour reduction can never equate to a zombie's damage as then the player would not get hurt = game is too easy.
    //As the zombie's damage changes in the later rounds, so does the armour endurance function as well. 
    public float CalculateArmourEndurance() {
        float armourEnduranceFunction = Mathf.Pow(-0.9f, armourEndurance) + 1;
        return (armourEnduranceFunction * zombieStats.damage);
    }

    //This function will damage the zombie depending on the gun damage.  
    public void DamageZombie() {
        float overallDamage = inflictingDamage + damageBoost;
        zombieStats.TakeDamage(overallDamage);
    }


    //This is an overriden function for the player which does the same thing, but updates the health on their health bar UI on the canvas. 
    public override void CheckHealth()
    {
        base.CheckHealth();
        UpdateHealthBar();
    }

    //Updates the stamina bar on the UI Canvas. 
    public void UpdateHealthBar() {
        healthBar.value = health;
    }

    //A simple getter for the isDead bool to declare whether the game has ended or not. 
    public bool DeathStatus() {
        return isDead;
    }

    //This is a getter for the health value of the player to check if the player health is full or not. 
    public float ReturnHealth() {
        return health;
    }
 }
