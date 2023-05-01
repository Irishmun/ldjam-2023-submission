using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.Windows;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerMovement : MonoBehaviour
{
    private const float SENSITIVITY_DIVIDER = 200f;
    [Header("Moving")]
    [SerializeField] private float MovementSpeed = 3.6f;
    [SerializeField] private float SprintSpeed = 6.1f;
    [Header("Looking")]
    [SerializeField] private Transform PlayerCamera;
    //[SerializeField] private int Sensitivity = 50;
    [SerializeField] private float MaxLookAngle = 75f;//75 degrees
    [Header("Interactions")]
    [SerializeField] private PlayerLookInteract playerLookInteract;
    [Header("Audio")]
    [SerializeField] private AudioClip[] WalkingClips;
    [Header("Animator")]
    [SerializeField] private Animator checklistAnimator;
    [Header("PauseMenu")]
    [SerializeField] private GameObject pauseObject;


    private AudioSource _audioWalking;
    private Animator _animator;

    private Vector3 _moveInput;
    private Vector2 _lookInput, _look;
    private float _currentSpeed;
    private bool _isSprinting;


    //generated in script
    private PlayerInput _input;
    private Rigidbody _rb;
    private PlayerInputActions.PlayerActions _inputActions;

    private bool _canWalk = true, _canLook = true, _canRun = true;
    private bool _showChecklist = false;



    private void Awake()
    {
        _inputActions = new PlayerInputActions().Player;
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _audioWalking = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log((float)(GlobalState.SENSITIVITY / SENSITIVITY_DIVIDER));
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalState.IS_PAUSED == false && GlobalState.IS_INTERACTING == false)
        {
            HandleSprint();
            MovePlayer();
            LookPlayer();
        }
    }
    private void MovePlayer()
    {
        Vector3 MoveDirection = transform.TransformDirection(_moveInput) * _currentSpeed;
        _rb.velocity = new Vector3(MoveDirection.x, _rb.velocity.y, MoveDirection.z);
    }
    private void LookPlayer()
    {
        _look.x += (_lookInput.x * (float)(GlobalState.SENSITIVITY / SENSITIVITY_DIVIDER));
        _look.y -= (_lookInput.y * (float)(GlobalState.SENSITIVITY / SENSITIVITY_DIVIDER));
        _look.y = Mathf.Clamp(_look.y - (_lookInput.y * (float)(GlobalState.SENSITIVITY / SENSITIVITY_DIVIDER)), -MaxLookAngle, MaxLookAngle);
        transform.localEulerAngles = new Vector3(0, _look.x, 0);
        PlayerCamera.localEulerAngles = new Vector3(_look.y, 0, 0);
    }
    private void HandleSprint()
    {
        if (_isSprinting == true)
        {
            _currentSpeed = SprintSpeed;
            if (_canWalk == true && _moveInput != Vector3.zero)
            {
                _animator.speed = SprintSpeed / MovementSpeed;
            }
        }
        else
        {
            _currentSpeed = MovementSpeed;
            _animator.speed = 1;
        }
    }

    private void OnEnable()
    {
        InputActionMap controls = _input.actions.FindActionMap("Player");
        controls.FindAction(_inputActions.Move.name).Enable();
        SubscribeMovementEvents(controls);
        SubscribeInteractionEvents(controls);
    }

    private void OnDisable()
    {
        InputActionMap controls = _input.actions.FindActionMap("Player");
        UnSubscribeMovementEvents(controls);
        UnSubscribeInteractionEvents(controls);
        controls.FindAction(_inputActions.Move.name).Disable();
    }


    private void Input_Move(InputAction.CallbackContext ctx)
    {
        if (_canWalk == true)
        {
            Vector2 moveInput = ctx.ReadValue<Vector2>();
            _moveInput = new Vector3(moveInput.x, 0, moveInput.y);
            _animator.SetBool("IsWalking", moveInput != Vector2.zero);
        }
        else
        {
            _moveInput = Vector3.zero;
            _animator.SetBool("IsWalking", false);
        }
    }
    private void Input_Look(InputAction.CallbackContext ctx)
    {
        if (_canLook == true)
        {
            _lookInput = ctx.ReadValue<Vector2>();
        }
        else
        {
            _lookInput = Vector2.zero;
        }
    }

    private void Input_Sprint(InputAction.CallbackContext ctx)
    {
        if (_canRun == true)
        {
            _isSprinting = ctx.started == true;

        }
        else
        {
            _isSprinting = false;
        }
    }
    private void Input_Interact(InputAction.CallbackContext obj)
    {
        if (playerLookInteract == null)
        { return; }
        playerLookInteract.TryInteract();
    }
    private void Input_Pause(InputAction.CallbackContext obj)
    {
        PauseGame(!GlobalState.IS_PAUSED);
    }
    private void Input_Cheklist(InputAction.CallbackContext ctx)
    {
        if (GlobalState.HAS_CHECKLIST == true)
        {
            _showChecklist = !_showChecklist;
            checklistAnimator.SetBool("ShowChecklist", _showChecklist);
        }
    }

    private void SubscribeMovementEvents(InputActionMap controls)
    {
        //Move
        InputAction MoveAction = controls.FindAction(_inputActions.Move.name);
        MoveAction.started += Input_Move;
        MoveAction.performed += Input_Move;
        MoveAction.canceled += Input_Move;
        //Look
        InputAction LookAction = controls.FindAction(_inputActions.Look.name);
        LookAction.started += Input_Look;
        LookAction.performed += Input_Look;
        LookAction.canceled += Input_Look;
        //Sprint
        InputAction SprintAction = controls.FindAction(_inputActions.Sprint.name);
        SprintAction.started += Input_Sprint;
        SprintAction.canceled += Input_Sprint;
    }
    private void UnSubscribeMovementEvents(InputActionMap controls)
    {
        //Move
        InputAction MoveAction = controls.FindAction(_inputActions.Move.name);
        MoveAction.started -= Input_Move;
        MoveAction.performed -= Input_Move;
        MoveAction.canceled -= Input_Move;
        //Look
        InputAction LookAction = controls.FindAction(_inputActions.Look.name);
        LookAction.started -= Input_Look;
        LookAction.performed -= Input_Look;
        LookAction.canceled -= Input_Look;
        //Sprint
        InputAction SprintAction = controls.FindAction(_inputActions.Sprint.name);
        SprintAction.started -= Input_Sprint;
        SprintAction.canceled -= Input_Sprint;
    }
    private void SubscribeInteractionEvents(InputActionMap controls)
    {
        InputAction interactAction = controls.FindAction(_inputActions.Interact.name);
        interactAction.started += Input_Interact;

        InputAction pauseGameAction = controls.FindAction(_inputActions.PauseGame.name);
        pauseGameAction.started += Input_Pause;

        InputAction checklistAction = controls.FindAction(_inputActions.Checklist.name);
        checklistAction.started += Input_Cheklist;
    }
    private void UnSubscribeInteractionEvents(InputActionMap controls)
    {
        InputAction InteractAction = controls.FindAction(_inputActions.Interact.name);
        InteractAction.started -= Input_Interact;

        InputAction pauseGameAction = controls.FindAction(_inputActions.PauseGame.name);
        pauseGameAction.started -= Input_Pause;

        InputAction checklistAction = controls.FindAction(_inputActions.Checklist.name);
        checklistAction.started -= Input_Cheklist;
    }


    public void PlayWalkSound()
    {
        if (_audioWalking == null)
        { return; }

        if (WalkingClips.Length > 0)
        {
            _audioWalking.PlayOneShot(WalkingClips[Random.Range(0, WalkingClips.Length)]);
        }
    }
    public void ForceChecklist()
    {
        _showChecklist = true;
        checklistAnimator.SetBool("ShowChecklist", _showChecklist);
    }

    public void PauseGame(bool pause)
    {
        GlobalState.IS_PAUSED = pause;
        if (pause == true)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
        pauseObject.SetActive(pause);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward);
    }
#endif

}
