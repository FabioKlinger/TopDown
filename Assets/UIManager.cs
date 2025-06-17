using System;
using System.Collections;
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

    [Header("Button References")]
    [SerializeField] private Button optionsApplyButton;
    
    private GameObject currentMenu;
    public int menuIndex = 0;
    #region InputSystem

    private InputSystem_Actions inputActions;
    private InputAction cancelAction;
    
    #endregion

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        cancelAction = inputActions.UI.Cancel;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        cancelAction.performed += CancelInput;
    }

    private void Start()
    {
        currentMenu = firstSelectedMenu;
        
        if (firstSelectedButton)
        {
            StartCoroutine(DelaySelection(firstSelectedButton));
        }
    }

    private void OnDisable()
    {
        inputActions.Disable();
        cancelAction.performed -= CancelInput;
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

    public void Menu_ShowMain()
    {
        menuIndex = 0;
        currentMenu.SetActive(false);
        currentMenu = mainMenuContainer;
        mainMenuContainer.SetActive(true);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
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
        optionsMenuContainer.SetActive(true);
    }
    
    public void Menu_ShowCredits()
    {

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
}