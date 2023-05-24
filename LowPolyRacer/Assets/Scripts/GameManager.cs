using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Public Variables
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelHelpMenu;
    [SerializeField] private GameObject panelCarSelection;
    [SerializeField] private GameObject panelPlay;
    [SerializeField] private GameObject panelCarGallery;
    [SerializeField] private GameObject panelHighscore;
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject[] tracks;
    [SerializeField] private List<GameObject> cars;
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
    State _previousState;
    Button _carGalleryButton;
    Button _highscoreButton;
    GameObject _playerCar;
    GameObject _currentTrack;
    int _highlightedCarId = 0;
    Vector3 _track00DefaultCameraPos = new Vector3(11f, 6.3f, 18.1f);
    Vector3 _track00DefaultCameraRot = new Vector3(162f, -0.3f, 180f);
    Vector3 _track01DefaultCameraPos = new Vector3(19f, 6f, -30f);
    Vector3 _track01DefaultCameraRot = new Vector3(160f, -77f, 180f);
    Vector3 _track02DefaultCameraPos = new Vector3(-0.1f, 9.3f, 12.8f);
    Vector3 _track02DefaultCameraRot = new Vector3(162f, -0.3f, 180f);
    Vector3 _track03DefaultCameraPos = new Vector3(12.9f, 7.4f, 45.3f);
    Vector3 _track03DefaultCameraRot = new Vector3(162f, -0.3f, 180f);
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
        Debug.Log("Entering State: " + newState);
        switch (newState)
        {
            case State.MENU:
                panelMainMenu.SetActive(true);
                DisableUnusedButtons();
                if (_previousState != State.HELP)
                {
                    SetupTrack(tracks[0]);
                }
                break;
            case State.HELP:
                panelHelpMenu.SetActive(true);
                break;
            case State.INIT:
                panelCarSelection.SetActive(true);
                HighlightCar(_highlightedCarId);
                break;
            case State.PLAY:
                SetupTrack(tracks[1]);
                panelPlay.SetActive(true);
                CameraFollow.Instance.Target = _playerCar.transform;
                CameraFollow.Instance.SwitchCameraPosition(false);
                break;
            case State.PAUSE:
                panelPause.SetActive(true);
                break;
            case State.LOADLEVEL:
                Destroy(_currentTrack);
                SwitchState(State.PLAY);
                break;
            case State.LEVELCOMPLETED:
            case State.GAMEOVER:
            default:
                break;
        }
    }
    void EndState()
    {
        Debug.Log("Ending State: " + _state);
        switch (_state)
        {
            case State.MENU:
                panelMainMenu.SetActive(false);
                break;
            case State.HELP:
                panelHelpMenu.SetActive(false);
                break;
            case State.INIT:
                panelCarSelection.SetActive(false);
                break;
            case State.PLAY:
                panelPlay.SetActive(false);
                CameraFollow.Instance.SwitchCameraPosition(true);
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
    void SetPlayerCar()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _playerCar = Instantiate(cars[_highlightedCarId],player.transform);
        Car.Instance.isSelected = true;
        _playerCar.transform.parent = player.transform;
        SwitchState(State.LOADLEVEL);
    }
    void SetupTrack(GameObject track)
    {
        if (_currentTrack != null)
        {
            Destroy(_currentTrack);
        }
        _currentTrack = Instantiate<GameObject>(track);
        SetCameraPositionAndRotation();
        AddCarsToGarage();
    }
    void SetCameraPositionAndRotation()
    {
        if (_currentTrack.name.StartsWith("Track00"))
        {
            Camera.main.transform.position = _track00DefaultCameraPos;
            Camera.main.transform.rotation = Quaternion.Euler(_track00DefaultCameraRot);
        }
        if (_currentTrack.name.StartsWith("Track01"))
        {
            Camera.main.transform.position = _track01DefaultCameraPos;
            Camera.main.transform.rotation = Quaternion.Euler(_track01DefaultCameraRot);
        }
        if (_currentTrack.name.StartsWith("Track02"))
        {
            Camera.main.transform.position = _track02DefaultCameraPos;
            Camera.main.transform.rotation = Quaternion.Euler(_track02DefaultCameraRot);
        }
        if (_currentTrack.name.StartsWith("Track03"))
        {
            Camera.main.transform.position = _track03DefaultCameraPos;
            Camera.main.transform.rotation = Quaternion.Euler(_track03DefaultCameraRot);
        }
    }
    void AddCarsToGarage()
    {
        var spawns = _currentTrack.GetComponentsInChildren<Transform>();

        spawns = spawns.Where(child => child.tag == "Respawn").ToArray();
        foreach (var spawn in spawns)
        {
            GameObject car = FindCarByName(spawn.name);
            if (car != null)
            {
                Instantiate(car.gameObject, spawn.transform.position, spawn.transform.rotation, spawn);
            }
        }
    }
    void RemoveSelectedCarFromGarage(){
        var spawns = _currentTrack.GetComponentsInChildren<Transform>();

        spawns = spawns.Where(child => child.tag == "Respawn").ToArray();
    }
    void CarLoop(string direction)
    {
        int tempId = _highlightedCarId;
        if (direction == "left")
        {
            _highlightedCarId = tempId + 1 == cars.Count ? 0 : _highlightedCarId + 1;
        }
        if (direction == "right")
        {
            _highlightedCarId = tempId - 1 < 0 ? cars.Count - 1 : _highlightedCarId - 1;
        }
        HighlightCar(_highlightedCarId);
    }
    void HighlightCar(int id)
    {
        var spawns = _currentTrack.GetComponentsInChildren<Transform>();
        spawns = spawns.Where(child => child.tag == "Respawn").ToArray();
        var car = spawns.Where(spawn => spawn.name == cars[id].name).First();
        Vector3 newCamPosition = new();
        Vector3 newCamRotation = new();
        switch (car.name)
        {
            case "Humvee":
                newCamPosition = new Vector3(-5.6f, 3.5f, 4.9f);
                newCamRotation = new Vector3(154.7f, -49.3f, 177.7f);
                break;
            case "PickupBrown":
                newCamPosition = new Vector3(.2f, 4.1f, 3.7f);
                newCamRotation = new Vector3(145.5f, -47.6f, 180f);
                break;
            case "SportsCarRed":
                newCamPosition = new Vector3(14.2f, 2.1f, 5.8f);
                newCamRotation = new Vector3(154.6f, 25f, 183.5f);
                break;
            case "SportsCarGreen":
                newCamPosition = new Vector3(14.7f, 1.9f, 5.2f);
                newCamRotation = new Vector3(164.9f, -50.4f, 180.8f);
                break;
            default:
                break;
        }
        Camera.main.transform.position = newCamPosition;
        Camera.main.transform.rotation = Quaternion.Euler(newCamRotation);
    }
    #endregion

    #region Custon Functions
    IEnumerator SwitchDelay(State newState, float delay)
    {
        _isSwitchingState = true;
        _previousState = _state;
        yield return new WaitForSeconds(delay);
        EndState();
        GameState = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }
    GameObject FindCarByName(string name)
    {
        foreach (GameObject car in cars)
        {
            if (car.name == name)
            {
                return car;
            }
        }
        return null;
    }
    #endregion

    #region Button Functions
    public void BackButtonClicked() { SwitchState(State.MENU); }
    public void HelpButtonClicked() { SwitchState(State.HELP); }
    public void StartButtonClicked() { SwitchState(State.INIT); }
    public void ScrollButtonClicked(string direction) { CarLoop(direction); }
    public void SelectCarButtonClicked() { SetPlayerCar(); }
    public void PauseButtonClicked() { SwitchState(State.PAUSE); }
    public void ContinueButtonClicked() { SwitchState(State.PLAY); }
    public void EndGameButtonClicked() { Destroy(_currentTrack); Destroy(_playerCar); SwitchState(State.MENU); }

    #endregion
}
