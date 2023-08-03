using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoStationCollider : StationCollider
{
    [SerializeField] private TextMeshProUGUI notAvailableText;
    private int ammoOnLoad;
    private int ammoBackUp;

    public override void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) {
            //If the item is available print out "press 'c' to get perk" 
            if(isAvailable) {   
                colliderTextPopUp.gameObject.SetActive(true);
                if(Input.GetKeyDown(KeyCode.C)) {
                    colliderTextPopUp.gameObject.SetActive(false);
                    Debug.Log("Ammo Gained");       //Will add this function feature to the player once we work on the ammo values for the gun.  
                    itemPerk.gameObject.SetActive(false);
                    isAvailable = false;
                    StartCoroutine(WaitTillSpawnsBack());
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
        }
    }
}
