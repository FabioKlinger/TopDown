using System;
using System.Collections.Generic;
using System.Linq;

using Ink;
using Ink.Runtime;

using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueController : MonoBehaviour
{
    private const string SpeakerSeparator = ":";
    private const string EscapedColon = "::";
    private const string EscapedColonPlaceholder = "ยง";
    
    public static event Action DialogueClosed;

    ///<summary>Generic Ink event supplying an identifier.</summary>
    public static event Action<string> InkEvent;

    #region Inspector

    [Header("Ink")]

    [SerializeField] private TextAsset inkAsset;

    [Header("UI")]

    [SerializeField] private DialogueBox dialogueBox;

    [Header("Avatar")] 
    [SerializeField] private Sprite avatar_player01;
    [SerializeField] private Sprite avatar_witch;
   
    #endregion

    private Story inkStory;
    private GameState gameState;
    
    
    #region Unity Event Functions

    private void Awake()
    {
        gameState = FindObjectOfType<GameState>();
        
        // Initialize Ink.
        inkStory = new Story(inkAsset.text);
        // Add error handling.
        inkStory.onError += OnInkError;
        // Connect an ink function to a C# function.
        inkStory.BindExternalFunction<string>("Event", Event);
        inkStory.BindExternalFunction<string>("Get_State", Get_State, true);
        inkStory.BindExternalFunction<string, int>("Add_State", Add_State);
    }

    private void OnEnable()
    {
        DialogueBox.DialogueContinued += OnDialogueContinued;
        DialogueBox.ChoiceSelected += OnChoiceSelected;
    }

    private void Start()
    {
        dialogueBox.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        DialogueBox.DialogueContinued -= OnDialogueContinued;
        DialogueBox.ChoiceSelected -= OnChoiceSelected;
    }

    private void OnDestroy()
    {
        inkStory.onError -= OnInkError;
    }

    #endregion

    #region Dialogue Lifecycle

    public void StartDialogue(string dialoguePath)
    {
        OpenDialogue();

        // Like '-> knot' in ink.
        inkStory.ChoosePathString(dialoguePath);
        ContinueDialogue();
    }

    private void OpenDialogue()
    {
        dialogueBox.gameObject.SetActive(true);
    }

    private void CloseDialogue()
    {
        EventSystem.current.SetSelectedGameObject(null);
        dialogueBox.gameObject.SetActive(false);
        DialogueClosed?.Invoke();
    }

    private void ContinueDialogue()
    {
        if (IsAtEnd())
        {
            CloseDialogue();
            return;
        }

        DialogueLine line;
        if (CanContinue())
        {
            string inkLine = inkStory.Continue();
            if (string.IsNullOrWhiteSpace(inkLine))
            {
                ContinueDialogue();
                return;
            }
            line = ParseText(inkLine, inkStory.currentTags);
        }
        else
        {
            line = new DialogueLine();
        }

        line.choices = inkStory.currentChoices;

        dialogueBox.DisplayText(line);
    }

    private void OnDialogueContinued(DialogueBox _)
    {
        ContinueDialogue();
    }

    private void OnChoiceSelected(DialogueBox _, int choiceIndex)
    {
        inkStory.ChooseChoiceIndex(choiceIndex);
        ContinueDialogue();
    }

    #endregion

    #region Ink

    private DialogueLine ParseText(string inkLine, List<string> tags)
    {
        DialogueLine line = new DialogueLine();

        inkLine = inkLine.Replace(EscapedColon, EscapedColonPlaceholder);

        List<string> parts = inkLine.Split(SpeakerSeparator).ToList();

        string speaker;
        string text;

        switch (parts.Count)
        {
            case 1:
                speaker = null;
                text = parts[0];
                break;
            case 2:
                speaker = parts[0];
                text = parts[1];
                
                // profile avatar action
                
                break;
            default:
                Debug.LogWarning($"Ink dialogue line was split at more {SpeakerSeparator} than expected." +
                                 $" Please make sure to use {EscapedColon} for {SpeakerSeparator} inside text");
                goto case 2;
        }

        line.speaker = speaker?.Trim();
        line.text = text.Trim().Replace(EscapedColonPlaceholder, SpeakerSeparator);

        if (tags.Contains("thought"))
        {
            line.text = $"<i>{line.text}</i>";
        }
        
        // profile avatar action + if 
        if (parts.Count > 1)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains("avatar"))
                {
                    //avatar:XYZ
                    List<string> avatar_parts = tags[i].Split(SpeakerSeparator).ToList();
                    // part 0 = avatar
                    // part 1 = XYZ
                    line.speakerImage = GetAvatar(avatar_parts[1]);
                    break;
                }
            }
            
            
        }

        return line;
    }

    private Sprite GetAvatar(string avatarId)
    {
        switch (avatarId)
        {
            case "player_01":
                return avatar_player01;
                break;

            case "witch":
                return avatar_witch;
                break;
            
            
            default:
                Debug.LogWarning($"The tag id {avatarId} has no specific case. " +
                                 $"Please add the case to the switch-statement.");
                return null;
                break;
        }
    }

    private bool CanContinue()
    {
        return inkStory.canContinue;
    }

    private bool HasChoices()
    {
        return inkStory.currentChoices.Count > 0;
    }

    private bool IsAtEnd()
    {
        return !CanContinue() && !HasChoices();
    }

    private void OnInkError(string message, ErrorType type)
    {
        switch (type)
        {
            case ErrorType.Author:
                break;
            case ErrorType.Warning:
                Debug.LogWarning(message);
                break;
            case ErrorType.Error:
                Debug.LogError(message);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private void Event(string eventName)
    {
        InkEvent?.Invoke(eventName);
    }

    private object Get_State(string id)
    {
        State state = gameState.Get(id);
        return state != null ? state.amount : 0;
    }

    private void Add_State(string id, int amount)
    {
        gameState.Add(id, amount);
    }

    #endregion
}

public struct DialogueLine
{
    public string speaker;
    public string text;
    public List<Choice> choices;

    // Here we can also add other information like speaker images or sounds.
    public Sprite speakerImage;
}