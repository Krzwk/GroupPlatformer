using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{

    [SerializeField]
    private Transform cube;
    [SerializeField]
    private Transform spawnPointCube;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody.CompareTag("Button"))
        {
            Debug.Log("Button hit the collider!");

            Transform t = Instantiate(cube);
            t.position = spawnPointCube.position;

        }
    }

}
