using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data.Common;
using System.Linq;

public class PlayerStats : CharacterStats
{
    //Zombie gameobjects: 
    private ZombieStats zombieStats;
    [SerializeField] GameObject zombiePrefab;

    //Player UI Variables: 
    [Header("Player UI")]
    private float maxBarValue = 100; 
    private float barMargin = 0.1f;
    public Slider healthBar;
    public Slider staminaBar;
    public Slider shieldBar;
    public Image staminaBarFill;  
    public TextMeshProUGUI playerPointsText;
    public TextMeshProUGUI ammoText;
    private WeaponShooting weaponSelected;

    //Player Stats Values: 
    [Header("Player Perk Values")]
    public float playerPoints;
    public float staminaEndurance;
    public float armourEndurance;
    public float damageBoost;
    public float pointMultiplier;

    [Header("Player Settings")]
    private float currentStamina;
    public float currentShield;
    public float damageTakenMultipler;
    private float inflictingDamage;
    public float ammoMagazineSize;
    public float storedAmmo;
    public float currentAmmoinMagazine;
    public float shieldsreplenishTimer = 3f;
    public float rifleDamage = 3f;
    public float pistolDamage = 7;
    public AudioClip[] hurtSounds;
    private AudioSource playerAudio;

    [Header("Player Booleans")]
    public bool beingAttacked = false;
    public bool outOfStamina = false;
    public bool outOfShields = false;
    public bool hasAmmoinMagazine;
    public bool house1Key = false;
    public bool house2Key = false;

    void Start() {
        zombieStats = zombiePrefab.gameObject.GetComponent<ZombieStats>();
        weaponSelected = GetComponent<WeaponShooting>();
        playerAudio = GetComponent<AudioSource>();
        InitializeVariables();
    }  

    //This simply updates the player's points and ammo on the screen at all times. 
    void Update() {
        playerPointsText.text = playerPoints.ToString();
        ammoText.text = currentAmmoinMagazine.ToString() + " | " + storedAmmo.ToString();
        UpdateInflictingDamage();

        // Check if being attacked by any zombie
        beingAttacked = CheckIfBeingAttacked();

        // Shield replenishment logic
        if (!beingAttacked)
        {
            StartCoroutine(ReplenishShieldsAfterDelay());
        }

        else {
            StopAllCoroutines();
        }
    }

    public override void InitializeVariables()
    {
        base.InitializeVariables();     //1) Player Health set to 100
        playerPoints = 0;               //2) Player Points set to 0
        inflictingDamage = 2;           
        damageTakenMultipler = 3f;

        //All perk values set to default.
        staminaEndurance = 0;
        armourEndurance = 0;
        damageBoost = 0;
        pointMultiplier = 1;

        //Stamina, Shields, Health UI Values set. 
        currentStamina = maxBarValue;
        currentShield = maxBarValue;
        staminaBar.value = maxBarValue;
        healthBar.value = maxBarValue;
        shieldBar.value = maxBarValue;

        //Ammo values set: 
        ammoMagazineSize = 120; 
        currentAmmoinMagazine = ammoMagazineSize;
        storedAmmo = 500;
        hasAmmoinMagazine = true;
    }

    //When the player runs, then the stamina bar will deplete faster. If it reaches close to 0, then they cannot run: out of stamina. 
    public void RunningStaminaDrain() {
        currentStamina -= (20 - CalculateStaminaEndurance()) * Time.deltaTime; 
        if(currentStamina < barMargin) {
            outOfStamina = true;
            StartCoroutine(FlashStaminaBar());
        }
    }

    //When the player does not run, their stamina will recharge. If their current stamina has recharged close to the full bar, then they aren't of out stamina. 
    public void NotRunningStaminaGain() {
        currentStamina += 10 * Time.deltaTime;          
        if(currentStamina > maxBarValue - barMargin) {
            currentStamina = maxBarValue;
            outOfStamina = false;
            StopCoroutine(FlashStaminaBar());
            staminaBarFill.color = new Color(0.01f, 0.78f, 0.11f);
        }
    }

    //Updates the stamina bar on the UI Canvas. 
    public void UpdateStaminaBar() {
        staminaBar.value = currentStamina;
    }

    //Updates the shield bar on the UI Canvas. 
    public void UpdateShieldsBar()
    {
        shieldBar.value = currentShield;
    }


    //This function has a formula which calculates how much should be added to staminaEndurance playerStats. 
    //This is because decay rate of player's stamina is 20points, hence the max cutoff should be at 20.
    //This funciton has a smooth exponential curve with an asymptote at 20, meaning the player will never actually counter its stamina's decay rate. 
    public float CalculateStaminaEndurance() {
        float staminaEnduranceToBeAdded = (-1 * Mathf.Pow(0.97f, staminaEndurance + (Mathf.Log(20)/Mathf.Log(0.97f)))) + 20;
        staminaEnduranceToBeAdded = Mathf.Round(staminaEnduranceToBeAdded * 100.0f) * 0.01f;    //Rounds to 2dp
        return staminaEnduranceToBeAdded;
    }


    //This function calculates the armour reduction value based on the damage inflicted from a zombie.  
    //The important thing to note is that the player armour reduction can never equate to a zombie's damage as then the player would not get hurt = game is too easy.
    //As the zombie's damage changes in the later rounds, so does the armour endurance function as well. 
    public float CalculateArmourEndurance() {
        float armourEnduranceFunction = (-1 * Mathf.Pow(0.9f, armourEndurance)) + 1;
        return (armourEnduranceFunction);
    }

    //This function will damage the zombie depending on the gun damage.  
    public void DamageZombie(ZombieStats statsToDamage) {
        float overallDamage = inflictingDamage + damageBoost;
        statsToDamage.TakeDamage(overallDamage);
    }

    public void UpdateInflictingDamage() {
        if(weaponSelected.RifleActive()) {
            inflictingDamage = rifleDamage;
        }

        else {
            inflictingDamage = pistolDamage;
        }
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

    public override void TakeDamage(float damage) {
        beingAttacked = true;

        if (hurtSounds.Length > 0)
        {
            int index = Random.Range(0, hurtSounds.Length);
            playerAudio.PlayOneShot(hurtSounds[index], 5f);
        }

        if (currentShield > barMargin) {
            currentShield -= (damage * damageTakenMultipler);
            UpdateShieldsBar();
            base.TakeDamage(damage/damageTakenMultipler);
        }
        
        if(currentShield < barMargin) {
            outOfShields = true;
        }

        if(outOfShields) {
            base.TakeDamage(damage);
        }
    }

    IEnumerator FlashStaminaBar()
    {
        bool toggle = false;
        Color darkForestGreen = new Color(0.01f, 0.78f, 0.11f);  
        Color lightGreen = new Color(0.65f, 0.95f, 0.2f);         
        while (outOfStamina)
        {
            staminaBarFill.color = toggle ? darkForestGreen : lightGreen; 
            toggle = !toggle;
            yield return new WaitForSeconds(0.5f); 
        }
        staminaBarFill.color = darkForestGreen; 
    }

    private bool CheckIfBeingAttacked()
    {
        return FindObjectsOfType<ZombieController>().Any(z => z.isAttacking);
    }

    IEnumerator ReplenishShieldsAfterDelay() 
    {
        yield return new WaitForSeconds(shieldsreplenishTimer);

        if (!beingAttacked)
        {  
            ReplenishShields();
        }
    }

    private void ReplenishShields()
    { 
        currentShield += 15 * Time.deltaTime;
        if (currentShield > maxBarValue - barMargin)
        {
            currentShield = maxBarValue;
            outOfShields = false;
        }
        UpdateShieldsBar();
    }
}
