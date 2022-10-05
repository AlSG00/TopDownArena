using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoachController : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject target;
    public NavMeshAgent agent;
    public float wanderMinDistance;
    public float wanderMaxDistance;
    public float wanderMinTime;
    public float wanderMaxTime;
    public float safeDistance;
    public bool run;
    private float timer;
    void Start()
    {
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        timer = 0;
    }

    private void FixedUpdate()
    {      
        if (Vector3.Distance(target.transform.position, transform.position) < safeDistance)
        {
            run = true;
            RunFrom();
        }
        else
        {
            run = false;
            Wander();
        }
    }
 
    private void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= Random.Range(wanderMinTime, wanderMaxTime))
        {
            Vector3 newPos = RandomNavSphere(transform.position, Random.Range(wanderMinDistance, wanderMaxDistance), -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public void RunFrom()
    {
        Vector3 runTo = transform.position + ((transform.position - target.transform.position) * 1);
        agent.SetDestination(runTo);
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * distance;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
