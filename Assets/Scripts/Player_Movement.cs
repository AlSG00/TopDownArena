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

    public float turnSpeed = 0.1f;

    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        animationController = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
      //  transform.parent.position = transform.position - transform.localPosition;
        //Debug.Log($"{transform.parent.name} position is {transform}");
        // GetTurnAngle();
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
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        Animating(horiz, vert);

      //  animationController.SetFloat("horizontal", horiz);
      //  animationController.SetFloat("vertical", vert);
        //movement = new Vector3(horiz, 0, vert);
        //// movement.Normalize();
        //transform.Translate(movement * player_speed * Time.deltaTime, Space.World);

        // TODO:ѕопробовать заменить этот код на обычный Clamp
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
            //lock;alskdh;fiolahs;fo
            //Vector3 direction = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            //Quaternion rotation = Quaternion.LookRotation(direction);
            //  transform.rotation = Quaternion.FromToRotation(transform.position, direction);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.time * turnSpeed);
            // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed);
             transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    private void PlayFootstepsSound()
    {
        //footstepsSource.clip = footstepsSample[Random.Range(0, footstepsSample.Count - 1)];
        footstepsSource.PlayOneShot(footstepsSample[Random.Range(0, footstepsSample.Count - 1)]);
    }

    // »спользуетс€, чтобы анимаци€ проигрывалась корректно независимо от поворота игрока
    private void Animating(float h, float v)
    {
        moveDirection = new Vector3(h, 0, v);

        if (moveDirection.magnitude > 1.0f)
        {
            moveDirection = moveDirection.normalized;
        }

        moveDirection = transform.InverseTransformDirection(moveDirection).normalized;

        animationController.SetFloat("horizontal", moveDirection.x, 1f, Time.deltaTime * 10f);
        animationController.SetFloat("vertical", moveDirection.z, 1f, Time.deltaTime * 10f);
    }
}
