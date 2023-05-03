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
    //[SerializeField] private SCRIPT_InventoryController inventory;
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

    #endregion


    private void Start()
    {
        //inventory = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
        animationController = gameObject.GetComponent<Animator>();
        _swapper = GetComponent<FootstepSwapper>();
    }

    private void FixedUpdate()
    {
        Move();
        Sprint();
        HandleRotationInput();
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

        // TODO:Попробовать заменить этот код на обычный Clamp
        double sinForce = Mathf.Abs(Mathf.Sin(Mathf.Atan2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"))));
        double cosForce = Mathf.Abs(Mathf.Cos(Mathf.Atan2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"))));

        //Vector3 _movement = new Vector3(Input.GetAxisRaw("Horizontal") * (float)cosForce, 0, Input.GetAxisRaw("Vertical") * (float)sinForce);
        movement = new Vector3(Input.GetAxisRaw("Horizontal") * (float)cosForce, 0, Input.GetAxisRaw("Vertical") * (float)sinForce);

        if (movement.magnitude > 0)
        {
            animationController.SetBool("isWalking", true);
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
            int n = Random.Range(0, fotstepsAudioClips.Count);
            footstepsAudioSource.PlayOneShot(fotstepsAudioClips[n]);
        }
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
}
