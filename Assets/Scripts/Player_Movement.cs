using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    private float player_speed;
    public Rigidbody rigidBody;
    //public Camera camera;

    Vector3 movement;

    //void Update()
    //{

    //    //movement.x = Input.GetAxis("Horizontal");
    //    //movement.z = Input.GetAxis("Vertical");
    //    FixedUpdate();
    //    HandleRotationInput();
    //}

    private void FixedUpdate()
    {
        //  rigidBody.MovePosition(rigidBody.position + movement * player_speed * Time.fixedDeltaTime);
        //float horiz = Input.GetAxis("Horizontal");
        //float vert = Input.GetAxis("Vertical");

        //movement = new Vector3(horiz, 0, vert);
        //movement.Normalize();
        //transform.Translate(movement * player_speed * Time.deltaTime, Space.World);
        Move();
        HandleRotationInput();
    }

    private void Move()
    {
        //float horiz = Input.GetAxis("Horizontal");
        //float vert = Input.GetAxis("Vertical");

        //movement = new Vector3(horiz, 0, vert);
        //// movement.Normalize();
        //transform.Translate(movement * player_speed * Time.deltaTime, Space.World);

        //    //Movement
        double sinForce = Mathf.Abs(Mathf.Sin(Mathf.Atan2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"))));
        double cosForce = Mathf.Abs(Mathf.Cos(Mathf.Atan2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"))));

        Vector3 _movement = new Vector3(Input.GetAxisRaw("Horizontal") * (float)cosForce, 0, Input.GetAxisRaw("Vertical") * (float)sinForce);

        transform.Translate(_movement * player_speed * Time.deltaTime, Space.World);
    }

        void HandleRotationInput()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }


    //Movement
    //double sinForce = Mathf.Abs(Mathf.Sin(Mathf.Atan2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"))));
    //double cosForce = Mathf.Abs(Mathf.Cos(Mathf.Atan2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"))));

    //Vector3 _movement = new Vector3(Input.GetAxisRaw("Horizontal") * (float)cosForce, 0, Input.GetAxisRaw("Vertical") * (float)sinForce);
}
