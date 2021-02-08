using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (Rigidbody))]

public class PlayerBehaviour : MonoBehaviour
{

    [System.Serializable]
    public class MoveSettings
    {
        public float runVelocity = 12;
        public float  rotateVelocity = 100;
        public float jumpVelocity = 8;
        public float  distanceToGround = 0.6f;
        public LayerMask ground;
    }
    [System.Serializable]

    public class InputSettings
    {
        public string FORWARD_AXIS = "Vertical";
        public string SIDEWAYS_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
        public string TURN_AXIS = "Mouse X";
        public string SHOOT_AXIS = "FIRE1";
    }
    
    [SerializeField]
    private GameObject dartPrefab;

    private enum WeaponStatus{
        Unarmed,
        Ready,
        Reloading
    }
    [SerializeField]
    private WeaponStatus weaponStatus;
    private float timeSinceLastShot;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private GameObject reloadBar;
    public MoveSettings moveSettings;
    public InputSettings inputSettings;

    private Rigidbody playerRigidbody;
    private Vector3 velocity;
    private Quaternion targetRotation;
    private float forwardInput, sidewaysInput, turnInput, jumpInput, shootInput;
    public Transform spawnpoint;


    void Start(){
        Spawn();
    }

    private void Spawn()
    {
        transform.position = spawnpoint.position;
    }

    void Awake()
    {
        weaponStatus = WeaponStatus.Unarmed;
        velocity = Vector3.zero;
        forwardInput = sidewaysInput = turnInput = jumpInput = shootInput = 0;
        targetRotation = transform.rotation;

        playerRigidbody = gameObject.GetComponent<Rigidbody>();

    }


    void Update()
    {
        GetInput();
        Turn();
    }

    void FixedUpdate()
    {
        Run();
        Jump();
        Shoot();
    }

    void GetInput()
    {
        if(inputSettings.FORWARD_AXIS.Length != 0)
            forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);
        if(inputSettings.SIDEWAYS_AXIS.Length != 0)
            sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);
        if(inputSettings.TURN_AXIS.Length != 0)
            turnInput = Input.GetAxis(inputSettings.TURN_AXIS);
        if(inputSettings.JUMP_AXIS.Length != 0)
            jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
        if(inputSettings.SHOOT_AXIS.Length != 0)
            shootInput = Input.GetAxisRaw(inputSettings.SHOOT_AXIS);
    }

    void Run()
    {
        velocity.x = sidewaysInput * moveSettings.runVelocity;
        velocity.y = playerRigidbody.velocity.y;
        velocity.z = forwardInput * moveSettings.runVelocity;

        playerRigidbody.velocity = transform.TransformDirection(velocity);

    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > 0)
        {
            targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVelocity * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }
    void Jump()
    {
        if ( jumpInput != 0 && Grounded())
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, moveSettings.jumpVelocity, playerRigidbody.velocity.z);
        }
    }
    bool Grounded()
    {
        return Physics.Raycast(gameObject.transform.position, Vector3.down, moveSettings.distanceToGround, moveSettings.ground);
    }

    void Shoot()
    {
        if (shootInput != 0 && weaponStatus.Equals(WeaponStatus.Ready))
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(dartPrefab, position, this.transform.rotation); 
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload(){
        weaponStatus= WeaponStatus.Reloading;
        while (timeSinceLastShot < reloadTime)
            {
                timeSinceLastShot += Time.deltaTime;
                reloadBar.transform.localScale = new Vector3 (timeSinceLastShot/reloadTime, reloadBar.transform.localScale.y, reloadBar.transform.localScale.z);
                yield return null;
            }
        weaponStatus = WeaponStatus.Ready;
        timeSinceLastShot = 0;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeathZone")
        {
            OnDeath();
        }
    }
    public void setCheckpoint(Checkpoints checkpoint){
        spawnpoint = checkpoint.transform;
        Destroy(checkpoint.GetComponent<BoxCollider>());
    }
    void OnDeath()
    {
        GameData.Instance.Lives -= 1;
        Spawn();
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Collider enemyCollider = collision.gameObject.GetComponent<Collider>();
            Collider playerCollider = gameObject.GetComponent<Collider>();

            if (enemy.invincible)
            {
                OnDeath();
            }
            else if (playerCollider.bounds.center.y - playerCollider.bounds.extents.y > enemyCollider.bounds.center.y + 0.5f* enemyCollider.bounds.extents.y)
            {
                GameData.Instance.Score += 10;
                JumpedOnEnemy(enemy.bumpSpeed);
                enemy.OnDeath();
            }
            else
            {
                OnDeath();
            }
        }
    }
    void JumpedOnEnemy(float bumpSpeed)
    {
        playerRigidbody.velocity  = new Vector3(playerRigidbody.velocity.x, bumpSpeed, playerRigidbody.velocity.z);
    }
}


