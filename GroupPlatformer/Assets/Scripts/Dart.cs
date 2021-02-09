using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField]
    private float dartSpeed;

    public GameObject explosionPrefab;
    private float timeAlive;
    
    void Awake(){
        this.transform.Rotate(new Vector3(90, 0, 0), Space.Self);
        timeAlive = 0;
    }
    void Update()
    {
        float amtToMove = dartSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * amtToMove);
        timeAlive += Time.deltaTime;
        if(timeAlive > 5)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.OnDeath();
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            Boss boss = other.gameObject.GetComponent<Boss>();
            if(!boss.invincible){
                boss.OnHit(10);
                }
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player"))
        Destroy(this.gameObject);
    }
}
