using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class KeyController : MonoBehaviour
{
    public float rotationSpeed = 50f;
    [SerializeField] TextMeshProUGUI pickupText;
    private PlayerStats player;
    [SerializeField] private AudioClip collectSound;
    private AudioSource keyAudio;
    private bool collected;

    // Start is called before the first frame update
    void Start()
    {
        collected = false;
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        keyAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its Y axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject.tag == "House 1 Key" && !collected)
        {
            pickupText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.C))
            {
                collected = true;
                player.house1Key = true;
                keyAudio.PlayOneShot(collectSound, 1.0f);
                pickupText.gameObject.SetActive(false);
                StartCoroutine(DestroyCountDown());
            }
        }

        if (gameObject.tag == "House 2 Key" && !collected)
        {
            pickupText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.C))
            {
                collected = true;
                player.house2Key = true;
                keyAudio.PlayOneShot(collectSound, 1.0f);
                pickupText.gameObject.SetActive(false);
                StartCoroutine(DestroyCountDown());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.tag == "House 1 Key")
        {
            pickupText.gameObject.SetActive(false);
        }

        if (gameObject.tag == "House 2 Key")
        {
            pickupText.gameObject.SetActive(false);
        }
    }

    IEnumerator DestroyCountDown() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}


