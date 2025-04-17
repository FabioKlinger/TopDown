using UnityEngine;

public class PlayerAnimationBehaviour : MonoBehaviour
{
    public PlayerController_Simple playerController;
    
    public void EndRolling()
    {
        playerController.EndRolling();
    }
}
