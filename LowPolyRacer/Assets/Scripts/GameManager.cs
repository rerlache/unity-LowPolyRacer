using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Public Variables
    public GameObject panelMainMenu;
    public GameObject panelHelpMenu;
    public GameObject panelPlay;
    public GameObject panelCarGallery;
    public GameObject panelHighscore;
    public GameObject panelPause;
    #endregion
    #region Public Properties 
    public static GameManager Instance { get; private set; }
    public InputController InputController { get; private set; }
    private State _state;
    public State GameState
    {
        get { return _state; }
        set { _state = value; }
    }

    public enum State { MENU, HELP, INIT, PLAY, PAUSE, LEVELCOMPLETED, LOADLEVEL, GAMEOVER }
    #endregion

    #region Private variables
    bool _isSwitchingState;
    Button _carGalleryButton;
    Button _highscoreButton;
    //State _state;
    #endregion
    #region Unity Methods/Functions
    void Awake()
    {
        Instance = this;
        SwitchState(State.MENU);
        InputController = GetComponentInChildren<InputController>();
    }
    void Start()
    {
    }
    void Update()
    {
    }
    void FixedUpdate()
    {
    }
    #endregion

    #region Custom Methods
    void SwitchState(State newState, float delay = 0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }
    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                panelMainMenu.SetActive(true);
                DisableUnusedButtons();
                break;
            case State.HELP:
                panelHelpMenu.SetActive(true);
                break;
            case State.INIT:
            case State.PLAY:
                panelPlay.SetActive(true);
                break;
            case State.PAUSE:
                panelPause.SetActive(true);
                break;
            case State.LEVELCOMPLETED:
            case State.LOADLEVEL:
            case State.GAMEOVER:
            default:
                break;
        }
    }
    void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMainMenu.SetActive(false);
                break;
            case State.HELP:
                panelHelpMenu.SetActive(false);
                break;
            case State.INIT:
            case State.PLAY:
                panelPlay.SetActive(false);
                break;
            case State.PAUSE:
                panelPause.SetActive(false);
                break;
            case State.LEVELCOMPLETED:
            case State.LOADLEVEL:
            case State.GAMEOVER:
            default:
                break;
        }
    }
    void DisableUnusedButtons()
    {
        _carGalleryButton = GameObject.Find("CarGalleryButton").GetComponent<Button>();
        _highscoreButton = GameObject.Find("HighscoreButton").GetComponent<Button>();
        _carGalleryButton.enabled = false;
        _highscoreButton.enabled = false;
    }
    #endregion
    #region Custon Functions
    IEnumerator SwitchDelay(State newState, float delay)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        //_state = newState;
        GameState = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }
    #endregion

    #region Button Functions
    public void HelpButtonClicked() { SwitchState(State.HELP); }
    public void BackButtonClicked() { SwitchState(State.MENU); }
    public void StartButtonClicked() { SwitchState(State.PLAY); } // TODO: change to INIT before!
    public void PauseButtonClicked() { SwitchState(State.PAUSE); }
    public void ContinueButtonClicked() { SwitchState(State.PLAY); }
    public void EndGameButtonClicked() { SwitchState(State.MENU); } // TODO: Clear everything before going to the menu! 

    #endregion
}
