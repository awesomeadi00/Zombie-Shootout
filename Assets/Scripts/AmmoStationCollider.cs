using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class AmmoStationCollider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI colliderTextPopUp;         
    private GameObject parentStation;
    private GameObject itemPerk;
    private PlayerStats playerStats;
    private bool isAvailable;

    [SerializeField] private TextMeshProUGUI notAvailableText;
    private int ammoOnLoad;
    private int ammoBackUp;
    [SerializeField] private AudioClip collectSound;
    private AudioSource stationAudio;
    private bool isCooldownActive = false;

    //Start is called before the first frame update
    void Start() {
        InitializeStationGameObjects();
        stationAudio = GetComponent<AudioSource>();   
    }

    public virtual void InitializeStationGameObjects() {
        parentStation = gameObject.transform.parent.gameObject;         //Get the parent object so you can refer to its children objects.
        itemPerk = parentStation.transform.GetChild(2).gameObject;      //Gets the actual item perk from the parent to deactivate/activate.
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        isAvailable = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) {
            //If the item is available print out "press 'c' to get perk" 
            if(isAvailable) {   
                colliderTextPopUp.gameObject.SetActive(true);
                //If pressed 'c', it will restock ammo into the player's stored ammo section. 
                if(Input.GetKeyDown(KeyCode.C)) {
                    stationAudio.PlayOneShot(collectSound, 1.0f);
                    colliderTextPopUp.gameObject.SetActive(false);
                    RestockAmmo();
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

    //This function is used when the player gets the item and then has to wait for a minute or two to receive it once more. 
    private IEnumerator WaitTillSpawnsBack()
    {
        isCooldownActive = true;
        float cooldownTime = 60;

        while (cooldownTime > 0)
        {
            notAvailableText.text = "Available in: " + Mathf.CeilToInt(cooldownTime) + "s";
            yield return new WaitForSeconds(1);
            cooldownTime--;
        }

        isAvailable = true;
        itemPerk.gameObject.SetActive(true);
        notAvailableText.gameObject.SetActive(false);
        isCooldownActive = false;
    }

    private void RestockAmmo() {
        playerStats.storedAmmo += 100;
    }
}
