using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{  

    private Camera mainCam;
    [SerializeField] private GameObject Zombie;
    private CharacterStats zombieStats;
    private ZombieStats blood;

    private bool reloadingCompleted = true;
    private PlayerStats playerStats;
    private float weaponRange = 20;
    private float lastShootTime = 0;

    public GameObject rifle;
    public GameObject pistol;
    public GameObject currentWeapon;
    private WeaponController currentWeaponController;
    // public Animator weaponAnimatorController;



    void Start() {
        playerStats = GetComponent<PlayerStats>();
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        EquipRifle();
        currentWeapon = rifle;
    }

    void Update()
    {   
        if(playerStats.DeathStatus() == false) {
            currentWeaponController = currentWeapon.GetComponent<WeaponController>();

            if (Input.GetKey(KeyCode.Mouse0)) {
                Shoot();
            }

            if(Input.GetKeyDown(KeyCode.R) && reloadingCompleted) {
                // weaponAnimatorController.SetTrigger("reload");
                currentWeaponController.ReloadSound();
                StartCoroutine(Reloading());
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SwapWeapon();
            }
        }   
    }

    //This will cast a ray and shoot it towards the center of the screen depending on the weapon's range which is 20m
    private void RaycastShoot() {
        Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
        RaycastHit hit;

        //This checks if the particle system is not playing then play it, else stop it. 
        if(!currentWeaponController.GunParticlePlayingBool() && !currentWeaponController.GunParticleExists()) {currentWeaponController.GunParticlePlay(); }
        else if(currentWeaponController.GunParticlePlayingBool() && !currentWeaponController.GunParticleExists()) {currentWeaponController.GunParticleStop(); }
        currentWeaponController.GunShotSound();

        //If there is no ammo in the magazine currently, then make the bool false. 
        if(playerStats.currentAmmoinMagazine == 0) {
            playerStats.hasAmmoinMagazine = false;
        }

        //If there is ammo in the magazine, then remove a bullet:
        if(playerStats.hasAmmoinMagazine) {
            playerStats.currentAmmoinMagazine -= 1;
        }

        //If it hits a zombie, call the damage zombie function in Player Stats and pass that specific zombie's characterstats
        if(Physics.Raycast(ray, out hit, weaponRange)) {
            if(hit.transform.CompareTag("Zombie")) {
                zombieStats = hit.transform.GetComponent<CharacterStats>();
                blood = hit.transform.GetComponent<ZombieStats>();
                blood.SpillBlood();
                playerStats.DamageZombie(zombieStats);
            }
        }
    }   

    //This function will call the RaycastShoot() every moment depending on the fireRate. 
    private void Shoot() {
        if(Time.time > lastShootTime + currentWeaponController.fireRate) {
            lastShootTime = Time.time;
            if(playerStats.hasAmmoinMagazine) {
                RaycastShoot();
            }
        }
    }

    void SwapWeapon()
    {
        if (currentWeapon == rifle)
        {
            EquipPistol();
        }
        else if (currentWeapon == pistol)
        {
            EquipRifle();
        }
    }

    void EquipRifle()
    {
        rifle.SetActive(true);
        pistol.SetActive(false);
        currentWeapon = rifle;
    }

    void EquipPistol()
    {
        rifle.SetActive(false);
        pistol.SetActive(true);
        currentWeapon = pistol;
    }

    public bool RifleActive() {
        return currentWeapon == rifle;
    }

    //Just a delay for the reloading sound effect so that you cannot execute it multiple times. 
    IEnumerator Reloading() {
        reloadingCompleted = false;
        yield return new WaitForSeconds(1.6f);

        //This calculates how much ammo to add from the stored ammo to the magazine and how much to remove from the stored ammo. 
        float ammoToAdd = playerStats.ammoMagazineSize - playerStats.currentAmmoinMagazine;
        playerStats.currentAmmoinMagazine = playerStats.ammoMagazineSize;
        playerStats.storedAmmo -= ammoToAdd;

        //If the stored ammo goes less than 0, make it 0. 
        if(playerStats.storedAmmo <= 0) {
            playerStats.storedAmmo = 0;
        }

        reloadingCompleted = true;
        playerStats.hasAmmoinMagazine = true;
    }
}
