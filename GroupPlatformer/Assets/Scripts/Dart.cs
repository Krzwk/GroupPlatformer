using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField]
    private float dartSpeed = 30;

    public GameObject explosionPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float amtToMove = dartSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * amtToMove);
        if(transform.position.y > 6.4f)
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
    }
}
