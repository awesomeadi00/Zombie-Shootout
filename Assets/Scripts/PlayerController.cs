using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // This gets the Horizontal Axis Movement from the Input Manager into a horizontalInput variable (1, -1).
        horizontalInput = Input.GetAxis("Horizontal");

        // This gets the Vertical Axis Movement from the Input Manager into a verticalInput variable (1, -1).
        verticalInput = Input.GetAxis("Vertical");

        // If verticalInput is > 0, then it will move forward, else it will move backwards
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);

        // If horizontalInput is > 0, then it will move right, else it will move left
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
    }
}
