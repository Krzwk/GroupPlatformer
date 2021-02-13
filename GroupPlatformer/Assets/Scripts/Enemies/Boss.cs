using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : AdvancedEnemy
{


    private float baseNormalSpeed;
    private float timeSincePlayerSeen;
    private int maxHealth;
    [SerializeField]
    private int health;
    public bool invincible;
    [SerializeField]
    private float stunTime;
    public GameObject explosionPrefab;
    [SerializeField]
    private GameObject healthBar;
    new public enum Behaviour{
        
        ChargeAndDestroy,
        SearchAndSpawn,
        PatrolAndShoot, 
        Stunned


    }
    private Behaviour bossBehaviour;
    private bool spawning;
    private bool charging;
    [SerializeField]
    private float chargeSpeedUp;
    [SerializeField]
    private float liftoffSpeed;
    [SerializeField]
    private float liftoffHeight;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject dartPrefab;
    private float baseWeaponCooldown;
    [SerializeField]
    private float weaponCooldown;
    [SerializeField]
    private float rotationSpeed;

    public Behaviour Behaviour1 { get => bossBehaviour; set => bossBehaviour = value; }

    new void Awake(){
        base.Awake();
        liftoffHeight += this.transform.position.y;
        invincible = false;
        healthBar.transform.parent.GetComponent<Image>().enabled = true;
        healthBar.GetComponent<Image>().enabled = true;
        baseNormalSpeed = normalSpeed;
        baseWeaponCooldown = weaponCooldown;
        timeSincePlayerSeen = 0;
    }
    private void FixedUpdate(){
        switch(Behaviour1) {
            case Behaviour.ChargeAndDestroy : 
                if (normalSpeed == baseNormalSpeed){
                    PatternMovement();
                }
                else if (normalSpeed < baseNormalSpeed){
                    normalSpeed = baseNormalSpeed;
                }
                else {
                    Charge(prey.transform.position);
                }
                break;
            case Behaviour.PatrolAndShoot : 

                    if (PlayerVisible(prey.transform.position)){
                        
                        timeSincePlayerSeen = 0;
                        if (Vector3.Angle(prey.transform.position, transform.position - prey.transform.position) < 5){

                            if(weaponCooldown <= 0){
                                Instantiate(dartPrefab, new Vector3(transform.position.x, transform.position.y + this.GetComponent<Collider>().bounds.extents.y, transform.position.z), Quaternion.identity);
                                weaponCooldown = baseWeaponCooldown;
                            }
                            else{
                                weaponCooldown -= Time.deltaTime;
                                Vector3 target = prey.transform.position - transform.position;
                                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target, rotationSpeed*Time.deltaTime, 0.0f));

                            }

                        }
                    }
                    else{
                        PatternMovement();
                        if (timeSincePlayerSeen > 8){
                            ShootBlindly();
                            timeSincePlayerSeen = 0;
                        }
                    }
                    break;
            case Behaviour.SearchAndSpawn:
                if(!spawning){
                    if (timeSincePlayerSeen > 5){
                        timeSincePlayerSeen = 0;
                        StartCoroutine(Spawn());
                    }
                    else{
                        timeSincePlayerSeen += Time.deltaTime;
                        PatternMovement();
                    }
                }
                break;


                
        }
    }

    public void OnHit(int hpLoss){
        health -= hpLoss;
        if (health < 2*maxHealth / 3){
            StartCoroutine(SwitchPhase(Behaviour.SearchAndSpawn));
        }
        else if (health < maxHealth / 3){
            StartCoroutine(SwitchPhase(Behaviour.PatrolAndShoot));
        }
        if (health <= 0){
            OnDeath();
        }
        invincible = true;
        UpdateHealthbar();
        StartCoroutine(Hit());
    }

    private IEnumerator SwitchPhase(Behaviour phase){
        timeSincePlayerSeen = 0;
        Color newColor;
        float colorChange = 0;
        Behaviour1 = Behaviour.Stunned;
        while (colorChange < 1){
            colorChange += Time.deltaTime/2;
            if (phase == Behaviour.SearchAndSpawn)
                newColor = Color.Lerp(Color.blue, Color.red, colorChange);
            else 
                newColor = Color.Lerp(Color.red, Color.black, colorChange);
            this.GetComponent<Material>().color = newColor;
            yield return null;
        }
        Behaviour1 = phase;

    }

    private void OnDeath(){
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    IEnumerator Hit(){
        yield return new WaitForSeconds(3);
        invincible = false;
    }

    private void UpdateHealthbar(){
        healthBar.transform.localScale = new Vector3 (health/maxHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    private void Charge(Vector3 targetPosition){
        Vector3 enemyPosition = transform.position;
        Vector3 velocityRelative, distance, predictedInterceptPoint;
        float timeToClose;

        velocityRelative = prey.GetComponent<Rigidbody>().velocity - enemyRigidbody.velocity;
        distance = targetPosition - enemyPosition;

        timeToClose = distance.magnitude / velocityRelative.magnitude;

        predictedInterceptPoint = targetPosition + (timeToClose* prey.GetComponent<Rigidbody>().velocity);
        if (PlayerVisible(prey.transform.position)) 
            normalSpeed += chargeSpeedUp; 
        else 
            normalSpeed -= chargeSpeedUp;
        ChaseLineOfSight(predictedInterceptPoint, normalSpeed);
    }

    private void OnCollisionEnter(Collision collision){
        if (charging){
            if (collision.gameObject.CompareTag("Breakable Wall")){
                charging = false;
                OnHit(5);
                gameObject.GetComponent<Collider>().enabled = false;
                Destroy(gameObject, 1f);
                StartCoroutine(Concussion());
            }
            if (collision.gameObject.CompareTag("Wall")){
                charging = false;
                OnHit(5);
                StartCoroutine(Concussion());
            }

        }
    }

    private IEnumerator Spawn(){
        spawning = true;
        this.GetComponent<Rigidbody>().useGravity = false;
        while(this.transform.position.y < liftoffHeight){
                this.transform.position += Vector3.up*Time.deltaTime*liftoffSpeed;
                yield return null;
            }
        float spawned = 0;
        while(spawned < 3){
            Instantiate(enemyPrefab, this.transform.position - Vector3.down * 2, Quaternion.identity);
            health -= 1;
            UpdateHealthbar();
            spawned++;
            yield return new WaitForSeconds(2);
        }
        this.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(1);
        spawning = false;


    }



    private IEnumerator Concussion(){
        Behaviour previousBehaviour = Behaviour1;
        Behaviour1 = Behaviour.Stunned;
        yield return new WaitForSeconds(stunTime);
        Behaviour1 = previousBehaviour; 
    }

    private IEnumerator ShootBlindly(){
        for (int dartsFired = 0; dartsFired < 36; dartsFired++){
                Instantiate(dartPrefab, new Vector3(transform.position.x, transform.position.y + this.GetComponent<Collider>().bounds.extents.y, transform.position.z), Quaternion.identity);
                for (float rotationAroundSelf = 0; rotationAroundSelf < 10; rotationAroundSelf += Time.deltaTime*rotationSpeed){
                    this.transform.Rotate(0, Time.deltaTime*rotationSpeed, 0, Space.Self);
                    yield return null;
                }
                yield return null;
        }
    }
        
}


