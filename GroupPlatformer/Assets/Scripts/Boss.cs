using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private int health;
    public bool invincible;
    public GameObject explosionPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit(int hpLoss){
        health -= hpLoss;
        if (health <= 0){
            OnDeath();
        }
        invincible = true;
        StartCoroutine(Hit());
    }

    private void OnDeath(){
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    IEnumerator Hit(){
        yield return new WaitForSeconds(3);
        invincible = false;
    }
}
