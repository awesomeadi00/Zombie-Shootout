using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillRotate : MonoBehaviour
{
    public float rotationSpeed = 50f; 

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its Z axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
