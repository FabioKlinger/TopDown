using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


//enum
public enum PlayerDir{Right, Left, Up, Down}
public class PlayerController_Simple : MonoBehaviour
{
    public static readonly int Hash_dirX = Animator.StringToHash("dirX");
    public static readonly int Hash_dirY = Animator.StringToHash("dirY");
    public static readonly int Hash_MovementType = Animator.StringToHash("MovementType");
    public static readonly int Hash_ActionTriggerValue = Animator.StringToHash("ActionTrigger");
    public static readonly int Hash_ActionIdValue = Animator.StringToHash("ActionId");
    
    #region Inspector Variables
    [Header("Player States")]
    public PlayerDir playerDir = PlayerDir.Down;
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    
    [SerializeField] private float accelerationtime = 0.1f; //Player erreicht in einer bestimmten Zeit den Maximum der Geschwindigkeit
    
    [Header("Animations")]
    [SerializeField] private Animator[] anim;
    
    [Header("Actions")]
    private bool isRolling;
    public float rollPower = 5f;
    public bool canRoll;
    
    
    #endregion
    
    #region Private Variables

    public InputSystem_Actions inputActions;
    private InputAction moveAction;
    private InputAction rollAction;
    
    private Rigidbody2D rb;
    
    
    private Vector2 moveInput;
    #endregion
    
    #region Unity Event Functions

    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        
        
        inputActions= new InputSystem_Actions();
        moveAction = inputActions.Player.Move;
        rollAction = inputActions.Player.Roll;
        
        canRoll = true;
    }
    
    private void OnEnable()
    {
    EnableInput();
    moveAction.performed += MoveInput;
    moveAction.canceled += MoveInput;
    
    rollAction.performed += Roll;
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
        
        rollAction.performed -= Roll;   
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
    
    void MoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>().normalized;
        PlayerDirection();
    }

    void Movement()
    {
        Vector2 targetVelocity = moveInput * walkSpeed; // (0,1) - (X:0,Y:5)
        Vector2 currentVelocity = rb.linearVelocity;
        
        rb.linearVelocity = Vector2.Lerp(currentVelocity, targetVelocity, Time.deltaTime / accelerationtime);
    }

    void PlayerDirection()
    {
        if (moveInput.x < 0)
        {
            playerDir = PlayerDir.Left;
        }
        else if (moveInput.x > 0)
        {
            playerDir = PlayerDir.Right;
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

    #region Roll
    private void Roll(InputAction.CallbackContext ctx)
    {
        if (canRoll)
        {
            //canRoll = false;
            Vector2 rollDirection = GetRollDirection();
            rb.AddForce(rollDirection * rollPower, ForceMode2D.Impulse);
            
            AnimatorAction(GetRollDirectionId());
        }
    }

    private Vector2 GetRollDirection()
    {
        switch (playerDir)
        {
            case PlayerDir.Up: return Vector2.up;
            case PlayerDir.Down: return Vector2.down;
            case PlayerDir.Left: return Vector2.left;
            case PlayerDir.Right: return Vector2.right;
            default: return Vector2.zero;
        }
    }
    
    private int GetRollDirectionId()
    {
        switch (playerDir)
        {
            case PlayerDir.Up: return 0;
            case PlayerDir.Down: return 1;
            case PlayerDir.Left: return 2;
            case PlayerDir.Right: return 3;
            default: return -1;
        }
    }
    

    #endregion
    
    #region Animations

    void UpdateAnimator()
    {
        for (int i = 0; i < anim.Length; i++)
        {
            if (moveInput !=
                Vector2.zero) //Verhindert das der Spieler nach dem Bewegen sich in eine bestimmte ungewollte Richtung dreht. 
            {
                anim[i].SetFloat(Hash_dirX, moveInput.x);
                anim[i].SetFloat(Hash_dirY, moveInput.y);
            }

            anim[i].SetFloat(Hash_MovementType, moveInput != Vector2.zero ? 1 : 0);
        }
    }
    
    void AnimatorAction(int actionId)
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(Hash_ActionTriggerValue);
            anim[i].SetInteger(Hash_ActionIdValue, actionId);
        }
    }
    #endregion
}
