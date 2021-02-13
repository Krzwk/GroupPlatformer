using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    Transform positionDoor;

    bool move = false;

    void Update()
    {
        if (move)
        {
            if (positionDoor.position.y <= 9.5f)
                positionDoor.Translate(Vector3.up * Time.deltaTime, Space.World);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("ActivatorCube"))
        {
            move = true;
        }
    }

}
