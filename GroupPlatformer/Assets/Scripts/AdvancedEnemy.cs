using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedEnemy : MonoBehaviour
{
    [SerializeField]
    private float chaseSpeed;
    [SerializeField]
    private float normalSpeed;
    [SerializeField]
    private GameObject prey;
    private Rigidbody enemyRigidbody; 

    public enum Behaviour
        {
        LineOfSight,
        Intercept,
        PatternMovement, 
        ChasePatternMovement, 
        Hide 
        }
    public Behaviour behaviour;

    private List<Waypoint> waypoints;
    private int currentWaypoint = 0;
    [SerializeField]
    private float distanceThreshold;


    void Awake(){
            enemyRigidbody = GetComponent<Rigidbody>();
        }

    void FixedUpdate(){
        switch (behaviour){
            case Behaviour.LineOfSight:
            ChaseLineOfSight(prey.transform.position, chaseSpeed);
            break;
            case Behaviour.Intercept:
            Intercept(prey.transform.position);
            break;
            case Behaviour.PatternMovement:
            PatternMovement();
            break;
            case Behaviour.ChasePatternMovement:
            if (Vector3.Distance(gameObject.transform.position, prey.transform.position) < distanceThreshold){
                ChaseLineOfSight(prey.transform.position, chaseSpeed);
            } 
            else {
                PatternMovement();
            }
            break;
            case Behaviour.Hide:
            if (PlayerVisible(prey.transform.position)){
                ChaseLineOfSight(prey.transform.position, chaseSpeed);
            }
            else{
                PatternMovement();
            }
            break;
            default:
            break;
        }
    }

    private void ChaseLineOfSight(Vector3 targetPosition, float speed){
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();
        enemyRigidbody.velocity = new Vector3(direction.x * speed, enemyRigidbody.velocity.y, direction.z * speed);
    }

    private void Intercept(Vector3 targetPosition)
    {
        Vector3 enemyPosition = transform.position;
        Vector3 velocityRelative, distance, predictedInterceptPoint;
        float timeToClose;

        velocityRelative = prey.GetComponent<Rigidbody>().velocity - enemyRigidbody.velocity;
        distance = targetPosition - enemyPosition;

        timeToClose = distance.magnitude / velocityRelative.magnitude;

        predictedInterceptPoint = targetPosition + (timeToClose* prey.GetComponent<Rigidbody>().velocity);
        ChaseLineOfSight(predictedInterceptPoint, chaseSpeed);


    }

    private void PatternMovement(){
        ChaseLineOfSight(waypoints[currentWaypoint].transform.position, normalSpeed);
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < distanceThreshold){
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
        }

    }

    private bool PlayerVisible(Vector3 targetPosition){
        Vector3 directionToTarget = targetPosition - gameObject.transform.position;
        directionToTarget.Normalize();

        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, directionToTarget, out hit);
        return hit.collider.gameObject.CompareTag("Player");
    }
}
