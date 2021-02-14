using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float startingY;
    private Vector3 hoverDirection;
    [SerializeField]
    private float hoverSpeed;
    [SerializeField]
    private float hoverHeight;

    void Awake(){
        startingY = transform.position.y;
        hoverDirection = Vector3.up * hoverSpeed;
    }
    void FixedUpdate(){
        if (transform.position.y > startingY + hoverHeight){
            hoverDirection = Vector3.down * hoverSpeed;
        }
        else if (transform.position.y < startingY - hoverHeight){
            hoverDirection = Vector3.up * hoverSpeed;
        }
        transform.Translate(hoverDirection);
    }
    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            player.Arm();
            Destroy(this.gameObject);
        }
    }

}
