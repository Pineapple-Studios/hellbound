using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AnimationCtrlMenu : MonoBehaviour
{
    public static AnimationCtrlMenu Instance;

    private const string GO_TO_OPTIONS = "toOptions";
    private const string GO_TO_EXTRAS = "toExtras";
    private const string FROM_OPTIONS_TO_MENU = "fromOptionsToMenu";
    private const string FROM_EXTRAS_TO_MENU = "fromExtraToMenu";
    private const string PRESS_ANY_KEY = "anyKeyPressed";
    private const string GAMEPLAY = "transitionPlay";

    [Header("Selectable Buttons")]
    [SerializeField] public GameObject btnPlay;
    [SerializeField] public GameObject btnOptions;
    [SerializeField] public GameObject btnExtras;


    GameObject goSelect;

    private Animator _ac;

    private Stack<MenuState> stateStack = new Stack<MenuState>();
    private MenuState currentState;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _ac = GetComponent<Animator>();
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
                _ac?.SetTrigger(GO_TO_OPTIONS);
                goSelect = null;

                Invoke(nameof(InvokeShowLastTab), 0.75f);
                break;

            case MenuState.Extras:
                _ac?.SetTrigger(GO_TO_EXTRAS);
                goSelect = btnExtras;
                break; 

            case MenuState.Play:
                _ac?.SetTrigger(GAMEPLAY);
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
                        _ac?.SetTrigger(FROM_EXTRAS_TO_MENU);
                        break;

                    case MenuState.Options:
                        goSelect = btnOptions;
                        _ac?.SetTrigger(FROM_OPTIONS_TO_MENU);
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
        if (goSelect != null)
            EventSystem.current.SetSelectedGameObject(goSelect);
    }

    private void InvokeShowLastTab()
    {
        OptionsSelector.Instance?.ForceSelectLastButton();
    }

    public void OnOptionsClosed()
    {
        if (OptionsSelector.Instance != null)
            OptionsSelector.Instance.HideAllTabs();
    }

    public void OnNextScene()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void NextAnim()
    {
        _ac.Play("clip_Menu");
    }
}
