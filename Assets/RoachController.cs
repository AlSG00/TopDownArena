using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoachController : MonoBehaviour
{
    private Rigidbody rb;

    //  [SerializeField]
    //  private float moveSpeed;

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
        //   rb = GetComponent<Rigidbody>();
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
      //  float distance = Vector3.Distance(transform.position, target.transform.position);
      /*  if (distance < safeDistance)*/ agent.SetDestination(runTo);
        // // store the starting transform
        // startTransform = transform;

        // //temporarily point the object to look away from the player
        // transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);

        // //Then we'll get the position on that rotation that's multiplyBy down the path (you could set a Random.range
        // // for this if you want variable results) and store it in a new Vector3 called runTo
        // Vector3 runTo = transform.position + transform.forward * multiplyBy;
        // //Debug.Log("runTo = " + runTo);

        // //So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.

        // NavMeshHit hit;    // stores the output in a variable called hit

        // // 5 is the distance to check, assumes you use default for the NavMesh Layer name
        // NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetNavMeshLayerFromName("Default"));
        // //Debug.Log("hit = " + hit + " hit.position = " + hit.position);

        // // just used for testing - safe to ignore
        //// nextTurnTime = Time.time + 5;

        // // reset the transform back to our start transform
        // transform.position = startTransform.position;
        // transform.rotation = startTransform.rotation;

        // // And get it to head towards the found NavMesh position
        // agent.SetDestination(hit.position);
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
