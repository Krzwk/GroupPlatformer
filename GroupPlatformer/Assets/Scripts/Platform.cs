using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected GameObject empty;
    private void OnTriggerEnter(Collider other){
        if (other.tag == "Player")
            {
                empty = new GameObject();
                empty.transform.parent = this.transform;
                other.transform.SetParent(empty.transform);
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
