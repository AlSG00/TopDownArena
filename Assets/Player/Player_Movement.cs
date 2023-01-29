using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player_Movement : MonoBehaviour
{
    #region Variables

    //TODO: Переименовать и красивенько оформить переменные
    [SerializeField] CinemachineVirtualCamera _vcamera;

    [Header("References")]
    [SerializeField] private SCRIPT_InventoryController inventory;
    [SerializeField] private SCRIPT_PlayerStamina _stamina;
    [SerializeField] private SCRIPT_PlayerCarryingWeight _carryingWeight;

    [Header("Movement parameters")]
    public float walkSpeed;
    public float walkSpeedDebuff;
    public float sprintSpeed;
    private float sprint; // Переименовать переменные по-нормальному
    public bool isRunning = false; 
    public float turnSpeed = 0.1f; // TODO: Пока не используется, исправить
    public Vector3 movement; //TODO: Нафига оно тут??
    
    [Header("Movement audio")]
    [SerializeField] private AudioSource footstepsAudioSource;
    [SerializeField] private List<AudioClip> fotstepsAudioClips;

    [Header("Movement animation")]
    [SerializeField] private Animator animationController;
    private Vector3 _moveDirection = Vector3.zero;
    [SerializeField] private LayerMask _footstepSoundLayer;

    private FootstepSwapper _swapper;
    public LayerMask activeLayers;

    /*НЕ УДАЛЯТЬ*/
    //private Vector3 _leftFootPosition;
    //private Vector3 _leftFootIKPosition;
    //private Vector3 _rightFootPosition;
    //private Vector3 _rightFootIKPosition;
    //private Quaternion _leftFootIKRotation;
    //private Quaternion _rightFootIKRotation;
    //private float _lastLeftFootPositionY;
    //private float _lastRightFootPositionY;
    //private float _lastPelvisPositionY;

    //[Header("Feet grounder")]
    //public bool enableFeetIK = true;
    //[Range(0, 2)] [SerializeField] private float heightFromGroundRaycast = 1f;
    //[Range(0, 2)] [SerializeField] private float raycastDownDistance = 1f;
    //[SerializeField] private LayerMask environmentLayer;
    //[SerializeField] private float pelvisOffset = 0f;
    //[Range(0, 1)] [SerializeField] private float pelvisUpAndDownSpeed = 0.2f;
    //[Range(0, 1)] [SerializeField] private float feetToIKPositionSpeed = 0.5f;

    //public string leftFootAnimVariableName = "LeftFootCurve";
    //public string rightFootAnimVariableName = "RightFootCurve";

    //public bool useProIKFeature = false;
    //public bool showSolverDebug = true;



    //public Vector3 movement;
    #endregion


    private void Start()
    {
        inventory = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
        animationController = gameObject.GetComponent<Animator>();
        _swapper = GetComponent<FootstepSwapper>();
    }

    private void FixedUpdate()
    {
        Move();
        Sprint();
        HandleRotationInput();

        /*НЕ УДАЛЯТЬ*/
        //if (!enableFeetIK)
        //{
        //    return;
        //}
        //if (animationController == null)
        //{
        //    return;
        //}

        //AdjustFeetTarget(ref _rightFootPosition, HumanBodyBones.RightFoot);
        //AdjustFeetTarget(ref _leftFootPosition, HumanBodyBones.LeftFoot);

        //FeetPositionSolver(_rightFootPosition, ref _rightFootIKPosition, ref _rightFootIKRotation);
        //FeetPositionSolver(_leftFootPosition, ref _leftFootIKPosition, ref _leftFootIKRotation);

    }

    private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), Vector3.down * 5, Color.yellow);
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!_stamina.isExhaused &&
                !_carryingWeight._isOvercarried)
            {
                sprint = sprintSpeed;
                isRunning = true;
                animationController.SetBool("isRunning", true);
            }
            else
            {
                Debug.Log("You are overcarried");
                sprint = 0;
                isRunning = false;
                animationController.SetBool("isRunning", false);
            }
        }
        else
        {
            sprint = 0;
            isRunning = false;
            animationController.SetBool("isRunning", false);
        }
    }

    private void Move()
    {
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        //TODO: Переименовать метод
        Animating(horiz, vert);

      //  animationController.SetFloat("horizontal", horiz);
      //  animationController.SetFloat("vertical", vert);
        //movement = new Vector3(horiz, 0, vert);
        //// movement.Normalize();
        //transform.Translate(movement * player_speed * Time.deltaTime, Space.World);

        // TODO:Попробовать заменить этот код на обычный Clamp
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

        transform.Translate(movement * (walkSpeed - walkSpeedDebuff + sprint) * Time.deltaTime, Space.World);
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

    private void PlayFootstepsSound(AnimationEvent evt)
    {
        _swapper.CheckLayers(activeLayers);
        if (evt.animatorClipInfo.weight > 0.5f)
        {
            //Debug.Log("Step");
            int n = Random.Range(0, fotstepsAudioClips.Count);
            footstepsAudioSource.PlayOneShot(fotstepsAudioClips[n]);
        }
        //footstepsSource.clip = footstepsSample[Random.Range(0, footstepsSample.Count - 1)];
        //footstepsAudioSource.PlayOneShot([Random.Range(0, footstepsSample.Count - 1)]);
        //Ray _ray = new Ray(gameObject.transform.position, Vector3.forward);
        //RaycastHit _hit;
        //if (Physics.Raycast(_ray, out _hit, 100, _footstepSoundLayer))
        //{
        //    _hit.collider
        //}
    }

    public void SwapFootsteps(FootstepCollection collection)
    {
        fotstepsAudioClips.Clear();
        for (int i = 0; i < collection.footstepSound.Count; i++)
        {
            fotstepsAudioClips.Add(collection.footstepSound[i]);
        }

    }

    // Используется, чтобы анимация проигрывалась корректно независимо от поворота игрока
    private void Animating(float h, float v)
    {
        _moveDirection = new Vector3(h, 0, v);

        if (_moveDirection.magnitude > 1.0f)
        {
            _moveDirection = _moveDirection.normalized;
        }

        _moveDirection = transform.InverseTransformDirection(_moveDirection).normalized;

        animationController.SetFloat("horizontal", _moveDirection.x, 1f, Time.deltaTime * 10f);
        animationController.SetFloat("vertical", _moveDirection.z, 1f, Time.deltaTime * 10f);
    }


    /*НЕ УДАЛЯТЬ*/
    //#region FeetGrounding

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    if (!enableFeetIK || animationController == null)
    //    {
    //        return;
    //    }

    //    MovePelvisHeight();

    //    animationController.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

    //    if (useProIKFeature)
    //    {
    //        animationController.SetIKRotationWeight(AvatarIKGoal.RightFoot, animationController.GetFloat(rightFootAnimVariableName));
    //    }

    //    MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, _leftFootIKPosition, _leftFootIKRotation, ref _lastLeftFootPositionY);

    //    animationController.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

    //    if (useProIKFeature)
    //    {
    //        animationController.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animationController.GetFloat(leftFootAnimVariableName));
    //    }

    //    MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, _leftFootIKPosition, _leftFootIKRotation, ref _lastLeftFootPositionY);
    //}

    //#endregion

    //#region FeetGroundingMethods

    //private void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY)
    //{
    //    Vector3 targetIKPosition = animationController.GetIKPosition(foot);

    //    if (positionIKHolder != Vector3.zero)
    //    {
    //        targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
    //        positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

    //        float yVariable = Mathf.Lerp(lastFootPositionY, positionIKHolder.y, feetToIKPositionSpeed);
    //        targetIKPosition.y += yVariable;

    //        lastFootPositionY = yVariable;

    //        targetIKPosition = transform.TransformPoint(targetIKPosition);
    //        animationController.SetIKRotation(foot, rotationIKHolder);
    //    }

    //    animationController.SetIKPosition(foot, targetIKPosition);
    //}

    //private void MovePelvisHeight()
    //{
    //    if (_rightFootIKPosition == Vector3.zero ||
    //        _leftFootIKPosition == Vector3.zero ||
    //        _lastPelvisPositionY == 0)
    //    {
    //        _lastPelvisPositionY = animationController.bodyPosition.y;
    //            return;
    //    }

    //    float lOffsetPosition = _leftFootIKPosition.y - transform.position.y;
    //    float rOffsetPosition = _rightFootIKPosition.y - transform.position.y;

    //    float totalOffset = (lOffsetPosition < rOffsetPosition) ? lOffsetPosition : rOffsetPosition;

    //    Vector3 newPelvisPosition = animationController.bodyPosition + Vector3.up * totalOffset;

    //    newPelvisPosition.y = Mathf.Lerp(_lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

    //    animationController.bodyPosition = newPelvisPosition;

    //    _lastPelvisPositionY = animationController.bodyPosition.y;
    //}

    //// Определяем позицию ступней через raycast
    //private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPositions, ref Quaternion feetIKRotation)
    //{
    //    RaycastHit feetOutHit;

    //    if (showSolverDebug)
    //    {
    //        Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.red);
    //    }

    //    if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
    //    {
    //        feetIKPositions = fromSkyPosition;
    //        feetIKPositions.y = feetOutHit.point.y + pelvisOffset;
    //        feetIKRotation = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

    //        return;
    //    }

    //    feetIKPositions = Vector3.zero;
    //}

    //private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
    //{
    //    feetPositions = animationController.GetBoneTransform(foot).position;
    //    feetPositions.y = transform.position.y + heightFromGroundRaycast;
    //}

    //#endregion
}
