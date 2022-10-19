using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float player_speed;
    [SerializeField] private float player_sprint;
   // public Rigidbody rigidBody;
    //public Camera camera;
    private float sprintSpeed;
    bool isSprinting;
    Vector3 movement;
    [SerializeField] AudioSource footstepsSource;
    [SerializeField] List<AudioClip> footstepsSample;
    [SerializeField] Animator animationController;

    private void Start()
    {
        animationController = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //  rigidBody.MovePosition(rigidBody.position + movement * player_speed * Time.fixedDeltaTime);
        //float horiz = Input.GetAxis("Horizontal");
        //float vert = Input.GetAxis("Vertical");

        //movement = new Vector3(horiz, 0, vert);
        //movement.Normalize();
        //transform.Translate(movement * player_speed * Time.deltaTime, Space.World);
        Move();
        Sprint();
        HandleRotationInput();
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprintSpeed = player_sprint;
            animationController.SetBool("isRunning", true);
        }
        else
        {
            sprintSpeed = 1;
            animationController.SetBool("isRunning", false);
        }
        
    }

    private void Move()
    {
        //float horiz = Input.GetAxis("Horizontal");
        //float vert = Input.GetAxis("Vertical");

        //movement = new Vector3(horiz, 0, vert);
        //// movement.Normalize();
        //transform.Translate(movement * player_speed * Time.deltaTime, Space.World);

        double sinForce = Mathf.Abs(Mathf.Sin(Mathf.Atan2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"))));
        double cosForce = Mathf.Abs(Mathf.Cos(Mathf.Atan2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"))));

        Vector3 _movement = new Vector3(Input.GetAxisRaw("Horizontal") * (float)cosForce, 0, Input.GetAxisRaw("Vertical") * (float)sinForce);
        if (_movement.magnitude > 0)
        {
            animationController.SetBool("isWalking", true);
        }
        else
        {
            animationController.SetBool("isWalking", false);
        }

        transform.Translate(_movement * player_speed * sprintSpeed * Time.deltaTime, Space.World);
    }

    void HandleRotationInput()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    private void PlayFootstepsSound()
    {
        //footstepsSource.clip = footstepsSample[Random.Range(0, footstepsSample.Count - 1)];
        footstepsSource.PlayOneShot(footstepsSample[Random.Range(0, footstepsSample.Count - 1)]);
    }
}
