using UnityEngine;

public class PlayerAnimationBehaviour : MonoBehaviour
{
    public PlayerController_Simple playerController;

    public GameObject sword_rh;
    
    public void Start_Rh_SwordAttack()
    {
        Vector2 currentMoveInput = playerController.GetMoveInput();
        
        sword_rh.SetActive(true);
        sword_rh.GetComponent<Animator>().SetFloat("dirX", currentMoveInput.x);
        sword_rh.GetComponent<Animator>().SetFloat("dirY", currentMoveInput.y);

        
    }
    
    public void End_Rh_SwordAttack()
    {
        playerController.EndAttacking();
        sword_rh.SetActive(false);
    }
    
    
    public void EndRolling()
    {
        playerController.EndRolling();
    }
    
    
}
