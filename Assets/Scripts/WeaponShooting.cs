using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{  

    private Camera mainCam;
    [SerializeField] private ParticleSystem gunShotEffect;
    [SerializeField] private AudioClip gunShotSound;
    [SerializeField] private AudioClip gunReloadSound;

    private PlayerStats playerStats;
    private AudioSource gunAudio;
    private float weaponRange = 20;
    private float lastShootTime = 0;
    private float fireRate = 0.07f;
    
    
    void Start() {
        playerStats = GetComponent<PlayerStats>();
        gunAudio = GetComponent<AudioSource>();
        mainCam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0)) {
            Shoot();
        }

        if(Input.GetKeyDown(KeyCode.R)) {
            gunAudio.PlayOneShot(gunReloadSound, 1.34f);
        }        
    }

    //This will cast a ray and shoot it towards the center of the screen depending on the weapon's range which is 20m
    private void RaycastShoot() {
        Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
        RaycastHit hit;

        //This checks if the particle system is not playing then play it, else stop it. 
        if(!gunShotEffect.isPlaying && gunShotEffect!= null) {gunShotEffect.Play(); }
        else if(gunShotEffect.isPlaying && gunShotEffect != null) {gunShotEffect.Stop(); }

        gunAudio.PlayOneShot(gunShotSound, 0.07f);

        if(Physics.Raycast(ray, out hit, weaponRange)) {
            if(hit.transform.CompareTag("Zombie")) {
                playerStats.DamageZombie();
            }
        }
    }   

    //This function will call the RaycastShoot() every moment depending on the fireRate. 
    private void Shoot() {
        if(Time.time > lastShootTime + fireRate) {
            lastShootTime = Time.time;
            RaycastShoot();
        }
    }
}
