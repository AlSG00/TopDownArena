using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //private Rigidbody rb;

  //  [SerializeField]
  //  private float moveSpeed;

    public Transform target;

    public NavMeshAgent agent;
    [SerializeField]
    private AudioSource footstepsSource;
    [SerializeField]
    private List<AudioClip> footstepsSample;
    [SerializeField]
    private Animator anim;

    void Start()
    {
        //   rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        target = GameObject.Find("Player").transform.GetChild(0);
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

    private void PlayFootstepsSound()
    {
        footstepsSource.PlayOneShot(footstepsSample[Random.Range(0, footstepsSample.Count - 1)]);
    }
}
