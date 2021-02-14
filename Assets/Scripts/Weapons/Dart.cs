using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField]
    private float dartSpeed;

    public GameObject explosionPrefab;
    
    void Awake(){
        this.transform.Rotate(new Vector3(90, 0, 0), Space.Self);
    }
    void Update()
    {
        float amtToMove = dartSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * amtToMove);
        Destroy(this.gameObject, 5f);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            AdvancedEnemy enemy = other.gameObject.GetComponent<AdvancedEnemy>();
            enemy.OnDeath();
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            Boss boss = other.gameObject.GetComponent<Boss>();
            if(!boss.invincible){
                boss.OnHit(10);
                }
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (!other.gameObject.CompareTag("Player"))
            Destroy(this.gameObject);
    }
}
