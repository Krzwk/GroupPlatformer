using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public float speed;
    public bool invincible;
    public float bumpSpeed;
    Rigidbody enemyRigidbody;
    private void Awake(){
        enemyRigidbody = gameObject.GetComponent<Rigidbody>();
}
    private void FixedUpdate(){
        enemyRigidbody.velocity = new Vector3(speed, enemyRigidbody.velocity.y, 0);
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("End")){
            speed*= -1;
        }
    }

    public void OnDeath(){
        gameObject.GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 1f);
    }

}
