using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// enum
public enum PlayerDir {Right, Left, Up, Down}
public class PlayerController_Simple : MonoBehaviour
{
    public static readonly int Hash_dirX = Animator.StringToHash("dirX");
    public static readonly int Hash_dirY = Animator.StringToHash("dirY");
    public static readonly int Hash_MovementType = Animator.StringToHash("MovementType");

    public static readonly int Hash_ActionTrigger = Animator.StringToHash("ActionTrigger");
    public static readonly int Hash_ActionId = Animator.StringToHash("ActionId");
    
    #region Inspektor Variables
    [Header("Player States")]
    public PlayerDir playerDir = PlayerDir.Down;
    
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float accelerationtime = 0.1f; 
    
    [Header("Roll")]
    [SerializeField] private float rollForce = 5f;
    
    [Header("Animations")]
    [SerializeField] private Animator[] anim;

    #endregion
    
    #region Private Variables
    public InputSystem_Actions inputActions;
    private InputAction moveAction;
    private InputAction rollAction;
    
    private Rigidbody2D rb;
    
    private Vector2 moveInput;
    private bool isRolling;
    #endregion
    
    #region Unity Event Functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        inputActions = new InputSystem_Actions();
        moveAction = inputActions.Player.Move;
        rollAction = inputActions.Player.Roll;
    }

    private void OnEnable()
    {
        EnableInput();
        moveAction.performed += MoveInput;
        moveAction.canceled += MoveInput;
        
        rollAction.performed += RollInput;
    }
    
    void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    private void OnDisable()
    {
        DisableInput();
        moveAction.performed -= MoveInput;
        moveAction.canceled -= MoveInput;
        
        rollAction.performed -= RollInput;

    }

    public void EnableInput()
    {
        inputActions.Enable();
    }

    public void DisableInput()
    {
        inputActions.Disable();
    }

    #endregion
    
    #region Movement
    void MoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
        PlayerDirection();
    }

    void RollInput(InputAction.CallbackContext context)
    {
        isRolling = true;
        
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(Hash_ActionTrigger);
            anim[i].SetInteger(Hash_ActionId, 1);
        }

        switch (playerDir)
        {
            case PlayerDir.Right:
                rb.linearVelocity = (Vector2.right * rollForce);
                break;
            
            case PlayerDir.Left:
                rb.AddForce(Vector2.left * rollForce, ForceMode2D.Impulse);
                break;
            
            
            case PlayerDir.Up:
                rb.AddForce(Vector2.up * rollForce, ForceMode2D.Impulse);
                break;
            
            case PlayerDir.Down:
                rb.AddForce(Vector2.down * rollForce, ForceMode2D.Impulse);
                break;
        }
    }
    
    void Movement()
    {
        if (isRolling) return;
        Vector2 targetVelocity = moveInput * walkSpeed; // (0,1) - (X:0,Y:5)
        Vector2 currentVelocity = rb.linearVelocity;
        
        rb.linearVelocity = Vector2.Lerp(currentVelocity, targetVelocity, Time.deltaTime / accelerationtime);
    }

    void PlayerDirection()
    {
        if (moveInput.x < 0)
        {
            playerDir = PlayerDir.Left;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveInput.x > 0)
        {
            playerDir = PlayerDir.Right;
            transform.rotation = Quaternion.Euler(0, 0, 0);

        }

        if (moveInput.y > 0)
        {
            playerDir = PlayerDir.Up;
        }
        else if (moveInput.y < 0)
        {
            playerDir = PlayerDir.Down;
        }
    }
    #endregion

    #region Animations

    void UpdateAnimator()
    {
        for (int i = 0; i < anim.Length; i++)
        {
            if (moveInput != Vector2.zero)
            {
                anim[i].SetFloat(Hash_dirX, moveInput.x);
                anim[i].SetFloat(Hash_dirY, moveInput.y);
            }

            anim[i].SetFloat(Hash_MovementType, moveInput != Vector2.zero ? 1 : 0);
        }
    }

    public void EndRolling()
    {
        isRolling = false;
    }
    
    #endregion
}
