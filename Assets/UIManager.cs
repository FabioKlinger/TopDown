using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Button firstSelectedButton;
    [SerializeField] private GameObject firstSelectedMenu;
    
    [Header("Ui Menus")] 
    [SerializeField] private GameObject mainMenuContainer;
    [SerializeField] private GameObject optionsMenuContainer;
    //....

    [Header("Options Menu")] 
    [SerializeField] private GameObject firstSelectedOption;

    [SerializeField] private GameObject options_controlContainer;
    [SerializeField] private Button controlButton;
    [SerializeField] private GameObject options_grahpicContainer;
    [SerializeField] private Button graphicButton;
    [SerializeField] private GameObject options_soundContainer;
    [SerializeField] private Button soundButton;

    [FormerlySerializedAs("maxIndex")] [SerializeField] private int menuNumber;
    [Header("Button References")]
    [SerializeField] private Button optionsApplyButton;
    
    private GameObject currentMenu;
    private GameObject currentOption;
    public int menuIndex = 0;
    public int optionsIndex;
    #region InputSystem

    private InputSystem_Actions inputActions;
    private InputAction cancelAction;
    private InputAction gamepadRsAction;
    private InputAction gamepadLsAction;
    #endregion

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        cancelAction = inputActions.UI.Cancel;
        gamepadRsAction = inputActions.UI.GamepadRs;
        gamepadLsAction = inputActions.UI.GamepadLs;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        cancelAction.performed += CancelInput;
        gamepadRsAction.performed += RsInput;
        gamepadLsAction.performed += LsInput;
    }

    private void Start()
    {
        currentMenu = firstSelectedMenu;
        currentOption = firstSelectedOption;
        
        if (firstSelectedButton)
        {
            StartCoroutine(DelaySelection(firstSelectedButton));
        }
    }

    private void OnDisable()
    {
        inputActions.Disable();
        cancelAction.performed -= CancelInput;
        gamepadRsAction.performed -= RsInput;
        gamepadLsAction.performed -= LsInput;
    }

    public void SelectButton(Button button)
    {
        StartCoroutine(DelaySelection(button));
    }

    private IEnumerator DelaySelection(Button button)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    public void ShowMenu(GameObject menuContainer)
    {
        currentMenu.SetActive(false);
        currentMenu = menuContainer;
        currentMenu.SetActive(true);
    }

    public void loadGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Menu_ShowMain()
    {
        menuIndex = 0;
        currentMenu.SetActive(false);
        currentMenu = mainMenuContainer;
        currentMenu.SetActive(true);
    }
    
    public void Menu_ShowLoad()
    {
        menuIndex = 1;
    }
    
    public void Menu_ShowOptions()
    {
        menuIndex = 2;
        currentMenu.SetActive(false);
        currentMenu = optionsMenuContainer;
        currentMenu.SetActive(true);
    }
    
    public void Menu_ShowCredits()
    {

    }
    
    
    public void Options_ShowControl()
    {
        optionsIndex = 0;
        currentOption.SetActive(false);
        currentOption = options_controlContainer;
        currentOption.SetActive(true);
        SelectButton(controlButton);
    }
    
    public void Options_ShowGraphic()
    {
        optionsIndex = 1;
        currentOption.SetActive(false);
        currentOption = options_grahpicContainer;
        currentOption.SetActive(true);
        SelectButton(graphicButton);
    }
    
    public void Options_ShowSound()
    {
        optionsIndex = 2;
        currentOption.SetActive(false);
        currentOption = options_soundContainer;
        currentOption.SetActive(true);
        SelectButton(soundButton);
    }
    
    private void CancelInput(InputAction.CallbackContext ctx)
    {
        
        print("cancel");
        switch (menuIndex)
        {
            case 0: // main menu
                
                break;
            
            case 1: // load menu
                Menu_ShowMain();
                break;
            
            case 2: // options menu
                SelectButton(optionsApplyButton);
                break;
            
            case 3: // credits menu
                Menu_ShowMain();
                break;
            
            case 4: // quit question
                
                break;
        }
    }
    
    private void RsInput(InputAction.CallbackContext ctx)
    {
        optionsIndex++;

        if (optionsIndex > menuNumber -1)
        {
            optionsIndex = 0;
        }

        ShowOptionMenu(optionsIndex);
    }
    private void LsInput(InputAction.CallbackContext ctx)
    {
        optionsIndex--;

        if (optionsIndex < 0)
        {
            optionsIndex = menuNumber - 1;
        }

        ShowOptionMenu(optionsIndex);
    }

    void ShowOptionMenu(int index)
    {
        switch (index)
        {
            case 0: // options controls
                Options_ShowControl();
                break;
            
            case 1: // options graphic
                Options_ShowGraphic();
                break;
            
            case 2: // options sound
                Options_ShowSound();
                break;
        }
    }
}