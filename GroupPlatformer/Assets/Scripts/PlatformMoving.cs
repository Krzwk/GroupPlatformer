using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoving : Platform
{

    [SerializeField]
    private Transform PointA;
    [SerializeField]
    private Transform PointB;
    [SerializeField]
    private float speed = 2;
    [SerializeField]
    private float waitingTime;
    private float waitingTimeLeft;
    private float distance = 0;
    private Transform currentTarget;
    private Transform currentStart;
    
    // Start is called before the first frame update
    void Start()
    {
        currentStart = PointA;
        currentTarget = PointB;
        waitingTimeLeft = waitingTime;

    }

    // Update is called once per frame
    void Update()
    {
        distance += speed*Time.deltaTime;
        transform.position = Vector3.Lerp(currentStart.position, currentTarget.position, distance);
        if (distance > 1)
        {
            waitingTimeLeft -= Time.deltaTime;
            if (waitingTimeLeft < 0)
            {
                distance = 0;
                Transform oldTarget = currentTarget;
                currentTarget = currentStart;
                currentStart = oldTarget;
                waitingTimeLeft = waitingTime;
             }
        }
    }



}