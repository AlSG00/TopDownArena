using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    #region Variables

    [SerializeField] private float player_speed;
    [SerializeField] private float player_sprint;
   // public Rigidbody rigidBody;
    //public Camera camera;
    private float sprintSpeed;
    public bool isRunning = false;
    public Vector3 movement;
    [SerializeField] private AudioSource footstepsSource;
    [SerializeField] private List<AudioClip> footstepsSample;
    [SerializeField] private Animator animationController;

    public float turnSpeed = 0.1f;
    private Vector3 moveDirection = Vector3.zero;

    private Vector3 _leftFootPosition;
    private Vector3 _leftFootIKPosition;
    private Vector3 _rightFootPosition;
    private Vector3 _rightFootIKPosition;
    private Quaternion _leftFootIKRotation;
    private Quaternion _rightFootIKRotation;
    private float _lastLeftFootPositionY;
    private float _lastRightFootPositionY;
    private float _lastPelvisPositionY;

    [Header("Feet grounder")]
    public bool enableFeetIK = true;
    [Range(0, 2)] [SerializeField] private float heightFromGroundRaycast = 1f;
    [Range(0, 2)] [SerializeField] private float raycastDownDistance = 1f;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private float pelvisOffset = 0f;
    [Range(0, 1)] [SerializeField] private float pelvisUpAndDownSpeed = 0.2f;
    [Range(0, 1)] [SerializeField] private float feetToIKPositionSpeed = 0.5f;

    public string leftFootAnimVariableName = "LeftFootCurve";
    public string rightFootAnimVariableName = "RightFootCurve";

    public bool useProIKFeature = false;
    public bool showSolverDebug = true;

    private SCRIPT_InventoryController inventory;

    [SerializeField] private SCRIPT_PlayerStamina _playerStamina;

    //public Vector3 movement;
    #endregion


    private void Start()
    {
        inventory = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
        animationController = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        Sprint();
        HandleRotationInput();

        if (!enableFeetIK)
        {
            return;
        }
        if (animationController == null)
        {
            return;
        }

        AdjustFeetTarget(ref _rightFootPosition, HumanBodyBones.RightFoot);
        AdjustFeetTarget(ref _leftFootPosition, HumanBodyBones.LeftFoot);

        FeetPositionSolver(_rightFootPosition, ref _rightFootIKPosition, ref _rightFootIKRotation);
        FeetPositionSolver(_leftFootPosition, ref _leftFootIKPosition, ref _leftFootIKRotation);

    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!_playerStamina.isExhaused)
            {
                sprintSpeed = player_sprint;
                isRunning = true;
                animationController.SetBool("isRunning", true);
            }
            else
            {
                sprintSpeed = 1;
                isRunning = false;
                animationController.SetBool("isRunning", false);
            }
        }
        else
        {
            sprintSpeed = 1;
            isRunning = false;
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

        //Vector3 _movement = new Vector3(Input.GetAxisRaw("Horizontal") * (float)cosForce, 0, Input.GetAxisRaw("Vertical") * (float)sinForce);
        movement = new Vector3(Input.GetAxisRaw("Horizontal") * (float)cosForce, 0, Input.GetAxisRaw("Vertical") * (float)sinForce);

        if (movement.magnitude > 0)
        {
            animationController.SetBool("isWalking", true);
            inventory.isCheckingInventory = false;
            inventory.HandleInventoryGrid(false);
        }
        else
        {
            animationController.SetBool("isWalking", false);
        }

        transform.Translate(movement * player_speed * sprintSpeed * Time.deltaTime, Space.World);

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

    #region FeetGrounding

    private void OnAnimatorIK(int layerIndex)
    {
        if (!enableFeetIK || animationController == null)
        {
            return;
        }

        MovePelvisHeight();

        animationController.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

        if (useProIKFeature)
        {
            animationController.SetIKRotationWeight(AvatarIKGoal.RightFoot, animationController.GetFloat(rightFootAnimVariableName));
        }

        MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, _leftFootIKPosition, _leftFootIKRotation, ref _lastLeftFootPositionY);

        animationController.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

        if (useProIKFeature)
        {
            animationController.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animationController.GetFloat(leftFootAnimVariableName));
        }

        MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, _leftFootIKPosition, _leftFootIKRotation, ref _lastLeftFootPositionY);
    }

    #endregion

    #region FeetGroundingMethods

    private void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY)
    {
        Vector3 targetIKPosition = animationController.GetIKPosition(foot);

        if (positionIKHolder != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

            float yVariable = Mathf.Lerp(lastFootPositionY, positionIKHolder.y, feetToIKPositionSpeed);
            targetIKPosition.y += yVariable;

            lastFootPositionY = yVariable;

            targetIKPosition = transform.TransformPoint(targetIKPosition);
            animationController.SetIKRotation(foot, rotationIKHolder);
        }

        animationController.SetIKPosition(foot, targetIKPosition);
    }

    private void MovePelvisHeight()
    {
        if (_rightFootIKPosition == Vector3.zero ||
            _leftFootIKPosition == Vector3.zero ||
            _lastPelvisPositionY == 0)
        {
            _lastPelvisPositionY = animationController.bodyPosition.y;
                return;
        }

        float lOffsetPosition = _leftFootIKPosition.y - transform.position.y;
        float rOffsetPosition = _rightFootIKPosition.y - transform.position.y;

        float totalOffset = (lOffsetPosition < rOffsetPosition) ? lOffsetPosition : rOffsetPosition;

        Vector3 newPelvisPosition = animationController.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(_lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

        animationController.bodyPosition = newPelvisPosition;

        _lastPelvisPositionY = animationController.bodyPosition.y;
    }

    // ќпредел€ем позицию ступней через raycast
    private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPositions, ref Quaternion feetIKRotation)
    {
        RaycastHit feetOutHit;

        if (showSolverDebug)
        {
            Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.red);
        }

        if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
        {
            feetIKPositions = fromSkyPosition;
            feetIKPositions.y = feetOutHit.point.y + pelvisOffset;
            feetIKRotation = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

            return;
        }

        feetIKPositions = Vector3.zero;
    }

    private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
    {
        feetPositions = animationController.GetBoneTransform(foot).position;
        feetPositions.y = transform.position.y + heightFromGroundRaycast;
    }

    #endregion
}
