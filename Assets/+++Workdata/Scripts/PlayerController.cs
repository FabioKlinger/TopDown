using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//public enum PlayerDir { Right, Up, Left, Down }
public enum PlayerAction { Default, Idle, Walk, Run, Roll, Attack }
public class PlayerController : MonoBehaviour
{
    public static readonly int Hash_MovementValue = Animator.StringToHash("MovementValue");
    public static readonly int Hash_dirX = Animator.StringToHash("dirX");
    public static readonly int Hash_dirY = Animator.StringToHash("dirY");
    
    #region Public Variables
    
    [Header("Player Type")]
    public PlayerDir playerDir = PlayerDir.Right;  
    public PlayerAction playerAction = PlayerAction.Idle;
    
    public Animator[] anim;

    [Header("Movement")]
    [SerializeField] private  float walkSpeed = 5f;
    [SerializeField] private  float runSpeed = 8f;
    [SerializeField] private  float maxVelocity = 10f;
    [SerializeField] private float accelerationTime;

    #endregion
    
    #region Private Variables
    public InputSystem_Actions inputActions;
    private InputAction moveAction;
    
    private Rigidbody2D rb;
    
    private Vector2 moveInput;
    private Vector2 _currentVelocity;
    
    private float currentSpeed;
    #endregion
    
    #region Unity Event Functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new InputSystem_Actions();
        moveAction = inputActions.Player.Move;

        currentSpeed = walkSpeed;
    }

    private void OnEnable()
    {
        EnableInput();
        moveAction.performed += MoveInput;
        moveAction.canceled += MoveInput;
    }
    
    void FixedUpdate()
    {
        ReadInput();
        Movement();

        CheckVelocity();
    }

    private void Update()
    {
        PlayerDirection();

        if (inputActions.Player.enabled)
            UpdateAnimations();
    }

    private void OnDisable()
    {
        DisableInput();
        moveAction.performed -= MoveInput;
        moveAction.canceled -= MoveInput;
    }

    public void EnableInput()
    {
        inputActions.Enable();
    }

    public void DisableInput()
    {
        inputActions.Disable();
    }

    void ReadInput()
    {
        
    }

    #endregion
    
    #region Movement
    void MoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
    }

    void Movement()
    {
        if (playerAction == PlayerAction.Attack)
        {
            return;
        }
    
        Vector2 targetVelocity;
        
        targetVelocity = moveInput * currentSpeed;
        

        Vector2 currentVelocity = rb.linearVelocity;
        

        rb.linearVelocity = Vector2.Lerp(currentVelocity, targetVelocity, Time.deltaTime / accelerationTime);
    }

    void PlayerDirection()
    {
        if (playerAction != PlayerAction.Attack)
        {
            if (moveInput.x < 0)
            {
                if (playerDir == PlayerDir.Left) return;
                playerDir = PlayerDir.Left;
                
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (moveInput.x > 0)
            {
                if (playerDir == PlayerDir.Right) return;
                playerDir = PlayerDir.Right;
                
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                if (moveInput.y > 0)
                {
                    if (playerDir == PlayerDir.Up) return;
                    playerDir = PlayerDir.Up;
                }
                else if (moveInput.y < 0)
                {
                    if (playerDir == PlayerDir.Down) return;
                    playerDir = PlayerDir.Down;
                }
            }
        }
    }
    
    #endregion
    
    #region Animation
    public void UpdateAnimations()
    {
        for (int i = 0; i < anim.Length; i++)
        {
            if (moveInput != Vector2.zero && playerAction != PlayerAction.Attack)
            {
                moveInput = moveInput.normalized;
                anim[i].SetFloat(Hash_dirX, moveInput.x);
                anim[i].SetFloat(Hash_dirY, moveInput.y);
            }
            anim[i].SetFloat(Hash_MovementValue, moveInput == Vector2.zero ? 0 : currentSpeed);
        }
    }
    
    #endregion

    #region Physics
    
    void CheckVelocity()
    {
        Vector2 currentVelocity = rb.linearVelocity;

        if (Mathf.Abs(currentVelocity.x) > maxVelocity)
        {
            currentVelocity.x = Mathf.Sign(currentVelocity.x) * maxVelocity;
        }

        if (Mathf.Abs(currentVelocity.y) > maxVelocity)
        {
            currentVelocity.y = Mathf.Sign(currentVelocity.y) * maxVelocity;
        }

        rb.linearVelocity = currentVelocity;
    }
    #endregion
}
