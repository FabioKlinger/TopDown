using UnityEngine;

public class PlayerAnimationBehaviour : MonoBehaviour
{
    public PlayerController_Simple playerController;

    public GameObject sword_rh;
    
    public void Start_Rh_SwordAttack()
    {
        sword_rh.SetActive(true);
    }
    
    public void End_Rh_SwordAttack()
    {
        sword_rh.SetActive(false);
    }
    
    
    public void EndRolling()
    {
        playerController.EndRolling();
    }
}
