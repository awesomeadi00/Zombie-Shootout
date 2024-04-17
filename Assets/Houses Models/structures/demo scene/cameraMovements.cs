using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovements : MonoBehaviour
{
    
    public float speedKey=50,speedMouse=50;
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical")*speedKey);
        transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * speedKey);
        if (Input.GetKey(KeyCode.Space))  transform.Translate(Vector3.up * Time.deltaTime*speedKey/5);
        if (Input.GetKey(KeyCode.LeftShift)) transform.Translate(Vector3.down * Time.deltaTime * speedKey/5);
        transform.eulerAngles+=Vector3.up* Time.deltaTime * Input.GetAxis("Mouse X") * speedMouse;
        transform.eulerAngles += Vector3.right * Time.deltaTime * -Input.GetAxis("Mouse Y") * speedMouse;
        GetComponent<Camera>().fieldOfView -=Input.GetAxis("Mouse ScrollWheel")*20;
    }
}
