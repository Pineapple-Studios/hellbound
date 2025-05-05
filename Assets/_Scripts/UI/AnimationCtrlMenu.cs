using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationCtrlMenu : MonoBehaviour
{
    public static AnimationCtrlMenu Instance;

    private const string GO_TO_OPTIONS = "toOptions";
    private const string GO_TO_EXTRAS = "toExtras";
    private const string FROM_OPTIONS_TO_MENU = "fromOptionsToMenu";
    private const string FROM_EXTRAS_TO_MENU = "fromExtraToMenu";

    [Header("Selectable Buttons")]
    [SerializeField] public GameObject btnPlay;
    [SerializeField] public GameObject btnOptions;
    [SerializeField] public GameObject btnAudio;
    [SerializeField] public GameObject btnExtras;

    GameObject goSelect;

    private Animator _an;

    private Stack<MenuState> stateStack = new Stack<MenuState>();
    private MenuState currentState;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        _an = GetComponent<Animator>();
        goSelect = btnPlay;
        Invoke(nameof(ButtonSelect), 1);
    }

    public void GoTo(MenuState newState)
    {
        stateStack.Push(currentState);
        currentState = newState;

        switch (newState)
        {
            case MenuState.MainMenu:
                Debug.LogWarning("Use GoBack() para retornar ao MainMenu.");
                break;
            case MenuState.Options:
                _an?.SetTrigger(GO_TO_OPTIONS);
                goSelect = btnAudio;
                break;
            case MenuState.Extras:
                _an?.SetTrigger(GO_TO_EXTRAS);
                goSelect = btnExtras;
                break;
        }

        Invoke(nameof(ButtonSelect), 1);
    }

    public void GoBack()
    {
        if (currentState == MenuState.MainMenu)
        {
            Debug.Log("MainMenu");
            return;
        }

        if (stateStack.Count > 0)
        {
            var previousState = stateStack.Pop();

            if (previousState == MenuState.MainMenu)
            {
                switch (currentState)
                {
                    case MenuState.Extras:
                        goSelect = btnExtras;
                        _an?.SetTrigger(FROM_EXTRAS_TO_MENU);
                        break;

                    case MenuState.Options:
                        _an?.SetTrigger(FROM_OPTIONS_TO_MENU);
                        goSelect = btnOptions;
                        break;
                }

                currentState = MenuState.MainMenu;
                
                Invoke(nameof(ButtonSelect), 1);
            }
            else
            {
                GoTo(previousState);
            }
        }
    }

    public void ButtonSelect()
    {
        EventSystem.current.SetSelectedGameObject(goSelect);
    }
}
