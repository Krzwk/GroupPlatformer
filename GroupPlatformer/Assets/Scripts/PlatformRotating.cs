using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotating : Platform
{
   
    [SerializeField]
    private float rotationSpeed;
    private float rotationAngle;

    void Start()
    {
    }

    void Update()
    {
        rotationAngle += rotationSpeed * Time.deltaTime * 10;
        transform.rotation = Quaternion.Euler(Vector3.up * rotationAngle);
    }

}
