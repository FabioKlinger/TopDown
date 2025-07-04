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
    public static Action InventoryAction;
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float accelerationtime = 0.1f; 
    
    [Header("Roll")]
    [SerializeField] private float rollForce = 5f;
    
    [Header("Animations")]
    [SerializeField] private Animator[] anim;

    private EnemyHealth enemyHealth;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private int damage;
    [SerializeField] private float colliderDistance;
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
    private InputAction inventoryAction;
    
    private Rigidbody2D rb;
    
    private Vector2 moveInput;
    private Vector2 lastMoveInput;
    private bool isRolling;
    private bool isAttacking;
    private bool isPickaxe;
    private bool isAxe;
    private bool isCan;
    private bool isBow;
    public bool autoMovement = false;
    

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
        inventoryAction = inputActions.Player.Inventory;
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
        
        inventoryAction.performed += InventoryInput;
        
    }
    
    void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        if (autoMovement) return;
        
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
        
        inventoryAction.performed -= InventoryInput;
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
    void InventoryInput(InputAction.CallbackContext context)
    {
        InventoryAction?.Invoke();
    }
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
#region Attack
    void AttackInput(InputAction.CallbackContext context)
    {
        if (isAttacking) return;
        boxCollider.enabled = true;
        isAttacking = true;
        for (int i = 0; i < anim.Length; i++)
        {
            if (EnemyInSight())
                 {
                     DamageEnemy();
                 }
            anim[i].SetTrigger(Hash_ActionTrigger);
            anim[i].SetInteger(Hash_ActionId, 2);
        }
        boxCollider.enabled = false;

        
    }

    private bool EnemyInSight()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
                0, Vector2.left, 0, enemyLayer);
        if (hit.collider != null)
            enemyHealth = hit.transform.GetComponent<EnemyHealth>();
        
        return hit.collider != null;
    }

    void DamageEnemy()
    {
        if (EnemyInSight())
        {
            
            enemyHealth.TakeDamage(damage);
        }
    }
    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    
    #endregion
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

    public void PlayerDirection()
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
        if (autoMovement) return;

        for (int i = 0; i < anim.Length; i++)
        {
            if (moveInput != Vector2.zero)
            {
                anim[i].SetFloat(Hash_dirY, moveInput.y);
                anim[i].SetFloat(Hash_dirX, moveInput.x);
                lastMoveInput = moveInput;
            }


            anim[i].SetFloat(Hash_MovementType, moveInput != Vector2.zero ? 1 : 0);

        }
    }
    
    
    public void UpdateAutoMoveAnimator(Vector3 direction)
    {
        if (!autoMovement) return;
        for (int i = 0; i < anim.Length; i++)
        {
            if (direction != Vector3.zero)
            {
                anim[i].SetFloat(Hash_dirX, direction.x);
                anim[i].SetFloat(Hash_dirY, direction.y);
                
            }

            anim[i].SetFloat(Hash_MovementType, direction != Vector3.zero ? 1 : 0);
        }
        
        if (direction.x < 0)
        {
            playerDir = PlayerDir.Left;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction.x > 0)
        {
            playerDir = PlayerDir.Right;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        if (direction.y > 0)
        {
            playerDir = PlayerDir.Up;
        }
        else if (direction.y < 0)
        {
            playerDir = PlayerDir.Down;
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
    
    #region AutoMovement

    public void AutoMovement(bool _autoMovement)
    {
        autoMovement = _autoMovement;
        
        if (autoMovement)
        {
            DisableInput();
        }
        else
        {
            EnableInput();
        }
    }

    #endregion
}
