using UnityEngine;

public class PlayerAnimationBehaviour : MonoBehaviour
{
    public PlayerController_Simple playerController;

    public GameObject sword_rh;
    public GameObject pickaxe;
    public GameObject axe;
    public GameObject can;
    public GameObject bow;
    
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
        playerController.EndPickaxe();
        playerController.EndCan();
        playerController.EndAxe();
        playerController.EndBow();
        sword_rh.SetActive(false);
    }
    
    public void Start_Rh_PickaxeAttack()
    {
        Vector2 currentMoveInput = playerController.GetMoveInput();
        
        pickaxe.SetActive(true);
        pickaxe.GetComponent<Animator>().SetFloat("dirX", currentMoveInput.x);
        pickaxe.GetComponent<Animator>().SetFloat("dirY", currentMoveInput.y);

        
    }
    
    public void End_Rh_PickaxeAttack()
    {
        playerController.EndPickaxe();
        playerController.EndAttacking();
        playerController.EndCan();
        playerController.EndAxe();
        playerController.EndBow();
        pickaxe.SetActive(false);
    }
    
    public void Start_Rh_AxeAttack()
    {
        Vector2 currentMoveInput = playerController.GetMoveInput();
        
        axe.SetActive(true);
        axe.GetComponent<Animator>().SetFloat("dirX", currentMoveInput.x);
        axe.GetComponent<Animator>().SetFloat("dirY", currentMoveInput.y);

        
    }
    
    public void End_Rh_AxeAttack()
    {
        playerController.EndAxe();
        playerController.EndPickaxe();
        playerController.EndAttacking();
        playerController.EndCan();
        playerController.EndBow();
        axe.SetActive(false);
    }
    
    public void Start_Rh_Can()
    {
        Vector2 currentMoveInput = playerController.GetMoveInput();
        
        can.SetActive(true);
        can.GetComponent<Animator>().SetFloat("dirX", currentMoveInput.x);
        can.GetComponent<Animator>().SetFloat("dirY", currentMoveInput.y);

        
    }
    
    public void End_Rh_Can()
    {
        playerController.EndCan();
        playerController.EndAxe();
        playerController.EndPickaxe();
        playerController.EndAttacking();
        playerController.EndBow();
        can.SetActive(false);
    }
    
    public void Start_Rh_Bow()
    {
        Vector2 currentMoveInput = playerController.GetMoveInput();
        
        bow.SetActive(true);
        bow.GetComponent<Animator>().SetFloat("dirX", currentMoveInput.x);
        bow.GetComponent<Animator>().SetFloat("dirY", currentMoveInput.y);

        
    }
    
    public void End_Rh_Bow()
    {
        playerController.EndBow();
        playerController.EndCan();
        playerController.EndAxe();
        playerController.EndPickaxe();
        playerController.EndAttacking();
        bow.SetActive(false);
    }
    
    
    
    public void EndRolling()
    {
        playerController.EndRolling();
    }
    
    
}
