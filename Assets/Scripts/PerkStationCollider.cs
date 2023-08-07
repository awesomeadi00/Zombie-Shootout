using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerkStationCollider :  MonoBehaviour
{
    //Perk Station UI Variables: 
    [SerializeField] private TextMeshProUGUI colliderTextPopUp; 
    [SerializeField] private TextMeshProUGUI notEnoughPointsText;
    [SerializeField] private TextMeshProUGUI armourValueText; 
    [SerializeField] private TextMeshProUGUI staminaValueText; 
    [SerializeField] private TextMeshProUGUI damageValueText; 
    [SerializeField] private TextMeshProUGUI pointValueText; 

    //Perk Station Values:
    private PlayerStats playerStats;
    private int pointsNeeded;
    private int armourPoints = 1500;
    private int staminaPoints = 2000;
    private int damagePoints = 4000;
    private int pointMultPoints = 3000;
    [SerializeField] private AudioClip collectSound;
    private AudioSource playerAudio;


    //Start is called before the first frame update
    void Start() {
        InitializeStationGameObjects(); 
        playerAudio = GameObject.Find("Player").GetComponent<AudioSource>();    
    }

    void Update() {
        armourValueText.text = "+" + playerStats.armourEndurance.ToString();
        staminaValueText.text = "+" + playerStats.staminaEndurance.ToString();
        damageValueText.text = "+" + playerStats.damageBoost.ToString();

        playerStats.pointMultiplier = Mathf.Round(playerStats.pointMultiplier * 100.0f) * 0.01f;    //Simply keeps it strictly to 2dp. 
        pointValueText.text = "x" + playerStats.pointMultiplier.ToString();
    }

    private void InitializeStationGameObjects()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        //Sets the points needed depending on the Perk Station it is. 
        if(gameObject.CompareTag("Armour Collider")) {pointsNeeded = armourPoints;}
        else if(gameObject.CompareTag("Stamina Collider")) {pointsNeeded = staminaPoints;}
        else if(gameObject.CompareTag("Damage Collider")) {pointsNeeded = damagePoints;}
        else if(gameObject.CompareTag("Point Collider")) {pointsNeeded = pointMultPoints;}
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) {
            //If the player has enough points to purchase the perk then print it out
            if(playerStats.playerPoints >= pointsNeeded) {
                colliderTextPopUp.gameObject.SetActive(true);
                //If they press 'c', then depending on the perk, that will boost that player stats value.
                if(Input.GetKeyDown(KeyCode.C)) {     
                    playerAudio.PlayOneShot(collectSound, 1.0f);               
                    if(gameObject.tag == "Armour Collider") {
                        playerStats.armourEndurance += 1;
                        playerStats.playerPoints -= armourPoints;
                    }

                    else if(gameObject.tag == "Stamina Collider") {
                        playerStats.staminaEndurance += 1;
                        playerStats.playerPoints -= staminaPoints;
                    }

                    else if(gameObject.tag == "Damage Collider") {
                        playerStats.damageBoost += 1;
                        playerStats.playerPoints -= damagePoints;
                    }

                    else if(gameObject.tag == "Point Collider") {
                        playerStats.pointMultiplier += 0.2f;
                        playerStats.playerPoints -= pointMultPoints;
                    }
                }
            }

            //If the player does not have enough points, then it will print out not enough points. 
            else {
                colliderTextPopUp.gameObject.SetActive(false);
                notEnoughPointsText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) {
            colliderTextPopUp.gameObject.SetActive(false);
            notEnoughPointsText.gameObject.SetActive(false);
        }
    }
}
