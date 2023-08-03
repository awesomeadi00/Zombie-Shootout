using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script will be applied to the collider of every station. 
public class StationCollider : MonoBehaviour
{   
    [SerializeField] protected TextMeshProUGUI colliderTextPopUp;         
    protected GameObject parentStation;
    protected GameObject itemPerk;
    protected PlayerStats playerStats;
    protected bool isAvailable;

    //Start is called before the first frame update
    void Start() {
        InitializeStationGameObjects(); 
        isAvailable = true;
    }

    public virtual void InitializeStationGameObjects() {
        parentStation = gameObject.transform.parent.gameObject;         //Get the parent object so you can refer to its children objects.
        itemPerk = parentStation.transform.GetChild(2).gameObject;      //Gets the actual item perk from the parent to deactivate/activate.
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    //If the player enters its collider border
    public virtual void OnTriggerStay(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            //If the item is available print out "press 'c' to get perk" 
            if(isAvailable) {   
                colliderTextPopUp.gameObject.SetActive(true);
                if(Input.GetKeyDown(KeyCode.C)) {
                    //Call the ActivatePerk function from within the individual scripts themselves. 
                    itemPerk.gameObject.SetActive(false);
                    isAvailable = false;
                    StartCoroutine(WaitTillSpawnsBack());
                }
            }
        }
    }

    //This function is used when the player gets the item and then has to wait for a minute or two to receive it once more. 
    public IEnumerator WaitTillSpawnsBack() {
        yield return new WaitForSeconds(120);
        isAvailable = true;
        itemPerk.gameObject.SetActive(true);
    }
}
