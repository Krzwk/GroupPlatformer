using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{   
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            PlayerBehaviour player = other.gameObject.GetComponent<PlayerBehaviour>();
            player.setCheckpoint(this);
        }
    }    
}
