using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; //reference to the pauseMenu GameObject
    public PlayerController pc; //reference to the PlayerController script
    private bool isPaused = false; //tracks whether the game is paused or not
    public InputSystem_Actions inputActions; //reference to the input actions
    private InputAction pauseAction; //reference to the pauseAction, input action

    private void Awake()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();  // Finds the GameObject named "Player" in the scene and uses its PlayerHealth component.
        // Initializes input actions and assigns the pause action
        inputActions = new InputSystem_Actions();
        pauseAction = inputActions.Player.Pause;
    }

    private void OnEnable()
    {
        inputActions.Enable(); // Enables input actions when the script is active
        pauseAction.performed += Pause; // Subscribes to the pause action event
    }

    private void Start()
    {
        pauseMenu.SetActive(false); // Ensures the pause menu is disabled at the start
        Time.timeScale = 1f; // Ensures the game starts in an unpaused state
    }
    

    private void OnDisable()
    {
        inputActions.Disable(); // Disables input actions when the script is disabled
        pauseAction.performed -= Pause; // Unsubscribes from the pause action event
    }
    
    private void Pause(InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused; //toggles the pause bool
        pauseMenu.SetActive(isPaused); //activates/deactivates the 
        Time.timeScale = isPaused ? 0f : 1f; //freezes or unfreezes the game

        // Disables or enables player input based on the pause bool
        if (isPaused)
            pc.inputActions.Disable();
        else
            pc.inputActions.Enable();
    }

    public void ReturnToGame() // Resumes the game by disabling the pause menu, re-enabling player controls and unfreezing the game
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        pc.inputActions.Enable();
        Time.timeScale = 1f;
    }

    public void ExitGame() //loads the scene that contains the main menu
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}