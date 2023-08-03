using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script will go on the Health Collider Objects
public class HealthStationCollider : StationCollider
{
    [SerializeField] private TextMeshProUGUI notAvailableText;
    [SerializeField] private TextMeshProUGUI healthFullText;
    private int healthStationPoints = 20;

    public override void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) {
            if(isAvailable) {   
                //If it's available, check if the player's health is full, if so, then print out the healthFullText
                if(playerStats.ReturnHealth() == 100) {
                    healthFullText.gameObject.SetActive(true);
                }

                //Else print out the get the health item: 
                else {
                    colliderTextPopUp.gameObject.SetActive(true);
                    if(Input.GetKeyDown(KeyCode.C)) {
                        colliderTextPopUp.gameObject.SetActive(false);
                        playerStats.Heal(healthStationPoints);
                        itemPerk.gameObject.SetActive(false);
                        isAvailable = false;
                        StartCoroutine(WaitTillSpawnsBack());
                    }
                }
            }

            //If the health item is not available, then just print out the notAvailableText
            else {
                notAvailableText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) {
            colliderTextPopUp.gameObject.SetActive(false);
            notAvailableText.gameObject.SetActive(false);
            healthFullText.gameObject.SetActive(false);
        }
    }
}
