using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// enum
public enum PlayerDir {Right, Left, Up, Down}
public class PlayerController : MonoBehaviour
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
    private InputAction interactAction;

    private InputAction attackAction;
    private InputAction pickaxeAction;
    private InputAction axeAction;
    private InputAction canAction;
    private InputAction bowAction;
    
    private Rigidbody2D rb;
    
    private Vector2 moveInput;
    private Vector2 lastMoveInput;
    private bool isRolling;
    private bool isAttacking;
    private bool isPickaxe;
    private bool isAxe;
    private bool isCan;
    private bool isBow;

    private Interactable selectedInteractable;
    #endregion
    
    #region Unity Event Functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        inputActions = new InputSystem_Actions();
        moveAction = inputActions.Player.Move;
        rollAction = inputActions.Player.Roll;
        attackAction = inputActions.Player.Attack;
        pickaxeAction = inputActions.Player.Pickaxe;
        axeAction = inputActions.Player.Axe;
        canAction = inputActions.Player.Can;
        bowAction = inputActions.Player.Bow;
        interactAction = inputActions.Player.Interact;
    }

    private void OnEnable()
    {
        EnableInput();
        moveAction.performed += MoveInput;
        moveAction.canceled += MoveInput;
        
        rollAction.performed += RollInput;
        
        attackAction.performed += AttackInput;
        
        pickaxeAction.performed += PickaxeInput;
        
        axeAction.performed += AxeInput;
        
        canAction.performed += CanInput;
        
        bowAction.performed += BowInput;
        
        interactAction.performed += Interact;
        
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

        attackAction.performed -= AttackInput;
        
        pickaxeAction.performed -= PickaxeInput;
        
        axeAction.performed -= AxeInput;
        
        canAction.performed -= CanInput;
        
        bowAction.performed -= BowInput;
        
        interactAction.performed -= Interact;
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
    
    #region Interaction

    private void Interact(InputAction.CallbackContext ctx)
    {
        if(selectedInteractable != null)
        {
            selectedInteractable.Interact();
        }
    }

    private void TrySelectInteractable(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null) return;

        if (selectedInteractable != null)
        {
            selectedInteractable.Deselect();
        }

        selectedInteractable = interactable;
        selectedInteractable.Select();
    }

    private void TryDeselectInteractable(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null) return;

        if (interactable == selectedInteractable)
        {
            selectedInteractable.Deselect();
            selectedInteractable = null;
        }
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
        if (isRolling) return;
        
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
                rb.linearVelocity = (Vector2.left * rollForce);
                break;
            
            
            case PlayerDir.Up:
                rb.linearVelocity = (Vector2.up * rollForce);
                break;
            
            case PlayerDir.Down:
                rb.linearVelocity = (Vector2.down * rollForce);
                break;
        }
    }

    void AttackInput(InputAction.CallbackContext context)
    {
        if (isAttacking) return;

        isAttacking = true;
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(Hash_ActionTrigger);
            anim[i].SetInteger(Hash_ActionId, 2);
        }
    }
    
    void PickaxeInput(InputAction.CallbackContext context)
    {
        if (isPickaxe) return;

        isPickaxe = true;
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(Hash_ActionTrigger);
            anim[i].SetInteger(Hash_ActionId, 3);
        }
    }
    
    void AxeInput(InputAction.CallbackContext context)
    {
        if (isAxe) return;

        isAxe = true;
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(Hash_ActionTrigger);
            anim[i].SetInteger(Hash_ActionId, 4);
        }
    }
    
    void CanInput(InputAction.CallbackContext context)
    {
        if (isCan) return;

        isCan = true;
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(Hash_ActionTrigger);
            anim[i].SetInteger(Hash_ActionId, 5);
        }
    }
    
    void BowInput(InputAction.CallbackContext context)
    {
        if (isBow) return;

        isBow = true;
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(Hash_ActionTrigger);
            anim[i].SetInteger(Hash_ActionId, 6);
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
    
    #region Physics
    private void OnTriggerEnter2D(Collider2D other)
    {
        TrySelectInteractable(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TryDeselectInteractable(other);
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

                lastMoveInput = moveInput;
            }

            anim[i].SetFloat(Hash_MovementType, moveInput != Vector2.zero ? 1 : 0);
        }
    }

    public void EndRolling()
    {
        isRolling = false;
    }
    
    public void EndAttacking()
    {
        isAttacking = false;
    }

    public void EndPickaxe()
    {
        isPickaxe = false;
    }
    
    public void EndAxe()
    {
        isAxe = false;
    }
    
    public void EndCan()
    {
        isCan = false;
    }
    
    public void EndBow()
    {
        isBow = false;
    }
    public Vector2 GetMoveInput()
    {
        return lastMoveInput;
    }
    
    #endregion
    
}
