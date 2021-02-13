using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform initialPlace;

    private float timer = 0.0f;
    private float waitingTime = 1.5f;
    
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > waitingTime)
        {
            transform.position = initialPlace.position;
            timer = 0;
        }
    }
}
