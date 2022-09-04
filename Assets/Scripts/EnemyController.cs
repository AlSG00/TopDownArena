using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody rb;

  //  [SerializeField]
  //  private float moveSpeed;

    public GameObject target;

    public NavMeshAgent agent;
    void Start()
    {
     //   rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
    }
 
    void Update()
    {
     //   transform.LookAt(target.transform.position);        
    }

    private void FixedUpdate()
    {
        // rb.velocity = (transform.forward * moveSpeed);
        agent.SetDestination(target.transform.position);
    }
}
