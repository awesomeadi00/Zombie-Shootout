using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerkStationCollider : StationCollider
{
    [SerializeField] private TextMeshProUGUI notEnoughPointsText;
    private int pointsNeeded;

    //Start is called before the first frame update
    void Start() {InitializeStationGameObjects(); }

    public override void InitializeStationGameObjects()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        //Example values for now: 
        if(gameObject.name == "Perk 1") {pointsNeeded = 1000;}
        else if(gameObject.name == "Perk 2") {pointsNeeded = 2000;}
        else if(gameObject.name == "Perk 3") {pointsNeeded = 3000;}
    }

    public override void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) { 
            colliderTextPopUp.gameObject.SetActive(true);
            if(Input.GetKeyDown(KeyCode.C)) {
                colliderTextPopUp.gameObject.SetActive(true);                
                //if(the player's stats points is >= points needed, then execute give perk function. 
                //else print out not enough points text.
                if(gameObject.name == "Perk 1") {
                        Debug.Log("YAY PERK 1");
                }

                else if(gameObject.name == "Perk 2") {
                        Debug.Log("YAY PERK 2");
                }

                else if(gameObject.name == "Perk 3") {
                    Debug.Log("YAY PERK 3");
                }
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
