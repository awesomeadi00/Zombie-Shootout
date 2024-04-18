using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI doorUnlockText;
    private PlayerStats player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    void Update() {
        if(player.house1Key && gameObject.tag == "House 1 Door") {
            doorUnlockText.gameObject.SetActive(false);
            // Destroy(gameObject);
            transform.parent.gameObject.SetActive(false);
        }

        if (player.house2Key && gameObject.tag == "House 2 Door")
        {
            doorUnlockText.gameObject.SetActive(false);
            // Destroy(gameObject);
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(gameObject.tag == "House 1 Door") {
            if (!player.house1Key)
            {
                doorUnlockText.gameObject.SetActive(true);
            }
        }

        if (gameObject.tag == "House 2 Door")
        {
            if (!player.house2Key)
            {
                doorUnlockText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.tag == "House 1 Door")
        {
            if (!player.house1Key)
            {
                doorUnlockText.gameObject.SetActive(false);
            }
        }

        if (gameObject.tag == "House 2 Door")
        {
            if (!player.house2Key)
            {
                doorUnlockText.gameObject.SetActive(false);
            }
        }
    }
}