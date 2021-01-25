using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDrop : Platform
{
    private enum State{
        Normal,
        Shaking,
        Dropping
    }
    private State state;
    private Vector3 PointA;
    private Vector3 PointB;
    private bool up;
    [SerializeField]
    private int numberOfShakes;
    private int numberOfShakesLeft;
    [SerializeField]
    private float shakeHeight;
    [SerializeField]
    private float shakeSpeed;
    [SerializeField]
    private float dropSpeed;
    private float distance = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        PointA = this.transform.position;
        PointB = new Vector3 (transform.position.x, transform.position.y - shakeHeight, transform.position.z);
        up = false;
        numberOfShakesLeft = numberOfShakes;
        state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Shaking){
            if (numberOfShakesLeft > 0)
            {
                distance += Time.deltaTime * shakeSpeed;
                if (!up)
                {
                    transform.position = Vector3.Lerp(PointA, PointB, distance);
                }
                else
                {
                    transform.position = Vector3.Lerp(PointB, PointA, distance);
                }
                if (distance > 1)
                {
                    up = !up;
                    if (!up)
                        {
                            numberOfShakesLeft--;
                        }
                    distance = 0;
                }
            }
            else
            {
                state = State.Dropping;
            }

        }
        else if (state == State.Dropping)
        {
            transform.position += dropSpeed * Vector3.down * Time.deltaTime;
        }
    }

    private void  OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            empty = new GameObject();
            empty.transform.parent = this.transform;
            other.transform.parent = empty.transform;
            state = State.Shaking;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(null);
            Destroy(empty);
        }
    }


    
}
